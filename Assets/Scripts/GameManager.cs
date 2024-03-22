using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase tile;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        // Detectar el clic del ratón
        if (Input.GetMouseButtonDown(0))
        {
            // Convertir la posición del ratón a coordenadas del Tilemap
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int tilePos = tilemap.WorldToCell(mouseWorldPos);

            // Colocar un tile en las coordenadas del Tilemap
            tilemap.SetTile(tilePos, tile);
        }
    }

}
