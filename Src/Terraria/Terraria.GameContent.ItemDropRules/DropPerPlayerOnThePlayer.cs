namespace GameManager.GameContent.ItemDropRules
{
	public class DropPerPlayerOnThePlayer : CommonDrop
	{
		private IItemDropRuleCondition _condition;

		public DropPerPlayerOnThePlayer(int itemId, int dropsOutOfY, int amountDroppedMinimum, int amountDroppedMaximum, IItemDropRuleCondition optionalCondition)
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
			CommonCode.DropItemForEachInteractingPlayerOnThePlayer(info.npc, _itemId, info.rng, _dropsXoutOfY, _dropsOutOfY, info.rng.Next(_amtDroppedMinimum, _amtDroppedMaximum + 1));
			ItemDropAttemptResult result = default(ItemDropAttemptResult);
			result.State = ItemDropAttemptResultState.Success;
			return result;
		}
	}
}
