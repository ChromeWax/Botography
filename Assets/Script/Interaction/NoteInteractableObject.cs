using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteInteractableObject : InteractableObject
{
    [SerializeField] private NoteSO _note;

    public NoteSO Note => _note;
}
