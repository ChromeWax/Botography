using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Botography.Player.Dialogue;
using Botography.SaveManagerNS;
using System;

public class Tutorial : MonoBehaviour
{
	[SerializeField]
	private GameObject _saveManager;
    public static Tutorial Instance { get; private set; }
    public static bool TutorialActive
    {
        get
        {
            return Instance != null && Instance.gameObject.activeInHierarchy;
        }
    }
    public bool _almanacCollected, _labCollected, _inventoryCollected, _mapCollected, _seedBagCollected;
    public GameObject _almanac, _lab, _inventory, _map, _seedBag;
    bool _toggleUI;

    DialoguePlayer _dialoguePlayer;
    
    void Awake()
    {
		try
		{
			if(SaveManager.loadGame == true)
				return;
		}
		catch {}
		
        if (Instance == null)
		{
			Instance = this;
		}
        else{
            Instance = null;
            Destroy(this);
        }

        _almanacCollected=false;
        _mapCollected=false;
        _labCollected=false;
        _inventoryCollected=false;
		_seedBagCollected=false;
        _almanac.SetActive(false);
        _lab.SetActive(false);
        _inventory.SetActive(false);
        _map.SetActive(false);
        _seedBag.SetActive(false);
    }

    private void Update()
    {
        if (transform.childCount == 0)
		//if (_almanacCollected && _labCollected && _inventoryCollected && _mapCollected && _seedBagCollected)
		{
			//Debug.Log("all UI Collected");
			gameObject.SetActive(false);
			tutorialCompleted();
		}     
    }

    public void BeginTutorial()
    { 
		_toggleUI = false;
		_dialoguePlayer = DialoguePlayer.Instance;
		_dialoguePlayer.PlayConvo("Tutorial");
	}

    public void TriggerDialogue(string name, Action callback){
        switch (name){
            case "Inventory": { 
                //Debug.Log("inventory collected");
                _dialoguePlayer.PlayConvo("Inventory Collected", callback);
                _inventoryCollected=true;
                break;
            }
            case "Map": { 
                //Debug.Log("map collected");
                _dialoguePlayer.PlayConvo("Map Collected", callback);
                _mapCollected=true;
                break;
            }
            case "Almanac": { 
                //Debug.Log("almanac collected");
                _dialoguePlayer.PlayConvo("Almanac Collected", callback);
                _almanacCollected=true;

                break;
            }
            case "Lab": { 
                //Debug.Log("lab collected");
                _dialoguePlayer.PlayConvo("Lab Collected", callback);
                _labCollected=true;
                break;
            }
			case "bttn_Sack": {
				_dialoguePlayer.PlayConvo("Seed Bag", callback);
				_seedBagCollected=true;
				break;
			}
        }
	}

	private void tutorialCompleted()
    {
		_saveManager.GetComponent<SaveManager>().tutorialFinished();
	}
}
