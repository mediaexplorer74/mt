// Decompiled with JetBrains decompiler
// Type: ReLogic.Localization.IME.Windows.ImeUi
// Assembly: ReLogic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 33513C57-D94A-4BED-935B-7012D40A5531
// Assembly location: C:\Users\Admin\Desktop\re\ReLogic.dll

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ReLogic.Localization.IME.Windows
{
  internal static class ImeUi
  {
    [DllImport("ReLogic.Native.dll", EntryPoint = "ImeUi_Initialize", CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool Initialize(IntPtr hWnd, [MarshalAs(UnmanagedType.I1)] bool bDisabled = false);

    [DllImport("ReLogic.Native.dll", EntryPoint = "ImeUi_Uninitialize", CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool Uninitialize();

    [DllImport("ReLogic.Native.dll", EntryPoint = "ImeUi_EnableIme", CharSet = CharSet.Unicode)]
    public static extern void Enable([MarshalAs(UnmanagedType.I1)] bool bEnable);

    [DllImport("ReLogic.Native.dll", EntryPoint = "ImeUi_IsEnabled", CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool IsEnabled();

    [DllImport("ReLogic.Native.dll", EntryPoint = "ImeUi_ProcessMessage", CharSet = CharSet.Unicode)]
    public static extern IntPtr ProcessMessage(
      IntPtr hWnd,
      int msg,
      IntPtr wParam,
      ref IntPtr lParam,
      [MarshalAs(UnmanagedType.I1)] ref bool trapped);

    [DllImport("ReLogic.Native.dll", EntryPoint = "ImeUi_GetCompositionString", CharSet = CharSet.Unicode)]
    public static extern IntPtr GetCompositionString();

    [DllImport("ReLogic.Native.dll", EntryPoint = "ImeUi_GetCandidate", CharSet = CharSet.Unicode)]
    public static extern IntPtr GetCandidate(uint index);

    [DllImport("ReLogic.Native.dll", EntryPoint = "ImeUi_GetCandidateSelection", CharSet = CharSet.Unicode)]
    public static extern uint GetCandidateSelection();

    [DllImport("ReLogic.Native.dll", EntryPoint = "ImeUi_GetCandidateCount", CharSet = CharSet.Unicode)]
    public static extern uint GetCandidateCount();

    [DllImport("ReLogic.Native.dll", EntryPoint = "ImeUi_FinalizeString", CharSet = CharSet.Unicode)]
    public static extern void FinalizeString([MarshalAs(UnmanagedType.I1)] bool bSend = false);

    [DllImport("ReLogic.Native.dll", EntryPoint = "ImeUi_GetCandidatePageSize", CharSet = CharSet.Unicode)]
    public static extern uint GetCandidatePageSize();

    [DllImport("ReLogic.Native.dll", EntryPoint = "ImeUi_IsShowCandListWindow", CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool IsCandidateListVisible();

    [DllImport("ReLogic.Native.dll", EntryPoint = "ImeUi_IgnoreHotKey", CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool ShouldIgnoreHotKey(ref Message message);

    public struct LanguageId
    {
      public static readonly ImeUi.LanguageId None = new ImeUi.LanguageId()
      {
        _id = IntPtr.Zero
      };
      private IntPtr _id;

      public ushort Id => (ushort) (this._id.ToInt32() & (int) ushort.MaxValue);

      public ushort Primary => (ushort) ((uint) this.Id & 1023U);

      public ushort Sub => (ushort) ((uint) this.Id >> 10);

      public override bool Equals(object obj) => obj is ImeUi.LanguageId languageId && this == languageId;

      public override int GetHashCode() => this._id.GetHashCode();

      public static bool operator ==(ImeUi.LanguageId lhs, ImeUi.LanguageId rhs) => lhs._id == rhs._id;

      public static bool operator !=(ImeUi.LanguageId lhs, ImeUi.LanguageId rhs) => lhs._id != rhs._id;
    }
  }
}
