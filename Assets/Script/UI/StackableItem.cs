using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class StackableItem : DraggableItem
{
    public static bool isOverSlot;
    [SerializeField] protected int _stackTotal = 1;
    protected GameObject _stackText;
    private bool enableRightClick = true;

    // Start is called before the first frame update
    protected override void Start()
    {
        InputHandler.Instance.OnCursorPressed += OnCursorPressed;
        InputHandler.Instance.OnCursorRightPressed += OnCursorRightClickPressed;
        isInitialized = true;
        ParentAfterDrag = transform.parent;
        //_stackTotal = 1;
        _stackText = transform.GetChild(0).gameObject;
        _stackText.SetActive(_stackTotal > 1);
        _stackText.GetComponent<TextMeshProUGUI>().text = _stackTotal.ToString();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        _stackText.GetComponent<TextMeshProUGUI>().text = _stackTotal.ToString();
        _stackText.SetActive(_stackTotal > 1);

        if (_stackTotal < 1)
            Destroy(gameObject);
    }

    public override void EnableDrag()
    {
        isDragging = true;
        itemBeingDragged = this;
        try
        {
            itemBeingDragged.transform.parent.GetComponent<Slot>().ItemRemoved();
        }
        catch
        {

        }
        ParentAfterDrag = transform.parent;
        //_stackText.SetActive(false);
        transform.SetParent(transform.GetComponentInParent<Canvas>().transform);
        transform.SetAsLastSibling();
    }

    public override void DisableDrag()
    {
        if (this == itemBeingDragged)
        {
            isDragging = false;
            /*
            if (_stackTotal > 1)
            {
                _stackText.SetActive(true);
            }
            */
            transform.SetParent(ParentAfterDrag);
            if (_stackTotal == 1)
            {
				ParentAfterDrag.gameObject.GetComponent<Slot>().ItemAdded(this);
			}
			itemBeingDragged = null;
		}
    }

    /*
    public override void OnPointerDown(PointerEventData eventData)
    {
        // There is no item being dragged
        if (itemBeingDragged == null && eventData.button == PointerEventData.InputButton.Left)
        {
            EnableDrag();
        }
        // There is an item being dragged and its this draggable
        else if (itemBeingDragged == this)
        {
            DisableDrag();
        }
        // There is an item being dragged but not this draggable
        else if (itemBeingDragged != null && itemBeingDragged != this)
        {
            if ((GetComponent<Jar>() != null && itemBeingDragged.GetComponent<Jar>() != null && GetComponent<Jar>().ItemAmount == 0 && itemBeingDragged.GetComponent<Jar>().ItemAmount == 0) || 
            (GetComponent<InventoryDraggable>() != null && itemBeingDragged.GetComponent<InventoryDraggable>() != null && GetComponent<InventoryDraggable>()._plantSO.PlantName == itemBeingDragged.GetComponent<InventoryDraggable>()._plantSO.PlantName))
            {
                StackableItem draggableItemStack = (StackableItem) itemBeingDragged;

                if (eventData.button == PointerEventData.InputButton.Left)
                {
                    if (GetStackTotal() == 5 || draggableItemStack.GetStackTotal() == 5)
                    {
                        itemBeingDragged.ParentAfterDrag = ParentAfterDrag;
                        itemBeingDragged.DisableDrag();
                        EnableDrag();
                    }

                    else if (GetStackTotal() + draggableItemStack.GetStackTotal() > 5)
                    {
                        int subtractTotal = (5 - GetStackTotal()) * -1;

                        SetStackTotal(5);
                        draggableItemStack.ChangeStackTotal(subtractTotal);
                    }

                    else
                    {
                        ChangeStackTotal(itemBeingDragged.GetComponent<StackableItem>()._stackTotal);
                        Destroy(itemBeingDragged.gameObject);
                    }
                }
                else if (eventData.button == PointerEventData.InputButton.Right)
                {
                    if (GetStackTotal() + 1 <= 5)
                    {
                        ChangeStackTotal(1);
                        draggableItemStack.ChangeStackTotal(-1);
                    }
                }
            }
            else
            {
                itemBeingDragged.ParentAfterDrag = ParentAfterDrag;
                itemBeingDragged.DisableDrag();
                EnableDrag();
            }
        }
    }
    */

    public int GetStackTotal()
    {
        return _stackTotal;
    }

    public override void OnCursorRightClickPressed()
    {
        if (itemBeingDragged == this && isOverSlot == false && enableRightClick)
        {
            if (ParentAfterDrag.transform.childCount == 0)
                DisableDrag();
            else if (ParentAfterDrag.transform.childCount == 1 & ParentAfterDrag.transform.GetChild(0).GetComponent<InventoryDraggable>()._plantSO == transform.GetComponent<InventoryDraggable>()._plantSO &&
                ParentAfterDrag.transform.GetChild(0).GetComponent<InventoryDraggable>().GetStackTotal() + transform.GetComponent<InventoryDraggable>().GetStackTotal() <= 5)
            {
                ParentAfterDrag.transform.GetChild(0).GetComponent<StackableItem>().ChangeStackTotal(GetStackTotal());
                Destroy(gameObject); 
            }
        }
    }

    public void ChangeStackTotal(int number)
    {
        _stackTotal += number;
    }

    public void SetStackTotal(int number)
    {
        _stackTotal = number;
    }

    public void RemoveRightClickAbility()
    {
        enableRightClick = false;
    }
}
