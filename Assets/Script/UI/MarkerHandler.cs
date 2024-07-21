using Botography.OverworldInteraction;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class MarkerHandler : MonoBehaviour
{
	[SerializeField]
	private GameObject _canvasObj;
	private Canvas _canvas;
	private GraphicRaycaster _gRaycaster;
	private Vector3 _scale;
	
	[SerializeField]
	public Texture2D StandardTexture;
	[SerializeField]
	public Texture2D CircleTexture;
	[SerializeField]
	public GameObject CircleMarker;
	[SerializeField]
	public Texture2D DiamondTexture;
	[SerializeField]
	public GameObject DiamondMarker;
	[SerializeField]
	public Texture2D DropTexture;
	[SerializeField]
	public GameObject DropMarker;
	[SerializeField]
	public Texture2D SquareTexture;
	[SerializeField]
	public GameObject SquareMarker;
	[SerializeField]
	public Texture2D StarTexture;
	[SerializeField]
	public GameObject StarMarker;
	[SerializeField]
	public Texture2D TriangleTexture;
	[SerializeField]
	public GameObject TriangleMarker;
	
	[SerializeField]
	private int layer;
	private Dictionary<string, Texture2D>  _labeledCursorDict;
	private Dictionary<string, GameObject> _labeledMarkerDict;
	private Dictionary<GameObject, Tuple<string, Vector2>> _markerInfoDict;
	private List<GameObject> _markerObjs;
	
	private string _curCursor;
	
    protected Vector2 mouseMovement;
	protected bool isInitialized;

	public static bool markerSelected = false;

	// Start is called before the first frame update
	void Awake()
    {
		_canvas = _canvasObj.GetComponent<Canvas>();
        _labeledCursorDict = new Dictionary<string, Texture2D>();
		_labeledCursorDict.Add("",			StandardTexture);
		_labeledCursorDict.Add("circle", 	CircleTexture);
		_labeledCursorDict.Add("diamond", 	DiamondTexture);
		_labeledCursorDict.Add("drop", 		DropTexture);
		_labeledCursorDict.Add("square", 	SquareTexture);
		_labeledCursorDict.Add("star", 		StarTexture);
		_labeledCursorDict.Add("triangle", TriangleTexture);
		
		//Marker objects should be stored under 'Markers' in the Map object
		_labeledMarkerDict = new Dictionary<string, GameObject>();
		_labeledMarkerDict.Add("circle", 	CircleMarker);
		_labeledMarkerDict.Add("diamond", 	DiamondMarker);
		_labeledMarkerDict.Add("drop", 		DropMarker);
		_labeledMarkerDict.Add("square", 	SquareMarker);
		_labeledMarkerDict.Add("star", 		StarMarker);
		_labeledMarkerDict.Add("triangle",	TriangleMarker);
		
		_markerInfoDict = new Dictionary<GameObject, Tuple<string, Vector2>>();
		_markerObjs = new List<GameObject>();
		_gRaycaster = _canvasObj.GetComponent<GraphicRaycaster>();
		_curCursor = "";
		_scale = new Vector3(.65f, .65f, 1);		//Marker Size
		isInitialized = true;
    }
	
	public void SetCursor(string str)
	{
		try
		{
			if(_curCursor == str)
			{
				ResetCursor();
			}
			else{
				markerSelected = true;
				Cursor.SetCursor(_labeledCursorDict[str.ToLower()], Vector2.zero, CursorMode.Auto);
				_curCursor = str.ToLower();
			}
		}
		catch(Exception e)
		{
			Debug.Log("!!!ERROR!!!\nMarkerButtons: " + str + " is not a valid cursor name");
		}
	}

	public string GetCurCursor()
	{
		return _curCursor;
	}
	
	public void ResetCursor()
	{
		markerSelected = false;
		Cursor.SetCursor(StandardTexture, Vector2.zero, CursorMode.Auto);
	}
	
//----------------------------------------------------------------------------------------------------

	public void MapEnabled()
	{
		if (!isInitialized)
			Awake();
		InputHandler.Instance.OnCursorPressed += OnCursorPressed;
		InputHandler.Instance.OnCursorLeftPressed += OnCursorLeftPressed;
		InputHandler.Instance.OnCursorRightPressed += OnCursorRightPressed;
		
	}

	public void MapDisabled()
	{
		InputHandler.Instance.OnCursorPressed -= OnCursorPressed;
		InputHandler.Instance.OnCursorLeftPressed -= OnCursorLeftPressed;
		InputHandler.Instance.OnCursorRightPressed -= OnCursorRightPressed;
		
		ResetCursor();
	}

	private void OnCursorPressed(Vector2 movement)
    {
        mouseMovement = movement;
    }

	private void OnCursorRightPressed()
	{
		if(_curCursor != "")
		{

			Cursor.SetCursor(StandardTexture, Vector2.zero, CursorMode.Auto);
			_curCursor = "";
		}
		else
		{
			Vector2 clickPos = mouseMovement * _canvas.scaleFactor;		//Math for this (and the two following lines) is a little off, but it should be good for now.
			GameObject obj = OverMarker(clickPos);
			if(obj != null)
			{
				Destroy(obj);
			}
		}
	}

	private void OnCursorLeftPressed()
	{
		Vector2 clickPos = mouseMovement * _canvas.scaleFactor;		//Math for this (and the two following lines) is a little off, but it should be good for now.
//Debug.Log("MarkerHandler: " + clickPos);				//Please leave
		if(_curCursor != "")
		{
			if(isRevealed(clickPos))
			{
				CreateMarker(_curCursor, clickPos);
			}
		}
		else
		{
			GameObject obj = OverMarker(clickPos);
			if(obj != null)
			{
				foreach(KeyValuePair<string, GameObject> entry in _labeledMarkerDict)
				{
					if(obj.GetComponent<Image>().sprite == entry.Value.GetComponent<Image>().sprite)
					{
						SetCursor(entry.Key);
						Destroy(obj);
						return;
					}
				}
			}
		}
	}
	
//----------------------------------------------------------------------------------------------------
	
	private bool isRevealed(Vector2 pos)
	{
		List<RaycastResult> hits = GetHits(pos);
		GameObject obj;
		for(int i=0; i<hits.Count; i++)
		{
			obj = hits[i].gameObject;
			Image img = obj.GetComponentInParent(typeof(Image)) as Image;	//consider replacing current verification method with some component unique to the column blocks
			if(img != null)
			{
				if(img.sprite == null)			//assumes that only the column blocks will have null sprite. Not a great assumption
				{
					return img.color == Color.clear;
				}
			}
		}
		return false;
	}
	
	private GameObject CreateMarker(string str, Vector2 pos)
	{
		GameObject obj = new GameObject();
		try
		{
			obj = Instantiate(_labeledMarkerDict[str]);
			obj.transform.SetParent(this.transform.parent, false);
			obj.transform.position = pos;
			obj.transform.localScale = _scale;
			obj.SetActive(true);
			SoundManager.Instance.PlaySFX("pin");
			_markerInfoDict.Add(obj, new Tuple<string, Vector2>(str, pos));
			_markerObjs.Add(obj);
			return obj;
		}
		catch(Exception e)
		{
			Debug.Log("MarkerButtons: cannot create obj " +str);
			if(obj != null)
				Destroy(obj);
			return null;
		}
	}
	
	private GameObject OverMarker(Vector2 pos)
	{
		List<RaycastResult> hits = GetHits(pos);
		GameObject obj;
		for(int i=0; i<hits.Count; i++)
		{
			obj = hits[i].gameObject;
			if(obj.GetComponent<Marker>() != null)
			{
				return obj;
			}
		}
		return null;
	}
	
	private List<RaycastResult> GetHits(Vector2 pos)
	{
		List<RaycastResult> hits = new List<RaycastResult>();
		PointerEventData evData = new PointerEventData(EventSystem.current);
		evData.position = pos;
		_gRaycaster.Raycast(evData, hits);
		return hits;
	}
	
	public List<Tuple<string, int, int>> getMarkersPlaced()
	{
		List<Tuple<string, int, int>> markersPlaced = new List<Tuple<string, int, int>>();
		Tuple<string, Vector2> tuple;
		foreach(GameObject obj in _markerObjs)
		{
			tuple = _markerInfoDict[obj];
			markersPlaced.Add(new Tuple <string, int, int>(tuple.Item1, (int) tuple.Item2.x, (int) tuple.Item2.y));
		}
		return markersPlaced;
	}
	
	public void setMarkersPlaced(List<Tuple<string, int, int>> markersPlaced)
	{
		if (!isInitialized)
			Awake();
		foreach(Tuple<string, int, int> entry in markersPlaced)
		{
			CreateMarker(entry.Item1, new Vector2(entry.Item2, entry.Item3));
		}
	}
}
