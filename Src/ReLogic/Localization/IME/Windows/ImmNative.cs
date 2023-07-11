// Decompiled with JetBrains decompiler
// Type: ReLogic.Localization.IME.Windows.ImmNative
// Assembly: ReLogic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 33513C57-D94A-4BED-935B-7012D40A5531
// Assembly location: C:\Users\Admin\Desktop\re\ReLogic.dll

using System;
using System.Runtime.InteropServices;
//using System.Windows.Forms;

namespace ReLogic.Localization.IME.Windows
{
  internal static class ImmNative
  {
    [DllImport("user32.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool TranslateMessage(ref Message message);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr GetForegroundWindow();
  }
}
