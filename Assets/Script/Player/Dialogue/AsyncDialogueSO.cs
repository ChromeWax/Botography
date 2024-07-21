using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Botography.Player.Dialogue
{
	/// <summary>
	/// Dialogue that is played and during which the player cannot make inputs except ones that progress dialogue.
	/// </summary>
	[CreateAssetMenu(menuName = "Dialogue/AsyncDialogue")]
	public class AsyncDialogueSO : DialogueSO
	{
		public override bool IsAsync()
		{
			return true;
		}
	}
}
