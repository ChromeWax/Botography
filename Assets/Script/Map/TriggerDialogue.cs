using System.Collections;
using System.Collections.Generic;
using Botography.Player.Dialogue;
using UnityEngine;

public class TriggerDialogue : MonoBehaviour
{
    [SerializeField] protected string convoName;
    [SerializeField] protected GameObject _dialObj;
    [SerializeField] protected bool successConvoDestroyGO = true;

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (convoName != "")
                DialoguePlayer.Instance.PlayConvo(convoName, () =>
                {
                    if (successConvoDestroyGO)
                    {
			            _dialObj.GetComponent<DialReference>().rmDialObj(gameObject);
                        Destroy(gameObject);
                    }
                });
        }
    }
}
