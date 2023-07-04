// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.ZipErrorEventArgs
// Assembly: Ionic.Zip.CF, Version=1.9.1.8, Culture=neutral, PublicKeyToken=edbe51ad942a3f5c
// MVID: 3D63C7F2-9FA1-46C2-9CAE-E29323A47C7A
// Assembly location: C:\Users\Admin\Desktop\re\CF.dll

using System;

namespace Ionic.Zip
{
  public class ZipErrorEventArgs : ZipProgressEventArgs
  {
    private Exception _exc;

    private ZipErrorEventArgs()
    {
    }

    internal static ZipErrorEventArgs Saving(
      string archiveName,
      ZipEntry entry,
      Exception exception)
    {
      ZipErrorEventArgs zipErrorEventArgs = new ZipErrorEventArgs();
      zipErrorEventArgs.EventType = ZipProgressEventType.Error_Saving;
      zipErrorEventArgs.ArchiveName = archiveName;
      zipErrorEventArgs.CurrentEntry = entry;
      zipErrorEventArgs._exc = exception;
      return zipErrorEventArgs;
    }

    public Exception Exception => this._exc;

    public string FileName => this.CurrentEntry.LocalFileName;
  }
}
