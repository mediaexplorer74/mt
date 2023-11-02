// Decompiled with JetBrains decompiler
// Type: GameManager.Dust
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 09A1FDD7-68B3-40FF-A030-32890DF6B0E7
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager v1 Beta\GameManager.exe

using Microsoft.Xna.Framework;

namespace GameManager
{
  public class Dust
  {
    public Vector2 position;
    public Vector2 velocity;
    public bool noGravity = false;
    public float scale;
    public float rotation;
    public bool noLight = false;
    public bool active = false;
    public int type = 0;
    public Color color;
    public int alpha;
    public Rectangle frame;

    public static int NewDust(
      Vector2 Position,
      int Width,
      int Height,
      int Type,
      float SpeedX = 0.0f,
      float SpeedY = 0.0f,
      int Alpha = 0,
      Color newColor = default (Color),
      float Scale = 1f)
    {
      if (WorldGen.gen || Game1.netMode == 2)
        return 0;
      int num = 0;
      for (int index = 0; index < 2000; ++index)
      {
        if (!Game1.dust[index].active)
        {
          num = index;
          Game1.dust[index].active = true;
          Game1.dust[index].type = Type;
          Game1.dust[index].noGravity = false;
          Game1.dust[index].color = newColor;
          Game1.dust[index].alpha = Alpha;
          Game1.dust[index].position.X = (float) ((double) Position.X + (double) Game1.rand.Next(Width - 4) + 4.0);
          Game1.dust[index].position.Y = (float) ((double) Position.Y + (double) Game1.rand.Next(Height - 4) + 4.0);
          Game1.dust[index].velocity.X = (float) Game1.rand.Next(-20, 21) * 0.1f + SpeedX;
          Game1.dust[index].velocity.Y = (float) Game1.rand.Next(-20, 21) * 0.1f + SpeedY;
          Game1.dust[index].frame.X = 10 * Type;
          Game1.dust[index].frame.Y = 10 * Game1.rand.Next(3);
          Game1.dust[index].frame.Width = 8;
          Game1.dust[index].frame.Height = 8;
          Game1.dust[index].rotation = 0.0f;
          Game1.dust[index].scale = (float) (1.0 + (double) Game1.rand.Next(-20, 21) * 0.0099999997764825821);
          Game1.dust[index].scale *= Scale;
          Game1.dust[index].noLight = false;
          if (Game1.dust[index].type == 6 || Game1.dust[index].type == 29)
          {
            Game1.dust[index].velocity.Y = (float) Game1.rand.Next(-10, 6) * 0.1f;
            Game1.dust[index].velocity.X *= 0.3f;
            Game1.dust[index].scale *= 0.7f;
          }
          if (Game1.dust[index].type == 33)
          {
            Game1.dust[index].alpha = 170;
            Game1.dust[index].velocity *= 0.5f;
            ++Game1.dust[index].velocity.Y;
          }
          if (Game1.dust[index].type == 41)
            Game1.dust[index].velocity *= 0.0f;
          if (Game1.dust[index].type == 34 || Game1.dust[index].type == 35)
          {
            Game1.dust[index].velocity *= 0.1f;
            Game1.dust[index].velocity.Y = -0.5f;
            if (Game1.dust[index].type == 34 && !Collision.WetCollision(new Vector2(Game1.dust[index].position.X, Game1.dust[index].position.Y - 8f), 4, 4))
              Game1.dust[index].active = false;
            break;
          }
          break;
        }
      }
      return num;
    }

