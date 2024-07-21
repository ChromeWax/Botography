using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalRunaway : MonoBehaviour
{
	[SerializeField] private Animator animator;
	[SerializeField] private GameObject shadow;
	private bool ranAway = false;
	public float timeBeforePositionResetAfterPlayerLeavesCollider;
	
	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.CompareTag("Player") && !ranAway)
		{
			Runaway();
		}
	}
	
	private void OnTriggerExit2D(Collider2D col)
	{
		if (col.gameObject.CompareTag("Player") && ranAway)
		{
			Invoke("ResetPos", timeBeforePositionResetAfterPlayerLeavesCollider);
		}
	}
	
	private void Runaway()
	{
		if (shadow != null)
			shadow.SetActive(false);
		animator.SetBool("Runaway", true);
		ranAway = true;
	}
	
	private void ResetPos()
	{
		if (shadow != null)
			shadow.SetActive(true);
		animator.SetBool("Runaway", false);
		ranAway = false;
	}
}
