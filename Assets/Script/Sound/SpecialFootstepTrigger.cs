using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialFootstepTrigger : MonoBehaviour
{
    public string specialFootstepType="";
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")){
            SoundManager.Instance.TriggerSpecialFootsteps(specialFootstepType);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player")){
            SoundManager.Instance.StopSpecialFootstep();
        }
    }
}
