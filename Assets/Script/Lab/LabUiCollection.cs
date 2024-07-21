using Botography.Dependencies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Botography.Notifications;
using static InventoryConstants;
using Unity.VisualScripting;

namespace Botography.Lab
{
    /// <summary>
    /// Manages the visual and UI elements of the lab.
    /// </summary>
    public class LabUiCollection : MenuUiCollectionBase
    {
        private const float SEED_TOUCHDOWN_TIME = 1.5f;

        [SerializeField] private Button _combineButton;
        [SerializeField] public Slot _soilSlot;
        [SerializeField] public Slot _waterSlot;
        [SerializeField] private List<Slot> _inventorySlots;
		[SerializeField] private Button _helpBubble;
		[SerializeField] private GameObject _helpPopup;
        [SerializeField] private Button _seedBag;
		private bool _helpActive;
		public GameObject defaultDraggable;
	    public GameObject muchroom;

        #region Soil Objs
        [SerializeField] private GameObject _loamObj;
        [SerializeField] private GameObject _clayObj;
        [SerializeField] private GameObject _siltObj;
        [SerializeField] private GameObject _sandObj;
        private GameObject _currSoilObj;
		#endregion Soil Objs

		#region Water pH Transforms
		[SerializeField] private Transform _maxBasic;
        [SerializeField] private Transform _minBasic;
		[SerializeField] private Transform _neutral;
		[SerializeField] private Transform _maxAcidic;
		[SerializeField] private Transform _minAcidic;
        [SerializeField] private GameObject _phPointer;
        #endregion Water pH Transforms

        #region Sunlight GOs
        [SerializeField] private GameObject _fullSun;        
        [SerializeField] private GameObject _partSun;        
        [SerializeField] private GameObject _halfSun;        
        [SerializeField] private GameObject _partShade;        
        [SerializeField] private GameObject _fullShade;

        private GameObject _currSun;
        #endregion Sunlight GOs

        #region Seed
        [SerializeField] private Rigidbody2D _seed;
        [SerializeField] private Transform _seedStart;
        [SerializeField] private Transform _seedEnd;
        private bool _dropSeed;
		#endregion Seed

		private void Awake()
        {
            // This no worky because soilJar and waterJar have no children on awake
            /*
            _combineButton.onClick.AddListener(() => {
                Jar soilJar = _soilSlot.GetComponentInChildren<Jar>();
                Debug.Log(soilJar.Contains);
                Jar waterJar = _waterSlot.GetComponentInChildren<Jar>();
                Debug.Log(waterJar.Contains);
                LabManager.Instance.TryCombination(SOIL_ATTRIBUTE_DICTIONARY[soilJar.Contains], WATER_ATTRIBUTE_DICTIONARY[waterJar.Contains]);
            });
            */
		}

		// Start is called before the first frame update
		void Start()
        {
			// TODO: Somehow subscribe the water change and soil change functions to the
			// water and soil slots so that it's called whenver the slot's contents change.
		}

		// Update is called once per frame
		void Update()
        {
            if (_dropSeed)
            {
                if (_seed.transform.position.y <= _seedEnd.position.y)
                {
                    _dropSeed = false;
                    _seed.Sleep();
                    _seed.transform.position = _seedStart.position;
                    StartCoroutine(TryCombination());
                }
            }
        }

        public void DropSeed()
        {
            Jar soilJar = _soilSlot.GetComponentInChildren<Jar>();
			Jar waterJar = _waterSlot.GetComponentInChildren<Jar>();

			if (soilJar == null && waterJar == null)
			{
				Notifyer.Instance.Notify("You need soil and water!");
				return;
			}
			else if (soilJar == null && waterJar != null)
			{
				Notifyer.Instance.Notify("You need soil!");
				return;
			}
			else if (soilJar != null && waterJar == null)
			{
				Notifyer.Instance.Notify("You need water!");
				return;
			}
            else if (soilJar.ItemAmount == 0 && waterJar.ItemAmount == 0)
            {
				Notifyer.Instance.Notify("Both of your jars is empty!");
				return;
            }
            else if (soilJar.ItemAmount == 0 || waterJar.ItemAmount == 0)
            {
				Notifyer.Instance.Notify("One of your jars is empty!");
				return;
            }
            SoundManager.Instance.PlaySFX("button clicked");
			_dropSeed = true;
			_seed.WakeUp();
		}

