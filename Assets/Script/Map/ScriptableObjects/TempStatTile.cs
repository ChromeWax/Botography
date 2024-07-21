using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BaseStatTile;

public enum TempLevel{
	cold = -2,
	cool = -1,
	nice = 0,
	warm = 1,
	hot = 2
}

[CreateAssetMenu]
public class TempStatTile : BaseStatTile
{
	public TempLevel Level;
}
