// Decompiled with JetBrains decompiler
// Type: GameManager.Lighting
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 09A1FDD7-68B3-40FF-A030-32890DF6B0E7
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager v1 Beta\GameManager.exe

using Microsoft.Xna.Framework;

namespace GameManager
{
  public class Lighting
  {
    public const int offScreenTiles = 21;
    public static int lightPasses = 2;
    public static int lightSkip = 1;
    private static float lightColor = 0.0f;
    public static int lightCounter = 0;
    private static int firstTileX;
    private static int lastTileX;
    private static int firstTileY;
    private static int lastTileY;
    public static float[,] color = new float[Game1.screenWidth / 16 + 42 + 10, Game1.screenHeight / 16 + 42 + 10];
    private static int maxTempLights = 100;
    private static int[] tempLightX = new int[Lighting.maxTempLights];
    private static int[] tempLightY = new int[Lighting.maxTempLights];
    private static float[] tempLight = new float[Lighting.maxTempLights];
    private static int tempLightCount;
    private static int firstToLightX;
    private static int firstToLightY;
    private static int lastToLightX;
    private static int lastToLightY;

    public static void LightTiles(int firstX, int lastX, int firstY, int lastY)
    {
      Lighting.firstTileX = firstX;
      Lighting.lastTileX = lastX;
      Lighting.firstTileY = firstY;
      Lighting.lastTileY = lastY;
      ++Lighting.lightCounter;
      if (Lighting.lightCounter <= Lighting.lightSkip)
      {
        Lighting.tempLightCount = 0;
        int num1 = Game1.screenWidth / 16 + 42 + 10;
        int num2 = Game1.screenHeight / 16 + 42 + 10;
        if ((int) ((double) Game1.screenPosition.X / 16.0) < (int) ((double) Game1.screenLastPosition.X / 16.0))
        {
          for (int index1 = num1 - 1; index1 > 1; --index1)
          {
            for (int index2 = 0; index2 < num2; ++index2)
              Lighting.color[index1, index2] = Lighting.color[index1 - 1, index2];
          }
        }
        else if ((int) ((double) Game1.screenPosition.X / 16.0) > (int) ((double) Game1.screenLastPosition.X / 16.0))
        {
          for (int index3 = 0; index3 < num1 - 1; ++index3)
          {
            for (int index4 = 0; index4 < num2; ++index4)
              Lighting.color[index3, index4] = Lighting.color[index3 + 1, index4];
          }
        }
        if ((int) ((double) Game1.screenPosition.Y / 16.0) < (int) ((double) Game1.screenLastPosition.Y / 16.0))
        {
          for (int index5 = num2 - 1; index5 > 1; --index5)
          {
            for (int index6 = 0; index6 < num1; ++index6)
              Lighting.color[index6, index5] = Lighting.color[index6, index5 - 1];
          }
        }
        else
        {
          if ((int) ((double) Game1.screenPosition.Y / 16.0) <= (int) ((double) Game1.screenLastPosition.Y / 16.0))
            return;
          for (int index7 = 0; index7 < num2 - 1; ++index7)
          {
            for (int index8 = 0; index8 < num1; ++index8)
              Lighting.color[index8, index7] = Lighting.color[index8, index7 + 1];
          }
        }
      }
      else
      {
        Lighting.lightCounter = 0;
        Lighting.firstToLightX = Lighting.firstTileX - 21;
        Lighting.firstToLightY = Lighting.firstTileY - 21;
        Lighting.lastToLightX = Lighting.lastTileX + 21;
        Lighting.lastToLightY = Lighting.lastTileY + 21;
        for (int index9 = 0; index9 < Game1.screenWidth / 16 + 42 + 10; ++index9)
        {
          for (int index10 = 0; index10 < Game1.screenHeight / 16 + 42 + 10; ++index10)
            Lighting.color[index9, index10] = 0.0f;
        }
        for (int index = 0; index < Lighting.tempLightCount; ++index)
        {
          if (Lighting.tempLightX[index] - Lighting.firstTileX + 21 >= 0 && Lighting.tempLightX[index] - Lighting.firstTileX + 21 < Game1.screenWidth / 16 + 42 + 10 && Lighting.tempLightY[index] - Lighting.firstTileY + 21 >= 0 && Lighting.tempLightY[index] - Lighting.firstTileY + 21 < Game1.screenHeight / 16 + 42 + 10)
            Lighting.color[Lighting.tempLightX[index] - Lighting.firstTileX + 21, Lighting.tempLightY[index] - Lighting.firstTileY + 21] = Lighting.tempLight[index];
        }
        Lighting.tempLightCount = 0;
        Game1.evilTiles = 0;
        Game1.meteorTiles = 0;
        Game1.jungleTiles = 0;
        Game1.dungeonTiles = 0;
        for (int firstToLightX = Lighting.firstToLightX; firstToLightX < Lighting.lastToLightX; ++firstToLightX)
        {
          for (int firstToLightY = Lighting.firstToLightY; firstToLightY < Lighting.lastToLightY; ++firstToLightY)
          {
            if (firstToLightX >= 0 && firstToLightX < Game1.maxTilesX && firstToLightY >= 0 && firstToLightY < Game1.maxTilesY)
            {
              if (Game1.tile[firstToLightX, firstToLightY] == null)
                Game1.tile[firstToLightX, firstToLightY] = new Tile();
              if (Game1.tile[firstToLightX, firstToLightY].active)
              {
                if (Game1.tile[firstToLightX, firstToLightY].type == (byte) 23 || Game1.tile[firstToLightX, firstToLightY].type == (byte) 24 || Game1.tile[firstToLightX, firstToLightY].type == (byte) 25 || Game1.tile[firstToLightX, firstToLightY].type == (byte) 32)
                  ++Game1.evilTiles;
                else if (Game1.tile[firstToLightX, firstToLightY].type == (byte) 27)
                  Game1.evilTiles -= 5;
                else if (Game1.tile[firstToLightX, firstToLightY].type == (byte) 37)
                  ++Game1.meteorTiles;
                else if (Game1.tileDungeon[(int) Game1.tile[firstToLightX, firstToLightY].type])
                  ++Game1.dungeonTiles;
                else if (Game1.tile[firstToLightX, firstToLightY].type == (byte) 60 || Game1.tile[firstToLightX, firstToLightY].type == (byte) 61)
                  ++Game1.jungleTiles;
              }
              if (Game1.tile[firstToLightX, firstToLightY] == null)
                Game1.tile[firstToLightX, firstToLightY] = new Tile();
              if (Game1.lightTiles)
                Lighting.color[firstToLightX - Lighting.firstTileX + 21, firstToLightY - Lighting.firstTileY + 21] = (float) Game1.tileColor.R / (float) byte.MaxValue;
              if (Game1.tile[firstToLightX, firstToLightY].lava)
              {
                float num = (float) ((double) ((int) Game1.tile[firstToLightX, firstToLightY].liquid / (int) byte.MaxValue) * 0.60000002384185791 + 0.10000000149011612);
                if ((double) Lighting.color[firstToLightX - Lighting.firstToLightX, firstToLightY - Lighting.firstToLightY] < (double) num)
                  Lighting.color[firstToLightX - Lighting.firstToLightX, firstToLightY - Lighting.firstToLightY] = num;
              }
              if ((!Game1.tile[firstToLightX, firstToLightY].active || !Game1.tileSolid[(int) Game1.tile[firstToLightX, firstToLightY].type] || Game1.tile[firstToLightX, firstToLightY].type == (byte) 37 || Game1.tile[firstToLightX, firstToLightY].type == (byte) 70) && (Game1.tile[firstToLightX, firstToLightY].lighted || Game1.tile[firstToLightX, firstToLightY].type == (byte) 4 || Game1.tile[firstToLightX, firstToLightY].type == (byte) 17 || Game1.tile[firstToLightX, firstToLightY].type == (byte) 31 || Game1.tile[firstToLightX, firstToLightY].type == (byte) 33 || Game1.tile[firstToLightX, firstToLightY].type == (byte) 34 || Game1.tile[firstToLightX, firstToLightY].type == (byte) 35 || Game1.tile[firstToLightX, firstToLightY].type == (byte) 36 || Game1.tile[firstToLightX, firstToLightY].type == (byte) 37 || Game1.tile[firstToLightX, firstToLightY].type == (byte) 42 || Game1.tile[firstToLightX, firstToLightY].type == (byte) 49 || Game1.tile[firstToLightX, firstToLightY].type == (byte) 61 || Game1.tile[firstToLightX, firstToLightY].type == (byte) 70 || Game1.tile[firstToLightX, firstToLightY].type == (byte) 71 || Game1.tile[firstToLightX, firstToLightY].type == (byte) 72))
              {
                if ((double) Lighting.color[firstToLightX - Lighting.firstToLightX, firstToLightY - Lighting.firstToLightY] * (double) byte.MaxValue < (double) Game1.tileColor.R && (double) Game1.tileColor.R > (double) Lighting.color[firstToLightX - Lighting.firstToLightX, firstToLightY - Lighting.firstToLightY] * (double) byte.MaxValue && Game1.tile[firstToLightX, firstToLightY].wall == (byte) 0 && (double) firstToLightY < Game1.worldSurface && Game1.tile[firstToLightX, firstToLightY].liquid < (byte) 128)
                  Lighting.color[firstToLightX - Lighting.firstToLightX, firstToLightY - Lighting.firstToLightY] = (float) Game1.tileColor.R / (float) byte.MaxValue;
                if (Game1.tile[firstToLightX, firstToLightY].type == (byte) 4 || Game1.tile[firstToLightX, firstToLightY].type == (byte) 33 || Game1.tile[firstToLightX, firstToLightY].type == (byte) 34 || Game1.tile[firstToLightX, firstToLightY].type == (byte) 35 || Game1.tile[firstToLightX, firstToLightY].type == (byte) 36)
                  Lighting.color[firstToLightX - Lighting.firstToLightX, firstToLightY - Lighting.firstToLightY] = 1f;
                else if (Game1.tile[firstToLightX, firstToLightY].type == (byte) 17 && (double) Lighting.color[firstToLightX - Lighting.firstToLightX, firstToLightY - Lighting.firstToLightY] < 0.800000011920929)
                  Lighting.color[firstToLightX - Lighting.firstToLightX, firstToLightY - Lighting.firstToLightY] = 0.8f;
                else if (Game1.tile[firstToLightX, firstToLightY].type == (byte) 37 && (double) Lighting.color[firstToLightX - Lighting.firstToLightX, firstToLightY - Lighting.firstToLightY] < 0.60000002384185791)
                  Lighting.color[firstToLightX - Lighting.firstToLightX, firstToLightY - Lighting.firstToLightY] = 0.6f;
                else if (Game1.tile[firstToLightX, firstToLightY].type == (byte) 58 && (double) Lighting.color[firstToLightX - Lighting.firstToLightX, firstToLightY - Lighting.firstToLightY] < 0.75)
                  Lighting.color[firstToLightX - Lighting.firstToLightX, firstToLightY - Lighting.firstToLightY] = 0.75f;
                else if (Game1.tile[firstToLightX, firstToLightY].type == (byte) 42 && (double) Lighting.color[firstToLightX - Lighting.firstToLightX, firstToLightY - Lighting.firstToLightY] < 0.75)
                  Lighting.color[firstToLightX - Lighting.firstToLightX, firstToLightY - Lighting.firstToLightY] = 0.75f;
                else if (Game1.tile[firstToLightX, firstToLightY].type == (byte) 49 && (double) Lighting.color[firstToLightX - Lighting.firstToLightX, firstToLightY - Lighting.firstToLightY] < 0.75)
                  Lighting.color[firstToLightX - Lighting.firstToLightX, firstToLightY - Lighting.firstToLightY] = 0.75f;
                else if (Game1.tile[firstToLightX, firstToLightY].type == (byte) 70 || Game1.tile[firstToLightX, firstToLightY].type == (byte) 71 || Game1.tile[firstToLightX, firstToLightY].type == (byte) 72)
                {
                  float num = (float) Game1.rand.Next(48, 52) * 0.01f;
                  if ((double) Lighting.color[firstToLightX - Lighting.firstToLightX, firstToLightY - Lighting.firstToLightY] < (double) num)
                    Lighting.color[firstToLightX - Lighting.firstToLightX, firstToLightY - Lighting.firstToLightY] = num;
                }
                else if (Game1.tile[firstToLightX, firstToLightY].type == (byte) 61 && Game1.tile[firstToLightX, firstToLightY].frameX == (short) 144 && (double) Lighting.color[firstToLightX - Lighting.firstToLightX, firstToLightY - Lighting.firstToLightY] < 0.75)
                  Lighting.color[firstToLightX - Lighting.firstToLightX, firstToLightY - Lighting.firstToLightY] = 0.75f;
                else if (Game1.tile[firstToLightX, firstToLightY].type == (byte) 31)
                {
                  float num = (float) Game1.rand.Next(-5, 6) * 0.01f;
                  if ((double) Lighting.color[firstToLightX - Lighting.firstToLightX, firstToLightY - Lighting.firstToLightY] < 0.40000000596046448 + (double) num)
                    Lighting.color[firstToLightX - Lighting.firstToLightX, firstToLightY - Lighting.firstToLightY] = 0.4f + num;
                }
              }
            }
          }
        }
        for (int index = 0; index < Lighting.lightPasses; ++index)
        {
          for (int firstToLightX = Lighting.firstToLightX; firstToLightX < Lighting.lastToLightX; ++firstToLightX)
          {
            Lighting.lightColor = 0.0f;
            for (int firstToLightY = Lighting.firstToLightY; firstToLightY < Lighting.lastToLightY; ++firstToLightY)
              Lighting.LightColor(firstToLightX, firstToLightY);
          }
          for (int firstToLightX = Lighting.firstToLightX; firstToLightX < Lighting.lastToLightX; ++firstToLightX)
          {
            Lighting.lightColor = 0.0f;
            for (int lastToLightY = Lighting.lastToLightY; lastToLightY >= Lighting.firstToLightY; --lastToLightY)
              Lighting.LightColor(firstToLightX, lastToLightY);
          }
          for (int firstToLightY = Lighting.firstToLightY; firstToLightY < Lighting.lastToLightY; ++firstToLightY)
          {
            Lighting.lightColor = 0.0f;
            for (int firstToLightX = Lighting.firstToLightX; firstToLightX < Lighting.lastToLightX; ++firstToLightX)
              Lighting.LightColor(firstToLightX, firstToLightY);
          }
          for (int firstToLightY = Lighting.firstToLightY; firstToLightY < Lighting.lastToLightY; ++firstToLightY)
          {
            Lighting.lightColor = 0.0f;
            for (int lastToLightX = Lighting.lastToLightX; lastToLightX >= Lighting.firstToLightX; --lastToLightX)
              Lighting.LightColor(lastToLightX, firstToLightY);
          }
        }
      }
    }

