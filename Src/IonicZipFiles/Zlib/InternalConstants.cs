// Decompiled with JetBrains decompiler
// Type: Ionic.Zlib.InternalConstants
// Assembly: Ionic.Zip.CF, Version=1.9.1.8, Culture=neutral, PublicKeyToken=edbe51ad942a3f5c
// MVID: 3D63C7F2-9FA1-46C2-9CAE-E29323A47C7A
// Assembly location: C:\Users\Admin\Desktop\re\CF.dll

namespace Ionic.Zlib
{
  internal static class InternalConstants
  {
    internal static readonly int MAX_BITS = 15;
    internal static readonly int BL_CODES = 19;
    internal static readonly int D_CODES = 30;
    internal static readonly int LITERALS = 256;
    internal static readonly int LENGTH_CODES = 29;
    internal static readonly int L_CODES = InternalConstants.LITERALS + 1 + InternalConstants.LENGTH_CODES;
    internal static readonly int MAX_BL_BITS = 7;
    internal static readonly int REP_3_6 = 16;
    internal static readonly int REPZ_3_10 = 17;
    internal static readonly int REPZ_11_138 = 18;
  }
}
