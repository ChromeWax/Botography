using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Botography.Dependencies;

public class InventorySystem : SystemBase
{
	//public Dictionary<BasePlant, int> inventoryDict= new Dictionary<BasePlant, int>();
	[SerializeField] private GameObject[] invSlot;
	[SerializeField] private GameObject[] hotBarSlot;
	[SerializeField] private GameObject[] panaceanSlot;
	[SerializeField] private GameObject _osmosisLotusUI;
    [SerializeField] private GameObject _mirrorMossUI;
    [SerializeField] private GameObject _stellaBloomUI;
    [SerializeField] private GameObject _phoenixRoseUI;
    [HideInInspector] public List<Jar> SampleJars;
	
	private void FindAllSampleJars()
	{
		SampleJars.Clear();

		for (int i = 0; i < invSlot.Length; i++)
		{
			if (invSlot[i].transform.childCount > 0 && invSlot[i].transform.GetChild(0).GetComponent<Jar>() != null)
				SampleJars.Add(invSlot[i].transform.GetChild(0).GetComponent<Jar>());
		}
	}

	public bool fillJar(AttributeValue sample)
	{
		FindAllSampleJars();

		Jar emptyJar = null;
		foreach(Jar jar in SampleJars){
			if (emptyJar == null && jar.Contains == null)
			{
				emptyJar = jar;
				continue;
			}

			if (jar.Contains == sample)
			{
				if(jar.ItemAmount < jar.MaxAmount)
				{
					jar.FillJar(sample);
					return true;
				}
			}
		}

		if (emptyJar != null)
		{
			StackableItem jarStack = emptyJar.gameObject.GetComponent<StackableItem>();
			if (jarStack.GetStackTotal() > 1)
			{
				foreach (GameObject slot in invSlot)
				{
					if (slot.transform.childCount == 0)
					{
						GameObject item = Instantiate(jarStack.gameObject);
						item.transform.SetParent(slot.transform);
						item.GetComponent<StackableItem>().SetStackTotal(1);
						item.GetComponent<Jar>().FillEmptyJar(sample);
						jarStack.ChangeStackTotal(-1);
						return true;
					}
				}
			}
			else
				emptyJar.FillEmptyJar(sample);
			return true;
		}

		return false;
	}

	public bool RemoveSampleFromJar(AttributeValue? sample, int itemAmount)
	{
		FindAllSampleJars();

		foreach(Jar jar in SampleJars)
		{
			if (jar.Contains == sample && jar.ItemAmount > 0 && jar.ItemAmount == itemAmount)
			{
				jar.AttemptRemove();
				return true;
			}
		}

		return false;
	}
	
	public bool RemoveSampleFromJarWithoutAmount(AttributeValue? sample)
	{
		FindAllSampleJars();

		foreach(Jar jar in SampleJars)
		{
			if (jar.Contains == sample && jar.ItemAmount > 0)
			{
				jar.AttemptRemove();
				return true;
			}
		}

		return false;
	}


	public bool DoesSampleExist(AttributeValue? sample)
	{
		FindAllSampleJars();

		foreach(Jar jar in SampleJars)
		{
			if (jar.Contains == sample && jar.ItemAmount > 0)
			{
				return true;
			}
		}

		return false;
	}

	
	public bool addItemToInv(PlantSO plant, GameObject defaultDraggable)
	{
		foreach (GameObject slot in invSlot)
		{
			if (slot.transform.childCount == 0)
			{
				GameObject item = Instantiate(defaultDraggable);
        		Image image = item.GetComponent<Image>();
				image.sprite = plant.Artwork;
				InventoryDraggable draggable = item.GetComponent<InventoryDraggable>();
				draggable._plantSO = plant;
				item.transform.SetParent(slot.transform);
				item.transform.localScale = Vector3.one;
				return true;
			}
		}
		return false;
	}

	public bool addMuchroomToInv(GameObject muchroom)
	{
		foreach (GameObject slot in invSlot)
		{
			if (slot.transform.childCount == 0)
			{
				GameObject item = Instantiate(muchroom);
				item.transform.SetParent(slot.transform);
				item.transform.localScale = Vector3.one;
				return true;
			}
		}
		return false;
	}

