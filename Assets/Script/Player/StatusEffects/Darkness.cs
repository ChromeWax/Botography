using Botography.Player.StatusEffects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Botography.OverworldInteraction;
using Botography.Player;
using UnityEngine.Tilemaps;

public class Darkness : MonoBehaviour
{
	[SerializeField] private Tilemap darknessTilemap;

	/*
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag.Equals("Player"))
		{
			StatusEffectsHandler.Instance.IsDark = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag.Equals("Player"))
		{
			StatusEffectsHandler.Instance.IsDark = false;
		}
	}
	*/

    private void Update()
    {
		Vector3Int gridPositionInt = OWIManager.getTile(PlayerManager.Instance.GetCurrentPlayerPosition());
		TileBase darkTile = darknessTilemap.GetTile(gridPositionInt);
		StatusEffectsHandler.Instance.IsDark = darkTile != null;
    }
}
