using System.Collections;
using System.Collections.Generic;
using Botography.OverworldInteraction;
using Botography.Player;
using Botography.Notifications;
using UnityEngine;
using UnityEngine.Tilemaps;
using static InventoryConstants;
using System.Linq;
using Unity.VisualScripting;

public class GrowcapPlant : BasePlant, IRadiusPlaceable
{
    public void PlaceableRadiusAbility(){
        Collider2D[] _plantsInsideRadius;
        _plantsInsideRadius = Physics2D.OverlapCircleAll((Vector2) transform.position, 3.0f, ~PlayerManager.Instance.gameObject.layer);
        if (_plantsInsideRadius != null)
        {
            foreach (var _plant in _plantsInsideRadius)
            {
                if (_plant.GetComponent<IRadiusPlaceable>() != null)
                {
                    ((IRadiusPlaceable)_plant.GetComponent<BasePlant>()).IncreaseRadius();
                }
            }
        }
    }

    public void RemoveAbility(){
        Collider2D[] _plantsInsideRadius;
        _plantsInsideRadius = Physics2D.OverlapCircleAll((Vector2) transform.position, 3.0f, ~PlayerManager.Instance.gameObject.layer);
        if (_plantsInsideRadius != null)
        {
            foreach (var _plant in _plantsInsideRadius)
            {
                if(_plant.GetComponent<IRadiusPlaceable>() != null)
                {
                    ((IRadiusPlaceable)_plant.GetComponent<BasePlant>()).ResetRadius();
                }
            }
        }
    }

    public void Pickup(Vector3Int gridPositionInt, Dictionary<Vector3Int, GameObject> objCoordinatesDict, GameObject objectToDestroy)
	{
        RemoveAbility();
        StartCoroutine(PickupAnimationAndNotify(gridPositionInt, objCoordinatesDict, objectToDestroy));
	}

    private void OnTriggerStay2D(Collider2D collider)
    {
        PlaceableRadiusAbility();
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

	public GameObject ValidatePlacement(Dictionary<int, GameObject> tilemapDict, GameObject playerController, Vector3Int gridPos)
    {
        GameObject curTilemapObj = tilemapDict[playerController.layer];			//Gets the tilemap GameObject the player is currently on
		Tilemap curTilemap = curTilemapObj.GetComponent<Tilemap>();				//Gets the tilemap component from ^^^
		TileBase tile = curTilemap.GetTile(gridPos);							//Gets the specific tile that was clicked
		
		if(tile == null)
			return null;
		return curTilemapObj;
    }
	
	public void instantiateObject(Vector3Int gridPositionInt, GameObject selectedObj, TilemapRenderer curRenderer, Dictionary<Vector3Int, GameObject> objCoordinatesDict)
	{
        GameObject obj = Instantiate(selectedObj);
        obj.GetComponent<GrowcapPlant>().StartCoroutine(PlaceAnimation());
		obj.GetComponent<SpriteRenderer>().sortingLayerID = curRenderer.sortingLayerID;
		obj.layer = curRenderer.gameObject.layer;
		float xOff = (Random.Range(-.15f, .15f));				//Adds some variation to the placement of the plant (makes it look less griddy)
		float yOff = (Random.Range(-.15f, .15f));
		Vector3 position = OWIManager.V3ItoV3(gridPositionInt);
		obj.transform.position = position;
		objCoordinatesDict.Add(gridPositionInt, obj);			//Coordinates used for saving and for placement
        //obj.GetComponent<GrowcapPlant>().PlaceableRadiusAbility();
	}

    private IEnumerator PlaceAnimation()
    {
        PlayerStateMachine.Instance.UnbindControls();
        PlayerManager.Instance.PlayAnimation("Player_Use");
        yield return new WaitForSeconds(0.5f);
        PlayerStateMachine.Instance.BindControls();
    }

    public bool IsOfInventoryType(InventoryItemType type)
    {
        return type == InventoryItemType.PlaceableInteractive;
    }

    public void IncreaseRadius()
    {
        return;
    }

    public void ResetRadius()
    {
        return;
    }

    public void PlaceableAbility()
    {
        return;
    }
}
