﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Achievements.CustomIntCondition
// Assembly: Terraria, Version=1.3.5.3, Culture=neutral, PublicKeyToken=null
// MVID: 68659D26-2BE6-448F-8663-74FA559E6F08
// Assembly location: H:\Steam\steamapps\common\Terraria\Terraria.exe

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Terraria.Achievements;

namespace Terraria.GameContent.Achievements
{
  public class CustomIntCondition : AchievementCondition
  {
    [JsonProperty("Value")]
    private int _value;
    private int _maxValue;

    public int Value
    {
      get
      {
        return this._value;
      }
      set
      {
        int newValue = Utils.Clamp<int>(value, 0, this._maxValue);
        if (this._tracker != null)
          ((AchievementTracker<int>) this._tracker).SetValue(newValue, true);
        this._value = newValue;
        if (this._value != this._maxValue)
          return;
        this.Complete();
      }
    }

    private CustomIntCondition(string name, int maxValue)
      : base(name)
    {
      this._maxValue = maxValue;
      this._value = 0;
    }

    public override void Clear()
    {
      this._value = 0;
      base.Clear();
    }

    public override void Load(JObject state)
    {
      base.Load(state);
      this._value = JToken.op_Explicit(state.get_Item("Value"));
      if (this._tracker == null)
        return;
      ((AchievementTracker<int>) this._tracker).SetValue(this._value, false);
    }

    protected override IAchievementTracker CreateAchievementTracker()
    {
      return (IAchievementTracker) new ConditionIntTracker(this._maxValue);
    }

    public static AchievementCondition Create(string name, int maxValue)
    {
      return (AchievementCondition) new CustomIntCondition(name, maxValue);
    }
  }
}
