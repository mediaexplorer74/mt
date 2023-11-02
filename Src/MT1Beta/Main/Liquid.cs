// Decompiled with JetBrains decompiler
// Type: GameManager.Liquid
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 09A1FDD7-68B3-40FF-A030-32890DF6B0E7
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager v1 Beta\GameManager.exe

using System;

namespace GameManager
{
  public class Liquid
  {
    public static int skipCount = 0;
    public static int stuckCount = 0;
    public static int stuckAmount = 0;
    public static int cycles = 10;
    public static int resLiquid = 5000;
    public static int maxLiquid = 5000;
    public static int numLiquid;
    public static bool stuck = false;
    public static bool quickFall = false;
    public static bool quickSettle = false;
    private static int wetCounter;
    public static int panicCounter = 0;
    public static bool panicMode = false;
    public static int panicY = 0;
    public int x;
    public int y;
    public int kill = 0;
    public int delay = 0;

    public static double QuickWater(int verbose = 0, int minY = -1, int maxY = -1)
    {
      int num1 = 0;
      if (minY == -1)
        minY = 3;
      if (maxY == -1)
        maxY = Game1.maxTilesY - 3;
      for (int index1 = maxY; index1 >= minY; --index1)
      {
        if (verbose > 0)
          Game1.statusText = "Settling liquids: " + (object) (int) ((double) ((float) (maxY - index1) / (float) (maxY - minY + 1) / (float) verbose) * 100.0 + 1.0) + "%";
        else if (verbose < 0)
          Game1.statusText = "Creating underworld: " + (object) (int) ((double) ((float) (maxY - index1) / (float) (maxY - minY + 1) / (float) -verbose) * 100.0 + 1.0) + "%";
        for (int index2 = 0; index2 < 2; ++index2)
        {
          int num2 = 2;
          int num3 = Game1.maxTilesX - 2;
          int num4 = 1;
          if (index2 == 1)
          {
            num2 = Game1.maxTilesX - 2;
            num3 = 2;
            num4 = -1;
          }
          for (int index3 = num2; index3 != num3; index3 += num4)
          {
            if (Game1.tile[index3, index1].liquid > (byte) 0)
            {
              int num5 = -num4;
              bool flag1 = false;
              int x = index3;
              int y = index1;
              bool flag2 = Game1.tile[index3, index1].lava;
              byte liquid = Game1.tile[index3, index1].liquid;
              Game1.tile[index3, index1].liquid = (byte) 0;
              bool flag3 = true;
              int num6 = 0;
              while (flag3 && x > 3 && x < Game1.maxTilesX - 3 && y < Game1.maxTilesY - 3)
              {
                flag3 = false;
                while (Game1.tile[x, y + 1].liquid == (byte) 0 && y < Game1.maxTilesY - 5 && (!Game1.tile[x, y + 1].active || !Game1.tileSolid[(int) Game1.tile[x, y + 1].type] || Game1.tileSolidTop[(int) Game1.tile[x, y + 1].type]))
                {
                  flag1 = true;
                  num5 = num4;
                  num6 = 0;
                  flag3 = true;
                  ++y;
                  if (y > WorldGen.waterLine)
                    flag2 = true;
                }
                if (Game1.tile[x, y + 1].liquid > (byte) 0 && Game1.tile[x, y + 1].liquid < byte.MaxValue && Game1.tile[x, y + 1].lava == flag2)
                {
                  int num7 = (int) byte.MaxValue - (int) Game1.tile[x, y + 1].liquid;
                  if (num7 > (int) liquid)
                    num7 = (int) liquid;
                  Game1.tile[x, y + 1].liquid += (byte) num7;
                  liquid -= (byte) num7;
                  if (liquid <= (byte) 0)
                  {
                    ++num1;
                    break;
                  }
                }
                if (num6 == 0)
                {
                  if (Game1.tile[x + num5, y].liquid == (byte) 0 && (!Game1.tile[x + num5, y].active || !Game1.tileSolid[(int) Game1.tile[x + num5, y].type] || Game1.tileSolidTop[(int) Game1.tile[x + num5, y].type]))
                    num6 = num5;
                  else if (Game1.tile[x - num5, y].liquid == (byte) 0 && (!Game1.tile[x - num5, y].active || !Game1.tileSolid[(int) Game1.tile[x - num5, y].type] || Game1.tileSolidTop[(int) Game1.tile[x - num5, y].type]))
                    num6 = -num5;
                }
                if (num6 != 0 && Game1.tile[x + num6, y].liquid == (byte) 0 && (!Game1.tile[x + num6, y].active || !Game1.tileSolid[(int) Game1.tile[x + num6, y].type] || Game1.tileSolidTop[(int) Game1.tile[x + num6, y].type]))
                {
                  flag3 = true;
                  x += num6;
                }
                if (flag1 && !flag3)
                {
                  flag1 = false;
                  flag3 = true;
                  num5 = -num4;
                  num6 = 0;
                }
              }
              if (index3 != x && index1 != y)
                ++num1;
              Game1.tile[x, y].liquid = liquid;
              Game1.tile[x, y].lava = flag2;
              if (Game1.tile[x - 1, y].liquid > (byte) 0 && Game1.tile[x - 1, y].lava != flag2)
              {
                if (flag2)
                  Liquid.LavaCheck(x, y);
                else
                  Liquid.LavaCheck(x - 1, y);
              }
              else if (Game1.tile[x + 1, y].liquid > (byte) 0 && Game1.tile[x + 1, y].lava != flag2)
              {
                if (flag2)
                  Liquid.LavaCheck(x, y);
                else
                  Liquid.LavaCheck(x + 1, y);
              }
              else if (Game1.tile[x, y - 1].liquid > (byte) 0 && Game1.tile[x, y - 1].lava != flag2)
              {
                if (flag2)
                  Liquid.LavaCheck(x, y);
                else
                  Liquid.LavaCheck(x, y - 1);
              }
              else if (Game1.tile[x, y + 1].liquid > (byte) 0 && Game1.tile[x, y + 1].lava != flag2)
              {
                if (flag2)
                  Liquid.LavaCheck(x, y);
                else
                  Liquid.LavaCheck(x, y + 1);
              }
            }
          }
        }
      }
      return (double) num1;
    }

