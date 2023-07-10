namespace GameManager.GameContent.ItemDropRules
{
	public class DropLocalPerClientAndResetsNPCMoneyTo0 : CommonDrop
	{
		private IItemDropRuleCondition _condition;

		public DropLocalPerClientAndResetsNPCMoneyTo0(int itemId, int dropsOutOfY, int amountDroppedMinimum, int amountDroppedMaximum, IItemDropRuleCondition optionalCondition)
			: base(itemId, dropsOutOfY, amountDroppedMinimum, amountDroppedMaximum)
		{
			_condition = optionalCondition;
		}

		public override bool CanDrop(DropAttemptInfo info)
		{
			if (_condition != null)
			{
				return _condition.CanDrop(info);
			}
			return true;
		}

		public override ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			ItemDropAttemptResult result;
			if (info.rng.Next(_dropsOutOfY) < _dropsXoutOfY)
			{
				CommonCode.DropItemLocalPerClientAndSetNPCMoneyTo0(info.npc, _itemId, info.rng.Next(_amtDroppedMinimum, _amtDroppedMaximum + 1));
				result = default(ItemDropAttemptResult);
				result.State = ItemDropAttemptResultState.Success;
				return result;
			}
			result = default(ItemDropAttemptResult);
			result.State = ItemDropAttemptResultState.FailedRandomRoll;
			return result;
		}
	}
}
