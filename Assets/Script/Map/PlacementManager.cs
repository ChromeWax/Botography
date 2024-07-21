using Botography.Notifications;
using Botography.OverworldInteraction;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

public class PlacementManager : OWIManager
{
    public static PlacementManager Instance { get; private set; }

	[SerializeField] 
	private GameObject _invManager;
	[SerializeField] 
	private List<Vector3Int> _objectCoordinates;
	[SerializeField] 
	private List<GameObject> _objectAtCoordinates;
	private Dictionary<Vector3Int, GameObject> _objCoordinatesDict;
	[SerializeField] 
	private List<GameObject> _tilemapObjects;
	private Dictionary<int, GameObject> _tilemapObjsDict;

	[SerializeField] 
    private Vector2 mouseMovement;
	
    [SerializeField] private Grid grid;

	public static readonly Dictionary<int, string> ElevationToSortingLayerDictionary = new Dictionary<int, string>() {
        { 7, "Elevation 1" },
		{ 8, "Elevation 2" },
		{ 9, "Elevation 3" }
	};

   	private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

	void Start()
	{
		_tilemapObjsDict = new Dictionary<int, GameObject>();
		for(int i=0; i<_tilemapObjects.Count; i++)
		{
			_tilemapObjsDict.Add(_tilemapObjects[i].layer, _tilemapObjects[i]);
		}
		_objCoordinatesDict = new Dictionary<Vector3Int, GameObject>();
		for(int i=0; i<_objectCoordinates.Count; i++)
		{
			_objCoordinatesDict.Add(_objectCoordinates[i], _objectAtCoordinates[i]);
		}

		InputHandler.Instance.OnCursorPressed += OnCursorPressed;
		InputHandler.Instance.OnCursorLeftPressed += OnCursorLeftPressed;
		InputHandler.Instance.OnCursorRightPressed += OnCursorRightPressed;
		isInitialized = true;
	}
	
	private void placeObject(Vector3Int gridPositionInt)		//Consider renaming function
	{
		PlantSO plantSO;
		if (DraggableItem.itemBeingDragged.gameObject.GetComponent<InventoryDraggable>() != null)
			plantSO = ((InventoryDraggable) DraggableItem.itemBeingDragged)._plantSO;
		else if (DraggableItem.itemBeingDragged.gameObject.GetComponent<MuchroomPlant>() != null)
			plantSO = DraggableItem.itemBeingDragged.gameObject.GetComponent<MuchroomPlant>().plantSO;
		else
			plantSO = null;
		GameObject selectedPlant = plantSO.Prefab;
		string plantName = plantSO.PlantName;
//Debug.Log("This is a " + verifyValidPosition(gridPositionInt, selectedPlant) + " position.");
		if (verifyValidPosition(gridPositionInt, selectedPlant))
		{
			DraggableItem.itemBeingDragged = null;
			Notifyer.Instance.Notify(plantName + " Plant Placed!");
			SoundManager.Instance.PlaySFX("plant placed");
			CursorManager.Instance.SetCursorType(CursorType.normal);
		}
	}
	
