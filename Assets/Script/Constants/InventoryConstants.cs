using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

/// <summary>
/// A class containing references to constants required for the inventory.
/// </summary>
public static class InventoryConstants
{
    public const int INVENTORY_CAPACITY = 16;

    public enum InventoryItemType
    {
        Generic,
        Jar,
        Muchroom,
        PlaceableRadius,
        PlaceableInteractive,
        Expendable,
        EquippableFeet,
        EquippableHead,
        EquippableTorso,
        Panacean
    }

    public static readonly Dictionary<AttributeValue?, SoilType> SOIL_ATTRIBUTE_DICTIONARY = new Dictionary<AttributeValue?, SoilType>()
    {
        { AttributeValue.Silt, SoilType.silt },
        { AttributeValue.Sand, SoilType.sand },
        { AttributeValue.Loam, SoilType.loam },
        { AttributeValue.Clay, SoilType.clay }
    };

	public static readonly Dictionary<AttributeValue?, WaterPh> WATER_ATTRIBUTE_DICTIONARY = new Dictionary<AttributeValue?, WaterPh>()
	{
		{ AttributeValue.Basic, WaterPh.basic },
		{ AttributeValue.Neutral, WaterPh.neutral },
		{ AttributeValue.Acidic, WaterPh.acidic }
	};

    public static readonly Dictionary<SoilType, AttributeValue?> SOILTYPE_DICTIONARY = new Dictionary<SoilType, AttributeValue?>()
    {
        { SoilType.silt, AttributeValue.Silt },
        { SoilType.clay, AttributeValue.Clay },
        { SoilType.loam, AttributeValue.Loam },
        { SoilType.sand, AttributeValue.Sand }
    };

	public static readonly Dictionary<WaterPh, AttributeValue?> WATERPH_DICTIONARY = new Dictionary<WaterPh, AttributeValue?>()
	{
        { WaterPh.neutral, AttributeValue.Neutral},
        { WaterPh.basic, AttributeValue.Basic}, 
        { WaterPh.acidic, AttributeValue.Acidic}
	};

    public static readonly Dictionary<InventoryItemType, string> InventoryPrefabPaths = new Dictionary<InventoryItemType, string>()
    {
        { InventoryItemType.Jar, "Assets/Wetphal/Prefabs/Jar.prefab" }
    };
}
