using Unity.VisualScripting;
using UnityEngine;
using Botography.Player;

public class PlayerStateMachine : MonoBehaviour
{
    public static PlayerStateMachine Instance { get; private set; }

    // Reference variables
    public Animator CharacterAnimator { get; private set; }
    public Rigidbody2D CharacterRigidbody { get; private set; }

    // State variables
    public PlayerBaseState CurrentState { get; set; }
    private PlayerStateFactory _states;

    // Variables to store optimized parameter IDs
    public int GroundedHash { get; private set; }
    public int HorizontalHash { get; private set; }
    public int VerticalHash { get; private set; }
    public int IdleHorizontalHash { get; private set; }
    public int IdleVerticalHash { get; private set; }
    public int SpeedHash { get; private set; }

    // Variables to store player input
    public Vector2 Movement { get; set; }
    public Vector2 MovementInput { get; private set; }
    public bool MovementDetected { get; private set; }
    public bool isClimbing = false;
    public bool isUnderwater = false;

    // Settings
    [Header("Settings")]
    [SerializeField] private float _walkSpeed = PlayerConstants.BaseWalkSpeed;

    private bool isInitialized = false;
    private bool _ctrlBound;

    // Getters and Setters
    public float WalkSpeed 
    { 
        get 
        { 
            return _walkSpeed; 
        } 
		set
		{
			_walkSpeed = value;
		}
    }

    public Transform PlayerTransform { get {return transform;} }

    private void Awake() 
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        CharacterAnimator = GetComponent<Animator>();
        CharacterRigidbody = GetComponent<Rigidbody2D>();

        _states = new PlayerStateFactory(this);
        CurrentState = _states.Grounded();
        CurrentState.EnterState();

        GroundedHash = Animator.StringToHash("Grounded");
        HorizontalHash = Animator.StringToHash("Horizontal");
        VerticalHash = Animator.StringToHash("Vertical");
        IdleHorizontalHash = Animator.StringToHash("IdleHorizontal");
        IdleVerticalHash = Animator.StringToHash("IdleVertical");
        SpeedHash = Animator.StringToHash("Speed");
    }

    private void Start()
    {
        isInitialized = true;
        BindControls();
        if (Tutorial.Instance != null)
            Tutorial.Instance.BeginTutorial();
    }

    private void Update() 
    {
        CurrentState.UpdateStates();
    }

    private void FixedUpdate()
    {
        Vector2 newPosition = CharacterRigidbody.position + Movement * Time.fixedDeltaTime;
        CharacterRigidbody.MovePosition(newPosition);
    }

    private void OnMovementPressed(Vector2 movement)
    {
        MovementInput = movement;
        MovementDetected = MovementInput.magnitude > 0;
        if(MovementInput.magnitude > 0){
            SoundManager.Instance.UnmuteFootsteps();
        }
    }

    public void BindControls()
    {
        if (!_ctrlBound)
        {
			InputHandler.Instance.OnMovementPressed += OnMovementPressed;
            InteractionManager.Instance.BindInteraction();
			_ctrlBound = true;
		}
    }

    public void UnbindControls()
    {
        if (_ctrlBound)
        {
            SoundManager.Instance.MuteFootsteps();
			InputHandler.Instance.OnMovementPressed -= OnMovementPressed;
			MovementInput = Vector2.zero;
            InteractionManager.Instance.UnbindInteraction();
            _ctrlBound = false;
		}
    }

    private void OnEnable()
    {
        if (isInitialized)
            BindControls();
    }

    private void OnDisable()
    {
        UnbindControls();
    }
	
	public void GoUnderwater()
	{
        isUnderwater = true;
		InventoryManager.Instance.ToggleHotbarSlots(false);
	}
	
	public void GoAbovewater()
	{
        isUnderwater = false;
		InventoryManager.Instance.ToggleHotbarSlots(true);

	}
}
