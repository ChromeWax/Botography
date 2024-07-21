using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoyoteTimeHandler : MonoBehaviour
{
	[SerializeField, HideInInspector] private List<CoyoteTime> _coyotes = new();
	public static CoyoteTimeHandler Instance { get; private set; }
	public static bool CoyoteIsActive = false;

	private void Awake()
	{
		if (Instance != null && Instance != this)
			Destroy(this);
		else
			Instance = this;
	}

	private void Update()
	{
		if (_coyotes.Count != 0)
		{
			for (int i = 0; i < _coyotes.Count;)
			{
				_coyotes[i].CurrentTime += Time.deltaTime;
				if (_coyotes[i].CurrentTime >= _coyotes[i].MaxTime)
				{
					_coyotes[i].InvokeCallback();
					_coyotes.RemoveAt(i);
					continue;
				}
				i++;
			}
		}
	}

	/// <summary>
	/// Creates coyote time and adds it to the active list of coyotes.
	/// </summary>
	/// <param name="maxTime">Once this time elapses, endCallback is invoked.</param>
	/// <param name="endCallback">The action that is to be invoked once coyote time ends.</param>
	/// <returns></returns>
	public CoyoteTime InitiateCoyoteTime(float maxTime, Action endCallback)
	{
		CoyoteTime coyote = new CoyoteTime(maxTime, endCallback);
		return InitiateCoyoteTime(coyote);
	}

	public CoyoteTime InitiateCoyoteTime(CoyoteTime coyote)
	{
		_coyotes.Add(coyote);
		return coyote;
	}

	public bool EndCoyoteTimePrematurely(CoyoteTime coyote)
	{
		return _coyotes.Remove(coyote);
	}
}
