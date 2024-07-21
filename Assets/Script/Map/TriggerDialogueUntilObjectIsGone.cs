using System.Collections;
using System.Collections.Generic;
using Botography.Player.Dialogue;
using UnityEngine;

public class TriggerDialogueUntilObjectIsGone : TriggerDialogue
{
    [SerializeField] private GameObject itemToHave;

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && InventoryDraggable.itemBeingDragged == null)
        {
            if (convoName != "")
            {
			    _dialObj.GetComponent<DialReference>().rmDialObj(gameObject);
                DialoguePlayer.Instance.PlayConvo(convoName);
            }
        }
    }

    private void Update()
    {
        if (itemToHave == null)
            Destroy(gameObject);
    }
}
