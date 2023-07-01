﻿// Decompiled with JetBrains decompiler
// Type: Terraria.DelegateMethods
// Assembly: Terraria, Version=1.3.5.3, Culture=neutral, PublicKeyToken=null
// MVID: 68659D26-2BE6-448F-8663-74FA559E6F08
// Assembly location: H:\Steam\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;

namespace Terraria
{
  public static class DelegateMethods
  {
    public static Vector3 v3_1 = Vector3.get_Zero();
    public static float f_1 = 0.0f;
    public static Color c_1 = Color.get_Transparent();
    public static int i_1 = 0;
    public static TileCuttingContext tilecut_0 = TileCuttingContext.Unknown;

    public static Color ColorLerp_BlackToWhite(float percent)
    {
      return Color.Lerp(Color.get_Black(), Color.get_White(), percent);
    }

    public static Color ColorLerp_HSL_H(float percent)
    {
      return Main.hslToRgb(percent, 1f, 0.5f);
    }

    public static Color ColorLerp_HSL_S(float percent)
    {
      return Main.hslToRgb((float) DelegateMethods.v3_1.X, percent, (float) DelegateMethods.v3_1.Z);
    }

    public static Color ColorLerp_HSL_L(float percent)
    {
      return Main.hslToRgb((float) DelegateMethods.v3_1.X, (float) DelegateMethods.v3_1.Y, (float) (0.150000005960464 + 0.850000023841858 * (double) percent));
    }

    public static Color ColorLerp_HSL_O(float percent)
    {
      return Color.Lerp(Color.get_White(), Main.hslToRgb((float) DelegateMethods.v3_1.X, (float) DelegateMethods.v3_1.Y, (float) DelegateMethods.v3_1.Z), percent);
    }

    public static bool TestDust(int x, int y)
    {
      if (x < 0 || x >= Main.maxTilesX || (y < 0 || y >= Main.maxTilesY))
        return false;
      int index = Dust.NewDust(Vector2.op_Addition(Vector2.op_Multiply(new Vector2((float) x, (float) y), 16f), new Vector2(8f)), 0, 0, 6, 0.0f, 0.0f, 0, (Color) null, 1f);
      Main.dust[index].noGravity = true;
      Main.dust[index].noLight = true;
      return true;
    }

    public static bool CastLight(int x, int y)
    {
      if (x < 0 || x >= Main.maxTilesX || (y < 0 || y >= Main.maxTilesY) || Main.tile[x, y] == null)
        return false;
      Lighting.AddLight(x, y, (float) DelegateMethods.v3_1.X, (float) DelegateMethods.v3_1.Y, (float) DelegateMethods.v3_1.Z);
      return true;
    }

    public static bool CastLightOpen(int x, int y)
    {
      if (x < 0 || x >= Main.maxTilesX || (y < 0 || y >= Main.maxTilesY) || Main.tile[x, y] == null)
        return false;
      if (!Main.tile[x, y].active() || Main.tile[x, y].inActive() || (Main.tileSolidTop[(int) Main.tile[x, y].type] || !Main.tileSolid[(int) Main.tile[x, y].type]))
        Lighting.AddLight(x, y, (float) DelegateMethods.v3_1.X, (float) DelegateMethods.v3_1.Y, (float) DelegateMethods.v3_1.Z);
      return true;
    }

    public static bool NotDoorStand(int x, int y)
    {
      if (Main.tile[x, y] == null || !Main.tile[x, y].active() || (int) Main.tile[x, y].type != 11)
        return true;
      if ((int) Main.tile[x, y].frameX >= 18)
        return (int) Main.tile[x, y].frameX < 54;
      return false;
    }

    public static bool CutTiles(int x, int y)
    {
      if (!WorldGen.InWorld(x, y, 1) || Main.tile[x, y] == null)
        return false;
      if (!Main.tileCut[(int) Main.tile[x, y].type] || !WorldGen.CanCutTile(x, y, DelegateMethods.tilecut_0))
        return true;
      WorldGen.KillTile(x, y, false, false, false);
      if (Main.netMode != 0)
        NetMessage.SendData(17, -1, -1, (NetworkText) null, 0, (float) x, (float) y, 0.0f, 0, 0, 0);
      return true;
    }

