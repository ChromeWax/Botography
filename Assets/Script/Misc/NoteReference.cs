using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteReference : MonoBehaviour
{
	[SerializeField]
	private List<GameObject> _noteList;
	private Dictionary<GameObject, int> _noteDict;
	private List<int> _rmNoteList;
	
    // Start is called before the first frame update
    void Start()
    {
		_rmNoteList = new List<int>();
		
		int len = _noteList.Count;
		_noteDict = new Dictionary<GameObject, int>();
        for(int i=0; i<len; i++){
			_noteDict.Add(_noteList[i], i);
		}
    }
	
	public void rmNote(GameObject noteObj)
	{
		Debug.Log(_noteDict.Count);
		_rmNoteList.Add(_noteDict[noteObj]);
	}
	
	public GameObject getNoteObj(int index)
	{
		return _noteList[index];
	}
	
	public List<int> getRMList()
	{
		return _rmNoteList;
	}
}
