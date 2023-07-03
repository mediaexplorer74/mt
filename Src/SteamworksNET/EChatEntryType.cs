// Decompiled with JetBrains decompiler
// Type: Steamworks.EChatEntryType
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

namespace Steamworks
{
  public enum EChatEntryType
  {
    k_EChatEntryTypeInvalid = 0,
    k_EChatEntryTypeChatMsg = 1,
    k_EChatEntryTypeTyping = 2,
    k_EChatEntryTypeInviteGame = 3,
    k_EChatEntryTypeEmote = 4,
    k_EChatEntryTypeLeftConversation = 6,
    k_EChatEntryTypeEntered = 7,
    k_EChatEntryTypeWasKicked = 8,
    k_EChatEntryTypeWasBanned = 9,
    k_EChatEntryTypeDisconnected = 10, // 0x0000000A
    k_EChatEntryTypeHistoricalChat = 11, // 0x0000000B
    k_EChatEntryTypeLinkBlocked = 14, // 0x0000000E
  }
}
