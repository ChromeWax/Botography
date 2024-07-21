using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryDraggable : StackableItem
{
    public PlantSO _plantSO;

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
        // Just to make it easier when starting the game what sprites are what
        gameObject.GetComponent<Image>().sprite = _plantSO.Artwork;
    }
}

