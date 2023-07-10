using Microsoft.Xna.Framework;
using GameManager.Achievements;
using GameManager.UI;
using GameManager.UI.Chat;

namespace GameManager.GameContent.UI.Chat
{
	public class AchievementTagHandler : ITagHandler
	{
		private class AchievementSnippet : TextSnippet
		{
			private Achievement _achievement;

			public AchievementSnippet(Achievement achievement)
				: base(achievement.FriendlyName.Value, Color.LightBlue)
			{
				CheckForHover = true;
				_achievement = achievement;
			}

			public override void OnClick()
			{
				IngameOptions.Close();
				IngameFancyUI.OpenAchievementsAndGoto(_achievement);
			}
		}

		TextSnippet ITagHandler.Parse(string text, Color baseColor, string options)
		{
			Achievement achievement = Main.Achievements.GetAchievement(text);
			if (achievement == null)
			{
				return new TextSnippet(text);
			}
			return new AchievementSnippet(achievement);
		}

		public static string GenerateTag(Achievement achievement)
		{
			return "[a:" + achievement.Name + "]";
		}
	}
}
