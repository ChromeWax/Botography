using System.Collections;
using Botography.OverworldInteraction;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static InventoryConstants;
using Botography.Player;
using Botography.Notifications;
using System;
using Botography.Dependencies;
using Botography.Player;
using Botography.Player.StatusEffects;

public class BeanstalkPlant : BasePlant, IInteractivePlaceable
{
    public static BeanstalkPlant beanstalkBeingAnchored = null;

    [SerializeField] private GameObject _anchorGO;
    [SerializeField] private GameObject _beanstalkCannonGO;
    [SerializeField] private GameObject _colliderGO;
    private LineRenderer _lineRenderer;

    private bool anchorDrag;
    private bool moving;
    private AnchorDraggable _anchorDraggable;
    public BeanstalkAnchor Anchor;
    private Vector2 mouseMovement;
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private SpriteRenderer _beanstalkRenderer;
    [SerializeField] private SpriteRenderer _cannonRenderer;
	[SerializeField] private Animator _beanstalkAnimator;
	[SerializeField] private Animator _cannonAnimator;
    [SerializeField] private Animator _vineAnimator;
    private bool isInitialized = false;
    public bool enableDrag = true;
    
    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCursorPressed(Vector2 movement)
    {
        mouseMovement = movement;
    }

    private void Start()
    {
		InputHandler.Instance.OnCursorPressed += OnCursorPressed;
		isInitialized = true;

        _lineRenderer.SetPosition(0, transform.position);

        if (enableDrag)
        {
            GameObject obj = Instantiate(_anchorGO);
            obj.transform.SetParent(UIManager.Instance.gameObject.transform);
            _anchorDraggable = obj.GetComponent<AnchorDraggable>();
            _anchorDraggable.EnableDrag();
            anchorDrag = true;
        }

        _colliderGO.SetActive(true);

    }

    public void Pickup(Vector3Int gridPositionInt, Dictionary<Vector3Int, GameObject> objCoordinatesDict, GameObject objectToDestroy)
    {
        if (!anchorDrag)
        {
            StartCoroutine(PickupAnimationAndNotify(gridPositionInt, objCoordinatesDict, objectToDestroy));
        }
    }

    private IEnumerator PickupAnimationAndNotify(Vector3Int gridPositionInt, Dictionary<Vector3Int, GameObject> objCoordinatesDict, GameObject objectToDestroy)
    {
        PlayerStateMachine.Instance.UnbindControls();
        PlayerManager.Instance.PlayAnimation("Player_Pluck");
        yield return new WaitForSeconds(0.5f);

        objCoordinatesDict.Remove(gridPositionInt);
        Notifyer.Instance.Notify(plantSO.getPlantName() + " has been picked up!");
        InventoryManager.Instance.addItemToInv(plantSO);
        Destroy(objectToDestroy);

        objCoordinatesDict.Remove(Anchor.gridPosition);
        Destroy(Anchor.gameObject);

        PlayerStateMachine.Instance.BindControls();
    }

