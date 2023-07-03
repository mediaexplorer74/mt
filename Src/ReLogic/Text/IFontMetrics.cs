// Decompiled with JetBrains decompiler
// Type: ReLogic.Text.IFontMetrics
// Assembly: ReLogic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 33513C57-D94A-4BED-935B-7012D40A5531
// Assembly location: C:\Users\Admin\Desktop\re\ReLogic.dll

namespace ReLogic.Text
{
  public interface IFontMetrics
  {
    int LineSpacing { get; }

    float CharacterSpacing { get; }

    GlyphMetrics GetCharacterMetrics(char character);
  }
}
