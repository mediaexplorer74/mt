namespace GameManager.GameContent.ItemDropRules
{
	public class CommonDropNotScalingWithLuck : CommonDrop
	{
		public CommonDropNotScalingWithLuck(int itemId, int dropsOutOfY, int amountDroppedMinimum, int amountDroppedMaximum)
			: base(itemId, dropsOutOfY, amountDroppedMinimum, amountDroppedMaximum)
		{
		}

		public override ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			ItemDropAttemptResult result;
			if (info.rng.Next(_dropsOutOfY) < _dropsXoutOfY)
			{
				CommonCode.DropItemFromNPC(info.npc, _itemId, info.rng.Next(_amtDroppedMinimum, _amtDroppedMaximum + 1));
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
