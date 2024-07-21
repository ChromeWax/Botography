using Botography.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Botography.Map
{
	/// <summary>
	/// A quadrant of a sector. Used when a sector has multiple regions within it.
	/// </summary>
	[CreateAssetMenu]
	public class QuadrantSO : ScriptableObject
	{
		// X and Y coordinates are either 0 or 1, with the top left
		// quadrant being (0, 0) and the bottom right quadrant being
		// (1, 1)
		[SerializeField] private int _xCoordinate;
		[SerializeField] private int _yCoordinate;
		[SerializeField] private RegionSO _region;

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

		public RegionSO Region
		{
			get { return _region; }
		}
		#endregion Properties
	}
}
