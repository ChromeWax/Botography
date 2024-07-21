using Botography.OverworldInteraction;
using System.Collections;
using System.Collections.Generic;
using Botography.Notifications;
using Botography.Player;
using UnityEngine;

using UnityEngine.Tilemaps;
using static InventoryConstants;
using Unity.VisualScripting;
using UnityEngine.Rendering.Universal;
using Botography.Player.StatusEffects;

public class LanternPlant : BasePlant, IRadiusPlaceable
{
    private float originalRadius;
    private bool enabled;

    public void Pickup(Vector3Int gridPositionInt, Dictionary<Vector3Int, GameObject> objCoordinatesDict, GameObject objectToDestroy)
    {
        StartCoroutine(PickupAnimationAndNotify(gridPositionInt, objCoordinatesDict, objectToDestroy));
    }

    private void Start()
    {
        originalRadius = transform.Find("Light 2D").GetComponent<Light2D>().pointLightOuterRadius;

        // fix for vfx
        for (int i = 0; i < 3; i++)
            transform.GetChild(i).gameObject.SetActive(i == gameObject.layer - 7);
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
        throw new System.NotImplementedException();
    }
	
	//return the GameObject to place the object on if valid. Otherwise, return null
	public GameObject ValidatePlacement(Dictionary<int, GameObject> tilemapDict, GameObject playerController, Vector3Int gridPos)
	{
		GameObject curTilemapObj = tilemapDict[playerController.layer];			//Gets the tilemap GameObject the player is currently on
		Tilemap curTilemap = curTilemapObj.GetComponent<Tilemap>();				//Gets the tilemap component from ^^^
		TileBase tile = curTilemap.GetTile(gridPos);							//Gets the specific tile that was clicked
		
		if(tile == null)
			return null;
		return curTilemapObj;
	}
	
	//instantiate the object and move it to the appropriate position
	public void instantiateObject(Vector3Int gridPositionInt, GameObject selectedObj, TilemapRenderer curRenderer, Dictionary<Vector3Int, GameObject> objCoordinatesDict)
	{
		GameObject obj = Instantiate(selectedObj);
        obj.GetComponent<LanternPlant>().StartCoroutine(PlaceAnimation());
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

	public bool IsOfInventoryType(InventoryItemType type)
	{
		return type == InventoryItemType.PlaceableRadius;
	}

    public void IncreaseRadius()
    {
        transform.Find("Light 2D").GetComponent<Light2D>().pointLightOuterRadius = originalRadius + 3.0f;
    }

    public void ResetRadius()
    {
        transform.Find("Light 2D").GetComponent<Light2D>().pointLightOuterRadius = originalRadius;
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag.Equals("Player") && PlayerManager.Instance.GetCurrentLayer() == gameObject.layer)
		{
			PlayerManager.Instance.AddLantern();
            enabled = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag.Equals("Player") && PlayerManager.Instance.GetCurrentLayer() == gameObject.layer)
		{
			PlayerManager.Instance.RemoveLantern();
            enabled = false;
		}
	}

    private void Update()
    {
        if (enabled && PlayerManager.Instance.gameObject.layer != gameObject.layer)
        {
			PlayerManager.Instance.RemoveLantern();
            enabled = false;
        }
    }
}
