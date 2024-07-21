using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Botography.Dependencies;

public class InventoryUI : UiCollectionBase
{
	[SerializeField] Text jar1;
	[SerializeField] Text jar2;
	[SerializeField] Text jar3;
	[SerializeField] private List<Slot> _slots;
	[SerializeField] GameObject UI;
	[SerializeField] private Button _helpBubble;
	[SerializeField] private GameObject _helpPopup;
	[SerializeField] private GameObject[] hotbarSlots;
	private bool _helpActive;
	public GameObject defaultDraggable;
	public GameObject muchroom;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void OnEnable()
	{
		InventoryManager.Instance.InventoryEnabled();
	}
	
	void OnDisable()
	{
		InventoryManager.Instance.InventoryDisabled();
		ToggleHelpPopup(false);
	}
	
	public List<GameObject> GetInventoryItems()
	{
		List<GameObject> results = new List<GameObject>();

		foreach (Slot slot in _slots)
		{
			if (slot.gameObject.transform.childCount > 0)
			{
				GameObject item = slot.gameObject.transform.GetChild(0).gameObject;
				if (item != null)
				{
					results.Add(item);
				}
			}
		}

		return results;
	}

	/*void OnEnable()
	{
		DisplayItemsInInventory();
	}
	
	protected void DisplayItemsInInventory()
	{
		string[] itemsList;
		
		itemsList = InventoryManager.instance.GetItemsInInventory();
		
		jar1.text = "Jar1:\n" + itemsList[0];
		jar2.text = "Jar2:\n" + itemsList[1];
		jar3.text = "Jar3:\n" + itemsList[2];
	}

	public void RemoveItemFromInventory(int jar)
	{
		InventoryManager.instance.RemoveItemFromInventory(jar);
		DisplayItemsInInventory();
	}*/
	
	public bool isUIActive()
	{
		return UI.activeSelf;
	}

	public void ToggleHelpPopup()
	{
		_helpActive = !_helpActive;
		_helpPopup.SetActive(_helpActive);
	}

	public void ToggleHelpPopup(bool active)
	{
		_helpActive = active;
		_helpPopup.SetActive(active);
	}
	
	public void ToggleHotbarSlots(bool boolean)
	{
		foreach(GameObject slot in hotbarSlots)
		{
			slot.SetActive(boolean);
		}
	}
}
