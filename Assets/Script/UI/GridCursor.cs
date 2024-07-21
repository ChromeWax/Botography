using System;
using System.Collections;
using System.Collections.Generic;
using Botography.Map;
using Botography.Player;
using UnityEngine;

public class GridCursor : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float interactionRadius;
    [SerializeField] private RegionSO _coldRegion;

    private Grid grid;
    private Vector2 mouseMovement;
	private bool isInitialized = false;

    void Start() 
    {
        grid = GetComponent<Grid>();

		InputHandler.Instance.OnCursorPressed += OnCursorPressed;
        isInitialized = true;
    }

	private void OnCursorPressed(Vector2 movement)
    {
        mouseMovement = movement;
    }

    // Update is called once per frame
    void Update()
    {
        if (DraggableItem.itemBeingDragged != null && !Meltable.onBreakableObject)    
        {
            try 
            {
                //Debug.Log(PlacementManager.Instance.getTileClicked() + " is the tile.");
                InventoryDraggable inventoryDraggable = (InventoryDraggable) DraggableItem.itemBeingDragged;
                if (inventoryDraggable != null)
                {
                    Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseMovement.x, mouseMovement.y, 10));
                    Vector3Int cellPosition = grid.WorldToCell(worldPosition);
                    Vector3 centerCell = grid.GetCellCenterWorld(cellPosition);
                    GameObject occupier1 = PlacementManager.Instance.coordinatesOccupied(Vector3Int.FloorToInt(grid.CellToWorld(cellPosition)), LayerMask.NameToLayer("Elevation 1"), inventoryDraggable._plantSO.Prefab.GetComponent<Collider2D>());
                    GameObject occupier2 = PlacementManager.Instance.coordinatesOccupied(Vector3Int.FloorToInt(grid.CellToWorld(cellPosition)), LayerMask.NameToLayer("Elevation 2"), inventoryDraggable._plantSO.Prefab.GetComponent<Collider2D>());
                    GameObject occupier3 = PlacementManager.Instance.coordinatesOccupied(Vector3Int.FloorToInt(grid.CellToWorld(cellPosition)), LayerMask.NameToLayer("Elevation 3"), inventoryDraggable._plantSO.Prefab.GetComponent<Collider2D>());

                    if (occupier1 == null && occupier2 == null && occupier3 == null && PlacementManager.Instance.VerifyValidPositionWithoutPlacing(Vector3Int.FloorToInt(grid.CellToWorld(cellPosition)), inventoryDraggable._plantSO.Prefab))
                    {
                        //Debug.Log(Vector3Int.FloorToInt(grid.CellToWorld(cellPosition)) + " is the tile.");
                        spriteRenderer.transform.position = centerCell;
                        spriteRenderer.enabled = true;

                        if (PlacementManager.Instance.outsideReach(Vector3Int.FloorToInt(grid.CellToWorld(cellPosition))))
                            spriteRenderer.color = Color.gray;
                        else
                        {
                            if (PlayerManager.Instance.GetCurrentRegion() == _coldRegion)
                                spriteRenderer.color = new Color32(17, 46, 115, 255);
                            else
                                spriteRenderer.color = Color.white;
                        }
                    }
                    else
                        spriteRenderer.enabled = false;
                }
            }
            catch (Exception e)
            {

            }
        }
        else
            spriteRenderer.enabled = false;
    }

    private void OnEnable()
	{
		if (isInitialized)
			InputHandler.Instance.OnCursorPressed += OnCursorPressed;
	}

	private void OnDisable()
	{
		InputHandler.Instance.OnCursorPressed -= OnCursorPressed;
	}
}
