using System.Collections.Generic;

namespace GameManager.GameContent.ItemDropRules
{
	public class CommonDrop : IItemDropRule
	{
		protected int _itemId;

		protected int _dropsOutOfY;

		protected int _amtDroppedMinimum;

		protected int _amtDroppedMaximum;

		protected int _dropsXoutOfY;

		public List<IItemDropRuleChainAttempt> ChainedRules
		{
			get;
			private set;
		}

		public CommonDrop(int itemId, int dropsOutOfY, int amountDroppedMinimum = 1, int amountDroppedMaximum = 1, int dropsXOutOfY = 1)
		{
			_itemId = itemId;
			_dropsOutOfY = dropsOutOfY;
			_amtDroppedMinimum = amountDroppedMinimum;
			_amtDroppedMaximum = amountDroppedMaximum;
			_dropsXoutOfY = dropsXOutOfY;
			ChainedRules = new List<IItemDropRuleChainAttempt>();
		}

		public virtual bool CanDrop(DropAttemptInfo info)
		{
			return true;
		}

		public virtual ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			ItemDropAttemptResult result;
			if (info.player.RollLuck(_dropsOutOfY) < _dropsXoutOfY)
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

		public virtual void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
		{
			float num = (float)_dropsXoutOfY / (float)_dropsOutOfY;
			float dropRate = num * ratesInfo.parentDroprateChance;
			drops.Add(new DropRateInfo(_itemId, _amtDroppedMinimum, _amtDroppedMaximum, dropRate, ratesInfo.conditions));
			Chains.ReportDroprates(ChainedRules, num, drops, ratesInfo);
		}
	}
}
