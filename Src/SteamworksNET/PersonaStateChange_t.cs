// Decompiled with JetBrains decompiler
// Type: Steamworks.PersonaStateChange_t
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [CallbackIdentity(304)]
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct PersonaStateChange_t
  {
    public const int k_iCallback = 304;
    public ulong m_ulSteamID;
    public EPersonaChange m_nChangeFlags;
  }
}
