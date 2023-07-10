using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using GameManager.Audio;
using GameManager.GameContent.NetModules;
using GameManager.GameContent.UI.Elements;
using GameManager.GameContent.UI.States;
using GameManager.Localization;
using GameManager.Net;
using GameManager.UI;
using GameManager.UI.Gamepad;

namespace GameManager.GameContent.Creative
{
	public class CreativeUI
	{
		public enum ItemSacrificeResult
		{
			CannotSacrifice,
			SacrificedButNotDone,
			SacrificedAndDone
		}

		public const int ItemSlotIndexes_SacrificeItem = 0;

		public const int ItemSlotIndexes_Count = 1;

		private bool _initialized;

		private Asset<Texture2D> _buttonTexture;

		private Asset<Texture2D> _buttonBorderTexture;

		private Item[] _itemSlotsForUI = new Item[1];

		private List<int> _itemIdsAvailableInfinitely = new List<int>();

		private UserInterface _powersUI = new UserInterface();

		public int GamepadPointIdForInfiniteItemSearchHack = -1;

		public bool GamepadMoveToSearchButtonHack;

		private UICreativePowersMenu _uiState;

		public bool Enabled
		{
			get;
			private set;
		}

		public bool Blocked
		{
			get
			{
				if (Main.LocalPlayer.talkNPC == -1)
				{
					return Main.LocalPlayer.chest != -1;
				}
				return true;
			}
		}

		public CreativeUI()
		{
			for (int i = 0; i < _itemSlotsForUI.Length; i++)
			{
				_itemSlotsForUI[i] = new Item();
			}
		}

		public void Initialize()
		{
			_buttonTexture = Main.Assets.Request<Texture2D>("Images/UI/Creative/Journey_Toggle", Main.content, (AssetRequestMode)1);
			_buttonBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/Creative/Journey_Toggle_MouseOver", Main.content, (AssetRequestMode)1);
			_itemIdsAvailableInfinitely.Clear();
			_uiState = new UICreativePowersMenu();
			_powersUI.SetState(_uiState);
			_initialized = true;
		}

