// Decompiled with JetBrains decompiler
// Type: Steamworks.CCallbackBaseVTable
// Assembly: Steamworks.NET, Version=9.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 03964DDB-6D2A-4A89-A283-51DA1C93775B
// Assembly location: C:\Users\Admin\Desktop\re\NET.dll

using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
  [StructLayout(LayoutKind.Sequential)]
  internal class CCallbackBaseVTable
  {
    private const CallingConvention cc = CallingConvention.ThisCall;
    [MarshalAs(UnmanagedType.FunctionPtr)]
    [NonSerialized]
    public CCallbackBaseVTable.RunCRDel m_RunCallResult;
    [MarshalAs(UnmanagedType.FunctionPtr)]
    [NonSerialized]
    public CCallbackBaseVTable.RunCBDel m_RunCallback;
    [MarshalAs(UnmanagedType.FunctionPtr)]
    [NonSerialized]
    public CCallbackBaseVTable.GetCallbackSizeBytesDel m_GetCallbackSizeBytes;

    [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
    public delegate void RunCBDel(IntPtr thisptr, IntPtr pvParam);

    [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
    public delegate void RunCRDel(
      IntPtr thisptr,
      IntPtr pvParam,
      [MarshalAs(UnmanagedType.I1)] bool bIOFailure,
      ulong hSteamAPICall);

    [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
    public delegate int GetCallbackSizeBytesDel(IntPtr thisptr);
  }
}
