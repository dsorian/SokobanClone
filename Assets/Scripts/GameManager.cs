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
                Debug.Log("clonando sprite"+i+" - "+j);
                // Instanciar el spritePrefab como un GameObject
                GameObject newSprite = Instantiate(elementoMapaPrefab, posMapa.transform);

                // Posicionar el sprite en la matriz
                newSprite.transform.localPosition = new Vector3(i, j, 0);

                // Guardar el sprite en la matriz de GameObjects
                matrizMapa[i, j] = newSprite;
            }
        }
    }

    void Update()
    {
        // Detectar el clic del ratón
        if (Input.GetMouseButtonDown(0))
        {
           
        }
    }

    public void ParedSeleccionada(){
        Debug.Log("Pared seleccionada.");
    }

}
