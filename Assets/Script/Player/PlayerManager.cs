using Botography.Dependencies;
using Botography.Dependencies.Interfaces;
using Botography.Map;
using Botography.OverworldInteraction;
using Botography.Player.StatusEffects;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Rendering;
using Unity.VisualScripting;

namespace Botography.Player
{
	public class PlayerManager : ManagerBase<PlayerSystem, PlayerUiCollection>
	{
		public static PlayerManager Instance { get; private set; }
		public GameObject GameObject		
		{
			get { return gameObject; }
		}
		[SerializeField] private GameObject _shadow;
		[SerializeField] private GameObject _occlusion;
		[SerializeField] private SpriteRenderer _spRend;
		[SerializeField] private GameObject _forestRespawnSign;
        [SerializeField] private GameObject _oilSwampRespawnSign;
        [SerializeField] private GameObject _frozenRespawnSign;
        [SerializeField] private GameObject _midnightRespawnSign;
        [SerializeField] private GameObject _ashenRespawnSign;
        [SerializeField] private RegionSO[] _regions;
        //[SerializeField] private GameObject _blackOutImage;
        //[SerializeField] private RegionSO _currentRegion;
		[SerializeField] private List<LightStatTile> _lTiles;
		private Dictionary<TileBase, LightStatTile> _tileToLTile;
		[SerializeField] private Tilemap _lMap;
		[SerializeField] private List<TempStatTile> _tTiles;
		private Dictionary<TileBase, TempStatTile> _tileToTTile;
		[SerializeField] private Tilemap _tMap;
		private string lanternLayer = "Elevation Water";
		private Rigidbody2D rb;
		private StatusEffectsHandler _seHandler;
		private short _lanternCount = 0;
		
		[SerializeField]
		private int xMin;
		[SerializeField]
		private int xMax;
		private int xRange;
		private float xSecSize;
		[SerializeField]
		private int yMin;
		[SerializeField]
		private int yMax;
		private int yRange;
		private float ySecSize;
		private float yAdj;
		private List<(int x, int y)> visitedSecsList;		//for ease of checking for changes externally
		private HashSet<(int, int)> visitedSecsSet;			//for ease of checking for duplicates internally
		private float timer;

		private Vector2 _prevPos;
		private Vector2 _curPos;

		#region Regions
		[SerializeField] private RegionSO _forest;
		[SerializeField] private RegionSO _desert;
		[SerializeField] private RegionSO _canyon;
		[SerializeField] private RegionSO _swamp;
		[SerializeField] private RegionSO _midnight;
		#endregion Regions

		public Vector2 MoveDir
		{
			get
			{
				Animator anim = PlayerStateMachine.Instance.CharacterAnimator;
				float x = anim.GetFloat("Horizontal");
				float y = anim.GetFloat("Vertical");
				return new Vector2(x == 0 ? 0 : x / Math.Abs(x), y == 0 ? 0 : y / Math.Abs(y));
			}
		}

		private void Awake()
		{
			if (Instance != null && Instance != this)
				Destroy(this);
			else
				Instance = this;
			_seHandler = new StatusEffectsHandler();
			
			_tileToLTile = new Dictionary<TileBase, LightStatTile>();
			foreach (var lTile in _lTiles)
			{
				foreach (var tile in lTile.Tiles)
				{
					_tileToLTile.Add(tile, lTile);
				}
			}
			
			_tileToTTile = new Dictionary<TileBase, TempStatTile>();
			foreach (var tTile in _tTiles)
			{
				foreach (var tile in tTile.Tiles)
				{
					_tileToTTile.Add(tile, tTile);
				}
			}
		}

		// Start is called before the first frame update
		void Start()
		{
			rb = GetComponent<Rigidbody2D>();
			//_blackOutImage.SetActive(true);
			//Color objectColor = _blackOutImage.GetComponent<Image>().color;
			//objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, 0);
            //_blackOutImage.GetComponent<Image>().color = objectColor;
			
			xRange = xMax - xMin;
			xSecSize = 27.5f;
			yRange = yMax - yMin;
			ySecSize = 16;
			yAdj = -.475f;
			timer = 0f;
			visitedSecsList = new List<(int x, int y)>();
			visitedSecsList.Add((0,0));
			visitedSecsSet = new HashSet<(int, int)>();
			visitedSecsSet.Add((0,0));
			_prevPos = transform.position;
			_curPos = transform.position;
        }
		// Update is called once per frame
		void Update()
		{
			timer += Time.deltaTime;
			if(timer >= 3)
			{
				(int x, int y) sector = GetCurrentSector();
				if(!visitedSecsSet.Contains(sector))
				{
					visitedSecsSet.Add(sector);
					visitedSecsList.Add(sector);
				}
				timer = 0f;
			}
			_prevPos = _curPos;
			_curPos = transform.position;
		}
		
