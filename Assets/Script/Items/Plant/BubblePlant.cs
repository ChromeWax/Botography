using System.Collections;
using System.Collections.Generic;
using Botography.Player;
using Botography.Player.StatusEffects;
using UnityEngine;
using static InventoryConstants;

public class BubblePlant : EquipmentPlant, IConsumable
{
	[SerializeField] EquipmentEffect equipEff;

    public void ConsumableAbility()
    {
        StatusEffectsHandler.Instance.ClearStatusEffects();
    }

    public override void EquipmentAbility(){}

    public override bool IsOfInventoryType(InventoryItemType type)
    {
        return type == InventoryItemType.EquippableHead;
    }
	
	public override void Equipped()
	{
        StatusEffectsHandler.Instance.CanBreatheUnderWater = true;
		EquipmentEffectManager.Instance.ToggleFlagOn(equipEff);
	}
	
	public override void Unequipped()
	{
        StatusEffectsHandler.Instance.CanBreatheUnderWater = false;
		EquipmentEffectManager.Instance.ToggleFlagOff(equipEff);
	}
}
