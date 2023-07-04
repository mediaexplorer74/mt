// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.StringReferenceExtensions
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using System;

namespace Newtonsoft.Json.Utilities
{
  internal static class StringReferenceExtensions
  {
    public static int IndexOf(this StringReference s, char c, int startIndex, int length)
    {
      int num = Array.IndexOf<char>(s.Chars, c, s.StartIndex + startIndex, length);
      return num == -1 ? -1 : num - s.StartIndex;
    }

    public static bool StartsWith(this StringReference s, string text)
    {
      if (text.Length > s.Length)
        return false;
      char[] chars = s.Chars;
      for (int index = 0; index < text.Length; ++index)
      {
        if ((int) text[index] != (int) chars[index + s.StartIndex])
          return false;
      }
      return true;
    }

    public static bool EndsWith(this StringReference s, string text)
    {
      if (text.Length > s.Length)
        return false;
      char[] chars = s.Chars;
      int num = s.StartIndex + s.Length - text.Length;
      for (int index = 0; index < text.Length; ++index)
      {
        if ((int) text[index] != (int) chars[index + num])
          return false;
      }
      return true;
    }
  }
}
