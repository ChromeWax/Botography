using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Botography.Player;
using Botography.Player.StatusEffects;
using Unity.VisualScripting;
using static Botography.Player.PlayerConstants;

public class UnderwaterObstacle : ObstacleAbstract
{
    public static UnderwaterObstacle Instance { get; private set; }

    public static bool entered = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    protected void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.tag != "Player") return;

        UnderwaterManager.Instance.ToggleUnderwater(false);
		PlayerManager.Instance.SetLayer(UnderwaterManager.Instance.GetPrevLayer(), UnderwaterManager.Instance.GetPrevSortingLayer());
        entered = false;
        StatusEffectsHandler.Instance.IsUnderWater = false;
        /*
        if (entered)
        {
        }
        else
            entered = true;
        */
	}

    protected override void OnTriggerStay2D(Collider2D collider)
    {

    }

    protected override void EquipmentCheckFailed()
    {
		StatusEffectsHandler.Instance.CanBreatheUnderWater = false;
    }

    protected override void EquipmentCheckSucceed()
    {
    }

    private void Update()
    {
        //Debug.Log(entered);
    }
	
	public bool hasEntered(){
		return entered;
	}
}
