﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Skies.SlimeSky
// Assembly: Terraria, Version=1.3.5.3, Culture=neutral, PublicKeyToken=null
// MVID: 68659D26-2BE6-448F-8663-74FA559E6F08
// Assembly location: H:\Steam\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Utilities;

namespace Terraria.GameContent.Skies
{
  public class SlimeSky : CustomSky
  {
    private UnifiedRandom _random = new UnifiedRandom();
    private Texture2D[] _textures;
    private SlimeSky.Slime[] _slimes;
    private int _slimesRemaining;
    private bool _isActive;
    private bool _isLeaving;

    public override void OnLoad()
    {
      this._textures = new Texture2D[4];
      for (int index = 0; index < 4; ++index)
        this._textures[index] = TextureManager.Load("Images/Misc/Sky_Slime_" + (object) (index + 1));
      this.GenerateSlimes();
    }

    private void GenerateSlimes()
    {
      this._slimes = new SlimeSky.Slime[Main.maxTilesY / 6];
      for (int index = 0; index < this._slimes.Length; ++index)
      {
        int maxValue = (int) ((double) Main.screenPosition.Y * 0.7 - (double) Main.screenHeight);
        int minValue = (int) ((double) maxValue - Main.worldSurface * 16.0);
        this._slimes[index].Position = new Vector2((float) (this._random.Next(0, Main.maxTilesX) * 16), (float) this._random.Next(minValue, maxValue));
        this._slimes[index].Speed = (float) (5.0 + 3.0 * this._random.NextDouble());
        this._slimes[index].Depth = (float) ((double) index / (double) this._slimes.Length * 1.75 + 1.60000002384186);
        this._slimes[index].Texture = this._textures[this._random.Next(2)];
        if (this._random.Next(60) == 0)
        {
          this._slimes[index].Texture = this._textures[3];
          this._slimes[index].Speed = (float) (6.0 + 3.0 * this._random.NextDouble());
          this._slimes[index].Depth += 0.5f;
        }
        else if (this._random.Next(30) == 0)
        {
          this._slimes[index].Texture = this._textures[2];
          this._slimes[index].Speed = (float) (6.0 + 2.0 * this._random.NextDouble());
        }
        this._slimes[index].Active = true;
      }
      this._slimesRemaining = this._slimes.Length;
    }

    public override void Update(GameTime gameTime)
    {
      if (Main.gamePaused || !Main.hasFocus)
        return;
      for (int index = 0; index < this._slimes.Length; ++index)
      {
        if (this._slimes[index].Active)
        {
          ++this._slimes[index].Frame;
          // ISSUE: explicit reference operation
          // ISSUE: variable of a reference type
          __Null& local = @this._slimes[index].Position.Y;
          // ISSUE: cast to a reference type
          // ISSUE: explicit reference operation
          double num = (double) ^(float&) local + (double) this._slimes[index].Speed;
          // ISSUE: cast to a reference type
          // ISSUE: explicit reference operation
          ^(float&) local = (float) num;
          if ((double) this._slimes[index].Position.Y > Main.worldSurface * 16.0)
          {
            if (!this._isLeaving)
            {
              this._slimes[index].Depth = (float) ((double) index / (double) this._slimes.Length * 1.75 + 1.60000002384186);
              this._slimes[index].Position = new Vector2((float) (this._random.Next(0, Main.maxTilesX) * 16), -100f);
              this._slimes[index].Texture = this._textures[this._random.Next(2)];
              this._slimes[index].Speed = (float) (5.0 + 3.0 * this._random.NextDouble());
              if (this._random.Next(60) == 0)
              {
                this._slimes[index].Texture = this._textures[3];
                this._slimes[index].Speed = (float) (6.0 + 3.0 * this._random.NextDouble());
                this._slimes[index].Depth += 0.5f;
              }
              else if (this._random.Next(30) == 0)
              {
                this._slimes[index].Texture = this._textures[2];
                this._slimes[index].Speed = (float) (6.0 + 2.0 * this._random.NextDouble());
              }
            }
            else
            {
              this._slimes[index].Active = false;
              this._slimesRemaining = this._slimesRemaining - 1;
            }
          }
        }
      }
      if (this._slimesRemaining != 0)
        return;
      this._isActive = false;
    }

