using UnityEngine;
using static InventoryConstants;

public interface IInventoryItem 
{
	GameObject GameObject { get;}

	bool IsOfInventoryType(InventoryItemType type);
}
