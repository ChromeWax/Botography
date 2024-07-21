using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlmanacToggle : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject almanac;
    public bool isOpen;
    void Start()
    {
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isOpen)
            {
                CloseAlmanac();
                isOpen = false;
            }
            else
            {
                almanac.SetActive(true);
                isOpen = true;
            }
        }
    }
    public void CloseAlmanac()
    {
        almanac.SetActive(false);
    }
}

