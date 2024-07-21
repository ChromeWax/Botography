using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StackedSlot : Slot, IPointerEnterHandler, IPointerExitHandler
{
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (DraggableItem.itemBeingDragged != null)
        {
            if (ValidSlot(eventData))
            {
                StackableItem draggableItemStack = (StackableItem) DraggableItem.itemBeingDragged;

                // Slot is empty
                if (transform.childCount == 0)
                {
                    // Simply moves draggable to slot when left clicked
                    if (eventData.button == PointerEventData.InputButton.Left)
                    {
                        DraggableItem.itemBeingDragged.ParentAfterDrag = transform;
                        DraggableItem.itemBeingDragged.DisableDrag();
					    SoundManager.Instance.PlaySFX("deselect");
                    }

                    // When right clicked
                    else if (eventData.button == PointerEventData.InputButton.Right)
                    {
                        // If stacktotal is over 1, subtract 1 from stacktotal and instantiate new stackableitem
                        if (draggableItemStack.GetStackTotal() > 1)
                        {
                            GameObject newStackableItem = Instantiate(draggableItemStack.gameObject);
                            newStackableItem.GetComponent<StackableItem>().ParentAfterDrag = transform;
                            newStackableItem.gameObject.transform.SetParent(transform); 
                            newStackableItem.GetComponent<StackableItem>().SetStackTotal(1);
                            draggableItemStack.ChangeStackTotal(-1);
					        SoundManager.Instance.PlaySFX("deselect");
                        }

                        // In case stack total is only one simply just move
                        else
                        {
                            DraggableItem.itemBeingDragged.ParentAfterDrag = transform;
                            DraggableItem.itemBeingDragged.DisableDrag();
					        SoundManager.Instance.PlaySFX("deselect");
                        }
                    }
                }

                // Slot is not empty
                else
                {
                    StackableItem childItemStack = transform.GetChild(0).GetComponent<StackableItem>();

                    // When left clicked
                    if (eventData.button == PointerEventData.InputButton.Left)
                    {
                        // Simply swaps
                        if (childItemStack.GetStackTotal() == 5 || draggableItemStack.GetStackTotal() == 5)
                        {
							Transform tempDraggingParent;
						
							tempDraggingParent = draggableItemStack.ParentAfterDrag;
                            draggableItemStack.ParentAfterDrag = childItemStack.ParentAfterDrag;
                            draggableItemStack.DisableDrag();
                            childItemStack.EnableDrag();
							childItemStack.ParentAfterDrag = tempDraggingParent;
					        SoundManager.Instance.PlaySFX("select");
                        }

                        // Handles overflow
                        else if (childItemStack.GetStackTotal() + draggableItemStack.GetStackTotal() > 5)
                        {
                            int subtractTotal = (5 - childItemStack.GetStackTotal()) * -1;

                            childItemStack.SetStackTotal(5);
                            draggableItemStack.ChangeStackTotal(subtractTotal);
					        SoundManager.Instance.PlaySFX("deselect");
                        }

                        // Just combines
                        else
                        {
                            childItemStack.ChangeStackTotal(((StackableItem)DraggableItem.itemBeingDragged).GetStackTotal());
                            Destroy(DraggableItem.itemBeingDragged.gameObject);
					        SoundManager.Instance.PlaySFX("deselect");
                        }
                    }

                    // When right clicked
                    else if (eventData.button == PointerEventData.InputButton.Right)
                    {
                        if (childItemStack.GetStackTotal() + 1 <= 5)
                        {
                            childItemStack.ChangeStackTotal(1);
                            draggableItemStack.ChangeStackTotal(-1);
					        SoundManager.Instance.PlaySFX("deselect");
                        }
                    }
                }
            }
            else
            {
				Transform tempDraggingParent;
						
                DraggableItem draggableItemStack = DraggableItem.itemBeingDragged;
                DraggableItem childItemStack = transform.GetChild(0).GetComponent<DraggableItem>();

				tempDraggingParent = draggableItemStack.ParentAfterDrag;
                draggableItemStack.ParentAfterDrag = childItemStack.ParentAfterDrag;
                draggableItemStack.DisableDrag();
                childItemStack.EnableDrag();
				childItemStack.ParentAfterDrag = tempDraggingParent;
				SoundManager.Instance.PlaySFX("deselect");
            }
        }
        else if (DraggableItem.itemBeingDragged == null && transform.childCount > 0)
		{
			transform.GetChild(0).GetComponent<DraggableItem>().EnableDrag();
			SoundManager.Instance.PlaySFX("select");
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
        if (transform.childCount > 0)
        {
            if (transform.GetChild(0).GetComponent<MuchroomPlant>() != null && DraggableItem.itemBeingDragged.gameObject.GetComponent<MuchroomPlant>() != null)
            {
                return transform.GetChild(0).GetComponent<Jar>().ItemAmount == 0 && DraggableItem.itemBeingDragged.gameObject.GetComponent<Jar>().ItemAmount == 0; 
            }
            else if (transform.GetChild(0).GetComponent<Jar>() != null && DraggableItem.itemBeingDragged.gameObject.GetComponent<Jar>() != null 
            && transform.GetChild(0).GetComponent<MuchroomPlant>() == null && DraggableItem.itemBeingDragged.gameObject.GetComponent<MuchroomPlant>() == null)
            {
                return transform.GetChild(0).GetComponent<Jar>().ItemAmount == 0 && DraggableItem.itemBeingDragged.gameObject.GetComponent<Jar>().ItemAmount == 0; 
            }
            else if (transform.GetChild(0).GetComponent<InventoryDraggable>() != null && DraggableItem.itemBeingDragged.gameObject.GetComponent<InventoryDraggable>() != null)
            {
                string _name = transform.GetChild(0).GetComponent<InventoryDraggable>()._plantSO.PlantName;
                return DraggableItem.itemBeingDragged.gameObject.GetComponent<InventoryDraggable>()._plantSO.PlantName == _name;
            }
            else
                return false;
        }
        else 
            return true;
    }
}