	public bool removeMuchroomFromInventory(GameObject muchroom)
	{
		FindAllSampleJars();

		foreach (Jar jar in SampleJars)
		{
			try
			{
				if (((MuchroomPlant) jar).gameObject == muchroom)
				{
					SampleJars.Remove(jar);
					Destroy(jar.gameObject);
					return true;
				}
			}
			catch
			{

			}
		}
		return false;
	}

	public List<GameObject> getHandItems()
	{
		List<GameObject> handItems;
		
		handItems = new List<GameObject>();
		
		foreach (GameObject slot in hotBarSlot)
		{
			if (slot.transform.childCount > 1)
			{
				handItems.Add(slot.transform.GetChild(1).gameObject);
			}
			else
			{
				handItems.Add(null);
			}
		}
		return handItems;
	}
	
	public void attachToHandSlots(List<GameObject> itemList)
	{
		for (int i = 0; i < itemList.Count; i++)
		{
			if (itemList[i] != null)
			{
				itemList[i].transform.SetParent(hotBarSlot[i].transform);
			}
		}
	}
	
	public void RemoveItemFromInventory(GameObject gameObject)
	{
		Destroy(gameObject);
	}
	
	public int PanaceanPlantCount()
	{
		for (int i = 0; i < panaceanSlot.Length; i++)
		{
			if (panaceanSlot[i].transform.childCount == 0)
			{
				return i;
			}
		}
		return 4;
	}
	
	public void PickUpPanaceanPlant(PanaceanPlantSO plant, GameObject plantObject)
	{
		GameObject plantToGet;
		int i = 0;
		foreach (GameObject slot in panaceanSlot)
		{
			if(slot.transform.childCount == 0)
			//if (slot.transform.GetChild().IsActive(false) == 0)
			{
				Debug.Log(plant.PlantName);
				//do whatever to add panacean plant to inv here
				if(plant.PlantName == "Stella Bloom")
				{
					Debug.Log("Stella Bloom found!");
					plantToGet = Instantiate(_stellaBloomUI);
                    plantToGet.transform.SetParent(slot.transform);
					Debug.Log("You have acquired " + panaceanSlot[i].transform.GetChild(0).GetComponent<MacguffinBasePlant>().GetPanaceanPlantSO().PlantName);
                }
                if (plant.PlantName == "Mirror Moss")
                {
                    //Debug.Log("Stella Bloom found!");
                    plantToGet = Instantiate(_mirrorMossUI);
                    plantToGet.transform.SetParent(slot.transform);
                    Debug.Log("You have acquired " + panaceanSlot[i].transform.GetChild(0).GetComponent<MacguffinBasePlant>().GetPanaceanPlantSO().PlantName);
                }
                if (plant.PlantName == "Phoenix Rose")
                {
                    //Debug.Log("Stella Bloom found!");
                    plantToGet = Instantiate(_phoenixRoseUI);
                    plantToGet.transform.SetParent(slot.transform);
                    Debug.Log("You have acquired " + panaceanSlot[i].transform.GetChild(0).GetComponent<MacguffinBasePlant>().GetPanaceanPlantSO().PlantName);
                }
                if (plant.PlantName == "Osmosis Lotus")
                {
                    //Debug.Log("Stella Bloom found!");
                    plantToGet = Instantiate(_osmosisLotusUI);
                    plantToGet.transform.SetParent(slot.transform);
                    Debug.Log("You have acquired " + panaceanSlot[i].transform.GetChild(0).GetComponent<MacguffinBasePlant>().GetPanaceanPlantSO().PlantName);
                }
                return;
			}
			i++;
		}
	}
	
	public GameObject GetPanacean()
	{
		GameObject panacean;
		
		for (int i = panaceanSlot.Length - 1; i > -1; i--)
		{
			if (panaceanSlot[i].transform.childCount >= 1)
			{
				panacean = Instantiate(panaceanSlot[i].transform.GetChild(0).GetComponent<MacguffinBasePlant>().GetPanaceanPlantSO().Prefab);
				Debug.Log("You have placed " + panaceanSlot[i].transform.GetChild(0).GetComponent<MacguffinBasePlant>().GetPanaceanPlantSO().PlantName + " that was in slot " + i);
				Destroy(panaceanSlot[i].transform.GetChild(0).gameObject);
				return panacean;
			}
		}
		return null;
	}
}
