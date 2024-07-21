using UnityEngine;

public class SwitchPostProcessing : MonoBehaviour
{
    [SerializeField] string postProcessType;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PostProcessManager.Instance.ChangePostProcessType(postProcessType);
        }
    }
}