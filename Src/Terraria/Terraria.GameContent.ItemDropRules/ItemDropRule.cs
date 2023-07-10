namespace GameManager.GameContent.ItemDropRules
{
	public class ItemDropRule
	{
		public static IItemDropRule Common(int itemId, int dropsOutOfX = 1, int minimumDropped = 1, int maximumDropped = 1)
		{
			return new CommonDrop(itemId, dropsOutOfX, minimumDropped, maximumDropped);
		}

		public static IItemDropRule BossBag(int itemId)
		{
			return new DropBasedOnExpertMode(DropNothing(), new DropLocalPerClientAndResetsNPCMoneyTo0(itemId, 1, 1, 1, null));
		}

		public static IItemDropRule BossBagByCondition(IItemDropRuleCondition condition, int itemId)
		{
			return new DropBasedOnExpertMode(DropNothing(), new DropLocalPerClientAndResetsNPCMoneyTo0(itemId, 1, 1, 1, condition));
		}

		public static IItemDropRule ExpertGetsRerolls(int itemId, int dropsOutOfX, int expertRerolls)
		{
			return new DropBasedOnExpertMode(WithRerolls(itemId, 0, dropsOutOfX), WithRerolls(itemId, expertRerolls, dropsOutOfX));
		}

		public static IItemDropRule MasterModeCommonDrop(int itemId)
		{
			return ByCondition(new Conditions.IsMasterMode(), itemId);
		}

		public static IItemDropRule MasterModeDropOnAllPlayers(int itemId, int dropsAtXOutOfY_TheY = 1)
		{
			return new DropBasedOnMasterMode(DropNothing(), new DropPerPlayerOnThePlayer(itemId, dropsAtXOutOfY_TheY, 1, 1, new Conditions.IsMasterMode()));
		}

		public static IItemDropRule WithRerolls(int itemId, int rerolls, int dropsOutOfX = 1, int minimumDropped = 1, int maximumDropped = 1)
		{
			return new CommonDropWithRerolls(itemId, dropsOutOfX, minimumDropped, maximumDropped, rerolls);
		}

		public static IItemDropRule ByCondition(IItemDropRuleCondition condition, int itemId, int dropsOutOfX = 1, int minimumDropped = 1, int maximumDropped = 1, int dropsXOutOfY = 1)
		{
			return new ItemDropWithConditionRule(itemId, dropsOutOfX, minimumDropped, maximumDropped, condition, dropsXOutOfY);
		}

		public static IItemDropRule NotScalingWithLuck(int itemId, int dropsOutOfX = 1, int minimumDropped = 1, int maximumDropped = 1)
		{
			return new CommonDrop(itemId, dropsOutOfX, minimumDropped, maximumDropped);
		}

		public static IItemDropRule OneFromOptionsNotScalingWithLuck(int dropsOutOfX, params int[] options)
		{
			return new OneFromOptionsNotScaledWithLuckDropRule(dropsOutOfX, 1, options);
		}

		public static IItemDropRule OneFromOptionsNotScalingWithLuckWithX(int dropsOutOfY, int xOutOfY, params int[] options)
		{
			return new OneFromOptionsNotScaledWithLuckDropRule(dropsOutOfY, xOutOfY, options);
		}

		public static IItemDropRule OneFromOptions(int dropsOutOfX, params int[] options)
		{
			return new OneFromOptionsDropRule(dropsOutOfX, 1, options);
		}

		public static IItemDropRule OneFromOptionsWithX(int dropsOutOfY, int xOutOfY, params int[] options)
		{
			return new OneFromOptionsDropRule(dropsOutOfY, xOutOfY, options);
		}

		public static IItemDropRule DropNothing()
		{
			return new DropNothing();
		}

		public static IItemDropRule NormalvsExpert(int itemId, int oncePerXInNormal, int oncePerXInExpert)
		{
			return new DropBasedOnExpertMode(Common(itemId, oncePerXInNormal), Common(itemId, oncePerXInExpert));
		}

		public static IItemDropRule NormalvsExpertNotScalingWithLuck(int itemId, int oncePerXInNormal, int oncePerXInExpert)
		{
			return new DropBasedOnExpertMode(NotScalingWithLuck(itemId, oncePerXInNormal), NotScalingWithLuck(itemId, oncePerXInExpert));
		}

		public static IItemDropRule NormalvsExpertOneFromOptionsNotScalingWithLuck(int dropsOutOfXNormalMode, int dropsOutOfXExpertMode, params int[] options)
		{
			return new DropBasedOnExpertMode(OneFromOptionsNotScalingWithLuck(dropsOutOfXNormalMode, options), OneFromOptionsNotScalingWithLuck(dropsOutOfXExpertMode, options));
		}

		public static IItemDropRule NormalvsExpertOneFromOptions(int dropsOutOfXNormalMode, int dropsOutOfXExpertMode, params int[] options)
		{
			return new DropBasedOnExpertMode(OneFromOptions(dropsOutOfXNormalMode, options), OneFromOptions(dropsOutOfXExpertMode, options));
		}

		public static IItemDropRule Food(int itemId, int dropsOutOfX, int minimumDropped = 1, int maximumDropped = 1)
		{
			return new ItemDropWithConditionRule(itemId, dropsOutOfX, minimumDropped, maximumDropped, new Conditions.NotFromStatue());
		}

		public static IItemDropRule StatusImmunityItem(int itemId, int dropsOutOfX)
		{
			return ExpertGetsRerolls(itemId, dropsOutOfX, 1);
		}
	}
}
