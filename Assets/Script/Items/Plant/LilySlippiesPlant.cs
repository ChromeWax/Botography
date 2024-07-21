using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Botography.Player;
using static InventoryConstants;

public class LilySlippiesPlant : EquipmentPlant
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
		EquipmentEffectManager.Instance.ToggleFlagOn(equipEff);
	}
	
	public override void Unequipped()
	{
		EquipmentEffectManager.Instance.ToggleFlagOff(equipEff);
    }
}
