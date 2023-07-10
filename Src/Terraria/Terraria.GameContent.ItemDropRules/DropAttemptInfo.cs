using GameManager.Utilities;

namespace GameManager.GameContent.ItemDropRules
{
	public struct DropAttemptInfo
	{
		public NPC npc;

		public Player player;

		public UnifiedRandom rng;

		public bool IsInSimulation;

		public bool IsExpertMode;

		public bool IsMasterMode;
	}
}
