using Botography.Dependencies;
using Botography.Map;
using Botography.Notifications;
using Botography.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static InventoryConstants;

namespace Botography.Lab
{
	/// <summary>
	/// Manages the lab content.
	/// </summary>
	public class LabManager : MenuManagerBase<LabSystem, LabUiCollection>
	{
		private static LabManager _instance;
        [SerializeField] public SoilSlot _soilSlot;
        [SerializeField] public WaterSlot _waterSlot;

		private List<Jar> jars = new List<Jar>();

		public static LabManager Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new LabManager();
				}
				return _instance;
			}
		}

		private void Awake()
		{
			_instance = this;
		}

		// Start is called before the first frame update
		void Start()
		{
			
		}

		// Update is called once per frame
		void Update()
		{

		}

		private void OnEnable()
		{
			PopulateInventory();
			system.ToggleMenu(true);
		}

		private void OnDisable()
		{
			system.ToggleMenu(false);
			uiCollection.ToggleHelpPopup(false);
		}

		public SunlightLevel GetSunLevel()
		{
			return PlayerManager.Instance.GetLightLevel();
		}

		public void TryCombination(SoilType soil, WaterPh water)
		{
			//Debug.Log("Lab Manager: " + PlayerManager.Instance.GetLightLevel());
            //SunlightLevel sun = PlayerManager.Instance.GetCurrentRegion().SunlightLevel;
            SunlightLevel sun = PlayerManager.Instance.GetLightLevel();
			PlantSO pso = system.TryCombination(sun, soil, water);
			if (pso != null)
			{
				bool invsuccess = false;
				if (pso.PlantName == "Muchroom")
				{
					if (InventoryManager.Instance.addMuchroomToInv())
					{
						uiCollection.addMuchroomToInv();
						Notifyer.Instance.Notify("Success! Added Muchroom");
						SoundManager.Instance.PlaySFX("plant placed");
						invsuccess = true;
					}
				}
				else
				{
					if (InventoryManager.Instance.addItemToInv(pso))
					{
						uiCollection.addItemToInv(pso);
						Notifyer.Instance.Notify("Success! Added " + pso.PlantName);
						SoundManager.Instance.PlaySFX("plant placed");
						invsuccess = true;
					}
				}

				if (!invsuccess)
				{
					Notifyer.Instance.Notify("Inventory Full");
					return;
				}

				InventoryManager.Instance.RemoveSampleFromJar(SOILTYPE_DICTIONARY[soil], uiCollection._soilSlot.GetComponentInChildren<Jar>().ItemAmount);
				InventoryManager.Instance.RemoveSampleFromJar(WATERPH_DICTIONARY[water], uiCollection._waterSlot.GetComponentInChildren<Jar>().ItemAmount);

				// Richard here. I'm tired. So this is what I'm doing
				//uiCollection.AddGameObjectToInventoryUi(pso);

				AlmanacManager.Instance.GetPlantInfo(pso.PageNumber - 1);
				AlmanacPlantDiscovered plant = AlmanacManager.Instance.GetPlantInfo(pso.PageNumber - 1);
				bool plantPreviouslyUndiscovered = !plant.discovered;
				if (pso.PlantSunlightLevel1 == sun && pso.PlantSoilType1 == soil && pso.PlantWaterPh1 == water)
					AlmanacManager.Instance.SetPlantDiscoveryStatus(pso.PageNumber - 1, true, true, plant.recipeTwoDiscovered);
				else if (pso.PlantSunlightLevel2 == sun && pso.PlantSoilType2 == soil && pso.PlantWaterPh2 == water)
					AlmanacManager.Instance.SetPlantDiscoveryStatus(pso.PageNumber - 1, true, plant.recipeOneDiscovered, true);
				if (plantPreviouslyUndiscovered)
				{
					this.gameObject.SetActive(false);
                    UIManager.Instance.OnAlmanacPressed();
                    //open the almanac and flip to the page in question
                    AlmanacManager.Instance.GoToPageNumber(pso.PageNumber - 1);
					SoundManager.Instance.PlaySFX("plant placed");
				}

				Jar soilJ = uiCollection._soilSlot.GetComponentInChildren<Jar>();
				Jar waterJ = uiCollection._waterSlot.GetComponentInChildren<Jar>();
				soilJ.AttemptRemove();
            	waterJ.AttemptRemove();

				if (soilJ.Contains == null || soilJ.Contains == AttributeValue.None)
				{
					uiCollection.SoilRemoved();
				}
				if (waterJ.Contains == null || waterJ.Contains == AttributeValue.None)
				{
					uiCollection.WaterRemoved();
				}
			}
			else
			{
				Notifyer.Instance.Notify("Hmm, seems that combo didn't work!");
				InventoryManager.Instance.RemoveSampleFromJar(SOILTYPE_DICTIONARY[soil], uiCollection._soilSlot.GetComponentInChildren<Jar>().ItemAmount);
				InventoryManager.Instance.RemoveSampleFromJar(WATERPH_DICTIONARY[water], uiCollection._waterSlot.GetComponentInChildren<Jar>().ItemAmount);
				Jar soilJ = uiCollection._soilSlot.GetComponentInChildren<Jar>();
				Jar waterJ = uiCollection._waterSlot.GetComponentInChildren<Jar>();
				soilJ.AttemptRemove();
            	waterJ.AttemptRemove();
			}
		}

		public void PopulateSlots(WaterPh water, SoilType soil)
		{
			Jar waterJar = null;
			Jar soilJar = null;

			foreach (Jar jar in jars)
			{
				if (jar.Contains == InventoryConstants.SOILTYPE_DICTIONARY[soil])
				{
					soilJar = jar;
					break;
				}
			}
			foreach (Jar jar in jars)
			{
				if (jar.Contains == InventoryConstants.WATERPH_DICTIONARY[water])
				{
					waterJar = jar;
					break;
				}
			}

			//waterJar.Contains = AttributeValue.Sand;
			//soilJar.Contains = AttributeValue.Sand;
			//waterJar.gameObject.transform.SetParent(_waterSlot.gameObject.transform);
			//soilJar.gameObject.transform.SetParent(_soilSlot.gameObject.transform);
			waterJar.GetComponent<StackableItem>().EnableDrag();
			_waterSlot.GetComponent<WaterSlot>().OnPointerDown(null);
			soilJar.GetComponent<StackableItem>().EnableDrag();
			_soilSlot.GetComponent<SoilSlot>().OnPointerDown(null);
		}

		public void PopulateInventory()
		{
			List<GameObject> items = InventoryManager.Instance.GetInventoryItems();
			system.LoadInventory(items);

			jars.Clear();
			List<GameObject> gos = new List<GameObject>();
            foreach (GameObject item in items)
            {
				GameObject go = Instantiate(item);
				try
				{
					Jar orig_jar = item.GetComponentInChildren<Jar>();	
					Jar new_jar = go.GetComponentInChildren<Jar>();	
					new_jar.Contains = orig_jar.Contains;
					jars.Add(new_jar);
				}
				catch
				{
				}
				gos.Add(go);
            }
            uiCollection.PopulateInventoryUi(gos);
		}

		public void ToggleHelpPopup()
		{
			uiCollection.ToggleHelpPopup();
		}

		public void ToggleHelpPopup(bool active)
		{
			uiCollection.ToggleHelpPopup(active);
		}

		public void ResetSeedPos()
		{
			uiCollection.ResetSeedPos();
		}
	}
}
