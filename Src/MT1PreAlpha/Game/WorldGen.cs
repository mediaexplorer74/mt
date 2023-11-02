
using Microsoft.Xna.Framework;
using System;

namespace GameManager
{
  internal class WorldGen
  {
    private static bool mergeUp = false;
    private static bool mergeDown = false;
    private static bool mergeLeft = false;
    private static bool mergeRight = false;
    private static bool destroyObject = false;

    public static void generateWorld()
    {
      int num1 = 0;
      int num2 = 0;
      double num3 = 875.34999999999991 * ((double) Game1.rand.Next(90, 110) * 0.01);
      double num4 = (num3 + 375.15) * ((double) Game1.rand.Next(90, 110) * 0.01);
      double minValue1 = num3;
      double num5 = num3;
      double minValue2 = num4;
      double num6 = num4;
      for (int index1 = 0; index1 < 5001; ++index1)
      {
        if (num3 < minValue1)
          minValue1 = num3;
        if (num3 > num5)
          num5 = num3;
        if (num4 < minValue2)
          minValue2 = num4;
        if (num4 > num6)
          num6 = num4;
        if (num2 <= 0)
        {
          num1 = Game1.rand.Next(0, 5);
          num2 = Game1.rand.Next(5, 40);
          if (num1 == 0)
            num2 *= (int) ((double) Game1.rand.Next(15, 30) * 0.1);
        }
        --num2;
        switch (num1)
        {
          case 0:
            while (Game1.rand.Next(0, 7) == 0)
              num3 += (double) Game1.rand.Next(-1, 2);
            break;
          case 1:
            while (Game1.rand.Next(0, 4) == 0)
              --num3;
            while (Game1.rand.Next(0, 10) == 0)
              ++num3;
            break;
          case 2:
            while (Game1.rand.Next(0, 4) == 0)
              ++num3;
            while (Game1.rand.Next(0, 10) == 0)
              --num3;
            break;
          case 3:
            while (Game1.rand.Next(0, 2) == 0)
              --num3;
            while (Game1.rand.Next(0, 6) == 0)
              ++num3;
            break;
          case 4:
            while (Game1.rand.Next(0, 2) == 0)
              ++num3;
            while (Game1.rand.Next(0, 5) == 0)
              --num3;
            break;
        }
        if (num3 < 125.05000000000001)
        {
          num3 = 125.05000000000001;
          num2 = 0;
        }
        else if (num3 > 2375.95)
        {
          num3 = 2375.95;
          num2 = 0;
        }
        while (Game1.rand.Next(0, 3) == 0)
          num4 += (double) Game1.rand.Next(-2, 3);
        if (num4 < num3 + 125.05000000000001)
          ++num4;
        if (num4 > num3 + 625.25)
          --num4;
        for (int index2 = 0; (double) index2 < num3; ++index2)
        {
          Game1.tile[index1, index2].active = false;
          Game1.tile[index1, index2].lighted = true;
        }
        for (int index3 = (int) num3; index3 < 2501; ++index3)
        {
          if ((double) index3 < num4)
          {
            Game1.tile[index1, index3].active = true;
            Game1.tile[index1, index3].type = (byte) 0;
            Game1.tile[index1, index3].frameX = (short) -1;
            Game1.tile[index1, index3].frameY = (short) -1;
          }
          else
          {
            Game1.tile[index1, index3].active = true;
            Game1.tile[index1, index3].type = (byte) 1;
            Game1.tile[index1, index3].frameX = (short) -1;
            Game1.tile[index1, index3].frameY = (short) -1;
          }
        }
      }
      Game1.worldSurface = num5;
      for (int index = 0; index < 25015; ++index)
        WorldGen.TileRunner(Game1.rand.Next(0, 5001), Game1.rand.Next((int) minValue1, (int) num6 + 1), (double) Game1.rand.Next(2, 7), Game1.rand.Next(2, 23), 1);
      for (int index = 0; index < 62537; ++index)
        WorldGen.TileRunner(Game1.rand.Next(0, 5001), Game1.rand.Next((int) minValue2, 2501), (double) Game1.rand.Next(2, 6), Game1.rand.Next(2, 40), 0);
      for (int index = 0; index < 12507; ++index)
        WorldGen.TileRunner(Game1.rand.Next(0, 5001), Game1.rand.Next((int) num5, 2501), (double) Game1.rand.Next(2, 5), Game1.rand.Next(2, 20), -1);
      for (int index = 0; index < 12507; ++index)
        WorldGen.TileRunner(Game1.rand.Next(0, 5001), Game1.rand.Next((int) num5, 2501), (double) Game1.rand.Next(8, 15), Game1.rand.Next(7, 30), -1);
      for (int index = 0; index < 2501; ++index)
      {
        if (num6 <= 2501.0)
          WorldGen.TileRunner(Game1.rand.Next(0, 5001), Game1.rand.Next((int) num6, 2501), (double) Game1.rand.Next(10, 20), Game1.rand.Next(50, 300), -1);
      }
      for (int index = 0; index < 25; ++index)
      {
        int i = Game1.rand.Next(0, 5001);
        for (int j = 0; (double) j < num5; ++j)
        {
          if (Game1.tile[i, j].active)
          {
            WorldGen.TileRunner(i, j, (double) Game1.rand.Next(3, 6), Game1.rand.Next(5, 50), -1, speedX: (float) Game1.rand.Next(-10, 11) * 0.1f, speedY: 1f);
            break;
          }
        }
      }
      for (int index = 0; index < 40; ++index)
      {
        int i = Game1.rand.Next(0, 5001);
        for (int j = 0; (double) j < num5; ++j)
        {
          if (Game1.tile[i, j].active)
          {
            WorldGen.TileRunner(i, j, (double) Game1.rand.Next(10, 15), Game1.rand.Next(50, 130), -1, speedX: (float) Game1.rand.Next(-10, 11) * 0.1f, speedY: 1f);
            break;
          }
        }
      }
      for (int index = 0; index < 10; ++index)
      {
        int i = Game1.rand.Next(0, 5001);
        for (int j = 0; (double) j < num5; ++j)
        {
          if (Game1.tile[i, j].active)
          {
            WorldGen.TileRunner(i, j, (double) Game1.rand.Next(12, 25), Game1.rand.Next(100, 400), -1, speedX: (float) Game1.rand.Next(-10, 11) * 0.1f, speedY: 3f);
            WorldGen.TileRunner(i, j, (double) Game1.rand.Next(8, 17), Game1.rand.Next(60, 200), -1, speedX: (float) Game1.rand.Next(-10, 11) * 0.1f, speedY: 2f);
            WorldGen.TileRunner(i, j, (double) Game1.rand.Next(5, 13), Game1.rand.Next(40, 170), -1, speedX: (float) Game1.rand.Next(-10, 11) * 0.1f, speedY: 2f);
            break;
          }
        }
      }
      for (int index4 = 0; index4 < 12507; ++index4)
      {
        int index5 = Game1.rand.Next(1, 5000);
        int index6 = Game1.rand.Next((int) minValue1, (int) num5);
        if (index6 >= 2501)
          index6 = 2499;
        if (Game1.tile[index5 - 1, index6].active && Game1.tile[index5 - 1, index6].type == (byte) 0 && Game1.tile[index5 + 1, index6].active && Game1.tile[index5 + 1, index6].type == (byte) 0 && Game1.tile[index5, index6 - 1].active && Game1.tile[index5, index6 - 1].type == (byte) 0 && Game1.tile[index5, index6 + 1].active && Game1.tile[index5, index6 + 1].type == (byte) 0)
        {
          Game1.tile[index5, index6].active = true;
          Game1.tile[index5, index6].type = (byte) 2;
        }
      }
      for (int index = 0; index < 625; ++index)
        WorldGen.TileRunner(Game1.rand.Next(0, 5001), Game1.rand.Next((int) minValue1, (int) num5), (double) Game1.rand.Next(3, 4), Game1.rand.Next(2, 5), 7);
      for (int index = 0; index < 750; ++index)
        WorldGen.TileRunner(Game1.rand.Next(0, 5001), Game1.rand.Next((int) num5, (int) num6), (double) Game1.rand.Next(3, 6), Game1.rand.Next(3, 6), 7);
      for (int index = 0; index < 3752; ++index)
        WorldGen.TileRunner(Game1.rand.Next(0, 5001), Game1.rand.Next((int) minValue2, 2501), (double) Game1.rand.Next(4, 9), Game1.rand.Next(4, 8), 7);
      for (int index = 0; index < 500; ++index)
        WorldGen.TileRunner(Game1.rand.Next(0, 5001), Game1.rand.Next((int) minValue1, (int) num5), (double) Game1.rand.Next(3, 4), Game1.rand.Next(2, 5), 6);
      for (int index = 0; index < 625; ++index)
        WorldGen.TileRunner(Game1.rand.Next(0, 5001), Game1.rand.Next((int) num5, (int) num6), (double) Game1.rand.Next(3, 6), Game1.rand.Next(3, 6), 6);
      for (int index = 0; index < 2501; ++index)
        WorldGen.TileRunner(Game1.rand.Next(0, 5001), Game1.rand.Next((int) minValue2, 2501), (double) Game1.rand.Next(4, 9), Game1.rand.Next(4, 8), 6);
      for (int index = 0; index < 125; ++index)
        WorldGen.TileRunner(Game1.rand.Next(0, 5001), Game1.rand.Next((int) minValue1, (int) num5), (double) Game1.rand.Next(3, 4), Game1.rand.Next(2, 5), 9);
      for (int index = 0; index < 250; ++index)
        WorldGen.TileRunner(Game1.rand.Next(0, 5001), Game1.rand.Next((int) num5, (int) num6), (double) Game1.rand.Next(3, 6), Game1.rand.Next(3, 6), 9);
      for (int index = 0; index < 1250; ++index)
        WorldGen.TileRunner(Game1.rand.Next(0, 5001), Game1.rand.Next((int) minValue2, 2501), (double) Game1.rand.Next(4, 9), Game1.rand.Next(4, 8), 9);
      for (int index = 0; index < 62; ++index)
        WorldGen.TileRunner(Game1.rand.Next(0, 5001), Game1.rand.Next((int) minValue1, (int) num5), (double) Game1.rand.Next(3, 4), Game1.rand.Next(2, 5), 8);
      for (int index = 0; index < 125; ++index)
        WorldGen.TileRunner(Game1.rand.Next(0, 5001), Game1.rand.Next((int) num5, (int) num6), (double) Game1.rand.Next(3, 6), Game1.rand.Next(3, 6), 8);
      for (int index = 0; index < 625; ++index)
        WorldGen.TileRunner(Game1.rand.Next(0, 5001), Game1.rand.Next((int) minValue2, 2501), (double) Game1.rand.Next(4, 9), Game1.rand.Next(4, 8), 8);
      for (int index = 0; index < 5001; ++index)
      {
        int i = index;
        for (int j = 0; (double) j < Game1.worldSurface - 1.0; ++j)
        {
          if (Game1.tile[i, j].active)
          {
            if (Game1.tile[i, j].type == (byte) 0)
            {
              WorldGen.SpreadGrass(i, j);
              break;
            }
            break;
          }
        }
      }
      for (int index7 = 0; index7 < 2501; ++index7)
      {
        int index8 = 2500;
        if (Game1.tile[index8, index7].active)
        {
          Game1.spawnTileX = index8;
          Game1.spawnTileY = index7;
          Game1.player[0].Spawn();
          break;
        }
      }
      WorldGen.AddTrees();
      WorldGen.EveryTileFrame();
      WorldGen.AddPlants();
    }

