using System.Collections.Generic;

namespace GameManager.GameContent.ItemDropRules
{
	public class DropBasedOnExpertMode : IItemDropRule, INestedItemDropRule
	{
		private IItemDropRule _ruleForNormalMode;

		private IItemDropRule _ruleForExpertMode;

		public List<IItemDropRuleChainAttempt> ChainedRules
		{
			get;
			private set;
		}

		public DropBasedOnExpertMode(IItemDropRule ruleForNormalMode, IItemDropRule ruleForExpertMode)
		{
			_ruleForNormalMode = ruleForNormalMode;
			_ruleForExpertMode = ruleForExpertMode;
			ChainedRules = new List<IItemDropRuleChainAttempt>();
		}

		public bool CanDrop(DropAttemptInfo info)
		{
			if (info.IsExpertMode)
			{
				return _ruleForExpertMode.CanDrop(info);
			}
			return _ruleForNormalMode.CanDrop(info);
		}

		public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			ItemDropAttemptResult result = default(ItemDropAttemptResult);
			result.State = ItemDropAttemptResultState.DidNotRunCode;
			return result;
		}

		public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info, ItemDropRuleResolveAction resolveAction)
		{
			if (info.IsExpertMode)
			{
				return resolveAction(_ruleForExpertMode, info);
			}
			return resolveAction(_ruleForNormalMode, info);
		}

		public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
		{
			DropRateInfoChainFeed ratesInfo2 = ratesInfo.With(1f);
			ratesInfo2.AddCondition(new Conditions.IsExpert());
			_ruleForExpertMode.ReportDroprates(drops, ratesInfo2);
			DropRateInfoChainFeed ratesInfo3 = ratesInfo.With(1f);
			ratesInfo3.AddCondition(new Conditions.NotExpert());
			_ruleForNormalMode.ReportDroprates(drops, ratesInfo3);
			Chains.ReportDroprates(ChainedRules, 1f, drops, ratesInfo);
		}
	}
}
