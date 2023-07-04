// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.ExtractProgressEventArgs
// Assembly: Ionic.Zip.CF, Version=1.9.1.8, Culture=neutral, PublicKeyToken=edbe51ad942a3f5c
// MVID: 3D63C7F2-9FA1-46C2-9CAE-E29323A47C7A
// Assembly location: C:\Users\Admin\Desktop\re\CF.dll

namespace Ionic.Zip
{
  public class ExtractProgressEventArgs : ZipProgressEventArgs
  {
    private int _entriesExtracted;
    private string _target;

    internal ExtractProgressEventArgs(
      string archiveName,
      bool before,
      int entriesTotal,
      int entriesExtracted,
      ZipEntry entry,
      string extractLocation)
      : base(archiveName, before ? ZipProgressEventType.Extracting_BeforeExtractEntry : ZipProgressEventType.Extracting_AfterExtractEntry)
    {
      this.EntriesTotal = entriesTotal;
      this.CurrentEntry = entry;
      this._entriesExtracted = entriesExtracted;
      this._target = extractLocation;
    }

    internal ExtractProgressEventArgs(string archiveName, ZipProgressEventType flavor)
      : base(archiveName, flavor)
    {
    }

    internal ExtractProgressEventArgs()
    {
    }

    internal static ExtractProgressEventArgs BeforeExtractEntry(
      string archiveName,
      ZipEntry entry,
      string extractLocation)
    {
      ExtractProgressEventArgs entry1 = new ExtractProgressEventArgs();
      entry1.ArchiveName = archiveName;
      entry1.EventType = ZipProgressEventType.Extracting_BeforeExtractEntry;
      entry1.CurrentEntry = entry;
      entry1._target = extractLocation;
      return entry1;
    }

    internal static ExtractProgressEventArgs ExtractExisting(
      string archiveName,
      ZipEntry entry,
      string extractLocation)
    {
      ExtractProgressEventArgs existing = new ExtractProgressEventArgs();
      existing.ArchiveName = archiveName;
      existing.EventType = ZipProgressEventType.Extracting_ExtractEntryWouldOverwrite;
      existing.CurrentEntry = entry;
      existing._target = extractLocation;
      return existing;
    }

    internal static ExtractProgressEventArgs AfterExtractEntry(
      string archiveName,
      ZipEntry entry,
      string extractLocation)
    {
      ExtractProgressEventArgs entry1 = new ExtractProgressEventArgs();
      entry1.ArchiveName = archiveName;
      entry1.EventType = ZipProgressEventType.Extracting_AfterExtractEntry;
      entry1.CurrentEntry = entry;
      entry1._target = extractLocation;
      return entry1;
    }

    internal static ExtractProgressEventArgs ExtractAllStarted(
      string archiveName,
      string extractLocation)
    {
      return new ExtractProgressEventArgs(archiveName, ZipProgressEventType.Extracting_BeforeExtractAll)
      {
        _target = extractLocation
      };
    }

    internal static ExtractProgressEventArgs ExtractAllCompleted(
      string archiveName,
      string extractLocation)
    {
      return new ExtractProgressEventArgs(archiveName, ZipProgressEventType.Extracting_AfterExtractAll)
      {
        _target = extractLocation
      };
    }

    internal static ExtractProgressEventArgs ByteUpdate(
      string archiveName,
      ZipEntry entry,
      long bytesWritten,
      long totalBytes)
    {
      ExtractProgressEventArgs progressEventArgs = new ExtractProgressEventArgs(archiveName, ZipProgressEventType.Extracting_EntryBytesWritten);
      progressEventArgs.ArchiveName = archiveName;
      progressEventArgs.CurrentEntry = entry;
      progressEventArgs.BytesTransferred = bytesWritten;
      progressEventArgs.TotalBytesToTransfer = totalBytes;
      return progressEventArgs;
    }

    public int EntriesExtracted => this._entriesExtracted;

    public string ExtractLocation => this._target;
  }
}