	private bool pickupObject(Vector3Int gridPositionInt){
		if(outsideReach(gridPositionInt))
		{
			Notifyer.Instance.Notify("Pickup spot too far from me");
			return false;
		}
		PlantSO plantSO = null;
		GameObject obj;
		if(_objCoordinatesDict.ContainsKey(gridPositionInt))
		{		//primary way of picking up
			obj = _objCoordinatesDict[gridPositionInt];
			if (obj.GetComponent<BasePlant>() != null)
				plantSO = obj.GetComponent<BasePlant>().getPlantSO();
			else if (obj.GetComponent<MuchroomPlant>() != null)
				plantSO = obj.GetComponent<MuchroomPlant>().plantSO;
			_objCoordinatesDict.Remove(gridPositionInt);
			SoundManager.Instance.PlaySFX("plant placed");
		}
		else
			return false;
		/*{
			int layer = playerController.layer;
			obj = coordinatesOccupied(gridPositionInt, layer);
			if(obj != false && plantSO == null){			//secondary way of picking up (objects picked up this way would not have been saved)
				if (obj.GetComponent<BasePlant>() != null)
					plantSO = obj.GetComponent<BasePlant>().getPlantSO();
				else if (obj.GetComponent<MuchroomPlant>() != null)
					plantSO = obj.GetComponent<MuchroomPlant>().plantSO;
			}
		}*/
		if(plantSO == null)
			return false;
		if (obj.GetComponent<IPlaceable>() != null)
		{
			var plantScript = obj.GetComponent<IPlaceable>();
			plantScript.Pickup(gridPositionInt, _objCoordinatesDict, obj);
		}
		else if (obj.GetComponent<MuchroomPlant>() != null)
		{
			var plantScript = obj.GetComponent<MuchroomPlant>();
			plantScript.Pickup(gridPositionInt, _objCoordinatesDict, obj);
		}
		else
			return false;
		/*
		bool b = _invManager.GetComponent<InventorySystem>().addItemToInv(plantSO, _defaultDraggable);
		if(b){
//Debug.Log("Placement Manager: " + _objCoordinatesDict.Count);
//Debug.Log("Placement Manager: " + _objCoordinatesDict[gridPositionInt]);
			plantScript.Pickup(gridPositionInt, _objCoordinatesDict);
			Destroy(obj);
		}
		*/
		return true;
	}

	private void OnEnable()
	{
		if (isInitialized)
		{
//Debug.Log("Placement Manager: OnEnable");
			InputHandler.Instance.OnCursorPressed += OnCursorPressed;
			InputHandler.Instance.OnCursorLeftPressed += OnCursorLeftPressed;
			InputHandler.Instance.OnCursorRightPressed += OnCursorRightPressed;
		}
	}

	private void OnDisable()
	{
//Debug.Log("Placement Manager: OnDisable");
		InputHandler.Instance.OnCursorPressed -= OnCursorPressed;
		InputHandler.Instance.OnCursorLeftPressed -= OnCursorLeftPressed;
		InputHandler.Instance.OnCursorRightPressed -= OnCursorRightPressed;
	}

	private void OnCursorLeftPressed()
	{
    	bool isPointerOverUI = EventSystem.current.IsPointerOverGameObject();
		if (!isPointerOverUI)
		{
			Vector3Int gridPositionInt = getTileClicked();
//Debug.Log("Placement Manager: " + DraggableItem.itemBeingDragged != null);
			if (DraggableItem.itemBeingDragged != null)
			{
				try 
				{
					placeObject(gridPositionInt);
				}
				catch 
				{
					//Debug.Log("Placement Manager: ERROR ERROR ERROR");
				}
			}
		}
		//else
			//Debug.Log("Placement Manager: Over UI");
	}

	private void OnCursorRightPressed()
	{
		/*
    	bool isPointerOverUI = EventSystem.current.IsPointerOverGameObject();
		if (!isPointerOverUI)
		{
			Vector3Int gridPositionInt = getTileClicked();
			if (DraggableItem.itemBeingDragged == null)
			{
				pickupObject(gridPositionInt);
			}
		}
		else
			Debug.Log("Placement Manager: Over UI");
		*/

		if (IsPointerOverPlant())
		{
			Vector3Int gridPositionInt = getTileClicked();
			if (DraggableItem.itemBeingDragged == null)
				pickupObject(gridPositionInt);
		}
		else
			Debug.Log("Placement Manager: Over UI");


	}

