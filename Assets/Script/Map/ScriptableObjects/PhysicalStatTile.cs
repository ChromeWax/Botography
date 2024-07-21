using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BaseStatTile;

public enum TileType{
	Ground,
	Water
}

public enum AttributeValue{
	None    = -5,
	Silt	= -4,
	Sand	= -3,
	Loam	= -2,
	Clay	= -1, 
	Acidic 	= 0,
	Neutral	= 1,
	Basic	= 2
}

[CreateAssetMenu]
public class PhysicalStatTile : BaseStatTile
{
	public TileType Type;
	public AttributeValue Attribute;
}
