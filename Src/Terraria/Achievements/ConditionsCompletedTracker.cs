// ConditionsCompletedTracker

using System;
using System.Collections.Generic;

namespace GameManager.Achievements
{
    public class ConditionsCompletedTracker : ConditionIntTracker
    {
        private List<AchievementCondition> _conditions = new List<AchievementCondition>();

        public void AddCondition(AchievementCondition condition)
        {
            ++_maxValue;
            condition.OnComplete += new AchievementCondition.AchievementUpdate(OnConditionCompleted);
            _conditions.Add(condition);
        }

        private void OnConditionCompleted(AchievementCondition condition)
        {
            SetValue(Math.Min(_value + 1, _maxValue), true);
        }

        protected override void Load()
        {
            for (int index = 0; index < _conditions.Count; ++index)
            {
                if (_conditions[index].IsCompleted)
                    ++_value;
            }
        }
    }
}
