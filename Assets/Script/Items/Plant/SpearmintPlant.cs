using System.Collections;
using Botography.OverworldInteraction;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static InventoryConstants;
using Botography.Player;
using Botography.Player.StatusEffects;
using Botography.Notifications;
using Unity.VisualScripting;
using UnityEngine.VFX;

public class SpearmintPlant : BasePlant, IRadiusPlaceable, IConsumable
{
    bool _withinRadius = false;
    TimedStatusEffect _consumableEffect = new TimedStatusEffect(PlayerConstants.StatusEffectType.Cooling, 30.0f);
    private float originalRadius;
    [SerializeField] private SpriteRenderer visualEffect;
    [SerializeField] private GameObject visualEffectLvl1;
    [SerializeField] private GameObject visualEffectLvl2;
    [SerializeField] private GameObject visualEffectLvl3;

    public void ConsumableAbility()
    {
        StatusEffectsHandler.Instance.AddTimedSE(_consumableEffect);
    }

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
        Notifyer.Instance.Notify(plantSO.name + " has been picked up!");

        InventoryManager.Instance.addItemToInv(plantSO);
        Destroy(objectToDestroy);

        PlayerStateMachine.Instance.BindControls();
    }

    public void PlaceableAbility()
    {
        if(_withinRadius){
            StatusEffectsHandler.Instance.Temperature--;
        }
        else
        {
            StatusEffectsHandler.Instance.Temperature++;
        }
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
        obj.GetComponent<SpearmintPlant>().StartCoroutine(PlaceAnimation());
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
        return type == InventoryItemType.Expendable || type == InventoryItemType.PlaceableRadius;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
			_withinRadius = true;
			PlaceableAbility();
		}
    }

    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
			_withinRadius = false;
			PlaceableAbility();
		}
    }


    // Start is called before the first frame update
    void Start()
    {
        originalRadius = gameObject.GetComponent<CircleCollider2D>().radius;
        visualEffect.sortingLayerName = gameObject.GetComponent<SpriteRenderer>().sortingLayerName;
        if (gameObject.layer == 7)
            visualEffectLvl1.SetActive(true);
        else if (gameObject.layer == 8)
            visualEffectLvl2.SetActive(true);
        else if (gameObject.layer == 9)
            visualEffectLvl3.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseRadius()
    {
        gameObject.GetComponent<CircleCollider2D>().radius = originalRadius + 6.0f;
    }

    public void ResetRadius()
    {
        gameObject.GetComponent<CircleCollider2D>().radius = originalRadius;
    }
}
