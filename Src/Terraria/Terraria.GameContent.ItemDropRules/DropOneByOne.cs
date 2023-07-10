using System.Collections.Generic;

namespace GameManager.GameContent.ItemDropRules
{
	public class DropOneByOne : IItemDropRule
	{
		public struct Parameters
		{
			public int DropsXOutOfYTimes_TheX;

			public int DropsXOutOfYTimes_TheY;

			public int MinimumItemDropsCount;

			public int MaximumItemDropsCount;

			public int MinimumStackPerChunkBase;

			public int MaximumStackPerChunkBase;

			public int BonusMinDropsPerChunkPerPlayer;

			public int BonusMaxDropsPerChunkPerPlayer;

			public float GetPersonalDropRate()
			{
				return (float)DropsXOutOfYTimes_TheX / (float)DropsXOutOfYTimes_TheY;
			}
		}

		private int _itemId;

		private Parameters _parameters;

		public List<IItemDropRuleChainAttempt> ChainedRules
		{
			get;
			private set;
		}

		public DropOneByOne(int itemId, Parameters parameters)
		{
			ChainedRules = new List<IItemDropRuleChainAttempt>();
			_parameters = parameters;
			_itemId = itemId;
		}

		public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			ItemDropAttemptResult result;
			if (info.player.RollLuck(_parameters.DropsXOutOfYTimes_TheY) < _parameters.DropsXOutOfYTimes_TheX)
			{
				int num = info.rng.Next(_parameters.MinimumItemDropsCount, _parameters.MaximumItemDropsCount + 1);
				int activePlayersCount = Main.CurrentFrameFlags.ActivePlayersCount;
				int minValue = _parameters.MinimumStackPerChunkBase + activePlayersCount * _parameters.BonusMinDropsPerChunkPerPlayer;
				int num2 = _parameters.MaximumStackPerChunkBase + activePlayersCount * _parameters.BonusMaxDropsPerChunkPerPlayer;
				for (int i = 0; i < num; i++)
				{
					CommonCode.DropItemFromNPC(info.npc, _itemId, info.rng.Next(minValue, num2 + 1), scattered: true);
				}
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
			float personalDropRate = _parameters.GetPersonalDropRate();
			float dropRate = personalDropRate * ratesInfo.parentDroprateChance;
			drops.Add(new DropRateInfo(_itemId, _parameters.MinimumItemDropsCount * (_parameters.MinimumStackPerChunkBase + _parameters.BonusMinDropsPerChunkPerPlayer), _parameters.MaximumItemDropsCount * (_parameters.MaximumStackPerChunkBase + _parameters.BonusMaxDropsPerChunkPerPlayer), dropRate, ratesInfo.conditions));
			Chains.ReportDroprates(ChainedRules, personalDropRate, drops, ratesInfo);
		}

		public bool CanDrop(DropAttemptInfo info)
		{
			return true;
		}
	}
}
