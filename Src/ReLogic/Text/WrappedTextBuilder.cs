// Decompiled with JetBrains decompiler
// Type: ReLogic.Text.WrappedTextBuilder
// Assembly: ReLogic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 33513C57-D94A-4BED-935B-7012D40A5531
// Assembly location: C:\Users\Admin\Desktop\re\ReLogic.dll

using System.Globalization;
using System.IO;
using System.Text;

namespace ReLogic.Text
{
  public class WrappedTextBuilder
  {
    private IFontMetrics _font;
    private CultureInfo _culture;
    private float _maxWidth;
    private StringBuilder _completedText = new StringBuilder();
    private StringBuilder _workingLine = new StringBuilder();
    private float _workingLineWidth;

    public WrappedTextBuilder(IFontMetrics font, float maxWidth, CultureInfo culture)
    {
      this._font = font;
      this._maxWidth = maxWidth;
      this._culture = culture;
      this._workingLineWidth = 0.0f;
    }

    public void CommitWorkingLine()
    {
      if (!this._completedText.IsEmpty())
        this._completedText.Append('\n');
      this._workingLineWidth = 0.0f;
      this._completedText.Append(this._workingLine.ToString());
      this._workingLine.Clear();
    }

    private void Append(WrappedTextBuilder.NonBreakingText textToken)
    {
      float num = !this._workingLine.IsEmpty() ? this._workingLineWidth + this._font.CharacterSpacing + textToken.Width : textToken.WidthOnNewLine;
      if ((double) textToken.WidthOnNewLine > (double) this._maxWidth)
      {
        if (!this._workingLine.IsEmpty())
          this.CommitWorkingLine();
        this.Append(textToken.GetAsWrappedText(this._maxWidth));
      }
      else if ((double) num <= (double) this._maxWidth)
      {
        this._workingLineWidth = num;
        this._workingLine.Append(textToken.Text);
      }
      else if (this._workingLine.IsEmpty())
      {
        this._completedText.Append(textToken.Text);
        this._workingLine.Clear();
        this._workingLineWidth = 0.0f;
      }
      else
      {
        this.CommitWorkingLine();
        if (textToken.IsWhitespace)
          return;
        this._workingLine.Append(textToken.Text);
        this._workingLineWidth = textToken.WidthOnNewLine;
      }
    }

    public void Append(string text)
    {
      StringReader reader = new StringReader(text);
      this._completedText.EnsureCapacity(this._completedText.Capacity + text.Length);
      while (reader.Peek() > 0)
      {
        if ((ushort) reader.Peek() == (ushort) 10)
        {
          reader.Read();
          this.CommitWorkingLine();
        }
        else
          this.Append(new WrappedTextBuilder.NonBreakingText(this._font, reader.ReadUntilBreakable(this._culture)));
      }
    }

    public override string ToString() => this._completedText.IsEmpty() ? this._workingLine.ToString() : this._completedText.ToString() + "\n" + this._workingLine.ToString();

    private struct NonBreakingText
    {
      public readonly string Text;
      public readonly float Width;
      public readonly float WidthOnNewLine;
      public readonly bool IsWhitespace;
      private IFontMetrics _font;

      public NonBreakingText(IFontMetrics font, string text)
      {
        this.Text = text;
        this.IsWhitespace = true;
        float num1 = 0.0f;
        float num2 = 0.0f;
        this._font = font;
        for (int index = 0; index < text.Length; ++index)
        {
          GlyphMetrics characterMetrics = font.GetCharacterMetrics(text[index]);
          if (index == 0)
            num2 = characterMetrics.KernedWidthOnNewLine - characterMetrics.KernedWidth;
          else
            num1 += font.CharacterSpacing;
          num1 += characterMetrics.KernedWidth;
          if (text[index] != ' ')
            this.IsWhitespace = false;
        }
        this.Width = num1;
        this.WidthOnNewLine = num1 + num2;
      }

      public string GetAsWrappedText(float maxWidth)
      {
        float num = 0.0f;
        StringBuilder stringBuilder = new StringBuilder(this.Text.Length);
        for (int index = 0; index < this.Text.Length; ++index)
        {
          GlyphMetrics characterMetrics = this._font.GetCharacterMetrics(this.Text[index]);
          if (index == 0)
            num += characterMetrics.KernedWidthOnNewLine;
          else
            num += this._font.CharacterSpacing + characterMetrics.KernedWidth;
          if ((double) num > (double) maxWidth)
          {
            num = characterMetrics.KernedWidthOnNewLine;
            stringBuilder.Append('\n');
          }
          stringBuilder.Append(this.Text[index]);
        }
        return stringBuilder.ToString();
      }
    }
  }
}
