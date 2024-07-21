using Botography.Player;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerVisibleOcclusion : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _playerSpriteRenderer;
    [SerializeField] private SpriteRenderer _headSpriteRenderer;
    [SerializeField] private SpriteRenderer _bodySpriteRenderer;
    [SerializeField] private SpriteRenderer _legSpriteRenderer;

    [SerializeField] private SpriteRenderer _occlusionPlayerSpriteRenderer;
    [SerializeField] private SpriteRenderer _occlusionHeadSpriteRenderer;
    [SerializeField] private SpriteRenderer _occlusionBodySpriteRenderer;
    [SerializeField] private SpriteRenderer _occlusionLegSpriteRenderer;

    private List<SpriteMask> maskList = new List<SpriteMask>();
    private bool checking = false;

    public void Update()
    {
        transform.position = PlayerManager.Instance.transform.position;
        gameObject.layer = PlayerManager.Instance.gameObject.layer;

        _occlusionPlayerSpriteRenderer.sprite = _playerSpriteRenderer.sprite;
        _occlusionHeadSpriteRenderer.sprite = _headSpriteRenderer.sprite;
        _occlusionBodySpriteRenderer.sprite = _bodySpriteRenderer.sprite;
        _occlusionLegSpriteRenderer.sprite = _legSpriteRenderer.sprite;

        _occlusionPlayerSpriteRenderer.enabled = _playerSpriteRenderer.enabled;
        _occlusionHeadSpriteRenderer.enabled = _headSpriteRenderer.enabled;
        _occlusionBodySpriteRenderer.enabled = _bodySpriteRenderer.enabled;
        _occlusionLegSpriteRenderer.enabled = _legSpriteRenderer.enabled;

        if (checking)
        {
            foreach (SpriteMask spriteMask in maskList)
            {
                if (transform.position.y > spriteMask.transform.position.y)
                    spriteMask.enabled = true;
                else
                    spriteMask.enabled = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SpriteMask spriteMask = collision.GetComponent<SpriteMask>();
        if (spriteMask != null)
        {
            maskList.Add(spriteMask);
            checking = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        SpriteMask spriteMask = collision.GetComponent<SpriteMask>();
        if (spriteMask != null)
        {
            spriteMask.enabled = false;
            maskList.Remove(spriteMask);
            if (maskList.Count <= 0)
                checking = false;
        }
    }
}