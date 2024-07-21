using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Botography.Dependencies;

public class InventoryManager : ManagerBase<InventorySystem, InventoryUI>
{
	public static InventoryManager Instance { get; private set; }
	//public string[] list;
	
	[SerializeField] GameObject[] hotBarSlots;
	
	/*[SerializeField] InventoryUI inventoryUI;
	[SerializeField] InventorySystem inventorySys;*/
	
	public InventoryManager()
	:base ()
	{
	}

	private void Awake() 
	{
		if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;	
	}

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void InventoryEnabled()
	{
		//Debug.Log("enabled");
		MoveHotbarSlotItemsToInventory();
	}
	
	public void InventoryDisabled()
	{
		//Debug.Log("disabled");
		MoveHandSlotItemsToHotbar();
	}
	
	/*public string[] GetItemsInInventory()
	{
		return list;
	}*/

	public List<GameObject> GetInventoryItems()
	{
		return uiCollection.GetInventoryItems();
	}
	
	/*public void RemoveItemFromInventory(int jar)
	{
		list[jar] = "Empty";
	}*/
	
	public bool addItemToInv(PlantSO plantSO)
	{
		return system.addItemToInv(plantSO, uiCollection.defaultDraggable);
	}

	public bool addMuchroomToInv()
	{
		return system.addMuchroomToInv(uiCollection.muchroom);
	}

	public bool removeMuchroomFromInventory(GameObject muchroom)
	{
		return system.removeMuchroomFromInventory(muchroom);
	}
	
	/*public void toggleInventoryUI()
	{
		if (uiCollection.isUIActive())
		{
			moveHandSlotItemsToHotbar();
		}
		else
		{
			MoveHotbarSlotItemsToInventory();
		}
	}*/

	public bool FillJar(AttributeValue value)
	{
		return system.fillJar(value);
	}

	public void RemoveSampleFromJar(AttributeValue? value, int itemAmount)
	{
		system.RemoveSampleFromJar(value, itemAmount);
	}

	public void RemoveSampleFromJarWithoutAmount(AttributeValue? value)
	{
		system.RemoveSampleFromJarWithoutAmount(value);
	}

	public bool DoesSampleExist(AttributeValue? value)
	{
		return system.DoesSampleExist(value);
	}
	
	private void MoveHandSlotItemsToHotbar()
	{
		List<GameObject> handItems;
		
		handItems = system.getHandItems();
		
		for (int i = 0; i < handItems.Count; i++)
		{
			if (handItems[i] != null)
			{
				handItems[i].transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
				handItems[i].transform.SetParent(hotBarSlots[i].transform);
			}
		}
	}

	public List<Jar> GetJars()
	{
		return system.SampleJars;
	}
	
	private void MoveHotbarSlotItemsToInventory()
	{
		List<GameObject> hotBarItems;
		
		hotBarItems = new List<GameObject>();
		
		foreach (GameObject slot in hotBarSlots)
		{
			if (slot.transform.childCount > 1)
			{
				slot.transform.GetChild(1).transform.localScale = Vector3.one;
				hotBarItems.Add(slot.transform.GetChild(1).gameObject);
			}
			else
			{
				hotBarItems.Add(null);
			}
		}
		
		system.attachToHandSlots(hotBarItems);
	}

	public void RemoveItem(GameObject gameObject)
	{
		if (gameObject.GetComponent<Jar>() != null)
		{
			Jar jar;
			
			jar = gameObject.GetComponent<Jar>();
			RemoveSampleFromJar(jar.Contains, jar.ItemAmount);
		}
		else if (gameObject.GetComponent<InventoryDraggable>()._plantSO.Prefab.GetComponent<MuchroomPlant>() != null)
		{
			removeMuchroomFromInventory(gameObject);
		}
		else
		{
			system.RemoveItemFromInventory(gameObject);
		}
	}
	
	public int PanaceanPlantCount()
	{
		return system.PanaceanPlantCount();
	}

	public void PickUpPanaceanPlant(PanaceanPlantSO panPlant, GameObject panPlantObject)
	{
		system.PickUpPanaceanPlant(panPlant, panPlantObject);
	}
	
	public GameObject GetPanacean()
	{
		Debug.Log("HJGUYGGUGHKYGUGU");
		return system.GetPanacean();
	}

	public void ToggleHelpPopup()
	{
		uiCollection.ToggleHelpPopup();
	}

	public void ToggleHelpPopup(bool active)
	{
		uiCollection.ToggleHelpPopup(active);
	}
	
	public void ToggleHotbarSlots(bool boolean)
	{
		uiCollection.ToggleHotbarSlots(boolean);
	}
}
