using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;
using static InventoryConstants;
using Botography.Notifications;

public class Jar : MonoBehaviour, IInventoryItem
{
	public const float MOD_MIN = 0;
	public const float MOD_MAX = 1f;

	public int ItemAmount = 0;
	public int MaxAmount = 3;       //may be able to make static? could be more efficient?
	protected List<int> thresh = new() { 1, 2, 3 };
	public AttributeValue? Contains = null;
	[SerializeField] protected Sprite _loamLevel1;
	[SerializeField] protected Sprite _loamLevel2;
	[SerializeField] protected Sprite _loamLevel3;
	[SerializeField] protected Sprite _siltLevel1;
	[SerializeField] protected Sprite _siltLevel2;
	[SerializeField] protected Sprite _siltLevel3;
	[SerializeField] protected Sprite _sandLevel1;
	[SerializeField] protected Sprite _sandLevel2;
	[SerializeField] protected Sprite _sandLevel3;
	[SerializeField] protected Sprite _clayLevel1;
	[SerializeField] protected Sprite _clayLevel2;
	[SerializeField] protected Sprite _clayLevel3;
	[SerializeField] protected Sprite _basicLevel1;
	[SerializeField] protected Sprite _basicLevel2;
	[SerializeField] protected Sprite _basicLevel3;
	[SerializeField] protected Sprite _neutralLevel1;
	[SerializeField] protected Sprite _neutralLevel2;
	[SerializeField] protected Sprite _neutralLevel3;
	[SerializeField] protected Sprite _acidicLevel1;
	[SerializeField] protected Sprite _acidicLevel2;
	[SerializeField] protected Sprite _acidicLevel3;
	[SerializeField] protected TextMeshProUGUI _label;
	[SerializeField] protected Image _fill;
	private float _phMod; // 0 <= phMod <= 1

	public GameObject GameObject
	{
		get { return gameObject; }
	}
	
	/*
	public Jar(){
		//sets starting values. May need to be changed to allow proper loading
        ItemAmount = 0;
		Contains = null;
    }
	*/

	private void Awake()
	{
	}

	private void Start()
	{
		UpdateLevel();
	}

	// This is bad programming, I wrote this, I don't have time
	// Fix filljar in Inventory System to use methods in here so we don't have to do this mess
	public virtual void Update()
	{
		
	}

	public bool IsOfInventoryType(InventoryItemType type)
	{
		return type == InventoryItemType.Jar;
	}

	//Adds a single "amount" of a soil/water type
	public bool AttemptAdd(AttributeValue? sample){
		bool res = false;
		if(sample == Contains){
			if(ItemAmount >= MaxAmount){
				ItemAmount = MaxAmount;
			}
			else{
				ItemAmount++;
				res = true;
			}
		}
		else if(sample == null){
			ItemAmount = 1;
			Contains = sample;
			res = true;
		}

		UpdateLevel();
		return res;
	}
	
	//Takes out a single "amount" of the contained soil/water type
	public AttributeValue? AttemptRemove(){
		AttributeValue? res = null;
		if(Contains != null){
			if(ItemAmount < 1){
				ItemAmount = 0;
				Contains = null;
			}
			else if(ItemAmount == 1){
				ItemAmount = 0;
				AttributeValue? sample = Contains;
				Contains = null;
				res = sample;
			}
			else{
				ItemAmount--;
				res = Contains;
			}
		}

		UpdateLevel();
		return res;
	}
	
	//Removes all of the soil/water type
	public AttributeValue? AttemptEmpty(){
		AttributeValue? res = null;
		if(Contains != null){
			ItemAmount = 0;
			AttributeValue? sample = Contains;
			Contains = null;
			res = sample;
		}

		UpdateLevel();
		return res;
	}

	public float GetPhMod()
	{
		return _phMod;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="mod"></param>
	/// <returns>If mod was set. Mod is not set if the contents are not non-neutral water.</returns>
	protected bool SetPhMod(float mod)
	{
		if (Contains != AttributeValue.Acidic && Contains != AttributeValue.Basic)
		{
			if (mod > MOD_MAX)
			{
				mod = MOD_MAX;
			}
			else if (mod < MOD_MIN)
			{
				mod = MOD_MIN;
			}

			_phMod = mod;
			return true;
		}

		return false;
	}

	/// <summary>
	/// Call if the jar is already known not to have a null Contains field
	/// </summary>
	public virtual void FillJar(AttributeValue type)
	{
		ItemAmount = MaxAmount;

		SendNotification(type);
		UpdateLevel();
	}

	/// <summary>
	/// Call if the jar is empty and needs to have its Contains field set
	/// </summary>
	/// <param name="type"></param>
	public virtual void FillEmptyJar(AttributeValue type)
	{
		Contains = type;
		ItemAmount = MaxAmount;
		SetPhMod(UnityEngine.Random.value);

		SendNotification(type);
		UpdateLevel();
	}

	protected virtual void SendNotification(AttributeValue type)
	{
		if (type == AttributeValue.Silt || type == AttributeValue.Sand || type == AttributeValue.Loam || type == AttributeValue.Clay)
			{
				Notifyer.Instance.Notify("Soil type: " + type.ToString() + ", has been added to jar");
				SoundManager.Instance.PlaySFX("soil collected");
			}
		else if (type == AttributeValue.Basic || type == AttributeValue.Acidic || type == AttributeValue.Neutral)
			{
				Notifyer.Instance.Notify("Water type: " + type.ToString() + ", has been added to jar");
				SoundManager.Instance.PlaySFX("water collected");
			}
	}

	protected virtual void UpdateLevel()
	{
		if (ItemAmount > 0)
		{
			List<Sprite> sprites = new();
			if (Contains == AttributeValue.Silt)
			{
				_label.text = "Si";
				sprites.Add(_siltLevel1);
				sprites.Add(_siltLevel2);
				sprites.Add(_siltLevel3);
			}
			else if (Contains == AttributeValue.Sand)
			{
				_label.text = "Sa";
				sprites.Add(_sandLevel1);
				sprites.Add(_sandLevel2);
				sprites.Add(_sandLevel3);
			}
			else if (Contains == AttributeValue.Loam)
			{
				_label.text = "Lo";
				sprites.Add(_loamLevel1);
				sprites.Add(_loamLevel2);
				sprites.Add(_loamLevel3);
			}
			else if (Contains == AttributeValue.Clay)
			{
				_label.text = "Cl";
				sprites.Add(_clayLevel1);
				sprites.Add(_clayLevel2);
				sprites.Add(_clayLevel3);
			}
			else if (Contains == AttributeValue.Acidic)
			{
				_label.text = "Ac";
				sprites.Add(_acidicLevel1);
				sprites.Add(_acidicLevel2);
				sprites.Add(_acidicLevel3);
			}
			else if (Contains == AttributeValue.Basic)
			{
				_label.text = "Ba";
				sprites.Add(_basicLevel1);
				sprites.Add(_basicLevel2);
				sprites.Add(_basicLevel3);
			}
			else if (Contains == AttributeValue.Neutral)
			{
				_label.text = "Ne";
				sprites.Add(_neutralLevel1);
				sprites.Add(_neutralLevel2);
				sprites.Add(_neutralLevel3);
			}

			thresh.Sort();
			for (int i = 0; i < thresh.Count; i++)
			{
				if (ItemAmount <= thresh[i])
				{
					_fill.sprite = sprites[i];
					break;
				}
			}
		}
		else
		{
			_fill.sprite = null;
			_label.text = "E";
		}
	}
}