    public static void AddTrees()
    {
      for (int index1 = 1; index1 < 5000; ++index1)
      {
        for (int index2 = 20; (double) index2 < Game1.worldSurface; ++index2)
        {
          if (Game1.tile[index1, index2].active && Game1.tile[index1, index2].type == (byte) 2 && Game1.tile[index1 - 1, index2].active && Game1.tile[index1 - 1, index2].type == (byte) 2 && Game1.tile[index1 + 1, index2].active && Game1.tile[index1 + 1, index2].type == (byte) 2 && WorldGen.EmptyTileCheck(index1 - 2, index1 + 2, index2 - 14, index2 - 1))
          {
            bool flag1 = false;
            bool flag2 = false;
            int num1 = Game1.rand.Next(5, 15);
            for (int index3 = index2 - num1; index3 < index2; ++index3)
            {
              Game1.tile[index1, index3].frameNumber = (byte) Game1.rand.Next(3);
              Game1.tile[index1, index3].active = true;
              Game1.tile[index1, index3].type = (byte) 5;
              int num2 = Game1.rand.Next(3);
              int num3 = Game1.rand.Next(10);
              if (index3 == index2 - 1 || index3 == index2 - num1)
                num3 = 0;
              while ((num3 == 5 || num3 == 7) && flag1 || (num3 == 6 || num3 == 7) && flag2)
                num3 = Game1.rand.Next(10);
              flag1 = false;
              flag2 = false;
              if (num3 == 5 || num3 == 7)
                flag1 = true;
              if (num3 == 6 || num3 == 7)
                flag2 = true;
              switch (num3)
              {
                case 1:
                  if (num2 == 0)
                  {
                    Game1.tile[index1, index3].frameX = (short) 0;
                    Game1.tile[index1, index3].frameY = (short) 66;
                  }
                  if (num2 == 1)
                  {
                    Game1.tile[index1, index3].frameX = (short) 0;
                    Game1.tile[index1, index3].frameY = (short) 88;
                  }
                  if (num2 == 2)
                  {
                    Game1.tile[index1, index3].frameX = (short) 0;
                    Game1.tile[index1, index3].frameY = (short) 110;
                    break;
                  }
                  break;
                case 2:
                  if (num2 == 0)
                  {
                    Game1.tile[index1, index3].frameX = (short) 22;
                    Game1.tile[index1, index3].frameY = (short) 0;
                  }
                  if (num2 == 1)
                  {
                    Game1.tile[index1, index3].frameX = (short) 22;
                    Game1.tile[index1, index3].frameY = (short) 22;
                  }
                  if (num2 == 2)
                  {
                    Game1.tile[index1, index3].frameX = (short) 22;
                    Game1.tile[index1, index3].frameY = (short) 44;
                    break;
                  }
                  break;
                case 3:
                  if (num2 == 0)
                  {
                    Game1.tile[index1, index3].frameX = (short) 44;
                    Game1.tile[index1, index3].frameY = (short) 66;
                  }
                  if (num2 == 1)
                  {
                    Game1.tile[index1, index3].frameX = (short) 44;
                    Game1.tile[index1, index3].frameY = (short) 88;
                  }
                  if (num2 == 2)
                  {
                    Game1.tile[index1, index3].frameX = (short) 44;
                    Game1.tile[index1, index3].frameY = (short) 110;
                    break;
                  }
                  break;
                case 4:
                  if (num2 == 0)
                  {
                    Game1.tile[index1, index3].frameX = (short) 22;
                    Game1.tile[index1, index3].frameY = (short) 66;
                  }
                  if (num2 == 1)
                  {
                    Game1.tile[index1, index3].frameX = (short) 22;
                    Game1.tile[index1, index3].frameY = (short) 88;
                  }
                  if (num2 == 2)
                  {
                    Game1.tile[index1, index3].frameX = (short) 22;
                    Game1.tile[index1, index3].frameY = (short) 110;
                    break;
                  }
                  break;
                case 5:
                  if (num2 == 0)
                  {
                    Game1.tile[index1, index3].frameX = (short) 88;
                    Game1.tile[index1, index3].frameY = (short) 0;
                  }
                  if (num2 == 1)
                  {
                    Game1.tile[index1, index3].frameX = (short) 88;
                    Game1.tile[index1, index3].frameY = (short) 22;
                  }
                  if (num2 == 2)
                  {
                    Game1.tile[index1, index3].frameX = (short) 88;
                    Game1.tile[index1, index3].frameY = (short) 44;
                    break;
                  }
                  break;
                case 6:
                  if (num2 == 0)
                  {
                    Game1.tile[index1, index3].frameX = (short) 66;
                    Game1.tile[index1, index3].frameY = (short) 66;
                  }
                  if (num2 == 1)
                  {
                    Game1.tile[index1, index3].frameX = (short) 66;
                    Game1.tile[index1, index3].frameY = (short) 88;
                  }
                  if (num2 == 2)
                  {
                    Game1.tile[index1, index3].frameX = (short) 66;
                    Game1.tile[index1, index3].frameY = (short) 110;
                    break;
                  }
                  break;
                case 7:
                  if (num2 == 0)
                  {
                    Game1.tile[index1, index3].frameX = (short) 110;
                    Game1.tile[index1, index3].frameY = (short) 66;
                  }
                  if (num2 == 1)
                  {
                    Game1.tile[index1, index3].frameX = (short) 110;
                    Game1.tile[index1, index3].frameY = (short) 88;
                  }
                  if (num2 == 2)
                  {
                    Game1.tile[index1, index3].frameX = (short) 110;
                    Game1.tile[index1, index3].frameY = (short) 110;
                    break;
                  }
                  break;
                default:
                  if (num2 == 0)
                  {
                    Game1.tile[index1, index3].frameX = (short) 0;
                    Game1.tile[index1, index3].frameY = (short) 0;
                  }
                  if (num2 == 1)
                  {
                    Game1.tile[index1, index3].frameX = (short) 0;
                    Game1.tile[index1, index3].frameY = (short) 22;
                  }
                  if (num2 == 2)
                  {
                    Game1.tile[index1, index3].frameX = (short) 0;
                    Game1.tile[index1, index3].frameY = (short) 44;
                  }
                  break;
              }
              if (num3 == 5 || num3 == 7)
              {
                Game1.tile[index1 - 1, index3].active = true;
                Game1.tile[index1 - 1, index3].type = (byte) 5;
                int num4 = Game1.rand.Next(3);
                if (Game1.rand.Next(3) < 2)
                {
                  if (num4 == 0)
                  {
                    Game1.tile[index1 - 1, index3].frameX = (short) 44;
                    Game1.tile[index1 - 1, index3].frameY = (short) 198;
                  }
                  if (num4 == 1)
                  {
                    Game1.tile[index1 - 1, index3].frameX = (short) 44;
                    Game1.tile[index1 - 1, index3].frameY = (short) 220;
                  }
                  if (num4 == 2)
                  {
                    Game1.tile[index1 - 1, index3].frameX = (short) 44;
                    Game1.tile[index1 - 1, index3].frameY = (short) 242;
                  }
                }
                else
                {
                  if (num4 == 0)
                  {
                    Game1.tile[index1 - 1, index3].frameX = (short) 66;
                    Game1.tile[index1 - 1, index3].frameY = (short) 0;
                  }
                  if (num4 == 1)
                  {
                    Game1.tile[index1 - 1, index3].frameX = (short) 66;
                    Game1.tile[index1 - 1, index3].frameY = (short) 22;
                  }
                  if (num4 == 2)
                  {
                    Game1.tile[index1 - 1, index3].frameX = (short) 66;
                    Game1.tile[index1 - 1, index3].frameY = (short) 44;
                  }
                }
              }
              if (num3 == 6 || num3 == 7)
              {
                Game1.tile[index1 + 1, index3].active = true;
                Game1.tile[index1 + 1, index3].type = (byte) 5;
                int num5 = Game1.rand.Next(3);
                if (Game1.rand.Next(3) < 2)
                {
                  if (num5 == 0)
                  {
                    Game1.tile[index1 + 1, index3].frameX = (short) 66;
                    Game1.tile[index1 + 1, index3].frameY = (short) 198;
                  }
                  if (num5 == 1)
                  {
                    Game1.tile[index1 + 1, index3].frameX = (short) 66;
                    Game1.tile[index1 + 1, index3].frameY = (short) 220;
                  }
                  if (num5 == 2)
                  {
                    Game1.tile[index1 + 1, index3].frameX = (short) 66;
                    Game1.tile[index1 + 1, index3].frameY = (short) 242;
                  }
                }
                else
                {
                  if (num5 == 0)
                  {
                    Game1.tile[index1 + 1, index3].frameX = (short) 88;
                    Game1.tile[index1 + 1, index3].frameY = (short) 66;
                  }
                  if (num5 == 1)
                  {
                    Game1.tile[index1 + 1, index3].frameX = (short) 88;
                    Game1.tile[index1 + 1, index3].frameY = (short) 88;
                  }
                  if (num5 == 2)
                  {
                    Game1.tile[index1 + 1, index3].frameX = (short) 88;
                    Game1.tile[index1 + 1, index3].frameY = (short) 110;
                  }
                }
              }
            }
            int num6 = Game1.rand.Next(3);
            if (num6 == 0 || num6 == 1)
            {
              Game1.tile[index1 + 1, index2 - 1].active = true;
              Game1.tile[index1 + 1, index2 - 1].type = (byte) 5;
              int num7 = Game1.rand.Next(3);
              if (num7 == 0)
              {
                Game1.tile[index1 + 1, index2 - 1].frameX = (short) 22;
                Game1.tile[index1 + 1, index2 - 1].frameY = (short) 132;
              }
              if (num7 == 1)
              {
                Game1.tile[index1 + 1, index2 - 1].frameX = (short) 22;
                Game1.tile[index1 + 1, index2 - 1].frameY = (short) 154;
              }
              if (num7 == 2)
              {
                Game1.tile[index1 + 1, index2 - 1].frameX = (short) 22;
                Game1.tile[index1 + 1, index2 - 1].frameY = (short) 176;
              }
            }
            if (num6 == 0 || num6 == 2)
            {
              Game1.tile[index1 - 1, index2 - 1].active = true;
              Game1.tile[index1 - 1, index2 - 1].type = (byte) 5;
              int num8 = Game1.rand.Next(3);
              if (num8 == 0)
              {
                Game1.tile[index1 - 1, index2 - 1].frameX = (short) 44;
                Game1.tile[index1 - 1, index2 - 1].frameY = (short) 132;
              }
              if (num8 == 1)
              {
                Game1.tile[index1 - 1, index2 - 1].frameX = (short) 44;
                Game1.tile[index1 - 1, index2 - 1].frameY = (short) 154;
              }
              if (num8 == 2)
              {
                Game1.tile[index1 - 1, index2 - 1].frameX = (short) 44;
                Game1.tile[index1 - 1, index2 - 1].frameY = (short) 176;
              }
            }
            int num9 = Game1.rand.Next(3);
            switch (num6)
            {
              case 0:
                if (num9 == 0)
                {
                  Game1.tile[index1, index2 - 1].frameX = (short) 88;
                  Game1.tile[index1, index2 - 1].frameY = (short) 132;
                }
                if (num9 == 1)
                {
                  Game1.tile[index1, index2 - 1].frameX = (short) 88;
                  Game1.tile[index1, index2 - 1].frameY = (short) 154;
                }
                if (num9 == 2)
                {
                  Game1.tile[index1, index2 - 1].frameX = (short) 88;
                  Game1.tile[index1, index2 - 1].frameY = (short) 176;
                  break;
                }
                break;
              case 1:
                if (num9 == 0)
                {
                  Game1.tile[index1, index2 - 1].frameX = (short) 0;
                  Game1.tile[index1, index2 - 1].frameY = (short) 132;
                }
                if (num9 == 1)
                {
                  Game1.tile[index1, index2 - 1].frameX = (short) 0;
                  Game1.tile[index1, index2 - 1].frameY = (short) 154;
                }
                if (num9 == 2)
                {
                  Game1.tile[index1, index2 - 1].frameX = (short) 0;
                  Game1.tile[index1, index2 - 1].frameY = (short) 176;
                  break;
                }
                break;
              case 2:
                if (num9 == 0)
                {
                  Game1.tile[index1, index2 - 1].frameX = (short) 66;
                  Game1.tile[index1, index2 - 1].frameY = (short) 132;
                }
                if (num9 == 1)
                {
                  Game1.tile[index1, index2 - 1].frameX = (short) 66;
                  Game1.tile[index1, index2 - 1].frameY = (short) 154;
                }
                if (num9 == 2)
                {
                  Game1.tile[index1, index2 - 1].frameX = (short) 66;
                  Game1.tile[index1, index2 - 1].frameY = (short) 176;
                }
                break;
            }
            if (Game1.rand.Next(3) < 2)
            {
              int num10 = Game1.rand.Next(3);
              if (num10 == 0)
              {
                Game1.tile[index1, index2 - num1].frameX = (short) 22;
                Game1.tile[index1, index2 - num1].frameY = (short) 198;
              }
              if (num10 == 1)
              {
                Game1.tile[index1, index2 - num1].frameX = (short) 22;
                Game1.tile[index1, index2 - num1].frameY = (short) 220;
              }
              if (num10 == 2)
              {
                Game1.tile[index1, index2 - num1].frameX = (short) 22;
                Game1.tile[index1, index2 - num1].frameY = (short) 242;
              }
            }
            else
            {
              int num11 = Game1.rand.Next(3);
              if (num11 == 0)
              {
                Game1.tile[index1, index2 - num1].frameX = (short) 0;
                Game1.tile[index1, index2 - num1].frameY = (short) 198;
              }
              if (num11 == 1)
              {
                Game1.tile[index1, index2 - num1].frameX = (short) 0;
                Game1.tile[index1, index2 - num1].frameY = (short) 220;
              }
              if (num11 == 2)
              {
                Game1.tile[index1, index2 - num1].frameX = (short) 0;
                Game1.tile[index1, index2 - num1].frameY = (short) 242;
              }
            }
          }
        }
      }
    }

    public static bool EmptyTileCheck(int startX, int endX, int startY, int endY)
    {
      if (startX < 0 || endX >= 5001 || startY < 0 || endY >= 2501)
        return false;
      for (int index1 = startX; index1 < endX + 1; ++index1)
      {
        for (int index2 = startY; index2 < endY + 1; ++index2)
        {
          if (Game1.tile[index1, index2].active)
            return false;
        }
      }
      return true;
    }

    public static bool PlaceDoor(int i, int j, int type)
    {
      try
      {
        if (!Game1.tile[i, j - 2].active || !Game1.tileSolid[(int) Game1.tile[i, j - 2].type] || !Game1.tile[i, j + 2].active || !Game1.tileSolid[(int) Game1.tile[i, j + 2].type])
          return false;
        Game1.tile[i, j - 1].active = true;
        Game1.tile[i, j - 1].type = (byte) 10;
        Game1.tile[i, j - 1].frameY = (short) 0;
        Game1.tile[i, j - 1].frameX = (short) (Game1.rand.Next(3) * 18);
        Game1.tile[i, j].active = true;
        Game1.tile[i, j].type = (byte) 10;
        Game1.tile[i, j].frameY = (short) 18;
        Game1.tile[i, j].frameX = (short) (Game1.rand.Next(3) * 18);
        Game1.tile[i, j + 1].active = true;
        Game1.tile[i, j + 1].type = (byte) 10;
        Game1.tile[i, j + 1].frameY = (short) 36;
        Game1.tile[i, j + 1].frameX = (short) (Game1.rand.Next(3) * 18);
        return true;
      }
      catch
      {
        return false;
      }
    }

    public static void CloseDoor(int i, int j)
    {
      int num1 = 0;
      int num2 = i;
      int num3 = j;
      int frameX = (int) Game1.tile[i, j].frameX;
      int frameY = (int) Game1.tile[i, j].frameY;
      switch (frameX)
      {
        case 0:
          num2 = i;
          num1 = 1;
          break;
        case 18:
          num2 = i - 1;
          num1 = 1;
          break;
        case 36:
          num2 = i + 1;
          num1 = -1;
          break;
        case 54:
          num2 = i;
          num1 = -1;
          break;
      }
      switch (frameY)
      {
        case 0:
          num3 = j;
          break;
        case 18:
          num3 = j - 1;
          break;
        case 36:
          num3 = j - 2;
          break;
      }
      int num4 = num2;
      if (num1 == -1)
        num4 = num2 - 1;
      for (int index1 = num4; index1 < num4 + 2; ++index1)
      {
        for (int index2 = num3; index2 < num3 + 3; ++index2)
        {
          if (index1 == num2)
          {
            Game1.tile[index1, index2].type = (byte) 10;
            Game1.tile[index1, index2].frameX = (short) (Game1.rand.Next(3) * 18);
          }
          else
            Game1.tile[index1, index2].active = false;
        }
      }
      Game1.soundInstanceDoorClosed.Stop();
      Game1.soundInstanceDoorClosed = Game1.soundDoorClosed.CreateInstance();
      Game1.soundInstanceDoorClosed.Play();
    }

    public static void OpenDoor(int i, int j, int direction)
    {
      int index1 = Game1.tile[i, j - 1].frameY != (short) 0 || (int) Game1.tile[i, j - 1].type != (int) Game1.tile[i, j].type ? (Game1.tile[i, j - 2].frameY != (short) 0 || (int) Game1.tile[i, j - 2].type != (int) Game1.tile[i, j].type ? (Game1.tile[i, j + 1].frameY != (short) 0 || (int) Game1.tile[i, j + 1].type != (int) Game1.tile[i, j].type ? j : j + 1) : j - 2) : j - 1;
      short num = 0;
      int index2;
      int i1;
      if (direction == -1)
      {
        index2 = i - 1;
        num = (short) 36;
        i1 = i - 1;
      }
      else
      {
        index2 = i;
        i1 = i + 1;
      }
      bool flag = true;
      for (int j1 = index1; j1 < index1 + 3; ++j1)
      {
        if (Game1.tile[i1, j1].active)
        {
          if (Game1.tile[i1, j1].type == (byte) 3)
          {
            WorldGen.KillTile(i1, j1);
          }
          else
          {
            flag = false;
            break;
          }
        }
      }
      if (!flag)
        return;
      Game1.soundInstanceDoorOpen.Stop();
      Game1.soundInstanceDoorOpen = Game1.soundDoorOpen.CreateInstance();
      Game1.soundInstanceDoorOpen.Play();
      Game1.tile[index2, index1].active = true;
      Game1.tile[index2, index1].type = (byte) 11;
      Game1.tile[index2, index1].frameY = (short) 0;
      Game1.tile[index2, index1].frameX = num;
      Game1.tile[index2 + 1, index1].active = true;
      Game1.tile[index2 + 1, index1].type = (byte) 11;
      Game1.tile[index2 + 1, index1].frameY = (short) 0;
      Game1.tile[index2 + 1, index1].frameX = (short) ((int) num + 18);
      Game1.tile[index2, index1 + 1].active = true;
      Game1.tile[index2, index1 + 1].type = (byte) 11;
      Game1.tile[index2, index1 + 1].frameY = (short) 18;
      Game1.tile[index2, index1 + 1].frameX = num;
      Game1.tile[index2 + 1, index1 + 1].active = true;
      Game1.tile[index2 + 1, index1 + 1].type = (byte) 11;
      Game1.tile[index2 + 1, index1 + 1].frameY = (short) 18;
      Game1.tile[index2 + 1, index1 + 1].frameX = (short) ((int) num + 18);
      Game1.tile[index2, index1 + 2].active = true;
      Game1.tile[index2, index1 + 2].type = (byte) 11;
      Game1.tile[index2, index1 + 2].frameY = (short) 36;
      Game1.tile[index2, index1 + 2].frameX = num;
      Game1.tile[index2 + 1, index1 + 2].active = true;
      Game1.tile[index2 + 1, index1 + 2].type = (byte) 11;
      Game1.tile[index2 + 1, index1 + 2].frameY = (short) 36;
      Game1.tile[index2 + 1, index1 + 2].frameX = (short) ((int) num + 18);
    }

    public static void PlaceTile(int i, int j, int type, bool mute = false)
    {
      if (i < 0 || j < 0 || i >= 5001 || j >= 2501 || !Collision.EmptyTile(i, j))
        return;
      Game1.tile[i, j].frameY = (short) 0;
      Game1.tile[i, j].frameX = (short) 0;
      switch (type)
      {
        case 3:
          if (j + 1 < 2501 && Game1.tile[i, j + 1].type == (byte) 2 && Game1.tile[i, j + 1].active)
          {
            if (Game1.rand.Next(50) == 0)
            {
              Game1.tile[i, j].active = true;
              Game1.tile[i, j].type = (byte) 3;
              Game1.tile[i, j].frameX = (short) 144;
            }
            else if (Game1.rand.Next(35) == 0)
            {
              Game1.tile[i, j].active = true;
              Game1.tile[i, j].type = (byte) 3;
              Game1.tile[i, j].frameX = (short) (Game1.rand.Next(2) * 18 + 108);
            }
            else
            {
              Game1.tile[i, j].active = true;
              Game1.tile[i, j].type = (byte) 3;
              Game1.tile[i, j].frameX = (short) (Game1.rand.Next(6) * 18);
            }
            break;
          }
          break;
        case 4:
          if (Game1.tile[i - 1, j].active && (Game1.tileSolid[(int) Game1.tile[i - 1, j].type] || Game1.tile[i - 1, j].type == (byte) 5 && Game1.tile[i - 1, j - 1].type == (byte) 5 && Game1.tile[i - 1, j + 1].type == (byte) 5) || Game1.tile[i + 1, j].active && (Game1.tileSolid[(int) Game1.tile[i + 1, j].type] || Game1.tile[i + 1, j].type == (byte) 5 && Game1.tile[i + 1, j - 1].type == (byte) 5 && Game1.tile[i + 1, j + 1].type == (byte) 5) || Game1.tile[i, j + 1].active && Game1.tileSolid[(int) Game1.tile[i, j + 1].type])
          {
            Game1.tile[i, j].active = true;
            Game1.tile[i, j].type = (byte) type;
            WorldGen.SquareTileFrame(i, j);
            break;
          }
          break;
        case 10:
          if (!Game1.tile[i, j - 1].active && !Game1.tile[i, j - 2].active && Game1.tile[i, j - 3].active && Game1.tileSolid[(int) Game1.tile[i, j - 3].type])
          {
            WorldGen.PlaceDoor(i, j - 1, type);
            WorldGen.SquareTileFrame(i, j);
            break;
          }
          if (Game1.tile[i, j + 1].active || Game1.tile[i, j + 2].active || !Game1.tile[i, j + 3].active || !Game1.tileSolid[(int) Game1.tile[i, j + 3].type])
            return;
          WorldGen.PlaceDoor(i, j + 1, type);
          WorldGen.SquareTileFrame(i, j);
          break;
        default:
          Game1.tile[i, j].active = true;
          Game1.tile[i, j].type = (byte) type;
          WorldGen.SquareTileFrame(i, j);
          break;
      }
      if (Game1.tile[i, j].active && !mute)
      {
        int index = Game1.rand.Next(3);
        Game1.soundInstanceDig[index].Stop();
        Game1.soundInstanceDig[index] = Game1.soundDig[index].CreateInstance();
        Game1.soundInstanceDig[index].Play();
      }
    }

