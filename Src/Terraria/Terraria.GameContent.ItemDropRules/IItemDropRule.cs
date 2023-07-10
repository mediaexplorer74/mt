using System.Collections.Generic;

namespace GameManager.GameContent.ItemDropRules
{
	public interface IItemDropRule
	{
		List<IItemDropRuleChainAttempt> ChainedRules
		{
			get;
		}

		bool CanDrop(DropAttemptInfo info);

		void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo);

		ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info);
	}
}
