using Botography.Notifications;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using static PhysicalStatTile;
using System.Linq;

// Foundation based on "How to store data in tiles (Unity Tilemap + Scriptable Objects)" by 'Shack Man'
//		https://www.youtube.com/watch?v=XIqtZnqutGg

namespace Botography.Map
{
	public class MapManager : MonoBehaviour
	{
		private static MapManager _instance = null;
		[SerializeField]
		private Tilemap _fMap;
		[SerializeField]
		private List<FeatureTile> _fTiles;
		private Dictionary<TileBase, FeatureTile> _tileToFTile;
		[SerializeField]
		private Tilemap _lMap;
		[SerializeField]
		private List<LightStatTile> _lTiles;
		private Dictionary<TileBase, LightStatTile> _tileToLTile;
		[SerializeField]
		private Tilemap _pMap;
		[SerializeField]
		private List<PhysicalStatTile> _pTiles;
		private Dictionary<TileBase, PhysicalStatTile> _tileToPTile;
		[SerializeField] private SectorSO[] _sectors;
		[SerializeField] private RegionSO[] _regions;

		public static MapManager Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new MapManager();
				}
				return _instance;
			}
		}

		private MapManager()
		{

		}

		void Start()
		{
			_tileToPTile = new Dictionary<TileBase, PhysicalStatTile>();
			foreach (var pTile in _pTiles)
			{
				foreach (var tile in pTile.Tiles)
				{
					_tileToPTile.Add(tile, pTile);
					Debug.Log(tile + "" + pTile);
				}
			}
		}

		public SectorSO GetSectorOfPosition(Vector2 pos)
		{
			for (int i = 0; i < _sectors.Length; i++)
			{
				if (_sectors[i].PositionIsInSector(pos))
				{
					return _sectors[i];
				}
			}

			return null;
		}
	}
}
