// Decompiled with JetBrains decompiler
// Type: ReLogic.Text.StringReaderWrapExtension
// Assembly: ReLogic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 33513C57-D94A-4BED-935B-7012D40A5531
// Assembly location: C:\Users\Admin\Desktop\re\ReLogic.dll

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace ReLogic.Text
{
  internal static class StringReaderWrapExtension
  {
    private static readonly HashSet<char> InvalidCharactersForLineStart = new HashSet<char>()
    {
      '!',
      '%',
      ')',
      ',',
      '.',
      ':',
      ';',
      '?',
      ']',
      '}',
      '¢',
      '°',
      '·',
      '’',
      '"',
      '"',
      '†',
      '‡',
      '›',
      '℃',
      '∶',
      '、',
      '。',
      '〃',
      '〆',
      '〕',
      '〗',
      '〞',
      '﹚',
      '﹜',
      '！',
      '＂',
      '％',
      '＇',
      '）',
      '，',
      '．',
      '：',
      '；',
      '？',
      '！',
      '］',
      '｝',
      '～',
      ' ',
      '\n'
    };
    private static readonly HashSet<char> InvalidCharactersForLineEnd = new HashSet<char>()
    {
      '$',
      '(',
      '£',
      '¥',
      '·',
      '‘',
      '"',
      '〈',
      '《',
      '「',
      '『',
      '【',
      '〔',
      '〖',
      '〝',
      '﹙',
      '﹛',
      '＄',
      '（',
      '．',
      '［',
      '｛',
      '￡',
      '￥'
    };
    private static readonly CultureInfo[] SUPPORTED_CULTURES = new CultureInfo[9]
    {
      new CultureInfo("en-US"),
      new CultureInfo("de-DE"),
      new CultureInfo("it-IT"),
      new CultureInfo("fr-FR"),
      new CultureInfo("es-ES"),
      new CultureInfo("ru-RU"),
      new CultureInfo("zh-Hans"),
      new CultureInfo("pt-BR"),
      new CultureInfo("pl-PL")
    };
    private static readonly CultureInfo SimplifiedChinese = new CultureInfo("zh-Hans");

    internal static bool IsCultureSupported(CultureInfo culture) => ((IEnumerable<CultureInfo>) StringReaderWrapExtension.SUPPORTED_CULTURES).Contains<CultureInfo>(culture);

    internal static bool IsIgnoredCharacter(char character) => character < ' ' && character != '\n';

    internal static bool CanBreakBetween(char previousChar, char nextChar, CultureInfo culture) => culture.LCID == StringReaderWrapExtension.SimplifiedChinese.LCID && !StringReaderWrapExtension.InvalidCharactersForLineEnd.Contains(previousChar) && !StringReaderWrapExtension.InvalidCharactersForLineStart.Contains(nextChar);

    internal static StringReaderWrapExtension.WrapScanMode GetModeForCharacter(char character)
    {
      if (StringReaderWrapExtension.IsIgnoredCharacter(character))
        return StringReaderWrapExtension.WrapScanMode.None;
      if (character == '\n')
        return StringReaderWrapExtension.WrapScanMode.NewLine;
      return character == ' ' ? StringReaderWrapExtension.WrapScanMode.Space : StringReaderWrapExtension.WrapScanMode.Word;
    }

    internal static string ReadUntilBreakable(this StringReader reader, CultureInfo culture)
    {
      StringBuilder stringBuilder = new StringBuilder();
      char ch = (char) reader.Peek();
      StringReaderWrapExtension.WrapScanMode wrapScanMode1 = StringReaderWrapExtension.WrapScanMode.None;
      while (reader.Peek() > 0)
      {
        if (!StringReaderWrapExtension.IsIgnoredCharacter((char) reader.Peek()))
        {
          char previousChar = ch;
          ch = (char) reader.Peek();
          StringReaderWrapExtension.WrapScanMode wrapScanMode2 = wrapScanMode1;
          wrapScanMode1 = StringReaderWrapExtension.GetModeForCharacter(ch);
          if (!stringBuilder.IsEmpty() && wrapScanMode2 != wrapScanMode1)
            return stringBuilder.ToString();
          if (stringBuilder.IsEmpty())
          {
            stringBuilder.Append((char) reader.Read());
          }
          else
          {
            if (StringReaderWrapExtension.CanBreakBetween(previousChar, ch, culture))
              return stringBuilder.ToString();
            stringBuilder.Append((char) reader.Read());
          }
        }
      }
      return stringBuilder.ToString();
    }

    internal enum WrapScanMode
    {
      Space,
      NewLine,
      Word,
      None,
    }
  }
}
