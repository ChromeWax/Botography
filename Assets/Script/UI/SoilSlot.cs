using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoilSlot : Slot
{
	protected override bool ValidSlot(PointerEventData eventData)
	{
		AttributeValue? temp = DraggableItem.itemBeingDragged.GetComponentInChildren<Jar>().Contains;
		return temp == AttributeValue.Silt || temp == AttributeValue.Sand || temp == AttributeValue.Loam || temp == AttributeValue.Clay;
	}

	public override void OnPointerDown(PointerEventData eventData)
    {
		if (DraggableItem.itemBeingDragged != null)
        {
			if (ValidSlot(eventData))
			{
				DraggableItem inSlot = GetComponentInChildren<DraggableItem>();
				Transform oldParent = DraggableItem.itemBeingDragged.ParentAfterDrag;
				DraggableItem.itemBeingDragged.ParentAfterDrag = transform;
				DraggableItem.itemBeingDragged.DisableDrag();
				if (inSlot != null)
				{
					inSlot.EnableDrag();
					inSlot.ParentAfterDrag = oldParent;
					SoundManager.Instance.PlaySFX("deselect");
				}
				else
					SoundManager.Instance.PlaySFX("soil collected");
			}
        }
		else if (DraggableItem.itemBeingDragged == null && transform.childCount > 0)
		{
			transform.GetChild(0).GetComponent<DraggableItem>().EnableDrag();
			SoundManager.Instance.PlaySFX("deselect");
		}
		else
		{
			return;
		}
    }
}
