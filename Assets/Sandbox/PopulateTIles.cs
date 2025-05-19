using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PopulateTIles : MonoBehaviour
{
    public Camera mainCamera;

    public Tilemap tilemap;
    public TileBase tile;
    public int width;
    public int height;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera.transform.position= new Vector3(width/2,height/10,-10);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                tilemap.SetTile(new Vector3Int(i, j), tile);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
