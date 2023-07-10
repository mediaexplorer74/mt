using System.Collections.Generic;

namespace GameManager.GameContent.ItemDropRules
{
	public class ItemDropWithConditionRule : CommonDrop
	{
		private IItemDropRuleCondition _condition;

		public ItemDropWithConditionRule(int itemId, int dropsOutOfY, int amountDroppedMinimum, int amountDroppedMaximum, IItemDropRuleCondition condition, int dropsXOutOfY = 1)
			: base(itemId, dropsOutOfY, amountDroppedMinimum, amountDroppedMaximum, dropsXOutOfY)
		{
			_condition = condition;
		}

		public override bool CanDrop(DropAttemptInfo info)
		{
			return _condition.CanDrop(info);
		}

		public override void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
		{
			DropRateInfoChainFeed ratesInfo2 = ratesInfo.With(1f);
			ratesInfo2.AddCondition(_condition);
			float num = (float)_dropsXoutOfY / (float)_dropsOutOfY;
			float dropRate = num * ratesInfo2.parentDroprateChance;
			drops.Add(new DropRateInfo(_itemId, _amtDroppedMinimum, _amtDroppedMaximum, dropRate, ratesInfo2.conditions));
			Chains.ReportDroprates(base.ChainedRules, num, drops, ratesInfo2);
		}
	}
}