    public void Update()
    {
      if (Game1.tile[this.x, this.y].active && Game1.tileSolid[(int) Game1.tile[this.x, this.y].type] && !Game1.tileSolidTop[(int) Game1.tile[this.x, this.y].type])
      {
        Game1.tile[this.x, this.y].liquid = (byte) 0;
        this.kill = 9;
      }
      else
      {
        byte liquid = Game1.tile[this.x, this.y].liquid;
        if (Game1.tile[this.x, this.y].liquid == (byte) 0)
        {
          this.kill = 9;
        }
        else
        {
          if (Game1.tile[this.x, this.y].lava)
          {
            Liquid.LavaCheck(this.x, this.y);
            if (!Liquid.quickFall)
            {
              if (this.delay < 5)
              {
                ++this.delay;
                return;
              }
              this.delay = 0;
            }
          }
          else
          {
            if (Game1.tile[this.x - 1, this.y].lava)
              Liquid.AddWater(this.x - 1, this.y);
            if (Game1.tile[this.x + 1, this.y].lava)
              Liquid.AddWater(this.x + 1, this.y);
            if (Game1.tile[this.x, this.y - 1].lava)
              Liquid.AddWater(this.x, this.y - 1);
            if (Game1.tile[this.x, this.y + 1].lava)
              Liquid.AddWater(this.x, this.y + 1);
          }
          if ((!Game1.tile[this.x, this.y + 1].active || !Game1.tileSolid[(int) Game1.tile[this.x, this.y + 1].type] || Game1.tileSolidTop[(int) Game1.tile[this.x, this.y + 1].type]) && (Game1.tile[this.x, this.y + 1].liquid <= (byte) 0 || Game1.tile[this.x, this.y + 1].lava == Game1.tile[this.x, this.y].lava) && Game1.tile[this.x, this.y + 1].liquid < byte.MaxValue)
          {
            float num = (float) ((int) byte.MaxValue - (int) Game1.tile[this.x, this.y + 1].liquid);
            if ((double) num > (double) Game1.tile[this.x, this.y].liquid)
              num = (float) Game1.tile[this.x, this.y].liquid;
            Game1.tile[this.x, this.y].liquid -= (byte) num;
            Game1.tile[this.x, this.y + 1].liquid += (byte) num;
            Game1.tile[this.x, this.y + 1].lava = Game1.tile[this.x, this.y].lava;
            Liquid.AddWater(this.x, this.y + 1);
            Game1.tile[this.x, this.y + 1].skipLiquid = true;
            Game1.tile[this.x, this.y].skipLiquid = true;
            if (Game1.tile[this.x, this.y].liquid > (byte) 250)
            {
              Game1.tile[this.x, this.y].liquid = byte.MaxValue;
            }
            else
            {
              Liquid.AddWater(this.x - 1, this.y);
              Liquid.AddWater(this.x + 1, this.y);
            }
          }
          if (Game1.tile[this.x, this.y].liquid > (byte) 0)
          {
            bool flag1 = true;
            bool flag2 = true;
            bool flag3 = true;
            bool flag4 = true;
            if (Game1.tile[this.x - 1, this.y].active && Game1.tileSolid[(int) Game1.tile[this.x - 1, this.y].type] && !Game1.tileSolidTop[(int) Game1.tile[this.x - 1, this.y].type])
              flag1 = false;
            else if (Game1.tile[this.x - 1, this.y].liquid > (byte) 0 && Game1.tile[this.x - 1, this.y].lava != Game1.tile[this.x, this.y].lava)
              flag1 = false;
            else if (Game1.tile[this.x - 2, this.y].active && Game1.tileSolid[(int) Game1.tile[this.x - 2, this.y].type] && !Game1.tileSolidTop[(int) Game1.tile[this.x - 2, this.y].type])
              flag3 = false;
            else if (Game1.tile[this.x - 2, this.y].liquid == (byte) 0)
              flag3 = false;
            else if (Game1.tile[this.x - 2, this.y].liquid > (byte) 0 && Game1.tile[this.x - 2, this.y].lava != Game1.tile[this.x, this.y].lava)
              flag3 = false;
            if (Game1.tile[this.x + 1, this.y].active && Game1.tileSolid[(int) Game1.tile[this.x + 1, this.y].type] && !Game1.tileSolidTop[(int) Game1.tile[this.x + 1, this.y].type])
              flag2 = false;
            else if (Game1.tile[this.x + 1, this.y].liquid > (byte) 0 && Game1.tile[this.x + 1, this.y].lava != Game1.tile[this.x, this.y].lava)
              flag2 = false;
            else if (Game1.tile[this.x + 2, this.y].active && Game1.tileSolid[(int) Game1.tile[this.x + 2, this.y].type] && !Game1.tileSolidTop[(int) Game1.tile[this.x + 2, this.y].type])
              flag4 = false;
            else if (Game1.tile[this.x + 2, this.y].liquid == (byte) 0)
              flag4 = false;
            else if (Game1.tile[this.x + 2, this.y].liquid > (byte) 0 && Game1.tile[this.x + 2, this.y].lava != Game1.tile[this.x, this.y].lava)
              flag4 = false;
            int num1 = 0;
            if (Game1.tile[this.x, this.y].liquid < (byte) 3)
              num1 = -1;
            if (flag1 && flag2)
            {
              if (flag3 && flag4)
              {
                bool flag5 = true;
                bool flag6 = true;
                if (Game1.tile[this.x - 3, this.y].active && Game1.tileSolid[(int) Game1.tile[this.x - 3, this.y].type] && !Game1.tileSolidTop[(int) Game1.tile[this.x - 3, this.y].type])
                  flag5 = false;
                else if (Game1.tile[this.x - 3, this.y].liquid == (byte) 0)
                  flag5 = false;
                else if (Game1.tile[this.x - 3, this.y].lava != Game1.tile[this.x, this.y].lava)
                  flag5 = false;
                if (Game1.tile[this.x + 3, this.y].active && Game1.tileSolid[(int) Game1.tile[this.x + 3, this.y].type] && !Game1.tileSolidTop[(int) Game1.tile[this.x + 3, this.y].type])
                  flag6 = false;
                else if (Game1.tile[this.x + 3, this.y].liquid == (byte) 0)
                  flag6 = false;
                else if (Game1.tile[this.x + 3, this.y].lava != Game1.tile[this.x, this.y].lava)
                  flag6 = false;
                if (flag5 && flag6)
                {
                  float num2 = (float) Math.Round((double) ((int) Game1.tile[this.x - 1, this.y].liquid + (int) Game1.tile[this.x + 1, this.y].liquid + (int) Game1.tile[this.x - 2, this.y].liquid + (int) Game1.tile[this.x + 2, this.y].liquid + (int) Game1.tile[this.x - 3, this.y].liquid + (int) Game1.tile[this.x + 3, this.y].liquid + (int) Game1.tile[this.x, this.y].liquid + num1) / 7.0);
                  int num3 = 0;
                  if ((int) Game1.tile[this.x - 1, this.y].liquid != (int) (byte) num2)
                  {
                    Liquid.AddWater(this.x - 1, this.y);
                    Game1.tile[this.x - 1, this.y].liquid = (byte) num2;
                  }
                  else
                    ++num3;
                  Game1.tile[this.x - 1, this.y].lava = Game1.tile[this.x, this.y].lava;
                  if ((int) Game1.tile[this.x + 1, this.y].liquid != (int) (byte) num2)
                  {
                    Liquid.AddWater(this.x + 1, this.y);
                    Game1.tile[this.x + 1, this.y].liquid = (byte) num2;
                  }
                  else
                    ++num3;
                  Game1.tile[this.x + 1, this.y].lava = Game1.tile[this.x, this.y].lava;
                  if ((int) Game1.tile[this.x - 2, this.y].liquid != (int) (byte) num2)
                  {
                    Liquid.AddWater(this.x - 2, this.y);
                    Game1.tile[this.x - 2, this.y].liquid = (byte) num2;
                  }
                  else
                    ++num3;
                  Game1.tile[this.x - 2, this.y].lava = Game1.tile[this.x, this.y].lava;
                  if ((int) Game1.tile[this.x + 2, this.y].liquid != (int) (byte) num2)
                  {
                    Liquid.AddWater(this.x + 2, this.y);
                    Game1.tile[this.x + 2, this.y].liquid = (byte) num2;
                  }
                  else
                    ++num3;
                  Game1.tile[this.x + 2, this.y].lava = Game1.tile[this.x, this.y].lava;
                  if ((int) Game1.tile[this.x - 3, this.y].liquid != (int) (byte) num2)
                  {
                    Liquid.AddWater(this.x - 3, this.y);
                    Game1.tile[this.x - 3, this.y].liquid = (byte) num2;
                  }
                  else
                    ++num3;
                  Game1.tile[this.x - 3, this.y].lava = Game1.tile[this.x, this.y].lava;
                  if ((int) Game1.tile[this.x + 3, this.y].liquid != (int) (byte) num2)
                  {
                    Liquid.AddWater(this.x + 3, this.y);
                    Game1.tile[this.x + 3, this.y].liquid = (byte) num2;
                  }
                  else
                    ++num3;
                  Game1.tile[this.x + 3, this.y].lava = Game1.tile[this.x, this.y].lava;
                  if (num3 != 6 || Game1.tile[this.x, this.y - 1].liquid <= (byte) 0)
                    Game1.tile[this.x, this.y].liquid = (byte) num2;
                }
                else
                {
                  int num4 = 0;
                  float num5 = (float) Math.Round((double) ((int) Game1.tile[this.x - 1, this.y].liquid + (int) Game1.tile[this.x + 1, this.y].liquid + (int) Game1.tile[this.x - 2, this.y].liquid + (int) Game1.tile[this.x + 2, this.y].liquid + (int) Game1.tile[this.x, this.y].liquid + num1) / 5.0);
                  if ((int) Game1.tile[this.x - 1, this.y].liquid != (int) (byte) num5)
                  {
                    Liquid.AddWater(this.x - 1, this.y);
                    Game1.tile[this.x - 1, this.y].liquid = (byte) num5;
                  }
                  else
                    ++num4;
                  Game1.tile[this.x - 1, this.y].lava = Game1.tile[this.x, this.y].lava;
                  if ((int) Game1.tile[this.x + 1, this.y].liquid != (int) (byte) num5)
                  {
                    Liquid.AddWater(this.x + 1, this.y);
                    Game1.tile[this.x + 1, this.y].liquid = (byte) num5;
                  }
                  else
                    ++num4;
                  Game1.tile[this.x + 1, this.y].lava = Game1.tile[this.x, this.y].lava;
                  if ((int) Game1.tile[this.x - 2, this.y].liquid != (int) (byte) num5)
                  {
                    Liquid.AddWater(this.x - 2, this.y);
                    Game1.tile[this.x - 2, this.y].liquid = (byte) num5;
                  }
                  else
                    ++num4;
                  Game1.tile[this.x - 2, this.y].lava = Game1.tile[this.x, this.y].lava;
                  if ((int) Game1.tile[this.x + 2, this.y].liquid != (int) (byte) num5)
                  {
                    Liquid.AddWater(this.x + 2, this.y);
                    Game1.tile[this.x + 2, this.y].liquid = (byte) num5;
                  }
                  else
                    ++num4;
                  Game1.tile[this.x + 2, this.y].lava = Game1.tile[this.x, this.y].lava;
                  if (num4 != 4 || Game1.tile[this.x, this.y - 1].liquid <= (byte) 0)
                    Game1.tile[this.x, this.y].liquid = (byte) num5;
                }
              }
              else if (flag3)
              {
                float num6 = (float) Math.Round((double) ((int) Game1.tile[this.x - 1, this.y].liquid + (int) Game1.tile[this.x + 1, this.y].liquid + (int) Game1.tile[this.x - 2, this.y].liquid + (int) Game1.tile[this.x, this.y].liquid + num1) / 4.0 + 0.001);
                if ((int) Game1.tile[this.x - 1, this.y].liquid != (int) (byte) num6)
                {
                  Liquid.AddWater(this.x - 1, this.y);
                  Game1.tile[this.x - 1, this.y].liquid = (byte) num6;
                }
                Game1.tile[this.x - 1, this.y].lava = Game1.tile[this.x, this.y].lava;
                if ((int) Game1.tile[this.x + 1, this.y].liquid != (int) (byte) num6)
                {
                  Liquid.AddWater(this.x + 1, this.y);
                  Game1.tile[this.x + 1, this.y].liquid = (byte) num6;
                }
                Game1.tile[this.x + 1, this.y].lava = Game1.tile[this.x, this.y].lava;
                if ((int) Game1.tile[this.x - 2, this.y].liquid != (int) (byte) num6)
                {
                  Liquid.AddWater(this.x - 2, this.y);
                  Game1.tile[this.x - 2, this.y].liquid = (byte) num6;
                }
                Game1.tile[this.x - 2, this.y].lava = Game1.tile[this.x, this.y].lava;
                Game1.tile[this.x, this.y].liquid = (byte) num6;
              }
              else if (flag4)
              {
                float num7 = (float) Math.Round((double) ((int) Game1.tile[this.x - 1, this.y].liquid + (int) Game1.tile[this.x + 1, this.y].liquid + (int) Game1.tile[this.x + 2, this.y].liquid + (int) Game1.tile[this.x, this.y].liquid + num1) / 4.0 + 0.001);
                if ((int) Game1.tile[this.x - 1, this.y].liquid != (int) (byte) num7)
                {
                  Liquid.AddWater(this.x - 1, this.y);
                  Game1.tile[this.x - 1, this.y].liquid = (byte) num7;
                }
                Game1.tile[this.x - 1, this.y].lava = Game1.tile[this.x, this.y].lava;
                if ((int) Game1.tile[this.x + 1, this.y].liquid != (int) (byte) num7)
                {
                  Liquid.AddWater(this.x + 1, this.y);
                  Game1.tile[this.x + 1, this.y].liquid = (byte) num7;
                }
                Game1.tile[this.x + 1, this.y].lava = Game1.tile[this.x, this.y].lava;
                if ((int) Game1.tile[this.x + 2, this.y].liquid != (int) (byte) num7)
                {
                  Liquid.AddWater(this.x + 2, this.y);
                  Game1.tile[this.x + 2, this.y].liquid = (byte) num7;
                }
                Game1.tile[this.x + 2, this.y].lava = Game1.tile[this.x, this.y].lava;
                Game1.tile[this.x, this.y].liquid = (byte) num7;
              }
              else
              {
                float num8 = (float) Math.Round((double) ((int) Game1.tile[this.x - 1, this.y].liquid + (int) Game1.tile[this.x + 1, this.y].liquid + (int) Game1.tile[this.x, this.y].liquid + num1) / 3.0 + 0.001);
                if ((int) Game1.tile[this.x - 1, this.y].liquid != (int) (byte) num8)
                {
                  Liquid.AddWater(this.x - 1, this.y);
                  Game1.tile[this.x - 1, this.y].liquid = (byte) num8;
                }
                Game1.tile[this.x - 1, this.y].lava = Game1.tile[this.x, this.y].lava;
                if ((int) Game1.tile[this.x + 1, this.y].liquid != (int) (byte) num8)
                {
                  Liquid.AddWater(this.x + 1, this.y);
                  Game1.tile[this.x + 1, this.y].liquid = (byte) num8;
                }
                Game1.tile[this.x + 1, this.y].lava = Game1.tile[this.x, this.y].lava;
                Game1.tile[this.x, this.y].liquid = (byte) num8;
              }
            }
            else if (flag1)
            {
              float num9 = (float) Math.Round((double) ((int) Game1.tile[this.x - 1, this.y].liquid + (int) Game1.tile[this.x, this.y].liquid + num1) / 2.0 + 0.001);
              if ((int) Game1.tile[this.x - 1, this.y].liquid != (int) (byte) num9)
              {
                Liquid.AddWater(this.x - 1, this.y);
                Game1.tile[this.x - 1, this.y].liquid = (byte) num9;
              }
              Game1.tile[this.x - 1, this.y].lava = Game1.tile[this.x, this.y].lava;
              Game1.tile[this.x, this.y].liquid = (byte) num9;
            }
            else if (flag2)
            {
              float num10 = (float) Math.Round((double) ((int) Game1.tile[this.x + 1, this.y].liquid + (int) Game1.tile[this.x, this.y].liquid + num1) / 2.0 + 0.001);
              if ((int) Game1.tile[this.x + 1, this.y].liquid != (int) (byte) num10)
              {
                Liquid.AddWater(this.x + 1, this.y);
                Game1.tile[this.x + 1, this.y].liquid = (byte) num10;
              }
              Game1.tile[this.x + 1, this.y].lava = Game1.tile[this.x, this.y].lava;
              Game1.tile[this.x, this.y].liquid = (byte) num10;
            }
          }
          if ((int) Game1.tile[this.x, this.y].liquid != (int) liquid)
          {
            if (Game1.tile[this.x, this.y].liquid == (byte) 254 && liquid == byte.MaxValue)
            {
              Game1.tile[this.x, this.y].liquid = byte.MaxValue;
              ++this.kill;
            }
            else
            {
              Liquid.AddWater(this.x, this.y - 1);
              this.kill = 0;
            }
          }
          else
            ++this.kill;
        }
      }
    }

