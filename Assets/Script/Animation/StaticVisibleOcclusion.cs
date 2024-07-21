using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticVisibleOcclusion : MonoBehaviour
{
    private void Awake () 
    {
        SpriteMask occlusionMask = gameObject.AddComponent<SpriteMask>();
        occlusionMask.sprite = GetComponent<SpriteRenderer>().sprite;
        occlusionMask.enabled = false;
    }
}
