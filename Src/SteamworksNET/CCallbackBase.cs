// Decompiled with JetBrains decompiler
// Type: Steamworks.CCallbackBase
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
  [StructLayout(LayoutKind.Sequential)]
  internal class CCallbackBase
  {
    public const byte k_ECallbackFlagsRegistered = 1;
    public const byte k_ECallbackFlagsGameServer = 2;
    public IntPtr m_vfptr;
    public byte m_nCallbackFlags;
    public int m_iCallback;
  }
}