    public static bool SearchAvoidedByNPCs(int x, int y)
    {
      return WorldGen.InWorld(x, y, 1) && Main.tile[x, y] != null && (!Main.tile[x, y].active() || !TileID.Sets.AvoidedByNPCs[(int) Main.tile[x, y].type]);
    }

    public static void RainbowLaserDraw(int stage, Vector2 currentPosition, float distanceLeft, Rectangle lastFrame, out float distCovered, out Rectangle frame, out Vector2 origin, out Color color)
    {
      color = DelegateMethods.c_1;
      if (stage == 0)
      {
        distCovered = 33f;
        frame = new Rectangle(0, 0, 26, 22);
        origin = Vector2.op_Division(frame.Size(), 2f);
      }
      else if (stage == 1)
      {
        frame = new Rectangle(0, 25, 26, 28);
        distCovered = (float) frame.Height;
        origin = new Vector2((float) (frame.Width / 2), 0.0f);
      }
      else if (stage == 2)
      {
        distCovered = 22f;
        frame = new Rectangle(0, 56, 26, 22);
        origin = new Vector2((float) (frame.Width / 2), 1f);
      }
      else
      {
        distCovered = 9999f;
        frame = Rectangle.get_Empty();
        origin = Vector2.get_Zero();
        color = Color.get_Transparent();
      }
    }

    public static void TurretLaserDraw(int stage, Vector2 currentPosition, float distanceLeft, Rectangle lastFrame, out float distCovered, out Rectangle frame, out Vector2 origin, out Color color)
    {
      color = DelegateMethods.c_1;
      if (stage == 0)
      {
        distCovered = 32f;
        frame = new Rectangle(0, 0, 22, 20);
        origin = Vector2.op_Division(frame.Size(), 2f);
      }
      else if (stage == 1)
      {
        ++DelegateMethods.i_1;
        int num = DelegateMethods.i_1 % 5;
        frame = new Rectangle(0, 22 * (num + 1), 22, 20);
        distCovered = (float) (frame.Height - 1);
        origin = new Vector2((float) (frame.Width / 2), 0.0f);
      }
      else if (stage == 2)
      {
        frame = new Rectangle(0, 154, 22, 30);
        distCovered = (float) frame.Height;
        origin = new Vector2((float) (frame.Width / 2), 1f);
      }
      else
      {
        distCovered = 9999f;
        frame = Rectangle.get_Empty();
        origin = Vector2.get_Zero();
        color = Color.get_Transparent();
      }
    }

    public static void LightningLaserDraw(int stage, Vector2 currentPosition, float distanceLeft, Rectangle lastFrame, out float distCovered, out Rectangle frame, out Vector2 origin, out Color color)
    {
      color = Color.op_Multiply(DelegateMethods.c_1, DelegateMethods.f_1);
      if (stage == 0)
      {
        distCovered = 0.0f;
        frame = new Rectangle(0, 0, 21, 8);
        origin = Vector2.op_Division(frame.Size(), 2f);
      }
      else if (stage == 1)
      {
        frame = new Rectangle(0, 8, 21, 6);
        distCovered = (float) frame.Height;
        origin = new Vector2((float) (frame.Width / 2), 0.0f);
      }
      else if (stage == 2)
      {
        distCovered = 8f;
        frame = new Rectangle(0, 14, 21, 8);
        origin = new Vector2((float) (frame.Width / 2), 2f);
      }
      else
      {
        distCovered = 9999f;
        frame = Rectangle.get_Empty();
        origin = Vector2.get_Zero();
        color = Color.get_Transparent();
      }
    }

    public static int CompareYReverse(Point a, Point b)
    {
      // ISSUE: explicit non-virtual call
      return __nonvirtual (b.Y.CompareTo((int) a.Y));
    }

    public static int CompareDrawSorterByYScale(DrawData a, DrawData b)
    {
      // ISSUE: explicit non-virtual call
      return __nonvirtual (a.scale.Y.CompareTo((float) b.scale.Y));
    }