		public (int, int) GetCurrentSector()		//TODO: goes over on the far right
		{
			Vector2 pos = system.Position;
			int x = (int) Math.Round( ((pos.x)/xSecSize) );
			x = Math.Max(Math.Min(x, 3),-3);
			int y = (int) Math.Round( ((pos.y)/ySecSize)+yAdj );
			y = Math.Max(Math.Min(y, 3),-3);
Debug.Log("PlayerManager: 0- (" + pos.x + ", " + pos.y + ")\n(" + ((pos.x)/xSecSize) + ", " + (((pos.y)/ySecSize)+yAdj) + ")\n(" + x + ", " + y + ")");
//^^^ DO NOT DELETE
			return (x, y);
		}
		
		public List<(int x, int y)> GetVisitedSectors()
		{
			return visitedSecsList;
		}

        public Vector2 GetCurrentPlayerPosition(bool adjustToColCenter = false)
		{
			if (!adjustToColCenter)
			{
				return system.Position;
			}

			Bounds colBounds = GetComponent<Collider2D>().bounds;
			Vector2 boundsCenter = colBounds.center;
			return boundsCenter;
		}

		public void SetPlayerPosition(Vector3 position, int layer, string sortingLayer)
		{
			transform.position = position;
			gameObject.layer = layer;
			GetComponent<SortingGroup>().sortingLayerName = sortingLayer;
		}

		public void SetLayer(int layer, string sortingLayer)
		{
			gameObject.layer = layer;
			GetComponent<SortingGroup>().sortingLayerName = sortingLayer;
		}

		public int GetLayer()
		{
			return gameObject.layer;
		}

		public string GetSortingLayer()
		{
			return GetComponent<SortingGroup>().sortingLayerName;
		}

		public void SetPlayerPhysics(bool enablePhysics)
		{
			rb.simulated = enablePhysics;
		}

		public RegionSO GetCurrentRegion()
		{
			Vector3Int gridPositionInt = OWIManager.getTile(GetCurrentPlayerPosition());
			TileBase lTile = _lMap.GetTile(gridPositionInt);
			SunlightLevel lightLevel = _tileToLTile[lTile].Level;

			foreach (RegionSO region in _regions)
			{
				if (region.SunlightLevel == lightLevel)
					return region;
			}

			return null;
		}

		public void ChangeSpeed(float amount)
		{
			
			PlayerStateMachine stateMachine = gameObject.GetComponent<PlayerStateMachine>();
			stateMachine.WalkSpeed = stateMachine.WalkSpeed + amount;
		}
		
		public void PlayAnimation(string animation)
		{
			Animator characterAnimator = PlayerStateMachine.Instance.CharacterAnimator;

			characterAnimator.Play(animation, 0, 0f);
		}

		public SunlightLevel GetLightLevel()
		{
			if (_seHandler.IsIlluminated)
			{
				return SunlightLevel.fullSun;
			}

			Vector3Int gridPositionInt = OWIManager.getTile(GetCurrentPlayerPosition(true));
			TileBase lTile = _lMap.GetTile(gridPositionInt);
			
			SunlightLevel lightLevel = _tileToLTile[lTile].Level;
			return lightLevel;
		}

		//Copy and pasted from above. Edit as needed.
		public TempLevel GetTempLevel()
		{
			Vector3Int gridPositionInt = OWIManager.getTile(GetCurrentPlayerPosition());
			TileBase tTile = _tMap.GetTile(gridPositionInt);
			
			TempLevel tempLevel = _tileToTTile[tTile].Level;
			Vector2 pos = GetCurrentPlayerPosition();
			int mask = LayerMask.GetMask(LayerMask.LayerToName(system.gameObject.layer));		//gets the mask that only looks for objects on the same layer as the player character
			Collider2D[] hitCollider = Physics2D.OverlapCircleAll(pos, 6f, mask);
			foreach(Collider2D hit in hitCollider){
				/*if(hit.gameObject.GetComponent<LanternPlant>() != null)		//template for using fernace and spearmint
				{																//Levels: cold, cool, nice, warm, hot
					_seHandler.IsIlluminated = true;
					return SunlightLevel.fullSun;
				}*/
			}
			return tempLevel;
		}

		public bool IsInDarkness()
		{
			return system.InDarkness;
		}

