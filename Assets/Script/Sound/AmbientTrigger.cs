using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientTrigger : MonoBehaviour
{
    public AmbientSFX ambientSFX;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")){
            SoundManager.Instance.PlayAmbient(ambientSFX);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player")){
            SoundManager.Instance.StopAmbient();
        }
    }
}
