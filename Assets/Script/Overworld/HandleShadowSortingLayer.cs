using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleShadowSortingLayer : MonoBehaviour
{
    private void Start()
    {
        if (transform.parent != null && transform.parent.GetComponent<SpriteRenderer>() != null && transform.tag != "Player")
        {
            GetComponent<SpriteRenderer>().sortingLayerID = transform.parent.GetComponent<SpriteRenderer>().sortingLayerID;
        }
    }
}
