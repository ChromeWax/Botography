using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscPrototyping : MonoBehaviour 
{
	
	//Auxilary script to quickly prototype main systems, should not be used for anything final
	
	public GameObject inventoryPanel;
	public bool isOpen;
	
    // Start is called before the first frame update
    void Start()
    {
		isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
		/*
		if (Input.GetKeyDown(KeyCode.E))
		{
			if (isOpen)
			{
				CloseInv();
				isOpen = false;
			}
			else
			{
				inventoryPanel.SetActive(true);
				isOpen = true;
			}
		}
		*/
    }
	
	public void CloseInv()
	{
		inventoryPanel.SetActive(false);
	}
}
