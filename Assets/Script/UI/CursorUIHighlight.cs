using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorUIHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
		if (DraggableItem.itemBeingDragged == null)
			CursorManager.Instance.SetCursorType(CursorType.hover);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
		if (DraggableItem.itemBeingDragged == null)
			CursorManager.Instance.SetCursorType(CursorType.normal);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
		if (DraggableItem.itemBeingDragged == null)
			CursorManager.Instance.SetCursorType(CursorType.normal);
    }
}
