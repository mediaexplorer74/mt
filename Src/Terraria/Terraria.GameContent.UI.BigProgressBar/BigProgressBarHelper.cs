using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace GameManager.GameContent.UI.BigProgressBar
{
	public class BigProgressBarHelper
	{
		private const string _bossBarTexturePath = "Images/UI/UI_BossBar";

		public static void DrawBareBonesBar(SpriteBatch spriteBatch, float lifePercent)
		{
			Rectangle rectangle = Utils.CenteredRectangle(Main.ScreenSize.ToVector2() * new Vector2(0.5f, 1f) + new Vector2(0f, -50f), new Vector2(400f, 20f));
			Rectangle destinationRectangle = rectangle;
			destinationRectangle.Inflate(2, 2);
			Texture2D value = TextureAssets.MagicPixel.Value;
			Rectangle value2 = new Rectangle(0, 0, 1, 1);
			Rectangle destinationRectangle2 = rectangle;
			destinationRectangle2.Width = (int)((float)destinationRectangle2.Width * lifePercent);
			spriteBatch.Draw(value, destinationRectangle, value2, Color.White * 0.6f);
			spriteBatch.Draw(value, rectangle, value2, Color.Black * 0.6f);
			spriteBatch.Draw(value, destinationRectangle2, value2, Color.LimeGreen * 0.5f);
		}

		public static void DrawFancyBar(SpriteBatch spriteBatch, float lifePercent, Texture2D barIconTexture, Rectangle barIconFrame)
		{
			Texture2D value = Main.Assets.Request<Texture2D>("Images/UI/UI_BossBar", Main.content, (AssetRequestMode)1).Value;
			Point p = new Point(456, 22);
			Point p2 = new Point(32, 24);
			int verticalFrames = 6;
			Rectangle value2 = value.Frame(1, verticalFrames, 0, 3);
			Color color = Color.White * 0.2f;
			int num = (int)((float)p.X * lifePercent);
			num -= num % 2;
			Rectangle value3 = value.Frame(1, verticalFrames, 0, 2);
			value3.X += p2.X;
			value3.Y += p2.Y;
			value3.Width = 2;
			value3.Height = p.Y;
			Rectangle value4 = value.Frame(1, verticalFrames, 0, 1);
			value4.X += p2.X;
			value4.Y += p2.Y;
			value4.Width = 2;
			value4.Height = p.Y;
			Rectangle r = Utils.CenteredRectangle(Main.ScreenSize.ToVector2() * new Vector2(0.5f, 1f) + new Vector2(0f, -50f), p.ToVector2());
			Vector2 vector = r.TopLeft() - p2.ToVector2();
			spriteBatch.Draw(value, vector, value2, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(value, r.TopLeft(), value3, Color.White, 0f, Vector2.Zero, new Vector2(num / value3.Width, 1f), SpriteEffects.None, 0f);
			spriteBatch.Draw(value, r.TopLeft() + new Vector2(num - 2, 0f), value4, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			Rectangle value5 = value.Frame(1, verticalFrames);
			spriteBatch.Draw(value, vector, value5, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			Vector2 value6 = new Vector2(4f, 20f) + new Vector2(26f, 28f) / 2f;
			spriteBatch.Draw(barIconTexture, vector + value6, barIconFrame, Color.White, 0f, barIconFrame.Size() / 2f, 1f, SpriteEffects.None, 0f);
		}

		public static void DrawFancyBar(SpriteBatch spriteBatch, float lifePercent, Texture2D barIconTexture, Rectangle barIconFrame, float shieldPercent)
		{
			Texture2D value = Main.Assets.Request<Texture2D>("Images/UI/UI_BossBar", Main.content, (AssetRequestMode)1).Value;
			Point p = new Point(456, 22);
			Point p2 = new Point(32, 24);
			int verticalFrames = 6;
			Rectangle value2 = value.Frame(1, verticalFrames, 0, 3);
			Color color = Color.White * 0.2f;
			int num = (int)((float)p.X * lifePercent);
			num -= num % 2;
			Rectangle value3 = value.Frame(1, verticalFrames, 0, 2);
			value3.X += p2.X;
			value3.Y += p2.Y;
			value3.Width = 2;
			value3.Height = p.Y;
			Rectangle value4 = value.Frame(1, verticalFrames, 0, 1);
			value4.X += p2.X;
			value4.Y += p2.Y;
			value4.Width = 2;
			value4.Height = p.Y;
			int num2 = (int)((float)p.X * shieldPercent);
			num2 -= num2 % 2;
			Rectangle value5 = value.Frame(1, verticalFrames, 0, 5);
			value5.X += p2.X;
			value5.Y += p2.Y;
			value5.Width = 2;
			value5.Height = p.Y;
			Rectangle value6 = value.Frame(1, verticalFrames, 0, 4);
			value6.X += p2.X;
			value6.Y += p2.Y;
			value6.Width = 2;
			value6.Height = p.Y;
			Rectangle r = Utils.CenteredRectangle(Main.ScreenSize.ToVector2() * new Vector2(0.5f, 1f) + new Vector2(0f, -50f), p.ToVector2());
			Vector2 vector = r.TopLeft() - p2.ToVector2();
			spriteBatch.Draw(value, vector, value2, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(value, r.TopLeft(), value3, Color.White, 0f, Vector2.Zero, new Vector2(num / value3.Width, 1f), SpriteEffects.None, 0f);
			spriteBatch.Draw(value, r.TopLeft() + new Vector2(num - 2, 0f), value4, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(value, r.TopLeft(), value5, Color.White, 0f, Vector2.Zero, new Vector2(num2 / value5.Width, 1f), SpriteEffects.None, 0f);
			spriteBatch.Draw(value, r.TopLeft() + new Vector2(num2 - 2, 0f), value6, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			Rectangle value7 = value.Frame(1, verticalFrames);
			spriteBatch.Draw(value, vector, value7, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			Vector2 value8 = new Vector2(4f, 20f) + barIconFrame.Size() / 2f;
			spriteBatch.Draw(barIconTexture, vector + value8, barIconFrame, Color.White, 0f, barIconFrame.Size() / 2f, 1f, SpriteEffects.None, 0f);
		}
	}
}
