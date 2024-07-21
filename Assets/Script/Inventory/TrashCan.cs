using System.Collections;
using System.Collections.Generic;
using Botography.Notifications;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TrashCan : Slot
{
    public override void OnPointerDown(PointerEventData eventData)
	{
		if (DraggableItem.itemBeingDragged != null)
		{
			GameObject gameObjectToBeDeleted = DraggableItem.itemBeingDragged.gameObject;

			if (gameObjectToBeDeleted.GetComponent<Jar>() != null)
			{
				Jar jar = gameObjectToBeDeleted.GetComponent<Jar>();
				if (jar.ItemAmount > 0)
					if (jar.AttemptEmpty() != null)
						{
							Notifyer.Instance.Notify("Removed all samples from jar!");
							SoundManager.Instance.PlaySFX("deselect");
						}
					else
						Notifyer.Instance.Notify("Could not remove sample from jar");
				else if (gameObjectToBeDeleted.GetComponent<MuchroomPlant>() != null)
				{
					Destroy(gameObjectToBeDeleted);
					Notifyer.Instance.Notify("Item has been deleted!");
					SoundManager.Instance.PlaySFX("deselect");
				}
			}
			else
			{
				Destroy(gameObjectToBeDeleted);
				Notifyer.Instance.Notify("Item has been deleted!");
				SoundManager.Instance.PlaySFX("deselect");
			}
		}
	}
}
