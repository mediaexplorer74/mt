// Decompiled with JetBrains decompiler
// Type: ReLogic.OS.Windows.NativeMethods
// Assembly: ReLogic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 33513C57-D94A-4BED-935B-7012D40A5531
// Assembly location: C:\Users\Admin\Desktop\re\ReLogic.dll

using System;
using System.Runtime.InteropServices;

namespace ReLogic.OS.Windows
{
  internal static class NativeMethods
  {
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr DefWindowProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
  }
}
