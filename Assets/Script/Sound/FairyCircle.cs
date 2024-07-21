using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyCircle : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")){
            SoundManager.Instance.PlayFairyMusic();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player")){
            SoundManager.Instance.StopFairyMusic();
        }
    }
}
