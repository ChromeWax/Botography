using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Botography.Player;
using UnityEngine;

[System.Serializable]
public enum DirectionType
{
    closest,
    queue
}

public class DirectionIndicator : MonoBehaviour
{
    public static DirectionIndicator Instance { get; private set;}

    [SerializeField] private List<GameObject> lookAtObjects;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private DirectionType directionType = DirectionType.queue;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);
    }


    void Update()
    {
        foreach (GameObject go in lookAtObjects.ToList<GameObject>()) 
            if (go == null || go.activeInHierarchy == false)
                lookAtObjects.Remove(go);

        if (lookAtObjects.Count > 0)
        {
            if (directionType == DirectionType.closest)
            {
                float shortestDistance = Mathf.Infinity;
                GameObject closestGameObject = null;

                for (int i = 0; i < lookAtObjects.Count; i++)
                {
                    float distance = (lookAtObjects[i].transform.position - transform.position).magnitude;
                    if (distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        closestGameObject = lookAtObjects[i];
                    }
                }

                transform.rotation = Quaternion.LookRotation(Vector3.forward, closestGameObject.transform.position - transform.position);
                transform.eulerAngles = transform.eulerAngles + new Vector3(0, 0, 90);

                transform.position = PlayerManager.Instance.transform.position + new Vector3(0, 0.5f, 0);

                if ((closestGameObject.transform.position - transform.position).magnitude < 2)
                    spriteRenderer.enabled = false;
                else
                    spriteRenderer.enabled = true;
            }
            else if (directionType == DirectionType.queue)
            {
                transform.rotation = Quaternion.LookRotation(Vector3.forward, lookAtObjects[0].transform.position - transform.position);
                transform.eulerAngles = transform.eulerAngles + new Vector3(0, 0, 90);

                transform.position = PlayerManager.Instance.transform.position + new Vector3(0, 0.5f, 0);

                if ((lookAtObjects[0].transform.position - transform.position).magnitude < 2)
                    spriteRenderer.enabled = false;
                else
                    spriteRenderer.enabled = true;
            }
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }

    public void AddToDirectionIndicator(GameObject go)
    {
        lookAtObjects.Add(go);
    }
}
