// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.ZipProgressEventType
// Assembly: Ionic.Zip.CF, Version=1.9.1.8, Culture=neutral, PublicKeyToken=edbe51ad942a3f5c
// MVID: 3D63C7F2-9FA1-46C2-9CAE-E29323A47C7A
// Assembly location: C:\Users\Admin\Desktop\re\CF.dll

namespace Ionic.Zip
{
  public enum ZipProgressEventType
  {
    Adding_Started,
    Adding_AfterAddEntry,
    Adding_Completed,
    Reading_Started,
    Reading_BeforeReadEntry,
    Reading_AfterReadEntry,
    Reading_Completed,
    Reading_ArchiveBytesRead,
    Saving_Started,
    Saving_BeforeWriteEntry,
    Saving_AfterWriteEntry,
    Saving_Completed,
    Saving_AfterSaveTempArchive,
    Saving_BeforeRenameTempArchive,
    Saving_AfterRenameTempArchive,
    Saving_AfterCompileSelfExtractor,
    Saving_EntryBytesRead,
    Extracting_BeforeExtractEntry,
    Extracting_AfterExtractEntry,
    Extracting_ExtractEntryWouldOverwrite,
    Extracting_EntryBytesWritten,
    Extracting_BeforeExtractAll,
    Extracting_AfterExtractAll,
    Error_Saving,
  }
}
