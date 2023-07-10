using ReLogic.Content;
using ReLogic.Graphics;

namespace GameManager.GameContent
{
	public static class FontAssets
	{
		public static Asset<DynamicSpriteFont> ItemStack;

		public static Asset<DynamicSpriteFont> MouseText;

		public static Asset<DynamicSpriteFont> DeathText;

		public static Asset<DynamicSpriteFont>[] CombatText = new Asset<DynamicSpriteFont>[2];
	}
}
