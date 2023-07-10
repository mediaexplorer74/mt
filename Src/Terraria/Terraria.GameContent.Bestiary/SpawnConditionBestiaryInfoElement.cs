using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace GameManager.GameContent.Bestiary
{
	public class SpawnConditionBestiaryInfoElement : FilterProviderInfoElement, IBestiaryBackgroundImagePathAndColorProvider
	{
		private string _backgroundImagePath;

		private Color? _backgroundColor;

		public SpawnConditionBestiaryInfoElement(string nameLanguageKey, int filterIconFrame, string backgroundImagePath = null, Color? backgroundColor = null)
			: base(nameLanguageKey, filterIconFrame)
		{
			_backgroundImagePath = backgroundImagePath;
			_backgroundColor = backgroundColor;
		}

		public Asset<Texture2D> GetBackgroundImage()
		{
			if (_backgroundImagePath == null)
			{
				return null;
			}
			return Main.Assets.Request<Texture2D>(_backgroundImagePath, Main.content, (AssetRequestMode)1);
		}

		public Color? GetBackgroundColor()
		{
			return _backgroundColor;
		}
	}
}
