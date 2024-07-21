using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PanaceanSlot : Slot
{
    protected override bool ValidSlot(PointerEventData eventData)
	{
		return false;
	}
}
