// CustomFlagCondition

using GameManager.Achievements;

namespace GameManager.GameContent.Achievements
{
    internal class CustomFlagCondition : AchievementCondition
    {
        private CustomFlagCondition(string name)
            : base(name) { }

        public static AchievementCondition Create(string name)
        {
            return (AchievementCondition)new CustomFlagCondition(name);
        }
    }
}
