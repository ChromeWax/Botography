using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class containing references to the colors in our color palettes.
/// </summary>
public class ColorConstants : MonoBehaviour
{
    public enum UiColors
    {
        None,
        White,
        Blue,
        Green,
        Black,
        Yellow,
        Red,
    }

    public static readonly Dictionary<UiColors, Color> BaseColorDictionary = new Dictionary<UiColors, Color>() {
        { UiColors.White, Color.white},
		{ UiColors.Blue, Color.blue},
		{ UiColors.Green, Color.green},
        { UiColors.Black, Color.black},
		{ UiColors.Yellow, Color.yellow},
        { UiColors.Red, Color.red}
	};
    
    public static Dictionary<UiColors, Color> GetRegionUiColorDictionary()
    {
        // TODO: Need to expand this function and this class when we have
        // a way to check what region the player is currently in.
        return BaseColorDictionary;
    }
}
