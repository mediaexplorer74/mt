using System.Collections.Generic;

namespace GameManager.GameContent.ItemDropRules
{
	public static class Chains
	{
		public class TryIfFailedRandomRoll : IItemDropRuleChainAttempt
		{
			private bool _hideLootReport;

			public IItemDropRule RuleToChain
			{
				get;
				private set;
			}

			public TryIfFailedRandomRoll(IItemDropRule rule, bool hideLootReport = false)
			{
				RuleToChain = rule;
				_hideLootReport = hideLootReport;
			}

			public bool CanChainIntoRule(ItemDropAttemptResult parentResult)
			{
				return parentResult.State == ItemDropAttemptResultState.FailedRandomRoll;
			}

			public void ReportDroprates(float personalDropRate, List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
			{
				if (!_hideLootReport)
				{
					RuleToChain.ReportDroprates(drops, ratesInfo.With(1f - personalDropRate));
				}
			}
		}

		public class TryIfSucceeded : IItemDropRuleChainAttempt
		{
			private bool _hideLootReport;

			public IItemDropRule RuleToChain
			{
				get;
				private set;
			}

			public TryIfSucceeded(IItemDropRule rule, bool hideLootReport = false)
			{
				RuleToChain = rule;
				_hideLootReport = hideLootReport;
			}

			public bool CanChainIntoRule(ItemDropAttemptResult parentResult)
			{
				return parentResult.State == ItemDropAttemptResultState.Success;
			}

			public void ReportDroprates(float personalDropRate, List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
			{
				if (!_hideLootReport)
				{
					RuleToChain.ReportDroprates(drops, ratesInfo.With(personalDropRate));
				}
			}
		}

		public class TryIfDoesntFillConditions : IItemDropRuleChainAttempt
		{
			private bool _hideLootReport;

			public IItemDropRule RuleToChain
			{
				get;
				private set;
			}

			public TryIfDoesntFillConditions(IItemDropRule rule, bool hideLootReport = false)
			{
				RuleToChain = rule;
				_hideLootReport = hideLootReport;
			}

			public bool CanChainIntoRule(ItemDropAttemptResult parentResult)
			{
				return parentResult.State == ItemDropAttemptResultState.DoesntFillConditions;
			}

			public void ReportDroprates(float personalDropRate, List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
			{
				if (!_hideLootReport)
				{
					RuleToChain.ReportDroprates(drops, ratesInfo.With(personalDropRate));
				}
			}
		}

		public static void ReportDroprates(List<IItemDropRuleChainAttempt> ChainedRules, float personalDropRate, List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
		{
			foreach (IItemDropRuleChainAttempt ChainedRule in ChainedRules)
			{
				ChainedRule.ReportDroprates(personalDropRate, drops, ratesInfo);
			}
		}

		public static IItemDropRule OnFailedRoll(this IItemDropRule rule, IItemDropRule ruleToChain, bool hideLootReport = false)
		{
			rule.ChainedRules.Add(new TryIfFailedRandomRoll(ruleToChain, hideLootReport));
			return ruleToChain;
		}

		public static IItemDropRule OnSuccess(this IItemDropRule rule, IItemDropRule ruleToChain, bool hideLootReport = false)
		{
			rule.ChainedRules.Add(new TryIfSucceeded(ruleToChain, hideLootReport));
			return ruleToChain;
		}

		public static IItemDropRule OnFailedConditions(this IItemDropRule rule, IItemDropRule ruleToChain, bool hideLootReport = false)
		{
			rule.ChainedRules.Add(new TryIfDoesntFillConditions(ruleToChain, hideLootReport));
			return ruleToChain;
		}
	}
}
