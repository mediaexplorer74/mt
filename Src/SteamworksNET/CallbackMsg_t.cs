// Decompiled with JetBrains decompiler
// Type: Steamworks.CallbackMsg_t
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  public struct CallbackMsg_t
  {
    public int m_hSteamUser;
    public int m_iCallback;
    public IntPtr m_pubParam;
    public int m_cubParam;
  }
}
