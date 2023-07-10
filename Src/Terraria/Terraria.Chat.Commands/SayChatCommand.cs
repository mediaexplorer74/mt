using GameManager.Localization;

namespace GameManager.Chat.Commands
{
	[ChatCommand("Say")]
	public class SayChatCommand : IChatCommand
	{
		public void ProcessIncomingMessage(string text, byte clientId)
		{
			ChatHelper.BroadcastChatMessageAs(clientId, NetworkText.FromLiteral(text), Main.player[clientId].ChatColor());
		}

		public void ProcessOutgoingMessage(ChatMessage message)
		{
		}
	}
}
