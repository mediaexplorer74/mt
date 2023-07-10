using System;

namespace GameManager.GameContent
{
	[Flags]
	public enum GameNotificationType
	{
		None = 0x0,
		Damage = 0x1,
		SpawnOrDeath = 0x2,
		WorldGen = 0x4,
		All = 0x7
	}
}
