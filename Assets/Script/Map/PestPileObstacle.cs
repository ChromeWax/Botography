using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Botography.Player.StatusEffects;
using Botography.Player;

public class PestPileObstacle : ObstacleAbstract
{	
	[SerializeField] private GameObject obstacleCollider;
	private bool entered = false;

	/*
	protected void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
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
	*/

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.CompareTag("Player"))
		{
			if (gameObject.GetComponent<BoxCollider2D>().isTrigger == true)
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
	}

	protected override void OnTriggerStay2D(Collider2D collider)
	{
		if (collider.gameObject.CompareTag("Player"))
		{
			if (gameObject.GetComponent<BoxCollider2D>().isTrigger == true)
			{
				if (!CheckForNecessaryEquipment() && entered == true)
				{
					PlayerManager.Instance.Respawn();
					obstacleCollider.SetActive(false);
					entered = false;
				}
			}	
		}
	}

	private void OnTriggerExit2D(Collider2D collider)
	{
		if (collider.tag == "Player")
		{
			obstacleCollider.SetActive(false);
			entered = false;
		}
	}
	
	protected override void EquipmentCheckSucceed()
	{
		/*
		BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
		if (collider.isTrigger == false)
		{
			collider.isTrigger = true;
		}
		*/

		obstacleCollider.SetActive(false);
		entered = true;
	}
	
	protected override void EquipmentCheckFailed()
	{
		/*
		gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
		*/

		obstacleCollider.SetActive(true);
		entered = false;
	}
}
