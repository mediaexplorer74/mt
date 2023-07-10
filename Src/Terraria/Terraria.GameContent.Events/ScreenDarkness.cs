using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameManager.GameContent.Events
{
	public class ScreenDarkness
	{
		public static float screenObstruction;

		public static void Update()
		{
			float value = 0f;
			float amount = 0.1f;
			Vector2 mountedCenter = Main.player[Main.myPlayer].MountedCenter;
			for (int i = 0; i < 200; i++)
			{
				if (Main.npc[i].active && Main.npc[i].type == 370 && Main.npc[i].Distance(mountedCenter) < 3000f && (Main.npc[i].ai[0] >= 10f || (Main.npc[i].ai[0] == 9f && Main.npc[i].ai[2] > 120f)))
				{
					value = 0.95f;
					amount = 0.03f;
				}
			}
			screenObstruction = MathHelper.Lerp(screenObstruction, value, amount);
		}

		public static void DrawBack(SpriteBatch spriteBatch)
		{
			if (screenObstruction != 0f)
			{
				Color color = Color.Black * screenObstruction;
				spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(-2, -2, Main.screenWidth + 4, Main.screenHeight + 4), new Rectangle(0, 0, 1, 1), color);
			}
		}

		public static void DrawFront(SpriteBatch spriteBatch)
		{
			if (screenObstruction != 0f)
			{
				Color color = new Color(0, 0, 120) * screenObstruction * 0.3f;
				spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(-2, -2, Main.screenWidth + 4, Main.screenHeight + 4), new Rectangle(0, 0, 1, 1), color);
			}
		}
	}
}
