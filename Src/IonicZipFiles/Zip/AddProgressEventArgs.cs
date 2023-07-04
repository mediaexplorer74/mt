// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.AddProgressEventArgs
// Assembly: Ionic.Zip.CF, Version=1.9.1.8, Culture=neutral, PublicKeyToken=edbe51ad942a3f5c
// MVID: 3D63C7F2-9FA1-46C2-9CAE-E29323A47C7A
// Assembly location: C:\Users\Admin\Desktop\re\CF.dll

namespace Ionic.Zip
{
  public class AddProgressEventArgs : ZipProgressEventArgs
  {
    internal AddProgressEventArgs()
    {
    }

    private AddProgressEventArgs(string archiveName, ZipProgressEventType flavor)
      : base(archiveName, flavor)
    {
    }

    internal static AddProgressEventArgs AfterEntry(
      string archiveName,
      ZipEntry entry,
      int entriesTotal)
    {
      AddProgressEventArgs progressEventArgs = new AddProgressEventArgs(archiveName, ZipProgressEventType.Adding_AfterAddEntry);
      progressEventArgs.EntriesTotal = entriesTotal;
      progressEventArgs.CurrentEntry = entry;
      return progressEventArgs;
    }

    internal static AddProgressEventArgs Started(string archiveName) => new AddProgressEventArgs(archiveName, ZipProgressEventType.Adding_Started);

    internal static AddProgressEventArgs Completed(string archiveName) => new AddProgressEventArgs(archiveName, ZipProgressEventType.Adding_Completed);
  }
}
