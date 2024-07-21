using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Botography.Map
{
	/// <summary>
	/// Contains all of the information for a region.
	/// </summary>
	[CreateAssetMenu]
	public class RegionSO : ScriptableObject
	{
		public SunlightLevel SunlightLevel;
	}
}
