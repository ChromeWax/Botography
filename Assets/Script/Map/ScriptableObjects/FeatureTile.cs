using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BaseStatTile;

public enum Type{
	Boundary,
	Block,
	Bridge
}

[CreateAssetMenu]
public class FeatureTile : BaseStatTile
{
	public Type FeatureType;
}
