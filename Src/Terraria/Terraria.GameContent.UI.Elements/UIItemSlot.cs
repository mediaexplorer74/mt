using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameManager.UI;

namespace GameManager.GameContent.UI.Elements
{
	public class UIItemSlot : UIElement
	{
		private Item[] _itemArray;

		private int _itemIndex;

		private int _itemSlotContext;

		public UIItemSlot(Item[] itemArray, int itemIndex, int itemSlotContext)
		{
			_itemArray = itemArray;
			_itemIndex = itemIndex;
			_itemSlotContext = itemSlotContext;
			Width = new StyleDimension(48f, 0f);
			Height = new StyleDimension(48f, 0f);
		}

		private void HandleItemSlotLogic()
		{
			if (base.IsMouseHovering)
			{
				Main.LocalPlayer.mouseInterface = true;
				Item inv = _itemArray[_itemIndex];
				ItemSlot.OverrideHover(inv, _itemSlotContext);
				ItemSlot.LeftClick(inv, _itemSlotContext);
				ItemSlot.RightClick(inv, _itemSlotContext);
				ItemSlot.MouseHover(inv, _itemSlotContext);
				_itemArray[_itemIndex] = inv;
			}
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			HandleItemSlotLogic();
			Item inv = _itemArray[_itemIndex];
			Vector2 position = GetDimensions().Center() + new Vector2(52f, 52f) * -0.5f * Main.inventoryScale;
			ItemSlot.Draw(spriteBatch, inv, _itemSlotContext, position);
		}
	}
}
