using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RotateTile : MonoBehaviour
{
    [SerializeField] private TilesToRotateSO tilesToRotateSO;

    void Start()
    {
        Random.InitState(0);

        Tilemap tilemap = GetComponent<Tilemap>();

        Matrix4x4 rotationZero = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, 0f), Vector3.one);
        Matrix4x4 rotationNinty = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, 90f), Vector3.one);
        Matrix4x4 rotationOneEighty = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, 180f), Vector3.one);
        Matrix4x4 rotationTwoSeventy = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, 270f), Vector3.one);
        tilemap.SetTransformMatrix(new Vector3Int(0, 0, 0), rotationNinty);
        tilemap.SetTransformMatrix(new Vector3Int(0, -1, 0), rotationOneEighty);

        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++) 
        {
            for (int y = 0; y < bounds.size.y; y++) 
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null && tilesToRotateSO.tilesToRotate.Contains(tile)) 
                {
                    int guess = Random.Range(0, 4);
                    
                    if (guess == 0)
                        tilemap.SetTransformMatrix(new Vector3Int(bounds.xMin + x, bounds.yMin + y, 0), rotationZero);

                    else if (guess == 1)
                        tilemap.SetTransformMatrix(new Vector3Int(bounds.xMin + x, bounds.yMin + y, 0), rotationNinty);

                    else if (guess == 2)
                        tilemap.SetTransformMatrix(new Vector3Int(bounds.xMin + x, bounds.yMin + y, 0), rotationOneEighty);

                    else if (guess == 3)
                        tilemap.SetTransformMatrix(new Vector3Int(bounds.xMin + x, bounds.yMin + y, 0), rotationTwoSeventy);
                }
            }
        } 
    }
}
