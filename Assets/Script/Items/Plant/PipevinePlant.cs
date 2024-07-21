using System.Collections;
using System.Collections.Generic;
using Botography.OverworldInteraction;
using Botography.Notifications;
using Botography.Player;
using Botography.Player.StatusEffects;
using UnityEngine;
using UnityEngine.Tilemaps;
using static InventoryConstants;

public class PipevinePlant : BasePlant, IInteractivePlaceable
{
    [SerializeField] private SpriteRenderer _spRend;
    public static PipevinePlant firstPipevine = null;
    public static PipevinePlant secondPipevine = null;
    public Animator Anim;
    private SpriteRenderer _spriteRenderer;
    public Transform StandingPos;

    // Start is called before the first frame update
	void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (firstPipevine != null && secondPipevine != null)
        {
            Notifyer.Instance.Notify("You can only place two of these plants");
            Destroy(gameObject);
        }
        else if (firstPipevine == null)
        {
            if (secondPipevine == null)
            {
                Anim.SetBool("Usable", false);
            }
            else
            {
                Anim.SetBool("Usable", true);
                secondPipevine.Anim.SetBool("Usable", true);
            }
            firstPipevine = this;
        }    
        else if (secondPipevine == null)
        {
			Anim.SetBool("Usable", true);
			firstPipevine.Anim.SetBool("Usable", true);
            secondPipevine = this;
        }
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

        if (firstPipevine == this)
        {
            firstPipevine = null;
            if (secondPipevine != null)
            {
                secondPipevine.Anim.SetBool("Usable", false);
            }
        }
        else if (secondPipevine == this)
        {
            secondPipevine = null;
            if (firstPipevine != null)
            {
                firstPipevine.Anim.SetBool("Usable", false);
            }
        }

		objCoordinatesDict.Remove(gridPositionInt);
        Notifyer.Instance.Notify(plantSO.getPlantName() + " has been picked up!");
        InventoryManager.Instance.addItemToInv(plantSO);
		Destroy(objectToDestroy);

		PlayerStateMachine.Instance.BindControls();
	}

    public void PlaceableAbility()
    {
        if ((firstPipevine == this && secondPipevine != null) || (secondPipevine == this && firstPipevine != null))
        {
			PlayerStateMachine.Instance.UnbindControls();
			PlayerManager.Instance.ShowInteractIcon(false);
            PlayerManager.Instance.SetPlayerPosition(StandingPos.position, gameObject.layer, _spriteRenderer.sortingLayerName);
            Anim.SetBool("TravelFrom", true);
            Anim.SetBool("TravelTo", false);
		}
        else if (firstPipevine == null || secondPipevine == null)
        {
            Notifyer.Instance.Notify("Nothing happened, maybe I need a second one");
        }
    }

    public void BeginTeleport()
    {
		PipevinePlant other = this == firstPipevine ? secondPipevine : firstPipevine;
		StartCoroutine(TeleportTransition(other.StandingPos.position, other.gameObject.layer, other._spriteRenderer.sortingLayerName));
	}

    private IEnumerator TeleportTransition(Vector3 position, int layer, string sortingLayerName)
    {
        TransitionManager.Instance.TransitionToBlackScreen();
        StatusEffectsHandler.Instance.StatusEffectsEnabled = false; 
        yield return new WaitForSeconds(1f);
		PlayerManager.Instance.SetPlayerPosition(position, layer, sortingLayerName);
		TransitionManager.Instance.TransitionToClearScreen();
        yield return new WaitForSeconds(1f);
        StatusEffectsHandler.Instance.StatusEffectsEnabled = true; 
		PipevinePlant other = this == firstPipevine ? secondPipevine : firstPipevine;
		other.Anim.SetBool("TravelFrom", false);
		other.Anim.SetBool("TravelTo", true);
	}

    public void Arrive()
    {
		PlayerManager.Instance.AppearPlayer();
    }

    public void PickedUp()
    {
        PlayerManager.Instance.DisappearPlayer();
    }

    public void EndTeleport()
    {
		PlayerStateMachine.Instance.BindControls();
        PlayerManager.Instance.ShowInteractIcon(true);
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
        obj.GetComponent<PipevinePlant>().StartCoroutine(PlaceAnimation());
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
		return type == InventoryItemType.PlaceableInteractive;
	}

    // Update is called once per frame
    void Update()
    {
        
    }

    private void TogglePlayerVisibilityFeaturesOn()
    {
        PlayerManager.Instance.ToggleShadow(true);
        PlayerManager.Instance.ToggleOcclusion(true);
    }

    private void TogglePlayerVisibilityFeaturesOff()
    {
        PlayerManager.Instance.ToggleShadow(false);
        PlayerManager.Instance.ToggleOcclusion(false);
    }
}
