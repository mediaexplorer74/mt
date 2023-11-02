
using Microsoft.Xna.Framework;

namespace GameManager
{
  public class Dust
  {
    public Vector2 position;
    public Vector2 velocity;
    public float scale;
    public float rotation;
    public bool active = false;
    public int type = 0;
    public Color color;
    public int alpha;
    public Rectangle frame;

    public static void NewDust(
      Vector2 Position,
      int Width,
      int Height,
      int Type,
      float SpeedX = 0.0f,
      float SpeedY = 0.0f,
      int Alpha = 0,
      Color newColor = default (Color))
    {
      for (int index = 0; index < 1000; ++index)
      {
        if (!Game1.dust[index].active)
        {
          Game1.dust[index].active = true;
          Game1.dust[index].type = Type;
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
          if (Game1.dust[index].type != 6)
            break;
          Game1.dust[index].velocity.Y = (float) Game1.rand.Next(-10, 6) * 0.1f;
          Game1.dust[index].velocity.X *= 0.3f;
          Game1.dust[index].scale *= 0.7f;
          break;
        }
      }
    }

    public static void UpdateDust()
    {
      for (int index = 0; index < 1000; ++index)
      {
        if (Game1.dust[index].active)
        {
          Game1.dust[index].position += Game1.dust[index].velocity;
          if (Game1.dust[index].type == 6)
            Game1.dust[index].velocity.Y += 0.05f;
          else
            Game1.dust[index].velocity.Y += 0.1f;
          Game1.dust[index].velocity.X *= 0.99f;
          Game1.dust[index].rotation += Game1.dust[index].velocity.X * 0.5f;
          Game1.dust[index].scale -= 0.01f;

          if ((double) Game1.dust[index].position.Y > 
                        (double) Game1.screenPosition.Y + (double) Game1.screenHeight)
            Game1.dust[index].active = false;

          if ((double) Game1.dust[index].scale < 0.1)
            Game1.dust[index].active = false;
        }
      }
    }

    public Color GetAlpha(Color newColor)
    {
      int r = (int) newColor.R - this.alpha;
      int g = (int) newColor.G - this.alpha;
      int b = (int) newColor.B - this.alpha;
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
