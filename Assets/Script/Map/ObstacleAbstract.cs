using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObstacleAbstract : MonoBehaviour
{
	[SerializeField] protected EquipmentEffect effToCheck;
	
    protected virtual void OnTriggerStay2D(Collider2D collider)
	{
		if (collider.gameObject.CompareTag("Player"))
		{
			if (CheckForNecessaryEquipment())
			{
				EquipmentCheckSucceed();
			}
			else
			{
				EquipmentCheckFailed();
			}
		}
	}
	
	protected virtual bool CheckForNecessaryEquipment()
	{
		return EquipmentEffectManager.Instance.CheckEffect(effToCheck);
	}
	
	protected abstract void EquipmentCheckSucceed();
	
	protected abstract void EquipmentCheckFailed();
}
