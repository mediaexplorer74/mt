﻿// Decompiled with JetBrains decompiler
// Type: Terraria.Achievements.IAchievementTracker
// Assembly: Terraria, Version=1.3.5.3, Culture=neutral, PublicKeyToken=null
// MVID: 68659D26-2BE6-448F-8663-74FA559E6F08
// Assembly location: H:\Steam\steamapps\common\Terraria\Terraria.exe

namespace Terraria.Achievements
{
  public interface IAchievementTracker
  {
    void ReportAs(string name);

    TrackerType GetTrackerType();

    void Load();

    void Clear();
  }
}
