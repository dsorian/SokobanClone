using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public GameObject[][] mapa;    //Matriz con las figuras que conforman el mapa

    // Start is called before the first frame update
    void Start()
    {
        
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
