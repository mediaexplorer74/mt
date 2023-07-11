// Decompiled with JetBrains decompiler
// Type: ReLogic.Localization.IME.WindowsIme
// Assembly: ReLogic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 33513C57-D94A-4BED-935B-7012D40A5531
// Assembly location: C:\Users\Admin\Desktop\re\ReLogic.dll

using System;

namespace ReLogic.Localization.IME
{
    public class Message
    {
        internal int Msg;
        internal IntPtr LParam;
        internal IntPtr HWnd;
        internal IntPtr WParam;

        public IntPtr Result { get; internal set; }

        internal static Message Create(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam)
        {
            throw new NotImplementedException();
        }
    }
}