    public static void UpdateLiquid()
    {
      if (Game1.netMode == 2)
      {
        Liquid.cycles = 25;
        Liquid.maxLiquid = 5000;
      }
      if (!WorldGen.gen)
      {
        if (!Liquid.panicMode)
        {
          if (Liquid.numLiquid + LiquidBuffer.numLiquidBuffer > 4000)
          {
            ++Liquid.panicCounter;
            if (Liquid.panicCounter > 1800 || Liquid.numLiquid + LiquidBuffer.numLiquidBuffer > 13500)
            {
              WorldGen.waterLine = Game1.maxTilesY;
              Liquid.numLiquid = 0;
              LiquidBuffer.numLiquidBuffer = 0;
              Liquid.panicCounter = 0;
              Liquid.panicMode = true;
              Liquid.panicY = Game1.maxTilesY - 3;
            }
          }
          else
            Liquid.panicCounter = 0;
        }
        if (Liquid.panicMode)
        {
          int num = 0;
          while (Liquid.panicY >= 3 && num < 5)
          {
            ++num;
            Liquid.QuickWater(minY: Liquid.panicY, maxY: Liquid.panicY);
            --Liquid.panicY;
            if (Liquid.panicY < 3)
            {
              Liquid.panicCounter = 0;
              Liquid.panicMode = false;
              WorldGen.WaterCheck();
              if (Game1.netMode == 2)
              {
                for (int index1 = 0; index1 < 8; ++index1)
                {
                  for (int index2 = 0; index2 < Game1.maxSectionsX; ++index2)
                  {
                    for (int index3 = 0; index3 < Game1.maxSectionsY; ++index3)
                      Netplay.serverSock[index1].tileSection[index2, index3] = false;
                  }
                }
              }
            }
          }
          return;
        }
      }
      Liquid.quickFall = Liquid.quickSettle || Liquid.numLiquid > 2000;
      ++Liquid.wetCounter;
      int num1 = Liquid.maxLiquid / Liquid.cycles;
      int num2 = num1 * (Liquid.wetCounter - 1);
      int num3 = num1 * Liquid.wetCounter;
      if (Liquid.wetCounter == Liquid.cycles)
        num3 = Liquid.numLiquid;
      if (num3 > Liquid.numLiquid)
      {
        num3 = Liquid.numLiquid;
        int netMode = Game1.netMode;
        Liquid.wetCounter = Liquid.cycles;
      }
      if (Liquid.quickFall)
      {
        for (int index = num2; index < num3; ++index)
        {
          Game1.liquid[index].delay = 10;
          Game1.liquid[index].Update();
          Game1.tile[Game1.liquid[index].x, Game1.liquid[index].y].skipLiquid = false;
        }
      }
      else
      {
        for (int index = num2; index < num3; ++index)
        {
          if (!Game1.tile[Game1.liquid[index].x, Game1.liquid[index].y].skipLiquid)
            Game1.liquid[index].Update();
          else
            Game1.tile[Game1.liquid[index].x, Game1.liquid[index].y].skipLiquid = false;
        }
      }
      if (Liquid.wetCounter < Liquid.cycles)
        return;
      Liquid.wetCounter = 0;
      for (int l = Liquid.numLiquid - 1; l >= 0; --l)
      {
        if (Game1.liquid[l].kill > 3)
          Liquid.DelWater(l);
      }
      int num4 = Liquid.maxLiquid - (Liquid.maxLiquid - Liquid.numLiquid);
      if (num4 > LiquidBuffer.numLiquidBuffer)
        num4 = LiquidBuffer.numLiquidBuffer;
      for (int index = 0; index < num4; ++index)
      {
        Game1.tile[Game1.liquidBuffer[0].x, Game1.liquidBuffer[0].y].checkingLiquid = false;
        Liquid.AddWater(Game1.liquidBuffer[0].x, Game1.liquidBuffer[0].y);
        LiquidBuffer.DelBuffer(0);
      }
      if (Liquid.numLiquid > 0 && Liquid.numLiquid > Liquid.stuckAmount - 50 && Liquid.numLiquid < Liquid.stuckAmount + 50)
      {
        ++Liquid.stuckCount;
        if (Liquid.stuckCount >= 10000)
        {
          Liquid.stuck = true;
          for (int l = Liquid.numLiquid - 1; l >= 0; --l)
            Liquid.DelWater(l);
          Liquid.stuck = false;
          Liquid.stuckCount = 0;
        }
      }
      else
      {
        Liquid.stuckCount = 0;
        Liquid.stuckAmount = Liquid.numLiquid;
      }
    }

