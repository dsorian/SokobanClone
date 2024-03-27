using System.Collections;
using System.Collections.Generic;
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

    public MapsCollection mapsCollection; //Referencia al scriptableObject que guardará los mapas del juego.

    // Start is called before the first frame update
    void Start()
    {
        // Inicializar la matriz de GameObjects
        matrizMapa = new GameObject[rows, cols];
        // Crear y posicionar los sprites
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                // Instanciar el spritePrefab como un GameObject
                GameObject newSprite = Instantiate(elementoMapaPrefab, posMapa.transform);
                
                newSprite.gameObject.GetComponent<ElementoMapa>().fila = i;
                newSprite.gameObject.GetComponent<ElementoMapa>().columna = j
                ;
                // Posicionar el sprite en la matriz
                newSprite.transform.localPosition = new Vector3(i, j, 0);

                // Guardar el sprite en la matriz de GameObjects
                matrizMapa[i, j] = newSprite;
            }
        }
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
                GameObject clickedObject = hit.collider.gameObject;
                SpriteRenderer spriteRenderer = clickedObject.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    // Llamar al método OnSpriteClicked con el objeto clicado como argumento
                    OnSpriteClicked(clickedObject);
                }
            }else
                Debug.Log("no ha funcionado el raycast");
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
        Debug.Log("Cargar clicked.");
    }
    public void GuardarClicked(){
        Debug.Log("Guardar clicked.");
        if (mapsCollection != null)
        {
            // Obtener las matrices de algún lugar...
            List<int[,]> mapToSave = GetMapToSave();
            mapsCollection.SaveMatrices(mapToSave);
        }
    }

    private List<int[,]> GetMapToSave()
    {
        // Aquí obtienes las matrices de algún lugar (por ejemplo, de la escena, de otros objetos, etc.)
        return new List<int[,]>();
    }

    // Método que será llamado cuando se haga clic en un sprite
    public void OnSpriteClicked(GameObject clickedSprite)
    {
        Debug.Log("Sprite clicado: " + clickedSprite.name);
        clickedSprite.gameObject.GetComponent<SpriteRenderer>().sprite = teselas[teselaSeleccionada];
    }
}
