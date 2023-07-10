using Microsoft.Xna.Framework;
using GameManager.Localization;

namespace GameManager.Chat.Commands
{
	[ChatCommand("Emote")]
	public class EmoteCommand : IChatCommand
	{
		private static readonly Color RESPONSE_COLOR = new Color(200, 100, 0);

		public void ProcessIncomingMessage(string text, byte clientId)
		{
			if (text != "")
			{
				text = $"*{Main.player[clientId].name} {text}";
				ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(text), RESPONSE_COLOR);
			}
		}

		public void ProcessOutgoingMessage(ChatMessage message)
		{
		}
	}
}
