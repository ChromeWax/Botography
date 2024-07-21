using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static InventoryConstants;
using Botography.Notifications;
using Botography.OverworldInteraction;
using Botography.Player;
using Unity.VisualScripting;

public class MuchroomPlant : Jar, IDecorativePlaceable
{
	[SerializeField] private Sprite _loamLevel1_5;
	[SerializeField] private Sprite _loamLevel2_5;
	[SerializeField] private Sprite _siltLevel1_5;
	[SerializeField] private Sprite _siltLevel2_5;
	[SerializeField] private Sprite _sandLevel1_5;
	[SerializeField] private Sprite _sandLevel2_5;
	[SerializeField] private Sprite _clayLevel1_5;
	[SerializeField] private Sprite _clayLevel2_5;
	[SerializeField] private Sprite _basicLevel1_5;
	[SerializeField] private Sprite _basicLevel2_5;
	[SerializeField] private Sprite _neutralLevel1_5;
	[SerializeField] private Sprite _neutralLevel2_5;
	[SerializeField] private Sprite _acidicLevel1_5;
	[SerializeField] private Sprite _acidicLevel2_5;

	[SerializeField] private GameObject _placeableMuchroom;
    [SerializeField] public PlantSO plantSO;
	public bool planted = false;

	private void Start()
	{
		MaxAmount = 5;
		thresh = new() { 1, 2, 3, 4, 5 };
        UpdateLevel();
	}

	protected override void SendNotification(AttributeValue type)
	{
		if (type == AttributeValue.Silt || type == AttributeValue.Sand || type == AttributeValue.Loam || type == AttributeValue.Clay)
			{
				Notifyer.Instance.Notify("Soil type: " + type.ToString() + ", has been added to muchroom jar");
				SoundManager.Instance.PlaySFX("soil collected");
			}
		else if (type == AttributeValue.Basic || type == AttributeValue.Acidic || type == AttributeValue.Neutral)
			{
				Notifyer.Instance.Notify("Water type: " + type.ToString() + ", has been added to muchroom jar");
				SoundManager.Instance.PlaySFX("water collected");
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

        objCoordinatesDict.Remove(gridPositionInt);
        Notifyer.Instance.Notify(plantSO.getPlantName() + " has been picked up!");

        InventoryManager.Instance.addMuchroomToInv();
        Destroy(objectToDestroy);

        PlayerStateMachine.Instance.BindControls();
    }

    public void PlaceableAbility()
    {
        return;
    }

    public void instantiateObject(Vector3Int gridPositionInt, GameObject selectedObj, TilemapRenderer curRenderer, Dictionary<Vector3Int, GameObject> objCoordinatesDict)
    {
		Debug.Log("Instantiate");
        GameObject obj = Instantiate(_placeableMuchroom);
        obj.GetComponent<MuchroomPlant>().StartCoroutine(PlaceAnimation());
		obj.GetComponent<SpriteRenderer>().sortingLayerID = curRenderer.sortingLayerID;
		obj.layer = curRenderer.gameObject.layer;
		float xOff = (Random.Range(-.15f, .15f));				//Adds some variation to the placement of the plant (makes it look less griddy)
		float yOff = (Random.Range(-.15f, .15f));
		Vector3 position = OWIManager.V3ItoV3(gridPositionInt);
		obj.transform.position = position;
		objCoordinatesDict.Add(gridPositionInt, obj);			//Coordinates used for saving and for placement

		InventoryManager.Instance.removeMuchroomFromInventory(DraggableItem.itemBeingDragged.gameObject);
		obj.GetComponent<MuchroomPlant>().planted = true;
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
		Debug.Log("Validate");
        GameObject curTilemapObj = tilemapDict[playerController.layer];			//Gets the tilemap GameObject the player is currently on
		Tilemap curTilemap = curTilemapObj.GetComponent<Tilemap>();				//Gets the tilemap component from ^^^
		TileBase tile = curTilemap.GetTile(gridPos);							//Gets the specific tile that was clicked
		
		if(tile == null)
			return null;
		return curTilemapObj;
    }

    protected override void UpdateLevel()
	{
		if (ItemAmount > 0)
		{
			List<Sprite> sprites = new();
			if (Contains == AttributeValue.Silt)
			{
				_label.text = "Si";
				sprites.Add(_siltLevel1);
				sprites.Add(_siltLevel1_5);
				sprites.Add(_siltLevel2);
				sprites.Add(_siltLevel2_5);
				sprites.Add(_siltLevel3);
			}
			else if (Contains == AttributeValue.Sand)
			{
				_label.text = "Sa";
				sprites.Add(_sandLevel1);
				sprites.Add(_sandLevel1_5);
				sprites.Add(_sandLevel2);
				sprites.Add(_sandLevel2_5);
				sprites.Add(_sandLevel3);
			}
			else if (Contains == AttributeValue.Loam)
			{
				_label.text = "Lo";
				sprites.Add(_loamLevel1);
				sprites.Add(_loamLevel1_5);
				sprites.Add(_loamLevel2);
				sprites.Add(_loamLevel2_5);
				sprites.Add(_loamLevel3);
			}
			else if (Contains == AttributeValue.Clay)
			{
				_label.text = "Cl";
				sprites.Add(_clayLevel1);
				sprites.Add(_clayLevel1_5);
				sprites.Add(_clayLevel2);
				sprites.Add(_clayLevel2_5);
				sprites.Add(_clayLevel3);
			}
			else if (Contains == AttributeValue.Acidic)
			{
				_label.text = "Ac";
				sprites.Add(_acidicLevel1);
				sprites.Add(_acidicLevel1_5);
				sprites.Add(_acidicLevel2);
				sprites.Add(_acidicLevel2_5);
				sprites.Add(_acidicLevel3);
			}
			else if (Contains == AttributeValue.Basic)
			{
				_label.text = "Ba";
				sprites.Add(_basicLevel1);
				sprites.Add(_basicLevel1_5);
				sprites.Add(_basicLevel2);
				sprites.Add(_basicLevel2_5);
				sprites.Add(_basicLevel3);
			}
			else if (Contains == AttributeValue.Neutral)
			{
				_label.text = "Ne";
				sprites.Add(_neutralLevel1);
				sprites.Add(_neutralLevel1_5);
				sprites.Add(_neutralLevel2);
				sprites.Add(_neutralLevel2_5);
				sprites.Add(_neutralLevel3);
			}

			thresh.Sort();
			for (int i = 0; i < thresh.Count; i++)
			{
				if (ItemAmount <= thresh[i])
				{
					_fill.sprite = sprites[i];
					break;
				}
			}
		}
		else
		{
			_fill.sprite = null;
			_label.text = "E";
		}
	}
}
