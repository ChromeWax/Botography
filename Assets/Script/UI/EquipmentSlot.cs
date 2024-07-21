using System.Collections;
using System.Collections.Generic;
using Botography.Notifications;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : Slot, IPointerEnterHandler, IPointerExitHandler
{	
	public BodyPart bodyPart;
	
	[SerializeField] GameObject backGroundImg;

	public override void OnPointerDown(PointerEventData eventData)
	{
   		if (DraggableItem.itemBeingDragged != null)
        {
            if (transform.childCount < 2 && ValidSlot(eventData))
            {
				if (bodyPart == DraggableItem.itemBeingDragged.GetComponent<InventoryDraggable>()._plantSO.Prefab.GetComponent<EquipmentPlant>().bodyPart)
				{
					if (((StackableItem) DraggableItem.itemBeingDragged).GetStackTotal() > 1)
					{
						StackableItem draggableItemStack = (StackableItem) DraggableItem.itemBeingDragged;

						GameObject newStackableItem = Instantiate(draggableItemStack.gameObject);
						newStackableItem.GetComponent<StackableItem>().ParentAfterDrag = transform;
						newStackableItem.gameObject.transform.SetParent(transform); 
						newStackableItem.GetComponent<StackableItem>().SetStackTotal(1);
						draggableItemStack.ChangeStackTotal(-1);
						SoundManager.Instance.PlaySFX("deselect");
					}
					else
					{
						DraggableItem.itemBeingDragged.ParentAfterDrag = transform;
						DraggableItem.itemBeingDragged.DisableDrag();
						SoundManager.Instance.PlaySFX("deselect");
					}
				}
				else
				{
					Notifyer.Instance.Notify("Plant is equippable, but for " + DraggableItem.itemBeingDragged.GetComponent<InventoryDraggable>()._plantSO.Prefab.GetComponent<EquipmentPlant>().bodyPart.ToString());
				}
            }
			else
			{
				DraggableItem.itemBeingDragged.DisableDrag();
				Notifyer.Instance.Notify("Object is not equippable");
				SoundManager.Instance.PlaySFX("deselect");
			}
        }
   		else if (DraggableItem.itemBeingDragged == null && transform.childCount > 1)
		{
			transform.GetChild(1).GetComponent<DraggableItem>().EnableDrag();
			SoundManager.Instance.PlaySFX("select");
		}
		else
		{
			return;
		}
		
		if (transform.childCount > 1)
		{
			transform.GetChild(1).GetComponent<InventoryDraggable>()._plantSO.Prefab.GetComponent<EquipmentPlant>().Equipped();
			backGroundImg.SetActive(false);
		}
	}

	public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        StackableItem.isOverSlot = true;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        StackableItem.isOverSlot = false;
    }

	protected override bool ValidSlot(PointerEventData eventData)
	{
		return DraggableItem.itemBeingDragged.GetComponent<InventoryDraggable>()._plantSO.PlantUsageType.HasFlag(UsageType.equippable);
	}
	
	public override void ItemRemoved()
	{
		transform.GetChild(1).GetComponent<InventoryDraggable>()._plantSO.Prefab.GetComponent<EquipmentPlant>().Unequipped();
		backGroundImg.SetActive(true);
	}
	
	public override void Update()
	{
		base.Update();
		
		if (transform.childCount == 1)
			backGroundImg.SetActive(true);
		else
			backGroundImg.SetActive(false);
	}
}