    public static void addLight(int i, int j, float Lightness)
    {
      if (Lighting.tempLightCount == Lighting.maxTempLights || i - Lighting.firstTileX + 21 < 0 || i - Lighting.firstTileX + 21 >= Game1.screenWidth / 16 + 42 + 10 || j - Lighting.firstTileY + 21 < 0 || j - Lighting.firstTileY + 21 >= Game1.screenHeight / 16 + 42 + 10)
        return;
      for (int index = 0; index < Lighting.tempLightCount; ++index)
      {
        if (Lighting.tempLightX[index] == i && Lighting.tempLightY[index] == j && (double) Lightness <= (double) Lighting.tempLight[index])
          return;
      }
      Lighting.tempLightX[Lighting.tempLightCount] = i;
      Lighting.tempLightY[Lighting.tempLightCount] = j;
      Lighting.tempLight[Lighting.tempLightCount] = Lightness;
      ++Lighting.tempLightCount;
    }

    private static void LightColor(int i, int j)
    {
      int index1 = i - Lighting.firstToLightX;
      int index2 = j - Lighting.firstToLightY;
      try
      {
        if ((double) Lighting.color[index1, index2] > (double) Lighting.lightColor)
        {
          Lighting.lightColor = Lighting.color[index1, index2];
        }
        else
        {
          if ((double) Lighting.lightColor == 0.0)
            return;
          Lighting.color[index1, index2] = Lighting.lightColor;
        }
        float num = 0.04f;
        if (Game1.tile[i, j].active && Game1.tileBlockLight[(int) Game1.tile[i, j].type])
          num = 0.16f;
        if ((double) (Lighting.lightColor - num) < 0.0)
        {
          Lighting.lightColor = 0.0f;
        }
        else
        {
          Lighting.lightColor -= num;
          if ((double) Lighting.lightColor > 0.0 && (!Game1.tile[i, j].active || !Game1.tileSolid[(int) Game1.tile[i, j].type]) && (double) j < Game1.worldSurface)
            Game1.tile[i, j].lighted = true;
        }
      }
      catch
      {
      }
    }

