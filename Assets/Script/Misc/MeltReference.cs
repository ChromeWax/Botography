using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeltReference : MonoBehaviour
{
	[SerializeField]
	private List<GameObject> _meltList;
	private Dictionary<GameObject, int> _meltDict;
	private List<int> _rmMeltList;
	
    // Start is called before the first frame update
    void Start()
    {
		_rmMeltList = new List<int>();
		
		int len = _meltList.Count;
		_meltDict = new Dictionary<GameObject, int>();
        for(int i=0; i<len; i++){
			_meltDict.Add(_meltList[i], i);
		}
    }
	
	public void rmMeltable(GameObject meltObj)
	{
		_rmMeltList.Add(_meltDict[meltObj]);
	}
	
	public GameObject getMeltableObj(int index)
	{
		return _meltList[index];
	}
	
	public List<int> getRMList()
	{
		return _rmMeltList;
	}
}
