using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialReference : MonoBehaviour
{
	[SerializeField]
	private List<GameObject> _dialList;
	private Dictionary<GameObject, int> _dialDict;
	private List<int> _rmDialList;
	
    // Start is called before the first frame update
    void Start()
    {
		_rmDialList = new List<int>();
		
		int len = _dialList.Count;
		_dialDict = new Dictionary<GameObject, int>();
        for(int i=0; i<len; i++){
			_dialDict.Add(_dialList[i], i);
		}
    }
	
	public void rmDialObj(GameObject dialObj)
	{
		_rmDialList.Add(_dialDict[dialObj]);
	}
	
	public GameObject getDialObj(int index)
	{
		return _dialList[index];
	}
	
	public List<int> getRMList()
	{
		return _rmDialList;
	}
}