        private IEnumerator TryCombination()
        {
			Jar soilJar = _soilSlot.GetComponentInChildren<Jar>();
			Jar waterJar = _waterSlot.GetComponentInChildren<Jar>();
			yield return new WaitForSecondsRealtime(SEED_TOUCHDOWN_TIME);
            LabManager.Instance.TryCombination(SOIL_ATTRIBUTE_DICTIONARY[soilJar.Contains], WATER_ATTRIBUTE_DICTIONARY[waterJar.Contains]);
        }

		public void PopulateInventoryUi(List<GameObject> gos)
		{
            if (_soilSlot.transform.childCount > 0)
                Destroy(_soilSlot.transform.GetChild(0).gameObject);

            if (_waterSlot.transform.childCount > 0)
                Destroy(_waterSlot.transform.GetChild(0).gameObject);

            for (int i = 0; i < _inventorySlots.Count; i++)
            {
                if (_inventorySlots[i].transform.childCount > 0)
                {
                    foreach (Transform child in _inventorySlots[i].transform)
                    {
                        Destroy(child.gameObject);
                    }
                }
            }

			for (int i = 0; i < _inventorySlots.Count; i++)
            {
                if (i < gos.Count)
                    if (gos[i] != null)
                    {
                        gos[i].transform.SetParent(_inventorySlots[i].transform);
                        gos[i].transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                    }
            }
		}

        // Richard here. I am getting tired. Im copying and pasting
        /*
        public bool AddGameObjectToInventoryUi(GameObject go)
        {
            return GetInventoryFilledSlotCount() < INVENTORY_CAPACITY && AddGameObjectToFirstOpenSlot(go);
        }
        */
        public bool addItemToInv(PlantSO plant)
	    {
            foreach (Slot slot in _inventorySlots)
            {
                if (slot.transform.childCount == 0)
                {
                    GameObject item = Instantiate(defaultDraggable);
                    Image image = item.GetComponent<Image>();
                    image.sprite = plant.Artwork;
                    InventoryDraggable draggable = item.GetComponent<InventoryDraggable>();
                    draggable._plantSO = plant;
                    item.transform.SetParent(slot.transform);
                    item.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                    return true;
                }
            }
            return false;
	    }

        public bool addMuchroomToInv()
	    {
            foreach (Slot slot in _inventorySlots)
            {
                if (slot.transform.childCount == 0)
                {
                    GameObject item = Instantiate(muchroom);
                    item.transform.SetParent(slot.transform);
                    item.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                    return true;
                }
            }
            return false;
	    }

        private int GetInventoryFilledSlotCount()
        {
            int total = 0;
            foreach (Slot slot in _inventorySlots)
            {
                total += slot.transform.childCount;
            }
            return total;
        }

        private bool AddGameObjectToFirstOpenSlot(GameObject go)
        {
            foreach (Slot slot in _inventorySlots)
            {
                if (slot.transform.childCount == 0)
                {
                    go.transform.SetParent(slot.transform);
                    return true;
                }
            }

            return false;
        }

        private void ChangeSoil(AttributeValue? type)
        {
            GameObject newSoil = null;
            switch (type)
            {
                case AttributeValue.Clay:
                    newSoil = _clayObj;
                    break;
				case AttributeValue.Silt:
					newSoil = _siltObj;
					break;
				case AttributeValue.Sand:
					newSoil = _sandObj;
					break;
				case AttributeValue.Loam:
					newSoil = _loamObj;
					break;

			}

            if (_currSoilObj == null || !_currSoilObj.Equals(newSoil))
            {
                if (_currSoilObj != null)
                {
					_currSoilObj.SetActive(false);
				}
                _currSoilObj = newSoil;
                if (_currSoilObj != null)
                {
					_currSoilObj.SetActive(true);
				}
            }
        }

