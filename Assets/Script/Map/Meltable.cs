using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Botography.Notifications;
using Botography.Player.Dialogue;

public class Meltable : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] GameObject _meltObj;
	[SerializeField] PlantSO pitcherSO;
    public static bool onBreakableObject = false;
	
	private void Start()
    {
        /*
        _spriteRenderer = GetComponent<SpriteRenderer>();
        */
    }
	
    public void OnPointerEnter(PointerEventData eventData)
    {
        /*
        if (DraggableItem.itemBeingDragged != null && ((InventoryDraggable)DraggableItem.itemBeingDragged)._plantSO == pitcherSO)
        {
            _spriteRenderer.color = Color.gray;
        }
        */
        _spriteRenderer.color = Color.gray;
        onBreakableObject = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        /*
        if (DraggableItem.itemBeingDragged != null && ((InventoryDraggable)DraggableItem.itemBeingDragged)._plantSO == pitcherSO)
        {
            _spriteRenderer.color = Color.white;
        }
        */
        _spriteRenderer.color = Color.white;
        onBreakableObject = false;
    }
	
	public void OnPointerClick(PointerEventData eventData)
	{
		if (DraggableItem.itemBeingDragged != null && ((InventoryDraggable)DraggableItem.itemBeingDragged)._plantSO == pitcherSO)
		{
			PlantSO _draggedItem = ((InventoryDraggable)DraggableItem.itemBeingDragged)._plantSO;
			
			_spriteRenderer.color = Color.white;
			_draggedItem.Prefab.GetComponent<PitcherPlant>().ConsumableAbility();
			_meltObj.GetComponent<MeltReference>().rmMeltable(_spriteRenderer.gameObject);
			Destroy(DraggableItem.itemBeingDragged.gameObject);
			Destroy(_spriteRenderer.gameObject);
			Notifyer.Instance.Notify($"{_draggedItem.PlantName} consumed");
            SoundManager.Instance.PlaySFX("melt");
            onBreakableObject = false;
		}
        else
        {
			//Notifyer.Instance.Notify("I can't break it with my fist. Maybe I can make something that can break it");
            DialoguePlayer.Instance.PlayConvo("Meltable");
        }
	}	
}
