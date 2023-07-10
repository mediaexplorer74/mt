using Steamworks;
using GameManager.Social.Base;

namespace GameManager.Social.Steam
{
	public class FriendsSocialModule : GameManager.Social.Base.FriendsSocialModule
	{
		public override void Initialize()
		{
		}

		public override void Shutdown()
		{
		}

		public override string GetUsername()
		{
			return SteamFriends.GetPersonaName();
		}

		public override void OpenJoinInterface()
		{
			SteamFriends.ActivateGameOverlay("Friends");
		}
	}
}
