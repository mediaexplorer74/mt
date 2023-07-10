using System.Collections.Generic;

namespace GameManager.GameContent.ItemDropRules
{
	public class LeadingConditionRule : IItemDropRule
	{
		private IItemDropRuleCondition _condition;

		public List<IItemDropRuleChainAttempt> ChainedRules
		{
			get;
			private set;
		}

		public LeadingConditionRule(IItemDropRuleCondition condition)
		{
			_condition = condition;
			ChainedRules = new List<IItemDropRuleChainAttempt>();
		}

		public bool CanDrop(DropAttemptInfo info)
		{
			return _condition.CanDrop(info);
		}

		public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
		{
			ratesInfo.AddCondition(_condition);
			Chains.ReportDroprates(ChainedRules, 1f, drops, ratesInfo);
		}

		public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			ItemDropAttemptResult result = default(ItemDropAttemptResult);
			result.State = ItemDropAttemptResultState.Success;
			return result;
		}
	}
}