    public static void AddWater(int x, int y)
    {
      if (Game1.tile[x, y].checkingLiquid || x >= Game1.maxTilesX - 5 || y >= Game1.maxTilesY - 5 || x < 5 || y < 5 || Game1.tile[x, y] == null || Game1.tile[x, y].liquid == (byte) 0)
        return;
      if (Liquid.numLiquid >= Liquid.maxLiquid - 1)
      {
        LiquidBuffer.AddBuffer(x, y);
      }
      else
      {
        Game1.tile[x, y].checkingLiquid = true;
        Game1.liquid[Liquid.numLiquid].kill = 0;
        Game1.liquid[Liquid.numLiquid].x = x;
        Game1.liquid[Liquid.numLiquid].y = y;
        Game1.liquid[Liquid.numLiquid].delay = 0;
        Game1.tile[x, y].skipLiquid = false;
        ++Liquid.numLiquid;
        if (Game1.netMode == 2)
          NetMessage.sendWater(x, y);
        if (!Game1.tile[x, y].active || !Game1.tileWaterDeath[(int) Game1.tile[x, y].type] && (!Game1.tile[x, y].lava || !Game1.tileLavaDeath[(int) Game1.tile[x, y].type]))
          return;
        if (WorldGen.gen)
        {
          Game1.tile[x, y].active = false;
        }
        else
        {
          WorldGen.KillTile(x, y);
          if (Game1.netMode == 2)
            NetMessage.SendData(17, number2: (float) x, number3: (float) y);
        }
      }
    }