    public void PlaceableAbility()
    {
        PlayerStateMachine.Instance.UnbindControls();
        PlayerManager.Instance.SetPlayerPhysics(false);
        if (gameObject.layer != Anchor.gameObject.layer)
            PlayerManager.Instance.SetPlayerPosition(transform.position, 0, "UI");
        else
            PlayerManager.Instance.SetPlayerPosition(transform.position, 0, gameObject.GetComponent<SpriteRenderer>().sortingLayerName);
        PlayerStateMachine.Instance.isClimbing = true;
		
		PlayShootAnimation();

        // Handles animation
        Vector3 position = Anchor.transform.position;
        Vector3 directionVector = position - transform.position;
        float angle = Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg - 90;
        if (Anchor.gameObject.GetComponent<SpriteRenderer>().sortingLayerID == gameObject.GetComponent<SpriteRenderer>().sortingLayerID)
        {
            PlayerManager.Instance.gameObject.GetComponent<Animator>().Play("Movement");
            PlayerManager.Instance.gameObject.GetComponent<Animator>().SetFloat("Speed", 2f);
            Vector2 direction = new Vector2(-Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
            PlayerManager.Instance.gameObject.GetComponent<Animator>().SetFloat("Horizontal", direction.x);
            PlayerManager.Instance.gameObject.GetComponent<Animator>().SetFloat("Vertical", direction.y);
        }
        else if (Anchor.gameObject.layer > gameObject.layer)
        {
            PlayerManager.Instance.gameObject.GetComponent<Animator>().Play("Player_Climb");
            if (angle > 90 || angle < -90)
                PlayerManager.Instance.gameObject.transform.eulerAngles = new Vector3(0, 0, angle + 180);
            else
                PlayerManager.Instance.gameObject.transform.eulerAngles = new Vector3(0, 0, angle);
        }
        else
        {
            PlayerManager.Instance.gameObject.GetComponent<Animator>().Play("Player_Climb");
            if (angle > 90 || angle < -90)
                PlayerManager.Instance.gameObject.transform.eulerAngles = new Vector3(0, 0, angle);
            else
                PlayerManager.Instance.gameObject.transform.eulerAngles = new Vector3(0, 0, angle + 180);
        }

        moving = true;
        StatusEffectsHandler.Instance.StatusEffectsEnabled = false;
        PlayerManager.Instance.ToggleShadow(false);
        PlayerManager.Instance.ToggleOcclusion(false);
    }
	
	private void PlayShootAnimation()
	{
		_beanstalkAnimator.SetTrigger("Shoot");
		_cannonAnimator.SetTrigger("Shoot");
        _vineAnimator.SetTrigger("Shoot");
    }
	
	public GameObject ValidatePlacement(Dictionary<int, GameObject> tilemapDict, GameObject playerController, Vector3Int gridPos)
    {
        GameObject curTilemapObj = tilemapDict[playerController.layer];			//Gets the tilemap GameObject the player is currently on
		Tilemap curTilemap = curTilemapObj.GetComponent<Tilemap>();				//Gets the tilemap component from ^^^
		TileBase tile = curTilemap.GetTile(gridPos);							//Gets the specific tile that was clicked
		
		if(tile == null)
			return null;
		return curTilemapObj;
    }
	
	public void instantiateObject(Vector3Int gridPositionInt, GameObject selectedObj, TilemapRenderer curRenderer, Dictionary<Vector3Int, GameObject> objCoordinatesDict)
	{
        GameObject obj = Instantiate(selectedObj);
        obj.GetComponent<BeanstalkPlant>().StartCoroutine(PlaceAnimation());
		obj.GetComponent<SpriteRenderer>().sortingLayerID = curRenderer.sortingLayerID;
		obj.layer = curRenderer.gameObject.layer;
		//float xOff = (UnityEngine.Random.Range(-.15f, .15f));				//Adds some variation to the placement of the plant (makes it look less griddy)
		//float yOff = (UnityEngine.Random.Range(-.15f, .15f));
		Vector3 position = OWIManager.V3ItoV3(gridPositionInt);
		obj.transform.position = position;
		objCoordinatesDict.Add(gridPositionInt, obj);			//Coordinates used for saving and for placement
        beanstalkBeingAnchored = obj.GetComponent<BeanstalkPlant>();
	}

    private IEnumerator PlaceAnimation()
    {
        PlayerStateMachine.Instance.UnbindControls();
        PlayerManager.Instance.PlayAnimation("Player_Use");
        yield return new WaitForSeconds(0.5f);
        PlayerStateMachine.Instance.BindControls();
    }

    public bool IsOfInventoryType(InventoryItemType type)
    {
        return type == InventoryItemType.PlaceableInteractive;
    }

    public void StopAnchor(BeanstalkAnchor anchor)
    {
        Anchor = anchor;
        _colliderGO.SetActive(false);
        anchorDrag = false;
        beanstalkBeingAnchored = null;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_colliderGO.activeSelf && Anchor != null)
            _colliderGO.SetActive(false);

        if (anchorDrag) 
        {
            // Handles linerenderer
		    Vector3 position = Camera.main.ScreenToWorldPoint(new Vector3(mouseMovement.x, mouseMovement.y, 10));
            Vector3 directionVector = position - transform.position;

            // Handles cannon direction
            float angle = Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg - 90;
	        _beanstalkCannonGO.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            float angleInRads = (angle * (MathF.PI)) / 180;
            float xOffset = 0.749f * Mathf.Sin(angleInRads);
            float yOffset = 0.749f * Mathf.Cos(angleInRads);
            _lineRenderer.SetPosition(0, new Vector3(transform.position.x - xOffset, transform.position.y + yOffset, transform.position.z));
            _lineRenderer.SetPosition(1, position);
        }
        else
        {
            _lineRenderer.SetPosition(1, Anchor.transform.position);
            Vector3 directionVector = Anchor.transform.position - transform.position;

            // Handles cannon direction
            float angle = Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg - 90;
	        _beanstalkCannonGO.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            float angleInRads = (angle * (MathF.PI)) / 180;
            float xOffset = 0.749f * Mathf.Sin(angleInRads);
            float yOffset = 0.749f * Mathf.Cos(angleInRads);
            _lineRenderer.SetPosition(0, new Vector3(transform.position.x - xOffset, transform.position.y + yOffset, transform.position.z));
            if (gameObject.layer == Anchor.gameObject.layer)
                _lineRenderer.sortingLayerName = GameObject.GetComponent<SpriteRenderer>().sortingLayerName;
        }

        if (moving)
        {
            Vector3 playerPos = PlayerManager.Instance.GetCurrentPlayerPosition();
            Vector3 newPos = Vector3.MoveTowards(playerPos, Anchor.transform.position, 2f * Time.deltaTime);
            if (gameObject.layer == Anchor.gameObject.layer)
                PlayerManager.Instance.SetPlayerPosition(newPos, 0, gameObject.GetComponent<SpriteRenderer>().sortingLayerName);
            else
                PlayerManager.Instance.SetPlayerPosition(newPos, 0, "UI");
            if (Mathf.Abs((newPos - Anchor.transform.position).magnitude) < 0.1f)
            {
                // Handles animation
                PlayerStateMachine.Instance.isClimbing = false;
                PlayerManager.Instance.gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
                PlayerManager.Instance.gameObject.GetComponent<Animator>().SetFloat("Speed", 0f);

                moving = false;
                PlayerStateMachine.Instance.BindControls();
                PlayerManager.Instance.SetPlayerPhysics(true);
                PlayerManager.Instance.SetLayer(Anchor.gameObject.layer, Anchor.GetAnchorLayer());
                StatusEffectsHandler.Instance.StatusEffectsEnabled = true; 
                PlayerManager.Instance.ToggleShadow(true);
                PlayerManager.Instance.ToggleOcclusion(true);
            }
        }

        _beanstalkRenderer.sortingLayerID = _spriteRenderer.sortingLayerID;
        _cannonRenderer.sortingLayerID = _spriteRenderer.sortingLayerID;
    }

    public string GetAnchorLayer()
    {
        return _spriteRenderer.sortingLayerName;
    }

    private void OnEnable()
	{
		if (isInitialized)
		{
			InputHandler.Instance.OnCursorPressed += OnCursorPressed;
		}
	}

	private void OnDisable()
	{
		InputHandler.Instance.OnCursorPressed -= OnCursorPressed;
	}
}
