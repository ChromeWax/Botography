using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationVisibleOcclusion : MonoBehaviour
{
    private SpriteMask mask;
    private SpriteRenderer spriteRenderer;

    void Start () 
    {
        mask = GetComponent<SpriteMask>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        mask.sprite = spriteRenderer.sprite; 
    }
}
