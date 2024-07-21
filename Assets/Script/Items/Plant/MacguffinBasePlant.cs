using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InventoryConstants;

public abstract class MacguffinBasePlant : BasePanaceanPlant, IKeyItem
{
	public bool IsOfInventoryType(InventoryItemType type)
	{
		return type == InventoryItemType.Panacean;
	}
	
	abstract public string getName();
}
