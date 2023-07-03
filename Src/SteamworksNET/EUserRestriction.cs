// Decompiled with JetBrains decompiler
// Type: Steamworks.EUserRestriction
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

namespace Steamworks
{
  public enum EUserRestriction
  {
    k_nUserRestrictionNone = 0,
    k_nUserRestrictionUnknown = 1,
    k_nUserRestrictionAnyChat = 2,
    k_nUserRestrictionVoiceChat = 4,
    k_nUserRestrictionGroupChat = 8,
    k_nUserRestrictionRating = 16, // 0x00000010
    k_nUserRestrictionGameInvites = 32, // 0x00000020
    k_nUserRestrictionTrading = 64, // 0x00000040
  }
}
