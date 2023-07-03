// IAchievementTracker

namespace GameManager.Achievements
{
    public interface IAchievementTracker
    {
        void ReportAs(string name);

        TrackerType GetTrackerType();

        void Load();

        void Clear();
    }
}
