using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Botography.Player;

public class PlayerVisibilityBehindObjects : MonoBehaviour
{
    /*
    private List<GameObject> objectsInFrontOfPlayer; 

    private void Start()
    {
        objectsInFrontOfPlayer = new List<GameObject>();
    }

    void Update()
    {
        foreach (var objectInFrontOfPlayer in objectsInFrontOfPlayer)
            objectInFrontOfPlayer.GetComponent<SpriteRenderer>().color = Color.white;
        objectsInFrontOfPlayer.Clear();

        Collider2D[] objectsInsideRadius = Physics2D.OverlapCircleAll((Vector2) transform.position, 5.0f, ~PlayerManager.Instance.gameObject.layer);
        if (objectsInsideRadius != null)
            foreach (var objectInRadius in objectsInsideRadius)
                if (objectInRadius.gameObject.GetComponent<SpriteRenderer>() != null)
                    if (objectInRadius.transform.position.y < transform.position.y)
                    {
                        PolygonCollider2D collider = objectInRadius.AddComponent(PolygonCollider2D);

                        objectsInFrontOfPlayer.Add(objectInRadius.gameObject);
                    }

        foreach (var objectInFrontOfPlayer in objectsInFrontOfPlayer)
            objectInFrontOfPlayer.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
    }
    */

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.CompareTag("Environment"))
        {
            SpriteRenderer spriteRenderder = collider2D.gameObject.GetComponent<SpriteRenderer>();

            if (spriteRenderder != null && collider2D.transform.position.y < transform.position.y)
            {
                spriteRenderder.color = new Color(1f, 1f, 1f, 0.5f);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.CompareTag("Environment"))
        {
            SpriteRenderer spriteRenderder = collider2D.gameObject.GetComponent<SpriteRenderer>();

            if (spriteRenderder != null)
            {
                spriteRenderder.color = Color.white;
            }
        }
    }
}
