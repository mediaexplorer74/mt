using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameManager.UI;

namespace GameManager.GameContent.UI.Elements
{
	public class UIItemIcon : UIElement
	{
		private Item _item;

		private bool _blackedOut;

		public UIItemIcon(Item item, bool blackedOut)
		{
			_item = item;
			Width.Set(32f, 0f);
			Height.Set(32f, 0f);
			_blackedOut = blackedOut;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			Main.DrawItemIcon(screenPositionForItemCenter: GetDimensions().Center(), spriteBatch: spriteBatch, theItem: _item, itemLightColor: _blackedOut ? Color.Black : Color.White, sizeLimit: 32f);
		}
	}
}
