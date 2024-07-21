using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BaseStatTile;

/*
public enum LightLevel{
	FullyShade 	= 0,
	MostlyShade	= 1,
	Equal		= 2,
	MostlySun	= 3,
	FullySun	= 4
}
*/

[CreateAssetMenu]
public class LightStatTile : BaseStatTile
{
	public SunlightLevel Level;
}
