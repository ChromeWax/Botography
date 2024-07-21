using Botography.Dependencies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MenuSystemBase : SystemBase
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected abstract void ToggleMenuOn();

    protected abstract void ToggleMenuOff();

    public void ToggleMenu(bool enabled)
    {
        if (enabled)
        {
            ToggleMenuOn();
        }
        else
        {
            ToggleMenuOff();
        }
    }
}
