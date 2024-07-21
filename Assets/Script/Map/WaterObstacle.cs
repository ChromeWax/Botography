using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Botography.Player.StatusEffects;
using Botography.Player;
using System;
using System.Linq;

public class WaterObstacle : ObstacleAbstract
{
	private List<GameObject> _obstacleColliderList = new List<GameObject>();
	private Dictionary<Vector2, GameObject> _obstacleColliderDictionary = new Dictionary<Vector2, GameObject>();
	private List<GameObject> _activeCollders = new List<GameObject>();
	public bool underwater = false;

	private void Start()
	{
		int i;
		for (i = 0; i < transform.childCount; i++)
		{
			_obstacleColliderList.Add(transform.GetChild(i).gameObject);
			transform.GetChild(i).GetComponent<SpriteRenderer>().color = Color.clear;
		}

		for (i = 0; i < _obstacleColliderList.Count; i++)
		{
			_obstacleColliderDictionary.Add(_obstacleColliderList[i].transform.position, _obstacleColliderList[i]);
		}

		foreach (var collider in _obstacleColliderList)
		{
			collider.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
			collider.SetActive(false);
		}
	}

	protected override void OnTriggerStay2D(Collider2D collider){}

	private void Update()
	{
		if (_obstacleColliderDictionary.Count > 0)
		{
			if (gameObject.layer == PlayerManager.Instance.gameObject.layer && _obstacleColliderDictionary.Count > 0)
			{
				Vector2 playerPos = PlayerManager.Instance.gameObject.transform.position;
				Vector2 gridPlayerPos = new Vector2(Mathf.Floor(playerPos.x) + 0.5f, Mathf.Floor(playerPos.y) + 0.5f);
			
				if (underwater)
				{
					List<GameObject> listOfGameObjects = _obstacleColliderDictionary.Values.ToList();
					List<float> distanceToGameObjects = new List<float>();

					foreach (var otherCollider in listOfGameObjects)
					{
						float playerDistanceToCollider = Vector2.Distance(playerPos, otherCollider.transform.position);
						distanceToGameObjects.Add(playerDistanceToCollider);
					}

					for (int i = 0; i < 5; i++)
					{
						int closestIndex = distanceToGameObjects.IndexOf(distanceToGameObjects.Min());
						listOfGameObjects.RemoveAt(closestIndex);
						distanceToGameObjects.RemoveAt(closestIndex);
					}

					List<Vector2> waterPos = _obstacleColliderDictionary.Keys.ToList();
					Vector2 playerMov = PlayerStateMachine.Instance.Movement.normalized;
					Vector2 newPlayerPos = new Vector2(Mathf.Floor(playerMov.x + playerPos.x) + 0.5f, Mathf.Floor(playerMov.y + playerPos.y) + 0.5f);

					if (waterPos.IndexOf(newPlayerPos) == -1)
						PlayerManager.Instance.transform.position = new Vector3(newPlayerPos.x, newPlayerPos.y, 0);
					else
					{
						foreach (var gameObjectCollider in _activeCollders)
						{
							if (waterPos.IndexOf(new Vector2(gameObjectCollider.transform.position.x + 1, gameObjectCollider.transform.position.y)) == -1)
							{
								PlayerManager.Instance.transform.position = gameObjectCollider.transform.position + new Vector3(1, 0, 0);
								break;
							}
							else if (waterPos.IndexOf(new Vector2(gameObjectCollider.transform.position.x - 1, gameObjectCollider.transform.position.y)) == -1)
							{
								PlayerManager.Instance.transform.position = gameObjectCollider.transform.position + new Vector3(-1, 0, 0);
								break;
							}
							else if (waterPos.IndexOf(new Vector2(gameObjectCollider.transform.position.x, gameObjectCollider.transform.position.y + 1)) == -1)
							{
								PlayerManager.Instance.transform.position = gameObjectCollider.transform.position + new Vector3(0, 1, 0);
								break;
							}
							else if (waterPos.IndexOf(new Vector2(gameObjectCollider.transform.position.x, gameObjectCollider.transform.position.y - 1)) == -1)
							{
								PlayerManager.Instance.transform.position = gameObjectCollider.transform.position + new Vector3(0, -1, 0);
								break;
							}
						}
					}

					underwater = false;
				}
				else
				{
					GameObject singleCollider;
					try
					{
						singleCollider = _obstacleColliderDictionary[gridPlayerPos];
					}
					catch (Exception temp)
					{
						singleCollider = null;
					}

					if (!singleCollider)
					{
						if (_activeCollders.Count > 0)
						{
							foreach (var collider in _activeCollders)
								collider.SetActive(false);

							_activeCollders.Clear();
						}

						if (!CheckForNecessaryEquipment())
						{
							List<GameObject> listOfGameObjects = _obstacleColliderDictionary.Values.ToList();
							List<float> distanceToGameObjects = new List<float>();

							foreach (var otherCollider in listOfGameObjects)
							{
								float playerDistanceToCollider = Vector2.Distance(playerPos, otherCollider.transform.position);
								distanceToGameObjects.Add(playerDistanceToCollider);
							}

							for (int i = 0; i < 5; i++)
							{
								int closestIndex = distanceToGameObjects.IndexOf(distanceToGameObjects.Min());
								GameObject closestGameObject = listOfGameObjects.ElementAt(closestIndex);
								closestGameObject.SetActive(true);
								_activeCollders.Add(closestGameObject);
								listOfGameObjects.RemoveAt(closestIndex);
								distanceToGameObjects.RemoveAt(closestIndex);
							}
						}
					}
					else if (_activeCollders.IndexOf(singleCollider) == -1)
					{
						if (!EquipmentEffectManager.Instance.CheckEffect(EquipmentEffect.waterWalk) && underwater == false && singleCollider.tag == "Water")
						{
							GoUnderwater();
						}
						else if (!EquipmentEffectManager.Instance.CheckEffect(EquipmentEffect.waterWalk) && underwater == false && singleCollider.tag == "Puddle")
						{
							PlayerManager.Instance.Respawn();
						}
					}
				}
			}
		}
	}

	protected override bool CheckForNecessaryEquipment()
	{
		return EquipmentEffectManager.Instance.CheckEffect(EquipmentEffect.waterWalk) || EquipmentEffectManager.Instance.CheckEffect(EquipmentEffect.walkUnderwater);
	}

	private void GoUnderwater()
	{
		int layer = gameObject.layer;
		string sortingLayer = PlacementManager.ElevationToSortingLayerDictionary[gameObject.layer];
		Vector2 playerPos = PlayerManager.Instance.gameObject.transform.position;
		Vector2 playerMov = PlayerStateMachine.Instance.Movement.normalized;
		Vector2 newPlayerPos = playerPos + (playerMov * 1f);
		PlayerManager.Instance.transform.position = new Vector3(newPlayerPos.x, newPlayerPos.y, 0);
		UnderwaterManager.Instance.SetPreviousLayers(layer, sortingLayer);
		UnderwaterManager.Instance.ToggleUnderwater(true);
		PlayerManager.Instance.SetLayer(6, "Elevation Water");
        StatusEffectsHandler.Instance.IsUnderWater = true;
		PlayerInteraction.Instance.ClearAll();
		underwater = true;
	}

	protected override void EquipmentCheckSucceed(){}
	
	protected override void EquipmentCheckFailed(){}
	
	public bool isUnderwater()
	{
		return underwater;
	}
}
