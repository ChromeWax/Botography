using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MapConstants;

namespace Botography.Map
{
	/// <summary>
	/// Contains the coordinates and real-world positional infromational of a sector.
	/// </summary>
	[CreateAssetMenu]
	public class SolidSectorSO : SectorSO
	{
		[SerializeField] private RegionSO _region;

		public RegionSO Region
		{
			get { return _region; }
			set { _region = value; }
		}

		public override RegionSO GetRegionOfPosition(Vector2 pos)
		{
			if (PositionIsInSector(pos))
			{
				return _region;
			}

			return null;
		}
	}
}
