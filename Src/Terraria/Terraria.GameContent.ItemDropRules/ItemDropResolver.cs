using System.Collections.Generic;

namespace GameManager.GameContent.ItemDropRules
{
	public class ItemDropResolver
	{
		private ItemDropDatabase _database;

		public ItemDropResolver(ItemDropDatabase database)
		{
			_database = database;
		}

		public void TryDropping(DropAttemptInfo info)
		{
			List<IItemDropRule> rulesForNPCID = _database.GetRulesForNPCID(info.npc.netID);
			for (int i = 0; i < rulesForNPCID.Count; i++)
			{
				ResolveRule(rulesForNPCID[i], info);
			}
		}

		private ItemDropAttemptResult ResolveRule(IItemDropRule rule, DropAttemptInfo info)
		{
			if (!rule.CanDrop(info))
			{
				ItemDropAttemptResult itemDropAttemptResult = default(ItemDropAttemptResult);
				itemDropAttemptResult.State = ItemDropAttemptResultState.DoesntFillConditions;
				ItemDropAttemptResult itemDropAttemptResult2 = itemDropAttemptResult;
				ResolveRuleChains(rule, info, itemDropAttemptResult2);
				return itemDropAttemptResult2;
			}
			ItemDropAttemptResult itemDropAttemptResult3 = (rule as INestedItemDropRule)?.TryDroppingItem(info, ResolveRule) ?? rule.TryDroppingItem(info);
			ResolveRuleChains(rule, info, itemDropAttemptResult3);
			return itemDropAttemptResult3;
		}

		private void ResolveRuleChains(IItemDropRule rule, DropAttemptInfo info, ItemDropAttemptResult parentResult)
		{
			ResolveRuleChains(info, parentResult, rule.ChainedRules);
		}

		private void ResolveRuleChains(DropAttemptInfo info, ItemDropAttemptResult parentResult, List<IItemDropRuleChainAttempt> ruleChains)
		{
			if (ruleChains == null)
			{
				return;
			}
			for (int i = 0; i < ruleChains.Count; i++)
			{
				IItemDropRuleChainAttempt itemDropRuleChainAttempt = ruleChains[i];
				if (itemDropRuleChainAttempt.CanChainIntoRule(parentResult))
				{
					ResolveRule(itemDropRuleChainAttempt.RuleToChain, info);
				}
			}
		}
	}
}
