// Decompiled with JetBrains decompiler
// Type: Ionic.Zlib.SharedUtils
// Assembly: Ionic.Zip.CF, Version=1.9.1.8, Culture=neutral, PublicKeyToken=edbe51ad942a3f5c
// MVID: 3D63C7F2-9FA1-46C2-9CAE-E29323A47C7A
// Assembly location: C:\Users\Admin\Desktop\re\CF.dll

using System.IO;
using System.Text;

namespace Ionic.Zlib
{
  internal class SharedUtils
  {
    public static int URShift(int number, int bits) => number >>> bits;

    public static int ReadInput(TextReader sourceTextReader, byte[] target, int start, int count)
    {
      if (target.Length == 0)
        return 0;
      char[] buffer = new char[target.Length];
      int num = sourceTextReader.Read(buffer, start, count);
      if (num == 0)
        return -1;
      for (int index = start; index < start + num; ++index)
        target[index] = (byte) buffer[index];
      return num;
    }

    internal static byte[] ToByteArray(string sourceString) => Encoding.UTF8.GetBytes(sourceString);

    internal static char[] ToCharArray(byte[] byteArray) => Encoding.UTF8.GetChars(byteArray);
  }
}
