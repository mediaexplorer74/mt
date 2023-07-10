using System;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using GameManager.DataStructures;
using GameManager.GameContent.UI.Elements;
using GameManager.ID;
using GameManager.UI;

namespace GameManager.GameContent.Creative
{
	public static class ItemFilters
	{
		public class BySearch : IItemEntryFilter, IEntryFilter<Item>, ISearchFilter<Item>
		{
			private const int _tooltipMaxLines = 30;

			private string[] _toolTipLines = new string[30];

			private bool[] _unusedPrefixLine = new bool[30];

			private bool[] _unusedBadPrefixLines = new bool[30];

			private int _unusedYoyoLogo;

			private int _unusedResearchLine;

			private string _search;

			public bool FitsFilter(Item entry)
			{
				if (_search == null)
				{
					return true;
				}
				int numLines = 1;
				float knockBack = entry.knockBack;
				Main.MouseText_DrawItemTooltip_GetLinesInfo(entry, _unusedYoyoLogo, _unusedResearchLine, knockBack, numLines, _toolTipLines, _unusedPrefixLine, _unusedBadPrefixLines);
				for (int i = 0; i < numLines; i++)
				{
					if (_toolTipLines[i].ToLower().IndexOf(_search, StringComparison.OrdinalIgnoreCase) != -1)
					{
						return true;
					}
				}
				return false;
			}

			public string GetDisplayNameKey()
			{
				return "CreativePowers.TabSearch";
			}

			public UIElement GetImage()
			{
				Asset<Texture2D> obj = Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Icon_Rank_Light", Main.content, (AssetRequestMode)1);
				return new UIImageFramed(obj, obj.Frame())
				{
					HAlign = 0.5f,
					VAlign = 0.5f
				};
			}

			public void SetSearch(string searchText)
			{
				_search = searchText;
			}
		}

		public class BuildingBlock : IItemEntryFilter, IEntryFilter<Item>
		{
			public bool FitsFilter(Item entry)
			{
				if (entry.createTile == -1 && entry.createWall == -1)
				{
					return entry.tileWand != -1;
				}
				return true;
			}

			public string GetDisplayNameKey()
			{
				return "CreativePowers.TabBlocks";
			}

			public UIElement GetImage()
			{
				Asset<Texture2D> obj = Main.Assets.Request<Texture2D>("Images/UI/Creative/Infinite_Icons", Main.content, (AssetRequestMode)1);
				return new UIImageFramed(obj, obj.Frame(9, 1, 4).OffsetSize(-2, 0))
				{
					HAlign = 0.5f,
					VAlign = 0.5f
				};
			}
		}

		public class Weapon : IItemEntryFilter, IEntryFilter<Item>
		{
			public bool FitsFilter(Item entry)
			{
				return entry.damage > 0;
			}

			public string GetDisplayNameKey()
			{
				return "CreativePowers.TabWeapons";
			}

			public UIElement GetImage()
			{
				Asset<Texture2D> obj = Main.Assets.Request<Texture2D>("Images/UI/Creative/Infinite_Icons", Main.content, (AssetRequestMode)1);
				return new UIImageFramed(obj, obj.Frame(9).OffsetSize(-2, 0))
				{
					HAlign = 0.5f,
					VAlign = 0.5f
				};
			}
		}

		public class Armor : IItemEntryFilter, IEntryFilter<Item>
		{
			public bool FitsFilter(Item entry)
			{
				if (entry.bodySlot == -1 && entry.headSlot == -1)
				{
					return entry.legSlot != -1;
				}
				return true;
			}

			public string GetDisplayNameKey()
			{
				return "CreativePowers.TabArmor";
			}

			public UIElement GetImage()
			{
				Asset<Texture2D> obj = Main.Assets.Request<Texture2D>("Images/UI/Creative/Infinite_Icons", Main.content, (AssetRequestMode)1);
				return new UIImageFramed(obj, obj.Frame(9, 1, 2).OffsetSize(-2, 0))
				{
					HAlign = 0.5f,
					VAlign = 0.5f
				};
			}
		}

		public class Accessories : IItemEntryFilter, IEntryFilter<Item>
		{
			public bool FitsFilter(Item entry)
			{
				return entry.accessory;
			}

			public string GetDisplayNameKey()
			{
				return "CreativePowers.TabAccessories";
			}

			public UIElement GetImage()
			{
				Asset<Texture2D> obj = Main.Assets.Request<Texture2D>("Images/UI/Creative/Infinite_Icons", Main.content, (AssetRequestMode)1);
				return new UIImageFramed(obj, obj.Frame(9, 1, 1).OffsetSize(-2, 0))
				{
					HAlign = 0.5f,
					VAlign = 0.5f
				};
			}
		}

		public class Consumables : IItemEntryFilter, IEntryFilter<Item>
		{
			public bool FitsFilter(Item entry)
			{
				bool flag = entry.createTile != -1 || entry.createWall != -1 || entry.tileWand != -1;
				if (entry.consumable)
				{
					return !flag;
				}
				return false;
			}

			public string GetDisplayNameKey()
			{
				return "CreativePowers.TabConsumables";
			}

			public UIElement GetImage()
			{
				Asset<Texture2D> obj = Main.Assets.Request<Texture2D>("Images/UI/Creative/Infinite_Icons", Main.content, (AssetRequestMode)1);
				return new UIImageFramed(obj, obj.Frame(9, 1, 3).OffsetSize(-2, 0))
				{
					HAlign = 0.5f,
					VAlign = 0.5f
				};
			}
		}

		public class GameplayItems : IItemEntryFilter, IEntryFilter<Item>
		{
			public bool FitsFilter(Item entry)
			{
				return ItemID.Sets.SortingPriorityBossSpawns[entry.type] != -1;
			}

			public string GetDisplayNameKey()
			{
				return "CreativePowers.TabMisc";
			}

			public UIElement GetImage()
			{
				Asset<Texture2D> obj = Main.Assets.Request<Texture2D>("Images/UI/Creative/Infinite_Icons", Main.content, (AssetRequestMode)1);
				return new UIImageFramed(obj, obj.Frame(9, 1, 5).OffsetSize(-2, 0))
				{
					HAlign = 0.5f,
					VAlign = 0.5f
				};
			}
		}

		public class Materials : IItemEntryFilter, IEntryFilter<Item>
		{
			public bool FitsFilter(Item entry)
			{
				return entry.material;
			}

			public string GetDisplayNameKey()
			{
				return "CreativePowers.TabMaterials";
			}

			public UIElement GetImage()
			{
				Asset<Texture2D> obj = Main.Assets.Request<Texture2D>("Images/UI/Creative/Infinite_Icons", Main.content, (AssetRequestMode)1);
				return new UIImageFramed(obj, obj.Frame(9, 1, 6).OffsetSize(-2, 0))
				{
					HAlign = 0.5f,
					VAlign = 0.5f
				};
			}
		}

		private const int framesPerRow = 9;

		private const int framesPerColumn = 1;

		private const int frameSizeOffsetX = -2;

		private const int frameSizeOffsetY = 0;
	}
}
