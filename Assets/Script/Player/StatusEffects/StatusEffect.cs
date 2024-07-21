using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Botography.Player.PlayerConstants;

namespace Botography.Player.StatusEffects
{
	/// <summary>
	/// Base class for status effects. 
	/// </summary>
	public abstract class StatusEffect
	{
		public StatusEffectType Type;

		protected StatusEffect(StatusEffectType type)
		{
			Type = type;
		}
	}
}
