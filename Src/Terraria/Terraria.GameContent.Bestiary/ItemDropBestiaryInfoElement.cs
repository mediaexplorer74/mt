using GameManager.GameContent.ItemDropRules;
using GameManager.GameContent.UI.Elements;
using GameManager.ID;
using GameManager.UI;

namespace GameManager.GameContent.Bestiary
{
	public class ItemDropBestiaryInfoElement : IItemBestiaryInfoElement, IBestiaryInfoElement, IProvideSearchFilterString
	{
		protected DropRateInfo _droprateInfo;

		public ItemDropBestiaryInfoElement(DropRateInfo info)
		{
			_droprateInfo = info;
		}

		public virtual UIElement ProvideUIElement(BestiaryUICollectionInfo info)
		{
			bool flag = ShouldShowItem(_droprateInfo);
			if (info.UnlockState < BestiaryEntryUnlockState.CanShowStats_2)
			{
				flag = false;
			}
			if (!flag)
			{
				return null;
			}
			return new UIBestiaryInfoItemLine(_droprateInfo, info);
		}

		private static bool ShouldShowItem(DropRateInfo dropRateInfo)
		{
			bool result = true;
			if (dropRateInfo.conditions != null && dropRateInfo.conditions.Count > 0)
			{
				for (int i = 0; i < dropRateInfo.conditions.Count; i++)
				{
					if (!dropRateInfo.conditions[i].CanShowItemDropInUI())
					{
						result = false;
						break;
					}
				}
			}
			return result;
		}

		public string GetSearchString(BestiaryUICollectionInfo info)
		{
			bool flag = ShouldShowItem(_droprateInfo);
			if (info.UnlockState < BestiaryEntryUnlockState.CanShowStats_2)
			{
				flag = false;
			}
			if (!flag)
			{
				return null;
			}
			return ContentSamples.ItemsByType[_droprateInfo.itemId].Name;
		}
	}
}