    public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
    {
      if (Main.screenPosition.Y > 10000.0 || Main.gameMenu)
        return;
      int num1 = -1;
      int num2 = 0;
      for (int index = 0; index < this._slimes.Length; ++index)
      {
        float depth = this._slimes[index].Depth;
        if (num1 == -1 && (double) depth < (double) maxDepth)
          num1 = index;
        if ((double) depth > (double) minDepth)
          num2 = index;
        else
          break;
      }
      if (num1 == -1)
        return;
      Vector2 vector2_1 = Vector2.op_Addition(Main.screenPosition, new Vector2((float) (Main.screenWidth >> 1), (float) (Main.screenHeight >> 1)));
      Rectangle rectangle;
      // ISSUE: explicit reference operation
      ((Rectangle) @rectangle).\u002Ector(-1000, -1000, 4000, 4000);
      for (int index = num1; index < num2; ++index)
      {
        if (this._slimes[index].Active)
        {
          // ISSUE: explicit reference operation
          Color color = Color.op_Multiply(new Color(Vector4.op_Addition(Vector4.op_Multiply(((Color) @Main.bgColor).ToVector4(), 0.9f), new Vector4(0.1f))), 0.8f);
          float num3 = 1f;
          if ((double) this._slimes[index].Depth > 3.0)
            num3 = 0.6f;
          else if ((double) this._slimes[index].Depth > 2.5)
            num3 = 0.7f;
          else if ((double) this._slimes[index].Depth > 2.0)
            num3 = 0.8f;
          else if ((double) this._slimes[index].Depth > 1.5)
            num3 = 0.9f;
          float num4 = num3 * 0.8f;
          // ISSUE: explicit reference operation
          // ISSUE: explicit reference operation
          // ISSUE: explicit reference operation
          // ISSUE: explicit reference operation
          // ISSUE: explicit reference operation
          ((Color) @color).\u002Ector((int) ((double) ((Color) @color).get_R() * (double) num4), (int) ((double) ((Color) @color).get_G() * (double) num4), (int) ((double) ((Color) @color).get_B() * (double) num4), (int) ((double) ((Color) @color).get_A() * (double) num4));
          Vector2 vector2_2;
          // ISSUE: explicit reference operation
          ((Vector2) @vector2_2).\u002Ector(1f / this._slimes[index].Depth, 0.9f / this._slimes[index].Depth);
          Vector2 vector2_3 = Vector2.op_Subtraction(Vector2.op_Addition(Vector2.op_Multiply(Vector2.op_Subtraction(this._slimes[index].Position, vector2_1), vector2_2), vector2_1), Main.screenPosition);
          vector2_3.X = (__Null) ((vector2_3.X + 500.0) % 4000.0);
          if (vector2_3.X < 0.0)
          {
            // ISSUE: explicit reference operation
            // ISSUE: variable of a reference type
            __Null& local = @vector2_3.X;
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            double num5 = (double) ^(float&) local + 4000.0;
            // ISSUE: cast to a reference type
            // ISSUE: explicit reference operation
            ^(float&) local = (float) num5;
          }
          // ISSUE: explicit reference operation
          // ISSUE: variable of a reference type
          __Null& local1 = @vector2_3.X;
          // ISSUE: cast to a reference type
          // ISSUE: explicit reference operation
          double num6 = (double) ^(float&) local1 - 500.0;
          // ISSUE: cast to a reference type
          // ISSUE: explicit reference operation
          ^(float&) local1 = (float) num6;
          // ISSUE: explicit reference operation
          if (((Rectangle) @rectangle).Contains((int) vector2_3.X, (int) vector2_3.Y))
            spriteBatch.Draw(this._slimes[index].Texture, vector2_3, new Rectangle?(this._slimes[index].GetSourceRectangle()), color, 0.0f, Vector2.get_Zero(), (float) (vector2_2.X * 2.0), (SpriteEffects) 0, 0.0f);
        }
      }
    }

    internal override void Activate(Vector2 position, params object[] args)
    {
      this.GenerateSlimes();
      this._isActive = true;
      this._isLeaving = false;
    }

    internal override void Deactivate(params object[] args)
    {
      this._isLeaving = true;
    }

    public override void Reset()
    {
      this._isActive = false;
    }

    public override bool IsActive()
    {
      return this._isActive;
    }

    private struct Slime
    {
      private const int MAX_FRAMES = 4;
      private const int FRAME_RATE = 6;
      private Texture2D _texture;
      public Vector2 Position;
      public float Depth;
      public int FrameHeight;
      public int FrameWidth;
      public float Speed;
      public bool Active;
      private int _frame;

      public Texture2D Texture
      {
        get
        {
          return this._texture;
        }
        set
        {
          this._texture = value;
          this.FrameWidth = value.get_Width();
          this.FrameHeight = value.get_Height() / 4;
        }
      }

      public int Frame
      {
        get
        {
          return this._frame;
        }
        set
        {
          this._frame = value % 24;
        }
      }

      public Rectangle GetSourceRectangle()
      {
        return new Rectangle(0, this._frame / 6 * this.FrameHeight, this.FrameWidth, this.FrameHeight);
      }
    }
  }
}
