using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class EquipmentOverlay : MonoBehaviour
{
    [SerializeField] private GameObject _headOverlay;
    [SerializeField] private GameObject _bodyOverlay;
    [SerializeField] private GameObject _legOverlay;
    [SerializeField] private GameObject _headSlot;
    [SerializeField] private GameObject _bodySlot;
    [SerializeField] private GameObject _legSlot;

    private SpriteRenderer _headSpriteRenderer;
    private SpriteRenderer _bodySpriteRenderer;
    private SpriteRenderer _legSpriteRenderer;

    private Animator _characterAnimator;
    private Animator _headAnimator;
    private Animator _bodyAnimator;
    private Animator _legAnimator;

    private void Start()
    {
        _headSpriteRenderer = _headOverlay.GetComponent<SpriteRenderer>();
        _bodySpriteRenderer = _bodyOverlay.GetComponent<SpriteRenderer>();
        _legSpriteRenderer = _legOverlay.GetComponent<SpriteRenderer>();
        
        _characterAnimator = GetComponent<Animator>();
        _headAnimator = _headOverlay.GetComponent<Animator>();
        _bodyAnimator = _bodyOverlay.GetComponent<Animator>();
        _legAnimator = _legOverlay.GetComponent<Animator>();
    }

    public void Update()
    {
        float horizontal = _characterAnimator.GetFloat("Horizontal");
        float vertical = _characterAnimator.GetFloat("Vertical");
        float idleHorizontal = _characterAnimator.GetFloat("IdleHorizontal");
        float idleVertical = _characterAnimator.GetFloat("IdleVertical");
        float speed = _characterAnimator.GetFloat("Speed");
        bool grounded = _characterAnimator.GetBool("Grounded");

        // Sets layer
        _headOverlay.layer = gameObject.layer;
        _bodyOverlay.layer = gameObject.layer;
        _legOverlay.layer = gameObject.layer;

        // Sets animator to be identical to character
        _headAnimator.SetFloat("Horizontal", horizontal);
        _headAnimator.SetFloat("Vertical", vertical);
        _headAnimator.SetFloat("IdleHorizontal", idleHorizontal);
        _headAnimator.SetFloat("IdleVertical", idleVertical);
        _headAnimator.SetFloat("Speed", speed);
        _headAnimator.SetBool("Grounded", grounded);

        _bodyAnimator.SetFloat("Horizontal", horizontal);
        _bodyAnimator.SetFloat("Vertical", vertical);
        _bodyAnimator.SetFloat("IdleHorizontal", idleHorizontal);
        _bodyAnimator.SetFloat("IdleVertical", idleVertical);
        _bodyAnimator.SetFloat("Speed", speed);
        _bodyAnimator.SetBool("Grounded", grounded);

        _legAnimator.SetFloat("Horizontal", horizontal);
        _legAnimator.SetFloat("Vertical", vertical);
        _legAnimator.SetFloat("IdleHorizontal", idleHorizontal);
        _legAnimator.SetFloat("IdleVertical", idleVertical);
        _legAnimator.SetFloat("Speed", speed);
        _legAnimator.SetBool("Grounded", grounded);

        // Enables/Disables rendering of overlays depending on if plant is equipped
        _headSpriteRenderer.enabled = _headSlot.transform.childCount > 1;
        _bodySpriteRenderer.enabled = _bodySlot.transform.childCount > 1;
        _legSpriteRenderer.enabled = _legSlot.transform.childCount > 1;
    }
}
