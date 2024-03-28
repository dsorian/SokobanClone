using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    public int kakota = 4;

    // Start is called before the first frame update
    void Start()
    {
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
            int[,] mapaCargado = mapsCollection.LoadMap(0);
            if( mapaCargado != null){
                for(int i = 0; i< rows; i++){
                    for(int j = 0; j< cols; j++){
                        Debug.Log("Se ha cargado el mapa. Elmento: "+i+"-"+j+" Contiene: "+mapaCargado[i,j]);
                        matrizMapa[i,j].gameObject.GetComponent<ElementoMapa>().tipoTesela = mapaCargado[i,j];
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
        int[,] mapParaSalvar = new int[rows,cols];

        if (mapsCollection != null)
        {
            //Generamos la matriz de números para guardar
            // Crear y posicionar los sprites
            for (int i = 0; i < rows; i++){
                for (int j = 0; j < cols; j++){

                    //Guardamos el tipo de baldosa
                    //tipoTesela--> 0=suelo 1=player 2=pared 3=caja 4=posicionCaja
Debug.Log("Guardando la casilla: "+i+"-"+j+" que contiene casilla tipo: "+matrizMapa[i,j].gameObject.GetComponent<ElementoMapa>().tipoTesela);
                    mapParaSalvar[i,j] = matrizMapa[i,j].gameObject.GetComponent<ElementoMapa>().tipoTesela;
                }
            }
            
            mapsCollection.SaveMap(mapParaSalvar);
        }
    }

    private void ReiniciarMapa(){
        //bestLapSO = (GhostLapData) ScriptableObject.CreateInstance(typeof(GhostLapData));
        //currentLapSO = (GhostLapData) ScriptableObject.CreateInstance(typeof(GhostLapData));

        //        mapsCollection.LoadMaps();
        mapsCollection = (MapsCollection) ScriptableObject.CreateInstance(typeof(MapsCollection));


    }
    // Método que será llamado cuando se haga clic en un sprite
    public void OnSpriteClicked(ElementoMapa clickedElemento)
    {
        Debug.Log("Sprite clicado: " + clickedElemento.name);
        clickedElemento.gameObject.GetComponent<SpriteRenderer>().sprite = teselas[teselaSeleccionada];
        matrizMapa[clickedElemento.GetComponent<ElementoMapa>().fila,clickedElemento.GetComponent<ElementoMapa>().columna].GetComponent<ElementoMapa>().tipoTesela 
            = clickedElemento.GetComponent<ElementoMapa>().tipoTesela;
    }

    private void CargarMapa(int numMapa){
        GameObject[,] mapaTemp = new GameObject[rows, cols];
        int[,] mapaTiposTesela = mapsCollection.LoadMap(numMapa);
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
                newSprite.gameObject.GetComponent<ElementoMapa>().tipoTesela = mapaTiposTesela[i,j];

                // Posicionar el sprite en la matriz
                newSprite.transform.localPosition = new Vector3(i, j, 0);

                // Guardar el sprite en la matriz de GameObjects
                matrizMapa[i, j] = newSprite;
            }
        }
    }
}