    public static int LightingX(int lightX)
    {
      if (lightX < 0)
        return 0;
      return lightX >= Game1.screenWidth / 16 + 42 + 10 ? Game1.screenWidth / 16 + 42 + 10 - 1 : lightX;
    }

    public static int LightingY(int lightY)
    {
      if (lightY < 0)
        return 0;
      return lightY >= Game1.screenHeight / 16 + 42 + 10 ? Game1.screenHeight / 16 + 42 + 10 - 1 : lightY;
    }

    public static Color GetColor(int x, int y, Color oldColor)
    {
      int index1 = x - Lighting.firstTileX + 21;
      int index2 = y - Lighting.firstTileY + 21;
      if (Game1.gameMenu)
        return oldColor;
      if (index1 < 0 || index2 < 0 || index1 >= Game1.screenWidth / 16 + 42 + 10 || index2 >= Game1.screenHeight / 16 + 42 + 10)
        return Color.Black;
      return Color.White with
      {
        R = (byte) ((double) oldColor.R * (double) Lighting.color[index1, index2]),
        G = (byte) ((double) oldColor.G * (double) Lighting.color[index1, index2]),
        B = (byte) ((double) oldColor.B * (double) Lighting.color[index1, index2])
      };
    }

    public static Color GetColor(int x, int y)
    {
      int index1 = x - Lighting.firstTileX + 21;
      int index2 = y - Lighting.firstTileY + 21;
      return index1 < 0 || index2 < 0 || index1 >= Game1.screenWidth / 16 + 42 + 10 || index2 >= Game1.screenHeight / 16 + 42 + 10 ? Color.Black : new Color((int) (byte) ((double) byte.MaxValue * (double) Lighting.color[index1, index2]), (int) (byte) ((double) byte.MaxValue * (double) Lighting.color[index1, index2]), (int) (byte) ((double) byte.MaxValue * (double) Lighting.color[index1, index2]), (int) byte.MaxValue);
    }

    public static Color GetBlackness(int x, int y)
    {
      int index1 = x - Lighting.firstTileX + 21;
      int index2 = y - Lighting.firstTileY + 21;
      return index1 < 0 || index2 < 0 || index1 >= Game1.screenWidth / 16 + 42 + 10 || index2 >= Game1.screenHeight / 16 + 42 + 10 ? Color.Black : new Color(0, 0, 0, (int) (byte) ((double) byte.MaxValue - (double) byte.MaxValue * (double) Lighting.color[index1, index2]));
    }

    public static float Brightness(int x, int y)
    {
      int index1 = x - Lighting.firstTileX + 21;
      int index2 = y - Lighting.firstTileY + 21;
      return index1 < 0 || index2 < 0 || index1 >= Game1.screenWidth / 16 + 42 + 10 || index2 >= Game1.screenHeight / 16 + 42 + 10 ? 0.0f : Lighting.color[index1, index2];
    }
  }
}
