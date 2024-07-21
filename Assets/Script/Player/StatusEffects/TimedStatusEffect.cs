using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using static Botography.Player.PlayerConstants;

namespace Botography.Player.StatusEffects
{
	/// <summary>
	/// Status effect that operates on a timer.
	/// </summary>
	public class TimedStatusEffect : StatusEffect
	{
		public float Duration;
		public float CurrentTime;

		public TimedStatusEffect(StatusEffectType type, float time) : base(type)
		{
			Duration = time;
			CurrentTime = time;
		}

		/// <summary>
		/// Returns a copy of the given TimedStatusEffect.
		/// </summary>
		public TimedStatusEffect(TimedStatusEffect effect) : base(effect.Type)
		{
			Duration = effect.Duration;
			CurrentTime = effect.Duration;
		}

		/// <summary>
		/// s1 and s2 must be of the same StatusEffectType.
		/// </summary>
		/// <returns>A new TimedStatusEffect with the times of s1 and s2 added.</returns>
		/// <exception cref="ArgumentException">Throws when the StatusEffectType of s1 and that of s2 don't match.</exception>
		public static TimedStatusEffect AddEffects(TimedStatusEffect s1, TimedStatusEffect s2)
		{
			TimedStatusEffect newTSE = new TimedStatusEffect(s1);
			if (s1.Type == s2.Type)
			{
				newTSE.Duration += s2.Duration;
				newTSE.CurrentTime += s2.CurrentTime;
				return newTSE;
			}
			else
			{
				throw new ArgumentException($"Cannot add TimedStatusEffects of {s1.Type} and {s2.Type}.");
			}
		}

		public static TimedStatusEffect operator +(TimedStatusEffect s1, TimedStatusEffect s2)
		{
			return AddEffects(s1, s2);
		}
	}
}

