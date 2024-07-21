using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndAnimationLock : MonoBehaviour
{
    public void EndAnimLock()
	{
		PlayerStateMachine.Instance.BindControls();
	}
}
