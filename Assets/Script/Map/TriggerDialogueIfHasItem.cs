using System.Collections;
using System.Collections.Generic;
using Botography.Player.Dialogue;
using UnityEngine;

public class TriggerDialogueIfHasItem : TriggerDialogue
{
    [SerializeField] private string failConvo;
    [SerializeField] private GameObject itemToHave;
    [SerializeField] private bool flip = false;
    [SerializeField] protected bool failedConvoDeleteGameObject = false;

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (flip)
            {
                if (itemToHave)
                {
                    if (convoName != "")
                        DialoguePlayer.Instance.PlayConvo(convoName);
                    if (successConvoDestroyGO)
                    {
			            _dialObj.GetComponent<DialReference>().rmDialObj(gameObject);
                        Destroy(gameObject);
                    }
                }
                else
                {
                    if (failConvo != "")
                        DialoguePlayer.Instance.PlayConvo(failConvo);
                    if (failedConvoDeleteGameObject)
                    {
			            _dialObj.GetComponent<DialReference>().rmDialObj(gameObject);
                        Destroy(gameObject);
                    }
                }
            }
            else
            {
                if (itemToHave)
                {
                    if (failConvo != "")
                        DialoguePlayer.Instance.PlayConvo(failConvo);
                    if (failedConvoDeleteGameObject)
                    {
			            _dialObj.GetComponent<DialReference>().rmDialObj(gameObject);
                        Destroy(gameObject);
                    }
                }
                else
                {
                    if (convoName != "")
                        DialoguePlayer.Instance.PlayConvo(convoName);
                    if (successConvoDestroyGO)
                    {
			            _dialObj.GetComponent<DialReference>().rmDialObj(gameObject);
                        Destroy(gameObject);
                    }
                }
            }

        }
    }
}
