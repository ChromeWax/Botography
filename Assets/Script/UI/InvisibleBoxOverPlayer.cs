using System.Collections;
using System.Collections.Generic;
using Botography.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InvisibleBoxOverPlayer : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    // Update is called once per frame
    /*
    private void Update()
    {
        Vector3 newPosition = Camera.main.WorldToScreenPoint(PlayerManager.Instance.GetCurrentPlayerPosition());
        newPosition = new Vector3(newPosition.x, newPosition.y + 100 ,newPosition.z);
        transform.position = newPosition;
    }
    */

    public void OnPointerClick(PointerEventData eventData)
    {
        ConsumptionManager.Instance.OnPointerClick(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ConsumptionManager.Instance.OnPointerEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ConsumptionManager.Instance.OnPointerExit();
    }
}
