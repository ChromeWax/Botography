using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    //[SerializeField] private AttributeValue _value;
    private GameObject _highlight;
    private bool _canHighlight;
    // Start is called before the first frame update
    void Start()
    {
        _highlight = gameObject.transform.GetChild(0).gameObject;
        _canHighlight = false;
    }

    // Update is called once per frame
    void Update()
    {
        _highlight.SetActive(_canHighlight);
    }

    public GameObject GetHighlight()
    {
        return _highlight;
    }

    //Turns the highlight on or off
    public void SetHighlight(bool light)
    {
        _canHighlight = light;
    }

    /*
    public AttributeValue GetValue()
    {
        return _value;
    }
    */
}
