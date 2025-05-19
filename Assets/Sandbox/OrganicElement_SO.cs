using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName ="OrganicElement",menuName ="HidrocarbEngine/OrganicElement")]
public class OrganicElement_SO : ScriptableObject
{
    public string nume;
    public string symbol;
    public int Z;
    public float AtmoicMass;
    public int valenta;
    public GameObject Model;
    public TileBase Tile;



    
}
