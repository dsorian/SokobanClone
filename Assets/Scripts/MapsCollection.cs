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
    public List<MatrixData> matrices = new List<MatrixData>();

    // Guardar la información de las matrices en el ScriptableObject
    public void SaveMatrices(List<int[,]> matricesToSave){
        matrices.Clear();

        foreach (var matrixData in matricesToSave)
        {
            MatrixData newMatrix = new MatrixData(matrixData.GetLength(0), matrixData.GetLength(1)); // Crear una nueva instancia de MatrixData
            newMatrix.matrix = matrixData;
            matrices.Add(newMatrix);
        }

        Debug.Log("Matrices guardadas en ScriptableObject.");
    }
    // Cargar la información de las matrices desde el ScriptableObject
    public List<int[,]> LoadMatrices()
    {
        List<int[,]> loadedMatrices = new List<int[,]>();

        foreach (var matrixData in matrices)
        {
            loadedMatrices.Add(matrixData.matrix);
        }

        return loadedMatrices;
    }
}

