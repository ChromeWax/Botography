using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicRegionTag : MonoBehaviour
{
    [SerializeField] string Region;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")){
            SoundManager.Instance.SwitchMusic(Region);
        }
    }
}
