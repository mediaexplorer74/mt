// Decompiled with JetBrains decompiler
// Type: ReLogic.Graphics.DynamicSpriteFontExtensionMethods
// Assembly: ReLogic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 33513C57-D94A-4BED-935B-7012D40A5531
// Assembly location: C:\Users\Admin\Desktop\re\ReLogic.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace ReLogic.Graphics
{
  public static class DynamicSpriteFontExtensionMethods
  {
    public static void DrawString(
      this SpriteBatch spriteBatch,
      DynamicSpriteFont spriteFont,
      string text,
      Vector2 position,
      Color color)
    {
      Vector2 one = Vector2.One;
      spriteFont.InternalDraw(text, spriteBatch, position, color, 0.0f, Vector2.Zero, ref one, SpriteEffects.None, 0.0f);
    }

    public static void DrawString(
      this SpriteBatch spriteBatch,
      DynamicSpriteFont spriteFont,
      StringBuilder text,
      Vector2 position,
      Color color)
    {
      Vector2 one = Vector2.One;
      spriteFont.InternalDraw(text.ToString(), spriteBatch, position, color, 0.0f, Vector2.Zero, ref one, SpriteEffects.None, 0.0f);
    }

    public static void DrawString(
      this SpriteBatch spriteBatch,
      DynamicSpriteFont spriteFont,
      string text,
      Vector2 position,
      Color color,
      float rotation,
      Vector2 origin,
      float scale,
      SpriteEffects effects,
      float layerDepth)
    {
      spriteFont.InternalDraw(text, spriteBatch, position, color, rotation, origin, ref new Vector2()
      {
        X = scale,
        Y = scale
      }, effects, layerDepth);
    }

    public static void DrawString(
      this SpriteBatch spriteBatch,
      DynamicSpriteFont spriteFont,
      StringBuilder text,
      Vector2 position,
      Color color,
      float rotation,
      Vector2 origin,
      float scale,
      SpriteEffects effects,
      float layerDepth)
    {
      Vector2 scale1 = new Vector2(scale);
      spriteFont.InternalDraw(text.ToString(), spriteBatch, position, color, rotation, origin, ref scale1, effects, layerDepth);
    }

    public static void DrawString(
      this SpriteBatch spriteBatch,
      DynamicSpriteFont spriteFont,
      string text,
      Vector2 position,
      Color color,
      float rotation,
      Vector2 origin,
      Vector2 scale,
      SpriteEffects effects,
      float layerDepth)
    {
      spriteFont.InternalDraw(text, spriteBatch, position, color, rotation, origin, ref scale, effects, layerDepth);
    }

    public static void DrawString(
      this SpriteBatch spriteBatch,
      DynamicSpriteFont spriteFont,
      StringBuilder text,
      Vector2 position,
      Color color,
      float rotation,
      Vector2 origin,
      Vector2 scale,
      SpriteEffects effects,
      float layerDepth)
    {
      spriteFont.InternalDraw(text.ToString(), spriteBatch, position, color, rotation, origin, ref scale, effects, layerDepth);
    }
  }
}
