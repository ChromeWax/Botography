using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A data structure for holding information about coyote time
/// </summary>
public class CoyoteTime
{
    public float MaxTime;
    public float CurrentTime;
    private Action _endCallback;

    public CoyoteTime(float maxTime, Action callback)
    {
        MaxTime = maxTime;
        _endCallback = callback;
        CurrentTime = 0;
    }

    public void InvokeCallback()
    {
        _endCallback.Invoke();
    }
}
