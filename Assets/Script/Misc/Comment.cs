using UnityEngine;

public class Comment : MonoBehaviour
{
    [SerializeField, TextArea] private string _notes = "Comment Here";

    private void Start()
    {
        _notes = "";
    }
}
