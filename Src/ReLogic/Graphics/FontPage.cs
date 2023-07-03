// Decompiled with JetBrains decompiler
// Type: ReLogic.Graphics.FontPage
// Assembly: ReLogic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 33513C57-D94A-4BED-935B-7012D40A5531
// Assembly location: C:\Users\Admin\Desktop\re\ReLogic.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ReLogic.Graphics
{
  internal sealed class FontPage
  {
    public readonly Texture2D Texture;
    public readonly List<Rectangle> Glyphs;
    public readonly List<Rectangle> Padding;
    public readonly List<char> Characters;
    public readonly List<Vector3> Kerning;

    public FontPage(
      Texture2D texture,
      List<Rectangle> glyphs,
      List<Rectangle> padding,
      List<char> characters,
      List<Vector3> kerning)
    {
      this.Texture = texture;
      this.Glyphs = glyphs;
      this.Padding = padding;
      this.Characters = characters;
      this.Kerning = kerning;
    }
  }
}
