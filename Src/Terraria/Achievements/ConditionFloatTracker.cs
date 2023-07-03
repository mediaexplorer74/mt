// ConditionFloatTracker

namespace GameManager.Achievements
{
    public class ConditionFloatTracker : AchievementTracker<float>
    {
        public ConditionFloatTracker(float maxValue)
            : base(TrackerType.Float)
        {
            _maxValue = maxValue;
        }

        public ConditionFloatTracker()
            : base(TrackerType.Float) { }

        public override void ReportUpdate() { }

        protected override void Load() { }
    }
}
