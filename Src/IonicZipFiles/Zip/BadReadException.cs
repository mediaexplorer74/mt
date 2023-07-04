// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.BadReadException
// Assembly: Ionic.Zip.CF, Version=1.9.1.8, Culture=neutral, PublicKeyToken=edbe51ad942a3f5c
// MVID: 3D63C7F2-9FA1-46C2-9CAE-E29323A47C7A
// Assembly location: C:\Users\Admin\Desktop\re\CF.dll

using System;
using System.Runtime.InteropServices;

namespace Ionic.Zip
{
  [Guid("ebc25cf6-9120-4283-b972-0e5520d0000A")]
  [Serializable]
  public class BadReadException : ZipException
  {
    public BadReadException()
    {
    }

    public BadReadException(string message)
      : base(message)
    {
    }

    public BadReadException(string message, Exception innerException)
      : base(message, innerException)
    {
    }
  }
}
