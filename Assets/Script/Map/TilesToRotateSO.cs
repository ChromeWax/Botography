using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TilesToRotateSO", menuName = "TilesToRotate")]
public class TilesToRotateSO : ScriptableObject
{
    public TileBase[] tilesToRotate;
}
