using GameManager.Localization;
using GameManager.UI;

namespace GameManager.GameContent.Bestiary
{
	public class RareSpawnBestiaryInfoElement : IBestiaryInfoElement, IProvideSearchFilterString
	{
		public int RarityLevel
		{
			get;
			private set;
		}

		public RareSpawnBestiaryInfoElement(int rarityLevel)
		{
			RarityLevel = rarityLevel;
		}

		public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
		{
			return null;
		}

		public string GetSearchString(BestiaryUICollectionInfo info)
		{
			if (info.UnlockState == BestiaryEntryUnlockState.NotKnownAtAll_0)
			{
				return null;
			}
			return Language.GetText("BestiaryInfo.IsRare").Value;
		}
	}
}
