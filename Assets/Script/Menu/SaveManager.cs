using Botography.Dependencies;
using Botography.Player;
using Botography.Player.Dialogue;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace Botography.SaveManagerNS
{
	public class SaveManager : MonoBehaviour
	{
		[SerializeField]
		private GameObject _almanacObj;
		private AlmanacManager _almanacRef;
		[SerializeField]
		private GameObject _noteRefObj;
		private NoteReference _noteRef;
		[SerializeField]
		private GameObject _meltRefObj;
		private MeltReference _meltRef;
		[SerializeField]
		private GameObject _dialRefObj;
		private DialReference _dialRef;
		[SerializeField]
		private GameObject _eemObj;
		private EquipmentEffectManager _eemRef;
		[SerializeField]
		private GameObject _invMainObj;
		[SerializeField]
		private GameObject _invHandObj;
		[SerializeField]
		private GameObject _invEquipObj;
		[SerializeField]
		private GameObject _invPanObj;
		[SerializeField]
		private GameObject _placementObj;
		private PlacementManager _placementRef;
		[SerializeField]
		private GameObject _underwaterObj;
		[SerializeField]
		private GameObject _waterObj1;
		[SerializeField]
		private GameObject _waterObj2;
		[SerializeField]
		private GameObject _dialogueObj;
		private DialoguePlayer _dialogueRef;
		[SerializeField]
		private GameObject _markerObj;
		[SerializeField]
		private GameObject _sectorObj;
		[SerializeField]
		private GameObject _tutorialObj;
		[SerializeField]
		private GameObject _ppmObj;
		[SerializeField]
		private GameObject _defaultDraggable;
		
		[SerializeField]
		private GameObject _pedestalTL;
		[SerializeField]
		private GameObject _pedestalTR;
		[SerializeField]
		private GameObject _pedestalBL;
		[SerializeField]
		private GameObject _pedestalBR;
		[SerializeField]
		private GameObject _lanternObj;
		[SerializeField]
		private GameObject _fernaceObj;
		[SerializeField]
		private GameObject _spearmintObj;
		[SerializeField]
		private GameObject _bBomberObj;
		[SerializeField]
		private GameObject _bBomberAnchorObj;
		[SerializeField]
		private GameObject _pipevineObj;
		[SerializeField]
		private GameObject _growcapObj;
		[SerializeField]
		private GameObject _bubbleObj;
		[SerializeField]
		private GameObject _lilyObj;
		[SerializeField]
		private GameObject _muchroomObj;
		[SerializeField]
		private GameObject _pitcherObj;
		[SerializeField]
		private GameObject _roseObj;
		[SerializeField]
		private GameObject _ev1;
		[SerializeField]
		private GameObject _ev2;
		[SerializeField]
		private GameObject _ev3;
		
		[SerializeField]
		private GameObject _mirrorObj;
		[SerializeField]
		private GameObject _osmosisObj;
		[SerializeField]
		private GameObject _phoenixObj;
		[SerializeField]
		private GameObject _prototypeObj;
		[SerializeField]
		private GameObject _stellaObj;

		[SerializeField]
		private GameObject _mirrorUI;
		[SerializeField]
		private GameObject _osmosisUI;
		[SerializeField]
		private GameObject _phoenixUI;
		[SerializeField]
		private GameObject _stellaUI;
		
		[SerializeField]
		private GameObject _jarInvObj;
		[SerializeField]
		private GameObject _muchjarInvObj;
		[SerializeField]
		private PlantSO _lanternSO;
		[SerializeField]
		private PlantSO _fernaceSO;
		[SerializeField]
		private PlantSO _spearmintSO;
		[SerializeField]
		private PlantSO _bBomberSO;
		[SerializeField]
		private PlantSO _pipevineSO;
		[SerializeField]
		private PlantSO _growcapSO;
		[SerializeField]
		private PlantSO _bubbleSO;
		[SerializeField]
		private PlantSO _lilySO;
		[SerializeField]
		private PlantSO _muchroomSO;
		[SerializeField]
		private PlantSO _pitcherSO;
		[SerializeField]
		private PlantSO _roseSO;
		
		public static bool loadGame;
		private bool tutorialActive;
		
		public void Start()
		{
			try
			{
Debug.Log("SaveManager: Load? " + loadGame);
				if(loadGame == true)
					load();
				else
					tutorialActive = true;
			}
			catch
			{
Debug.Log("SaveManager: catch :(");
				tutorialActive = true;
			}
		}
		
		public void save()
		{
			try
			{
				if(tutorialActive)
				{
					Debug.Log("SaveManager: Tutorial Incomplete\n" );
					return;
				}
				SaveFile save = new SaveFile();
			
				_almanacRef = _almanacObj.GetComponent<AlmanacManager>();
				_noteRef = _noteRefObj.GetComponent<NoteReference>();
				_meltRef = _meltRefObj.GetComponent<MeltReference>();
				_dialRef = _dialRefObj.GetComponent<DialReference>();
				_eemRef = _eemObj.GetComponent<EquipmentEffectManager>();
				_placementRef = _placementObj.GetComponent<PlacementManager>();
				_dialogueRef = _dialogueObj.GetComponent<DialoguePlayer>();
				
				save.norPDiscovered = saveNorPInfo(_almanacRef.getPlantsDiscovered());
				save.panPDiscovered = savePanPInfo(_almanacRef.getPanaceanDiscovered());
				save.noteInd = _noteRef.getRMList();
				save.meltInd = _meltRef.getRMList();
				save.dialInd = _dialRef.getRMList();
				save.currentEquipEff = _eemRef.getCurrentEffect();
				(save.plantCoordinates, save.beanstalkToAnchorList) = savePlaced(_placementObj.GetComponent<PlacementManager>().getObjCoordinates());
				//save.hasEntered = _underwaterObj.GetComponent<UnderwaterObstacle>().hasEntered();		//obj does not have this class. NOTHING has this class
				save.isUnderwater1 = _waterObj1.GetComponent<WaterObstacle>().isUnderwater();
				save.isUnderwater2 = _waterObj2.GetComponent<WaterObstacle>().isUnderwater();
				save.panaceanPlantsPlaced = PlayerInteraction.Instance._panaceanPlantsPlaced;
				
				save.invMain = compInvInfo(_invMainObj.transform, 0);
				save.invHand = compInvInfoForSlotWithImage(_invHandObj.transform, 0, 3);
				save.invEquip = compInvInfoForSlotWithImage(_invEquipObj.transform, 0);
				save.invPan = compInvInfo(_invPanObj.transform, 0, 5);	
				
				save.curConvo = _dialogueRef.getCurConvo();
				save.curDialogue = _dialogueRef.getCurDialogue();
				save.isPlaying = _dialogueRef.isPlaying();
				save.waiting = _dialogueRef.getWaiting();
				save.curCallback = _dialogueRef.getCurrCallback();
				save.waitCallback = _dialogueRef.getWaitCallback();
				
				save.panTL = getPanPlant(_pedestalTL);
				save.panTR = getPanPlant(_pedestalTR);
				save.panBL = getPanPlant(_pedestalBL);
				save.panBR = getPanPlant(_pedestalBR);
				
				Vector2 playerPos = PlayerManager.Instance.GetCurrentPlayerPosition();
				save.xPos = playerPos.x;
				save.yPos = playerPos.y;
				save.playerLayer = PlayerManager.Instance.GetCurrentLayer();
				
				save.visitedSectors = _sectorObj.GetComponent<HandleSectors>().getVisitedSectors();
				save.markers = _markerObj.GetComponent<MarkerHandler>().getMarkersPlaced();
				save.processType = _ppmObj.GetComponent<PostProcessManager>().GetCurrentProcessType();
				
				string saveRaw = JsonConvert.SerializeObject(save, Formatting.Indented, new JsonSerializerSettings
					{
						ReferenceLoopHandling = ReferenceLoopHandling.Ignore
					});
				File.WriteAllText("BotoSave.json", saveRaw);
				Debug.Log("SaveManager: Save Succeeded\n" );
			}
			catch
			{
				Debug.Log("SaveManager: Save failed\n" );
				return;
			}
		}
		
		public void loadSaveBtn()
		{
			load();
		}
		
		public void load()
		{
			try
			{
				_almanacRef = _almanacObj.GetComponent<AlmanacManager>();
				_noteRef = _noteRefObj.GetComponent<NoteReference>();
				_meltRef = _meltRefObj.GetComponent<MeltReference>();
				_dialRef = _dialRefObj.GetComponent<DialReference>();
				_eemRef = _eemObj.GetComponent<EquipmentEffectManager>();
				_placementRef = _placementObj.GetComponent<PlacementManager>();
				_dialogueRef = _dialogueObj.GetComponent<DialoguePlayer>();
			
				string saveRaw = File.ReadAllText("BotoSave.json");
				SaveFile save = JsonConvert.DeserializeObject<SaveFile>(saveRaw);
				
				for (int i=0; i<save.norPDiscovered.Count; i++)
				{
					_almanacRef.SetPlantDiscoveryStatus(i, save.norPDiscovered[i].discovered, save.norPDiscovered[i].r1Discovered, save.norPDiscovered[i].r2Discovered);
				}
				for (int i=0; i<save.panPDiscovered.Count; i++)
				{
					_almanacRef.SetPanaceanPlantDiscoveryStatus(i, save.panPDiscovered[i].discovered);
				}
				save.noteInd.Sort((a, b) => b.CompareTo(a));		//Sorts in descending order
				foreach (int ind in save.noteInd){
					GameObject noteObj = _noteRef.getNoteObj(ind);
					if (noteObj == null)
						continue;
					_noteRef.rmNote(noteObj);
					_almanacRef.AddNote(noteObj.GetComponent<NoteInteractableObject>().Note);
					Destroy(noteObj);
				}
				save.meltInd.Sort((a, b) => b.CompareTo(a));
				foreach (int ind in save.meltInd){
					GameObject meltObj = _meltRef.getMeltableObj(ind);
					if (meltObj == null)
						continue;
					_meltRef.rmMeltable(meltObj);
					Destroy(meltObj);
				}
				save.dialInd.Sort((a, b) => b.CompareTo(a));
				foreach (int ind in save.dialInd){
					GameObject dialObj = _dialRef.getDialObj(ind);
					if (dialObj == null)
						continue;
					_dialRef.rmDialObj(dialObj);
					Destroy(dialObj);
				}
				if (save.currentEquipEff != null)
					_eemRef.ToggleFlagOn(save.currentEquipEff);

				PlayerInteraction.Instance._panaceanPlantsPlaced = save.panaceanPlantsPlaced;
				
				loadPlacedPlants(save.plantCoordinates, save.beanstalkToAnchorList);
				
				loadInvInfo(save.invMain, _invMainObj.transform, 0);
				loadHandInfo(save.invHand, _invHandObj.transform, 0, 3);
				loadEqpInfo(save.invEquip, _invEquipObj.transform);
				loadInvInfo(save.invPan, _invPanObj.transform, 0, 5);
				
				save.curConvo = _dialogueRef.getCurConvo();
				save.curDialogue = _dialogueRef.getCurDialogue();
				save.isPlaying = _dialogueRef.isPlaying();
				save.waiting = _dialogueRef.getWaiting();
				save.curCallback = _dialogueRef.getCurrCallback();
				save.waitCallback = _dialogueRef.getWaitCallback();
				
				loadPanPlant(_pedestalTL, save.panTL);
				loadPanPlant(_pedestalTR, save.panTR);
				loadPanPlant(_pedestalBL, save.panBL);
				loadPanPlant(_pedestalBR, save.panBR);
				
				Vector3 v = new Vector3(save.xPos, save.yPos, 0);
				PlayerManager.Instance.SetPlayerPosition(v, save.playerLayer, PlacementManager.ElevationToSortingLayerDictionary[save.playerLayer]);
				if (save.isUnderwater1 || save.isUnderwater2 || save.hasEntered)
					PlayerManager.Instance.Respawn();
				
				_sectorObj.GetComponent<HandleSectors>().setVisitedSectors(save.visitedSectors);
				_markerObj.GetComponent<MarkerHandler>().setMarkersPlaced(save.markers);
				_ppmObj.GetComponent<PostProcessManager>().SetCurrentProcessType(save.processType);

				_tutorialObj.SetActive(false);
				
Debug.Log("SaveManager: Load Successful");
				return;
			}
			catch (Exception e)
			{
				Debug.Log(e);
				SceneManager.LoadScene("Main Menu");
				Debug.Log("SaveManager: Load failed\n" );
				return;
			}
		}

	/*----------------------------------------- Helper Functions ----------------------------------------*/

		public void tutorialFinished()
		{
			tutorialActive = false;
		}
		
		//TODO: make return what panacean plant is stored
		private string getPanPlant(GameObject pedestal)
		{
			int len = pedestal.transform.childCount;
			for(int i=0; i<len; i++){
				if( pedestal.transform.GetChild(i).GetComponent<MacguffinBasePlant>() != null )
				{
					return pedestal.transform.GetChild(i).GetComponent<MacguffinBasePlant>().getName();
				}
			}
			return null;
		}
		
		private void loadPanPlant(GameObject pedestalObj, string plantName)
		{
			if (plantName == null)
				return;
			switch (plantName)
			{
				case "mirror":
					pedestalObj.GetComponent<PedestalInteractableObject>().PlacePanacean(Instantiate(_mirrorObj));
					return;
				case "osmosis":
					pedestalObj.GetComponent<PedestalInteractableObject>().PlacePanacean(Instantiate(_osmosisObj));
					return;
				case "phoenix":
					pedestalObj.GetComponent<PedestalInteractableObject>().PlacePanacean(Instantiate(_phoenixObj));
					return;
				case "prototype":
					pedestalObj.GetComponent<PedestalInteractableObject>().PlacePanacean(Instantiate(_prototypeObj));
					return;
				case "stella":
					pedestalObj.GetComponent<PedestalInteractableObject>().PlacePanacean(Instantiate(_stellaObj));
					return;
			}
	Debug.Log("SaveManager: No match (switch)");
		}
		
		private List<SlotInfo> compInvInfo(Transform invRef, int offset, int end = 999)
		{
	//Debug.Log("SaveManager: compInvInfo");
			List<SlotInfo> siList = new List<SlotInfo>();
			int len = Math.Min(invRef.childCount, end);
			Transform slotTrans;
			Transform tTrans;
			//TODO stack empty jars and muchroom as well plants maybe
			for (int i=offset; i<len; i++)
			{
				SlotInfo si = new SlotInfo();
				slotTrans = invRef.GetChild(i);
				if(slotTrans.childCount > 0)
				{
					si.count = 0;
					tTrans = slotTrans.GetChild(0);
	//Debug.Log("SaveManager:\n obj " + tTrans + "\nplant " + tTrans.GetComponent<InventoryDraggable>() + "\njar " + tTrans.GetComponent<Jar>());
					if(tTrans.GetComponent<InventoryDraggable>() != null){
						si.type = tTrans.GetComponent<InventoryDraggable>()._plantSO.PlantName;
						if(si.type == "Muchroom")
						{
							si.attribute = tTrans.GetComponent<MuchroomPlant>().Contains;
							if(si.attribute != null)
							{
								si.count = tTrans.GetComponent<MuchroomPlant>().ItemAmount;
							}
							else
								si.count = tTrans.GetComponent<StackableItem>().GetStackTotal();
						}
						else
							si.count = tTrans.GetComponent<StackableItem>().GetStackTotal();
					}
					else if(tTrans.GetComponent<Jar>() != null)
					{
						if (tTrans.GetComponent<MuchroomPlant>() != null)
							si.type = "Muchroom";
						else
							si.type = "jar";
						si.attribute = tTrans.GetComponent<Jar>().Contains;
						if(si.attribute != null)
						{
							si.count = tTrans.GetComponent<Jar>().ItemAmount;
						}
						else
							si.count = tTrans.GetComponent<StackableItem>().GetStackTotal();
					}
					else if(tTrans.GetComponent<MacguffinBasePlant>() != null)
					{
						si.type = tTrans.GetComponent<MacguffinBasePlant>().getName();
					}
				}
				siList.Add(si);
			}
			printList(siList);
			return siList;
		}
		private List<SlotInfo> compInvInfoForSlotWithImage(Transform invRef, int offset, int end = 999)
		{
	//Debug.Log("SaveManager: compInvInfo");
			List<SlotInfo> siList = new List<SlotInfo>();
			int len = Math.Min(invRef.childCount, end);
			Transform slotTrans;
			Transform tTrans;
			//TODO stack empty jars and muchroom as well plants maybe
			for (int i=offset; i<len; i++)
			{
				SlotInfo si = new SlotInfo();
				slotTrans = invRef.GetChild(i);
				if(slotTrans.childCount > 1)
				{
					si.count = 0;
					tTrans = slotTrans.GetChild(1);
	//Debug.Log("SaveManager:\n obj " + tTrans + "\nplant " + tTrans.GetComponent<InventoryDraggable>() + "\njar " + tTrans.GetComponent<Jar>());
					if(tTrans.GetComponent<InventoryDraggable>() != null){
						si.type = tTrans.GetComponent<InventoryDraggable>()._plantSO.PlantName;
						if(si.type == "Muchroom")
						{
							si.attribute = tTrans.GetComponent<Jar>().Contains;
							if(si.attribute != null)
							{
								si.count = tTrans.GetComponent<Jar>().ItemAmount;
							}
							else
								si.count = tTrans.GetComponent<StackableItem>().GetStackTotal();
						}
						else
							si.count = tTrans.GetComponent<StackableItem>().GetStackTotal();
					}
					else if(tTrans.GetComponent<Jar>() != null){
						si.type = "jar";
						si.attribute = tTrans.GetComponent<Jar>().Contains;
						if(si.attribute != null)
						{
							si.count = tTrans.GetComponent<Jar>().ItemAmount;
						}
						else
							si.count = tTrans.GetComponent<StackableItem>().GetStackTotal();
					}
				}
				siList.Add(si);
			}
			printList(siList);
			return siList;
		}

		
		private void loadEqpInfo(List<SlotInfo> invInfo, Transform invRef)
		{
			Transform slotTrans;
			Transform tTrans;
			for (int i=0; i<3; i++)
			{
				tTrans = null;
				if(invInfo[i].type == null)
					continue;
				slotTrans = invRef.GetChild(i);
				switch (invInfo[i].type)
				{
					case "Bubble Bulb":
						tTrans = createInvDraggable(_bubbleSO).transform;
						tTrans.GetComponent<StackableItem>().SetStackTotal(invInfo[i].count);
						tTrans.SetParent(slotTrans);
						break;
					case "Lily Slippies":
						tTrans = createInvDraggable(_lilySO).transform;
						tTrans.GetComponent<StackableItem>().SetStackTotal(invInfo[i].count);
						tTrans.SetParent(slotTrans);
						break;
					case "Rose Fringe":
						tTrans = createInvDraggable(_roseSO).transform;
						tTrans.GetComponent<StackableItem>().SetStackTotal(invInfo[i].count);
						tTrans.SetParent(slotTrans);
						break;
				}
				tTrans.GetComponent<InventoryDraggable>()._plantSO.Prefab.GetComponent<EquipmentPlant>().Equipped();
			}
		}
		
		private void loadInvInfo(List<SlotInfo> invInfo, Transform invRef, int offset, int end = 999)
		{
			int len = Math.Min(invRef.childCount, end);
			Transform slotTrans;
			Transform tTrans;
			PlantSO pso;
			for (int i=offset; i<len; i++)
			{
				tTrans = null;
				if(invInfo[i-offset].type == null)
					continue;
				slotTrans = invRef.GetChild(i);
				if(slotTrans.childCount > 0)
				{
					for (int j=slotTrans.childCount; j>=0; j--)
						Destroy(slotTrans.GetChild(0).gameObject);
				}
				switch (invInfo[i-offset].type)
				{
					case "jar":
						tTrans = Instantiate(_jarInvObj).transform;
						tTrans.GetComponent<Jar>().Contains = invInfo[i-offset].attribute;
						if(invInfo[i-offset].attribute == null)
							tTrans.GetComponent<StackableItem>().SetStackTotal(invInfo[i-offset].count);
						else
							tTrans.GetComponent<Jar>().ItemAmount = invInfo[i-offset].count;
						tTrans.SetParent(slotTrans);
						break;
					case "Muchroom":
						tTrans = Instantiate(_muchjarInvObj).transform;
						tTrans.GetComponent<MuchroomPlant>().Contains = invInfo[i-offset].attribute;
						if(invInfo[i-offset].attribute == null)
							tTrans.GetComponent<StackableItem>().SetStackTotal(invInfo[i-offset].count);
						else
							tTrans.GetComponent<MuchroomPlant>().ItemAmount = invInfo[i-offset].count;
						tTrans.SetParent(slotTrans);
						break;

					case "Lantern":
						tTrans = createInvDraggable(_lanternSO).transform;
						tTrans.GetComponent<StackableItem>().SetStackTotal(invInfo[i-offset].count);
						tTrans.SetParent(slotTrans);
						break;
					case "Fernace":
						tTrans = createInvDraggable(_fernaceSO).transform;
						tTrans.GetComponent<StackableItem>().SetStackTotal(invInfo[i-offset].count);
						tTrans.SetParent(slotTrans);
						break;
					case "Spearmint":
						tTrans = createInvDraggable(_spearmintSO).transform;
						tTrans.GetComponent<StackableItem>().SetStackTotal(invInfo[i-offset].count);
						tTrans.SetParent(slotTrans);
						break;
					case "Beanstalk Bomber":
						tTrans = createInvDraggable(_bBomberSO).transform;
						tTrans.GetComponent<StackableItem>().SetStackTotal(invInfo[i-offset].count);
						tTrans.SetParent(slotTrans);
						break;
					case "Pipevine":
						tTrans = createInvDraggable(_pipevineSO).transform;
						tTrans.GetComponent<StackableItem>().SetStackTotal(invInfo[i-offset].count);
						tTrans.SetParent(slotTrans);
						break;
					case "Growcap":
						tTrans = createInvDraggable(_growcapSO).transform;
						tTrans.GetComponent<StackableItem>().SetStackTotal(invInfo[i-offset].count);
						tTrans.SetParent(slotTrans);
						break;
					case "Bubble Bulb":
						tTrans = createInvDraggable(_bubbleSO).transform;
						tTrans.GetComponent<StackableItem>().SetStackTotal(invInfo[i-offset].count);
						tTrans.SetParent(slotTrans);
						break;
					case "Lily Slippies":
						tTrans = createInvDraggable(_lilySO).transform;
						tTrans.GetComponent<StackableItem>().SetStackTotal(invInfo[i-offset].count);
						tTrans.SetParent(slotTrans);
						break;
						/*
					case "Muchroom":
						tTrans = createInvDraggable(_muchroomSO).transform;
						tTrans.GetComponent<StackableItem>().SetStackTotal(invInfo[i-offset].count);
						tTrans.SetParent(slotTrans);
						break;
						*/
					case "Pitcher":
						tTrans = createInvDraggable(_pitcherSO).transform;
						tTrans.GetComponent<StackableItem>().SetStackTotal(invInfo[i-offset].count);
						tTrans.SetParent(slotTrans);
						break;
					case "Rose Fringe":
						tTrans = createInvDraggable(_roseSO).transform;
						tTrans.GetComponent<StackableItem>().SetStackTotal(invInfo[i-offset].count);
						tTrans.SetParent(slotTrans);
						break;
					case "mirror":
						tTrans = Instantiate(_mirrorUI).transform;
						tTrans.SetParent(slotTrans);
						break;
					case "stella":
						tTrans = Instantiate(_stellaUI).transform;
						tTrans.SetParent(slotTrans);
						break;
					case "phoenix":
						tTrans = Instantiate(_phoenixUI).transform;
						tTrans.SetParent(slotTrans);
						break;
					case "osmosis":
						tTrans = Instantiate(_osmosisUI).transform;
						tTrans.SetParent(slotTrans);
						break;
				}
			}
		}
		
		private void loadHandInfo(List<SlotInfo> invInfo, Transform invRef, int offset, int end = 999)
		{
			int len = Math.Min(invRef.childCount, end);
			Transform slotTrans;
			Transform tTrans;
			PlantSO pso;
			float size = 0.7f;
			for (int i=offset; i<len; i++)
			{
				tTrans = null;
				if(invInfo[i-offset].type == null)
					continue;
				slotTrans = invRef.GetChild(i);
				if(slotTrans.childCount > 1)
				{
					for (int j=slotTrans.childCount; j>=0; j--)
						Destroy(slotTrans.GetChild(1).gameObject);
				}
				switch (invInfo[i-offset].type)
				{
					case "jar":
						tTrans = Instantiate(_jarInvObj).transform;
						tTrans.GetComponent<Jar>().Contains = invInfo[i-offset].attribute;
						if(invInfo[i-offset].attribute == null)
							tTrans.GetComponent<StackableItem>().SetStackTotal(invInfo[i-offset].count);
						else
							tTrans.GetComponent<Jar>().ItemAmount = invInfo[i-offset].count;
						tTrans.SetParent(slotTrans);
						break;
					case "Muchroom":
						tTrans = Instantiate(_muchjarInvObj).transform;
						tTrans.GetComponent<MuchroomPlant>().Contains = invInfo[i-offset].attribute;
						if(invInfo[i-offset].attribute == null)
							tTrans.GetComponent<StackableItem>().SetStackTotal(invInfo[i-offset].count);
						else
							tTrans.GetComponent<MuchroomPlant>().ItemAmount = invInfo[i-offset].count;
						tTrans.SetParent(slotTrans);
						break;
					case "Lantern":
						tTrans = createInvDraggable(_lanternSO, size).transform;
						tTrans.GetComponent<StackableItem>().SetStackTotal(invInfo[i-offset].count);
						tTrans.SetParent(slotTrans);
						break;
					case "Fernace":
						tTrans = createInvDraggable(_fernaceSO, size).transform;
						tTrans.GetComponent<StackableItem>().SetStackTotal(invInfo[i-offset].count);
						tTrans.SetParent(slotTrans);
						break;
					case "Spearmint":
						tTrans = createInvDraggable(_spearmintSO, size).transform;
						tTrans.GetComponent<StackableItem>().SetStackTotal(invInfo[i-offset].count);
						tTrans.SetParent(slotTrans);
						break;
					case "Beanstalk Bomber":
						tTrans = createInvDraggable(_bBomberSO, size).transform;
						tTrans.GetComponent<StackableItem>().SetStackTotal(invInfo[i-offset].count);
						tTrans.SetParent(slotTrans);
						break;
					case "Pipevine":
						tTrans = createInvDraggable(_pipevineSO, size).transform;
						tTrans.GetComponent<StackableItem>().SetStackTotal(invInfo[i-offset].count);
						tTrans.SetParent(slotTrans);
						break;
					case "Growcap":
						tTrans = createInvDraggable(_growcapSO, size).transform;
						tTrans.GetComponent<StackableItem>().SetStackTotal(invInfo[i-offset].count);
						tTrans.SetParent(slotTrans);
						break;
					case "Bubble Bulb":
						tTrans = createInvDraggable(_bubbleSO, size).transform;
						tTrans.GetComponent<StackableItem>().SetStackTotal(invInfo[i-offset].count);
						tTrans.SetParent(slotTrans);
						break;
					case "Lily Slippies":
						tTrans = createInvDraggable(_lilySO, size).transform;
						tTrans.GetComponent<StackableItem>().SetStackTotal(invInfo[i-offset].count);
						tTrans.SetParent(slotTrans);
						break;
						/*
					case "Muchroom":
						tTrans = createInvDraggable(_muchroomSO, size).transform;
						tTrans.GetComponent<StackableItem>().SetStackTotal(invInfo[i-offset].count);
						tTrans.SetParent(slotTrans);
						break;
						*/
					case "Pitcher":
						tTrans = createInvDraggable(_pitcherSO, size).transform;
						tTrans.GetComponent<StackableItem>().SetStackTotal(invInfo[i-offset].count);
						tTrans.SetParent(slotTrans);
						break;
					case "Rose Fringe":
						tTrans = createInvDraggable(_roseSO, size).transform;
						tTrans.GetComponent<StackableItem>().SetStackTotal(invInfo[i-offset].count);
						tTrans.SetParent(slotTrans);
						break;
				}
			}
		}

		private GameObject createInvDraggable(PlantSO pso, float size = 1)
		{
			GameObject item = Instantiate(_defaultDraggable);
			Image image = item.GetComponent<Image>();
			image.sprite = pso.Artwork;
			InventoryDraggable draggable = item.GetComponent<InventoryDraggable>();
			draggable._plantSO = pso;
			draggable.transform.localScale = new Vector3(size, size, 1);
			return item;
		}
		
		private List<almPlantInfo> saveNorPInfo(List<AlmanacPlantDiscovered> inList)
		{
			List<almPlantInfo> outList = new List<almPlantInfo>();
			
			for(int i=0; i<inList.Count; i++){
				almPlantInfo pi = new almPlantInfo();
				pi.discovered = inList[i].discovered;
				pi.r1Discovered = inList[i].recipeOneDiscovered;
				pi.r2Discovered = inList[i].recipeTwoDiscovered;
				outList.Add(pi);
			}
			return outList;
		}
		
		private List<almPlantInfo> savePanPInfo(List<AlmanacPanaceanPlantDiscovered> inList)
		{
			List<almPlantInfo> outList = new List<almPlantInfo>();
			
			for(int i=0; i<inList.Count; i++){
				almPlantInfo pi = new almPlantInfo();
				pi.discovered = inList[i].discovered;
				outList.Add(pi);
			}
			return outList;
		}
		
		private (List<Tuple<int, int, int, string>>, List<Tuple<int, int, int>>) savePlaced(Dictionary<Vector3Int, GameObject> objCoordinatesDict)
		{
			List<Tuple<int, int, int, string>> outList = new List<Tuple<int, int, int, string>>();
			List<Tuple<int, int, int>> beanstalkList = new List<Tuple<int, int, int>>();
			foreach (KeyValuePair<Vector3Int, GameObject> entry in objCoordinatesDict)
			{
				string plantName = entry.Value.GetComponent<BasePlant>().getPlantSO().PlantName;
				if (plantName == "Beanstalk Bomber")
				{
					BeanstalkAnchor anchor = entry.Value.GetComponent<BeanstalkPlant>().Anchor;
					beanstalkList.Add(new Tuple<int, int, int>((int) anchor.transform.position.x, (int) anchor.transform.position.y, anchor.gameObject.layer));
				}
				if (plantName != "Beanstalk Anchor")
					outList.Add(new Tuple <int, int, int, string>(entry.Key.x, entry.Key.y, entry.Value.layer, plantName));
			}
			return (outList, beanstalkList);
		}
		
		private void loadPlacedPlants(List<Tuple<int, int, int, string>> plantCoordinates, List<Tuple<int, int, int>> beanstalkToAnchorList)
		{
			int len = plantCoordinates.Count;
			int beanstalkIndex = 0;
			Dictionary<Vector3Int, GameObject> _objCoordinatesDict = new Dictionary<Vector3Int, GameObject>();
			GameObject selectedPlant = null;
			GameObject curTilemapObj = null;
			for(int i=0; i<len; i++)
			{
				switch (plantCoordinates[i].Item3)
				{
					case 7:
						curTilemapObj = _ev1;
						break;
					case 8:
						curTilemapObj = _ev2;
						break;
					case 9:
						curTilemapObj = _ev3;
						break;
					default:
						//Debug.Log("SaveManager: layer #" + plantCoordinates[i].Item3);
						break;
				}
				switch (plantCoordinates[i].Item4)
				{
					case "Lantern":
						selectedPlant = _lanternObj;
						break;
					case "Fernace":
						selectedPlant = _fernaceObj;
						break;
					case "Spearmint":
						selectedPlant = _spearmintObj;
						break;
					case "Beanstalk Bomber":
						selectedPlant = _bBomberObj;
						break;
					case "Pipevine":
						selectedPlant = _pipevineObj;
						break;
					case "Growcap":
						selectedPlant = _growcapObj;
						break;
					case "Bubble Bulb":
						selectedPlant = _bubbleObj;
						break;
					case "Lily Slippies":
						selectedPlant = _lilyObj;
						break;
					case "Muchroom":
						selectedPlant = _muchroomObj;
						break;
					case "Pitcher":
						selectedPlant = _pitcherObj;
						break;
					case "Rose Fringe":
						selectedPlant = _roseObj;
						break;
					default:
						//Debug.Log("SaveManager: plant named " + plantCoordinates[i].Item4);
						break;
				}
				Vector3Int GPI = new Vector3Int(plantCoordinates[i].Item1, plantCoordinates[i].Item2, 0);
				selectedPlant.GetComponent<IPlaceable>().instantiateObject(GPI, selectedPlant, curTilemapObj.GetComponent<TilemapRenderer>(), _objCoordinatesDict);
				if (selectedPlant == _bBomberObj)
				{
					BeanstalkPlant.beanstalkBeingAnchored.enableDrag = false;

					GameObject anchorElevationLayer = null;
					Tuple<int, int, int> beanstalkToAnchor = beanstalkToAnchorList[beanstalkIndex];
					switch (beanstalkToAnchor.Item3)
					{
						case 7:
							anchorElevationLayer = _ev1;
							break;
						case 8:
							anchorElevationLayer = _ev2;
							break;
						case 9:
							anchorElevationLayer = _ev3;
							break;
					}

					Vector3Int AnchorPos = new Vector3Int(beanstalkToAnchor.Item1, beanstalkToAnchor.Item2, 0);
					_bBomberAnchorObj.GetComponent<IPlaceable>().instantiateObject(AnchorPos, _bBomberAnchorObj, anchorElevationLayer.GetComponent<TilemapRenderer>(), _objCoordinatesDict);
					beanstalkIndex += 1;
				}	
			}
			_placementObj.GetComponent<PlacementManager>().setObjCoordinates(_objCoordinatesDict);
		}
		
		private void printList(List<SlotInfo> list)
		{
			string toPrint = "SaveManager: printList\n";
			
			for (int i=0; i<list.Count; i++)
				toPrint += list[i]+"\n";
			Debug.Log(toPrint);
		}
		
		/*private bool isStackable(string type)
		{
			return true;		//TODO: figure out and implement way of checking if something is stackable. ?obj.GetComponent<Stackable>()?
		}*/
		
		/*private int findCount(Transform trans, string type)
		{
			return 5;			//TODO: figure out and implement way of getting the number of items stacked. Consider merging with above
		}*/
		
		/*private void SaveHR(string saveRaw)
		{
			int tabCount = 0;
			int len = saveRaw.Length;
			char[] nArr = {'{', '}', '[', ']', ','};
			char c;
			var nSet = new HashSet<char>(nArr);
			
			string saveRawHR = "";
			for(int i=0; i<len; i++){
				c = saveRaw[i];
				if (nSet.Contains(c)){
					if(c == '{' || c == '['){
						saveRawHR += c;
						saveRawHR += '\n';
						tabCount++;
						for(int j=0; j<tabCount; j++)
							saveRawHR += '\t';
					}
					else if(c == '}' || c == ']'){
						saveRawHR += '\n';
						tabCount--;
						for(int j=0; j<tabCount; j++)
							saveRawHR += '\t';
						saveRawHR += c;
					}
					else{
						saveRawHR += c;
						saveRawHR += '\n';
						for(int j=0; j<tabCount; j++)
							saveRawHR += '\t';
					}
				}
				else
					saveRawHR += c;
			}
			File.WriteAllText("BotoSaveHR.json", saveRawHR);
		}*/
	}

	/*----------------------------------------- Helper Classes ----------------------------------------*/

	public class SaveFile
	{
		public List<almPlantInfo> norPDiscovered;
		public List<almPlantInfo> panPDiscovered;
		public List<int> noteInd;
		public List<int> meltInd;
		public List<int> dialInd;
		public EquipmentEffect currentEquipEff;
		public List<Tuple<int, int, int, string>> plantCoordinates;
		public List<Tuple<int, int, int>> beanstalkToAnchorList;
		public bool hasEntered;
		public bool isUnderwater1;
		public bool isUnderwater2;
		public int panaceanPlantsPlaced;
		
		public List<SlotInfo> invMain;
		public List<SlotInfo> invHand;
		public List<SlotInfo> invEquip;
		public List<SlotInfo> invPan;
		
		public ConvoSO curConvo;
		public DialogueSO curDialogue;
		public bool isPlaying;
		public ConvoSO waiting;
		public Action curCallback;
		public Action waitCallback;
		
		public string panTL;
		public string panTR;
		public string panBL;
		public string panBR;
		
		public float xPos;
		public float yPos;
		public int playerLayer;
		
		public List<(int x, int y)> visitedSectors;
		public List<Tuple<string, int, int>> markers;
		public string processType;
	}

	public class SlotInfo
	{
		public string type;
		public AttributeValue? attribute;
		public int count;
	}

	public class almPlantInfo
	{
		public bool discovered;
		public bool r1Discovered;
		public bool r2Discovered;
	}
}