    public static void LavaCheck(int x, int y)
    {
      if (Game1.tile[x - 1, y].liquid > (byte) 0 && !Game1.tile[x - 1, y].lava || Game1.tile[x + 1, y].liquid > (byte) 0 && !Game1.tile[x + 1, y].lava || Game1.tile[x, y - 1].liquid > (byte) 0 && !Game1.tile[x, y - 1].lava)
      {
        int num = 0;
        if (!Game1.tile[x - 1, y].lava)
        {
          num += (int) Game1.tile[x - 1, y].liquid;
          Game1.tile[x - 1, y].liquid = (byte) 0;
        }
        if (!Game1.tile[x + 1, y].lava)
        {
          num += (int) Game1.tile[x + 1, y].liquid;
          Game1.tile[x + 1, y].liquid = (byte) 0;
        }
        if (!Game1.tile[x, y - 1].lava)
        {
          num += (int) Game1.tile[x, y - 1].liquid;
          Game1.tile[x, y - 1].liquid = (byte) 0;
        }
        if (num < 128)
          return;
        Game1.tile[x, y].liquid = (byte) 0;
        Game1.tile[x, y].lava = false;
        WorldGen.PlaceTile(x, y, 56, true, true);
        WorldGen.SquareTileFrame(x, y);
        if (Game1.netMode == 2)
          NetMessage.SendTileSquare(-1, x - 1, y - 1, 3);
      }
      else
      {
        if (Game1.tile[x, y + 1].liquid <= (byte) 0 || Game1.tile[x, y + 1].lava)
          return;
        Game1.tile[x, y].liquid = (byte) 0;
        Game1.tile[x, y].lava = false;
        WorldGen.PlaceTile(x, y + 1, 56, true, true);
        WorldGen.SquareTileFrame(x, y);
        if (Game1.netMode == 2)
          NetMessage.SendTileSquare(-1, x - 1, y, 3);
      }
    }

