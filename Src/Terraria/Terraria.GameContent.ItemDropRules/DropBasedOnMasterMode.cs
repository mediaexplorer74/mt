using System.Collections.Generic;

namespace GameManager.GameContent.ItemDropRules
{
	public class DropBasedOnMasterMode : IItemDropRule, INestedItemDropRule
	{
		private IItemDropRule _ruleForDefault;

		private IItemDropRule _ruleForMasterMode;

		public List<IItemDropRuleChainAttempt> ChainedRules
		{
			get;
			private set;
		}

		public DropBasedOnMasterMode(IItemDropRule ruleForDefault, IItemDropRule ruleForMasterMode)
		{
			_ruleForDefault = ruleForDefault;
			_ruleForMasterMode = ruleForMasterMode;
			ChainedRules = new List<IItemDropRuleChainAttempt>();
		}

		public bool CanDrop(DropAttemptInfo info)
		{
			if (info.IsMasterMode)
			{
				return _ruleForMasterMode.CanDrop(info);
			}
			return _ruleForDefault.CanDrop(info);
		}

		public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			ItemDropAttemptResult result = default(ItemDropAttemptResult);
			result.State = ItemDropAttemptResultState.DidNotRunCode;
			return result;
		}

		public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info, ItemDropRuleResolveAction resolveAction)
		{
			if (info.IsMasterMode)
			{
				return resolveAction(_ruleForMasterMode, info);
			}
			return resolveAction(_ruleForDefault, info);
		}

		public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
		{
			DropRateInfoChainFeed ratesInfo2 = ratesInfo.With(1f);
			ratesInfo2.AddCondition(new Conditions.IsMasterMode());
			_ruleForMasterMode.ReportDroprates(drops, ratesInfo2);
			DropRateInfoChainFeed ratesInfo3 = ratesInfo.With(1f);
			ratesInfo3.AddCondition(new Conditions.NotMasterMode());
			_ruleForDefault.ReportDroprates(drops, ratesInfo3);
			Chains.ReportDroprates(ChainedRules, 1f, drops, ratesInfo);
		}
	}
}
