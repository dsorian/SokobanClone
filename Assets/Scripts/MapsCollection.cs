using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class MapMatrix
{
    /*
    Como no consigo guardar la matriz en el JSon la guardo como un array unidimensional guardando el número
    de filas y columnas.
    Accederé a la celda [row,col] como [row*col+col]
    */
    public int[] matrix;
    public int rows, cols; 
    public String nombreMapa = "Nivel?";
    public int numMapa;

    public MapMatrix(int rows, int cols)
    {
        matrix = new int[rows * cols];
        this.rows = rows; 
        this.cols = cols;
    }
}

//[CreateAssetMenu(fileName = "NewMapsCollection", menuName = "Custom/MapsCollection")]
public class MapsCollection : ScriptableObject
{
    public List<MapMatrix> mapas;

    public int filas, columnas;
 
    public MapsCollection(){
        mapas = new List<MapMatrix>();
    }
        
    public void CreateIniMap(int filas, int columnas){
        Debug.Log("Creando el scriptable Object");
        //Creamos el primer mapa todo con casillas de suelo
        mapas = new List<MapMatrix>();
        mapas.Add(new MapMatrix(filas,columnas));

        for (int i = 0; i < filas ; i++ ){
            for( int j = 0; j<columnas;j++){
                mapas[0].matrix[i * this.columnas + j ] = 0;
            }
        }
        mapas[0].nombreMapa="Nivel "+(mapas.Count-1);
        this.filas = filas;
        this.columnas = columnas;
    }

    public int CreateEmptyMap(int filas, int columnas){
        //Creamos un mapa nuevo todo con casillas de suelo
        mapas.Add(new MapMatrix(filas,columnas));

        for (int i = 0; i < filas ; i++ ){
            for( int j = 0; j<columnas;j++){
                 mapas[mapas.Count-1].matrix[i * this.columnas + j ] = 0;
            }
        }
        mapas[mapas.Count-1].nombreMapa="Nivel "+(mapas.Count-1);
        return mapas.Count-1;
    }
    public int AddNewMap(int[] elMapa,int rows, int cols){
        mapas.Add(new MapMatrix(rows,cols));
        int numMapas = mapas.Count-1;
        mapas[numMapas].matrix= elMapa;
        mapas[numMapas].nombreMapa = "Nivel "+numMapas;
        return numMapas;
    }

    public void GetMapAt(int numMapa, out int[] elMapa){
        Debug.Log("Voy a devolver el mapa número: "+numMapa);
        elMapa = mapas[numMapa].matrix;
    }

    public void Reset(){
        mapas.Clear();
    }

    public void RemoveMap(MapMatrix elMapa){
        mapas.Remove(elMapa);
    }

    public List<string> GetMapsNames(){
        List<string> mapsNames = new List<string>();

        foreach (var map in mapas){
            mapsNames.Add(map.nombreMapa);
        }
        return mapsNames;
    }

    /*
     *Creará los mapas de los niveles del juego
     */
    public void CrearMapasJuego(){
        /*
        int[] nivel1 = {0,0,0,0,1,0,0,0,0};
        int[] nivel2 = {0,0,0,0,2,0,0,0,0};
        int[] nivel3 = {0,0,0,0,3,0,0,0,0};
        int[] nivel4 = {0,0,0,0,4,0,0,0,0};
        */
        int[] nivel1 = {2,2,2,2,2,2,2,2,2,2,2,0,0,0,0,0,0,0,0,2,2,0,3,2,2,2,2,0,0,2,2,0,0,0,0,0,0,0,0,2,2,0,0,2,2,2,2,0,0,2,2,0,0,1,0,3,0,0,0,2,2,0,0,4,0,0,2,3,0,2,2,0,0,0,0,0,2,0,0,2,2,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0};
        int[] nivel2 = {1,0,0,0,2,3,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
        int[] nivel3 = {1,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
        int[] nivel4 = {1,0,0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};

        AddNewMap(nivel1,3,3);
        AddNewMap(nivel2,3,3);
        AddNewMap(nivel3,3,3);
        AddNewMap(nivel4,3,3);
    }

}

