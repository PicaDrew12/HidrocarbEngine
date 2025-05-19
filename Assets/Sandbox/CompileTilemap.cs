using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Tilemaps;


public class CorectTilemapStructure
{
    public Vector3Int position;
    public string type;
    public CorectTilemapStructure(Vector3Int position, string type)
    {
        this.position = position;
        this.type = type;
    }
}



public class CompileTilemap : MonoBehaviour
{
    PopulateTIles PopulateGridWithCellsScript;

    
    public Tilemap tilemap;
    public OrganicElement_SO carbon;
    public OrganicElement_SO hydrogen;
    public Conection_SO sigma;
    private TileBase sigmaTile;

    private TileBase carbonTile;
    private TileBase hidrogenTile;

    public List<Vector3Int> carbonTilesPositions = new List<Vector3Int>();
    public List<Vector3Int> hidrogenTilesPositions = new List<Vector3Int>();
    public List<Vector3Int> sigmaTilesPositions = new List<Vector3Int>();
    public string SEPARATOR;
    public List<Vector3Int>goodCarbonTilesPositions = new List<Vector3Int>();
    public string targetElement = "n/a";
    public int longestCatenY = -1;
    [SerializeField] public List<CorectTilemapStructure> corectTilemapStructures = new List<CorectTilemapStructure>();
    public Vector3Int firstCarbonInList;


    void Start()
    {
        longestCatenY = int.MinValue;
        PopulateGridWithCellsScript = GameObject.FindGameObjectWithTag("Grid").GetComponent<PopulateTIles>();
        carbonTile = carbon.Tile;
        hidrogenTile = hydrogen.Tile;
        sigmaTile = sigma.tile;
    }

    public void AddItemsToStructureList(Vector3Int position,string type)
    {
        corectTilemapStructures.Add(new CorectTilemapStructure(position, type));
    }

    
    public void Compile()
    {
        ClearLists();
        BoundsInt bounds = tilemap.cellBounds;
        foreach(var cell in bounds.allPositionsWithin)
        {
            Vector3Int tilePosition = new Vector3Int(cell.x, cell.y, cell.z);  
            TileBase tile = tilemap.GetTile(tilePosition);
            if(tile == carbonTile )
            {
                targetElement = "C";
                
                if (CheckIfHasConections(tilePosition))
                {
                    carbonTilesPositions.Add(tilePosition);
                    
                }
                else
                {
                   
                }
            }
            else if(tile == hidrogenTile )
            {
                targetElement = "H";
                
                if(CheckIfHasConections(tilePosition))
                {
                   
                    hidrogenTilesPositions.Add(tilePosition);
                }
                else
                {
                    
                }
            }
            else if(tile == sigmaTile )
            {
                sigmaTilesPositions.Add(tilePosition);
            }
        }
        DetermineLongestCaten();
        DetermineStructure();
    }

