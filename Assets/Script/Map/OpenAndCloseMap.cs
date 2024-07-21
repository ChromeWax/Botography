using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenAndCloseMap : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isOpen;
    public GameObject map;
    void Start()
    {
        isOpen = map.activeInHierarchy;
    }

    // Update is called once per frame
    void Update()
    {
        // Not Necessary
        /*
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (isOpen)
            {
                CloseMap();
                isOpen = false;
            }
            else
            {
                map.SetActive(true);
                isOpen = true;
            }
        }
        */
    }

    public void CloseMap()
    {
        map.SetActive(false);
    }
}
