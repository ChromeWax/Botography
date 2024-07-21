using System;

[Flags]
public enum EquipmentEffect
{
	None = 0,
	waterWalk = 1 << 0,
	pestProtection = 1 << 1,
	walkUnderwater = 1 << 2
}