    public static void NetAddWater(int x, int y)
    {
      if (x >= Game1.maxTilesX - 5 || y >= Game1.maxTilesY - 5 || x < 5 || y < 5 || Game1.tile[x, y] == null || Game1.tile[x, y].liquid == (byte) 0)
        return;
      for (int index = 0; index < Liquid.numLiquid; ++index)
      {
        if (Game1.liquid[index].x == x && Game1.liquid[index].y == y)
        {
          Game1.liquid[index].kill = 0;
          Game1.tile[x, y].skipLiquid = true;
          return;
        }
      }
      if (Liquid.numLiquid >= Liquid.maxLiquid - 1)
      {
        LiquidBuffer.AddBuffer(x, y);
      }
      else
      {
        Game1.tile[x, y].checkingLiquid = true;
        Game1.tile[x, y].skipLiquid = true;
        Game1.liquid[Liquid.numLiquid].kill = 0;
        Game1.liquid[Liquid.numLiquid].x = x;
        Game1.liquid[Liquid.numLiquid].y = y;
        ++Liquid.numLiquid;
        if (Game1.netMode == 2)
          NetMessage.sendWater(x, y);
        if (!Game1.tile[x, y].active || !Game1.tileWaterDeath[(int) Game1.tile[x, y].type] && (!Game1.tile[x, y].lava || !Game1.tileLavaDeath[(int) Game1.tile[x, y].type]))
          return;
        WorldGen.KillTile(x, y);
        if (Game1.netMode == 2)
          NetMessage.SendData(17, number2: (float) x, number3: (float) y);
      }
    }

