using Botography.OverworldInteraction;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using Botography.Notifications;
using Botography.Player;
using Botography.Player.Dialogue;

public class InteractionManager : OWIManager
{
	public static InteractionManager Instance { get; private set; }
    [SerializeField] private List<PhysicalStatTile> _pTiles;
	private Dictionary<TileBase, PhysicalStatTile> _tileToPTile;
	[SerializeField] private List<GameObject> _tilemapObjects;
	private Dictionary<int, GameObject> _tilemapObjsDict;
	private AttributeValue _currSample;

	private bool _interactBound = false;

	public AttributeValue CurSample
	{
		get
		{
			return _currSample;
		}
	}

	private void Awake()
	{
		if (Instance != null && Instance != this)
			Destroy(this);
		else
			Instance = this;
	}

		//Start is called before the first frame update
		void Start()
	{
		_tilemapObjsDict = new Dictionary<int, GameObject>();
		for (int i=0; i<_tilemapObjects.Count; i++)
			_tilemapObjsDict.Add(_tilemapObjects[i].layer, _tilemapObjects[i]);

		Tilemap map = _tilemapObjsDict[playerController.layer].GetComponent<Tilemap>();

		_tileToPTile = new Dictionary<TileBase, PhysicalStatTile>();
		foreach (var pTile in _pTiles)
			foreach (var tile in pTile.Tiles)
				_tileToPTile.Add(tile, pTile);

		BindInteraction();
		isInitialized = true;
		SetSample();
	}

	private void OnInteractionPressed() 
	{ 
		AttemptTakeSample();
	}

	private void AttemptTakeSample(){
		if (_currSample != AttributeValue.None)
		{
			if (Tutorial.TutorialActive && !Tutorial.Instance._inventoryCollected)
			{
				DialoguePlayer.Instance.PlayConvo("SampleCollectBeforeTutorial");
				return;
			}
			if (InventoryManager.Instance.FillJar(_currSample))
			{
				PlayerStateMachine.Instance.UnbindControls();
				PlayerManager.Instance.PlayAnimation("Player_Pluck");
				return;
			}
			else
			{
				Notifyer.Instance.Notify("No more jars to fill. Maybe I can find some material and craft more.");
				return;
			}
		}
	}
	
	/// <summary>
	/// Call if you have a particular sample you want to set.
	/// </summary>
	/// <param name="sample"></param>
	public void SetSample(AttributeValue sample)
	{
		_currSample = sample;
	}

	/// <summary>
	/// Call to have the InteractionManager iterate through the tiles and determine the sample.
	/// </summary>
	public AttributeValue SetSample()
	{
		Tilemap _pMap = _tilemapObjsDict[playerController.layer].GetComponent<Tilemap>();

		Vector3 pos = PlayerManager.Instance.GetCurrentPlayerPosition(true);

		int range = 2;

		for (int i = 0; i < range + 1; i++)
		{
			List<TileBase> tileArea = new List<TileBase>();
			for (int j = -i; j < i + 1; j++)
				for (int k = -i; k < i + 1; k++)
				{
					if (Math.Abs(j) >= i && Math.Abs(k) >= i)
					{
						tileArea.Add(_pMap.GetTile(getTile(pos.x + j, pos.y + k)));
					}
				}

			foreach (TileBase tile in tileArea)
			{
				if (tile != null && PlayerInteraction.Instance.GetHighlightObject() == null)
				{
					_currSample = _tileToPTile[tile].Attribute;
					return _currSample;
				}
			}
		}

		_currSample = AttributeValue.None;
		return AttributeValue.None;
	}

	public bool PlayerIsOnCurSample()
	{
		Tilemap _pMap;
		try
		{
			_pMap = _tilemapObjsDict[playerController.layer].GetComponent<Tilemap>();
		}
		catch (Exception ex)
		{
			return false;
		}

		Vector3 pos = PlayerManager.Instance.GetCurrentPlayerPosition(true);

		PhysicalStatTile t = null;
		PhysicalStatTile front = null;
		try
		{
			t = _tileToPTile[_pMap.GetTile(getTile(pos.x, pos.y))];
			front = _tileToPTile[_pMap.GetTile(getTile(pos.x + PlayerManager.Instance.MoveDir.x, pos.y + PlayerManager.Instance.MoveDir.y))];
		}
		catch (ArgumentNullException e)
		{
			return false;
		}

		return t.Attribute == _currSample && front.Attribute == _currSample;
	}

	public void UnbindInteraction()
	{
		if (_interactBound)
		{
			InputHandler.Instance.OnInteractPressed -= OnInteractionPressed;
			_interactBound = false;
		}
	}

	public void BindInteraction()
	{
		if (!_interactBound)
		{
			InputHandler.Instance.OnInteractPressed += OnInteractionPressed;
			_interactBound = true;
		}
	}

	private void OnEnable()
	{
        if (isInitialized)
			BindInteraction();
	}

	private void OnDisable()
	{
		UnbindInteraction();
	}
}
