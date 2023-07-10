using Microsoft.Xna.Framework;

namespace GameManager.Graphics
{
	public struct VirtualCamera
	{
		public readonly Player Player;

		public Vector2 Position => Center - Size * 0.5f;

		public Vector2 Size => new Vector2(Main.maxScreenW, Main.maxScreenH);

		public Vector2 Center => Player.Center;

		public VirtualCamera(Player player)
		{
			Player = player;
		}
	}
}
