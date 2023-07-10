using System;

namespace GameManager.DataStructures
{
	[Flags]
	public enum TileDataType
	{
		Tile = 0x1,
		TilePaint = 0x2,
		Wall = 0x4,
		WallPaint = 0x8,
		Liquid = 0x10,
		Wiring = 0x20,
		Actuator = 0x40,
		Slope = 0x80,
		All = 0xFF
	}
}