		public void Update(GameTime gameTime)
		{
			if (Enabled && Main.playerInventory)
			{
				_powersUI.Update(gameTime);
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (!_initialized)
			{
				Initialize();
			}
			if (Main.LocalPlayer.difficulty != 3)
			{
				Enabled = false;
			}
			else if (!Blocked)
			{
				Vector2 location = new Vector2(28f, 267f);
				Vector2 value = new Vector2(353f, 258f);
				new Vector2(40f, 267f);
				_ = value + new Vector2(50f, 50f);
				if (Main.screenHeight < 650 && Enabled)
				{
					location.X += 52f * Main.inventoryScale;
				}
				DrawToggleButton(spriteBatch, location);
				if (Enabled)
				{
					_powersUI.Draw(spriteBatch, Main.gameTimeCache);
				}
			}
		}

		public UIElement ProvideItemSlotElement(int itemSlotContext)
		{
			if (itemSlotContext != 0)
			{
				return null;
			}
			return new UIItemSlot(_itemSlotsForUI, itemSlotContext, 30);
		}

		public Item GetItemByIndex(int itemSlotContext)
		{
			if (itemSlotContext != 0)
			{
				return null;
			}
			return _itemSlotsForUI[itemSlotContext];
		}

		public void SetItembyIndex(Item item, int itemSlotContext)
		{
			if (itemSlotContext == 0)
			{
				_itemSlotsForUI[itemSlotContext] = item;
			}
		}

		private void DrawToggleButton(SpriteBatch spritebatch, Vector2 location)
		{
			Vector2 vector = _buttonTexture.Size();
			Rectangle rectangle = Utils.CenteredRectangle(location + vector / 2f, vector);
			UILinkPointNavigator.SetPosition(311, rectangle.Center.ToVector2());
			spritebatch.Draw(_buttonTexture.Value, location, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			Main.LocalPlayer.creativeInterface = false;
			if (rectangle.Contains(Main.MouseScreen.ToPoint()))
			{
				Main.LocalPlayer.creativeInterface = true;
				Main.LocalPlayer.mouseInterface = true;
				if (Enabled)
				{
					Main.instance.MouseText(Language.GetTextValue("CreativePowers.PowersMenuOpen"), 0, 0);
				}
				else
				{
					Main.instance.MouseText(Language.GetTextValue("CreativePowers.PowersMenuClosed"), 0, 0);
				}
				spritebatch.Draw(_buttonBorderTexture.Value, location, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
				if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					ToggleMenu();
				}
			}
		}

		public void SwapItem(Item item)
		{
			Utils.Swap(item, _itemSlotsForUI[0]);
		}

		public void CloseMenu()
		{
			Enabled = false;
			if (_itemSlotsForUI[0].stack > 0)
			{
				_itemSlotsForUI[0] = Main.LocalPlayer.GetItem(Main.myPlayer, _itemSlotsForUI[0], GetItemSettings.InventoryUIToInventorySettings);
			}
		}

		public void ToggleMenu()
		{
			Enabled = !Enabled;
			SoundEngine.PlaySound(12);
			if (Enabled)
			{
				Recipe.FindRecipes();
				Main.LocalPlayer.tileEntityAnchor.Clear();
				RefreshAvailableInfiniteItemsList();
			}
			else if (_itemSlotsForUI[0].stack > 0)
			{
				_itemSlotsForUI[0] = Main.LocalPlayer.GetItem(Main.myPlayer, _itemSlotsForUI[0], GetItemSettings.InventoryUIToInventorySettings);
			}
		}

		public bool IsShowingResearchMenu()
		{
			if (Enabled && _uiState != null)
			{
				return _uiState.IsShowingResearchMenu;
			}
			return false;
		}

		public bool ShouldDrawSacrificeArea()
		{
			if (!_itemSlotsForUI[0].IsAir)
			{
				return true;
			}
			Item mouseItem = Main.mouseItem;
			if (mouseItem.IsAir)
			{
				return false;
			}
			if (!CreativeItemSacrificesCatalog.Instance.TryGetSacrificeCountCapToUnlockInfiniteItems(mouseItem.type, out var amountNeeded))
			{
				return false;
			}
			if (Main.LocalPlayerCreativeTracker.ItemSacrifices.GetSacrificeCount(mouseItem.type) < amountNeeded)
			{
				return true;
			}
			return false;
		}

		public bool GetSacrificeNumbers(out int itemIdChecked, out int amountWeHave, out int amountNeededTotal)
		{
			amountWeHave = 0;
			amountNeededTotal = 0;
			itemIdChecked = 0;
			Item item = _itemSlotsForUI[0];
			if (!item.IsAir)
			{
				itemIdChecked = item.type;
			}
			if (!CreativeItemSacrificesCatalog.Instance.TryGetSacrificeCountCapToUnlockInfiniteItems(item.type, out amountNeededTotal))
			{
				return false;
			}
			Main.LocalPlayerCreativeTracker.ItemSacrifices.SacrificesCountByItemIdCache.TryGetValue(item.type, out amountWeHave);
			return true;
		}

		public ItemSacrificeResult SacrificeItem(out int amountWeSacrificed)
		{
			int amountNeeded = 0;
			amountWeSacrificed = 0;
			Item item = _itemSlotsForUI[0];
			if (!CreativeItemSacrificesCatalog.Instance.TryGetSacrificeCountCapToUnlockInfiniteItems(item.type, out amountNeeded))
			{
				return ItemSacrificeResult.CannotSacrifice;
			}
			int value = 0;
			Main.LocalPlayerCreativeTracker.ItemSacrifices.SacrificesCountByItemIdCache.TryGetValue(item.type, out value);
			int num = Utils.Clamp(amountNeeded - value, 0, amountNeeded);
			if (num == 0)
			{
				return ItemSacrificeResult.CannotSacrifice;
			}
			int num2 = Math.Min(num, item.stack);
			if (!Main.ServerSideCharacter)
			{
				Main.LocalPlayerCreativeTracker.ItemSacrifices.RegisterItemSacrifice(item.type, num2);
			}
			else
			{
				NetPacket packet = NetCreativeUnlocksPlayerReportModule.SerializeSacrificeRequest(item.type, num2);
				NetManager.Instance.SendToServerOrLoopback(packet);
			}
			bool num3 = num2 == num;
			item.stack -= num2;
			if (item.stack <= 0)
			{
				item.TurnToAir();
			}
			amountWeSacrificed = num2;
			RefreshAvailableInfiniteItemsList();
			if (item.stack > 0)
			{
				item.position.X = Main.player[Main.myPlayer].Center.X - (float)(item.width / 2);
				item.position.Y = Main.player[Main.myPlayer].Center.Y - (float)(item.height / 2);
				_itemSlotsForUI[0] = Main.LocalPlayer.GetItem(Main.myPlayer, item, GetItemSettings.InventoryUIToInventorySettings);
			}
			if (!num3)
			{
				return ItemSacrificeResult.SacrificedButNotDone;
			}
			return ItemSacrificeResult.SacrificedAndDone;
		}

		private void RefreshAvailableInfiniteItemsList()
		{
			_itemIdsAvailableInfinitely.Clear();
			CreativeItemSacrificesCatalog.Instance.FillListOfItemsThatCanBeObtainedInfinitely(_itemIdsAvailableInfinitely);
		}

		public void Reset()
		{
			for (int i = 0; i < _itemSlotsForUI.Length; i++)
			{
				_itemSlotsForUI[i].TurnToAir();
			}
			_initialized = false;
			Enabled = false;
		}
	}
}
