using Botography.Lab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Botography.Dependencies;
using Botography.Player;

public class MapUI : UiCollectionBase
{
	public static MapUI Instance { get; private set; }
	[SerializeField] private GameObject _markerHandler;
	[SerializeField] private GameObject _mapSectorHandler;
	[SerializeField] private GameObject _helpBubble;
	[SerializeField] private GameObject _helpPopup;
	private bool _helpActive;

	private void Awake()
	{
		if (Instance != null && Instance != this)
			Destroy(this);
		else
			Instance = this;
	}
	
	void OnDisable(){
		ToggleHelpPopup(false);
		_markerHandler.GetComponent<MarkerHandler>().MapDisabled();
	}
	
	void OnEnable(){
		ToggleHelpPopup(false);
		_mapSectorHandler.GetComponent<HandleSectors>().SectorsEnabled();
		_markerHandler.GetComponent<MarkerHandler>().MapEnabled();
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
}