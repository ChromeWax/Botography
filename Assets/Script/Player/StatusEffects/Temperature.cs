using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Botography.Player.PlayerConstants;

namespace Botography.Player.StatusEffects
{
	/// <summary>
	/// Handles the temperature of the character for status effects.
	/// </summary>
	class Temperature
	{
		private int _temp;
		public int Temp
		{
			get { return _temp; }
			set
			{
				if (value > MaxTemp)
				{
					_temp = MaxTemp;
				}
				else if (value < MinTemp)
				{
					_temp = MinTemp;
				}
				else
				{
					_temp = value;
				}
			}
		}

		public Temperature()
		{
			_temp = 0;
		}
	}
}
