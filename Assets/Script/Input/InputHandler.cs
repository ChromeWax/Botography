using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance { get; private set; }
    private InputGameState _gameState;
    private bool _almanacAcquired, _seedBagAcquired, _mapAcquired, _labAcquired, _inventoryAcquired;
    public event Action<Vector2> OnMovementPressed;
    public event Action<Vector2> OnCursorPressed;
    public event Action OnCursorLeftPressed;
    public event Action<InputAction.CallbackContext> OnCursorLeftContextPressed;
    public event Action OnCursorRightPressed;
    public event Action OnInteractPressed;
    public event Action OnAlmanacPressed;
    public event Action OnLabPressed;
    public event Action OnMapPressed;
    public event Action OnInventoryPressed;
    public event Action OnExitPressed;
    public event Action OnHotbarPressed;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    public void OnCursorInput(InputAction.CallbackContext context) 
    {
        Vector2 cursorPosition = context.ReadValue<Vector2>();

        if (_gameState == InputGameState.inGame)
            OnCursorPressed?.Invoke(cursorPosition);
    }

    public void OnCursorLeftInput(InputAction.CallbackContext context) 
    {
        if (context.started && _gameState == InputGameState.inGame)
        {
            OnCursorLeftPressed?.Invoke();
            //_gameState = InputGameState.inAlmanac;
        }
    }
    
    public void OnCursorLeftContextInput(InputAction.CallbackContext context) 
    {
        OnCursorLeftContextPressed?.Invoke(context);
    }

     public void OnCursorRightInput(InputAction.CallbackContext context) 
    {
        if (context.started && _gameState == InputGameState.inGame)
        {
            OnCursorRightPressed?.Invoke();
            //_gameState = InputGameState.inAlmanac;
        }
    }

    public void OnMovementInput(InputAction.CallbackContext context) 
    {
        Vector2 movement = context.ReadValue<Vector2>();

        if (_gameState == InputGameState.inGame)
            OnMovementPressed?.Invoke(movement);
    }

    public void OnInteractInput(InputAction.CallbackContext context) 
    {
        if (context.started && _gameState == InputGameState.inGame)
            OnInteractPressed?.Invoke();
    }

    public void OnLabInput(InputAction.CallbackContext context) 
    {
        if(Tutorial.Instance != null)
        {
            _labAcquired = Tutorial.Instance._labCollected;
        }
        else
        {
            _labAcquired = true;
        }
        if (context.started && _gameState == InputGameState.inGame && _labAcquired)
            OnLabPressed?.Invoke();
    }

    public void OnAlmanacInput(InputAction.CallbackContext context)
    {
        if (Tutorial.Instance != null)
        {
            _almanacAcquired = Tutorial.Instance._almanacCollected;
        }
        else
        {
            _almanacAcquired = true;
        }
        if (context.started && _gameState == InputGameState.inGame && _almanacAcquired)
        {
            OnAlmanacPressed?.Invoke();
            //_gameState = InputGameState.inAlmanac;
        }
    }

    public void OnMapInput(InputAction.CallbackContext context)
    {
        if (Tutorial.Instance != null)
        {
            _mapAcquired = Tutorial.Instance._mapCollected;
        }
        else
        {
            _mapAcquired = true;
        }
        if (context.started && _gameState == InputGameState.inGame && _mapAcquired)
        {
            OnMapPressed?.Invoke();
            //_gameState = InputGameState.inMap;
        }
    }

    public void OnInventoryInput(InputAction.CallbackContext context)
    {
        if (Tutorial.Instance != null)
        {
            _inventoryAcquired = Tutorial.Instance._inventoryCollected;
        }
        else
        {
            _inventoryAcquired = true;
        }
        if (context.started && _gameState == InputGameState.inGame && _inventoryAcquired)
        {
            OnInventoryPressed?.Invoke();
            //_gameState = InputGameState.inInventory;
        }
    }

    public void OnExitInput(InputAction.CallbackContext context) 
    {
        if (context.started && _gameState == InputGameState.inGame)
        {
            OnExitPressed?.Invoke();
            //_gameState = InputGameState.inOption;
        }
    }

    public void OnHotbarInput(InputAction.CallbackContext context)
    {
        if (context.started && _gameState == InputGameState.inGame)
            OnHotbarPressed?.Invoke();
    }

    public void SwitchGameState(InputGameState gameState)
    {
        _gameState = gameState;
    }
}
