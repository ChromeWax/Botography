using System.Collections;
using System.Collections.Generic;
using Botography.Dependencies;
using Unity.VisualScripting;
using UnityEngine;

public class TransitionManager: ManagerBase<TransitionSystem, TransitionUI>
{
	public static TransitionManager Instance { get; private set; }

    private void Awake() 
	{
		if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;	
	}

    public void TransitionToBlackScreen()
    {
        uiCollection.FadeToBlack.Play("ToBlackScreen");
    }

    public void TransitionToClearScreen()
    {
        uiCollection.FadeToBlack.Play("ToClearScreen");
    }
}