        public void SoilRemoved()
        {
            if (_soilSlot.CurrItem == null || _soilSlot.CurrItem.GetComponent<Jar>().Contains == null || _soilSlot.CurrItem.GetComponent<Jar>().Contains == AttributeValue.None)
            {
                if (_currSoilObj != null)
                {
					_currSoilObj.SetActive(false);
					_currSoilObj = null;
				}
            }
        }

        public void WaterRemoved()
        {
			if (_waterSlot.CurrItem == null || _soilSlot.CurrItem.GetComponent<Jar>().Contains == null || _soilSlot.CurrItem.GetComponent<Jar>().Contains == AttributeValue.None)
			{
                ChangeWater(null);
			}
		}

        private void ChangeWater(AttributeValue? ph)
        {
            if (ph == null || ph == AttributeValue.Neutral)
            {
                _phPointer.transform.position = _neutral.position;
            }
            else
            {
                float mod = _waterSlot.GetComponentInChildren<Jar>().GetPhMod();
                Transform min;
                Transform max;
                if (ph == AttributeValue.Basic)
                {
                    min = _minBasic;
                    max = _maxBasic;
                }
                else
                {
                    min = _minAcidic;
                    max = _maxAcidic;
                }

                float newY = min.position.y + (max.position.y - min.position.y) * mod;
                _phPointer.transform.position = new Vector3(_phPointer.transform.position.x, newY, _phPointer.transform.position.z);
            }
		}

        private void SetSunlightLevel()
        {
            SunlightLevel sun = LabManager.Instance.GetSunLevel();

            switch (sun)
            {
                case SunlightLevel.fullSun:
                    _currSun = _fullSun;
                    break;
				case SunlightLevel.partialSun:
					_currSun = _partSun;
					break;
				case SunlightLevel.halfAndHalf:
					_currSun = _halfSun;
					break;
				case SunlightLevel.partialShade:
					_currSun = _partShade;
					break;
				case SunlightLevel.fullShade:
					_currSun = _fullShade;
					break;
			}

            _currSun.SetActive(true);
        }

		#region Slot Listeners
        private void WaterAddedListener(DraggableItem draggable)
        {
			Jar j = draggable.GetComponent<Jar>();
			ChangeWater(j.Contains);
		}

		private void SoilAddedListener(DraggableItem draggable)
		{
			Jar j = draggable.GetComponent<Jar>();
			ChangeSoil(j.Contains);
		}

		private void WaterRemovedListener(DraggableItem draggable)
		{
			WaterRemoved();
		}

		private void SoilRemovedListener(DraggableItem draggable)
		{
			SoilRemoved();
		}
		#endregion Slot Listeners

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

		private void OnEnable()
		{
            SetSunlightLevel();

            _waterSlot.DraggableAdded += WaterAddedListener;
			_waterSlot.DraggableRemoved += WaterRemovedListener;

			_soilSlot.DraggableAdded += SoilAddedListener;
			_soilSlot.DraggableRemoved += SoilRemovedListener;
		}

		private void OnDisable()
		{
            _currSun.SetActive(false);
            _currSun = null;

            _waterSlot.DraggableAdded -= WaterAddedListener;
			_waterSlot.DraggableRemoved -= WaterRemovedListener;

			_soilSlot.DraggableAdded -= SoilAddedListener;
			_soilSlot.DraggableRemoved -= SoilRemovedListener;

            WaterRemoved();
            SoilRemoved();
		}

        public void ResetSeedPos()
        {
            _dropSeed = false;
            _seed.Sleep();
            _seed.transform.position = _seedStart.position;
        }

        public List<Jar> FindAllSampleJars()
        {
            List<Jar> jars = new List<Jar>();

		    for (int i = 0; i < _inventorySlots.Count; i++)
		    {
			    if (_inventorySlots[i].transform.childCount > 0 && _inventorySlots[i].transform.GetChild(0).GetComponent<Jar>() != null)
				    jars.Add(_inventorySlots[i].transform.GetChild(0).GetComponent<Jar>());
		    }

            return jars;
	    }
	}
}
