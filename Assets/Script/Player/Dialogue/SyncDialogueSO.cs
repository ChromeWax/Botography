using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Botography.Player.Dialogue
{
	/// <summary>
	/// Dialogue that is played while the player is free to make other inputs.
	/// </summary>
	[CreateAssetMenu(menuName = "Dialogue/SyncDialogue")]
	public class SyncDialogueSO : DialogueSO
	{
		public override bool IsAsync()
		{
			return false;
		}
	}
}
