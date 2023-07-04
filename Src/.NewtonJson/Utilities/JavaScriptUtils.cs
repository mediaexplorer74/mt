// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.JavaScriptUtils
// Assembly: Newtonsoft.Json, Version=10.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 216EEFC5-80B5-4735-B900-1F7A1E8A25B9
// Assembly location: C:\Users\Admin\Desktop\re\Json.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Newtonsoft.Json.Utilities
{
  internal static class JavaScriptUtils
  {
    internal static readonly bool[] SingleQuoteCharEscapeFlags = new bool[128];
    internal static readonly bool[] DoubleQuoteCharEscapeFlags = new bool[128];
    internal static readonly bool[] HtmlCharEscapeFlags = new bool[128];
    private const int UnicodeTextLength = 6;
    private const string EscapedUnicodeText = "!";

    static JavaScriptUtils()
    {
      IList<char> first = (IList<char>) new List<char>()
      {
        '\n',
        '\r',
        '\t',
        '\\',
        '\f',
        '\b'
      };
      for (int index = 0; index < 32; ++index)
        first.Add((char) index);
      foreach (char index in first.Union<char>((IEnumerable<char>) new char[1]
      {
        '\''
      }))
        JavaScriptUtils.SingleQuoteCharEscapeFlags[(int) index] = true;
      foreach (char index in first.Union<char>((IEnumerable<char>) new char[1]
      {
        '"'
      }))
        JavaScriptUtils.DoubleQuoteCharEscapeFlags[(int) index] = true;
      foreach (char index in first.Union<char>((IEnumerable<char>) new char[5]
      {
        '"',
        '\'',
        '<',
        '>',
        '&'
      }))
        JavaScriptUtils.HtmlCharEscapeFlags[(int) index] = true;
    }

    public static bool[] GetCharEscapeFlags(
      StringEscapeHandling stringEscapeHandling,
      char quoteChar)
    {
      if (stringEscapeHandling == StringEscapeHandling.EscapeHtml)
        return JavaScriptUtils.HtmlCharEscapeFlags;
      return quoteChar == '"' ? JavaScriptUtils.DoubleQuoteCharEscapeFlags : JavaScriptUtils.SingleQuoteCharEscapeFlags;
    }

    public static bool ShouldEscapeJavaScriptString(string s, bool[] charEscapeFlags)
    {
      if (s == null)
        return false;
      foreach (char index in s)
      {
        if ((int) index >= charEscapeFlags.Length || charEscapeFlags[(int) index])
          return true;
      }
      return false;
    }

    public static void WriteEscapedJavaScriptString(
      TextWriter writer,
      string s,
      char delimiter,
      bool appendDelimiters,
      bool[] charEscapeFlags,
      StringEscapeHandling stringEscapeHandling,
      IArrayPool<char> bufferPool,
      ref char[] writeBuffer)
    {
      if (appendDelimiters)
        writer.Write(delimiter);
      if (!string.IsNullOrEmpty(s))
      {
        int num1 = JavaScriptUtils.FirstCharToEscape(s, charEscapeFlags, stringEscapeHandling);
        switch (num1)
        {
          case -1:
            writer.Write(s);
            break;
          case 0:
            for (int index = num1; index < s.Length; ++index)
            {
              char c = s[index];
              if ((int) c >= charEscapeFlags.Length || charEscapeFlags[(int) c])
              {
                string a;
                switch (c)
                {
                  case '\b':
                    a = "\\b";
                    break;
                  case '\t':
                    a = "\\t";
                    break;
                  case '\n':
                    a = "\\n";
                    break;
                  case '\f':
                    a = "\\f";
                    break;
                  case '\r':
                    a = "\\r";
                    break;
                  case '\\':
                    a = "\\\\";
                    break;
                  case '\u0085':
                    a = "\\u0085";
                    break;
                  case '\u2028':
                    a = "\\u2028";
                    break;
                  case '\u2029':
                    a = "\\u2029";
                    break;
                  default:
                    if ((int) c < charEscapeFlags.Length || stringEscapeHandling == StringEscapeHandling.EscapeNonAscii)
                    {
                      if (c == '\'' && stringEscapeHandling != StringEscapeHandling.EscapeHtml)
                      {
                        a = "\\'";
                        break;
                      }
                      if (c == '"' && stringEscapeHandling != StringEscapeHandling.EscapeHtml)
                      {
                        a = "\\\"";
                        break;
                      }
                      if (writeBuffer == null || writeBuffer.Length < 6)
                        writeBuffer = BufferUtils.EnsureBufferSize(bufferPool, 6, writeBuffer);
                      StringUtils.ToCharAsUnicode(c, writeBuffer);
                      a = "!";
                      break;
                    }
                    a = (string) null;
                    break;
                }
                if (a != null)
                {
                  bool flag = string.Equals(a, "!");
                  if (index > num1)
                  {
                    int minSize = index - num1 + (flag ? 6 : 0);
                    int num2 = flag ? 6 : 0;
                    if (writeBuffer == null || writeBuffer.Length < minSize)
                    {
                      char[] destinationArray = BufferUtils.RentBuffer(bufferPool, minSize);
                      if (flag)
                        Array.Copy((Array) writeBuffer, (Array) destinationArray, 6);
                      BufferUtils.ReturnBuffer(bufferPool, writeBuffer);
                      writeBuffer = destinationArray;
                    }
                    s.CopyTo(num1, writeBuffer, num2, minSize - num2);
                    writer.Write(writeBuffer, num2, minSize - num2);
                  }
                  num1 = index + 1;
                  if (!flag)
                    writer.Write(a);
                  else
                    writer.Write(writeBuffer, 0, 6);
                }
              }
            }
            int num3 = s.Length - num1;
            if (num3 > 0)
            {
              if (writeBuffer == null || writeBuffer.Length < num3)
                writeBuffer = BufferUtils.EnsureBufferSize(bufferPool, num3, writeBuffer);
              s.CopyTo(num1, writeBuffer, 0, num3);
              writer.Write(writeBuffer, 0, num3);
              break;
            }
            break;
          default:
            if (writeBuffer == null || writeBuffer.Length < num1)
              writeBuffer = BufferUtils.EnsureBufferSize(bufferPool, num1, writeBuffer);
            s.CopyTo(0, writeBuffer, 0, num1);
            writer.Write(writeBuffer, 0, num1);
            goto case 0;
        }
      }
      if (!appendDelimiters)
        return;
      writer.Write(delimiter);
    }

    public static string ToEscapedJavaScriptString(
      string value,
      char delimiter,
      bool appendDelimiters,
      StringEscapeHandling stringEscapeHandling)
    {
      bool[] charEscapeFlags = JavaScriptUtils.GetCharEscapeFlags(stringEscapeHandling, delimiter);
      using (StringWriter stringWriter = StringUtils.CreateStringWriter(value != null ? value.Length : 16))
      {
        char[] writeBuffer = (char[]) null;
        JavaScriptUtils.WriteEscapedJavaScriptString((TextWriter) stringWriter, value, delimiter, appendDelimiters, charEscapeFlags, stringEscapeHandling, (IArrayPool<char>) null, ref writeBuffer);
        return stringWriter.ToString();
      }
    }

    private static int FirstCharToEscape(
      string s,
      bool[] charEscapeFlags,
      StringEscapeHandling stringEscapeHandling)
    {
      for (int index1 = 0; index1 != s.Length; ++index1)
      {
        char index2 = s[index1];
        if ((int) index2 < charEscapeFlags.Length)
        {
          if (charEscapeFlags[(int) index2])
            return index1;
        }
        else if (stringEscapeHandling == StringEscapeHandling.EscapeNonAscii || index2 == '\u0085' || index2 == '\u2028' || index2 == '\u2029')
          return index1;
      }
      return -1;
    }
  }
}