    public static void KillWall(int i, int j, bool fail = false)
    {
      if (i < 0 || j < 0 || i >= 5001 || j >= 2501 || Game1.tile[i, j].wall <= (byte) 0)
        return;
      int index1 = Game1.rand.Next(3);
      Game1.soundInstanceDig[index1].Stop();
      Game1.soundInstanceDig[index1] = Game1.soundDig[index1].CreateInstance();
      Game1.soundInstanceDig[index1].Play();
      int num = 10;
      if (fail)
        num = 3;
      for (int index2 = 0; index2 < num; ++index2)
      {
        int Type = 0;
        if (Game1.tile[i, j].wall == (byte) 1)
          Type = 1;
        Dust.NewDust(new Vector2((float) (i * 16), (float) (j * 16)), 16, 16, Type);
      }
      if (fail)
      {
        WorldGen.SquareWallFrame(i, j);
      }
      else
      {
        int Type = 0;
        if (Game1.tile[i, j].wall == (byte) 1)
          Type = 26;
        if (Type > 0)
          Item.NewItem(i * 16, j * 16, 16, 16, Type);
        Game1.tile[i, j].wall = (byte) 0;
        WorldGen.SquareWallFrame(i, j);
      }
    }

    public static void KillTile(int i, int j, bool fail = false)
    {
      if (i < 0 || j < 0 || i >= 5001 || j >= 2501 || !Game1.tile[i, j].active || j >= 1 && Game1.tile[i, j - 1].active && Game1.tile[i, j - 1].type == (byte) 5 && Game1.tile[i, j].type != (byte) 5 && (Game1.tile[i, j - 1].frameX != (short) 66 || Game1.tile[i, j - 1].frameY < (short) 0 || Game1.tile[i, j - 1].frameY > (short) 44) && (Game1.tile[i, j - 1].frameX != (short) 88 || Game1.tile[i, j - 1].frameY < (short) 66 || Game1.tile[i, j - 1].frameY > (short) 110) && Game1.tile[i, j - 1].frameY < (short) 198)
        return;
      if (Game1.tile[i, j].type == (byte) 3)
      {
        Game1.soundInstanceGrass.Stop();
        Game1.soundInstanceGrass = Game1.soundGrass.CreateInstance();
        Game1.soundInstanceGrass.Play();
        if (Game1.tile[i, j].frameX == (short) 144)
          Item.NewItem(i * 16, j * 16, 16, 16, 5);
      }
      else
      {
        int index = Game1.rand.Next(3);
        Game1.soundInstanceDig[index].Stop();
        Game1.soundInstanceDig[index] = Game1.soundDig[index].CreateInstance();
        Game1.soundInstanceDig[index].Play();
      }
      int num = 10;
      if (fail)
        num = 3;
      for (int index = 0; index < num; ++index)
      {
        int Type = 0;
        if (Game1.tile[i, j].type == (byte) 0)
          Type = 0;
        if (Game1.tile[i, j].type == (byte) 1)
          Type = 1;
        if (Game1.tile[i, j].type == (byte) 4)
          Type = 6;
        if (Game1.tile[i, j].type == (byte) 5 || Game1.tile[i, j].type == (byte) 10 || Game1.tile[i, j].type == (byte) 11)
          Type = 7;
        if (Game1.tile[i, j].type == (byte) 2)
          Type = Game1.rand.Next(2) != 0 ? 2 : 0;
        if (Game1.tile[i, j].type == (byte) 6)
          Type = 8;
        if (Game1.tile[i, j].type == (byte) 7)
          Type = 9;
        if (Game1.tile[i, j].type == (byte) 8)
          Type = 10;
        if (Game1.tile[i, j].type == (byte) 9)
          Type = 11;
        if (Game1.tile[i, j].type == (byte) 3)
          Type = 3;
        Dust.NewDust(new Vector2((float) (i * 16), (float) (j * 16)), 16, 16, Type);
      }
      if (fail)
      {
        if (Game1.tile[i, j].type == (byte) 2)
          Game1.tile[i, j].type = (byte) 0;
        WorldGen.SquareTileFrame(i, j);
      }
      else
      {
        int Type = 0;
        if (Game1.tile[i, j].type == (byte) 0 || Game1.tile[i, j].type == (byte) 2)
          Type = 2;
        else if (Game1.tile[i, j].type == (byte) 1)
          Type = 3;
        else if (Game1.tile[i, j].type == (byte) 4)
          Type = 8;
        else if (Game1.tile[i, j].type == (byte) 5)
          Type = 9;
        else if (Game1.tile[i, j].type == (byte) 6)
          Type = 11;
        else if (Game1.tile[i, j].type == (byte) 7)
          Type = 12;
        else if (Game1.tile[i, j].type == (byte) 8)
          Type = 13;
        else if (Game1.tile[i, j].type == (byte) 9)
          Type = 14;
        if (Type > 0)
          Item.NewItem(i * 16, j * 16, 16, 16, Type);
        Game1.tile[i, j].active = false;
        Game1.tile[i, j].lighted = false;
        Game1.tile[i, j].frameX = (short) -1;
        Game1.tile[i, j].frameY = (short) -1;
        Game1.tile[i, j].frameNumber = (byte) 0;
        Game1.tile[i, j].type = (byte) 0;
        WorldGen.SquareTileFrame(i, j);
      }
    }

    public static void UpdateWorld()
    {
      float num1 = 0.0002f;
      for (int index1 = 0; (double) index1 < 12507501.0 * (double) num1; ++index1)
      {
        int i1 = Game1.rand.Next(5001);
        int index2 = Game1.rand.Next((int) Game1.worldSurface - 1);
        int num2 = i1 - 1;
        int num3 = i1 + 2;
        int j1 = index2 - 1;
        int num4 = index2 + 2;
        if (num2 < 0)
          num2 = 0;
        if (num3 > 5001)
          num3 = 5001;
        if (j1 < 0)
          j1 = 0;
        if (num4 > 2501)
          num4 = 2501;
        if (Game1.tile[i1, index2].active && Game1.tile[i1, index2].type == (byte) 2)
        {
          if (!Game1.tile[i1, j1].active && Game1.rand.Next(10) == 0)
            WorldGen.PlaceTile(i1, j1, 3, true);
          for (int i2 = num2; i2 < num3; ++i2)
          {
            for (int j2 = j1; j2 < num4; ++j2)
            {
              if ((i1 != i2 || index2 != j2) && Game1.tile[i2, j2].active && Game1.tile[i2, j2].type == (byte) 0)
              {
                WorldGen.SpreadGrass(i2, j2, repeat: false);
                if (Game1.tile[i2, j2].type == (byte) 2)
                  WorldGen.SquareTileFrame(i2, j2);
              }
            }
          }
        }
      }
    }

    public static void PlaceWall(int i, int j, int type, bool mute = false)
    {
      if ((int) Game1.tile[i, j].wall == type)
        return;
      Game1.tile[i, j].wall = (byte) type;
      WorldGen.SquareWallFrame(i, j);
      if (!mute)
      {
        int index = Game1.rand.Next(3);
        Game1.soundInstanceDig[index].Stop();
        Game1.soundInstanceDig[index] = Game1.soundDig[index].CreateInstance();
        Game1.soundInstanceDig[index].Play();
      }
    }

    public static void AddPlants()
    {
      for (int i = 0; i < 5001; ++i)
      {
        for (int index = 1; index < 2501; ++index)
        {
          if (Game1.tile[i, index].type == (byte) 2 && Game1.tile[i, index].active && !Game1.tile[i, index - 1].active)
            WorldGen.PlaceTile(i, index - 1, 3, true);
        }
      }
    }

    public static void SpreadGrass(int i, int j, int dirt = 0, int grass = 2, bool repeat = true)
    {
      if ((int) Game1.tile[i, j].type != dirt || !Game1.tile[i, j].active || (double) j >= Game1.worldSurface)
        return;
      int num1 = i - 1;
      int num2 = i + 2;
      int num3 = j - 1;
      int num4 = j + 2;
      if (num1 < 0)
        num1 = 0;
      if (num2 > 5001)
        num2 = 5001;
      if (num3 < 0)
        num3 = 0;
      if (num4 > 2501)
        num4 = 2501;
      bool flag = true;
      for (int index1 = num1; index1 < num2; ++index1)
      {
        for (int index2 = num3; index2 < num4; ++index2)
        {
          if (!Game1.tile[index1, index2].active)
          {
            flag = false;
            break;
          }
        }
      }
      if (flag)
        return;
      Game1.tile[i, j].type = (byte) grass;
      for (int i1 = num1; i1 < num2; ++i1)
      {
        for (int j1 = num3; j1 < num4; ++j1)
        {
          if (Game1.tile[i1, j1].active && (int) Game1.tile[i1, j1].type == dirt && repeat)
            WorldGen.SpreadGrass(i1, j1, dirt, grass);
        }
      }
    }

    public static void TileRunner(
      int i,
      int j,
      double strength,
      int steps,
      int type,
      bool addTile = false,
      float speedX = 0.0f,
      float speedY = 0.0f)
    {
      double num1 = strength;
      float num2 = (float) steps;
      Vector2 vector2_1;
      vector2_1.X = (float) i;
      vector2_1.Y = (float) j;
      Vector2 vector2_2;
      vector2_2.X = (float) Game1.rand.Next(-10, 11) * 0.1f;
      vector2_2.Y = (float) Game1.rand.Next(-10, 11) * 0.1f;
      if ((double) speedX != 0.0 || (double) speedY != 0.0)
      {
        vector2_2.X = speedX;
        vector2_2.Y = speedY;
      }
      while (num1 > 0.0 && (double) num2 > 0.0)
      {
        num1 = strength * ((double) num2 / (double) steps);
        --num2;
        int num3 = (int) ((double) vector2_1.X - num1 * 0.5);
        int num4 = (int) ((double) vector2_1.X + num1 * 0.5);
        int num5 = (int) ((double) vector2_1.Y - num1 * 0.5);
        int num6 = (int) ((double) vector2_1.Y + num1 * 0.5);
        if (num3 < 0)
          num3 = 0;
        if (num4 > 5001)
          num4 = 5001;
        if (num5 < 0)
          num5 = 0;
        if (num6 > 2501)
          num6 = 2501;
        for (int index1 = num3; index1 < num4; ++index1)
        {
          for (int index2 = num5; index2 < num6; ++index2)
          {
            if ((double) Math.Abs((float) index1 - vector2_1.X) + (double) Math.Abs((float) index2 - vector2_1.Y) < strength * 0.5 * (1.0 + (double) Game1.rand.Next(-10, 11) * 0.015))
            {
              if (type == -1)
              {
                Game1.tile[index1, index2].active = false;
              }
              else
              {
                if (addTile)
                  Game1.tile[index1, index2].active = true;
                Game1.tile[index1, index2].type = (byte) type;
              }
            }
          }
        }
        vector2_1 += vector2_2;
        vector2_2.X += (float) Game1.rand.Next(-10, 11) * 0.05f;
        vector2_2.Y += (float) Game1.rand.Next(-10, 11) * 0.05f;
        if ((double) vector2_2.X > 1.0)
          vector2_2.X = 1f;
        if ((double) vector2_2.X < -1.0)
          vector2_2.X = -1f;
        if ((double) vector2_2.Y > 1.0)
          vector2_2.Y = 1f;
        if ((double) vector2_2.Y < -1.0)
          vector2_2.Y = -1f;
      }
    }

    public static void SquareTileFrame(int i, int j, bool resetFrame = true)
    {
      WorldGen.TileFrame(i - 1, j - 1);
      WorldGen.TileFrame(i - 1, j);
      WorldGen.TileFrame(i - 1, j + 1);
      WorldGen.TileFrame(i, j - 1);
      WorldGen.TileFrame(i, j, resetFrame);
      WorldGen.TileFrame(i, j + 1);
      WorldGen.TileFrame(i + 1, j - 1);
      WorldGen.TileFrame(i + 1, j);
      WorldGen.TileFrame(i + 1, j + 1);
    }

    public static void SquareWallFrame(int i, int j, bool resetFrame = true)
    {
      WorldGen.WallFrame(i - 1, j - 1);
      WorldGen.WallFrame(i - 1, j);
      WorldGen.WallFrame(i - 1, j + 1);
      WorldGen.WallFrame(i, j - 1);
      WorldGen.WallFrame(i, j, resetFrame);
      WorldGen.WallFrame(i, j + 1);
      WorldGen.WallFrame(i + 1, j - 1);
      WorldGen.WallFrame(i + 1, j);
      WorldGen.WallFrame(i + 1, j + 1);
    }

    public static void EveryTileFrame()
    {
      for (int i = 0; i < 5001; ++i)
      {
        for (int j = 0; j < 2501; ++j)
          WorldGen.TileFrame(i, j, true);
      }
    }

    public static void PlantCheck(int i, int j)
    {
      int num1 = -1;
      int num2 = -1;
      int num3 = -1;
      int num4 = -1;
      int num5 = -1;
      int num6 = -1;
      int num7 = -1;
      int num8 = -1;
      int type = (int) Game1.tile[i, j].type;
      if (i - 1 < 0)
      {
        num1 = type;
        num4 = type;
        num6 = type;
      }
      if (i + 1 >= 5001)
      {
        num3 = type;
        num5 = type;
        num8 = type;
      }
      if (j - 1 < 0)
      {
        num1 = type;
        num2 = type;
        num3 = type;
      }
      if (j + 1 >= 2501)
      {
        num6 = type;
        num7 = type;
        num8 = type;
      }
      if (i - 1 >= 0 && Game1.tile[i - 1, j].active)
        num4 = (int) Game1.tile[i - 1, j].type;
      if (i + 1 < 5001 && Game1.tile[i + 1, j].active)
        num5 = (int) Game1.tile[i + 1, j].type;
      if (j - 1 >= 0 && Game1.tile[i, j - 1].active)
        num2 = (int) Game1.tile[i, j - 1].type;
      if (j + 1 < 2501 && Game1.tile[i, j + 1].active)
        num7 = (int) Game1.tile[i, j + 1].type;
      if (i - 1 >= 0 && j - 1 >= 0 && Game1.tile[i - 1, j - 1].active)
        num1 = (int) Game1.tile[i - 1, j - 1].type;
      if (i + 1 < 5001 && j - 1 >= 0 && Game1.tile[i + 1, j - 1].active)
        num3 = (int) Game1.tile[i + 1, j - 1].type;
      if (i - 1 >= 0 && j + 1 < 2501 && Game1.tile[i - 1, j + 1].active)
        num6 = (int) Game1.tile[i - 1, j + 1].type;
      if (i + 1 < 5001 && j + 1 < 2501 && Game1.tile[i + 1, j + 1].active)
        num8 = (int) Game1.tile[i + 1, j + 1].type;
      if (num7 == 2)
        return;
      WorldGen.KillTile(i, j);
    }

