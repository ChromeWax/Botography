using Botography.Notifications;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Botography.OverworldInteraction
{
	public abstract class OWIManager : MonoBehaviour
	{
		[SerializeField]
		protected GameObject playerController;
		[SerializeField]
		protected GameObject inventorySystem;
		[SerializeField] 
		protected Notifyer notifyer;
		
		[SerializeField] 
		protected float interactionRadius;
		protected bool isInitialized = false;		//make sure this is not shared
		
		
		
		// Offset now make sures plant is placed in center of grid cell
		static public Vector3 V3ItoV3(Vector3Int V3I, float xOffset = .5f, float yOffset = .5f, float zOffset = 0f){
			return new Vector3( V3I.x+xOffset, V3I.y+yOffset, V3I.z+zOffset);
		}
		
		static public Vector3Int getTile(float x, float y, float z = 0f){
			int gridPosX = RoundToTile(x);
			int gridPosY = RevRoundToTile(y);
			int gridPosZ = RoundToTile(z);
			return new Vector3Int (gridPosX, gridPosY, gridPosZ);
		}
		//functions that may help make code more readable
		static public Vector3Int getTile(Vector3 pos){ return getTile(pos.x, pos.y, pos.z); }
		static public Vector3Int getTile(Vector2 pos){ return getTile(pos.x, pos.y); }
		
		static private int RoundToTile(double num)
		{
			if (num < 0)
			{
				num = Math.Ceiling(num);
			}
			else if (num > 0)
			{
				num = Math.Floor(num);
			}

			return (int)num;
		}

		static private int RevRoundToTile(double num)
		{
			if (num > 0)
			{
				num = Math.Ceiling(num);
			}
			else if (num < 0)
			{
				num = Math.Floor(num);
			}

			return (int)num;
		}
	}
}
