using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance { get; private set; }

    [SerializeField] private Texture2D defaultCursor;
    [SerializeField] private Texture2D clickCursor;
    [SerializeField] private Texture2D dragCursor;
    [SerializeField] private Texture2D hoverCursor;
    [SerializeField] private List<Texture2D> markerCursors;
    [SerializeField] private Vector2 cursorHotspot;

    private CursorType pointerCursorType = CursorType.normal;
    private bool isPressed = false;
    private bool isInitialized = false;
    private bool isCircleMarker = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    private void Start()
    {
		InputHandler.Instance.OnCursorLeftContextPressed += OnCursorLeftContextPressed;
        isInitialized = true;
    }

    private void Update()
    {
        if (isPressed && !MarkerHandler.markerSelected)
            Cursor.SetCursor(clickCursor, cursorHotspot, CursorMode.Auto);
        else if (!isPressed && !MarkerHandler.markerSelected)
        {
            if (pointerCursorType == CursorType.hover)
                Cursor.SetCursor(hoverCursor, cursorHotspot, CursorMode.Auto);
            else if (pointerCursorType == CursorType.drag)
                Cursor.SetCursor(dragCursor, cursorHotspot, CursorMode.Auto);
            else
                Cursor.SetCursor(defaultCursor, cursorHotspot, CursorMode.Auto);
        }
    }

	private void OnCursorLeftContextPressed(InputAction.CallbackContext context)
	{
        isPressed = context.performed;
    }

    public void SetCursorType(CursorType cursorType)
    {
        pointerCursorType = cursorType;
    }

    public CursorType GetCursorType()
    {
        return pointerCursorType;
    }

	private void OnEnable()
	{
		if (isInitialized)
		    InputHandler.Instance.OnCursorLeftContextPressed += OnCursorLeftContextPressed;
	}

	private void OnDisable()
	{
		InputHandler.Instance.OnCursorLeftContextPressed -= OnCursorLeftContextPressed;
	}
}
