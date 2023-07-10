using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace GameManager.GameContent.Bestiary
{
	public interface IBestiaryBackgroundImagePathAndColorProvider
	{
		Asset<Texture2D> GetBackgroundImage();

		Color? GetBackgroundColor();
	}
}
