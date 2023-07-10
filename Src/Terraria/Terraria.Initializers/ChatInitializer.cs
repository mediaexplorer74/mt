using GameManager.Chat.Commands;
using GameManager.GameContent.UI;
using GameManager.GameContent.UI.Chat;
using GameManager.Localization;
using GameManager.UI.Chat;

namespace GameManager.Initializers
{
	public static class ChatInitializer
	{
		public static void Load()
		{
			ChatManager.Register<ColorTagHandler>(new string[2]
			{
				"c",
				"color"
			});
			ChatManager.Register<ItemTagHandler>(new string[2]
			{
				"i",
				"item"
			});
			ChatManager.Register<NameTagHandler>(new string[2]
			{
				"n",
				"name"
			});
			ChatManager.Register<AchievementTagHandler>(new string[2]
			{
				"a",
				"achievement"
			});
			ChatManager.Register<GlyphTagHandler>(new string[2]
			{
				"g",
				"glyph"
			});
			ChatManager.Commands.AddCommand<PartyChatCommand>().AddCommand<RollCommand>().AddCommand<EmoteCommand>()
				.AddCommand<ListPlayersCommand>()
				.AddCommand<RockPaperScissorsCommand>()
				.AddCommand<EmojiCommand>()
				.AddCommand<HelpCommand>()
				.AddDefaultCommand<SayChatCommand>();
			for (int i = 0; i < 145; i++)
			{
				string name = EmoteID.Search.GetName(i);
				string key = "EmojiCommand." + name;
				ChatManager.Commands.AddAlias(Language.GetText(key), NetworkText.FromFormattable("{0} {1}", Language.GetText("ChatCommand.Emoji_1"), Language.GetText("EmojiName." + name)));
			}
		}
	}
}
