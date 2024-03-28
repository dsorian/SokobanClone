using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MatrixData
{
    public int[,] matrix;

    public MatrixData(int rows, int columns)
    {
        matrix = new int[rows, columns];
    }
}


[CreateAssetMenu(fileName = "NewMapsCollection", menuName = "Custom/MapsCollection")]
public class MapsCollection : ScriptableObject
{
    public List<MatrixData> mapas = new List<MatrixData>();

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
}

