// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.ZipProgressEventArgs
// Assembly: Ionic.Zip.CF, Version=1.9.1.8, Culture=neutral, PublicKeyToken=edbe51ad942a3f5c
// MVID: 3D63C7F2-9FA1-46C2-9CAE-E29323A47C7A
// Assembly location: C:\Users\Admin\Desktop\re\CF.dll

using System;

namespace Ionic.Zip
{
  public class ZipProgressEventArgs : EventArgs
  {
    private int _entriesTotal;
    private bool _cancel;
    private ZipEntry _latestEntry;
    private ZipProgressEventType _flavor;
    private string _archiveName;
    private long _bytesTransferred;
    private long _totalBytesToTransfer;

    internal ZipProgressEventArgs()
    {
    }

    internal ZipProgressEventArgs(string archiveName, ZipProgressEventType flavor)
    {
      this._archiveName = archiveName;
      this._flavor = flavor;
    }

    public int EntriesTotal
    {
      get => this._entriesTotal;
      set => this._entriesTotal = value;
    }

    public ZipEntry CurrentEntry
    {
      get => this._latestEntry;
      set => this._latestEntry = value;
    }

    public bool Cancel
    {
      get => this._cancel;
      set => this._cancel = this._cancel || value;
    }

    public ZipProgressEventType EventType
    {
      get => this._flavor;
      set => this._flavor = value;
    }

    public string ArchiveName
    {
      get => this._archiveName;
      set => this._archiveName = value;
    }

    public long BytesTransferred
    {
      get => this._bytesTransferred;
      set => this._bytesTransferred = value;
    }

    public long TotalBytesToTransfer
    {
      get => this._totalBytesToTransfer;
      set => this._totalBytesToTransfer = value;
    }
  }
}
