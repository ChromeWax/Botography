using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Botography.Lab;
using Botography.Notifications;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject _pauseMenuUI;
    [SerializeField] private GameObject _labUI;
    [SerializeField] private GameObject _almanacUI;
    [SerializeField] private GameObject _inventoryUI;
    [SerializeField] private GameObject _mapUI;
    [SerializeField] private GameObject _hotBarUI;
    [SerializeField] private GameObject _statEff;
	[SerializeField] private GameObject _placementManager;
    private List<GameObject> _uiList = new List<GameObject>();
    private bool isInitialized = false;

    private void Awake() 
	{
		if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;	
	}

    private void Start()
    {
        _uiList.Add(_pauseMenuUI);
        _uiList.Add(_labUI);
        _uiList.Add(_almanacUI);
        _uiList.Add(_inventoryUI);
        _uiList.Add(_mapUI);

        isInitialized = true;
        InputHandler.Instance.OnAlmanacPressed += OnAlmanacPressed;
        InputHandler.Instance.OnLabPressed += OnLabPressed;
        InputHandler.Instance.OnInventoryPressed += OnInventoryPressed;
        InputHandler.Instance.OnMapPressed += OnMapPressed;
        InputHandler.Instance.OnExitPressed += OnExitPressed;
        InputHandler.Instance.OnHotbarPressed += OnHotbarPressed;
    }

    public void ToggleUI(GameObject ui)
    {
        if (DraggableItem.itemBeingDragged != null) return;

        // This if prevents weird behaviour when closing a menu while an item is being dragged
        if (!((ui == _inventoryUI || ui == _labUI) && ui.activeSelf && DraggableItem.itemBeingDragged != null))
        {
            if (!ui.activeSelf)
            {
                PlayerStateMachine.Instance.UnbindControls();

                foreach (GameObject uiElement in _uiList)
                    uiElement.SetActive(false);

                Notifyer.Instance.stackNotification = false;
                _placementManager.SetActive(false);
                ui.SetActive(true);
                SoundManager.Instance.MenuOpened();
                if (DraggableItem.itemBeingDragged != null)
                    DraggableItem.itemBeingDragged.DisableDrag();
            }

            // This toggles the UI off if active
            else
            {
                SoundManager.Instance.MenuClosed();
                Notifyer.Instance.stackNotification = true;
                PlayerStateMachine.Instance.BindControls();
                ui.SetActive(false);
                _placementManager.SetActive(true);
                if (DraggableItem.itemBeingDragged != null)
                    DraggableItem.itemBeingDragged.DisableDrag();
            }

            if (ui == _labUI)
            {
                LabManager.Instance.ResetSeedPos();
            }    

			CursorManager.Instance.SetCursorType(CursorType.normal);
        }
    }

    public void OnAlmanacPressed()
    {
        ToggleUI(_almanacUI);
    }

    private void OnLabPressed()
    {
        ToggleUI(_labUI);
    }

    private void OnMapPressed()
    {
        ToggleUI(_mapUI);
    }

    private void OnInventoryPressed()
    {
		//InventoryManager.Instance.toggleInventoryUI();
        ToggleUI(_inventoryUI);
    }

    public void OnExitPressed()
    {
        foreach (GameObject uiElement in _uiList)
        {
            if (uiElement.activeSelf)
            {
                // This if prevents weird behaviour when closing a menu while an item is being dragged
                if ((uiElement == _inventoryUI || uiElement == _labUI) && DraggableItem.itemBeingDragged != null)
                {
                    return;
                }

			    CursorManager.Instance.SetCursorType(CursorType.normal);
                Notifyer.Instance.stackNotification = true;
			    _placementManager.SetActive(true);
                PlayerStateMachine.Instance.BindControls();
                uiElement.SetActive(false);
                SoundManager.Instance.MenuClosed();
                if (DraggableItem.itemBeingDragged != null)
                    DraggableItem.itemBeingDragged.DisableDrag();
                return;
            }
        }

        if (DraggableItem.itemBeingDragged == null)
        {
            SoundManager.Instance.MenuOpened();
            Notifyer.Instance.stackNotification = false;
		    _placementManager.SetActive(false);
            _pauseMenuUI.SetActive(true);
        }
    }

    private void OnHotbarPressed()
    {
        if (_hotBarUI.activeSelf)
        {
            _hotBarUI.SetActive(false);
            _statEff.SetActive(false);
        }
        else
        {
            _hotBarUI.SetActive(true);
            _statEff.SetActive(true);
        }
    }

    private void OnEnable()
    {
        if (isInitialized)
        {
            InputHandler.Instance.OnAlmanacPressed += OnAlmanacPressed;
            InputHandler.Instance.OnLabPressed += OnLabPressed;
            InputHandler.Instance.OnInventoryPressed += OnInventoryPressed;
            InputHandler.Instance.OnMapPressed += OnMapPressed;
            InputHandler.Instance.OnExitPressed += OnExitPressed;
            InputHandler.Instance.OnHotbarPressed += OnHotbarPressed;
        }
    }

    private void OnDisable()
    {
        InputHandler.Instance.OnAlmanacPressed -= OnAlmanacPressed;
        InputHandler.Instance.OnLabPressed -= OnLabPressed;
        InputHandler.Instance.OnInventoryPressed -= OnInventoryPressed;
        InputHandler.Instance.OnMapPressed -= OnMapPressed;
        InputHandler.Instance.OnExitPressed -= OnExitPressed;
        InputHandler.Instance.OnHotbarPressed -= OnHotbarPressed;
    }
}