    public bool CheckIfHasConections(Vector3Int positionToCheck)
    {
        int conectionCountFound = 0;
        if(tilemap.GetTile(positionToCheck + new Vector3Int(1, 0, 0)) == sigmaTile)
        {
            conectionCountFound++;
        }
        if (tilemap.GetTile(positionToCheck + new Vector3Int(-1, 0, 0)) == sigmaTile)
        {
            conectionCountFound++;
        }
        if (tilemap.GetTile(positionToCheck + new Vector3Int(0, 1, 0)) == sigmaTile)
        {
            conectionCountFound++;
        }
        if (tilemap.GetTile(positionToCheck + new Vector3Int(0, -1, 0)) == sigmaTile)
        {
            conectionCountFound++;
        }
        Debug.Log(conectionCountFound);
        
        
        if(targetElement == "C")
        {
            if (conectionCountFound>0&& conectionCountFound<=carbon.valenta)
            {
                return true;
            }
                
            else
            {
                return false ;
            }
        }
        else if(targetElement == "H")
        {
            if ( conectionCountFound > 0 && conectionCountFound <= hydrogen.valenta)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
        
    }

    public void DetermineLongestCaten()
    {
        for(int y = 0; y< PopulateGridWithCellsScript.width; y++)
        {
            int xLength = 0;
            for(int x = 0; x < PopulateGridWithCellsScript.height; x++)
            {
                Vector3Int target = new Vector3Int(x,y, 0);
                if (tilemap.GetTile(target) == carbonTile && tilemap.GetTile(target + Vector3Int.right) == sigmaTile)
                {
                    xLength++;
                }
            }
            if (xLength > longestCatenY)
            {
                longestCatenY = y;
            }
        }

    }


    public void DetermineStructure()
    {
        firstCarbonInList = new Vector3Int(-1,-1,-1);
        for (int i = 0; i < PopulateGridWithCellsScript.width; i++)
        {
            if(tilemap.GetTile(new Vector3Int(i,longestCatenY,0)) == carbonTile)
            {
                firstCarbonInList = new Vector3Int(i, longestCatenY, 0);
                break;
            }
            
        }
        
        

        
        //Carbon checking
        for (int i = firstCarbonInList.x; i <= PopulateGridWithCellsScript.width; i++)
        {
            if (tilemap.GetTile(new Vector3Int(i, firstCarbonInList.y, firstCarbonInList.z)) == carbonTile && tilemap.GetTile(new Vector3Int(i, firstCarbonInList.y, firstCarbonInList.z) + Vector3Int.right) == sigmaTile)
            {
                Vector3Int tileToCheck = new Vector3Int(i, firstCarbonInList.y, firstCarbonInList.z);
                for(int checkEachCarbonYCoordonate = 0; checkEachCarbonYCoordonate < tilemap.cellBounds.size.y; checkEachCarbonYCoordonate++)
                {
                    if (tilemap.GetTile(new Vector3Int(tileToCheck.x, checkEachCarbonYCoordonate, tileToCheck.z)) == carbonTile)
                    {
                        if (!isTileAlreadyInList(new Vector3Int(tileToCheck.x, checkEachCarbonYCoordonate, tileToCheck.z), goodCarbonTilesPositions))
                        {
                            goodCarbonTilesPositions.Add(tileToCheck);
                            AddItemsToStructureList(new Vector3Int(i, firstCarbonInList.y, firstCarbonInList.z), "C");
                        }   
                        
                    }
                }
                goodCarbonTilesPositions.Add(tileToCheck);
                AddItemsToStructureList(new Vector3Int(i, firstCarbonInList.y, firstCarbonInList.z),"C");

            }
            else if(tilemap.GetTile(new Vector3Int(i, firstCarbonInList.y, firstCarbonInList.z)) == carbonTile && tilemap.GetTile(new Vector3Int(i, firstCarbonInList.y, firstCarbonInList.z) + Vector3Int.left) == sigmaTile)
            {
                goodCarbonTilesPositions.Add(new Vector3Int(i, firstCarbonInList.y, firstCarbonInList.z));
                AddItemsToStructureList(new Vector3Int(i, firstCarbonInList.y, firstCarbonInList.z), "C");
            }
            
        }
        
        
        
    }

    public bool isTileAlreadyInList(Vector3Int position, List<Vector3Int> list)
    {
        bool foundItem = false;
        for (int i = 0; i < list.Count - 1; i++)
        {
            if (list[i] == position)
            {
                foundItem = true;
            }
            else
            {
                foundItem = false;
            }
            
        }
        if(foundItem )
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool CheckIfTileIsOnAxis(bool isX,int value,TileBase tileToCheck)
    {
        if (tileToCheck != null)
        {
            if (isX)
            {
                for (int xCoordonate = 0; xCoordonate < tilemap.cellBounds.size.x; xCoordonate++)
                {
                    if (tilemap.GetTile(new Vector3Int(xCoordonate, value, firstCarbonInList.z)) == tileToCheck)
                    {
                        return true;
                    }
                }
            }
            else
            {
                for (int yCoordonate = 0; yCoordonate < tilemap.cellBounds.size.y; yCoordonate++)
                {
                    if (tilemap.GetTile(new Vector3Int(value, yCoordonate, firstCarbonInList.z)) == tileToCheck)
                    {
                        return true;
                    }
                }
            }
            // If none of the coordinates satisfy the condition, return false outside the loop
            return false;
        }
        else
        {
            Debug.LogWarning($"Tile {tileToCheck} is null");
            return false;
        }

    }

    private void ClearLists()
    {
        
        longestCatenY = int.MinValue;
        corectTilemapStructures.Clear();
        carbonTilesPositions.Clear();
        hidrogenTilesPositions.Clear();
        sigmaTilesPositions.Clear();
        goodCarbonTilesPositions.Clear();

    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log(corectTilemapStructures.Count);
    }
}