    public static void UpdateDust()
    {
      for (int index = 0; index < 2000; ++index)
      {
        if (Game1.dust[index].active)
        {
          Game1.dust[index].position += Game1.dust[index].velocity;
          if (Game1.dust[index].type == 6 || Game1.dust[index].type == 29)
          {
            if (!Game1.dust[index].noGravity)
              Game1.dust[index].velocity.Y += 0.05f;
            if (!Game1.dust[index].noLight)
            {
              float Lightness = Game1.dust[index].scale * 1.6f;
              if (Game1.dust[index].type == 29)
                Lightness *= 0.3f;
              if ((double) Lightness > 1.0)
                Lightness = 1f;
              Lighting.addLight((int) ((double) Game1.dust[index].position.X / 16.0), (int) ((double) Game1.dust[index].position.Y / 16.0), Lightness);
            }
          }
          else if (Game1.dust[index].type == 14 || Game1.dust[index].type == 16 || Game1.dust[index].type == 31)
          {
            Game1.dust[index].velocity.Y *= 0.98f;
            Game1.dust[index].velocity.X *= 0.98f;
          }
          else if (Game1.dust[index].type == 32)
          {
            Game1.dust[index].scale -= 0.01f;
            Game1.dust[index].velocity.X *= 0.96f;
            Game1.dust[index].velocity.Y += 0.1f;
          }
          else if (Game1.dust[index].type == 15)
          {
            Game1.dust[index].velocity.Y *= 0.98f;
            Game1.dust[index].velocity.X *= 0.98f;
            float Lightness = Game1.dust[index].scale;
            if ((double) Lightness > 1.0)
              Lightness = 1f;
            Lighting.addLight((int) ((double) Game1.dust[index].position.X / 16.0), (int) ((double) Game1.dust[index].position.Y / 16.0), Lightness);
          }
          else if (Game1.dust[index].type == 20 || Game1.dust[index].type == 21)
          {
            Game1.dust[index].scale += 0.005f;
            Game1.dust[index].velocity.Y *= 0.94f;
            Game1.dust[index].velocity.X *= 0.94f;
            float Lightness = Game1.dust[index].scale * 0.8f;
            if (Game1.dust[index].type == 21)
              Lightness = Game1.dust[index].scale * 0.4f;
            if ((double) Lightness > 1.0)
              Lightness = 1f;
            Lighting.addLight((int) ((double) Game1.dust[index].position.X / 16.0), (int) ((double) Game1.dust[index].position.Y / 16.0), Lightness);
          }
          else if (Game1.dust[index].type == 27)
          {
            Game1.dust[index].velocity *= 0.94f;
            Game1.dust[index].scale += 1f / 500f;
            float Lightness = Game1.dust[index].scale;
            if ((double) Lightness > 1.0)
              Lightness = 1f;
            Lighting.addLight((int) ((double) Game1.dust[index].position.X / 16.0), (int) ((double) Game1.dust[index].position.Y / 16.0), Lightness);
          }
          else if (!Game1.dust[index].noGravity && Game1.dust[index].type != 41)
            Game1.dust[index].velocity.Y += 0.1f;
          if (Game1.dust[index].type == 5 && Game1.dust[index].noGravity)
            Game1.dust[index].scale -= 0.04f;
          if (Game1.dust[index].type == 33)
          {
            if (Collision.WetCollision(new Vector2(Game1.dust[index].position.X, Game1.dust[index].position.Y), 4, 4))
            {
              Game1.dust[index].alpha += 20;
              Game1.dust[index].scale -= 0.1f;
            }
            Game1.dust[index].alpha += 2;
            Game1.dust[index].scale -= 0.005f;
            if (Game1.dust[index].alpha > (int) byte.MaxValue)
              Game1.dust[index].scale = 0.0f;
            Game1.dust[index].velocity.X *= 0.93f;
            if ((double) Game1.dust[index].velocity.Y > 4.0)
              Game1.dust[index].velocity.Y = 4f;
            if (Game1.dust[index].noGravity)
            {
              if ((double) Game1.dust[index].velocity.X < 0.0)
                Game1.dust[index].rotation -= 0.2f;
              else
                Game1.dust[index].rotation += 0.2f;
              Game1.dust[index].scale += 0.03f;
              Game1.dust[index].velocity.X *= 1.05f;
              Game1.dust[index].velocity.Y += 0.15f;
            }
          }
          if (Game1.dust[index].type == 35 && Game1.dust[index].noGravity)
          {
            Game1.dust[index].scale += 0.02f;
            if ((double) Game1.dust[index].scale < 1.0)
              Game1.dust[index].velocity.Y += 0.075f;
            Game1.dust[index].velocity.X *= 1.08f;
            if ((double) Game1.dust[index].velocity.X > 0.0)
              Game1.dust[index].rotation += 0.01f;
            else
              Game1.dust[index].rotation -= 0.01f;
          }
          else if (Game1.dust[index].type == 34 || Game1.dust[index].type == 35)
          {
            if (!Collision.WetCollision(new Vector2(Game1.dust[index].position.X, Game1.dust[index].position.Y - 8f), 4, 4))
            {
              Game1.dust[index].scale = 0.0f;
            }
            else
            {
              Game1.dust[index].alpha += Game1.rand.Next(2);
              if (Game1.dust[index].alpha > (int) byte.MaxValue)
                Game1.dust[index].scale = 0.0f;
              Game1.dust[index].velocity.Y = -0.5f;
              if (Game1.dust[index].type == 34)
              {
                Game1.dust[index].scale += 0.005f;
              }
              else
              {
                ++Game1.dust[index].alpha;
                Game1.dust[index].scale -= 0.01f;
                Game1.dust[index].velocity.Y = -0.2f;
              }
              Game1.dust[index].velocity.X += (float) Game1.rand.Next(-10, 10) * (1f / 500f);
              if ((double) Game1.dust[index].velocity.X < -0.25)
                Game1.dust[index].velocity.X = -0.25f;
              if ((double) Game1.dust[index].velocity.X > 0.25)
                Game1.dust[index].velocity.X = 0.25f;
            }
            if (Game1.dust[index].type == 35)
            {
              float Lightness = Game1.dust[index].scale * 1.6f;
              if ((double) Lightness > 1.0)
                Lightness = 1f;
              Lighting.addLight((int) ((double) Game1.dust[index].position.X / 16.0), (int) ((double) Game1.dust[index].position.Y / 16.0), Lightness);
            }
          }
          if (Game1.dust[index].type == 41)
          {
            Game1.dust[index].velocity.X += (float) Game1.rand.Next(-10, 11) * 0.01f;
            Game1.dust[index].velocity.Y += (float) Game1.rand.Next(-10, 11) * 0.01f;
            if ((double) Game1.dust[index].velocity.X > 0.75)
              Game1.dust[index].velocity.X = 0.75f;
            if ((double) Game1.dust[index].velocity.X < -0.75)
              Game1.dust[index].velocity.X = -0.75f;
            if ((double) Game1.dust[index].velocity.Y > 0.75)
              Game1.dust[index].velocity.Y = 0.75f;
            if ((double) Game1.dust[index].velocity.Y < -0.75)
              Game1.dust[index].velocity.Y = -0.75f;
            Game1.dust[index].scale += 0.007f;
            float Lightness = Game1.dust[index].scale * 0.7f;
            if ((double) Lightness > 1.0)
              Lightness = 1f;
            Lighting.addLight((int) ((double) Game1.dust[index].position.X / 16.0), (int) ((double) Game1.dust[index].position.Y / 16.0), Lightness);
          }
          else
            Game1.dust[index].velocity.X *= 0.99f;
          Game1.dust[index].rotation += Game1.dust[index].velocity.X * 0.5f;
          Game1.dust[index].scale -= 0.01f;
          if (Game1.dust[index].noGravity)
          {
            Game1.dust[index].velocity *= 0.92f;
            Game1.dust[index].scale -= 0.04f;
          }
          if ((double) Game1.dust[index].position.Y > (double) Game1.screenPosition.Y + (double) Game1.screenHeight)
            Game1.dust[index].active = false;
          if ((double) Game1.dust[index].scale < 0.1)
            Game1.dust[index].active = false;
        }
      }
    }

