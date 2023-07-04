// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.ReadOptions
// Assembly: Ionic.Zip.CF, Version=1.9.1.8, Culture=neutral, PublicKeyToken=edbe51ad942a3f5c
// MVID: 3D63C7F2-9FA1-46C2-9CAE-E29323A47C7A
// Assembly location: C:\Users\Admin\Desktop\re\CF.dll

using System;
using System.IO;
using System.Text;

namespace Ionic.Zip
{
  public class ReadOptions
  {
    private EventHandler<ReadProgressEventArgs> \u003CReadProgress\u003Ek__BackingField;
    private TextWriter \u003CStatusMessageWriter\u003Ek__BackingField;
    private Encoding \u003CEncoding\u003Ek__BackingField;

    public EventHandler<ReadProgressEventArgs> ReadProgress
    {
      get => this.\u003CReadProgress\u003Ek__BackingField;
      set => this.\u003CReadProgress\u003Ek__BackingField = value;
    }

    public TextWriter StatusMessageWriter
    {
      get => this.\u003CStatusMessageWriter\u003Ek__BackingField;
      set => this.\u003CStatusMessageWriter\u003Ek__BackingField = value;
    }

    public Encoding Encoding
    {
      get => this.\u003CEncoding\u003Ek__BackingField;
      set => this.\u003CEncoding\u003Ek__BackingField = value;
    }
  }
}
