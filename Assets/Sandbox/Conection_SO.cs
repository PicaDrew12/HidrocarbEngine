using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "OrganicElement", menuName = "HidrocarbEngine/Conection")]
public class Conection_SO : ScriptableObject
{
    public string nume;
    public int tieFactor;
    public TileBase tile;
}