    public Color GetAlpha(Color newColor)
    {
      int r;
      int g;
      int b;
      if (this.type == 15 || this.type == 20 || this.type == 21 || this.type == 29 || this.type == 35 || this.type == 41)
      {
        r = (int) newColor.R - this.alpha / 3;
        g = (int) newColor.G - this.alpha / 3;
        b = (int) newColor.B - this.alpha / 3;
      }
      else
      {
        r = (int) newColor.R - this.alpha;
        g = (int) newColor.G - this.alpha;
        b = (int) newColor.B - this.alpha;
      }
      int a = (int) newColor.A - this.alpha;
      if (a < 0)
        a = 0;
      if (a > (int) byte.MaxValue)
        a = (int) byte.MaxValue;
      return new Color(r, g, b, a);
    }

    public Color GetColor(Color newColor)
    {
      int r = (int) this.color.R - ((int) byte.MaxValue - (int) newColor.R);
      int g = (int) this.color.G - ((int) byte.MaxValue - (int) newColor.G);
      int b = (int) this.color.B - ((int) byte.MaxValue - (int) newColor.B);
      int a = (int) this.color.A - ((int) byte.MaxValue - (int) newColor.A);
      if (r < 0)
        r = 0;
      if (r > (int) byte.MaxValue)
        r = (int) byte.MaxValue;
      if (g < 0)
        g = 0;
      if (g > (int) byte.MaxValue)
        g = (int) byte.MaxValue;
      if (b < 0)
        b = 0;
      if (b > (int) byte.MaxValue)
        b = (int) byte.MaxValue;
      if (a < 0)
        a = 0;
      if (a > (int) byte.MaxValue)
        a = (int) byte.MaxValue;
      return new Color(r, g, b, a);
    }
  }
}
