using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Botography.Notifications;

public class CharacterRepresentationUI : Slot, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image _character;
    [SerializeField] private Image _headImage;
    [SerializeField] private Image _bodyImage;
    [SerializeField] private Image _legImage;
    [SerializeField] private GameObject _headSlot;
    [SerializeField] private GameObject _bodySlot;
    [SerializeField] private GameObject _legSlot;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (DraggableItem.itemBeingDragged != null)
        {
            _character.color = Color.white; 

            if (((StackableItem) DraggableItem.itemBeingDragged).GetStackTotal() > 1)
            {
                StackableItem draggableItemStack = (StackableItem) DraggableItem.itemBeingDragged;

                draggableItemStack.ChangeStackTotal(-1);
                ConsumptionManager.Instance.OnPointerClick(false);
            }
            else
                ConsumptionManager.Instance.OnPointerClick(true);

            enableHighlight = false;
        }
        else
            Notifyer.Instance.Notify("You're clicking me with nothing!");
    }

	public override void OnPointerEnter(PointerEventData eventData)
	{
		if (DraggableItem.itemBeingDragged == null)
			CursorManager.Instance.SetCursorType(CursorType.hover);
        if (DraggableItem.itemBeingDragged != null)
        {
			enableHighlight = true;
		}
	}

	public override void Update()
    {
		if (enableHighlight)
        {
			_character.color = Color.gray;
            _headImage.color = Color.gray;
            _bodyImage.color = Color.gray;
            _legImage.color = Color.gray;
        }
		else
        {
			_character.color = Color.white;
            _headImage.color = Color.white;
            _bodyImage.color = Color.white;
            _legImage.color = Color.white;
        }

        _headImage.enabled = _headSlot.transform.childCount > 1;
        _bodyImage.enabled = _bodySlot.transform.childCount > 1;
        _legImage.enabled = _legSlot.transform.childCount > 1;
    }
}