	private bool IsPointerOverPlant() 
	{
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(mouseMovement.x, mouseMovement.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
		if (results.Count > 0)
			if (results[0].gameObject.tag == "Plant")
				return true;	
        return results.Count > 0;
	}
	
	private void OnCursorPressed(Vector2 movement)
    {
        mouseMovement = movement;
    }
	
//-------------------------------------------HELPERS-------------------------------------------//
	
	private bool verifyValidPosition(Vector3Int gridPositionInt, GameObject selectedPlant){
		if(outsideReach(gridPositionInt))
		{
			Notifyer.Instance.Notify("Placement too far from me");
			return false;
		}
		int layer = playerController.layer;

		Collider2D col = selectedPlant.GetComponent<Collider2D>();
		GameObject occupier1 = coordinatesOccupied(gridPositionInt, 7, col);
		GameObject occupier2 = coordinatesOccupied(gridPositionInt, 8, col);
		GameObject occupier3 = coordinatesOccupied(gridPositionInt, 9, col);
		if(occupier1 != null || occupier2 != null || occupier3 != null){			//checks that nothing with a collider is at those coordinates
			if (occupier1 == playerController || occupier2 == playerController || occupier3 == playerController)
			{
				// This is now being handled by InvisibleBoxOverPlayer script
				//ConsumptionManager.Instance.OnPointerClick(null);
			}
			else
				notifyer.Notify("Space Occupied");
			return false;
		}
		// Handles if there is a anchordraggable, so that it can be placed on the top most layer OLD
		/*
		if (DraggableItem.itemBeingDragged.gameObject.GetComponent<AnchorDraggable>() != null)	
		{
			try
			{
				occupier = coordinatesOccupied(gridPositionInt, layer + 1, col);
				if(occupier != null){			//checks that nothing with a collider is at those coordinates
					if (occupier == playerController)
					{
						// This is now being handled by InvisibleBoxOverPlayer script
						//ConsumptionManager.Instance.OnPointerClick(null);
					}
					else
						notifyer.Notify("Space Occupied");
					return false;
				}
			}
			catch{}
			try
			{
				occupier = coordinatesOccupied(gridPositionInt, layer - 1, col);
				if(occupier != null){			//checks that nothing with a collider is at those coordinates
					if (occupier == playerController)
					{
						// This is now being handled by InvisibleBoxOverPlayer script
						//ConsumptionManager.Instance.OnPointerClick(null);
					}
					else
						notifyer.Notify("Space Occupied");
					return false;
				}
			}
			catch{}
		}
		*/

//Debug.Log("Placement Manager: " + occupier);
		
		if(_objCoordinatesDict.ContainsKey(gridPositionInt)){		//checks that nothing has already been saved to those coordinates
			notifyer.Notify("Space Occupied");
			return false;
		}

		var plantScript = selectedPlant.GetComponent<IPlaceable>();
		if(plantScript != null){
			Vector3Int adjGPI = new Vector3Int(gridPositionInt.x, gridPositionInt.y, gridPositionInt.z);		//adjusts GPI to reflect valid grid tiles
			GameObject curTilemapObj = plantScript.ValidatePlacement(_tilemapObjsDict, playerController, adjGPI);
//Debug.Log(curTilemapObj);
			if(curTilemapObj != null){
				// New code to handle new lighting
				TilemapRenderer tilemapRenderer = curTilemapObj.GetComponent<TilemapRenderer>();
				string originalSortingLayer = tilemapRenderer.sortingLayerName;
				tilemapRenderer.sortingLayerName = ElevationToSortingLayerDictionary[tilemapRenderer.gameObject.layer];
				plantScript.instantiateObject(gridPositionInt, selectedPlant, tilemapRenderer, _objCoordinatesDict);
				tilemapRenderer.sortingLayerName = originalSortingLayer;

				// Orig
				//plantScript.instantiateObject(gridPositionInt, selectedPlant, curTilemapObj.GetComponent<TilemapRenderer>(), _objCoordinatesDict);
				if (DraggableItem.itemBeingDragged != null)
				{
					Destroy(DraggableItem.itemBeingDragged.gameObject);
					DraggableItem.itemBeingDragged = null;
				}
//Debug.Log("Placement Manager: " + _objCoordinatesDict[gridPositionInt]);
				return true;
			}
		}
		return false;
	}

    public bool VerifyValidPositionWithoutPlacing(Vector3Int gridPositionInt, GameObject selectedPlant)
    {
        //if (outsideReach(gridPositionInt))
        //{
            //Notifyer.Instance.Notify("Placement too far from me");
            //return false;
        //}

        Collider2D col = selectedPlant.GetComponent<Collider2D>();
        GameObject occupier1 = coordinatesOccupied(gridPositionInt, 7, col);
        GameObject occupier2 = coordinatesOccupied(gridPositionInt, 8, col);
        GameObject occupier3 = coordinatesOccupied(gridPositionInt, 9, col);
        if (occupier1 != null || occupier2 != null || occupier3 != null)
        {           //checks that nothing with a collider is at those coordinates
            if (occupier1 == playerController || occupier2 == playerController || occupier3 == playerController)
            {
                // This is now being handled by InvisibleBoxOverPlayer script
                //ConsumptionManager.Instance.OnPointerClick(null);
            }
            else
                notifyer.Notify("Space Occupied");
            return false;
        }
        // Handles if there is a anchordraggable, so that it can be placed on the top most layer OLD

        //Debug.Log("Placement Manager: " + occupier);

        if (_objCoordinatesDict.ContainsKey(gridPositionInt))
        {       //checks that nothing has already been saved to those coordinates
            notifyer.Notify("Space Occupied");
            return false;
        }

        var plantScript = selectedPlant.GetComponent<IPlaceable>();
        if (plantScript != null)
        {
            Vector3Int adjGPI = new Vector3Int(gridPositionInt.x, gridPositionInt.y, gridPositionInt.z);        //adjusts GPI to reflect valid grid tiles
            GameObject curTilemapObj = plantScript.ValidatePlacement(_tilemapObjsDict, playerController, adjGPI);
            //Debug.Log(curTilemapObj);
            if (curTilemapObj != null)
            {

                // Orig
                //plantScript.instantiateObject(gridPositionInt, selectedPlant, curTilemapObj.GetComponent<TilemapRenderer>(), _objCoordinatesDict);
                //Debug.Log("Placement Manager: " + _objCoordinatesDict[gridPositionInt]);
                return true;
            }
        }
        return false;
    }

    public bool outsideReach(Vector3Int gridPositionInt){
		Vector2 gridPosition = new Vector2(gridPositionInt.x + 0.5f, gridPositionInt.y + 0.5f);
		Vector2 closestPoint = playerController.GetComponent<BoxCollider2D>().ClosestPoint(gridPosition);
		if (DraggableItem.itemBeingDragged != null && DraggableItem.itemBeingDragged.gameObject.GetComponent<AnchorDraggable>() != null)	
			return Vector2.Distance(gridPosition, closestPoint) > interactionRadius * 5;
		else
			return Vector2.Distance(gridPosition, closestPoint) > interactionRadius;
	}
	
	public GameObject coordinatesOccupied(Vector3Int gridPositionInt, int layer, Collider2D col){
		Vector2 max = col.bounds.max - col.bounds.center;
		Vector2 min = col.bounds.min - col.bounds.center;
		// .5 offset to center plants within grid cell, .3 margin
		Vector2 topRight = new Vector2(gridPositionInt.x + max.x + 0.5f, gridPositionInt.y + max.y + 0.5f);
		Vector2 bottomLeft = new Vector2(gridPositionInt.x + min.x + 0.5f, gridPositionInt.y + min.y + 0.5f);
		//Vector2 topRight = new Vector2(gridPositionInt.x-.1f, gridPositionInt.y-.1f);		//Gives a .1 margin where a collider can be and still allow plant placement
		//Vector2 bottomLeft = new Vector2(gridPositionInt.x-.9f, gridPositionInt.y-.9f);
		Collider2D hitCollider = Physics2D.OverlapArea(topRight, bottomLeft, LayerMask.GetMask(LayerMask.LayerToName(layer)));

		//if (hitCollider == null || hitCollider.isTrigger || hitCollider.gameObject.GetComponent<GrowcapPlant>() != null)
		if (hitCollider == null || hitCollider.tag == "Plant" || hitCollider.gameObject.GetComponent<GrowcapPlant>() != null)
			return null;
		return hitCollider.gameObject;
	}
	
	public Vector3Int getTileClicked(){
		// Needs replaced with input system
		Vector3 gridPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseMovement.x, mouseMovement.y, 10));
        Vector3Int cellPosition = grid.WorldToCell(gridPosition);
        Vector3 centerCell = grid.CellToWorld(cellPosition);
		//Debug.Log(Vector3Int.FloorToInt(centerCell));
		//return Vector3Int.FloorToInt(centerCell) + new Vector3Int(1, 0, 0);
		return Vector3Int.FloorToInt(centerCell);
		//return getTile(gridPosition);
	}
	
	public Dictionary<Vector3Int, GameObject> getObjCoordinates()
	{
		return _objCoordinatesDict;
	}
	
	public void setObjCoordinates(Dictionary<Vector3Int, GameObject> objCoords)
	{
		_objCoordinatesDict = objCoords;
	}
}
