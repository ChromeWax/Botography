using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentEffectManager : MonoBehaviour
{
	public static EquipmentEffectManager Instance;
	
	[SerializeField] private EquipmentEffect _currentEquipEff;
	
    void Start()
	{
		if (Instance != null)
		{
			Destroy(this);
		}
		else
		{
			Instance = this;
		}
		
		_currentEquipEff = EquipmentEffect.None;
	}
	
	public bool CheckEffect(EquipmentEffect effToCheck)
	{
		return _currentEquipEff.HasFlag(effToCheck);
	}
	
	public void ToggleFlagOn(EquipmentEffect equipEff)
	{
		_currentEquipEff = _currentEquipEff | equipEff;
	}
	
	public void ToggleFlagOff(EquipmentEffect equipEff)
	{
		_currentEquipEff = _currentEquipEff & ~equipEff;
	}
	
	public EquipmentEffect getCurrentEffect()
	{
		return _currentEquipEff;
	}
}
