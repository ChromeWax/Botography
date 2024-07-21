using Botography.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandleSectors : MonoBehaviour
{
	[SerializeField]
	private int _coordAdjustment;
	[SerializeField]
	private GameObject _playerController;
	private PlayerManager _playerManager;
	private List<(int x, int y)> visitedSectors;
	
	private Dictionary<int, Dictionary<int, GameObject>> _rowDict;

	public GameObject positionMarker;

	// Start is called before the first frame update
	void Awake()
    {
		_rowDict = new Dictionary<int, Dictionary<int, GameObject>>();
		Transform selfT = this.gameObject.GetComponent<Transform>();
		
		int nChildren = selfT.childCount;
        for(int i=0; i<nChildren; i++){
			Dictionary<int, GameObject> _sectorDict = new Dictionary<int, GameObject>();
			Transform childT = selfT.GetChild(i);		//GetChild returns a transform object... for some reason
			
			int mChildren = childT.childCount;
			for(int j=0; j<mChildren; j++){
				_sectorDict.Add(j, childT.GetChild(j).gameObject);
			}
			_rowDict.Add(6-i, _sectorDict);
		}
		_playerManager = _playerController.GetComponent<PlayerManager>();
		visitedSectors = new List<(int x, int y)>();
//		SectorsEnabled();
	}
	
	public void SectorsEnabled()
	{
Debug.Log("HandleSectors: Enable");
		if(visitedSectors == null)
			Awake();
		List<(int x, int y)> upVisitedSectors = PlayerManager.Instance.GetVisitedSectors();
Debug.Log("HandleSectors: " + upVisitedSectors.Count + ", " + visitedSectors.Count);
		if(upVisitedSectors.Count == visitedSectors.Count)
			return;
		for(int i=visitedSectors.Count; i<upVisitedSectors.Count; i++){
			RevealSector(upVisitedSectors[i]);
			visitedSectors.Add(upVisitedSectors[i]);
		}
		SetPositionMarker();
		positionMarker.SetActive(true);
	}

	public void SetPositionMarker(){
		(int x, int y) currentSector = PlayerManager.Instance.GetCurrentSector();
		int row = currentSector.y-_coordAdjustment;
		int col = currentSector.x-_coordAdjustment;

		Dictionary<int, GameObject> _sectorDict = _rowDict[row];
		GameObject sector = _sectorDict[col];
		//sector.GetComponent<Image>().color = Color.white;
		//sector.GetComponent<Image>().sprite = positionMarker;
		positionMarker.transform.parent = sector.transform;
		positionMarker.transform.localPosition = Vector2.zero;
	}
			
	
	public void RevealSector((int x, int y) s){
		int row = s.y-_coordAdjustment;
		int col = s.x-_coordAdjustment;
		
		Dictionary<int, GameObject> _sectorDict = _rowDict[row];
		GameObject sector = _sectorDict[col];
		sector.GetComponent<Image>().color = Color.clear;
	}
	
	public List<(int x, int y)> getVisitedSectors()
	{
		return visitedSectors;
	}
	
	public void setVisitedSectors(List<(int x, int y)> inList)
	{
		if(visitedSectors == null)
			Awake();
		visitedSectors = inList;
		for(int i=0; i<visitedSectors.Count; i++){
			RevealSector(visitedSectors[i]);
		}
	}
}
