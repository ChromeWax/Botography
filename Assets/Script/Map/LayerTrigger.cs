using UnityEngine;
using UnityEngine.Rendering;

public class LayerTrigger : MonoBehaviour
{
    [SerializeField] private string layer;
    [SerializeField] private string sortinglayer;

    private void OnTriggerExit2D(Collider2D other) 
    {
        other.gameObject.layer = LayerMask.NameToLayer(layer);

        other.gameObject.GetComponent<SortingGroup>().sortingLayerName = sortinglayer;
        /*
        SpriteRenderer[] srs = other.gameObject.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in srs)
        {
            sr.sortingLayerName = sortinglayer;
        }
        */
    }
}
