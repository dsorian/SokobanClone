using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    MapMatrix mapaTemp;   //Para ir cargando los mapas
    private int modoJuego=0;    //0=jugar   1=Diseñar nivel
    public GameObject laCamara;
    public GameObject[] matrizMapa;    //Matriz con las figuras que conforman el mapa

    public GameObject elSuelo, elPlayer, elMuro, laCaja, laPosCaja;    //Los prefabs para jugar

    public int rows;                  //Filas del mapa
    public int cols;                  //Columnas del mapa
    public GameObject elementoMapaPrefab;  //Uno de los objetos que conforman el mapa
    public GameObject posMapa;      //Posición del mapa

    private int teselaSeleccionada;   //Indicar la tesela seleccionada de teselas

    public Sprite[] teselasDisponibles;    //Las teselas que tenemos para poner

    public TMPro.TMP_Dropdown elDropdown;   //El dropdown con el listado de mapas

    public GameObject pantallaPausa;
    public GameObject pantallaEditorNiveles;
    public GameObject pantallaJugar;
    public GameObject pantallaFinJuego;

    private MapsCollection lamapsCollection; //Referencia al scriptableObject que guardará los mapas del juego.

    private int mapaActual = 0;    //Mapa mostrado actualmente.
    private bool juegoPausado = true;

    //Para el desarrollo del juego
    int numCajas;
    private List<Caja> listaCajas;



    // Start is called before the first frame update
    void Start()
    {
        matrizMapa = new GameObject[rows*cols];
        //Cargamos los mapas guardados 
        ReiniciarMapa();

    }

    void Update()
    {

        if( modoJuego == 1 ){
            //Modo de edición, movemos el mapa si no cabe en la pantalla con WASD
            if (Input.GetKeyUp(KeyCode.W)){
                //posMapa.transform.position = new Vector3();  
                posMapa.transform.Translate(0,1,0);
            }
            if (Input.GetKeyUp(KeyCode.S)){
                //posMapa.transform.position = new Vector3();  
                posMapa.transform.Translate(0,-1,0);
            }
            if (Input.GetKeyUp(KeyCode.A)){
                //posMapa.transform.position = new Vector3();  
                posMapa.transform.Translate(-1,0,0);
            }
            if (Input.GetKeyUp(KeyCode.D)){
                //posMapa.transform.position = new Vector3();  
                posMapa.transform.Translate(1,0,0);
            }
        }

        // Click izquierdo del ratón pulsado
        if (Input.GetMouseButtonDown(0))
        {
            // Convertir la posición del ratón
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Realizar un raycast para detectar objetos clicados
            if (Physics.Raycast(ray, out hit))
            {
                // Verificar si el objeto clicado tiene un componente SpriteRenderer
                ElementoMapa clickedObject = hit.collider.gameObject.GetComponent<ElementoMapa>();
                SpriteRenderer spriteRenderer = clickedObject.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    // Llamar al método OnSpriteClicked con el objeto clicado como argumento
                    OnSpriteClicked(clickedObject);
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Escape)){
            PausarJuego();
        }
        if (Input.GetKeyUp(KeyCode.Alpha7)){
            mapaActual=7;

        }
    }

    public void SueloClicked(){
        teselaSeleccionada=0;
    }

    public void PlayerClicked(){
        Debug.Log("Player clicked.");
        teselaSeleccionada=1;
    }

    public void ParedClicked(){
        Debug.Log("Pared clicked.");
        teselaSeleccionada=2;
    }

    public void CajaClicked(){
        Debug.Log("Caja clicked.");
        teselaSeleccionada=3;
    }

    public void PosCajaClicked(){
        Debug.Log("PosCaja clicked.");
        teselaSeleccionada=4;
    }

    public void GuardarClicked(){
        Debug.Log("Guardar clicked. Deberemos guardar el mapa actual ");
        // Obtener las matrices de algún lugar...
        GuardarMapaActual();
    }

    //Creamos un nuevo mapa con todo suelo y lo añadimos al listado.
    public void NuevoMapaClicked(){
        Debug.Log("Hemos pulsado nuevo. Deberemos crear un nuevo mapa y añadirlo al listado.");
        int numMapa = lamapsCollection.CreateEmptyMap(rows,cols);
Debug.Log("Hemos creado el mapa número: "+numMapa);        
        elDropdown.gameObject.GetComponent<TMP_Dropdown>().ClearOptions();
        elDropdown.gameObject.GetComponent<TMP_Dropdown>().AddOptions(lamapsCollection.GetMapsNames());
        elDropdown.gameObject.GetComponent<TMP_Dropdown>().value = lamapsCollection.mapas.Count;
    }

    public void ReiniciarNivelClicked(){
        ReiniciarNivel();
        JugarMapa(mapaActual);
    }

    private void GuardarMapaActual(){
        int[] mapParaSalvar = new int[rows*cols+cols];
        if (lamapsCollection != null)
        {
            //Generamos la matriz de números para guardar
            // Crear y posicionar los sprites
            for (int i = 0; i < rows; i++){
                for (int j = 0; j < cols; j++){

                    //Guardamos el tipo de baldosa
                    //tipoTesela--> 0=suelo 1=player 2=pared 3=caja 4=posicionCaja
                    mapParaSalvar[i*cols+j] = matrizMapa[i*cols+j].gameObject.GetComponent<ElementoMapa>().tipoTesela;
                }
            }
            //mapaActual = mapsCollection.AddNewMap(mapParaSalvar,rows,cols);
            lamapsCollection.mapas[mapaActual].matrix = mapParaSalvar;
            Debug.Log("Mapa guardado. Faltaría actualizar droplist con el nombre del nuevo mapa. mapaActual:"+mapaActual+" número de mapas: "+lamapsCollection.mapas.Count);
        }

        //Archivo donde guardaremos la información (texto plano, para más seguridad usaríamos binario pero así podemos verlo)
        string nombreArchivo = "/MapasSokoban"+SceneManager.GetActiveScene().buildIndex+"Mapa"+mapaActual+".txt";
        string json = JsonUtility.ToJson(lamapsCollection.mapas[mapaActual]);
Debug.Log("El json creado: "+json+" número de elementos en la matriz: "+lamapsCollection.mapas[mapaActual].matrix.Length);
        //Guardamos la info


        if(File.Exists(Application.persistentDataPath+nombreArchivo)){
            Debug.Log("GuardarMapaActual(): El fichero json existe y lo voy a borrar para escribirlo de nuevo."+Application.persistentDataPath+nombreArchivo);
            File.Delete(Application.persistentDataPath + nombreArchivo);
        }
        File.WriteAllText(Application.persistentDataPath+nombreArchivo,json);
    }

    private void ReiniciarMapa(){
        lamapsCollection = (MapsCollection) ScriptableObject.CreateInstance(typeof(MapsCollection));
        lamapsCollection.CreateIniMap(rows,cols);
        Debug.Log("Hemos creado el listado de mapas. Tenemos: "+lamapsCollection.mapas.Count+" mapas.");
    }

    // Método que será llamado cuando se haga clic en un sprite
    public void OnSpriteClicked(ElementoMapa clickedElemento)
    {
        int fila = clickedElemento.GetComponent<ElementoMapa>().fila;
        int columna = clickedElemento.GetComponent<ElementoMapa>().columna;
//        Debug.Log("Sprite clicado: "+ fila+"-"+columna+", tenía casilla: " + clickedElemento.GetComponent<ElementoMapa>().tipoTesela+" Cambiaré: "+(fila*columna+columna));
        clickedElemento.gameObject.GetComponent<SpriteRenderer>().sprite = teselasDisponibles[teselaSeleccionada];
        //matrizMapa[fila * columna + columna].gameObject.GetComponent<SpriteRenderer>().sprite = teselas[teselaSeleccionada];
        clickedElemento.GetComponent<ElementoMapa>().tipoTesela = teselaSeleccionada;
//        Debug.Log("Sprite clicado: "+ fila+"-"+columna+", ahora tiene casilla: " + clickedElemento.GetComponent<ElementoMapa>().tipoTesela);
    }

    private void CargarMapa(int numMapa){
        Debug.Log("CargarMapa. Vamos a ver si cargamos el mapa: "+numMapa+". Mapas cargados: "+lamapsCollection.mapas.Count);
        int[] mapaTiposTeselaTemp;
        //Cogemos los datos de las casillas del mapa en cuestión
        lamapsCollection.GetMapAt(numMapa,out mapaTiposTeselaTemp);

        // Crear y posicionar los sprites
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                matrizMapa[i * cols + j].gameObject.GetComponent<ElementoMapa>().fila = i;
                matrizMapa[i * cols + j].gameObject.GetComponent<ElementoMapa>().columna = j;
                matrizMapa[i * cols + j].gameObject.GetComponent<ElementoMapa>().tipoTesela = mapaTiposTeselaTemp[i*cols+j];
                matrizMapa[i * cols + j].gameObject.GetComponent<SpriteRenderer>().sprite = teselasDisponibles[mapaTiposTeselaTemp[i*cols+j]];
            }
        }
    }

    //Carga todos los mapas guardados y los del juego para mostrarlos en el dropdown 
    //y tenerlos en el scriptable object para poder seleccionarlos.
    private void CargarMapas(){
        Debug.Log("CargarMapas(). Pendiente, cargar todos los mapas más los creados por el jugador y mostrarlos en el dropdown");
        lamapsCollection.CrearMapasDefecto(rows,cols);
        //Inicializamos la matriz de casillas

        if( modoJuego == 1 ){ //Diseñar nivel
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    GameObject newSprite = Instantiate(elementoMapaPrefab, posMapa.transform);
                    newSprite.gameObject.GetComponent<ElementoMapa>().fila = i;
                    newSprite.gameObject.GetComponent<ElementoMapa>().columna = j;
                    newSprite.gameObject.GetComponent<ElementoMapa>().tipoTesela = 0;
                    newSprite.gameObject.GetComponent<SpriteRenderer>().sprite = teselasDisponibles[0];

                    newSprite.transform.localPosition = new Vector3(i, j, 0);
                    
                    // Guardar el sprite en la matriz de GameObjects
                    matrizMapa[i * cols + j] = newSprite;
                }
            }
        }

        Debug.Log("FALTA BUSCAR LOS MAPAS GUARDADOS Y CARGARLOS.");

        //Una vez cargados los niveles del juego, cargamos los mapas de disco si hay alguno
        //"/MapasSokoban"+SceneManager.GetActiveScene().buildIndex+"Mapa"+mapaActual+".txt";
        string[] archivosJSON = Directory.GetFiles(Application.persistentDataPath,"*.txt");
        string json;
        foreach(string archivo in archivosJSON){
            mapaTemp = new MapMatrix(rows,cols);
            json = File.ReadAllText(archivo);
            JsonUtility.FromJsonOverwrite(json,mapaTemp);
Debug.Log("He leído el mapa: "+mapaTemp.nombreMapa);
            //Debug.Log("He leído el nombre del archivo: "+contenido+" de "+archivosJSON.Length);
            lamapsCollection.AddNewMap(mapaTemp.matrix,mapaTemp.rows,mapaTemp.cols);
        }

        elDropdown.gameObject.GetComponent<TMP_Dropdown>().ClearOptions();
        elDropdown.gameObject.GetComponent<TMP_Dropdown>().AddOptions(lamapsCollection.GetMapsNames());
    }

    private void JugarMapa(int numMapa){
        Debug.Log("JugarMapa. Vamos a ver si cargamos el mapa: "+numMapa+" para jugarlo Mapas cargados: "+lamapsCollection.mapas.Count);
        int[] mapaTiposTeselaTemp;
        //Cogemos los datos de las casillas del mapa en cuestión
        lamapsCollection.GetMapAt(numMapa,out mapaTiposTeselaTemp);
        listaCajas = new List<Caja>();
        // Crear y posicionar los prefabs
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                // Instanciar el spritePrefab que corresponda
                GameObject casilla,casilla2;
                switch (mapaTiposTeselaTemp[i*cols+j]){
                    case 0:
                        casilla = Instantiate(elSuelo, posMapa.transform);
                        casilla.transform.localPosition = new Vector3(i, j, 0.1f);
                        casilla.transform.position.Set(casilla.transform.position.x,casilla.transform.position.y,casilla.transform.position.z+0.1f);
                        break;
                    case 1:
                        casilla = Instantiate(elPlayer, posMapa.transform);
                        casilla.transform.localPosition = new Vector3(i, j, 0);
                        laCamara.transform.SetParent(casilla.transform, false);
                        //En algunos niveles no enfoca bien y no sé porqué, lo corregimos
                        laCamara.transform.SetLocalPositionAndRotation(new Vector3(laCamara.transform.position.x,laCamara.transform.position.y,laCamara.transform.position.z-1),laCamara.transform.rotation);
                        //Creamos la casilla suelo "debajo" de la actual para que se vea cuando se mueva
                        casilla2 = Instantiate(elSuelo, posMapa.transform);
                        casilla2.transform.localPosition = new Vector3(i, j, 0.1f);
                        //casilla2.transform.position.Set(casilla2.transform.position.x,casilla2.transform.position.y,casilla2.transform.position.z+0.1f);
                        break;
                    case 2:
                        casilla = Instantiate(elMuro, posMapa.transform);
                        casilla.transform.localPosition = new Vector3(i, j, 0);
                        break;
                    case 3: 
                        casilla = Instantiate(laCaja, posMapa.transform);
                        numCajas++;
                        casilla.transform.localPosition = new Vector3(i, j, 0);
                        casilla.GetComponent<Caja>().elGameManager = this;
                        listaCajas.Add(casilla.GetComponent<Caja>());
                        //Creamos la casilla suelo "debajo" de la actual para que se vea cuando se mueva
                        casilla2 = Instantiate(elSuelo, posMapa.transform);
                        casilla2.transform.localPosition = new Vector3(i, j, 0.1f);
                        //casilla2.transform.position.Set(casilla2.transform.position.x,casilla2.transform.position.y,casilla2.transform.position.z+0.1f);
                        break;
                    case 4:
                        casilla = Instantiate(laPosCaja, posMapa.transform);
                        casilla.transform.localPosition = new Vector3(i, j, 0.1f);
                        break;
                }
            }
        }
    }

    // Método que se llamará cuando cambie la opción seleccionada del dropdown
    public void OnDropdownValueChanged()
    {      
        //Al seleccionar un mapa lo cargamos
        Debug.Log("Opción seleccionada: "+elDropdown.value);
        CargarMapa(elDropdown.value);
        mapaActual=elDropdown.value;
    }

    public void Jugar(int numMapa){
        Debug.Log("Jugar. A jugar!");
        CargarMapas();
        modoJuego=0;
        juegoPausado = false;
        mapaActual=1;
        ReiniciarNivel();
        pantallaJugar.SetActive(true);
        pantallaFinJuego.SetActive(false);
        pantallaEditorNiveles.SetActive(false);
        pantallaPausa.SetActive(false);
        JugarMapa(mapaActual);
//        Instantiate(elPlayer);
        elPlayer.transform.position = new Vector3(0, 0);
        pantallaPausa.SetActive(juegoPausado);
    }

    public void EditarNiveles(){
        Debug.Log("EditarNiveles. A editar niveles!");
        modoJuego=1;
        juegoPausado = false;
        pantallaPausa.SetActive(false);
        pantallaEditorNiveles.SetActive(true);
        pantallaJugar.SetActive(false);
        pantallaFinJuego.SetActive(false);
        CargarMapas();
    }

    private void PausarJuego()
    {
        juegoPausado = !juegoPausado;
        if( juegoPausado ){
            Time.timeScale = 0;
            pantallaEditorNiveles.SetActive(false);
            pantallaJugar.SetActive(false);
        }else{
            Time.timeScale = 1;
            if(modoJuego == 0){
                pantallaEditorNiveles.SetActive(false);
                pantallaJugar.SetActive(true);
            }else{
                pantallaEditorNiveles.SetActive(true);
                pantallaJugar.SetActive(false);
            }
        }
        pantallaPausa.SetActive(juegoPausado);
        AudioListener.pause = juegoPausado;
    }

    public void CajaColocada(){
        int totalColocadas = ContarColocadas();
        if( totalColocadas == numCajas){
            Debug.Log("Cargando siguiente mapa.");
            mapaActual++;
            if( lamapsCollection.mapas.Count>mapaActual){
                ReiniciarNivel();
                JugarMapa(mapaActual);
            }else{
                FindDeJuego();
            }
        }
    }

    public int ContarColocadas(){
        int cuantas=0;
        foreach( Caja unaCaja in listaCajas){
            if(unaCaja.estoyColocada == true)
                cuantas++;
        }
        return cuantas;
    }

    //Preparamos el nivel para cargar reiniciando lo necesario
    private void ReiniciarNivel(){
        pantallaJugar.transform.Find("NumLevel").GetComponent<TMP_Text>().SetText("Level :"+mapaActual);
        numCajas=0;
        laCamara.transform.SetParent(null);
        foreach (Transform child in posMapa.transform)
        {
Debug.Log("Destruyendo a: "+child.name);            
            // Destruir cada hijo
            Destroy(child.gameObject);
        }
    }
    private void FindDeJuego(){
        pantallaFinJuego.SetActive(true);
    }

    public void Salir(){
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
