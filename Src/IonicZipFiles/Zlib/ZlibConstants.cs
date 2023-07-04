// Decompiled with JetBrains decompiler
// Type: Ionic.Zlib.ZlibConstants
// Assembly: Ionic.Zip.CF, Version=1.9.1.8, Culture=neutral, PublicKeyToken=edbe51ad942a3f5c
// MVID: 3D63C7F2-9FA1-46C2-9CAE-E29323A47C7A
// Assembly location: C:\Users\Admin\Desktop\re\CF.dll

namespace Ionic.Zlib
{
  public static class ZlibConstants
  {
    public const int WindowBitsMax = 15;
    public const int WindowBitsDefault = 15;
    public const int Z_OK = 0;
    public const int Z_STREAM_END = 1;
    public const int Z_NEED_DICT = 2;
    public const int Z_STREAM_ERROR = -2;
    public const int Z_DATA_ERROR = -3;
    public const int Z_BUF_ERROR = -5;
    public const int WorkingBufferSizeDefault = 8192;
    public const int WorkingBufferSizeMin = 1024;
  }
}
