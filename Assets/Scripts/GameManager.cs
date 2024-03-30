using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using System.IO;

public class GameManager : MonoBehaviour
{
    public GameObject[,] matrizMapa;    //Matriz con las figuras que conforman el mapa
    public int rows=3;                  //Filas del mapa
    public int cols=3;                  //Columnas del mapa
    public GameObject elementoMapaPrefab;  //Uno de los objetos que conforman el mapa
    public GameObject posMapa;      //Posición del mapa

    private int teselaSeleccionada;   //Indicar la tesela seleccionada de teselas

    public Sprite[] teselas;    //Las teselas que tenemos para poner

    private MapsCollection mapsCollection; //Referencia al scriptableObject que guardará los mapas del juego.

    private int mapaActual = 0;    //Mapa mostrado actualmente.

    // Start is called before the first frame update
    void Start()
    {
        matrizMapa = new GameObject[rows,cols];
        //Cargamos los mapas guardados 
        ReiniciarMapa();

        if( mapsCollection == null)
            Debug.Log("Los mapas son null!");
        else
            Debug.Log("Los mapas se han cargado correctamente.");


        // Inicializar la matriz de GameObjects
        CargarMapa(0);

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
        if (Input.GetKeyUp(KeyCode.Alpha2)){
            Debug.Log("Has pulsado 2");

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
        if (mapsCollection != null){
            Debug.Log("Cargar clicked y mapsCollection no es null.");
            int[] mapaCargado;
            mapsCollection.GetDataAt(0, out mapaCargado);
            if( mapaCargado != null){
                for(int i = 0; i< rows; i++){
                    for(int j = 0; j< cols; j++){
                        Debug.Log("Se ha cargado el mapa. Elmento: "+i+"-"+j+" Contiene: "+mapaCargado[i*j+j]);
                        matrizMapa[i,j].gameObject.GetComponent<ElementoMapa>().tipoTesela = mapaCargado[i*j+j];
                    }
                }
            }
            else{
                Debug.Log("Error al cargar el mapa. mapsCollection no es null pero LoadMap ha devuelto null.");
            }
        }else{
            Debug.Log("Error al cargar. mapsCollection es null");
        }
    }
    public void GuardarClicked(){
        Debug.Log("Guardar clicked.");
        // Obtener las matrices de algún lugar...
        GuardarMapaActual();
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
//Debug.Log("Guardando la casilla: "+i+"-"+j+" que contiene casilla tipo: "+matrizMapa[i,j].gameObject.GetComponent<ElementoMapa>().tipoTesela);
                    mapParaSalvar[i*j+j] = matrizMapa[i,j].gameObject.GetComponent<ElementoMapa>().tipoTesela;
                }
            }
            mapsCollection.AddNewData(mapParaSalvar,rows,cols);
        }
            for (int i = 0; i < rows; i++){
                for (int j = 0; j < cols; j++){
                    mapsCollection.mapas[0].matrix[i*j+j] = 88;
                    Debug.Log("Mostrando la casilla: "+i+"-"+j+": "+mapsCollection.mapas[0].matrix[i*j+j]);
                }
            }

        //Archivo donde guardaremos la información (texto plano, para más seguridad usaríamos binario pero así podemos verlo)
        string nombreArchivo = "/MapasSokoban"+SceneManager.GetActiveScene().buildIndex+"Mapa0.txt";
        string json = JsonUtility.ToJson(mapsCollection.mapas[0]);
Debug.Log("El json creado: "+json+" número de elementos en la matriz: "+mapsCollection.mapas[0].matrix.Length);
        //Guardamos la info


        if(File.Exists(Application.persistentDataPath+nombreArchivo)){
            Debug.Log("GuardarMapaActual(): El fichero json existe y lo voy a borrar para escribirlo de nuevo."+Application.persistentDataPath+nombreArchivo);
            File.Delete(Application.persistentDataPath + nombreArchivo);
        }
        File.WriteAllText(Application.persistentDataPath +nombreArchivo,json);
    }

    private void ReiniciarMapa(){
        mapsCollection = (MapsCollection) ScriptableObject.CreateInstance(typeof(MapsCollection));
        mapsCollection.CreateIniMap(rows,cols);        
        Debug.Log("Hemos creado el listado de mapas. Tenemos: "+mapsCollection.mapas.Count+" mapas.");
    }
    // Método que será llamado cuando se haga clic en un sprite
    public void OnSpriteClicked(ElementoMapa clickedElemento)
    {
        Debug.Log("Sprite clicado: " + clickedElemento.name);
        clickedElemento.gameObject.GetComponent<SpriteRenderer>().sprite = teselas[teselaSeleccionada];
        matrizMapa[clickedElemento.GetComponent<ElementoMapa>().fila , clickedElemento.GetComponent<ElementoMapa>().columna].GetComponent<ElementoMapa>().tipoTesela 
            = teselaSeleccionada;
    }

    private void CargarMapa(int numMapa){
        Debug.Log("CargarMapa. Vamos a ver si cargamos el mapa: "+numMapa);
//        GameObject[,] mapaTemp = new GameObject[rows, cols];
        int[] mapaTiposTesela;
        mapsCollection.GetDataAt(numMapa,out mapaTiposTesela);
        
if(mapaTiposTesela == null){
    Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
    return;
}
        // Crear y posicionar los sprites
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                // Instanciar el spritePrefab como un GameObject
                GameObject newSprite = Instantiate(elementoMapaPrefab, posMapa.transform);
                newSprite.gameObject.GetComponent<ElementoMapa>().fila = i;
                newSprite.gameObject.GetComponent<ElementoMapa>().columna = j;
                newSprite.gameObject.GetComponent<ElementoMapa>().tipoTesela = mapaTiposTesela[i*j+j];

                // Posicionar el sprite en la matriz
                newSprite.transform.localPosition = new Vector3(i, j, 0);

                // Guardar el sprite en la matriz de GameObjects
                matrizMapa[i, j] = newSprite;
            }
        }
    }
}
