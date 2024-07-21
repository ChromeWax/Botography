using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitterTrigger : MonoBehaviour
{
    public List<AmbientSFX> emitterSFX;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")){
            foreach(var emitter in emitterSFX){
                SoundManager.Instance.PlayEmitter(emitter);
            }
        }
    }
}