    public static void WallFrame(int i, int j, bool resetFrame = false)
    {
      int num1 = -1;
      int num2 = -1;
      int num3 = -1;
      int num4 = -1;
      int num5 = -1;
      int num6 = -1;
      int num7 = -1;
      int num8 = -1;
      int wall = (int) Game1.tile[i, j].wall;
      if (wall == 0)
        return;
      int wallFrameX = (int) Game1.tile[i, j].wallFrameX;
      int wallFrameY = (int) Game1.tile[i, j].wallFrameY;
      Rectangle rectangle;
      rectangle.X = -1;
      rectangle.Y = -1;
      if (i - 1 < 0)
      {
        num1 = wall;
        num4 = wall;
        num6 = wall;
      }
      if (i + 1 >= 5001)
      {
        num3 = wall;
        num5 = wall;
        num8 = wall;
      }
      if (j - 1 < 0)
      {
        num1 = wall;
        num2 = wall;
        num3 = wall;
      }
      if (j + 1 >= 2501)
      {
        num6 = wall;
        num7 = wall;
        num8 = wall;
      }
      if (i - 1 >= 0)
        num4 = (int) Game1.tile[i - 1, j].wall;
      if (i + 1 < 5001)
        num5 = (int) Game1.tile[i + 1, j].wall;
      if (j - 1 >= 0)
        num2 = (int) Game1.tile[i, j - 1].wall;
      if (j + 1 < 2501)
        num7 = (int) Game1.tile[i, j + 1].wall;
      if (i - 1 >= 0 && j - 1 >= 0)
        num1 = (int) Game1.tile[i - 1, j - 1].wall;
      if (i + 1 < 5001 && j - 1 >= 0)
        num3 = (int) Game1.tile[i + 1, j - 1].wall;
      if (i - 1 >= 0 && j + 1 < 2501)
        num6 = (int) Game1.tile[i - 1, j + 1].wall;
      if (i + 1 < 5001 && j + 1 < 2501)
        num8 = (int) Game1.tile[i + 1, j + 1].wall;
      int num9;
      if (resetFrame)
      {
        num9 = Game1.rand.Next(0, 3);
        Game1.tile[i, j].wallFrameNumber = (byte) num9;
      }
      else
        num9 = (int) Game1.tile[i, j].wallFrameNumber;
      if (rectangle.X < 0 || rectangle.Y < 0)
      {
        if (num2 == wall && num7 == wall && num4 == wall & num5 == wall)
        {
          if (num1 != wall && num3 != wall)
          {
            if (num9 == 0)
            {
              rectangle.X = 108;
              rectangle.Y = 18;
            }
            if (num9 == 1)
            {
              rectangle.X = 126;
              rectangle.Y = 18;
            }
            if (num9 == 2)
            {
              rectangle.X = 144;
              rectangle.Y = 18;
            }
          }
          else if (num6 != wall && num8 != wall)
          {
            if (num9 == 0)
            {
              rectangle.X = 108;
              rectangle.Y = 36;
            }
            if (num9 == 1)
            {
              rectangle.X = 126;
              rectangle.Y = 36;
            }
            if (num9 == 2)
            {
              rectangle.X = 144;
              rectangle.Y = 36;
            }
          }
          else if (num1 != wall && num6 != wall)
          {
            if (num9 == 0)
            {
              rectangle.X = 180;
              rectangle.Y = 0;
            }
            if (num9 == 1)
            {
              rectangle.X = 180;
              rectangle.Y = 18;
            }
            if (num9 == 2)
            {
              rectangle.X = 180;
              rectangle.Y = 36;
            }
          }
          else if (num3 != wall && num8 != wall)
          {
            if (num9 == 0)
            {
              rectangle.X = 198;
              rectangle.Y = 0;
            }
            if (num9 == 1)
            {
              rectangle.X = 198;
              rectangle.Y = 18;
            }
            if (num9 == 2)
            {
              rectangle.X = 198;
              rectangle.Y = 36;
            }
          }
          else
          {
            if (num9 == 0)
            {
              rectangle.X = 18;
              rectangle.Y = 18;
            }
            if (num9 == 1)
            {
              rectangle.X = 36;
              rectangle.Y = 18;
            }
            if (num9 == 2)
            {
              rectangle.X = 54;
              rectangle.Y = 18;
            }
          }
        }
        else if (num2 != wall && num7 == wall && num4 == wall & num5 == wall)
        {
          if (num9 == 0)
          {
            rectangle.X = 18;
            rectangle.Y = 0;
          }
          if (num9 == 1)
          {
            rectangle.X = 36;
            rectangle.Y = 0;
          }
          if (num9 == 2)
          {
            rectangle.X = 54;
            rectangle.Y = 0;
          }
        }
        else if (num2 == wall && num7 != wall && num4 == wall & num5 == wall)
        {
          if (num9 == 0)
          {
            rectangle.X = 18;
            rectangle.Y = 36;
          }
          if (num9 == 1)
          {
            rectangle.X = 36;
            rectangle.Y = 36;
          }
          if (num9 == 2)
          {
            rectangle.X = 54;
            rectangle.Y = 36;
          }
        }
        else if (num2 == wall && num7 == wall && num4 != wall & num5 == wall)
        {
          if (num9 == 0)
          {
            rectangle.X = 0;
            rectangle.Y = 0;
          }
          if (num9 == 1)
          {
            rectangle.X = 0;
            rectangle.Y = 18;
          }
          if (num9 == 2)
          {
            rectangle.X = 0;
            rectangle.Y = 36;
          }
        }
        else if (num2 == wall && num7 == wall && num4 == wall & num5 != wall)
        {
          if (num9 == 0)
          {
            rectangle.X = 72;
            rectangle.Y = 0;
          }
          if (num9 == 1)
          {
            rectangle.X = 72;
            rectangle.Y = 18;
          }
          if (num9 == 2)
          {
            rectangle.X = 72;
            rectangle.Y = 36;
          }
        }
        else if (num2 != wall && num7 == wall && num4 != wall & num5 == wall)
        {
          if (num9 == 0)
          {
            rectangle.X = 0;
            rectangle.Y = 54;
          }
          if (num9 == 1)
          {
            rectangle.X = 36;
            rectangle.Y = 54;
          }
          if (num9 == 2)
          {
            rectangle.X = 72;
            rectangle.Y = 54;
          }
        }
        else if (num2 != wall && num7 == wall && num4 == wall & num5 != wall)
        {
          if (num9 == 0)
          {
            rectangle.X = 18;
            rectangle.Y = 54;
          }
          if (num9 == 1)
          {
            rectangle.X = 54;
            rectangle.Y = 54;
          }
          if (num9 == 2)
          {
            rectangle.X = 90;
            rectangle.Y = 54;
          }
        }
        else if (num2 == wall && num7 != wall && num4 != wall & num5 == wall)
        {
          if (num9 == 0)
          {
            rectangle.X = 0;
            rectangle.Y = 72;
          }
          if (num9 == 1)
          {
            rectangle.X = 36;
            rectangle.Y = 72;
          }
          if (num9 == 2)
          {
            rectangle.X = 72;
            rectangle.Y = 72;
          }
        }
        else if (num2 == wall && num7 != wall && num4 == wall & num5 != wall)
        {
          if (num9 == 0)
          {
            rectangle.X = 18;
            rectangle.Y = 72;
          }
          if (num9 == 1)
          {
            rectangle.X = 54;
            rectangle.Y = 72;
          }
          if (num9 == 2)
          {
            rectangle.X = 90;
            rectangle.Y = 72;
          }
        }
        else if (num2 == wall && num7 == wall && num4 != wall & num5 != wall)
        {
          if (num9 == 0)
          {
            rectangle.X = 90;
            rectangle.Y = 0;
          }
          if (num9 == 1)
          {
            rectangle.X = 90;
            rectangle.Y = 18;
          }
          if (num9 == 2)
          {
            rectangle.X = 90;
            rectangle.Y = 36;
          }
        }
        else if (num2 != wall && num7 != wall && num4 == wall & num5 == wall)
        {
          if (num9 == 0)
          {
            rectangle.X = 108;
            rectangle.Y = 72;
          }
          if (num9 == 1)
          {
            rectangle.X = 126;
            rectangle.Y = 72;
          }
          if (num9 == 2)
          {
            rectangle.X = 144;
            rectangle.Y = 72;
          }
        }
        else if (num2 != wall && num7 == wall && num4 != wall & num5 != wall)
        {
          if (num9 == 0)
          {
            rectangle.X = 108;
            rectangle.Y = 0;
          }
          if (num9 == 1)
          {
            rectangle.X = 126;
            rectangle.Y = 0;
          }
          if (num9 == 2)
          {
            rectangle.X = 144;
            rectangle.Y = 0;
          }
        }
        else if (num2 == wall && num7 != wall && num4 != wall & num5 != wall)
        {
          if (num9 == 0)
          {
            rectangle.X = 108;
            rectangle.Y = 54;
          }
          if (num9 == 1)
          {
            rectangle.X = 126;
            rectangle.Y = 54;
          }
          if (num9 == 2)
          {
            rectangle.X = 144;
            rectangle.Y = 54;
          }
        }
        else if (num2 != wall && num7 != wall && num4 != wall & num5 == wall)
        {
          if (num9 == 0)
          {
            rectangle.X = 162;
            rectangle.Y = 0;
          }
          if (num9 == 1)
          {
            rectangle.X = 162;
            rectangle.Y = 18;
          }
          if (num9 == 2)
          {
            rectangle.X = 162;
            rectangle.Y = 36;
          }
        }
        else if (num2 != wall && num7 != wall && num4 == wall & num5 != wall)
        {
          if (num9 == 0)
          {
            rectangle.X = 216;
            rectangle.Y = 0;
          }
          if (num9 == 1)
          {
            rectangle.X = 216;
            rectangle.Y = 18;
          }
          if (num9 == 2)
          {
            rectangle.X = 216;
            rectangle.Y = 36;
          }
        }
        else if (num2 != wall && num7 != wall && num4 != wall & num5 != wall)
        {
          if (num9 == 0)
          {
            rectangle.X = 162;
            rectangle.Y = 54;
          }
          if (num9 == 1)
          {
            rectangle.X = 180;
            rectangle.Y = 54;
          }
          if (num9 == 2)
          {
            rectangle.X = 198;
            rectangle.Y = 54;
          }
        }
      }
      if (rectangle.X <= -1 || rectangle.Y <= -1)
      {
        if (num9 <= 0)
        {
          rectangle.X = 18;
          rectangle.Y = 18;
        }
        if (num9 == 1)
        {
          rectangle.X = 36;
          rectangle.Y = 18;
        }
        if (num9 >= 2)
        {
          rectangle.X = 54;
          rectangle.Y = 18;
        }
      }
      Game1.tile[i, j].wallFrameX = (byte) rectangle.X;
      Game1.tile[i, j].wallFrameY = (byte) rectangle.Y;
            
      if (rectangle.X == wallFrameX || rectangle.Y == wallFrameY || wallFrameX < 0 || wallFrameY < 0)
      { }
    }

