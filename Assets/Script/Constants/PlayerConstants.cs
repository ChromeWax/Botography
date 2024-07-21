using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Botography.Player
{
	/// <summary>
	/// Constant values related to the player and status effects.
	/// </summary>
	public class PlayerConstants
	{
		// Status Effects
		public enum StatusEffectType
		{
			Cooling,
			Heating,
			Illuminated,
			Underwater
		}
		public const float MaxTimeUnbearable = 1.5f;
		public const int MaxTemp = 2;
		public const int MinTemp = -2;
		public const float TIME_TO_CHECK_ENV_TEMP = 0.5f;
		public const float BaseWalkSpeed = 4;

		public const float UniversalCoyoteTime = 0.25f;

		public static readonly Dictionary<int, float> TEMP_FILL = new()
		{
			{ -2, 0.375f },
			{ -1, 0.465f },
			{ 0, 0.6425f },
			{ 1, 0.8f },
			{ 2, 0.875f }
		};

		public static readonly Dictionary<StatusEffectType, string> UNBEARABLE_ANIM_STRINGS = new()
		{
			{ StatusEffectType.Cooling, "Freezing" },
			{ StatusEffectType.Heating, "Melting" },
		};

		// Dialogue
		public const float DIALOGUE_S_BETWEEN_CHARS = 0.05f;
		public const float DIALOGUE_END_TIME_CONST = 0.05f;
		public const string DEFAULT_DIALOGUE_NAME = "Daphney";

		// Interaction
		public static readonly Dictionary<AttributeValue, string> INTERACTION_ACTION_LABEL = new()
		{
			{ AttributeValue.Loam, "Loam Soil" },
			{ AttributeValue.Sand, "Sand" },
			{ AttributeValue.Silt, "Silt Soil" },
			{ AttributeValue.Clay, "Clay" },
			{ AttributeValue.Neutral, "Neutral Water" },
			{ AttributeValue.Basic, "Basic Water" },
			{ AttributeValue.Acidic, "Acidic Water" }
		};
	}
}
