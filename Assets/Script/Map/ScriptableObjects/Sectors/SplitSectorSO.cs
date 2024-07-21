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
	public class SplitSectorSO : SectorSO
	{
		public QuadrantSO[] quads = new QuadrantSO[4];

		public override RegionSO GetRegionOfPosition(Vector2 pos)
		{
			if (PositionIsInSector(pos))
			{
				foreach (QuadrantSO quadrant in quads)
				{
					if (pos.x > (TopLeft.x + (0.5 * quadrant.XCoordinate * SECTOR_X_SIZE))
						&& pos.x <= (BottomRight.x + (0.5 * (quadrant.XCoordinate - 1) * SECTOR_X_SIZE))
						&& pos.y > (BottomRight.y - (0.5 * (quadrant.YCoordinate - 1)) * SECTOR_Y_SIZE)
						&& pos.y <= (TopLeft.y - (0.5 * quadrant.YCoordinate * SECTOR_Y_SIZE)))
					{
						return quadrant.Region;
					}
				}
			}

			return null;
		}
	}
}
