// Decompiled with JetBrains decompiler
// Type: ReLogic.OS.WindowsPlatform
// Assembly: ReLogic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 33513C57-D94A-4BED-935B-7012D40A5531
// Assembly location: C:\Users\Admin\Desktop\re\ReLogic.dll

using Microsoft.Xna.Framework;
using ReLogic.Localization.IME;
using ReLogic.OS.Windows;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace ReLogic.OS
{
  internal class WindowsPlatform : Platform
  {
    private WindowsMessageHook _wndProcHook;

    public WindowsPlatform()
      : base(PlatformType.Windows)
    {
    }

    private ApartmentState GetApartmentStateSafely()
    {
      try
      {
        return Thread.CurrentThread.GetApartmentState();
      }
      catch
      {
        return ApartmentState.MTA;
      }
    }

    protected override string GetClipboard() => this.InvokeInStaThread<string>((Func<string>) (() => System.Windows.Forms.Clipboard.GetText()));

    protected override void SetClipboard(string text)
    {
      if (text == null || !(text != ""))
        return;
      this.InvokeInStaThread((Action) (() => System.Windows.Forms.Clipboard.SetText(text)));
    }

    private T InvokeInStaThread<T>(Func<T> callback)
    {
      if (this.GetApartmentStateSafely() == ApartmentState.STA)
        return callback();
      T result = default (T);
      Thread thread = new Thread((ThreadStart) (() => result = callback()));
      thread.SetApartmentState(ApartmentState.STA);
      thread.Start();
      thread.Join();
      return result;
    }

    private void InvokeInStaThread(Action callback)
    {
      if (this.GetApartmentStateSafely() == ApartmentState.STA)
      {
        callback();
      }
      else
      {
        Thread thread = new Thread((ThreadStart) (() => callback()));
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();
      }
    }

    public override string GetStoragePath() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "My Games");

    public override void SetWindowUnicodeTitle(GameWindow window, string title)
    {
      WindowsPlatform.WndProcCallback d = new WindowsPlatform.WndProcCallback(ReLogic.OS.Windows.NativeMethods.DefWindowProc);
      IntPtr dwNewLong = ReLogic.OS.Windows.NativeMethods.SetWindowLong(window.Handle, -4, (int) Marshal.GetFunctionPointerForDelegate((Delegate) d));
      base.SetWindowUnicodeTitle(window, title);
      ReLogic.OS.Windows.NativeMethods.SetWindowLong(window.Handle, -4, (int) dwNewLong);
    }

    protected override PlatformIme CreateIme(IntPtr windowHandle)
    {
      if (this._wndProcHook == null)
        this._wndProcHook = new WindowsMessageHook(windowHandle);
      return (PlatformIme) new WindowsIme(this._wndProcHook, windowHandle);
    }

    private delegate IntPtr WndProcCallback(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
  }
}
