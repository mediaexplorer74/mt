// Decompiled with JetBrains decompiler
// Type: Steamworks.EAuthSessionResponse
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

namespace Steamworks
{
  public enum EAuthSessionResponse
  {
    k_EAuthSessionResponseOK,
    k_EAuthSessionResponseUserNotConnectedToSteam,
    k_EAuthSessionResponseNoLicenseOrExpired,
    k_EAuthSessionResponseVACBanned,
    k_EAuthSessionResponseLoggedInElseWhere,
    k_EAuthSessionResponseVACCheckTimedOut,
    k_EAuthSessionResponseAuthTicketCanceled,
    k_EAuthSessionResponseAuthTicketInvalidAlreadyUsed,
    k_EAuthSessionResponseAuthTicketInvalid,
    k_EAuthSessionResponsePublisherIssuedBan,
  }
}
