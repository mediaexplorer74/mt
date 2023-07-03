// Decompiled with JetBrains decompiler
// Type: Steamworks.ValidateAuthTicketResponse_t
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(143)]
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct ValidateAuthTicketResponse_t
  {
    public const int k_iCallback = 143;
    public CSteamID m_SteamID;
    public EAuthSessionResponse m_eAuthSessionResponse;
    public CSteamID m_OwnerSteamID;
  }
}
