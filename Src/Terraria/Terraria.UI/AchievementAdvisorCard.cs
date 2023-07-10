using Microsoft.Xna.Framework;
using GameManager.Achievements;

namespace GameManager.UI
{
	public class AchievementAdvisorCard
	{
		private const int _iconSize = 64;

		private const int _iconSizeWithSpace = 66;

		private const int _iconsPerRow = 8;

		public Achievement achievement;

		public float order;

		public Rectangle frame;

		public int achievementIndex;

		public AchievementAdvisorCard(Achievement achievement, float order)
		{
			this.achievement = achievement;
			this.order = order;
			achievementIndex = Main.Achievements.GetIconIndex(achievement.Name);
			frame = new Rectangle(achievementIndex % 8 * 66, achievementIndex / 8 * 66, 64, 64);
		}

		public bool IsAchievableInWorld()
		{
			string name = achievement.Name;
			if (!(name == "MASTERMIND"))
			{
				if (name == "WORM_FODDER")
				{
					return !WorldGen.crimson;
				}
				return true;
			}
			return WorldGen.crimson;
		}
	}
}
