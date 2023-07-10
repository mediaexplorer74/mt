using System.Collections.Generic;

namespace GameManager.GameContent.ItemDropRules
{
	public class OneFromRulesRule : IItemDropRule, INestedItemDropRule
	{
		private IItemDropRule[] _options;

		private int _outOfY;

		public List<IItemDropRuleChainAttempt> ChainedRules
		{
			get;
			private set;
		}

		public OneFromRulesRule(int outOfY, params IItemDropRule[] options)
		{
			_outOfY = outOfY;
			_options = options;
			ChainedRules = new List<IItemDropRuleChainAttempt>();
		}

		public bool CanDrop(DropAttemptInfo info)
		{
			return true;
		}

		public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			ItemDropAttemptResult result = default(ItemDropAttemptResult);
			result.State = ItemDropAttemptResultState.DidNotRunCode;
			return result;
		}

		public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info, ItemDropRuleResolveAction resolveAction)
		{
			int num = -1;
			ItemDropAttemptResult result;
			if (info.rng.Next(_outOfY) == 0)
			{
				num = info.rng.Next(_options.Length);
				resolveAction(_options[num], info);
				result = default(ItemDropAttemptResult);
				result.State = ItemDropAttemptResultState.Success;
				return result;
			}
			result = default(ItemDropAttemptResult);
			result.State = ItemDropAttemptResultState.FailedRandomRoll;
			return result;
		}

		public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
		{
			float num = 1f / (float)_outOfY;
			float num2 = num * ratesInfo.parentDroprateChance;
			float multiplier = 1f / (float)_options.Length * num2;
			for (int i = 0; i < _options.Length; i++)
			{
				_options[i].ReportDroprates(drops, ratesInfo.With(multiplier));
			}
			Chains.ReportDroprates(ChainedRules, num, drops, ratesInfo);
		}
	}
}
