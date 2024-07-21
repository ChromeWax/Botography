using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Botography.Dependencies;
using TMPro;
using Botography.Notifications;
using Botography.Player;
using Botography.Map;
using UnityEngine.SceneManagement;
using Botography.Player.Dialogue;

/// <summary>
/// This highlights an object with which the player can interact.  The player can then interact with the object.
/// </summary>

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private InteractionManager _interactMan;
	[SerializeField] private GameObject _noteRefObj;

    private List<GameObject> _interactableNotes;
    private List<GameObject> _interactableSigns;
    private List<GameObject> _interactablePlants;
	private List<GameObject> _interactablePedestals;
    private List<AttributeValue> _interactableSamples;
    private GameObject _objectToHighlight;
    public int _panaceanPlantsPlaced = 0;

    private bool _checkSample;
    private AttributeValue _sampleToCheck;
    private int _fSinceSampleCheck;

	private bool _interactBound = true;
    
    public static PlayerInteraction Instance { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        _interactableNotes = new List<GameObject>();
        _interactableSigns = new List<GameObject>();
        _interactablePlants = new List<GameObject>();
		_interactablePedestals = new List<GameObject>();
        _interactableSamples = new();
        _objectToHighlight = null;

        InputHandler.Instance.OnInteractPressed += OnInteractPressed;
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        //If the distance between the interactable object and the player is at most 2, then the object receives a white outline.
        //
        _objectToHighlight = null;
        float _shortestDistance = 3.0f;
        if (_interactableNotes.Count != 0)
        {
            foreach (GameObject note in _interactableNotes)
            {
                if (Vector2.Distance(transform.position, note.transform.position) < _shortestDistance)
                {
                    _objectToHighlight = note;
                    _shortestDistance = Vector2.Distance(transform.position, note.transform.position);
                }
               
            }
        }

        if (_interactableSigns.Count != 0)
        {
            foreach (GameObject sign in _interactableSigns)
            {
                if (Vector2.Distance(transform.position, sign.transform.position) < _shortestDistance)
                {
                    _objectToHighlight = sign;
                    _shortestDistance = Vector2.Distance(transform.position, sign.transform.position);
                }
            }
        }

        if (_interactablePlants.Count != 0)
        {
            foreach (GameObject plant in _interactablePlants)
            {
                if (Vector2.Distance(transform.position, plant.transform.position) < _shortestDistance)//add whether or not the player can interact with the plant here
                {
                    _objectToHighlight = plant;
                    _shortestDistance = Vector2.Distance(transform.position, plant.transform.position);
                }
            }
        }
		
		if (_interactablePedestals.Count != 0 && InventoryManager.Instance.PanaceanPlantCount() > 0)
		{
			foreach (GameObject pedestal in _interactablePedestals)
			{
				if (Vector2.Distance(transform.position, pedestal.transform.position) < _shortestDistance)
				{
					_objectToHighlight = pedestal;
					_shortestDistance = Vector2.Distance(transform.position, pedestal.transform.position);
				}
			}
		}
        HighlightObject(_objectToHighlight);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Note"))
        {
            
            _interactableNotes.Add(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Sign"))
        {
            _interactableSigns.Add(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Plant") || collision.gameObject.CompareTag("PanaceanPlant"))
        {
            if (collision.gameObject.TryGetComponent<IInteractivePlaceable>(out _) || collision.gameObject.TryGetComponent<MacguffinBasePlant>(out _))
            {
                _interactablePlants.Add(collision.gameObject);
            }
        }
        if (collision.gameObject.CompareTag("Pedestal"))
        {
            _interactablePedestals.Add(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Sample"))
        {
            AttributeValue sample = _interactMan.SetSample();
            if (sample == AttributeValue.None)
            {
                return;
            }

            if (!_interactableSamples.Contains(sample))
            {
				_interactableSamples.Add(sample);
			}
        }

        CheckInteractionIcon();
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Note"))
        {
            _interactableNotes.Remove(collision.gameObject);
            collision.gameObject.GetComponent<InteractableObject>().SetHighlight(false);
        }
        else if (collision.gameObject.CompareTag("Sign"))
        {
            _interactableSigns.Remove(collision.gameObject);
            collision.gameObject.GetComponent<InteractableObject>().SetHighlight(false);
        }
        else if (collision.gameObject.CompareTag("Plant") || collision.gameObject.CompareTag("PanaceanPlant"))
        {
            InteractableObject inter = collision.gameObject.GetComponent<InteractableObject>();
            if (inter != null)
            {
				_interactablePlants.Remove(collision.gameObject);
                inter.SetHighlight(false);
			}
        }
        else if (collision.gameObject.CompareTag("Pedestal"))
        {
            _interactablePedestals.Remove(collision.gameObject);
            collision.gameObject.GetComponent<InteractableObject>().SetHighlight(false);
        }
        else if (collision.gameObject.CompareTag("Sample"))
        {
            if (!_interactMan.PlayerIsOnCurSample())
            {
                _interactableSamples.Remove(_interactMan.CurSample);
                _interactMan.SetSample(AttributeValue.None);
            }
        }
		CheckInteractionIcon();
	}

    private void CheckInteractionIcon()
    {
		string action = "";
        if (_interactableNotes.Count != 0)
        {
            action = "Note";
            _interactMan.SetSample(AttributeValue.None);
        }
        else if (_interactableSigns.Count != 0)
        {
            action = "Sign";
			_interactMan.SetSample(AttributeValue.None);
		}
        else if (_interactablePlants.Count != 0)
        {
            // TODO: Make it say the specific plant
            action = "Plant";
			_interactMan.SetSample(AttributeValue.None);
		}
        else if (_interactablePedestals.Count != 0)
        {
            action = "Pedestal";
			_interactMan.SetSample(AttributeValue.None);
		}
        else if (_interactableSamples.Count != 0)
        {
            _interactMan.SetSample(_interactableSamples[0]);
            action = PlayerConstants.INTERACTION_ACTION_LABEL[_interactableSamples[0]];
		}
        else
        {
            PlayerManager.Instance.DeactivateInteractIcon();
            return;
        }

        PlayerManager.Instance.ActivateInteractIcon(action);
    }

    public void HighlightObject(GameObject obj)
    {
        if (obj != null)
        {
            ClearSignHighlights();
            ClearNoteHighlights();
            ClearPlantHighlights();
			ClearPedestalHighlights();
            obj.GetComponent<InteractableObject>().SetHighlight(true);
        }
    }

    private void OnInteractPressed()
    {
        if (_interactBound == false) return;

        if (DraggableItem.itemBeingDragged == null)
        {
            if (_objectToHighlight != null)
            {
                if (_objectToHighlight.CompareTag("Plant"))
                {
                    ((IPlaceable)_objectToHighlight.GetComponent<BasePlant>()).PlaceableAbility();
                }
                else if (_objectToHighlight.CompareTag("Sign"))
                {
                    RegionSO region = _objectToHighlight.GetComponent<SignInteractableObject>().Region;
                    if (region.SunlightLevel == SunlightLevel.fullShade)
                        DialoguePlayer.Instance.PlayConvo("Midnight");
                    else if (region.SunlightLevel == SunlightLevel.partialShade)
                        DialoguePlayer.Instance.PlayConvo("Frozen");
                    else if (region.SunlightLevel == SunlightLevel.halfAndHalf)
                        DialoguePlayer.Instance.PlayConvo("Forest");
                    else if (region.SunlightLevel == SunlightLevel.partialSun)
                        DialoguePlayer.Instance.PlayConvo("Oil");
                    else if (region.SunlightLevel == SunlightLevel.fullSun)
                        DialoguePlayer.Instance.PlayConvo("Ashen");
                }
                else if (_objectToHighlight.CompareTag("Note"))
                {
                    if (!Tutorial.TutorialActive || Tutorial.Instance._almanacCollected)
                    {
						AlmanacManager.Instance.AddNote(_objectToHighlight.GetComponent<NoteInteractableObject>().Note);
						_noteRefObj.GetComponent<NoteReference>().rmNote(_objectToHighlight);
						int noteCount = AlmanacManager.Instance.GetCurrentNoteCount();
						int temp = 15 + (int)Mathf.Floor((noteCount - 1) / 2);
						AlmanacManager.Instance.SetCurrentPageNumber(15 + (int)((int)(noteCount - 1) / 2));
						AlmanacManager.Instance.SetPageToCurrentPageCount();
						UIManager.Instance.OnAlmanacPressed();
						Notifyer.Instance.Notify("A note has been added.");
						Destroy(_objectToHighlight);
						SoundManager.Instance.PlaySFX("page collected");
					}
                    else
                    {
                        DialoguePlayer.Instance.PlayConvo("NoteCollectBeforeTutorial");
                    }
                }
                else if (_objectToHighlight.CompareTag("PanaceanPlant"))
                {
                    Notifyer.Instance.Notify("Panacean plant has been found.");
                    SoundManager.Instance.PlaySFX("plant placed");
                    InventoryManager.Instance.PickUpPanaceanPlant(_objectToHighlight.GetComponent<MacguffinBasePlant>().GetPanaceanPlantSO(), _objectToHighlight);

                    int index = -1; 
                    switch (_objectToHighlight.gameObject.GetComponent<MacguffinBasePlant>().getName())
                    {
                        case "osmosis":
                            index = 0;
                            break;
                        case "mirror":
                            index = 1;
                            break;
                        case "stella":
                            index = 2;
                            break;
                        case "phoenix":
                            index = 3;
                            break;
                    }

                    if (index != -1)
                        AlmanacManager.Instance.SetPanaceanPlantDiscoveryStatus(index, true);

                    Debug.Log(_objectToHighlight);
                    Destroy(_objectToHighlight);
                }
                else if (_objectToHighlight.CompareTag("Pedestal"))
                {
                    Notifyer.Instance.Notify("Panacean plant has been placed on the pedestal.");
                    SoundManager.Instance.PlaySFX("plant placed");
                    _objectToHighlight.GetComponent<PedestalInteractableObject>().PlacePanacean(InventoryManager.Instance.GetPanacean());
                    _panaceanPlantsPlaced++;
                    if (_panaceanPlantsPlaced == 4)
                    {
                        TransitionManager.Instance.TransitionToBlackScreen();
                        PlayerStateMachine.Instance.UnbindControls();
                        Invoke(nameof(LoadVictoryScene), 2.5f);
                    }
                }
            }
        }
        else
        {
            Notifyer.Instance.Notify("I should probably set this down first.");
        }
    }

    public void ClearPlantHighlights()
    {
        if (_interactablePlants != null)
            foreach (var plant in _interactablePlants)
                plant.GetComponent<InteractableObject>().SetHighlight(false);
    }

    public void ClearNoteHighlights()
    {
        if (_interactableNotes != null)
            foreach (var note in _interactableNotes)
                note.GetComponent<InteractableObject>().SetHighlight(false);
    }

    public void ClearSignHighlights()
    {
        if (_interactableSigns != null)
            foreach (var sign in _interactableSigns)
                sign.GetComponent<InteractableObject>().SetHighlight(false);
    }
	
	public void ClearPedestalHighlights()
	{
		if (_interactablePedestals != null)
		{
			foreach (var pedestal in _interactablePedestals)
			{
				pedestal.GetComponent<InteractableObject>().SetHighlight(false);
			}
		}
	}

    public void ClearAll()
    {
        _interactablePlants.Clear();
        _interactableNotes.Clear();
        _interactableSamples.Clear();
        _interactablePedestals.Clear();
        _interactableSigns.Clear();
    }

    public GameObject GetHighlightObject()
    {
        return _objectToHighlight;
    }

    public void LoadVictoryScene()
    {
        SceneManager.LoadScene("Ending");
    }

    public void UnbindInteraction()
	{
		if (_interactBound)
		{
			_interactBound = false;
		}
	}

	public void BindInteraction()
	{
		if (!_interactBound)
		{
			_interactBound = true;
		}
	}

}
