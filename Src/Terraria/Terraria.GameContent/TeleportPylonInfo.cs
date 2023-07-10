using System;
using GameManager.DataStructures;

namespace GameManager.GameContent
{
	public struct TeleportPylonInfo : IEquatable<TeleportPylonInfo>
	{
		public Point16 PositionInTiles;

		public TeleportPylonType TypeOfPylon;

		public bool Equals(TeleportPylonInfo other)
		{
			if (PositionInTiles == other.PositionInTiles)
			{
				return TypeOfPylon == other.TypeOfPylon;
			}
			return false;
		}
	}
}