    public static void TileFrame(int i, int j, bool resetFrame = false)
    {
      if (i < 0 || j < 0 || i >= 5001 || j >= 2501 || !Game1.tile[i, j].active)
        return;
      int num1 = -1;
      int num2 = -1;
      int num3 = -1;
      int index1 = -1;
      int index2 = -1;
      int num4 = -1;
      int index3 = -1;
      int num5 = -1;
      int type = (int) Game1.tile[i, j].type;
      int frameX1 = (int) Game1.tile[i, j].frameX;
      int frameY1 = (int) Game1.tile[i, j].frameY;
      Rectangle rectangle;
      rectangle.X = -1;
      rectangle.Y = -1;
      if (type == 3)
      {
        WorldGen.PlantCheck(i, j);
      }
      else
      {
        WorldGen.mergeUp = false;
        WorldGen.mergeDown = false;
        WorldGen.mergeLeft = false;
        WorldGen.mergeRight = false;
        if (i - 1 < 0)
        {
          num1 = type;
          index1 = type;
          num4 = type;
        }
        if (i + 1 >= 5001)
        {
          num3 = type;
          index2 = type;
          num5 = type;
        }
        if (j - 1 < 0)
        {
          num1 = type;
          num2 = type;
          num3 = type;
        }
        if (j + 1 >= 2501)
        {
          num4 = type;
          index3 = type;
          num5 = type;
        }
        if (i - 1 >= 0 && Game1.tile[i - 1, j].active)
          index1 = (int) Game1.tile[i - 1, j].type;
        if (i + 1 < 5001 && Game1.tile[i + 1, j].active)
          index2 = (int) Game1.tile[i + 1, j].type;
        if (j - 1 >= 0 && Game1.tile[i, j - 1].active)
          num2 = (int) Game1.tile[i, j - 1].type;
        if (j + 1 < 2501 && Game1.tile[i, j + 1].active)
          index3 = (int) Game1.tile[i, j + 1].type;
        if (i - 1 >= 0 && j - 1 >= 0 && Game1.tile[i - 1, j - 1].active)
          num1 = (int) Game1.tile[i - 1, j - 1].type;
        if (i + 1 < 5001 && j - 1 >= 0 && Game1.tile[i + 1, j - 1].active)
          num3 = (int) Game1.tile[i + 1, j - 1].type;
        if (i - 1 >= 0 && j + 1 < 2501 && Game1.tile[i - 1, j + 1].active)
          num4 = (int) Game1.tile[i - 1, j + 1].type;
        if (i + 1 < 5001 && j + 1 < 2501 && Game1.tile[i + 1, j + 1].active)
          num5 = (int) Game1.tile[i + 1, j + 1].type;
        switch (type)
        {
          case 4:
            if (index3 >= 0 && Game1.tileSolid[index3])
            {
              Game1.tile[i, j].frameX = (short) 0;
              return;
            }
            if (index1 >= 0 && Game1.tileSolid[index1] || index1 == 5 && num1 == 5 && num4 == 5)
            {
              Game1.tile[i, j].frameX = (short) 22;
              return;
            }
            if (index2 >= 0 && Game1.tileSolid[index2] || index2 == 5 && num3 == 5 && num5 == 5)
            {
              Game1.tile[i, j].frameX = (short) 44;
              return;
            }
            WorldGen.KillTile(i, j);
            return;
          case 5:
            if (index3 != type && index3 != -1)
            {
              if (Game1.tile[i, j].frameX >= (short) 22 && Game1.tile[i, j].frameX <= (short) 44 && Game1.tile[i, j].frameY >= (short) 132 && Game1.tile[i, j].frameY <= (short) 176 && index1 != type && index2 != type)
                WorldGen.KillTile(i, j);
            }
            else if (Game1.tile[i, j].frameX == (short) 88 && Game1.tile[i, j].frameY >= (short) 0 && Game1.tile[i, j].frameY <= (short) 44 || Game1.tile[i, j].frameX == (short) 66 && Game1.tile[i, j].frameY >= (short) 66 && Game1.tile[i, j].frameY <= (short) 130 || Game1.tile[i, j].frameX == (short) 110 && Game1.tile[i, j].frameY >= (short) 66 && Game1.tile[i, j].frameY <= (short) 110 || Game1.tile[i, j].frameX == (short) 132 && Game1.tile[i, j].frameY >= (short) 0 && Game1.tile[i, j].frameY <= (short) 176)
            {
              if (index1 == type && index2 == type)
              {
                if (Game1.tile[i, j].frameNumber == (byte) 0)
                {
                  Game1.tile[i, j].frameX = (short) 110;
                  Game1.tile[i, j].frameY = (short) 66;
                }
                if (Game1.tile[i, j].frameNumber == (byte) 1)
                {
                  Game1.tile[i, j].frameX = (short) 110;
                  Game1.tile[i, j].frameY = (short) 88;
                }
                if (Game1.tile[i, j].frameNumber == (byte) 2)
                {
                  Game1.tile[i, j].frameX = (short) 110;
                  Game1.tile[i, j].frameY = (short) 110;
                }
              }
              else if (index1 == type)
              {
                if (Game1.tile[i, j].frameNumber == (byte) 0)
                {
                  Game1.tile[i, j].frameX = (short) 88;
                  Game1.tile[i, j].frameY = (short) 0;
                }
                if (Game1.tile[i, j].frameNumber == (byte) 1)
                {
                  Game1.tile[i, j].frameX = (short) 88;
                  Game1.tile[i, j].frameY = (short) 22;
                }
                if (Game1.tile[i, j].frameNumber == (byte) 2)
                {
                  Game1.tile[i, j].frameX = (short) 88;
                  Game1.tile[i, j].frameY = (short) 44;
                }
              }
              else if (index2 == type)
              {
                if (Game1.tile[i, j].frameNumber == (byte) 0)
                {
                  Game1.tile[i, j].frameX = (short) 66;
                  Game1.tile[i, j].frameY = (short) 66;
                }
                if (Game1.tile[i, j].frameNumber == (byte) 1)
                {
                  Game1.tile[i, j].frameX = (short) 66;
                  Game1.tile[i, j].frameY = (short) 88;
                }
                if (Game1.tile[i, j].frameNumber == (byte) 2)
                {
                  Game1.tile[i, j].frameX = (short) 66;
                  Game1.tile[i, j].frameY = (short) 110;
                }
              }
              else
              {
                if (Game1.tile[i, j].frameNumber == (byte) 0)
                {
                  Game1.tile[i, j].frameX = (short) 0;
                  Game1.tile[i, j].frameY = (short) 22;
                }
                if (Game1.tile[i, j].frameNumber == (byte) 1)
                {
                  Game1.tile[i, j].frameX = (short) 0;
                  Game1.tile[i, j].frameY = (short) 22;
                }
                if (Game1.tile[i, j].frameNumber == (byte) 2)
                {
                  Game1.tile[i, j].frameX = (short) 0;
                  Game1.tile[i, j].frameY = (short) 22;
                }
              }
            }
            if (Game1.tile[i, j].frameY >= (short) 132 && Game1.tile[i, j].frameY <= (short) 176 && (Game1.tile[i, j].frameX == (short) 0 || Game1.tile[i, j].frameX == (short) 66 || Game1.tile[i, j].frameX == (short) 88))
            {
              if (index1 != type && index2 != type)
              {
                if (Game1.tile[i, j].frameNumber == (byte) 0)
                {
                  Game1.tile[i, j].frameX = (short) 0;
                  Game1.tile[i, j].frameY = (short) 0;
                }
                if (Game1.tile[i, j].frameNumber == (byte) 1)
                {
                  Game1.tile[i, j].frameX = (short) 0;
                  Game1.tile[i, j].frameY = (short) 22;
                }
                if (Game1.tile[i, j].frameNumber == (byte) 2)
                {
                  Game1.tile[i, j].frameX = (short) 0;
                  Game1.tile[i, j].frameY = (short) 44;
                }
              }
              else if (index1 != type)
              {
                if (Game1.tile[i, j].frameNumber == (byte) 0)
                {
                  Game1.tile[i, j].frameX = (short) 0;
                  Game1.tile[i, j].frameY = (short) 132;
                }
                if (Game1.tile[i, j].frameNumber == (byte) 1)
                {
                  Game1.tile[i, j].frameX = (short) 0;
                  Game1.tile[i, j].frameY = (short) 154;
                }
                if (Game1.tile[i, j].frameNumber == (byte) 2)
                {
                  Game1.tile[i, j].frameX = (short) 0;
                  Game1.tile[i, j].frameY = (short) 176;
                }
              }
              else if (index2 != type)
              {
                if (Game1.tile[i, j].frameNumber == (byte) 0)
                {
                  Game1.tile[i, j].frameX = (short) 66;
                  Game1.tile[i, j].frameY = (short) 132;
                }
                if (Game1.tile[i, j].frameNumber == (byte) 1)
                {
                  Game1.tile[i, j].frameX = (short) 66;
                  Game1.tile[i, j].frameY = (short) 154;
                }
                if (Game1.tile[i, j].frameNumber == (byte) 2)
                {
                  Game1.tile[i, j].frameX = (short) 66;
                  Game1.tile[i, j].frameY = (short) 176;
                }
              }
              else
              {
                if (Game1.tile[i, j].frameNumber == (byte) 0)
                {
                  Game1.tile[i, j].frameX = (short) 88;
                  Game1.tile[i, j].frameY = (short) 132;
                }
                if (Game1.tile[i, j].frameNumber == (byte) 1)
                {
                  Game1.tile[i, j].frameX = (short) 88;
                  Game1.tile[i, j].frameY = (short) 154;
                }
                if (Game1.tile[i, j].frameNumber == (byte) 2)
                {
                  Game1.tile[i, j].frameX = (short) 88;
                  Game1.tile[i, j].frameY = (short) 176;
                }
              }
            }
            if (Game1.tile[i, j].frameX == (short) 66 && (Game1.tile[i, j].frameY == (short) 0 || Game1.tile[i, j].frameY == (short) 22 || Game1.tile[i, j].frameY == (short) 44) || Game1.tile[i, j].frameX == (short) 88 && (Game1.tile[i, j].frameY == (short) 66 || Game1.tile[i, j].frameY == (short) 88 || Game1.tile[i, j].frameY == (short) 110) || Game1.tile[i, j].frameX == (short) 44 && (Game1.tile[i, j].frameY == (short) 198 || Game1.tile[i, j].frameY == (short) 220 || Game1.tile[i, j].frameY == (short) 242) || Game1.tile[i, j].frameX == (short) 66 && (Game1.tile[i, j].frameY == (short) 198 || Game1.tile[i, j].frameY == (short) 220 || Game1.tile[i, j].frameY == (short) 242))
            {
              if (index1 != type && index2 != type)
                WorldGen.KillTile(i, j);
            }
            else if (index3 == -1)
              WorldGen.KillTile(i, j);
            else if (num2 != type && Game1.tile[i, j].frameY < (short) 198 && (Game1.tile[i, j].frameX != (short) 22 && Game1.tile[i, j].frameX != (short) 44 || Game1.tile[i, j].frameY < (short) 132))
            {
              if (index1 == type || index2 == type)
              {
                if (index3 == type)
                {
                  if (index1 == type && index2 == type)
                  {
                    if (Game1.tile[i, j].frameNumber == (byte) 0)
                    {
                      Game1.tile[i, j].frameX = (short) 132;
                      Game1.tile[i, j].frameY = (short) 132;
                    }
                    if (Game1.tile[i, j].frameNumber == (byte) 1)
                    {
                      Game1.tile[i, j].frameX = (short) 132;
                      Game1.tile[i, j].frameY = (short) 154;
                    }
                    if (Game1.tile[i, j].frameNumber == (byte) 2)
                    {
                      Game1.tile[i, j].frameX = (short) 132;
                      Game1.tile[i, j].frameY = (short) 176;
                    }
                  }
                  else if (index1 == type)
                  {
                    if (Game1.tile[i, j].frameNumber == (byte) 0)
                    {
                      Game1.tile[i, j].frameX = (short) 132;
                      Game1.tile[i, j].frameY = (short) 0;
                    }
                    if (Game1.tile[i, j].frameNumber == (byte) 1)
                    {
                      Game1.tile[i, j].frameX = (short) 132;
                      Game1.tile[i, j].frameY = (short) 22;
                    }
                    if (Game1.tile[i, j].frameNumber == (byte) 2)
                    {
                      Game1.tile[i, j].frameX = (short) 132;
                      Game1.tile[i, j].frameY = (short) 44;
                    }
                  }
                  else if (index2 == type)
                  {
                    if (Game1.tile[i, j].frameNumber == (byte) 0)
                    {
                      Game1.tile[i, j].frameX = (short) 132;
                      Game1.tile[i, j].frameY = (short) 66;
                    }
                    if (Game1.tile[i, j].frameNumber == (byte) 1)
                    {
                      Game1.tile[i, j].frameX = (short) 132;
                      Game1.tile[i, j].frameY = (short) 88;
                    }
                    if (Game1.tile[i, j].frameNumber == (byte) 2)
                    {
                      Game1.tile[i, j].frameX = (short) 132;
                      Game1.tile[i, j].frameY = (short) 110;
                    }
                  }
                }
                else if (index1 == type && index2 == type)
                {
                  if (Game1.tile[i, j].frameNumber == (byte) 0)
                  {
                    Game1.tile[i, j].frameX = (short) 154;
                    Game1.tile[i, j].frameY = (short) 132;
                  }
                  if (Game1.tile[i, j].frameNumber == (byte) 1)
                  {
                    Game1.tile[i, j].frameX = (short) 154;
                    Game1.tile[i, j].frameY = (short) 154;
                  }
                  if (Game1.tile[i, j].frameNumber == (byte) 2)
                  {
                    Game1.tile[i, j].frameX = (short) 154;
                    Game1.tile[i, j].frameY = (short) 176;
                  }
                }
                else if (index1 == type)
                {
                  if (Game1.tile[i, j].frameNumber == (byte) 0)
                  {
                    Game1.tile[i, j].frameX = (short) 154;
                    Game1.tile[i, j].frameY = (short) 0;
                  }
                  if (Game1.tile[i, j].frameNumber == (byte) 1)
                  {
                    Game1.tile[i, j].frameX = (short) 154;
                    Game1.tile[i, j].frameY = (short) 22;
                  }
                  if (Game1.tile[i, j].frameNumber == (byte) 2)
                  {
                    Game1.tile[i, j].frameX = (short) 154;
                    Game1.tile[i, j].frameY = (short) 44;
                  }
                }
                else if (index2 == type)
                {
                  if (Game1.tile[i, j].frameNumber == (byte) 0)
                  {
                    Game1.tile[i, j].frameX = (short) 154;
                    Game1.tile[i, j].frameY = (short) 66;
                  }
                  if (Game1.tile[i, j].frameNumber == (byte) 1)
                  {
                    Game1.tile[i, j].frameX = (short) 154;
                    Game1.tile[i, j].frameY = (short) 88;
                  }
                  if (Game1.tile[i, j].frameNumber == (byte) 2)
                  {
                    Game1.tile[i, j].frameX = (short) 154;
                    Game1.tile[i, j].frameY = (short) 110;
                  }
                }
              }
              else
              {
                if (Game1.tile[i, j].frameNumber == (byte) 0)
                {
                  Game1.tile[i, j].frameX = (short) 110;
                  Game1.tile[i, j].frameY = (short) 0;
                }
                if (Game1.tile[i, j].frameNumber == (byte) 1)
                {
                  Game1.tile[i, j].frameX = (short) 110;
                  Game1.tile[i, j].frameY = (short) 22;
                }
                if (Game1.tile[i, j].frameNumber == (byte) 2)
                {
                  Game1.tile[i, j].frameX = (short) 110;
                  Game1.tile[i, j].frameY = (short) 44;
                }
              }
            }
            rectangle.X = (int) Game1.tile[i, j].frameX;
            rectangle.Y = (int) Game1.tile[i, j].frameY;
            break;
          case 10:
            if (WorldGen.destroyObject)
              return;
            int frameY2 = (int) Game1.tile[i, j].frameY;
            int j1 = j;
            bool flag1 = false;
            if (frameY2 == 0)
              j1 = j;
            if (frameY2 == 18)
              j1 = j - 1;
            if (frameY2 == 36)
              j1 = j - 2;
            if (!Game1.tile[i, j1 - 1].active || !Game1.tileSolid[(int) Game1.tile[i, j1 - 1].type])
              flag1 = true;
            if (!Game1.tile[i, j1 + 3].active || !Game1.tileSolid[(int) Game1.tile[i, j1 + 3].type])
              flag1 = true;
            if (!Game1.tile[i, j1].active || (int) Game1.tile[i, j1].type != type)
              flag1 = true;
            if (!Game1.tile[i, j1 + 1].active || (int) Game1.tile[i, j1 + 1].type != type)
              flag1 = true;
            if (!Game1.tile[i, j1 + 2].active || (int) Game1.tile[i, j1 + 2].type != type)
              flag1 = true;
            if (flag1)
            {
              WorldGen.destroyObject = true;
              WorldGen.KillTile(i, j1);
              WorldGen.KillTile(i, j1 + 1);
              WorldGen.KillTile(i, j1 + 2);
              Item.NewItem(i * 16, j * 16, 16, 16, 25);
            }
            WorldGen.destroyObject = false;
            return;
          case 11:
            if (WorldGen.destroyObject)
              return;
            int num6 = 0;
            int index4 = i;
            int num7 = j;
            int frameX2 = (int) Game1.tile[i, j].frameX;
            int frameY3 = (int) Game1.tile[i, j].frameY;
            bool flag2 = false;
            switch (frameX2)
            {
              case 0:
                index4 = i;
                num6 = 1;
                break;
              case 18:
                index4 = i - 1;
                num6 = 1;
                break;
              case 36:
                index4 = i + 1;
                num6 = -1;
                break;
              case 54:
                index4 = i;
                num6 = -1;
                break;
            }
            switch (frameY3)
            {
              case 0:
                num7 = j;
                break;
              case 18:
                num7 = j - 1;
                break;
              case 36:
                num7 = j - 2;
                break;
            }
            if (!Game1.tile[index4, num7 - 1].active || !Game1.tileSolid[(int) Game1.tile[index4, num7 - 1].type] || !Game1.tile[index4, num7 + 3].active || !Game1.tileSolid[(int) Game1.tile[index4, num7 + 3].type])
            {
              flag2 = true;
              WorldGen.destroyObject = true;
              Item.NewItem(i * 16, j * 16, 16, 16, 25);
            }
            int num8 = index4;
            if (num6 == -1)
              num8 = index4 - 1;
            for (int i1 = num8; i1 < num8 + 2; ++i1)
            {
              for (int j2 = num7; j2 < num7 + 3; ++j2)
              {
                if (!flag2 && (Game1.tile[i1, j2].type != (byte) 11 || !Game1.tile[i1, j2].active))
                {
                  WorldGen.destroyObject = true;
                  Item.NewItem(i * 16, j * 16, 16, 16, 25);
                  flag2 = true;
                  i1 = num8;
                  j2 = num7;
                }
                if (flag2)
                  WorldGen.KillTile(i1, j2);
              }
            }
            WorldGen.destroyObject = false;
            return;
        }
        int num9;
        if (resetFrame)
        {
          num9 = Game1.rand.Next(0, 3);
          Game1.tile[i, j].frameNumber = (byte) num9;
        }
        else
          num9 = (int) Game1.tile[i, j].frameNumber;
        if (type == 0)
        {
          for (int index5 = 0; index5 < 12; ++index5)
          {
            if (index5 == 1 || index5 == 6 || index5 == 7 || index5 == 8 || index5 == 9)
            {
              if (num2 == index5)
              {
                WorldGen.TileFrame(i, j - 1);
                if (WorldGen.mergeDown)
                  num2 = type;
              }
              if (index3 == index5)
              {
                WorldGen.TileFrame(i, j + 1);
                if (WorldGen.mergeUp)
                  index3 = type;
              }
              if (index1 == index5)
              {
                WorldGen.TileFrame(i - 1, j);
                if (WorldGen.mergeRight)
                  index1 = type;
              }
              if (index2 == index5)
              {
                WorldGen.TileFrame(i + 1, j);
                if (WorldGen.mergeLeft)
                  index2 = type;
              }
              if (num1 == index5)
                num1 = type;
              if (num3 == index5)
                num3 = type;
              if (num4 == index5)
                num4 = type;
              if (num5 == index5)
                num5 = type;
            }
          }
          if (num2 == 2)
            num2 = type;
          if (index3 == 2)
            index3 = type;
          if (index1 == 2)
            index1 = type;
          if (index2 == 2)
            index2 = type;
          if (num1 == 2)
            num1 = type;
          if (num3 == 2)
            num3 = type;
          if (num4 == 2)
            num4 = type;
          if (num5 == 2)
            num5 = type;
        }
        if (type == 1 || type == 6 || type == 7 || type == 8 || type == 9)
        {
          for (int index6 = 0; index6 < 12; ++index6)
          {
            if (index6 == 1 || index6 == 6 || index6 == 7 || index6 == 8 || index6 == 9)
            {
              if (num2 == 0)
                num2 = -2;
              if (index3 == 0)
                index3 = -2;
              if (index1 == 0)
                index1 = -2;
              if (index2 == 0)
                index2 = -2;
              if (num1 == 0)
                num1 = -2;
              if (num3 == 0)
                num3 = -2;
              if (num4 == 0)
                num4 = -2;
              if (num5 == 0)
                num5 = -2;
            }
          }
        }
        if (type == 2)
        {
          int num10 = 0;
          if (num2 != type && num2 != num10 && (index3 == type || index3 == num10))
          {
            if (index1 == num10 && index2 == type)
            {
              if (num9 == 0)
              {
                rectangle.X = 0;
                rectangle.Y = 198;
              }
              if (num9 == 1)
              {
                rectangle.X = 18;
                rectangle.Y = 198;
              }
              if (num9 == 2)
              {
                rectangle.X = 36;
                rectangle.Y = 198;
              }
            }
            else if (index1 == type && index2 == num10)
            {
              if (num9 == 0)
              {
                rectangle.X = 54;
                rectangle.Y = 198;
              }
              if (num9 == 1)
              {
                rectangle.X = 72;
                rectangle.Y = 198;
              }
              if (num9 == 2)
              {
                rectangle.X = 90;
                rectangle.Y = 198;
              }
            }
          }
          else if (index3 != type && index3 != num10 && (num2 == type || num2 == num10))
          {
            if (index1 == num10 && index2 == type)
            {
              if (num9 == 0)
              {
                rectangle.X = 0;
                rectangle.Y = 216;
              }
              if (num9 == 1)
              {
                rectangle.X = 18;
                rectangle.Y = 216;
              }
              if (num9 == 2)
              {
                rectangle.X = 36;
                rectangle.Y = 216;
              }
            }
            else if (index1 == type && index2 == num10)
            {
              if (num9 == 0)
              {
                rectangle.X = 54;
                rectangle.Y = 216;
              }
              if (num9 == 1)
              {
                rectangle.X = 72;
                rectangle.Y = 216;
              }
              if (num9 == 2)
              {
                rectangle.X = 90;
                rectangle.Y = 216;
              }
            }
          }
          else if (index1 != type && index1 != num10 && (index2 == type || index2 == num10))
          {
            if (num2 == num10 && index3 == type)
            {
              if (num9 == 0)
              {
                rectangle.X = 72;
                rectangle.Y = 144;
              }
              if (num9 == 1)
              {
                rectangle.X = 72;
                rectangle.Y = 162;
              }
              if (num9 == 2)
              {
                rectangle.X = 72;
                rectangle.Y = 180;
              }
            }
            else if (index3 == type && index2 == num2)
            {
              if (num9 == 0)
              {
                rectangle.X = 72;
                rectangle.Y = 90;
              }
              if (num9 == 1)
              {
                rectangle.X = 72;
                rectangle.Y = 108;
              }
              if (num9 == 2)
              {
                rectangle.X = 72;
                rectangle.Y = 126;
              }
            }
          }
          else if (index2 != type && index2 != num10 && (index1 == type || index1 == num10))
          {
            if (num2 == num10 && index3 == type)
            {
              if (num9 == 0)
              {
                rectangle.X = 90;
                rectangle.Y = 144;
              }
              if (num9 == 1)
              {
                rectangle.X = 90;
                rectangle.Y = 162;
              }
              if (num9 == 2)
              {
                rectangle.X = 90;
                rectangle.Y = 180;
              }
            }
            else if (index3 == type && index2 == num2)
            {
              if (num9 == 0)
              {
                rectangle.X = 90;
                rectangle.Y = 90;
              }
              if (num9 == 1)
              {
                rectangle.X = 90;
                rectangle.Y = 108;
              }
              if (num9 == 2)
              {
                rectangle.X = 90;
                rectangle.Y = 126;
              }
            }
          }
          else if (num2 == type && index3 == type && index1 == type && index2 == type)
          {
            if (num1 != type && num3 != type && num4 != type && num5 != type)
            {
              if (num5 == num10)
              {
                if (num9 == 0)
                {
                  rectangle.X = 108;
                  rectangle.Y = 324;
                }
                if (num9 == 1)
                {
                  rectangle.X = 126;
                  rectangle.Y = 324;
                }
                if (num9 == 2)
                {
                  rectangle.X = 144;
                  rectangle.Y = 324;
                }
              }
              else if (num3 == num10)
              {
                if (num9 == 0)
                {
                  rectangle.X = 108;
                  rectangle.Y = 342;
                }
                if (num9 == 1)
                {
                  rectangle.X = 126;
                  rectangle.Y = 342;
                }
                if (num9 == 2)
                {
                  rectangle.X = 144;
                  rectangle.Y = 342;
                }
              }
              else if (num4 == num10)
              {
                if (num9 == 0)
                {
                  rectangle.X = 108;
                  rectangle.Y = 360;
                }
                if (num9 == 1)
                {
                  rectangle.X = 126;
                  rectangle.Y = 360;
                }
                if (num9 == 2)
                {
                  rectangle.X = 144;
                  rectangle.Y = 360;
                }
              }
              else if (num1 == num10)
              {
                if (num9 == 0)
                {
                  rectangle.X = 108;
                  rectangle.Y = 378;
                }
                if (num9 == 1)
                {
                  rectangle.X = 126;
                  rectangle.Y = 378;
                }
                if (num9 == 2)
                {
                  rectangle.X = 144;
                  rectangle.Y = 378;
                }
              }
              else
              {
                if (num9 == 0)
                {
                  rectangle.X = 144;
                  rectangle.Y = 234;
                }
                if (num9 == 1)
                {
                  rectangle.X = 198;
                  rectangle.Y = 234;
                }
                if (num9 == 2)
                {
                  rectangle.X = 252;
                  rectangle.Y = 234;
                }
              }
            }
            else if (num1 != type && num5 != type)
            {
              if (num9 == 0)
              {
                rectangle.X = 36;
                rectangle.Y = 306;
              }
              if (num9 == 1)
              {
                rectangle.X = 54;
                rectangle.Y = 306;
              }
              if (num9 == 2)
              {
                rectangle.X = 72;
                rectangle.Y = 306;
              }
            }
            else if (num3 != type && num4 != type)
            {
              if (num9 == 0)
              {
                rectangle.X = 90;
                rectangle.Y = 306;
              }
              if (num9 == 1)
              {
                rectangle.X = 108;
                rectangle.Y = 306;
              }
              if (num9 == 2)
              {
                rectangle.X = 126;
                rectangle.Y = 306;
              }
            }
            else if (num1 != type && num3 == type && num4 == type && num5 == type)
            {
              if (num9 == 0)
              {
                rectangle.X = 54;
                rectangle.Y = 108;
              }
              if (num9 == 1)
              {
                rectangle.X = 54;
                rectangle.Y = 144;
              }
              if (num9 == 2)
              {
                rectangle.X = 54;
                rectangle.Y = 180;
              }
            }
            else if (num1 == type && num3 != type && num4 == type && num5 == type)
            {
              if (num9 == 0)
              {
                rectangle.X = 36;
                rectangle.Y = 108;
              }
              if (num9 == 1)
              {
                rectangle.X = 36;
                rectangle.Y = 144;
              }
              if (num9 == 2)
              {
                rectangle.X = 36;
                rectangle.Y = 180;
              }
            }
            else if (num1 == type && num3 == type && num4 != type && num5 == type)
            {
              if (num9 == 0)
              {
                rectangle.X = 54;
                rectangle.Y = 90;
              }
              if (num9 == 1)
              {
                rectangle.X = 54;
                rectangle.Y = 126;
              }
              if (num9 == 2)
              {
                rectangle.X = 54;
                rectangle.Y = 162;
              }
            }
            else if (num1 == type && num3 == type && num4 == type && num5 != type)
            {
              if (num9 == 0)
              {
                rectangle.X = 36;
                rectangle.Y = 90;
              }
              if (num9 == 1)
              {
                rectangle.X = 36;
                rectangle.Y = 126;
              }
              if (num9 == 2)
              {
                rectangle.X = 36;
                rectangle.Y = 162;
              }
            }
          }
          else if (num2 == type && index3 == num10 && index1 == type && index2 == type && num1 == -1 && num3 == -1)
          {
            if (num9 == 0)
            {
              rectangle.X = 108;
              rectangle.Y = 18;
            }
            if (num9 == 1)
            {
              rectangle.X = 126;
              rectangle.Y = 18;
            }
            if (num9 == 2)
            {
              rectangle.X = 144;
              rectangle.Y = 18;
            }
          }
          else if (num2 == num10 && index3 == type && index1 == type && index2 == type && num4 == -1 && num5 == -1)
          {
            if (num9 == 0)
            {
              rectangle.X = 108;
              rectangle.Y = 36;
            }
            if (num9 == 1)
            {
              rectangle.X = 126;
              rectangle.Y = 36;
            }
            if (num9 == 2)
            {
              rectangle.X = 144;
              rectangle.Y = 36;
            }
          }
          else if (num2 == type && index3 == type && index1 == num10 && index2 == type && num3 == -1 && num5 == -1)
          {
            if (num9 == 0)
            {
              rectangle.X = 198;
              rectangle.Y = 0;
            }
            if (num9 == 1)
            {
              rectangle.X = 198;
              rectangle.Y = 18;
            }
            if (num9 == 2)
            {
              rectangle.X = 198;
              rectangle.Y = 36;
            }
          }
          else if (num2 == type && index3 == type && index1 == type && index2 == num10 && num1 == -1 && num4 == -1)
          {
            if (num9 == 0)
            {
              rectangle.X = 180;
              rectangle.Y = 0;
            }
            if (num9 == 1)
            {
              rectangle.X = 180;
              rectangle.Y = 18;
            }
            if (num9 == 2)
            {
              rectangle.X = 180;
              rectangle.Y = 36;
            }
          }
          else if (num2 == type && index3 == num10 && index1 == type && index2 == type)
          {
            if (num3 != -1)
            {
              if (num9 == 0)
              {
                rectangle.X = 54;
                rectangle.Y = 108;
              }
              if (num9 == 1)
              {
                rectangle.X = 54;
                rectangle.Y = 144;
              }
              if (num9 == 2)
              {
                rectangle.X = 54;
                rectangle.Y = 180;
              }
            }
            else if (num1 != -1)
            {
              if (num9 == 0)
              {
                rectangle.X = 36;
                rectangle.Y = 108;
              }
              if (num9 == 1)
              {
                rectangle.X = 36;
                rectangle.Y = 144;
              }
              if (num9 == 2)
              {
                rectangle.X = 36;
                rectangle.Y = 180;
              }
            }
          }
          else if (num2 == num10 && index3 == type && index1 == type && index2 == type)
          {
            if (num5 != -1)
            {
              if (num9 == 0)
              {
                rectangle.X = 54;
                rectangle.Y = 90;
              }
              if (num9 == 1)
              {
                rectangle.X = 54;
                rectangle.Y = 126;
              }
              if (num9 == 2)
              {
                rectangle.X = 54;
                rectangle.Y = 162;
              }
            }
            else if (num4 != -1)
            {
              if (num9 == 0)
              {
                rectangle.X = 36;
                rectangle.Y = 90;
              }
              if (num9 == 1)
              {
                rectangle.X = 36;
                rectangle.Y = 126;
              }
              if (num9 == 2)
              {
                rectangle.X = 36;
                rectangle.Y = 162;
              }
            }
          }
          else if (num2 == type && index3 == type && index1 == type && index2 == num10)
          {
            if (num1 != -1)
            {
              if (num9 == 0)
              {
                rectangle.X = 54;
                rectangle.Y = 90;
              }
              if (num9 == 1)
              {
                rectangle.X = 54;
                rectangle.Y = 126;
              }
              if (num9 == 2)
              {
                rectangle.X = 54;
                rectangle.Y = 162;
              }
            }
            else if (num4 != -1)
            {
              if (num9 == 0)
              {
                rectangle.X = 54;
                rectangle.Y = 108;
              }
              if (num9 == 1)
              {
                rectangle.X = 54;
                rectangle.Y = 144;
              }
              if (num9 == 2)
              {
                rectangle.X = 54;
                rectangle.Y = 180;
              }
            }
          }
          else if (num2 == type && index3 == type && index1 == num10 && index2 == type)
          {
            if (num3 != -1)
            {
              if (num9 == 0)
              {
                rectangle.X = 36;
                rectangle.Y = 90;
              }
              if (num9 == 1)
              {
                rectangle.X = 36;
                rectangle.Y = 126;
              }
              if (num9 == 2)
              {
                rectangle.X = 36;
                rectangle.Y = 162;
              }
            }
            else if (num5 != -1)
            {
              if (num9 == 0)
              {
                rectangle.X = 36;
                rectangle.Y = 108;
              }
              if (num9 == 1)
              {
                rectangle.X = 36;
                rectangle.Y = 144;
              }
              if (num9 == 2)
              {
                rectangle.X = 36;
                rectangle.Y = 180;
              }
            }
          }
          else if (num2 == num10 && index3 == type && index1 == type && index2 == type || num2 == type && index3 == num10 && index1 == type && index2 == type || num2 == type && index3 == type && index1 == num10 && index2 == type || num2 == type && index3 == type && index1 == type && index2 == num10)
          {
            if (num9 == 0)
            {
              rectangle.X = 18;
              rectangle.Y = 18;
            }
            if (num9 == 1)
            {
              rectangle.X = 36;
              rectangle.Y = 18;
            }
            if (num9 == 2)
            {
              rectangle.X = 54;
              rectangle.Y = 18;
            }
          }
          if ((num2 == type || num2 == num10) && (index3 == type || index3 == num10) && (index1 == type || index1 == num10) && (index2 == type || index2 == num10))
          {
            if (num1 != type && num1 != num10 && (num3 == type || num3 == num10) && (num4 == type || num4 == num10) && (num5 == type || num5 == num10))
            {
              if (num9 == 0)
              {
                rectangle.X = 54;
                rectangle.Y = 108;
              }
              if (num9 == 1)
              {
                rectangle.X = 54;
                rectangle.Y = 144;
              }
              if (num9 == 2)
              {
                rectangle.X = 54;
                rectangle.Y = 180;
              }
            }
            else if (num3 != type && num3 != num10 && (num1 == type || num1 == num10) && (num4 == type || num4 == num10) && (num5 == type || num5 == num10))
            {
              if (num9 == 0)
              {
                rectangle.X = 36;
                rectangle.Y = 108;
              }
              if (num9 == 1)
              {
                rectangle.X = 36;
                rectangle.Y = 144;
              }
              if (num9 == 2)
              {
                rectangle.X = 36;
                rectangle.Y = 180;
              }
            }
            else if (num4 != type && num4 != num10 && (num1 == type || num1 == num10) && (num3 == type || num3 == num10) && (num5 == type || num5 == num10))
            {
              if (num9 == 0)
              {
                rectangle.X = 54;
                rectangle.Y = 90;
              }
              if (num9 == 1)
              {
                rectangle.X = 54;
                rectangle.Y = 126;
              }
              if (num9 == 2)
              {
                rectangle.X = 54;
                rectangle.Y = 162;
              }
            }
            else if (num5 != type && num5 != num10 && (num1 == type || num1 == num10) && (num4 == type || num4 == num10) && (num3 == type || num3 == num10))
            {
              if (num9 == 0)
              {
                rectangle.X = 36;
                rectangle.Y = 90;
              }
              if (num9 == 1)
              {
                rectangle.X = 36;
                rectangle.Y = 126;
              }
              if (num9 == 2)
              {
                rectangle.X = 36;
                rectangle.Y = 162;
              }
            }
          }
          if (num2 != num10 && num2 != type && index3 == type && index1 != num10 && index1 != type && index2 == type && num5 != num10 && num5 != type)
          {
            if (num9 == 0)
            {
              rectangle.X = 90;
              rectangle.Y = 270;
            }
            if (num9 == 1)
            {
              rectangle.X = 108;
              rectangle.Y = 270;
            }
            if (num9 == 2)
            {
              rectangle.X = 126;
              rectangle.Y = 270;
            }
          }
          else if (num2 != num10 && num2 != type && index3 == type && index1 == type && index2 != num10 && index2 != type && num4 != num10 && num4 != type)
          {
            if (num9 == 0)
            {
              rectangle.X = 144;
              rectangle.Y = 270;
            }
            if (num9 == 1)
            {
              rectangle.X = 162;
              rectangle.Y = 270;
            }
            if (num9 == 2)
            {
              rectangle.X = 180;
              rectangle.Y = 270;
            }
          }
          else if (index3 != num10 && index3 != type && num2 == type && index1 != num10 && index1 != type && index2 == type && num3 != num10 && num3 != type)
          {
            if (num9 == 0)
            {
              rectangle.X = 90;
              rectangle.Y = 288;
            }
            if (num9 == 1)
            {
              rectangle.X = 108;
              rectangle.Y = 288;
            }
            if (num9 == 2)
            {
              rectangle.X = 126;
              rectangle.Y = 288;
            }
          }
          else if (index3 != num10 && index3 != type && num2 == type && index1 == type && index2 != num10 && index2 != type && num1 != num10 && num1 != type)
          {
            if (num9 == 0)
            {
              rectangle.X = 144;
              rectangle.Y = 288;
            }
            if (num9 == 1)
            {
              rectangle.X = 162;
              rectangle.Y = 288;
            }
            if (num9 == 2)
            {
              rectangle.X = 180;
              rectangle.Y = 288;
            }
          }
          else if (num2 != type && num2 != num10 && index3 == type && index1 == type && index2 == type && num4 != type && num4 != num10 && num5 != type && num5 != num10)
          {
            if (num9 == 0)
            {
              rectangle.X = 144;
              rectangle.Y = 216;
            }
            if (num9 == 1)
            {
              rectangle.X = 198;
              rectangle.Y = 216;
            }
            if (num9 == 2)
            {
              rectangle.X = 252;
              rectangle.Y = 216;
            }
          }
          else if (index3 != type && index3 != num10 && num2 == type && index1 == type && index2 == type && num1 != type && num1 != num10 && num3 != type && num3 != num10)
          {
            if (num9 == 0)
            {
              rectangle.X = 144;
              rectangle.Y = 252;
            }
            if (num9 == 1)
            {
              rectangle.X = 198;
              rectangle.Y = 252;
            }
            if (num9 == 2)
            {
              rectangle.X = 252;
              rectangle.Y = 252;
            }
          }
          else if (index1 != type && index1 != num10 && index3 == type && num2 == type && index2 == type && num3 != type && num3 != num10 && num5 != type && num5 != num10)
          {
            if (num9 == 0)
            {
              rectangle.X = 126;
              rectangle.Y = 234;
            }
            if (num9 == 1)
            {
              rectangle.X = 180;
              rectangle.Y = 234;
            }
            if (num9 == 2)
            {
              rectangle.X = 234;
              rectangle.Y = 234;
            }
          }
          else if (index2 != type && index2 != num10 && index3 == type && num2 == type && index1 == type && num1 != type && num1 != num10 && num4 != type && num4 != num10)
          {
            if (num9 == 0)
            {
              rectangle.X = 162;
              rectangle.Y = 234;
            }
            if (num9 == 1)
            {
              rectangle.X = 216;
              rectangle.Y = 234;
            }
            if (num9 == 2)
            {
              rectangle.X = 270;
              rectangle.Y = 234;
            }
          }
          else if (num2 != num10 && num2 != type && (index3 == num10 || index3 == type) && index1 == num10 && index2 == num10)
          {
            if (num9 == 0)
            {
              rectangle.X = 36;
              rectangle.Y = 270;
            }
            if (num9 == 1)
            {
              rectangle.X = 54;
              rectangle.Y = 270;
            }
            if (num9 == 2)
            {
              rectangle.X = 72;
              rectangle.Y = 270;
            }
          }
          else if (index3 != num10 && index3 != type && (num2 == num10 || num2 == type) && index1 == num10 && index2 == num10)
          {
            if (num9 == 0)
            {
              rectangle.X = 36;
              rectangle.Y = 288;
            }
            if (num9 == 1)
            {
              rectangle.X = 54;
              rectangle.Y = 288;
            }
            if (num9 == 2)
            {
              rectangle.X = 72;
              rectangle.Y = 288;
            }
          }
          else if (index1 != num10 && index1 != type && (index2 == num10 || index2 == type) && num2 == num10 && index3 == num10)
          {
            if (num9 == 0)
            {
              rectangle.X = 0;
              rectangle.Y = 270;
            }
            if (num9 == 1)
            {
              rectangle.X = 0;
              rectangle.Y = 288;
            }
            if (num9 == 2)
            {
              rectangle.X = 0;
              rectangle.Y = 306;
            }
          }
          else if (index2 != num10 && index2 != type && (index1 == num10 || index1 == type) && num2 == num10 && index3 == num10)
          {
            if (num9 == 0)
            {
              rectangle.X = 18;
              rectangle.Y = 270;
            }
            if (num9 == 1)
            {
              rectangle.X = 18;
              rectangle.Y = 288;
            }
            if (num9 == 2)
            {
              rectangle.X = 18;
              rectangle.Y = 306;
            }
          }
          else if (num2 == type && index3 == num10 && index1 == num10 && index2 == num10)
          {
            if (num9 == 0)
            {
              rectangle.X = 198;
              rectangle.Y = 288;
            }
            if (num9 == 1)
            {
              rectangle.X = 216;
              rectangle.Y = 288;
            }
            if (num9 == 2)
            {
              rectangle.X = 234;
              rectangle.Y = 288;
            }
          }
          else if (num2 == num10 && index3 == type && index1 == num10 && index2 == num10)
          {
            if (num9 == 0)
            {
              rectangle.X = 198;
              rectangle.Y = 270;
            }
            if (num9 == 1)
            {
              rectangle.X = 216;
              rectangle.Y = 270;
            }
            if (num9 == 2)
            {
              rectangle.X = 234;
              rectangle.Y = 270;
            }
          }
          else if (num2 == num10 && index3 == num10 && index1 == type && index2 == num10)
          {
            if (num9 == 0)
            {
              rectangle.X = 198;
              rectangle.Y = 306;
            }
            if (num9 == 1)
            {
              rectangle.X = 216;
              rectangle.Y = 306;
            }
            if (num9 == 2)
            {
              rectangle.X = 234;
              rectangle.Y = 306;
            }
          }
          else if (num2 == num10 && index3 == num10 && index1 == num10 && index2 == type)
          {
            if (num9 == 0)
            {
              rectangle.X = 144;
              rectangle.Y = 306;
            }
            if (num9 == 1)
            {
              rectangle.X = 162;
              rectangle.Y = 306;
            }
            if (num9 == 2)
            {
              rectangle.X = 180;
              rectangle.Y = 306;
            }
          }
          if (num2 != type && num2 != num10 && index3 == type && index1 == type && index2 == type)
          {
            if ((num4 == num10 || num4 == type) && num5 != num10 && num5 != type)
            {
              if (num9 == 0)
              {
                rectangle.X = 0;
                rectangle.Y = 324;
              }
              if (num9 == 1)
              {
                rectangle.X = 18;
                rectangle.Y = 324;
              }
              if (num9 == 2)
              {
                rectangle.X = 36;
                rectangle.Y = 324;
              }
            }
            else if ((num5 == num10 || num5 == type) && num4 != num10 && num4 != type)
            {
              if (num9 == 0)
              {
                rectangle.X = 54;
                rectangle.Y = 324;
              }
              if (num9 == 1)
              {
                rectangle.X = 72;
                rectangle.Y = 324;
              }
              if (num9 == 2)
              {
                rectangle.X = 90;
                rectangle.Y = 324;
              }
            }
          }
          else if (index3 != type && index3 != num10 && num2 == type && index1 == type && index2 == type)
          {
            if ((num1 == num10 || num1 == type) && num3 != num10 && num3 != type)
            {
              if (num9 == 0)
              {
                rectangle.X = 0;
                rectangle.Y = 342;
              }
              if (num9 == 1)
              {
                rectangle.X = 18;
                rectangle.Y = 342;
              }
              if (num9 == 2)
              {
                rectangle.X = 36;
                rectangle.Y = 342;
              }
            }
            else if ((num3 == num10 || num3 == type) && num1 != num10 && num1 != type)
            {
              if (num9 == 0)
              {
                rectangle.X = 54;
                rectangle.Y = 342;
              }
              if (num9 == 1)
              {
                rectangle.X = 72;
                rectangle.Y = 342;
              }
              if (num9 == 2)
              {
                rectangle.X = 90;
                rectangle.Y = 342;
              }
            }
          }
          else if (index1 != type && index1 != num10 && num2 == type && index3 == type && index2 == type)
          {
            if ((num3 == num10 || num3 == type) && num5 != num10 && num5 != type)
            {
              if (num9 == 0)
              {
                rectangle.X = 54;
                rectangle.Y = 360;
              }
              if (num9 == 1)
              {
                rectangle.X = 72;
                rectangle.Y = 360;
              }
              if (num9 == 2)
              {
                rectangle.X = 90;
                rectangle.Y = 360;
              }
            }
            else if ((num5 == num10 || num5 == type) && num3 != num10 && num3 != type)
            {
              if (num9 == 0)
              {
                rectangle.X = 0;
                rectangle.Y = 360;
              }
              if (num9 == 1)
              {
                rectangle.X = 18;
                rectangle.Y = 360;
              }
              if (num9 == 2)
              {
                rectangle.X = 36;
                rectangle.Y = 360;
              }
            }
          }
          else if (index2 != type && index2 != num10 && num2 == type && index3 == type && index1 == type)
          {
            if ((num1 == num10 || num1 == type) && num4 != num10 && num4 != type)
            {
              if (num9 == 0)
              {
                rectangle.X = 0;
                rectangle.Y = 378;
              }
              if (num9 == 1)
              {
                rectangle.X = 18;
                rectangle.Y = 378;
              }
              if (num9 == 2)
              {
                rectangle.X = 36;
                rectangle.Y = 378;
              }
            }
            else if ((num4 == num10 || num4 == type) && num1 != num10 && num1 != type)
            {
              if (num9 == 0)
              {
                rectangle.X = 54;
                rectangle.Y = 378;
              }
              if (num9 == 1)
              {
                rectangle.X = 72;
                rectangle.Y = 378;
              }
              if (num9 == 2)
              {
                rectangle.X = 90;
                rectangle.Y = 378;
              }
            }
          }
          if ((num2 == type || num2 == num10) && (index3 == type || index3 == num10) && (index1 == type || index1 == num10) && (index2 == type || index2 == num10) && num1 != -1 && num3 != -1 && num4 != -1 && num5 != -1)
          {
            if (num9 == 0)
            {
              rectangle.X = 18;
              rectangle.Y = 18;
            }
            if (num9 == 1)
            {
              rectangle.X = 36;
              rectangle.Y = 18;
            }
            if (num9 == 2)
            {
              rectangle.X = 54;
              rectangle.Y = 18;
            }
          }
          if (num2 == num10)
            num2 = -2;
          if (index3 == num10)
            index3 = -2;
          if (index1 == num10)
            index1 = -2;
          if (index2 == num10)
            index2 = -2;
          if (num1 == num10)
            num1 = -2;
          if (num3 == num10)
            num3 = -2;
          if (num4 == num10)
            num4 = -2;
          if (num5 == num10)
            num5 = -2;
        }
        if ((type == 1 || type == 2 || type == 6 || type == 7) && rectangle.X == -1 && rectangle.Y == -1)
        {
          if (num2 >= 0 && num2 != type)
            num2 = -1;
          if (index3 >= 0 && index3 != type)
            index3 = -1;
          if (index1 >= 0 && index1 != type)
            index1 = -1;
          if (index2 >= 0 && index2 != type)
            index2 = -1;
          if (num2 != -1 && index3 != -1 && index1 != -1 && index2 != -1)
          {
            if (num2 == -2 && index3 == type && index1 == type && index2 == type)
            {
              if (num9 == 0)
              {
                rectangle.X = 144;
                rectangle.Y = 108;
              }
              if (num9 == 1)
              {
                rectangle.X = 162;
                rectangle.Y = 108;
              }
              if (num9 == 2)
              {
                rectangle.X = 180;
                rectangle.Y = 108;
              }
              WorldGen.mergeUp = true;
            }
            else if (num2 == type && index3 == -2 && index1 == type && index2 == type)
            {
              if (num9 == 0)
              {
                rectangle.X = 144;
                rectangle.Y = 90;
              }
              if (num9 == 1)
              {
                rectangle.X = 162;
                rectangle.Y = 90;
              }
              if (num9 == 2)
              {
                rectangle.X = 180;
                rectangle.Y = 90;
              }
              WorldGen.mergeDown = true;
            }
            else if (num2 == type && index3 == type && index1 == -2 && index2 == type)
            {
              if (num9 == 0)
              {
                rectangle.X = 162;
                rectangle.Y = 126;
              }
              if (num9 == 1)
              {
                rectangle.X = 162;
                rectangle.Y = 144;
              }
              if (num9 == 2)
              {
                rectangle.X = 162;
                rectangle.Y = 162;
              }
              WorldGen.mergeLeft = true;
            }
            else if (num2 == type && index3 == type && index1 == type && index2 == -2)
            {
              if (num9 == 0)
              {
                rectangle.X = 144;
                rectangle.Y = 126;
              }
              if (num9 == 1)
              {
                rectangle.X = 144;
                rectangle.Y = 144;
              }
              if (num9 == 2)
              {
                rectangle.X = 144;
                rectangle.Y = 162;
              }
              WorldGen.mergeRight = true;
            }
            else if (num2 == -2 && index3 == type && index1 == -2 && index2 == type)
            {
              if (num9 == 0)
              {
                rectangle.X = 36;
                rectangle.Y = 90;
              }
              if (num9 == 1)
              {
                rectangle.X = 36;
                rectangle.Y = 126;
              }
              if (num9 == 2)
              {
                rectangle.X = 36;
                rectangle.Y = 162;
              }
              WorldGen.mergeUp = true;
              WorldGen.mergeLeft = true;
            }
            else if (num2 == -2 && index3 == type && index1 == type && index2 == -2)
            {
              if (num9 == 0)
              {
                rectangle.X = 54;
                rectangle.Y = 90;
              }
              if (num9 == 1)
              {
                rectangle.X = 54;
                rectangle.Y = 126;
              }
              if (num9 == 2)
              {
                rectangle.X = 54;
                rectangle.Y = 162;
              }
              WorldGen.mergeUp = true;
              WorldGen.mergeRight = true;
            }
            else if (num2 == type && index3 == -2 && index1 == -2 && index2 == type)
            {
              if (num9 == 0)
              {
                rectangle.X = 36;
                rectangle.Y = 108;
              }
              if (num9 == 1)
              {
                rectangle.X = 36;
                rectangle.Y = 144;
              }
              if (num9 == 2)
              {
                rectangle.X = 36;
                rectangle.Y = 180;
              }
              WorldGen.mergeDown = true;
              WorldGen.mergeLeft = true;
            }
            else if (num2 == type && index3 == -2 && index1 == type && index2 == -2)
            {
              if (num9 == 0)
              {
                rectangle.X = 54;
                rectangle.Y = 108;
              }
              if (num9 == 1)
              {
                rectangle.X = 54;
                rectangle.Y = 144;
              }
              if (num9 == 2)
              {
                rectangle.X = 54;
                rectangle.Y = 180;
              }
              WorldGen.mergeDown = true;
              WorldGen.mergeRight = true;
            }
            else if (num2 == type && index3 == type && index1 == -2 && index2 == -2)
            {
              if (num9 == 0)
              {
                rectangle.X = 180;
                rectangle.Y = 126;
              }
              if (num9 == 1)
              {
                rectangle.X = 180;
                rectangle.Y = 144;
              }
              if (num9 == 2)
              {
                rectangle.X = 180;
                rectangle.Y = 162;
              }
              WorldGen.mergeLeft = true;
              WorldGen.mergeRight = true;
            }
            else if (num2 == -2 && index3 == -2 && index1 == type && index2 == type)
            {
              if (num9 == 0)
              {
                rectangle.X = 144;
                rectangle.Y = 180;
              }
              if (num9 == 1)
              {
                rectangle.X = 162;
                rectangle.Y = 180;
              }
              if (num9 == 2)
              {
                rectangle.X = 180;
                rectangle.Y = 180;
              }
              WorldGen.mergeUp = true;
              WorldGen.mergeDown = true;
            }
            else if (num2 == -2 && index3 == type && index1 == -2 && index2 == -2)
            {
              if (num9 == 0)
              {
                rectangle.X = 198;
                rectangle.Y = 90;
              }
              if (num9 == 1)
              {
                rectangle.X = 198;
                rectangle.Y = 108;
              }
              if (num9 == 2)
              {
                rectangle.X = 198;
                rectangle.Y = 126;
              }
              WorldGen.mergeUp = true;
              WorldGen.mergeLeft = true;
              WorldGen.mergeRight = true;
            }
            else if (num2 == type && index3 == -2 && index1 == -2 && index2 == -2)
            {
              if (num9 == 0)
              {
                rectangle.X = 198;
                rectangle.Y = 144;
              }
              if (num9 == 1)
              {
                rectangle.X = 198;
                rectangle.Y = 162;
              }
              if (num9 == 2)
              {
                rectangle.X = 198;
                rectangle.Y = 180;
              }
              WorldGen.mergeDown = true;
              WorldGen.mergeLeft = true;
              WorldGen.mergeRight = true;
            }
            else if (num2 == -2 && index3 == -2 && index1 == type && index2 == -2)
            {
              if (num9 == 0)
              {
                rectangle.X = 216;
                rectangle.Y = 144;
              }
              if (num9 == 1)
              {
                rectangle.X = 216;
                rectangle.Y = 162;
              }
              if (num9 == 2)
              {
                rectangle.X = 216;
                rectangle.Y = 180;
              }
              WorldGen.mergeUp = true;
              WorldGen.mergeDown = true;
              WorldGen.mergeRight = true;
            }
            else if (num2 == -2 && index3 == -2 && index1 == -2 && index2 == type)
            {
              if (num9 == 0)
              {
                rectangle.X = 216;
                rectangle.Y = 90;
              }
              if (num9 == 1)
              {
                rectangle.X = 216;
                rectangle.Y = 108;
              }
              if (num9 == 2)
              {
                rectangle.X = 216;
                rectangle.Y = 126;
              }
              WorldGen.mergeUp = true;
              WorldGen.mergeDown = true;
              WorldGen.mergeLeft = true;
            }
            else if (num2 == -2 && index3 == -2 && index1 == -2 && index2 == -2)
            {
              if (num9 == 0)
              {
                rectangle.X = 108;
                rectangle.Y = 198;
              }
              if (num9 == 1)
              {
                rectangle.X = 126;
                rectangle.Y = 198;
              }
              if (num9 == 2)
              {
                rectangle.X = 144;
                rectangle.Y = 198;
              }
              WorldGen.mergeUp = true;
              WorldGen.mergeDown = true;
              WorldGen.mergeLeft = true;
              WorldGen.mergeRight = true;
            }
            else if (num2 == type && index3 == type && index1 == type && index2 == type)
            {
              if (num1 == -2)
              {
                if (num9 == 0)
                {
                  rectangle.X = 18;
                  rectangle.Y = 108;
                }
                if (num9 == 1)
                {
                  rectangle.X = 18;
                  rectangle.Y = 144;
                }
                if (num9 == 2)
                {
                  rectangle.X = 18;
                  rectangle.Y = 180;
                }
              }
              if (num3 == -2)
              {
                if (num9 == 0)
                {
                  rectangle.X = 0;
                  rectangle.Y = 108;
                }
                if (num9 == 1)
                {
                  rectangle.X = 0;
                  rectangle.Y = 144;
                }
                if (num9 == 2)
                {
                  rectangle.X = 0;
                  rectangle.Y = 180;
                }
              }
              if (num4 == -2)
              {
                if (num9 == 0)
                {
                  rectangle.X = 18;
                  rectangle.Y = 90;
                }
                if (num9 == 1)
                {
                  rectangle.X = 18;
                  rectangle.Y = 126;
                }
                if (num9 == 2)
                {
                  rectangle.X = 18;
                  rectangle.Y = 162;
                }
              }
              if (num5 == -2)
              {
                if (num9 == 0)
                {
                  rectangle.X = 0;
                  rectangle.Y = 90;
                }
                if (num9 == 1)
                {
                  rectangle.X = 0;
                  rectangle.Y = 126;
                }
                if (num9 == 2)
                {
                  rectangle.X = 0;
                  rectangle.Y = 162;
                }
              }
            }
          }
          else if (num2 != -1 && index3 != -1 && index1 == -1 && index2 == type)
          {
            if (num2 == -2 && index3 == type)
            {
              if (num9 == 0)
              {
                rectangle.X = 72;
                rectangle.Y = 144;
              }
              if (num9 == 1)
              {
                rectangle.X = 72;
                rectangle.Y = 162;
              }
              if (num9 == 2)
              {
                rectangle.X = 72;
                rectangle.Y = 180;
              }
              WorldGen.mergeUp = true;
            }
            else if (index3 == -2 && num2 == type)
            {
              if (num9 == 0)
              {
                rectangle.X = 72;
                rectangle.Y = 90;
              }
              if (num9 == 1)
              {
                rectangle.X = 72;
                rectangle.Y = 108;
              }
              if (num9 == 2)
              {
                rectangle.X = 72;
                rectangle.Y = 126;
              }
              WorldGen.mergeDown = true;
            }
          }
          else if (num2 != -1 && index3 != -1 && index1 == type && index2 == -1)
          {
            if (num2 == -2 && index3 == type)
            {
              if (num9 == 0)
              {
                rectangle.X = 90;
                rectangle.Y = 144;
              }
              if (num9 == 1)
              {
                rectangle.X = 90;
                rectangle.Y = 162;
              }
              if (num9 == 2)
              {
                rectangle.X = 90;
                rectangle.Y = 180;
              }
              WorldGen.mergeUp = true;
            }
            else if (index3 == -2 && num2 == type)
            {
              if (num9 == 0)
              {
                rectangle.X = 90;
                rectangle.Y = 90;
              }
              if (num9 == 1)
              {
                rectangle.X = 90;
                rectangle.Y = 108;
              }
              if (num9 == 2)
              {
                rectangle.X = 90;
                rectangle.Y = 126;
              }
              WorldGen.mergeDown = true;
            }
          }
          else if (num2 == -1 && index3 == type && index1 != -1 && index2 != -1)
          {
            if (index1 == -2 && index2 == type)
            {
              if (num9 == 0)
              {
                rectangle.X = 0;
                rectangle.Y = 198;
              }
              if (num9 == 1)
              {
                rectangle.X = 18;
                rectangle.Y = 198;
              }
              if (num9 == 2)
              {
                rectangle.X = 36;
                rectangle.Y = 198;
              }
              WorldGen.mergeLeft = true;
            }
            else if (index2 == -2 && index1 == type)
            {
              if (num9 == 0)
              {
                rectangle.X = 54;
                rectangle.Y = 198;
              }
              if (num9 == 1)
              {
                rectangle.X = 72;
                rectangle.Y = 198;
              }
              if (num9 == 2)
              {
                rectangle.X = 90;
                rectangle.Y = 198;
              }
              WorldGen.mergeRight = true;
            }
          }
          else if (num2 == type && index3 == -1 && index1 != -1 && index2 != -1)
          {
            if (index1 == -2 && index2 == type)
            {
              if (num9 == 0)
              {
                rectangle.X = 0;
                rectangle.Y = 216;
              }
              if (num9 == 1)
              {
                rectangle.X = 18;
                rectangle.Y = 216;
              }
              if (num9 == 2)
              {
                rectangle.X = 36;
                rectangle.Y = 216;
              }
              WorldGen.mergeLeft = true;
            }
            else if (index2 == -2 && index1 == type)
            {
              if (num9 == 0)
              {
                rectangle.X = 54;
                rectangle.Y = 216;
              }
              if (num9 == 1)
              {
                rectangle.X = 72;
                rectangle.Y = 216;
              }
              if (num9 == 2)
              {
                rectangle.X = 90;
                rectangle.Y = 216;
              }
              WorldGen.mergeRight = true;
            }
          }
          else if (num2 != -1 && index3 != -1 && index1 == -1 && index2 == -1)
          {
            if (num2 == -2 && index3 == -2)
            {
              if (num9 == 0)
              {
                rectangle.X = 108;
                rectangle.Y = 216;
              }
              if (num9 == 1)
              {
                rectangle.X = 108;
                rectangle.Y = 234;
              }
              if (num9 == 2)
              {
                rectangle.X = 108;
                rectangle.Y = 252;
              }
              WorldGen.mergeUp = true;
              WorldGen.mergeDown = true;
            }
            else if (num2 == -2)
            {
              if (num9 == 0)
              {
                rectangle.X = 126;
                rectangle.Y = 144;
              }
              if (num9 == 1)
              {
                rectangle.X = 126;
                rectangle.Y = 162;
              }
              if (num9 == 2)
              {
                rectangle.X = 126;
                rectangle.Y = 180;
              }
              WorldGen.mergeUp = true;
            }
            else if (index3 == -2)
            {
              if (num9 == 0)
              {
                rectangle.X = 126;
                rectangle.Y = 90;
              }
              if (num9 == 1)
              {
                rectangle.X = 126;
                rectangle.Y = 108;
              }
              if (num9 == 2)
              {
                rectangle.X = 126;
                rectangle.Y = 126;
              }
              WorldGen.mergeDown = true;
            }
          }
          else if (num2 == -1 && index3 == -1 && index1 != -1 && index2 != -1)
          {
            if (index1 == -2 && index2 == -2)
            {
              if (num9 == 0)
              {
                rectangle.X = 162;
                rectangle.Y = 198;
              }
              if (num9 == 1)
              {
                rectangle.X = 180;
                rectangle.Y = 198;
              }
              if (num9 == 2)
              {
                rectangle.X = 198;
                rectangle.Y = 198;
              }
              WorldGen.mergeLeft = true;
              WorldGen.mergeRight = true;
            }
            else if (index1 == -2)
            {
              if (num9 == 0)
              {
                rectangle.X = 0;
                rectangle.Y = 252;
              }
              if (num9 == 1)
              {
                rectangle.X = 18;
                rectangle.Y = 252;
              }
              if (num9 == 2)
              {
                rectangle.X = 36;
                rectangle.Y = 252;
              }
              WorldGen.mergeLeft = true;
            }
            else if (index2 == -2)
            {
              if (num9 == 0)
              {
                rectangle.X = 54;
                rectangle.Y = 252;
              }
              if (num9 == 1)
              {
                rectangle.X = 72;
                rectangle.Y = 252;
              }
              if (num9 == 2)
              {
                rectangle.X = 90;
                rectangle.Y = 252;
              }
              WorldGen.mergeRight = true;
            }
          }
          else if (num2 == -2 && index3 == -1 && index1 == -1 && index2 == -1)
          {
            if (num9 == 0)
            {
              rectangle.X = 108;
              rectangle.Y = 144;
            }
            if (num9 == 1)
            {
              rectangle.X = 108;
              rectangle.Y = 162;
            }
            if (num9 == 2)
            {
              rectangle.X = 108;
              rectangle.Y = 180;
            }
            WorldGen.mergeUp = true;
          }
          else if (num2 == -1 && index3 == -2 && index1 == -1 && index2 == -1)
          {
            if (num9 == 0)
            {
              rectangle.X = 108;
              rectangle.Y = 90;
            }
            if (num9 == 1)
            {
              rectangle.X = 108;
              rectangle.Y = 108;
            }
            if (num9 == 2)
            {
              rectangle.X = 108;
              rectangle.Y = 126;
            }
            WorldGen.mergeDown = true;
          }
          else if (num2 == -1 && index3 == -1 && index1 == -2 && index2 == -1)
          {
            if (num9 == 0)
            {
              rectangle.X = 0;
              rectangle.Y = 234;
            }
            if (num9 == 1)
            {
              rectangle.X = 18;
              rectangle.Y = 234;
            }
            if (num9 == 2)
            {
              rectangle.X = 36;
              rectangle.Y = 234;
            }
            WorldGen.mergeLeft = true;
          }
          else if (num2 == -1 && index3 == -1 && index1 == -1 && index2 == -2)
          {
            if (num9 == 0)
            {
              rectangle.X = 54;
              rectangle.Y = 234;
            }
            if (num9 == 1)
            {
              rectangle.X = 72;
              rectangle.Y = 234;
            }
            if (num9 == 2)
            {
              rectangle.X = 90;
              rectangle.Y = 234;
            }
            WorldGen.mergeRight = true;
          }
        }
        if (rectangle.X < 0 || rectangle.Y < 0)
        {
          if (type == 2)
          {
            if (num2 == -2)
              num2 = type;
            if (index3 == -2)
              index3 = type;
            if (index1 == -2)
              index1 = type;
            if (index2 == -2)
              index2 = type;
            if (num1 == -2)
              num1 = type;
            if (num3 == -2)
              num3 = type;
            if (num4 == -2)
              num4 = type;
            if (num5 == -2)
              num5 = type;
          }
          if (num2 == type && index3 == type && index1 == type & index2 == type)
          {
            if (num1 != type && num3 != type)
            {
              if (num9 == 0)
              {
                rectangle.X = 108;
                rectangle.Y = 18;
              }
              if (num9 == 1)
              {
                rectangle.X = 126;
                rectangle.Y = 18;
              }
              if (num9 == 2)
              {
                rectangle.X = 144;
                rectangle.Y = 18;
              }
            }
            else if (num4 != type && num5 != type)
            {
              if (num9 == 0)
              {
                rectangle.X = 108;
                rectangle.Y = 36;
              }
              if (num9 == 1)
              {
                rectangle.X = 126;
                rectangle.Y = 36;
              }
              if (num9 == 2)
              {
                rectangle.X = 144;
                rectangle.Y = 36;
              }
            }
            else if (num1 != type && num4 != type)
            {
              if (num9 == 0)
              {
                rectangle.X = 180;
                rectangle.Y = 0;
              }
              if (num9 == 1)
              {
                rectangle.X = 180;
                rectangle.Y = 18;
              }
              if (num9 == 2)
              {
                rectangle.X = 180;
                rectangle.Y = 36;
              }
            }
            else if (num3 != type && num5 != type)
            {
              if (num9 == 0)
              {
                rectangle.X = 198;
                rectangle.Y = 0;
              }
              if (num9 == 1)
              {
                rectangle.X = 198;
                rectangle.Y = 18;
              }
              if (num9 == 2)
              {
                rectangle.X = 198;
                rectangle.Y = 36;
              }
            }
            else
            {
              if (num9 == 0)
              {
                rectangle.X = 18;
                rectangle.Y = 18;
              }
              if (num9 == 1)
              {
                rectangle.X = 36;
                rectangle.Y = 18;
              }
              if (num9 == 2)
              {
                rectangle.X = 54;
                rectangle.Y = 18;
              }
            }
          }
          else if (num2 != type && index3 == type && index1 == type & index2 == type)
          {
            if (num9 == 0)
            {
              rectangle.X = 18;
              rectangle.Y = 0;
            }
            if (num9 == 1)
            {
              rectangle.X = 36;
              rectangle.Y = 0;
            }
            if (num9 == 2)
            {
              rectangle.X = 54;
              rectangle.Y = 0;
            }
          }
          else if (num2 == type && index3 != type && index1 == type & index2 == type)
          {
            if (num9 == 0)
            {
              rectangle.X = 18;
              rectangle.Y = 36;
            }
            if (num9 == 1)
            {
              rectangle.X = 36;
              rectangle.Y = 36;
            }
            if (num9 == 2)
            {
              rectangle.X = 54;
              rectangle.Y = 36;
            }
          }
          else if (num2 == type && index3 == type && index1 != type & index2 == type)
          {
            if (num9 == 0)
            {
              rectangle.X = 0;
              rectangle.Y = 0;
            }
            if (num9 == 1)
            {
              rectangle.X = 0;
              rectangle.Y = 18;
            }
            if (num9 == 2)
            {
              rectangle.X = 0;
              rectangle.Y = 36;
            }
          }
          else if (num2 == type && index3 == type && index1 == type & index2 != type)
          {
            if (num9 == 0)
            {
              rectangle.X = 72;
              rectangle.Y = 0;
            }
            if (num9 == 1)
            {
              rectangle.X = 72;
              rectangle.Y = 18;
            }
            if (num9 == 2)
            {
              rectangle.X = 72;
              rectangle.Y = 36;
            }
          }
          else if (num2 != type && index3 == type && index1 != type & index2 == type)
          {
            if (num9 == 0)
            {
              rectangle.X = 0;
              rectangle.Y = 54;
            }
            if (num9 == 1)
            {
              rectangle.X = 36;
              rectangle.Y = 54;
            }
            if (num9 == 2)
            {
              rectangle.X = 72;
              rectangle.Y = 54;
            }
          }
          else if (num2 != type && index3 == type && index1 == type & index2 != type)
          {
            if (num9 == 0)
            {
              rectangle.X = 18;
              rectangle.Y = 54;
            }
            if (num9 == 1)
            {
              rectangle.X = 54;
              rectangle.Y = 54;
            }
            if (num9 == 2)
            {
              rectangle.X = 90;
              rectangle.Y = 54;
            }
          }
          else if (num2 == type && index3 != type && index1 != type & index2 == type)
          {
            if (num9 == 0)
            {
              rectangle.X = 0;
              rectangle.Y = 72;
            }
            if (num9 == 1)
            {
              rectangle.X = 36;
              rectangle.Y = 72;
            }
            if (num9 == 2)
            {
              rectangle.X = 72;
              rectangle.Y = 72;
            }
          }
          else if (num2 == type && index3 != type && index1 == type & index2 != type)
          {
            if (num9 == 0)
            {
              rectangle.X = 18;
              rectangle.Y = 72;
            }
            if (num9 == 1)
            {
              rectangle.X = 54;
              rectangle.Y = 72;
            }
            if (num9 == 2)
            {
              rectangle.X = 90;
              rectangle.Y = 72;
            }
          }
          else if (num2 == type && index3 == type && index1 != type & index2 != type)
          {
            if (num9 == 0)
            {
              rectangle.X = 90;
              rectangle.Y = 0;
            }
            if (num9 == 1)
            {
              rectangle.X = 90;
              rectangle.Y = 18;
            }
            if (num9 == 2)
            {
              rectangle.X = 90;
              rectangle.Y = 36;
            }
          }
          else if (num2 != type && index3 != type && index1 == type & index2 == type)
          {
            if (num9 == 0)
            {
              rectangle.X = 108;
              rectangle.Y = 72;
            }
            if (num9 == 1)
            {
              rectangle.X = 126;
              rectangle.Y = 72;
            }
            if (num9 == 2)
            {
              rectangle.X = 144;
              rectangle.Y = 72;
            }
          }
          else if (num2 != type && index3 == type && index1 != type & index2 != type)
          {
            if (num9 == 0)
            {
              rectangle.X = 108;
              rectangle.Y = 0;
            }
            if (num9 == 1)
            {
              rectangle.X = 126;
              rectangle.Y = 0;
            }
            if (num9 == 2)
            {
              rectangle.X = 144;
              rectangle.Y = 0;
            }
          }
          else if (num2 == type && index3 != type && index1 != type & index2 != type)
          {
            if (num9 == 0)
            {
              rectangle.X = 108;
              rectangle.Y = 54;
            }
            if (num9 == 1)
            {
              rectangle.X = 126;
              rectangle.Y = 54;
            }
            if (num9 == 2)
            {
              rectangle.X = 144;
              rectangle.Y = 54;
            }
          }
          else if (num2 != type && index3 != type && index1 != type & index2 == type)
          {
            if (num9 == 0)
            {
              rectangle.X = 162;
              rectangle.Y = 0;
            }
            if (num9 == 1)
            {
              rectangle.X = 162;
              rectangle.Y = 18;
            }
            if (num9 == 2)
            {
              rectangle.X = 162;
              rectangle.Y = 36;
            }
          }
          else if (num2 != type && index3 != type && index1 == type & index2 != type)
          {
            if (num9 == 0)
            {
              rectangle.X = 216;
              rectangle.Y = 0;
            }
            if (num9 == 1)
            {
              rectangle.X = 216;
              rectangle.Y = 18;
            }
            if (num9 == 2)
            {
              rectangle.X = 216;
              rectangle.Y = 36;
            }
          }
          else if (num2 != type && index3 != type && index1 != type & index2 != type)
          {
            if (num9 == 0)
            {
              rectangle.X = 162;
              rectangle.Y = 54;
            }
            if (num9 == 1)
            {
              rectangle.X = 180;
              rectangle.Y = 54;
            }
            if (num9 == 2)
            {
              rectangle.X = 198;
              rectangle.Y = 54;
            }
          }
        }
        if (rectangle.X <= -1 || rectangle.Y <= -1)
        {
          if (num9 <= 0)
          {
            rectangle.X = 18;
            rectangle.Y = 18;
          }
          if (num9 == 1)
          {
            rectangle.X = 36;
            rectangle.Y = 18;
          }
          if (num9 >= 2)
          {
            rectangle.X = 54;
            rectangle.Y = 18;
          }
        }
        Game1.tile[i, j].frameX = (short) rectangle.X;
        Game1.tile[i, j].frameY = (short) rectangle.Y;
        if (rectangle.X != frameX1 && rectangle.Y != frameY1 && frameX1 >= 0 && frameY1 >= 0)
        {
          bool mergeUp = WorldGen.mergeUp;
          bool mergeDown = WorldGen.mergeDown;
          bool mergeLeft = WorldGen.mergeLeft;
          bool mergeRight = WorldGen.mergeRight;
          WorldGen.TileFrame(i - 1, j);
          WorldGen.TileFrame(i + 1, j);
          WorldGen.TileFrame(i, j - 1);
          WorldGen.TileFrame(i, j + 1);
          WorldGen.mergeUp = mergeUp;
          WorldGen.mergeDown = mergeDown;
          WorldGen.mergeLeft = mergeLeft;
          WorldGen.mergeRight = mergeRight;
        }
      }
    }
  }
}
