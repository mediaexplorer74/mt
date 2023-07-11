// Decompiled with JetBrains decompiler
// Type: ReLogic.Localization.IME.WindowsIme
// Assembly: ReLogic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 33513C57-D94A-4BED-935B-7012D40A5531
// Assembly location: C:\Users\Admin\Desktop\re\ReLogic.dll

namespace ReLogic.Localization.IME
{
    internal interface IMessageFilter
    {
        bool PreFilterMessage(ref Message message);
    }
}