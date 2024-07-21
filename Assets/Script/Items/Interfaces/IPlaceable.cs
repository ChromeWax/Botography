using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

interface IPlaceable : IInventoryItem
{
    void Pickup(Vector3Int gridPositionInt, Dictionary<Vector3Int, GameObject> objCoordinatesDict, GameObject objectToDestroy);
    void PlaceableAbility();
	void instantiateObject(Vector3Int gridPositionInt, GameObject selectedObj, TilemapRenderer curRenderer, Dictionary<Vector3Int, GameObject> objCoordinatesDict);
	GameObject ValidatePlacement(Dictionary<int, GameObject> tilemapDict, GameObject playerController, Vector3Int gridPos);
}
