using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabToggler : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject labTab;
    public bool isOpen;
    void Start()
    {
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (isOpen)
            {
                CloseLab();
                isOpen = false;
            }
            else
            {
                labTab.SetActive(true);
                isOpen = true;
            }
        }
    }
    public void CloseLab()
    {
        labTab.SetActive(false);
    }
}
