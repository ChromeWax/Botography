using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanaceanMusic : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")){
            SoundManager.Instance.PlayPanaceanMusic();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player")){
            SoundManager.Instance.StopPanaceanMusic();
        }
    }
}
