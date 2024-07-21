using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
	public Action<DraggableItem> DraggableRemoved;
	public Action<DraggableItem> DraggableAdded;
	private DraggableItem _currItem;
	protected bool enableHighlight;

	public DraggableItem CurrItem
	{
		get { return _currItem; }
	}

	public virtual void Update()
	{
		if (enableHighlight)
		{
			GetComponent<Image>().color = new Color(.8f, .8f, .8f, 1f);
			if (DraggableItem.itemBeingDragged == null)
				CursorManager.Instance.SetCursorType(CursorType.hover);
		}
		else
			GetComponent<Image>().color = Color.white;
	}

    public virtual void OnPointerDown(PointerEventData eventData)
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
					SoundManager.Instance.PlaySFX("select");
				}
				else
					SoundManager.Instance.PlaySFX("deselect");

			}
			else
			{
				DraggableItem.itemBeingDragged.DisableDrag();
				SoundManager.Instance.PlaySFX("deselect");
			}
        }
		else if (DraggableItem.itemBeingDragged == null && transform.childCount > 0)
		{
			transform.GetChild(0).GetComponent<DraggableItem>().EnableDrag();
			SoundManager.Instance.PlaySFX("select");
		}
		else
		{
			return;
		}
    }
	
	protected virtual bool ValidSlot(PointerEventData eventData)
	{
		return true;
	}
	
	public virtual void ItemRemoved()
	{
		DraggableItem item = _currItem;
		_currItem = null;
		if (DraggableRemoved != null)
		{
			DraggableRemoved.Invoke(item);
		}
		
	}

	public virtual void ItemAdded(DraggableItem item)
	{
		if (DraggableAdded != null)
		{
			DraggableAdded.Invoke(item);
		}
	}

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
		if (DraggableItem.itemBeingDragged == null)
			CursorManager.Instance.SetCursorType(CursorType.hover);
		enableHighlight = true;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
		if (DraggableItem.itemBeingDragged == null)
			CursorManager.Instance.SetCursorType(CursorType.normal);
		enableHighlight = false;
    }

	public virtual void OnDisable()
	{
		enableHighlight = false;
	}
}
