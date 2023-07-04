// Decompiled with JetBrains decompiler
// Type: Ionic.Zip.ZipConstants
// Assembly: Ionic.Zip.CF, Version=1.9.1.8, Culture=neutral, PublicKeyToken=edbe51ad942a3f5c
// MVID: 3D63C7F2-9FA1-46C2-9CAE-E29323A47C7A
// Assembly location: C:\Users\Admin\Desktop\re\CF.dll

namespace Ionic.Zip
{
  internal static class ZipConstants
  {
    public const uint PackedToRemovableMedia = 808471376;
    public const uint Zip64EndOfCentralDirectoryRecordSignature = 101075792;
    public const uint Zip64EndOfCentralDirectoryLocatorSignature = 117853008;
    public const uint EndOfCentralDirectorySignature = 101010256;
    public const int ZipEntrySignature = 67324752;
    public const int ZipEntryDataDescriptorSignature = 134695760;
    public const int SplitArchiveSignature = 134695760;
    public const int ZipDirEntrySignature = 33639248;
    public const int AesKeySize = 192;
    public const int AesBlockSize = 128;
    public const ushort AesAlgId128 = 26126;
    public const ushort AesAlgId192 = 26127;
    public const ushort AesAlgId256 = 26128;
  }
}
