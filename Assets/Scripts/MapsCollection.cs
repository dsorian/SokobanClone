using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class MapMatrix
{
    public int[,] matrix;
    public String nombreMapa = "Nivel ?";

    public MapMatrix(int rows, int columns)
    {
        matrix = new int[rows, columns];
    }
}

//[CreateAssetMenu(fileName = "NewMapsCollection", menuName = "Custom/MapsCollection")]
public class MapsCollection : ScriptableObject
{
    public List<MapMatrix> mapas;
    public int numMapas;

    public MapsCollection(){
        mapas = new List<MapMatrix>();
        numMapas=0;
    }
        
    public void CreateIniMap(int filas, int columnas){
        Debug.Log("Creando el scriptable Object");
        //Creamos el primer mapa todo con casillas de suelo
        mapas = new List<MapMatrix>();
        mapas.Add(new MapMatrix(filas,columnas));

        //int[,] unMapa = new int[filas,columnas];        

        for (int i = 0; i < filas ; i++ ){
            for( int j = 0; j<columnas;j++){
                Debug.Log("Creando una casilla del mapa inicializado, todo a 0 (suelo).");
                mapas[0].matrix[i,j] = 0;
            }
        }
        //mapas.Add(unMapa);
        numMapas = 1;
    }

    public void AddNewData(int[,] elMapa){
        mapas.Add(new MapMatrix(elMapa.GetLength(0),elMapa.GetLength(1)));
        //numMapas++;
        mapas[numMapas].matrix= elMapa;
        mapas[numMapas].nombreMapa = "Nivel "+numMapas;
    }

    public void GetDataAt(int numMapa, out int[,] elMapa){
        elMapa = mapas[numMapa].matrix;
    }

    public void Reset(){
        mapas.Clear();
    }

    public void RemoveData(MapMatrix elMapa){
        mapas.Remove(elMapa);
    }

/*
    // Guardar la información de las matrices en el ScriptableObject
    public void SaveMap(int[,] mapaParaSalvar){
        mapas.Clear();

        MatrixData newMap = new MatrixData(mapaParaSalvar.GetLength(0), mapaParaSalvar.GetLength(1)); // Crear una nueva instancia de MatrixData
        newMap.matrix = mapaParaSalvar;
        mapas.Add(newMap);

        Debug.Log("Matrices guardadas en ScriptableObject.");
    }
    // Cargar la información de las matrices desde el ScriptableObject
    public List<int[,]> LoadMaps()
    {
        List<int[,]> loadedMatrices = new List<int[,]>();

        foreach (var matrixData in mapas)
        {
            loadedMatrices.Add(matrixData.matrix);
        }

        return loadedMatrices;
    }

    public int[,] LoadMap(int index)
    {
        if (index >= 0 && index < mapas.Count)
        {
            Debug.Log("En MapsCollection.LoadMap("+index+"). Mapas que existen: "+mapas.Count);
            for(int i=0;i<9;i++){
                for(int j=0;j<9;j++){
                    Debug.Log("Recorriendo el mapa. elemento: "+i+"-"+j+" contiene: "+mapas[index].matrix[i,j]);
                }
            }
            return mapas[index].matrix;
        }
        else
        {
            Debug.LogWarning("Índice de mapa fuera de rango.");
            return null;
        }
    }

    // Elimina una matriz de la colección por su índice
    public void RemoveMap(int index)
    {
        if (index >= 0 && index < mapas.Count)
        {
            mapas.RemoveAt(index);
            Debug.Log("Mapa eliminado.");
        }
        else
        {
            Debug.LogWarning("Índice de mapa fuera de rango.");
        }
    }
*/
}

