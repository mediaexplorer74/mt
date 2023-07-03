// Decompiled with JetBrains decompiler
// Type: Steamworks.ESteamItemFlags
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System;

namespace Steamworks
{
  [Flags]
  public enum ESteamItemFlags
  {
    k_ESteamItemNoTrade = 1,
    k_ESteamItemRemoved = 256, // 0x00000100
    k_ESteamItemConsumed = 512, // 0x00000200
  }
}