    public static void DelWater(int l)
    {
      int x = Game1.liquid[l].x;
      int y = Game1.liquid[l].y;
      if (Game1.tile[x, y].liquid < (byte) 2)
        Game1.tile[x, y].liquid = (byte) 0;
      else if (Game1.tile[x, y].liquid < (byte) 20)
      {
        if ((int) Game1.tile[x - 1, y].liquid < (int) Game1.tile[x, y].liquid && (!Game1.tile[x - 1, y].active || !Game1.tileSolid[(int) Game1.tile[x - 1, y].type] || Game1.tileSolidTop[(int) Game1.tile[x - 1, y].type]) || (int) Game1.tile[x + 1, y].liquid < (int) Game1.tile[x, y].liquid && (!Game1.tile[x + 1, y].active || !Game1.tileSolid[(int) Game1.tile[x + 1, y].type] || Game1.tileSolidTop[(int) Game1.tile[x + 1, y].type]) || Game1.tile[x, y + 1].liquid < byte.MaxValue && (!Game1.tile[x, y + 1].active || !Game1.tileSolid[(int) Game1.tile[x, y + 1].type] || Game1.tileSolidTop[(int) Game1.tile[x, y + 1].type]))
          Game1.tile[x, y].liquid = (byte) 0;
      }
      else if (Game1.tile[x, y + 1].liquid < byte.MaxValue && (!Game1.tile[x, y + 1].active || !Game1.tileSolid[(int) Game1.tile[x, y + 1].type] || Game1.tileSolidTop[(int) Game1.tile[x, y + 1].type]) && !Liquid.stuck)
      {
        Game1.liquid[l].kill = 0;
        return;
      }
      if (Game1.tile[x, y].liquid == (byte) 0)
        Game1.tile[x, y].lava = false;
      else if (Game1.tile[x, y].lava)
        Liquid.LavaCheck(x, y);
      if (Game1.netMode == 2)
        NetMessage.sendWater(x, y);
      --Liquid.numLiquid;
      Game1.tile[Game1.liquid[l].x, Game1.liquid[l].y].checkingLiquid = false;
      Game1.liquid[l].x = Game1.liquid[Liquid.numLiquid].x;
      Game1.liquid[l].y = Game1.liquid[Liquid.numLiquid].y;
      Game1.liquid[l].kill = Game1.liquid[Liquid.numLiquid].kill;
    }
  }
}
