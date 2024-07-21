using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MapConstants;

namespace Botography.Map
{
	/// <summary>
	/// Contains the coordinates and real-world positional infromational of a sector.
	/// </summary>
	public abstract class SectorSO : ScriptableObject
	{
		[SerializeField] private int _xCoordinate;
		[SerializeField] private int _yCoordinate;
		[SerializeField] private bool _isDiscovered;

		#region Properties
		public int XCoordinate
		{
			get
			{
				return _xCoordinate;
			}
			set
			{
				_xCoordinate = value;
			}
		}

		public int YCoordinate
		{
			get
			{
				return (_yCoordinate);
			}
			set
			{
				_yCoordinate = value;
			}
		}

		public Vector2 TopLeft
		{
			get
			{
				return new Vector2((_xCoordinate * SECTOR_X_SIZE) - 0.5f, (_yCoordinate * SECTOR_Y_SIZE) + 0.5f);
			}
		}

		public Vector2 BottomRight
		{
			get
			{
				return new Vector2(((_xCoordinate + 1) * SECTOR_X_SIZE) - 0.5f, ((_yCoordinate - 1) * SECTOR_Y_SIZE) + 0.5f);
			}
		}

		public bool IsDiscovered
		{
			get { return _isDiscovered; }
			set { _isDiscovered = value; }
		}
		#endregion Properties

		public bool PositionIsInSector(Vector2 pos)
		{
			return pos.x <= BottomRight.x
				&& pos.x > TopLeft.x
				&& pos.y > BottomRight.y
				&& pos.y <= TopLeft.y;
		}

		public abstract RegionSO GetRegionOfPosition(Vector2 pos);
	}
}
