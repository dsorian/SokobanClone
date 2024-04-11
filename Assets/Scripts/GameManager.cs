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

    private MapsCollection mapsCollection; //Referencia al scriptableObject que guardará los mapas del juego.

    private int mapaActual = 0;    //Mapa mostrado actualmente.
    private bool juegoPausado = true;



    // Start is called before the first frame update
    void Start()
    {
        matrizMapa = new GameObject[rows*cols];
        //Cargamos los mapas guardados 
        ReiniciarMapa();

        CargarMapas();
    
      //  PausarJuego();

    }

    void Update()
    {
        
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
            }else
                Debug.Log("no ha funcionado el raycast");
        }

        if (Input.GetKeyUp(KeyCode.Alpha1)){
            Debug.Log("Has pulsado 1. Cargamos el mapa 1");
            JugarMapa(1);
        }

        if (Input.GetKeyUp(KeyCode.Escape)){
            if( !juegoPausado){
                juegoPausado = !juegoPausado;
                PausarJuego();
            }
        }

    }

    public void SueloClicked(){
        Debug.Log("Suelo clicked.");
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

    public void CargarClicked(){
        Debug.Log("CargarClicked. Borrar porque ya no se usa.");
       /*
        if (mapsCollection != null){
            Debug.Log("Cargar clicked y mapsCollection no es null.");

 //En lugar de todo esto, sólo habría que cambiar mapaActual al número que toque. 
 //mapaActual lo obtendremos del dropdown list y chinpun.

            int[] mapaCargado;
            mapsCollection.GetMapAt(elDropdown.gameObject.GetComponent<TMP_Dropdown>().value, out mapaCargado);
            if( mapaCargado != null){
                for(int i = 0; i< rows; i++){
                    for(int j = 0; j< cols; j++){
                        Debug.Log("Se ha cargado el mapa. Elmento: "+i+"-"+j+" Contiene: "+mapaCargado[i*cols+j]);
                        matrizMapa[i*cols+j].gameObject.GetComponent<ElementoMapa>().tipoTesela = mapaCargado[i*cols+j];
                    }
                }
            }
            else{
                Debug.Log("Error al cargar el mapa. mapsCollection no es null pero LoadMap ha devuelto null.");
            }
        }else{
            Debug.Log("Error al cargar. mapsCollection es null");
        }
        */
    }
    public void GuardarClicked(){
        Debug.Log("Guardar clicked. Deberemos guardar el mapa actual ");
        // Obtener las matrices de algún lugar...
        GuardarMapaActual();
    }

    //Creamos un nuevo mapa con todo suelo y lo añadimos al listado.
    public void NuevoMapaClicked(){
        Debug.Log("Hemos pulsado nuevo. Deberemos crear un nuevo mapa y añadirlo al listado.");
        int numMapa = mapsCollection.CreateEmptyMap(rows,cols);
Debug.Log("Hemos creado el mapa número: "+numMapa);        
        elDropdown.gameObject.GetComponent<TMP_Dropdown>().ClearOptions();
        elDropdown.gameObject.GetComponent<TMP_Dropdown>().AddOptions(mapsCollection.GetMapsNames());
        elDropdown.gameObject.GetComponent<TMP_Dropdown>().value = mapsCollection.mapas.Count;
    }

    private void GuardarMapaActual(){
        int[] mapParaSalvar = new int[rows*cols+cols];
        if (mapsCollection != null)
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
            mapsCollection.mapas[mapaActual].matrix = mapParaSalvar;
            Debug.Log("Mapa guardado. Faltaría actualizar droplist con el nombre del nuevo mapa. mapaActual:"+mapaActual+" número de mapas: "+mapsCollection.mapas.Count);
        }

        //Archivo donde guardaremos la información (texto plano, para más seguridad usaríamos binario pero así podemos verlo)
        string nombreArchivo = "/MapasSokoban"+SceneManager.GetActiveScene().buildIndex+"Mapa"+mapaActual+".txt";
        string json = JsonUtility.ToJson(mapsCollection.mapas[mapaActual]);
Debug.Log("El json creado: "+json+" número de elementos en la matriz: "+mapsCollection.mapas[mapaActual].matrix.Length);
        //Guardamos la info


        if(File.Exists(Application.persistentDataPath+nombreArchivo)){
            Debug.Log("GuardarMapaActual(): El fichero json existe y lo voy a borrar para escribirlo de nuevo."+Application.persistentDataPath+nombreArchivo);
            File.Delete(Application.persistentDataPath + nombreArchivo);
        }
        File.WriteAllText(Application.persistentDataPath+nombreArchivo,json);
    }

    private void ReiniciarMapa(){
        mapsCollection = (MapsCollection) ScriptableObject.CreateInstance(typeof(MapsCollection));
        mapsCollection.CreateIniMap(rows,cols);
        Debug.Log("Hemos creado el listado de mapas. Tenemos: "+mapsCollection.mapas.Count+" mapas.");
    }

    // Método que será llamado cuando se haga clic en un sprite
    public void OnSpriteClicked(ElementoMapa clickedElemento)
    {
        int fila = clickedElemento.GetComponent<ElementoMapa>().fila;
        int columna = clickedElemento.GetComponent<ElementoMapa>().columna;
        Debug.Log("Sprite clicado: "+ fila+"-"+columna+", tenía casilla: " + clickedElemento.GetComponent<ElementoMapa>().tipoTesela+" Cambiaré: "+(fila*columna+columna));
        clickedElemento.gameObject.GetComponent<SpriteRenderer>().sprite = teselasDisponibles[teselaSeleccionada];
        //matrizMapa[fila * columna + columna].gameObject.GetComponent<SpriteRenderer>().sprite = teselas[teselaSeleccionada];
        clickedElemento.GetComponent<ElementoMapa>().tipoTesela = teselaSeleccionada;
        Debug.Log("Sprite clicado: "+ fila+"-"+columna+", ahora tiene casilla: " + clickedElemento.GetComponent<ElementoMapa>().tipoTesela);
    }

    private void CargarMapa(int numMapa){
        Debug.Log("CargarMapa. Vamos a ver si cargamos el mapa: "+numMapa+". Mapas cargados: "+mapsCollection.mapas.Count);
        int[] mapaTiposTeselaTemp;
        //Cogemos los datos de las casillas del mapa en cuestión
        mapsCollection.GetMapAt(numMapa,out mapaTiposTeselaTemp);

        // Crear y posicionar los sprites
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                // Instanciar el spritePrefab como un GameObject
                /*
                GameObject newSprite = Instantiate(elementoMapaPrefab, posMapa.transform);
                newSprite.gameObject.GetComponent<ElementoMapa>().fila = i;
                newSprite.gameObject.GetComponent<ElementoMapa>().columna = j;
                newSprite.gameObject.GetComponent<ElementoMapa>().tipoTesela = mapaTiposTeselaTemp[i*cols+j];
                newSprite.gameObject.GetComponent<SpriteRenderer>().sprite = teselas[mapaTiposTeselaTemp[i*cols+j]];

                // Posicionar el sprite en la matriz
                newSprite.transform.localPosition = new Vector3(i, j, 0);
                // Guardar el sprite en la matriz de GameObjects

                matrizMapa[i * cols + j] = newSprite;
*/
Debug.Log("KK: Cargando mapa: "+numMapa+" celda: "+i+"-"+j+" Tiene: "+matrizMapa[i * cols + j].gameObject.GetComponent<ElementoMapa>().tipoTesela);
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
        mapsCollection.CrearMapasJuego();
        //Inicializamos la matriz de casillas
/*CREO QUE ESTO YA NO HARÁ FALTA*/
        if( modoJuego == 1 ){ //Diseñar nivel
Debug.Log("kaka");        
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
/**/        
        elDropdown.gameObject.GetComponent<TMP_Dropdown>().ClearOptions();
        elDropdown.gameObject.GetComponent<TMP_Dropdown>().AddOptions(mapsCollection.GetMapsNames());

        Debug.Log("FALTA BUSCAR LOS MAPAS GUARDADOS Y CARGARLOS.");
    }

    private void JugarMapa(int numMapa){
        Debug.Log("JugarMapa. Vamos a ver si cargamos el mapa: "+numMapa+" para jugarlo Mapas cargados: "+mapsCollection.mapas.Count);
        int[] mapaTiposTeselaTemp;
        //Cogemos los datos de las casillas del mapa en cuestión
        mapsCollection.GetMapAt(numMapa,out mapaTiposTeselaTemp);
Debug.Log("MOSTRANDO MAPA A JUGAR: "+numMapa);
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Debug.Log("Casilla: "+i+"-"+j+" tiene: "+ mapaTiposTeselaTemp[i*cols+j]);
            }
        }
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
                        casilla.transform.localPosition = new Vector3(i, j, 0);
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
                //matrizMapa[i*cols+j] = elPlayer;

/*
                matrizMapa[i * cols + j].gameObject.GetComponent<ElementoMapa>().fila = i;
                matrizMapa[i * cols + j].gameObject.GetComponent<ElementoMapa>().columna = j;
                matrizMapa[i * cols + j].gameObject.GetComponent<ElementoMapa>().tipoTesela = mapaTiposTeselaTemp[i*cols+j];
                matrizMapa[i * cols + j].gameObject.GetComponent<SpriteRenderer>().sprite = teselasDisponibles[mapaTiposTeselaTemp[i*cols+j]];
*/                
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
        modoJuego=0;
        juegoPausado = false;
        JugarMapa(1);
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
        CargarMapas();
    }

    private void PausarJuego()
    {
        if( juegoPausado ){
            Time.timeScale = 0;
        }else{
            Time.timeScale = 1;
        }
        AudioListener.pause = juegoPausado;
        pantallaPausa.SetActive(juegoPausado);
    }

}
