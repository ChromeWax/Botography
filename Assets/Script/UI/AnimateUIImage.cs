using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimateUIImage : MonoBehaviour
{
    [SerializeField] private Sprite[] _frames;
    [SerializeField] private float _frameRate;

    private Image image;
    [HideInInspector] public int currentFrame = 0;
    [HideInInspector] public float timer;

    private void Start()
    {
        image = GetComponent<Image>();
        image.sprite = _frames[0];
    }

    private void Update()
    {
        timer += Time.deltaTime;
        
        if (timer >= 1 / _frameRate)
        {
            timer = 0;
            currentFrame = (currentFrame + 1) % _frames.Length;
            image.sprite = _frames[currentFrame];
        }
    }

}
