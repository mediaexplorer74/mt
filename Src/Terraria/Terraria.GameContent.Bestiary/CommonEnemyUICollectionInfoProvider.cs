using GameManager.UI;

namespace GameManager.GameContent.Bestiary
{
	public class CommonEnemyUICollectionInfoProvider : IBestiaryUICollectionInfoProvider
	{
		private string _persistentIdentifierToCheck;

		private bool _quickUnlock;

		public CommonEnemyUICollectionInfoProvider(string persistentId, bool quickUnlock)
		{
			_persistentIdentifierToCheck = persistentId;
			_quickUnlock = quickUnlock;
		}

		public BestiaryUICollectionInfo GetEntryUICollectionInfo()
		{
			BestiaryEntryUnlockState unlockStateByKillCount = GetUnlockStateByKillCount(Main.BestiaryTracker.Kills.GetKillCount(_persistentIdentifierToCheck), _quickUnlock);
			BestiaryUICollectionInfo result = default(BestiaryUICollectionInfo);
			result.UnlockState = unlockStateByKillCount;
			return result;
		}

		public static BestiaryEntryUnlockState GetUnlockStateByKillCount(int killCount, bool quickUnlock)
		{
			BestiaryEntryUnlockState bestiaryEntryUnlockState = BestiaryEntryUnlockState.NotKnownAtAll_0;
			if (quickUnlock && killCount > 0)
			{
				return BestiaryEntryUnlockState.CanShowDropsWithDropRates_4;
			}
			if (killCount >= 50)
			{
				return BestiaryEntryUnlockState.CanShowDropsWithDropRates_4;
			}
			if (killCount >= 25)
			{
				return BestiaryEntryUnlockState.CanShowDropsWithoutDropRates_3;
			}
			if (killCount >= 10)
			{
				return BestiaryEntryUnlockState.CanShowStats_2;
			}
			if (killCount >= 1)
			{
				return BestiaryEntryUnlockState.CanShowPortraitOnly_1;
			}
			return BestiaryEntryUnlockState.NotKnownAtAll_0;
		}

		public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
		{
			return null;
		}
	}
}
