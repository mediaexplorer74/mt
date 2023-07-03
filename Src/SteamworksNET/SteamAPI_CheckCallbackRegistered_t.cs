// Decompiled with JetBrains decompiler
// Type: Steamworks.SteamAPI_CheckCallbackRegistered_t
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System.Runtime.InteropServices;

namespace Steamworks
{
  [UnmanagedFunctionPointer(CallingConvention.StdCall)]
  public delegate void SteamAPI_CheckCallbackRegistered_t(int iCallbackNum);
}
