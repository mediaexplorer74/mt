// Decompiled with JetBrains decompiler
// Type: ReLogic.Graphics.DynamicSpriteFontReader
// Assembly: ReLogic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 33513C57-D94A-4BED-935B-7012D40A5531
// Assembly location: C:\Users\Admin\Desktop\re\ReLogic.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ReLogic.Graphics
{
  public class DynamicSpriteFontReader : ContentTypeReader<DynamicSpriteFont>
  {
    protected override DynamicSpriteFont Read(
      ContentReader input,
      DynamicSpriteFont existingInstance)
    {
      double spacing = (double) input.ReadSingle();
      int num = input.ReadInt32();
      char ch = input.ReadChar();
      int lineSpacing = num;
      int defaultCharacter = (int) ch;
      DynamicSpriteFont dynamicSpriteFont = new DynamicSpriteFont((float) spacing, lineSpacing, (char) defaultCharacter);
      int length = input.ReadInt32();
      FontPage[] pages = new FontPage[length];
      for (int index = 0; index < length; ++index)
      {
        Texture2D texture = input.ReadObject<Texture2D>();
        List<Rectangle> glyphs = input.ReadObject<List<Rectangle>>();
        List<Rectangle> padding = input.ReadObject<List<Rectangle>>();
        List<char> characters = input.ReadObject<List<char>>();
        List<Vector3> kerning = input.ReadObject<List<Vector3>>();
        pages[index] = new FontPage(texture, glyphs, padding, characters, kerning);
      }
      dynamicSpriteFont.SetPages(pages);
      return dynamicSpriteFont;
    }
  }
}
