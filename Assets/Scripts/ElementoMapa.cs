using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementoMapa : MonoBehaviour
{
    public int fila, columna;  //La fila y columna en la matriz de este elemento del mapa
    public int tipoTesela;   //0=suelo 1=player 2=pared 3=caja 4=posicionCaja

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown(){
        Debug.Log("Me has clickado, soy la casilla: "+fila+"-"+columna);
    }
}
