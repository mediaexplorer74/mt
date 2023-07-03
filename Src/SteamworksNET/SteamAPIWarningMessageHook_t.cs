// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamAPIWarningMessageHook_t
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System.Runtime.InteropServices;
using System.Text;

namespace Steamworks
{
  [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
  public delegate void SteamAPIWarningMessageHook_t(int nSeverity, StringBuilder pchDebugText);
}
