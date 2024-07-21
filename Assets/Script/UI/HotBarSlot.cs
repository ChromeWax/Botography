using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class HotBarSlot : Slot
{
    [SerializeField] private GameObject handImage;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (ValidSlot(eventData) == false) return;

        if (DraggableItem.itemBeingDragged != null)
        {
            // Slot is empty
            if (transform.childCount == 1)
            {
                DraggableItem.itemBeingDragged.ParentAfterDrag = transform;
                DraggableItem.itemBeingDragged.DisableDrag();
				SoundManager.Instance.PlaySFX("deselect");
            }

            // Slot is not empty
            else if ((transform.GetChild(1).GetComponent<MuchroomPlant>() != null && DraggableItem.itemBeingDragged.GetComponent<MuchroomPlant>() != null) || 
            (transform.GetChild(1).GetComponent<InventoryDraggable>() != null && DraggableItem.itemBeingDragged.GetComponent<InventoryDraggable>() != null && 
            transform.GetChild(1).GetComponent<InventoryDraggable>()._plantSO.PlantName == DraggableItem.itemBeingDragged.GetComponent<InventoryDraggable>()._plantSO.PlantName))
            {
                StackableItem childItemStack = transform.GetChild(1).GetComponent<StackableItem>();
                childItemStack.ChangeStackTotal(((StackableItem)DraggableItem.itemBeingDragged).GetStackTotal());
                Destroy(DraggableItem.itemBeingDragged.gameObject);
				SoundManager.Instance.PlaySFX("deselect");
            }
        }
        else if (DraggableItem.itemBeingDragged == null && transform.childCount > 1)
		{
            GameObject newDraggable = Instantiate(transform.GetChild(1).gameObject);
            newDraggable.GetComponent<StackableItem>().SetStackTotal(1);
            newDraggable.gameObject.transform.SetParent(transform.GetComponentInParent<Canvas>().transform); 
            newDraggable.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
            newDraggable.GetComponent<StackableItem>().RemoveRightClickAbility();
            newDraggable.GetComponent<StackableItem>().EnableDrag();
            transform.GetChild(1).gameObject.GetComponent<StackableItem>().ChangeStackTotal(-1);
			SoundManager.Instance.PlaySFX("select");
		}
    }

    public override void Update()
    {
        base.Update();

        if (transform.childCount == 1)
            handImage.SetActive(true);
        else
            handImage.SetActive(false);
    }

    protected override bool ValidSlot(PointerEventData eventData)
	{
        if (DraggableItem.itemBeingDragged != null)
        {
            if (DraggableItem.itemBeingDragged.GetComponent<AnchorDraggable>() != null)
                return false;
        }
        
		return true;
	}

}
