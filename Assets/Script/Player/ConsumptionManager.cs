using Botography.Notifications;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;


public class ConsumptionManager : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;
    [SerializeField] List<PlantSO> _consumablePlants;
    List<PlantSO> _listOfConsumablePlants => _consumablePlants;
	public static ConsumptionManager Instance { get; private set; }

    private void Awake() 
	{
		if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;	
	}

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        /*foreach (var plant in _listOfConsumablePlants)
        {
            Debug.Log(plant);
        }*/
    }

    public void OnPointerClick(bool destroyGO)
    {
        if (DraggableItem.itemBeingDragged != null)
        {
            PlantSO _draggedItem = ((InventoryDraggable)DraggableItem.itemBeingDragged)._plantSO;
            
            if (_listOfConsumablePlants.Contains(_draggedItem)){
                _spriteRenderer.color = Color.white;
                ((IConsumable)_draggedItem.Prefab.GetComponent<BasePlant>()).ConsumableAbility();
				Notifyer.Instance.Notify($"{_draggedItem.PlantName} consumed");
                SoundManager.Instance.PlaySFX("deselect");
                if (destroyGO)
				    Destroy(DraggableItem.itemBeingDragged.gameObject);
            }
            else
				Notifyer.Instance.Notify("Ouch! Can't consume that!");
        }
        /*
        else
			Notifyer.Instance.Notify("You're clicking me with nothing!");
        */
    }

    public void OnPointerEnter()
    {
		//_spriteRenderer.color = Color.gray;
        if (DraggableItem.itemBeingDragged != null)
        {
			_spriteRenderer.color = Color.gray;
            // I'm making this change so it's more obvious to the player that the player can consume plants
            // Not sure if this is srs friendly
            /*
			PlantSO _draggedItem = ((InventoryDraggable)DraggableItem.itemBeingDragged)._plantSO;
			
            if (_listOfConsumablePlants.Contains(_draggedItem)){
				//Debug.Log("entered");
				_spriteRenderer.color = Color.gray;
            }
            */
        }
    }

    public void OnPointerExit()
    {
		//_spriteRenderer.color = Color.white;
        if (DraggableItem.itemBeingDragged != null)
        {
			_spriteRenderer.color = Color.white;
            // I'm making this change so it's more obvious to the player that the player can consume plants
            // Not sure if this is srs friendly
            /*
			PlantSO _draggedItem = ((InventoryDraggable)DraggableItem.itemBeingDragged)._plantSO;
			
            if (_listOfConsumablePlants.Contains(_draggedItem)){
				//Debug.Log("exited");
				_spriteRenderer.color = Color.white;
            }
            */
        }
    }
}

    
