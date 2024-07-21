using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandSlot : Slot, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject handImage;

	protected override bool ValidSlot(PointerEventData eventData)
	{
		GameObject draggableItem = DraggableItem.itemBeingDragged.gameObject;
		return draggableItem.GetComponent<MuchroomPlant>() != null || draggableItem.GetComponent<InventoryDraggable>()._plantSO.PlantUsageType.HasFlag(UsageType.consumable) || draggableItem.GetComponent<InventoryDraggable>()._plantSO.PlantUsageType.HasFlag(UsageType.placeable);
	}

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (DraggableItem.itemBeingDragged != null)
        {
            if (ValidSlot(eventData))
            {
                StackableItem draggableItemStack = (StackableItem) DraggableItem.itemBeingDragged;

                // Slot is empty
                if (transform.childCount == 1)
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
                    StackableItem childItemStack = transform.GetChild(1).GetComponent<StackableItem>();
                    InventoryDraggable potentialChildPlantDraggable; 
                    transform.GetChild(1).TryGetComponent<InventoryDraggable>(out potentialChildPlantDraggable);
                    InventoryDraggable potentialPlantDraggable;
                    DraggableItem.itemBeingDragged.TryGetComponent<InventoryDraggable>(out potentialPlantDraggable);

                    if (potentialChildPlantDraggable != null && potentialPlantDraggable == null) return;
                    if (potentialChildPlantDraggable == null && potentialPlantDraggable != null) return;
                    if (potentialChildPlantDraggable != null && potentialPlantDraggable != null && potentialPlantDraggable._plantSO != potentialChildPlantDraggable._plantSO) return;
                    if (childItemStack.GetComponent<MuchroomPlant>() != null && childItemStack.GetComponent<MuchroomPlant>().Contains != null) return;
                    if (draggableItemStack.GetComponent<MuchroomPlant>() != null && draggableItemStack.GetComponent<MuchroomPlant>().Contains != null) return;

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
        }
        else if (DraggableItem.itemBeingDragged == null && transform.childCount > 1)
		{
			transform.GetChild(1).GetComponent<DraggableItem>().EnableDrag();
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

    public override void Update()
    {
        base.Update();

        if (transform.childCount == 1)
            handImage.SetActive(true);
        else
            handImage.SetActive(false);
    }

}
