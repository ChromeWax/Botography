using Botography.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class RespawnHandler : MonoBehaviour
{
    private GameObject _player;
	public static RespawnHandler Instance { get; private set; }

	private void Awake()
	{
		if (Instance != null && Instance != this)
			Destroy(this);
		else
			Instance = this;
	}

	
	void Start()
   {
       _player = PlayerManager.Instance.gameObject;
   }

   void Update()
   {
        
   }

   public void RespawnPlayer()
   {
       throw new NotImplementedException();
   }
}
