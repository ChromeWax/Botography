using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Botography.Dependencies;

public class CloseMenuButton : MonoBehaviour
{
    public void CloseMenu()
	{
		UIManager.Instance.OnExitPressed();
	}
}