    public static class Minecart
    {
      public static Vector2 rotationOrigin;
      public static float rotation;

      public static void Sparks(Vector2 dustPosition)
      {
        dustPosition = Vector2.op_Addition(dustPosition, new Vector2(Main.rand.Next(2) == 0 ? 13f : -13f, 0.0f).RotatedBy((double) DelegateMethods.Minecart.rotation, (Vector2) null));
        int index = Dust.NewDust(dustPosition, 1, 1, 213, (float) Main.rand.Next(-2, 3), (float) Main.rand.Next(-2, 3), 0, (Color) null, 1f);
        Main.dust[index].noGravity = true;
        Main.dust[index].fadeIn = (float) ((double) Main.dust[index].scale + 1.0 + 0.00999999977648258 * (double) Main.rand.Next(0, 51));
        Main.dust[index].noGravity = true;
        Dust dust = Main.dust[index];
        Vector2 vector2 = Vector2.op_Multiply(dust.velocity, (float) Main.rand.Next(15, 51) * 0.01f);
        dust.velocity = vector2;
        // ISSUE: explicit reference operation
        // ISSUE: variable of a reference type
        __Null& local1 = @Main.dust[index].velocity.X;
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        double num1 = (double) ^(float&) local1 * ((double) Main.rand.Next(25, 101) * 0.00999999977648258);
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        ^(float&) local1 = (float) num1;
        // ISSUE: explicit reference operation
        // ISSUE: variable of a reference type
        __Null& local2 = @Main.dust[index].velocity.Y;
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        double num2 = (double) ^(float&) local2 - (double) Main.rand.Next(15, 31) * 0.100000001490116;
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        ^(float&) local2 = (float) num2;
        // ISSUE: explicit reference operation
        // ISSUE: variable of a reference type
        __Null& local3 = @Main.dust[index].position.Y;
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        double num3 = (double) ^(float&) local3 - 4.0;
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        ^(float&) local3 = (float) num3;
        if (Main.rand.Next(3) != 0)
          Main.dust[index].noGravity = false;
        else
          Main.dust[index].scale *= 0.6f;
      }

      public static void SparksMech(Vector2 dustPosition)
      {
        dustPosition = Vector2.op_Addition(dustPosition, new Vector2(Main.rand.Next(2) == 0 ? 13f : -13f, 0.0f).RotatedBy((double) DelegateMethods.Minecart.rotation, (Vector2) null));
        int index = Dust.NewDust(dustPosition, 1, 1, 260, (float) Main.rand.Next(-2, 3), (float) Main.rand.Next(-2, 3), 0, (Color) null, 1f);
        Main.dust[index].noGravity = true;
        Main.dust[index].fadeIn = (float) ((double) Main.dust[index].scale + 0.5 + 0.00999999977648258 * (double) Main.rand.Next(0, 51));
        Main.dust[index].noGravity = true;
        Dust dust = Main.dust[index];
        Vector2 vector2 = Vector2.op_Multiply(dust.velocity, (float) Main.rand.Next(15, 51) * 0.01f);
        dust.velocity = vector2;
        // ISSUE: explicit reference operation
        // ISSUE: variable of a reference type
        __Null& local1 = @Main.dust[index].velocity.X;
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        double num1 = (double) ^(float&) local1 * ((double) Main.rand.Next(25, 101) * 0.00999999977648258);
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        ^(float&) local1 = (float) num1;
        // ISSUE: explicit reference operation
        // ISSUE: variable of a reference type
        __Null& local2 = @Main.dust[index].velocity.Y;
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        double num2 = (double) ^(float&) local2 - (double) Main.rand.Next(15, 31) * 0.100000001490116;
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        ^(float&) local2 = (float) num2;
        // ISSUE: explicit reference operation
        // ISSUE: variable of a reference type
        __Null& local3 = @Main.dust[index].position.Y;
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        double num3 = (double) ^(float&) local3 - 4.0;
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        ^(float&) local3 = (float) num3;
        if (Main.rand.Next(3) != 0)
          Main.dust[index].noGravity = false;
        else
          Main.dust[index].scale *= 0.6f;
      }
    }
  }
}
