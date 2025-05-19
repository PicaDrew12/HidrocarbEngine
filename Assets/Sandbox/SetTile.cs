using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class SetTile : MonoBehaviour
{
    
    public Tilemap tilemap;
    public string tileType;
    public TileBase emptyTile;
    public TileBase tileToSet;
    public OrganicElement_SO carbon;
    public OrganicElement_SO hydrogen;
    public Conection_SO sigma;
    private TileBase sigmaTile;
    public Camera mainCamera;

    void Start()
    {
        sigmaTile = sigma.tile;
        
    }

    public void SetTileType(string type)
    {
        tileType = type;
        if (type == "C")
        {
            tileToSet = carbon.Tile;
        }
        else if (type == "H")
        {
            tileToSet = hydrogen.Tile;
        }
        else if (type == "sigma")
        {
            tileToSet = sigmaTile;
        }
    }

    public void PlaceTile(Vector3Int cellPosition)
    {
        tilemap.SetTile(cellPosition, tileToSet);
    }

    void Update()
    {
        // Check if the mouse is not over UI elements
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int cellPosition = tilemap.WorldToCell(mouseWorldPosition);
                PlaceTile(cellPosition);
            }
            if (Input.GetMouseButtonDown(1))
            {
                Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int cellPosition = tilemap.WorldToCell(mouseWorldPosition);
                tilemap.SetTile(cellPosition, emptyTile);
            }
        }
    }
}
