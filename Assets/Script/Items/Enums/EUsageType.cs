using System;

[Flags]
public enum UsageType
{
    placeable = 1 << 0,
    equippable = 1 << 1,
    consumable = 1 << 2 
}
