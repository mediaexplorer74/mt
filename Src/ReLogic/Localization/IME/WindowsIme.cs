// Decompiled with JetBrains decompiler
// Type: ReLogic.Localization.IME.WindowsIme
// Assembly: ReLogic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 33513C57-D94A-4BED-935B-7012D40A5531
// Assembly location: C:\Users\Admin\Desktop\re\ReLogic.dll

using ReLogic.Localization.IME.Windows;
using ReLogic.OS.Windows;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ReLogic.Localization.IME
{
  internal class WindowsIme : PlatformIme, IMessageFilter
  {
    private IntPtr _windowHandle;
    private bool _isFocused;
    private WindowsMessageHook _wndProcHook;
    private bool _disposedValue;

    public override string CompositionString => Marshal.PtrToStringUni(ImeUi.GetCompositionString()).ToString();

    public override bool IsCandidateListVisible => ImeUi.IsCandidateListVisible();

    public override uint SelectedCandidate => ImeUi.GetCandidateSelection();

    public override uint CandidateCount => ImeUi.GetCandidatePageSize();

    public WindowsIme(WindowsMessageHook wndProcHook, IntPtr windowHandle)
    {
      this._wndProcHook = wndProcHook;
      this._windowHandle = windowHandle;
      this._isFocused = ImmNative.GetForegroundWindow() == this._windowHandle;
      this._wndProcHook.AddMessageFilter((IMessageFilter) this);
      ImeUi.Initialize(this._windowHandle);
    }

    protected override void OnEnable()
    {
      if (!this._isFocused)
        return;
      ImeUi.Enable(true);
    }

    protected override void OnDisable()
    {
      ImeUi.FinalizeString();
      ImeUi.Enable(false);
    }

    public override string GetCandidate(uint index) => Marshal.PtrToStringUni(ImeUi.GetCandidate(index));

    public bool PreFilterMessage(ref Message message)
    {
      if (message.Msg == 8)
      {
        ImeUi.Enable(false);
        this._isFocused = false;
        return true;
      }
      if (message.Msg == 7)
      {
        if (this.IsEnabled)
          ImeUi.Enable(true);
        this._isFocused = true;
        return true;
      }
      if (!this.IsEnabled)
        return false;
      bool trapped = false;
      IntPtr lparam = message.LParam;
      ImeUi.ProcessMessage(message.HWnd, message.Msg, message.WParam, ref lparam, ref trapped);
      message.LParam = lparam;
      if (trapped)
        return true;
      switch (message.Msg)
      {
        case 81:
          return true;
        case 256:
          if (!ImeUi.ShouldIgnoreHotKey(ref message))
          {
            ImmNative.TranslateMessage(ref message);
            break;
          }
          break;
        case 258:
          if (this.OnKeyPress != null)
          {
            this.OnKeyPress((char) message.WParam.ToInt32());
            break;
          }
          break;
        case 269:
          return true;
        case 641:
          message.LParam = IntPtr.Zero;
          break;
        case 642:
          return true;
      }
      return false;
    }

    protected override void Dispose(bool disposing)
    {
      if (this._disposedValue)
        return;
      int num = disposing ? 1 : 0;
      if (this.IsEnabled)
        this.Disable();
      this._wndProcHook.RemoveMessageFilter((IMessageFilter) this);
      ImeUi.Uninitialize();
      this._disposedValue = true;
    }

    ~WindowsIme() => this.Dispose(false);
  }
}
