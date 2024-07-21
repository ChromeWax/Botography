using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Botography.OverworldInteraction;
using Botography.Notifications;
using Botography.Player;
using static InventoryConstants;

public abstract class EquipmentPlant : BasePlant, IEquipment, IDecorativePlaceable
{
    public BodyPart bodyPart;

	public virtual void EquipmentAbility()
    {
        throw new System.NotImplementedException();
    }
	
	public virtual bool IsOfInventoryType(InventoryItemType type)
	{
        return type == InventoryItemType.EquippableFeet;
	}
	
    public abstract void Equipped();
	
	public abstract void Unequipped();

    public void Pickup(Vector3Int gridPositionInt, Dictionary<Vector3Int, GameObject> objCoordinatesDict, GameObject objectToDestroy)
    {
        StartCoroutine(PickupAnimationAndNotify(gridPositionInt, objCoordinatesDict, objectToDestroy));
    }
   
    private IEnumerator PickupAnimationAndNotify(Vector3Int gridPositionInt, Dictionary<Vector3Int, GameObject> objCoordinatesDict, GameObject objectToDestroy)
    {
        PlayerStateMachine.Instance.UnbindControls();
        PlayerManager.Instance.PlayAnimation("Player_Pluck");
        yield return new WaitForSeconds(0.5f);

        objCoordinatesDict.Remove(gridPositionInt);
        Notifyer.Instance.Notify(plantSO.getPlantName() + " has been picked up!");

        InventoryManager.Instance.addItemToInv(plantSO);
        Destroy(objectToDestroy);

        PlayerStateMachine.Instance.BindControls();
    }

    public void PlaceableAbility()
    {
        return;
    }

    public void instantiateObject(Vector3Int gridPositionInt, GameObject selectedObj, TilemapRenderer curRenderer, Dictionary<Vector3Int, GameObject> objCoordinatesDict)
    {
        GameObject obj = Instantiate(selectedObj);
        obj.GetComponent<EquipmentPlant>().StartCoroutine(PlaceAnimation());
		obj.GetComponent<SpriteRenderer>().sortingLayerID = curRenderer.sortingLayerID;
		obj.layer = curRenderer.gameObject.layer;
		float xOff = (Random.Range(-.15f, .15f));				//Adds some variation to the placement of the plant (makes it look less griddy)
		float yOff = (Random.Range(-.15f, .15f));
		Vector3 position = OWIManager.V3ItoV3(gridPositionInt); // Removed the offsets for now. Would like to add them back in. Making plants appear in weird spots, though -Brandon
		obj.transform.position = position;
		objCoordinatesDict.Add(gridPositionInt, obj);			//Coordinates used for saving and for placement
    }

    private IEnumerator PlaceAnimation()
    {
        PlayerStateMachine.Instance.UnbindControls();
        PlayerManager.Instance.PlayAnimation("Player_Use");
        yield return new WaitForSeconds(0.5f);
        PlayerStateMachine.Instance.BindControls();
    }

    public GameObject ValidatePlacement(Dictionary<int, GameObject> tilemapDict, GameObject playerController, Vector3Int gridPos)
    {
        GameObject curTilemapObj = tilemapDict[playerController.layer];			//Gets the tilemap GameObject the player is currently on
		Tilemap curTilemap = curTilemapObj.GetComponent<Tilemap>();				//Gets the tilemap component from ^^^
		TileBase tile = curTilemap.GetTile(gridPos);							//Gets the specific tile that was clicked
		
		if(tile == null)
			return null;
		return curTilemapObj;
    }
}
