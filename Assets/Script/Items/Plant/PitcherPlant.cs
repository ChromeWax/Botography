using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Botography.OverworldInteraction;
using Botography.Notifications;
using Botography.Player;
using static InventoryConstants;

public class PitcherPlant : BasePlant, IConsumable, IDecorativePlaceable
{
    public void ConsumableAbility()
    {
		//Add whatever other abilities here, if any
    }
	
	public bool IsOfInventoryType(InventoryItemType type)
	{
		return type == InventoryItemType.Expendable;
	}

    /*public void instantiateObject(Vector3Int gridPositionInt, GameObject selectedObj, TilemapRenderer curRenderer, Dictionary<Vector3Int, GameObject> objCoordinatesDict)
    {
        throw new System.NotImplementedException();
    }

    public void Pickup(Vector3Int gridPositionInt, Dictionary<Vector3Int, GameObject> objCoordinatesDict, GameObject objectToDestroy)
    {
        throw new System.NotImplementedException();
    }

    public void PlaceableAbility()
    {
        throw new System.NotImplementedException();
    }

    public GameObject ValidatePlacement(Dictionary<int, GameObject> tilemapDict, GameObject playerController, Vector3Int gridPos)
    {
		Debug.Log("Validate placeable pitch");
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
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
        obj.GetComponent<PitcherPlant>().StartCoroutine(PlaceAnimation());
		obj.GetComponent<SpriteRenderer>().sortingLayerID = curRenderer.sortingLayerID;
		obj.layer = curRenderer.gameObject.layer;
		float xOff = (Random.Range(-.15f, .15f));				//Adds some variation to the placement of the plant (makes it look less griddy)
		float yOff = (Random.Range(-.15f, .15f));
		Vector3 position = OWIManager.V3ItoV3(gridPositionInt);
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
