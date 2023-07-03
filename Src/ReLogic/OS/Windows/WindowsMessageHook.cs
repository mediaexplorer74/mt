// Decompiled with JetBrains decompiler
// Type: ReLogic.OS.Windows.WindowsMessageHook
// Assembly: ReLogic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 33513C57-D94A-4BED-935B-7012D40A5531
// Assembly location: C:\Users\Admin\Desktop\re\ReLogic.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ReLogic.OS.Windows
{
  internal class WindowsMessageHook : IDisposable, IMessageFilter
  {
    private const int GWL_WNDPROC = -4;
    private IntPtr _windowHandle = IntPtr.Zero;
    private IntPtr _previousWndProc = IntPtr.Zero;
    private WindowsMessageHook.WndProcCallback _wndProc;
    private List<IMessageFilter> _filters = new List<IMessageFilter>();
    private bool disposedValue;

    public WindowsMessageHook(IntPtr windowHandle)
    {
      this._windowHandle = windowHandle;
      Application.AddMessageFilter((IMessageFilter) this);
      this._wndProc = new WindowsMessageHook.WndProcCallback(this.WndProc);
      this._previousWndProc = (IntPtr) WindowsApi.SetWindowLong(this._windowHandle, -4, (int) Marshal.GetFunctionPointerForDelegate((Delegate) this._wndProc));
    }

    public void AddMessageFilter(IMessageFilter filter) => this._filters.Add(filter);

    public void RemoveMessageFilter(IMessageFilter filter) => this._filters.Remove(filter);

    private IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam)
    {
      Message message = Message.Create(hWnd, msg, wParam, lParam);
      return this.InternalWndProc(ref message) ? message.Result : WindowsApi.CallWindowProc(this._previousWndProc, message.HWnd, message.Msg, message.WParam, message.LParam);
    }

    public bool PreFilterMessage(ref Message message) => message.Msg != 258 && this.InternalWndProc(ref message);

    private bool InternalWndProc(ref Message message)
    {
      foreach (IMessageFilter filter in this._filters)
      {
        if (filter.PreFilterMessage(ref message))
          return true;
      }
      return false;
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposedValue)
        return;
      int num = disposing ? 1 : 0;
      Application.RemoveMessageFilter((IMessageFilter) this);
      WindowsApi.SetWindowLong(this._windowHandle, -4, (int) this._previousWndProc);
      this.disposedValue = true;
    }

    ~WindowsMessageHook() => this.Dispose(false);

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private delegate IntPtr WndProcCallback(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
  }
}
