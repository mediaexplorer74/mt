// Decompiled with JetBrains decompiler
// Type: ReLogic.Graphics.DynamicSpriteFont
// Assembly: ReLogic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 33513C57-D94A-4BED-935B-7012D40A5531
// Assembly location: C:\Users\Admin\Desktop\re\ReLogic.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Text;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace ReLogic.Graphics
{
  public class DynamicSpriteFont : IFontMetrics
  {
    private Dictionary<char, DynamicSpriteFont.SpriteCharacterData> _spriteCharacters = new Dictionary<char, DynamicSpriteFont.SpriteCharacterData>();
    private DynamicSpriteFont.SpriteCharacterData _defaultCharacterData;
    public readonly char DefaultCharacter;
    private readonly float _characterSpacing;
    private readonly int _lineSpacing;

    public float CharacterSpacing => this._characterSpacing;

    public int LineSpacing => this._lineSpacing;

    public DynamicSpriteFont(float spacing, int lineSpacing, char defaultCharacter)
    {
      this._characterSpacing = spacing;
      this._lineSpacing = lineSpacing;
      this.DefaultCharacter = defaultCharacter;
    }

    public bool IsCharacterSupported(char character) => character == '\n' || character == '\r' || this._spriteCharacters.ContainsKey(character);

    public bool AreCharactersSupported(IEnumerable<char> characters)
    {
      foreach (char character in characters)
      {
        if (!this.IsCharacterSupported(character))
          return false;
      }
      return true;
    }

    internal void SetPages(FontPage[] pages)
    {
      int capacity = 0;
      foreach (FontPage page in pages)
        capacity += page.Characters.Count;
      this._spriteCharacters = new Dictionary<char, DynamicSpriteFont.SpriteCharacterData>(capacity);
      foreach (FontPage page in pages)
      {
        for (int index = 0; index < page.Characters.Count; ++index)
        {
          this._spriteCharacters.Add(page.Characters[index], new DynamicSpriteFont.SpriteCharacterData(page.Texture, page.Glyphs[index], page.Padding[index], page.Kerning[index]));
          if ((int) page.Characters[index] == (int) this.DefaultCharacter)
            this._defaultCharacterData = this._spriteCharacters[page.Characters[index]];
        }
      }
    }

    internal void InternalDraw(
      string text,
      SpriteBatch spriteBatch,
      Vector2 startPosition,
      Color color,
      float rotation,
      Vector2 origin,
      ref Vector2 scale,
      SpriteEffects spriteEffects,
      float depth)
    {
      Matrix matrix = Matrix.CreateTranslation(-origin.X * scale.X, -origin.Y * scale.Y, 0.0f) * Matrix.CreateRotationZ(rotation);
      Vector2 zero = Vector2.Zero;
      Vector2 one = Vector2.One;
      bool flag = true;
      float num = 0.0f;
      if (spriteEffects != SpriteEffects.None)
      {
        Vector2 vector2 = this.MeasureString(text);
        if (spriteEffects.HasFlag((Enum) SpriteEffects.FlipHorizontally))
        {
          num = vector2.X * scale.X;
          one.X = -1f;
        }
        if (spriteEffects.HasFlag((Enum) SpriteEffects.FlipVertically))
        {
          zero.Y = (vector2.Y - (float) this.LineSpacing) * scale.Y;
          one.Y = -1f;
        }
      }
      zero.X = num;
      for (int index = 0; index < text.Length; ++index)
      {
        char character = text[index];
        switch (character)
        {
          case '\n':
            zero.X = num;
            zero.Y += (float) this.LineSpacing * scale.Y * one.Y;
            flag = true;
            continue;
          case '\r':
            continue;
          default:
            DynamicSpriteFont.SpriteCharacterData characterData = this.GetCharacterData(character);
            Vector3 kerning = characterData.Kerning;
            Rectangle padding = characterData.Padding;
            if (spriteEffects.HasFlag((Enum) SpriteEffects.FlipHorizontally))
              padding.X -= padding.Width;
            if (spriteEffects.HasFlag((Enum) SpriteEffects.FlipVertically))
              padding.Y = this.LineSpacing - characterData.Glyph.Height - padding.Y;
            if (flag)
              kerning.X = Math.Max(kerning.X, 0.0f);
            else
              zero.X += this.CharacterSpacing * scale.X * one.X;
            zero.X += kerning.X * scale.X * one.X;
            Vector2 result = zero;
            result.X += (float) padding.X * scale.X;
            result.Y += (float) padding.Y * scale.Y;
            Vector2.Transform(ref result, ref matrix, out result);
            Vector2 position = result + startPosition;
            spriteBatch.Draw(characterData.Texture, position, new Rectangle?(characterData.Glyph), color, rotation, Vector2.Zero, scale, spriteEffects, depth);
            zero.X += (kerning.Y + kerning.Z) * scale.X * one.X;
            flag = false;
            continue;
        }
      }
    }

    private DynamicSpriteFont.SpriteCharacterData GetCharacterData(char character)
    {
      DynamicSpriteFont.SpriteCharacterData defaultCharacterData;
      if (!this._spriteCharacters.TryGetValue(character, out defaultCharacterData))
        defaultCharacterData = this._defaultCharacterData;
      return defaultCharacterData;
    }

    public Vector2 MeasureString(string text)
    {
      if (text.Length == 0)
        return Vector2.Zero;
      Vector2 zero = Vector2.Zero with
      {
        Y = (float) this.LineSpacing
      };
      float val2 = 0.0f;
      int num = 0;
      float val1 = 0.0f;
      bool flag = true;
      for (int index = 0; index < text.Length; ++index)
      {
        char character = text[index];
        switch (character)
        {
          case '\n':
            val2 = Math.Max(zero.X + Math.Max(val1, 0.0f), val2);
            val1 = 0.0f;
            zero = Vector2.Zero with
            {
              Y = (float) this.LineSpacing
            };
            flag = true;
            ++num;
            continue;
          case '\r':
            continue;
          default:
            DynamicSpriteFont.SpriteCharacterData characterData = this.GetCharacterData(character);
            Vector3 kerning = characterData.Kerning;
            if (flag)
              kerning.X = Math.Max(kerning.X, 0.0f);
            else
              zero.X += this.CharacterSpacing + val1;
            zero.X += kerning.X + kerning.Y;
            val1 = kerning.Z;
            zero.Y = Math.Max(zero.Y, (float) characterData.Padding.Height);
            flag = false;
            continue;
        }
      }
      zero.X += Math.Max(val1, 0.0f);
      zero.Y += (float) (num * this.LineSpacing);
      zero.X = Math.Max(zero.X, val2);
      return zero;
    }

    public string CreateWrappedText(string text, float maxWidth) => this.CreateWrappedText(text, maxWidth, Thread.CurrentThread.CurrentCulture);

    public string CreateWrappedText(string text, float maxWidth, CultureInfo culture)
    {
      WrappedTextBuilder wrappedTextBuilder = new WrappedTextBuilder((IFontMetrics) this, maxWidth, culture);
      wrappedTextBuilder.Append(text);
      return wrappedTextBuilder.ToString();
    }

    public GlyphMetrics GetCharacterMetrics(char character) => this.GetCharacterData(character).ToGlyphMetric();

    private struct SpriteCharacterData
    {
      public readonly Texture2D Texture;
      public readonly Rectangle Glyph;
      public readonly Rectangle Padding;
      public readonly Vector3 Kerning;

      public SpriteCharacterData(
        Texture2D texture,
        Rectangle glyph,
        Rectangle padding,
        Vector3 kerning)
      {
        this.Texture = texture;
        this.Glyph = glyph;
        this.Padding = padding;
        this.Kerning = kerning;
      }

      public GlyphMetrics ToGlyphMetric() => GlyphMetrics.FromKerningData(this.Kerning.X, this.Kerning.Y, this.Kerning.Z);
    }
  }
}
