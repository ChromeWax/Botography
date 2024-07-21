using System.Collections;
using Botography.OverworldInteraction;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static InventoryConstants;
using Botography.Player;
using Botography.Player.StatusEffects;

public class BeanstalkAnchor : BasePlant, IInteractivePlaceable
{
    private BeanstalkPlant plant;
    private SpriteRenderer _spriteRenderer;
    private bool moving;
    public Vector3Int gridPosition;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void instantiateObject(Vector3Int gridPositionInt, GameObject selectedObj, TilemapRenderer curRenderer, Dictionary<Vector3Int, GameObject> objCoordinatesDict)
    {
        GameObject obj = Instantiate(selectedObj);
        obj.GetComponent<BeanstalkAnchor>().StartCoroutine(PlaceAnimation());
		obj.GetComponent<SpriteRenderer>().sortingLayerID = curRenderer.sortingLayerID;
		obj.layer = curRenderer.gameObject.layer;
		float xOff = (Random.Range(-.15f, .15f));				//Adds some variation to the placement of the plant (makes it look less griddy)
		float yOff = (Random.Range(-.15f, .15f));
		Vector3 position = OWIManager.V3ItoV3(gridPositionInt);
		obj.transform.position = position;
		objCoordinatesDict.Add(gridPositionInt, obj);			//Coordinates used for saving and for placement
        if (DraggableItem.itemBeingDragged)
        {
            Destroy(DraggableItem.itemBeingDragged.gameObject);
            DraggableItem.itemBeingDragged = null;
        }

        BeanstalkAnchor anchor = obj.GetComponent<BeanstalkAnchor>();
        anchor.plant = BeanstalkPlant.beanstalkBeingAnchored;
        anchor.plant.StopAnchor(anchor);
        anchor.gridPosition = gridPositionInt;
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

    private void Update()
    {
        if (moving)
        {
            Vector3 playerPos = PlayerManager.Instance.GetCurrentPlayerPosition();
            Vector3 newPos = Vector3.MoveTowards(playerPos, plant.transform.position, 2f * Time.deltaTime);
            if (gameObject.layer == plant.gameObject.layer)
                PlayerManager.Instance.SetPlayerPosition(newPos, 0, gameObject.GetComponent<SpriteRenderer>().sortingLayerName);
            else
                PlayerManager.Instance.SetPlayerPosition(newPos, 0, "UI");
            if (Mathf.Abs((newPos - plant.transform.position).magnitude) < 0.1f)
            {
                // Handles animation
                moving = false;
                PlayerManager.Instance.gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
                PlayerManager.Instance.gameObject.GetComponent<Animator>().SetFloat("Speed", 0f);

                PlayerStateMachine.Instance.isClimbing = false;
                PlayerStateMachine.Instance.BindControls();
                PlayerManager.Instance.SetPlayerPhysics(true);
                PlayerManager.Instance.SetLayer(plant.gameObject.layer, plant.GetAnchorLayer());
                StatusEffectsHandler.Instance.StatusEffectsEnabled = true; 
                PlayerManager.Instance.ToggleShadow(true);
                PlayerManager.Instance.ToggleOcclusion(true);
            }
        }
    }

    public void Pickup(Vector3Int gridPositionInt, Dictionary<Vector3Int, GameObject> objCoordinatesDict, GameObject objectToDestroy)
    {
        throw new System.NotImplementedException();
    }

    public void PlaceableAbility()
    {
        PlayerStateMachine.Instance.UnbindControls();
        PlayerManager.Instance.SetPlayerPhysics(false);
        if (gameObject.layer != plant.gameObject.layer)
            PlayerManager.Instance.SetPlayerPosition(transform.position, 0, "UI");
        else
            PlayerManager.Instance.SetPlayerPosition(transform.position, 0, gameObject.GetComponent<SpriteRenderer>().sortingLayerName);
        PlayerStateMachine.Instance.isClimbing = true;

        // Handles animation
        Vector3 position = plant.transform.position;
        Vector3 directionVector = position - transform.position;
        float angle = Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg - 90;
        if (plant.gameObject.GetComponent<SpriteRenderer>().sortingLayerID == gameObject.GetComponent<SpriteRenderer>().sortingLayerID)
        {
            PlayerManager.Instance.gameObject.GetComponent<Animator>().Play("Movement");
            PlayerManager.Instance.gameObject.GetComponent<Animator>().SetFloat("Speed", 2f);
            Vector2 direction = new Vector2(-Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
            PlayerManager.Instance.gameObject.GetComponent<Animator>().SetFloat("Horizontal", direction.x);
            PlayerManager.Instance.gameObject.GetComponent<Animator>().SetFloat("Vertical", direction.y);
        }
        else if (gameObject.layer > plant.gameObject.layer)
        {
            PlayerManager.Instance.gameObject.GetComponent<Animator>().Play("Player_Climb");
            if (angle > 90 || angle < -90)
                PlayerManager.Instance.gameObject.transform.eulerAngles = new Vector3(0, 0, angle + 180);
            else
                PlayerManager.Instance.gameObject.transform.eulerAngles = new Vector3(0, 0, angle);
        }
        else
        {
            PlayerManager.Instance.gameObject.GetComponent<Animator>().Play("Player_Climb");
            if (angle > 90 || angle < -90)
                PlayerManager.Instance.gameObject.transform.eulerAngles = new Vector3(0, 0, angle);
            else
                PlayerManager.Instance.gameObject.transform.eulerAngles = new Vector3(0, 0, angle + 180);
        }
        moving = true;
        StatusEffectsHandler.Instance.StatusEffectsEnabled = false; 
        PlayerManager.Instance.ToggleShadow(false);
        PlayerManager.Instance.ToggleOcclusion(false);
    }

    public string GetAnchorLayer()
    {
        return _spriteRenderer.sortingLayerName;
    }

    public GameObject ValidatePlacement(Dictionary<int, GameObject> tilemapDict, GameObject playerController, Vector3Int gridPos)
    {
        GameObject curTilemapObj;			
		Tilemap curTilemap;				
		TileBase tile;

        try
        {
            curTilemapObj = tilemapDict[7];			
		    curTilemap = curTilemapObj.GetComponent<Tilemap>();				
		    tile = curTilemap.GetTile(gridPos);					
		    if (tile != null)
            {
		        return curTilemapObj;
            }
        }
        catch{}
        try
        {
            curTilemapObj = tilemapDict[8];			
		    curTilemap = curTilemapObj.GetComponent<Tilemap>();				
		    tile = curTilemap.GetTile(gridPos);					
		    if (tile != null)
            {
		        return curTilemapObj;
            }
        }
        catch{}

        curTilemapObj = tilemapDict[9];			
		curTilemap = curTilemapObj.GetComponent<Tilemap>();				
		tile = curTilemap.GetTile(gridPos);					
		
		if (tile == null)
        {
			return null;
        }
		return curTilemapObj;
    }
}
