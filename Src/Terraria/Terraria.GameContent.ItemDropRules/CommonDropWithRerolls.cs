using System.Collections.Generic;

namespace GameManager.GameContent.ItemDropRules
{
	public class CommonDropWithRerolls : CommonDrop
	{
		private int _timesToRoll;

		public CommonDropWithRerolls(int itemId, int dropsOutOfY, int amountDroppedMinimum, int amountDroppedMaximum, int rerolls)
			: base(itemId, dropsOutOfY, amountDroppedMinimum, amountDroppedMaximum)
		{
			_timesToRoll = rerolls + 1;
		}

		public override ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			bool flag = false;
			for (int i = 0; i < _timesToRoll; i++)
			{
				flag = flag || info.player.RollLuck(_dropsOutOfY) < _dropsXoutOfY;
			}
			ItemDropAttemptResult result;
			if (flag)
			{
				CommonCode.DropItemFromNPC(info.npc, _itemId, info.rng.Next(_amtDroppedMinimum, _amtDroppedMaximum + 1));
				result = default(ItemDropAttemptResult);
				result.State = ItemDropAttemptResultState.Success;
				return result;
			}
			result = default(ItemDropAttemptResult);
			result.State = ItemDropAttemptResultState.FailedRandomRoll;
			return result;
		}

		public override void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
		{
			float num = (float)_dropsXoutOfY / (float)_dropsOutOfY;
			float num2 = 1f - num;
			float num3 = 1f;
			for (int i = 0; i < _timesToRoll; i++)
			{
				num3 *= num2;
			}
			float num4 = 1f - num3;
			float dropRate = num4 * ratesInfo.parentDroprateChance;
			drops.Add(new DropRateInfo(_itemId, _amtDroppedMinimum, _amtDroppedMaximum, dropRate, ratesInfo.conditions));
			Chains.ReportDroprates(base.ChainedRules, num4, drops, ratesInfo);
		}
	}
}
