using Microsoft.Xna.Framework;
using GameManager.Graphics.Shaders;

namespace GameManager.GameContent.Shaders
{
	public class BloodMoonScreenShaderData : ScreenShaderData
	{
		public BloodMoonScreenShaderData(string passName)
			: base(passName)
		{
		}

		public override void Update(GameTime gameTime)
		{
			float num = 1f - Utils.SmoothStep((float)Main.worldSurface + 50f, (float)Main.rockLayer + 100f, (Main.screenPosition.Y + (float)(Main.screenHeight / 2)) / 16f);
			UseOpacity(num * 0.75f);
		}
	}
}
