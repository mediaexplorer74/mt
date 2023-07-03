// Decompiled with JetBrains decompiler
// Type: ReLogic.Text.GlyphMetrics
// Assembly: ReLogic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 33513C57-D94A-4BED-935B-7012D40A5531
// Assembly location: C:\Users\Admin\Desktop\re\ReLogic.dll

using System;

namespace ReLogic.Text
{
  public struct GlyphMetrics
  {
    public readonly float LeftPadding;
    public readonly float CharacterWidth;
    public readonly float RightPadding;

    public float KernedWidth => this.LeftPadding + this.CharacterWidth + this.RightPadding;

    public float KernedWidthOnNewLine => Math.Max(0.0f, this.LeftPadding) + this.CharacterWidth + this.RightPadding;

    private GlyphMetrics(float leftPadding, float characterWidth, float rightPadding)
    {
      this.LeftPadding = leftPadding;
      this.CharacterWidth = characterWidth;
      this.RightPadding = rightPadding;
    }

    public static GlyphMetrics FromKerningData(
      float leftPadding,
      float characterWidth,
      float rightPadding)
    {
      return new GlyphMetrics(leftPadding, characterWidth, rightPadding);
    }
  }
}
