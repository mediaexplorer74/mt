// CustomIntCondition

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using GameManager.Achievements;

namespace GameManager.GameContent.Achievements
{
    internal class CustomIntCondition : AchievementCondition
    {
        [JsonProperty("Value")]
        private int _value;
        private int _maxValue;

        public int Value
        {
            get { return +_value; }
            set
            {
                int num = Utils.Clamp<int>(value, 0, _maxValue);
                if (_tracker != null)
                    ((ConditionIntTracker)_tracker).SetValue(num, true);
                _value = num;
                if (_value == _maxValue)
                    Complete();
            }
        }

        private CustomIntCondition(string name, int maxVal)
            : base(name)
        {
            _maxValue = maxVal;
            _value = 0;
        }

        public override void Clear()
        {
            _value = 0;
            base.Clear();
        }

        public override void Load(JObject state)
        {
            base.Load(state);
            _value = (int)state.GetValue("Value");
            if (this._tracker != null)
                ((ConditionIntTracker)_tracker).SetValue(_value, false);
        }

        protected override IAchievementTracker CreateAchievementTracker()
        {
            return new ConditionIntTracker(_maxValue);
        }

        public static AchievementCondition Create(string name, int maxValue)
        {
            return new CustomIntCondition(name, maxValue);
        }
    }
}