		public void Respawn()
		{
			PlayerStateMachine.Instance.UnbindControls();
			PlayerInteraction.Instance.UnbindInteraction();
			InteractionManager.Instance.UnbindInteraction();
			StartCoroutine(FadeToBlack(() =>
			{
				// Order of these are important!
				StatusEffectsHandler.Instance.SetPrevTemp();
				ResetPlayerPositionToSign();
				StatusEffectsHandler.Instance.ClearStatusEffects();
				StatusEffectsHandler.Instance.Setup();
				PlayerStateMachine.Instance.CharacterAnimator.SetBool("RespawnBlackScreen", true);
				TransitionManager.Instance.TransitionToClearScreen();
				//gameObject.GetComponent<PlayerStateMachine>().WalkSpeed = PlayerConstants.BaseWalkSpeed;
				// I'm a monkey programmer that no longer gives fucks, if shit works it works
				bool isPlayerUnderwater = PlayerStateMachine.Instance.isUnderwater;
				if (PlayerStateMachine.Instance.isUnderwater)
					gameObject.GetComponent<PlayerStateMachine>().WalkSpeed = PlayerConstants.BaseWalkSpeed - 1.5f;
				else
					gameObject.GetComponent<PlayerStateMachine>().WalkSpeed = PlayerConstants.BaseWalkSpeed;
				UnderwaterManager.Instance.ToggleUnderwater(false);
				PlayerStateMachine.Instance.GoAbovewater();
				PlayerInteraction.Instance.ClearAll();
				PlayerStateMachine.Instance.BindControls();
				PlayerInteraction.Instance.BindInteraction();
				InteractionManager.Instance.BindInteraction();
				if (BeanstalkPlant.beanstalkBeingAnchored != null && DraggableItem.itemBeingDragged != null)
				{
					Destroy(BeanstalkPlant.beanstalkBeingAnchored.gameObject);
					Destroy(DraggableItem.itemBeingDragged.gameObject);
				}
				DeactivateInteractIcon();
			}));
			//Invoke(FadeFromBlack(), 3);
			//throw new NotImplementedException();
		}

        public IEnumerator FadeToBlack(Action callback = null)
        {
			TransitionManager.Instance.TransitionToBlackScreen();
            yield return new WaitForSeconds(3);
			if (callback != null)
			{
				callback.Invoke();
			}
			
        }

		public void ResetPlayerPositionToSign()
		{
			RegionSO r = GetCurrentRegion();
			if (r == _midnight)
			{
				Vector2 pos = new Vector2(_midnightRespawnSign.transform.position.x + 1, _midnightRespawnSign.transform.position.y - 1);
				SetPlayerPosition(pos, _midnightRespawnSign.layer, _midnightRespawnSign.GetComponent<SpriteRenderer>().sortingLayerName);
            }
			else if (r == _desert)
            {
                Vector2 pos = new Vector2(_frozenRespawnSign.transform.position.x + 1, _frozenRespawnSign.transform.position.y - 1);
				SetPlayerPosition(pos, _frozenRespawnSign.layer, _frozenRespawnSign.GetComponent<SpriteRenderer>().sortingLayerName);
            }
			else if (r == _forest)
            {
                Vector2 pos = new Vector2(_forestRespawnSign.transform.position.x + 1, _forestRespawnSign.transform.position.y - 1);
				SetPlayerPosition(pos, _forestRespawnSign.layer, _forestRespawnSign.GetComponent<SpriteRenderer>().sortingLayerName);
            }
			else if (r == _swamp)
            {
                Vector2 pos = new Vector2(_oilSwampRespawnSign.transform.position.x + 1, _oilSwampRespawnSign.transform.position.y - 1);
				SetPlayerPosition(pos, _oilSwampRespawnSign.layer, _oilSwampRespawnSign.GetComponent<SpriteRenderer>().sortingLayerName);
            }
			else if (r == _canyon)
            {
                Vector2 pos = new Vector2(_ashenRespawnSign.transform.position.x + 1, _ashenRespawnSign.transform.position.y - 1);
				SetPlayerPosition(pos, _ashenRespawnSign.layer, _ashenRespawnSign.GetComponent<SpriteRenderer>().sortingLayerName);
            }
        }

		public int GetCurrentLayer()
		{
			return gameObject.layer;
		}

		public void ActivateInteractIcon(string action)
		{
			uiCollection.ActivateInteractIcon(action);
		}

		public void DeactivateInteractIcon()
		{
			uiCollection.DeactivateInteractIcon();
		}

		public void ShowInteractIcon(bool active)
		{
			uiCollection.ShowInteractionIcon(active);
		}

		public void DisappearPlayer()
		{
			_spRend.color = new Color(_spRend.color.r, _spRend.color.g, _spRend.color.b, 0);
		}

		public void AppearPlayer()
		{
			_spRend.color = new Color(_spRend.color.r, _spRend.color.g, _spRend.color.b, 1);
		}

		public void AddLantern()
		{
			_lanternCount++;
			AssessIllumination();
		}

		public void RemoveLantern()
		{
			_lanternCount--;
			AssessIllumination();
		}

		public void ToggleShadow(bool enabled)
		{
			_shadow.SetActive(enabled);
		}

		public void ToggleOcclusion(bool enabled)
		{
			_occlusion.SetActive(enabled);
		}

		public void AssessIllumination()
		{
			if (_lanternCount > 0)
			{
				StatusEffectsHandler.Instance.IsIlluminated = true;
			}
			else
			{
				StatusEffectsHandler.Instance.IsIlluminated = false;
			}
		}
	}
}
