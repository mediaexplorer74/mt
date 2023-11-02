// Decompiled with JetBrains decompiler
// Type: GameManager.Gore
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 09A1FDD7-68B3-40FF-A030-32890DF6B0E7
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager v1 Beta\GameManager.exe

using Microsoft.Xna.Framework;
using System;

namespace GameManager
{
  public class Gore
  {
    public static int goreTime = 600;
    public Vector2 position;
    public Vector2 velocity;
    public float rotation;
    public float scale;
    public int alpha;
    public int type;
    public float light;
    public bool active;
    public bool sticky = true;
    public int timeLeft = Gore.goreTime;

    public void Update()
    {
      if (!this.active)
        return;
      if (this.type == 11 || this.type == 12 || this.type == 13 || this.type == 61 || this.type == 62 || this.type == 63)
      {
        this.velocity.Y *= 0.98f;
        this.velocity.X *= 0.98f;
        this.scale -= 0.007f;
        if ((double) this.scale < 0.1)
        {
          this.scale = 0.1f;
          this.alpha = (int) byte.MaxValue;
        }
      }
      else if (this.type == 16 || this.type == 17)
      {
        this.velocity.Y *= 0.98f;
        this.velocity.X *= 0.98f;
        this.scale -= 0.01f;
        if ((double) this.scale < 0.1)
        {
          this.scale = 0.1f;
          this.alpha = (int) byte.MaxValue;
        }
      }
      else
        this.velocity.Y += 0.2f;
      this.rotation += this.velocity.X * 0.1f;
      if (this.sticky)
      {
        int num1 = Game1.goreTexture[this.type].Width;
        if (Game1.goreTexture[this.type].Height < num1)
          num1 = Game1.goreTexture[this.type].Height;
        int num2 = (int) ((double) num1 * 0.89999997615814209);
        this.velocity = Collision.TileCollision(this.position, this.velocity, (int) ((double) num2 * (double) this.scale), (int) ((double) num2 * (double) this.scale));
        if ((double) this.velocity.Y == 0.0)
        {
          this.velocity.X *= 0.97f;
          if ((double) this.velocity.X > -0.01 && (double) this.velocity.X < 0.01)
            this.velocity.X = 0.0f;
        }
        if (this.timeLeft > 0)
          --this.timeLeft;
        else
          ++this.alpha;
      }
      else
        this.alpha += 2;
      this.position += this.velocity;
      if (this.alpha >= (int) byte.MaxValue)
        this.active = false;
      if ((double) this.light > 0.0)
        Lighting.addLight((int) (((double) this.position.X + (double) Game1.goreTexture[this.type].Width * (double) this.scale / 2.0) / 16.0), (int) (((double) this.position.Y + (double) Game1.goreTexture[this.type].Height * (double) this.scale / 2.0) / 16.0), this.light);
    }

    public static int NewGore(Vector2 Position, Vector2 Velocity, int Type)
    {
      if (Game1.rand == null)
        Game1.rand = new Random();
      if (Game1.netMode == 2)
        return 0;
      int index1 = 200;
      for (int index2 = 0; index2 < 200; ++index2)
      {
        if (!Game1.gore[index2].active)
        {
          index1 = index2;
          break;
        }
      }
      if (index1 == 200)
        return index1;
      Game1.gore[index1].light = 0.0f;
      Game1.gore[index1].position = Position;
      Game1.gore[index1].velocity = Velocity;
      Game1.gore[index1].velocity.Y -= (float) Game1.rand.Next(10, 31) * 0.1f;
      Game1.gore[index1].velocity.X += (float) Game1.rand.Next(-20, 21) * 0.1f;
      Game1.gore[index1].type = Type;
      Game1.gore[index1].active = true;
      Game1.gore[index1].alpha = 0;
      Game1.gore[index1].rotation = 0.0f;
      Game1.gore[index1].scale = 1f;
      if (Gore.goreTime == 0 || Type == 11 || Type == 12 || Type == 13 || Type == 16 || Type == 17 || Type == 61 || Type == 62 || Type == 63)
      {
        Game1.gore[index1].sticky = false;
      }
      else
      {
        Game1.gore[index1].sticky = true;
        Game1.gore[index1].timeLeft = Gore.goreTime;
      }
      if (Type == 16 || Type == 17)
      {
        Game1.gore[index1].alpha = 100;
        Game1.gore[index1].scale = 0.7f;
        Game1.gore[index1].light = 1f;
      }
      return index1;
    }

    public Color GetAlpha(Color newColor)
    {
      int r;
      int g;
      int b;
      if (this.type == 16 || this.type == 17)
      {
        r = (int) newColor.R - this.alpha / 2;
        g = (int) newColor.G - this.alpha / 2;
        b = (int) newColor.B - this.alpha / 2;
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
  }
}
