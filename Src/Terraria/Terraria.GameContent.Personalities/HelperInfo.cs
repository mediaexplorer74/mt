using System.Collections.Generic;

namespace GameManager.GameContent.Personalities
{
	public struct HelperInfo
	{
		public Player player;

		public NPC npc;

		public List<NPC> NearbyNPCs;

		public int PrimaryPlayerBiome;

		public bool[] nearbyNPCsByType;
	}
}
