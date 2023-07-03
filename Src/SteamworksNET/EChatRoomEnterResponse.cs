// Decompiled with JetBrains decompiler
// Type: Steamworks.EChatRoomEnterResponse
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

namespace Steamworks
{
  public enum EChatRoomEnterResponse
  {
    k_EChatRoomEnterResponseSuccess = 1,
    k_EChatRoomEnterResponseDoesntExist = 2,
    k_EChatRoomEnterResponseNotAllowed = 3,
    k_EChatRoomEnterResponseFull = 4,
    k_EChatRoomEnterResponseError = 5,
    k_EChatRoomEnterResponseBanned = 6,
    k_EChatRoomEnterResponseLimited = 7,
    k_EChatRoomEnterResponseClanDisabled = 8,
    k_EChatRoomEnterResponseCommunityBan = 9,
    k_EChatRoomEnterResponseMemberBlockedYou = 10, // 0x0000000A
    k_EChatRoomEnterResponseYouBlockedMember = 11, // 0x0000000B
  }
}
