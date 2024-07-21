using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "NoteSO", menuName = "Note")]
public class NoteSO : ScriptableObject
{
    [SerializeField] private string _author;
    [SerializeField, TextArea(3, 10)] private string _description;
    //[SerializeField] private string _coord;

    public string Author => _author;
    public string Description => _description;
    //public string Coord => _coord;
}
