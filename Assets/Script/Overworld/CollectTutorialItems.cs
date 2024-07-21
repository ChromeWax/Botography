using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectTutorialItems : MonoBehaviour
{
    [SerializeField] GameObject itemHUDIcon, itemUI;
    [SerializeField] GameObject lab;
    bool _dialogueFinished;


    void Start()
    {
        _dialogueFinished = false;
        //itemHUDIcon.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            Tutorial.Instance.TriggerDialogue(itemUI.name, () =>
            {
                itemHUDIcon.gameObject.SetActive(true);
                itemUI.gameObject.SetActive(true);
                Destroy(this.gameObject);
                SoundManager.Instance.PlaySFX("page collected");
                if (lab)
                    lab.SetActive(true);
                //TODO: Get popup to display when UI opens.
            });
    }
    }
}
