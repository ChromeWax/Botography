using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InventoryConstants;
using Botography.Player;

public class RoseCuffsPlant : EquipmentPlant
{
	[SerializeField] EquipmentEffect equipEff;
	[SerializeField] private int speedChangeAmount;
	
	public override void EquipmentAbility()
    {
        throw new System.NotImplementedException();
    }
	
    public override bool IsOfInventoryType(InventoryItemType type)
	{
        return type == InventoryItemType.EquippableFeet;
	}
	
	public override void Equipped()
	{
		PlayerManager.Instance.ChangeSpeed(speedChangeAmount);
		EquipmentEffectManager.Instance.ToggleFlagOn(equipEff);
	}
	
	public override void Unequipped()
	{
		PlayerManager.Instance.ChangeSpeed(speedChangeAmount * (-1));
		EquipmentEffectManager.Instance.ToggleFlagOff(equipEff);
	}
}
