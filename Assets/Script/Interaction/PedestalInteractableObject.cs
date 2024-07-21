using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestalInteractableObject : InteractableObject
{
	[SerializeField] GameObject displayLocation;
	
	public void PlacePanacean(GameObject plant)
	{
		plant.transform.position = displayLocation.transform.position;
		plant.transform.SetParent(gameObject.transform);
		GetComponent<CircleCollider2D>().enabled = false;
		plant.transform.GetChild(0).gameObject.SetActive(false);
        plant.GetComponent<CircleCollider2D>().enabled = false;
        plant.GetComponent<CircleCollider2D>().enabled = false;
		plant.GetComponent<SpriteRenderer>().sortingOrder = 11;
    }
}
