using System.Collections;
using System.Collections.Generic;
using Botography.Notifications;
using UnityEngine;

public class AnchorDraggable : InventoryDraggable
{
    protected override void Start()
    {
        InputHandler.Instance.OnCursorPressed += OnCursorPressed;
        isInitialized = true;
        ParentAfterDrag = transform.parent;
        Notifyer.Instance.Notify("I can place the beanstalk's anchor further away!");
    }

    protected override void Update()
    {
        if (isDragging)
        {
            transform.position = mouseMovement + new Vector2(30f, 30f);
            transform.localScale = new Vector3(0.8f, 0.8f, 1f);
        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }

    protected override void OnEnable()
    {
        if (isInitialized)
        {
            InputHandler.Instance.OnCursorPressed += OnCursorPressed;
        }
    }

    protected override void OnDisable()
    {
        InputHandler.Instance.OnCursorPressed -= OnCursorPressed;
    }
}
