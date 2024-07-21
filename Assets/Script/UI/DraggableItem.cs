using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Unity.VisualScripting;

public class DraggableItem : MonoBehaviour
{
    public static DraggableItem itemBeingDragged = null;

    [HideInInspector] public Transform ParentAfterDrag { get; set; } 
    protected bool isDragging = false;
    protected bool isInitialized = false;
    protected Vector2 mouseMovement;
    protected Image image;

    protected void Awake()
    {
        image = GetComponent<Image>();
    }

    protected virtual void Start()
    {
        InputHandler.Instance.OnCursorPressed += OnCursorPressed;
        InputHandler.Instance.OnCursorRightPressed += OnCursorRightClickPressed;
        isInitialized = true;
        ParentAfterDrag = transform.parent;
    }

    protected virtual void Update()
    {
        if (isDragging)
        {
            transform.position = mouseMovement + new Vector2(0f, 30f);
            //transform.localScale = new Vector3(0.8f, 0.8f, 1f);
			CursorManager.Instance.SetCursorType(CursorType.drag);
        }
        else
        {
            //transform.localScale = Vector3.one;
        }
    } 

    public virtual void EnableDrag()
    {
        isDragging = true;
        itemBeingDragged = this;
        try
        {
		    itemBeingDragged.transform.parent.GetComponent<Slot>()?.ItemRemoved();
        }
        catch
        {

        }
        ParentAfterDrag = transform.parent;
        transform.SetParent(transform.GetComponentInParent<Canvas>().transform);
        transform.SetAsLastSibling();
    }

    public virtual void DisableDrag()
    {
        if (this == itemBeingDragged)
        {
			
			isDragging = false;
            transform.SetParent(ParentAfterDrag);
			ParentAfterDrag.gameObject.GetComponent<Slot>().ItemAdded(this);
			itemBeingDragged = null;
		}
    }

    /*
    public virtual void OnPointerDown(PointerEventData eventData)
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
            itemBeingDragged.ParentAfterDrag = ParentAfterDrag;
            itemBeingDragged.DisableDrag();
            EnableDrag();
        }
    }
    */

    protected void OnCursorPressed(Vector2 movement)
    {
        mouseMovement = movement;
    }

    public virtual void OnCursorRightClickPressed()
    {
        if (itemBeingDragged == this && ParentAfterDrag.transform.childCount == 0)
            DisableDrag();
    }

    protected virtual void OnEnable()
    {
        if (isInitialized)
        {
            InputHandler.Instance.OnCursorPressed += OnCursorPressed;
            InputHandler.Instance.OnCursorRightPressed += OnCursorRightClickPressed;
        }
    }

    protected virtual void OnDisable()
    {
        InputHandler.Instance.OnCursorPressed -= OnCursorPressed;
        InputHandler.Instance.OnCursorRightPressed -= OnCursorRightClickPressed;
    }
}
