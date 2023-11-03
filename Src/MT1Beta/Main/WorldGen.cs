// Decompiled with JetBrains decompiler
// Type: GameManager.WorldGen
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 09A1FDD7-68B3-40FF-A030-32890DF6B0E7
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager v1 Beta\GameManager.exe

using Microsoft.Xna.Framework;
using System;
using System.IO;
using System.Threading;
using Windows.System.Threading;

namespace GameManager
{
  internal class WorldGen
  {
    public static int lavaLine;
    public static int waterLine;
    public static bool gen = false;
    public static bool shadowOrbSmashed = false;
    public static bool spawnMeteor = false;
    public static bool loadFailed = false;
    public static bool worldCleared = false;
    private static int lastMaxTilesX = 0;
    private static int lastMaxTilesY = 0;
    public static bool saveLock = false;
    private static bool mergeUp = false;
    private static bool mergeDown = false;
    private static bool mergeLeft = false;
    private static bool mergeRight = false;
    private static int tempMoonPhase = Game1.moonPhase;
    private static bool tempDayTime = Game1.dayTime;
    private static bool tempBloodMoon = Game1.bloodMoon;
    private static double tempTime = Game1.time;
    private static bool stopDrops = false;
    public static bool noLiquidCheck = false;
    [ThreadStatic]
    public static Random genRand = new Random();
    public static string statusText = "";
    private static bool destroyObject = false;
    public static int spawnDelay = 0;
    public static int spawnNPC = 0;
    public static int maxRoomTiles = 1900;
    public static int numRoomTiles;
    public static int[] roomX = new int[WorldGen.maxRoomTiles];
    public static int[] roomY = new int[WorldGen.maxRoomTiles];
    public static int roomX1;
    public static int roomX2;
    public static int roomY1;
    public static int roomY2;
    public static bool canSpawn;
    public static bool[] houseTile = new bool[76];
    public static int bestX = 0;
    public static int bestY = 0;
    public static int hiScore = 0;
    public static int dungeonX;
    public static int dungeonY;
    public static Vector2 lastDungeonHall = new Vector2();
    public static int maxDRooms = 100;
    public static int numDRooms = 0;
    public static int[] dRoomX = new int[WorldGen.maxDRooms];
    public static int[] dRoomY = new int[WorldGen.maxDRooms];
    public static int[] dRoomSize = new int[WorldGen.maxDRooms];
    private static bool[] dRoomTreasure = new bool[WorldGen.maxDRooms];
    private static int[] dRoomL = new int[WorldGen.maxDRooms];
    private static int[] dRoomR = new int[WorldGen.maxDRooms];
    private static int[] dRoomT = new int[WorldGen.maxDRooms];
    private static int[] dRoomB = new int[WorldGen.maxDRooms];
    private static int numDDoors;
    private static int[] DDoorX = new int[300];
    private static int[] DDoorY = new int[300];
    private static int[] DDoorPos = new int[300];
    private static int numDPlats;
    private static int[] DPlatX = new int[300];
    private static int[] DPlatY = new int[300];
    public static int dEnteranceX = 0;
    public static bool dSurface = false;
    private static double dxStrength1;
    private static double dyStrength1;
    private static double dxStrength2;
    private static double dyStrength2;
    private static int dMinX;
    private static int dMaxX;
    private static int dMinY;
    private static int dMaxY;
    private static int numIslandHouses = 0;
    private static int houseCount = 0;
    private static int[] fihX = new int[300];
    private static int[] fihY = new int[300];
    private static int numMCaves = 0;
    private static int[] mCaveX = new int[300];
    private static int[] mCaveY = new int[300];

    public static void SpawnNPC(int x, int y)
    {
      if (Game1.wallHouse[(int) Game1.tile[x, y].wall])
        WorldGen.canSpawn = true;
      if (!WorldGen.canSpawn || !WorldGen.StartRoomCheck(x, y) || !WorldGen.RoomNeeds(WorldGen.spawnNPC))
        return;
      WorldGen.ScoreRoom();
      if (WorldGen.hiScore <= 0)
        return;
      int index1 = -1;
      for (int index2 = 0; index2 < 1000; ++index2)
      {
        if (Game1.npc[index2].active && Game1.npc[index2].homeless && Game1.npc[index2].type == WorldGen.spawnNPC)
        {
          index1 = index2;
          break;
        }
      }
      if (index1 == -1)
      {
        int index3 = WorldGen.bestX;
        int index4 = WorldGen.bestY;
        bool flag = false;
        Rectangle rectangle1;
        Rectangle rectangle2;
        if (!flag)
        {
          flag = true;
          rectangle1 = new Rectangle(index3 * 16 + 8 - Game1.screenWidth / 2 - NPC.safeRangeX, index4 * 16 + 8 - Game1.screenHeight / 2 - NPC.safeRangeY, Game1.screenWidth + NPC.safeRangeX * 2, Game1.screenHeight + NPC.safeRangeY * 2);
          for (int index5 = 0; index5 < 8; ++index5)
          {
            if (Game1.player[index5].active)
            {
              rectangle2 = new Rectangle((int) Game1.player[index5].position.X, (int) Game1.player[index5].position.Y, Game1.player[index5].width, Game1.player[index5].height);
              if (rectangle2.Intersects(rectangle1))
              {
                flag = false;
                break;
              }
            }
          }
        }
        if (!flag)
        {
          for (int index6 = 1; index6 < 500; ++index6)
          {
            for (int index7 = 0; index7 < 2; ++index7)
            {
              index3 = index7 != 0 ? WorldGen.bestX - index6 : WorldGen.bestX + index6;
              if (index3 > 10 && index3 < Game1.maxTilesX - 10)
              {
                int num1 = WorldGen.bestY - index6;
                double num2 = (double) (WorldGen.bestY + index6);
                if (num1 < 10)
                  num1 = 10;
                if (num2 > Game1.worldSurface)
                  num2 = Game1.worldSurface;
                for (int index8 = num1; (double) index8 < num2; ++index8)
                {
                  index4 = index8;
                  if (Game1.tile[index3, index4].active && Game1.tileSolid[(int) Game1.tile[index3, index4].type])
                  {
                    if (!Collision.SolidTiles(index3 - 1, index3 + 1, index4 - 3, index4 - 1))
                    {
                      flag = true;
                      rectangle1 = new Rectangle(index3 * 16 + 8 - Game1.screenWidth / 2 - NPC.safeRangeX, index4 * 16 + 8 - Game1.screenHeight / 2 - NPC.safeRangeY, Game1.screenWidth + NPC.safeRangeX * 2, Game1.screenHeight + NPC.safeRangeY * 2);
                      for (int index9 = 0; index9 < 8; ++index9)
                      {
                        if (Game1.player[index9].active)
                        {
                          rectangle2 = new Rectangle((int) Game1.player[index9].position.X, (int) Game1.player[index9].position.Y, Game1.player[index9].width, Game1.player[index9].height);
                          if (rectangle2.Intersects(rectangle1))
                          {
                            flag = false;
                            break;
                          }
                        }
                      }
                      break;
                    }
                    break;
                  }
                }
              }
              if (flag)
                break;
            }
            if (flag)
              break;
          }
        }
        int index10 = NPC.NewNPC(index3 * 16, index4 * 16, WorldGen.spawnNPC, 1);
        Game1.npc[index10].homeTileX = WorldGen.bestX;
        Game1.npc[index10].homeTileY = WorldGen.bestY;
        if (index3 < WorldGen.bestX)
          Game1.npc[index10].direction = 1;
        else if (index3 > WorldGen.bestX)
          Game1.npc[index10].direction = -1;
        Game1.npc[index10].netUpdate = true;
        switch (Game1.netMode)
        {
          case 0:
            Game1.NewText(Game1.npc[index10].name + " has arrived!", (byte) 50, (byte) 125);
            break;
          case 2:
            NetMessage.SendData(25, text: Game1.npc[index10].name + " has arrived!", 
                number: 8, number2: 50f, number3: 125f, number4: (float) byte.MaxValue);
            break;
        }
      }
      else
      {
        WorldGen.spawnNPC = 0;
        Game1.npc[index1].homeTileX = WorldGen.bestX;
        Game1.npc[index1].homeTileY = WorldGen.bestY;
        Game1.npc[index1].homeless = false;
      }
      WorldGen.spawnNPC = 0;
    }

    public static bool RoomNeeds(int npcType)
    {
      WorldGen.canSpawn = WorldGen.houseTile[15] 
                && (WorldGen.houseTile[14] || WorldGen.houseTile[18])
                && (WorldGen.houseTile[4] || WorldGen.houseTile[33] 
                || WorldGen.houseTile[34] || WorldGen.houseTile[35] 
                || WorldGen.houseTile[36] || WorldGen.houseTile[42] 
                || WorldGen.houseTile[49])
                && (WorldGen.houseTile[10] || WorldGen.houseTile[11]);
      return WorldGen.canSpawn;
    }

    public static void QuickFindHome(int npc)
    {
      if (Game1.npc[npc].homeTileX <= 10 || Game1.npc[npc].homeTileY <= 10
                || Game1.npc[npc].homeTileX >= Game1.maxTilesX - 10
                || Game1.npc[npc].homeTileY >= Game1.maxTilesY)
        return;

      WorldGen.canSpawn = false;
      WorldGen.StartRoomCheck(Game1.npc[npc].homeTileX, Game1.npc[npc].homeTileY - 1);
      if (!WorldGen.canSpawn)
      {
        for (int x = Game1.npc[npc].homeTileX - 1; x < Game1.npc[npc].homeTileX + 2; ++x)
        {
          int y = Game1.npc[npc].homeTileY - 1;
          while (y < Game1.npc[npc].homeTileY + 2 && !WorldGen.StartRoomCheck(x, y))
            ++y;
        }
      }
      if (!WorldGen.canSpawn)
      {
        int num = 10;
        for (int x = Game1.npc[npc].homeTileX - num; x <= Game1.npc[npc].homeTileX + num; x += 2)
        {
          int y = Game1.npc[npc].homeTileY - num;
          while (y <= Game1.npc[npc].homeTileY + num && !WorldGen.StartRoomCheck(x, y))
            y += 2;
        }
      }
      if (WorldGen.canSpawn)
      {
        WorldGen.RoomNeeds(Game1.npc[npc].type);
        if (WorldGen.canSpawn)
          WorldGen.ScoreRoom(npc);
        if (WorldGen.canSpawn && WorldGen.hiScore > 0)
        {
          Game1.npc[npc].homeTileX = WorldGen.bestX;
          Game1.npc[npc].homeTileY = WorldGen.bestY;
          Game1.npc[npc].homeless = false;
          WorldGen.canSpawn = false;
        }
        else
          Game1.npc[npc].homeless = true;
      }
      else
        Game1.npc[npc].homeless = true;
    }

    public static void ScoreRoom(int ignoreNPC = -1)
    {
      for (int index1 = 0; index1 < 1000; ++index1)
      {
        if (Game1.npc[index1].active && Game1.npc[index1].townNPC && ignoreNPC != index1 && !Game1.npc[index1].homeless)
        {
          for (int index2 = 0; index2 < WorldGen.numRoomTiles; ++index2)
          {
            if (Game1.npc[index1].homeTileX == WorldGen.roomX[index2] && Game1.npc[index1].homeTileY == WorldGen.roomY[index2])
            {
              WorldGen.hiScore = -1;
              return;
            }
          }
        }
      }
      WorldGen.hiScore = 0;
      int num1 = 0;
      int num2 = WorldGen.roomX1 - Game1.screenWidth / 2 / 16 - 1 - 21;
      int num3 = WorldGen.roomX2 + Game1.screenWidth / 2 / 16 + 1 + 21;
      int num4 = WorldGen.roomY1 - Game1.screenHeight / 2 / 16 - 1 - 21;
      int num5 = WorldGen.roomY2 + Game1.screenHeight / 2 / 16 + 1 + 21;
      if (num2 < 0)
        num2 = 0;
      if (num3 >= Game1.maxTilesX)
        num3 = Game1.maxTilesX - 1;
      if (num4 < 0)
        num4 = 0;
      if (num5 > Game1.maxTilesX)
        num5 = Game1.maxTilesX;
      for (int index3 = num2 + 1; index3 < num3; ++index3)
      {
        for (int index4 = num4 + 2; index4 < num5 + 2; ++index4)
        {
          if (Game1.tile[index3, index4].active)
          {
            if ( Game1.tile[index3, index4].type == (byte) 23 
                            || Game1.tile[index3, index4].type == (byte) 24
                            || Game1.tile[index3, index4].type == (byte) 25 
                            || Game1.tile[index3, index4].type == (byte) 32 )
              ++Game1.evilTiles;
            else if (Game1.tile[index3, index4].type == (byte) 27)
              Game1.evilTiles -= 5;
          }
        }
      }
      if (num1 < 50)
        num1 = 0;
      int num6 = -num1;
      if (num6 <= -250)
      {
        WorldGen.hiScore = num6;
      }
      else
      {
        int roomX1 = WorldGen.roomX1;
        int roomX2 = WorldGen.roomX2;
        int roomY1 = WorldGen.roomY1;
        int roomY2 = WorldGen.roomY2;
        for (int index5 = roomX1 + 1; index5 < roomX2; ++index5)
        {
          for (int index6 = roomY1 + 2; index6 < roomY2 + 2; ++index6)
          {
            if (Game1.tile[index5, index6].active)
            {
              int num7 = num6;
              if (Game1.tileSolid[(int) Game1.tile[index5, index6].type] 
                                && !Game1.tileSolidTop[(int) Game1.tile[index5, index6].type] && !Collision.SolidTiles(index5 - 1, index5 + 1, index6 - 3, index6 - 1) && Game1.tile[index5 - 1, index6].active && Game1.tileSolid[(int) Game1.tile[index5 - 1, index6].type] && !Game1.tileSolidTop[(int) Game1.tile[index5 - 1, index6].type] && Game1.tile[index5 + 1, index6].active && Game1.tileSolid[(int) Game1.tile[index5 + 1, index6].type] && !Game1.tileSolidTop[(int) Game1.tile[index5 + 1, index6].type])
              {
                for (int index7 = index5 - 2; index7 < index5 + 3; ++index7)
                {
                  for (int index8 = index6 - 4; index8 < index6; ++index8)
                  {
                    if (Game1.tile[index7, index8].active)
                    {
                      if (index7 == index5)
                        num7 -= 15;
                      else if (Game1.tile[index7, index8].type == (byte) 10
                                                || Game1.tile[index7, index8].type == (byte) 11)
                        num7 -= 20;
                      else if (Game1.tileSolid[(int) Game1.tile[index7, index8].type])
                        num7 -= 5;
                      else
                        num7 += 5;
                    }
                  }
                }
                if (num7 > WorldGen.hiScore)
                {
                  bool flag = false;
                  for (int index9 = 0; index9 < WorldGen.numRoomTiles; ++index9)
                  {
                    if (WorldGen.roomX[index9] == index5 && WorldGen.roomY[index9] == index6)
                    {
                      flag = true;
                      break;
                    }
                  }
                  if (flag)
                  {
                    WorldGen.hiScore = num7;
                    WorldGen.bestX = index5;
                    WorldGen.bestY = index6;
                  }
                }
              }
            }
          }
        }
      }
    }

    public static bool StartRoomCheck(int x, int y)
    {
      WorldGen.roomX1 = x;
      WorldGen.roomX2 = x;
      WorldGen.roomY1 = y;
      WorldGen.roomY2 = y;
      WorldGen.numRoomTiles = 0;
      for (int index = 0; index < 76; ++index)
        WorldGen.houseTile[index] = false;
      WorldGen.canSpawn = true;
      if (Game1.tile[x, y].active && Game1.tileSolid[(int) Game1.tile[x, y].type])
        WorldGen.canSpawn = false;
      WorldGen.CheckRoom(x, y);
      if (WorldGen.numRoomTiles < 60)
        WorldGen.canSpawn = false;
      return WorldGen.canSpawn;
    }

    public static void CheckRoom(int x, int y)
    {
      if (!WorldGen.canSpawn)
        return;
      if (x < 10 || y < 10 || x >= Game1.maxTilesX - 10 || y >= WorldGen.lastMaxTilesY - 10)
      {
        WorldGen.canSpawn = false;
      }
      else
      {
        for (int index = 0; index < WorldGen.numRoomTiles; ++index)
        {
          if (WorldGen.roomX[index] == x && WorldGen.roomY[index] == y)
            return;
        }
        WorldGen.roomX[WorldGen.numRoomTiles] = x;
        WorldGen.roomY[WorldGen.numRoomTiles] = y;
        ++WorldGen.numRoomTiles;
        if (WorldGen.numRoomTiles >= WorldGen.maxRoomTiles)
        {
          WorldGen.canSpawn = false;
        }
        else
        {
          if (Game1.tile[x, y].active)
          {
            WorldGen.houseTile[(int) Game1.tile[x, y].type] = true;
            if (Game1.tileSolid[(int) Game1.tile[x, y].type] || Game1.tile[x, y].type == (byte) 11)
              return;
          }
          if (x < WorldGen.roomX1)
            WorldGen.roomX1 = x;
          if (x > WorldGen.roomX2)
            WorldGen.roomX2 = x;
          if (y < WorldGen.roomY1)
            WorldGen.roomY1 = y;
          if (y > WorldGen.roomY2)
            WorldGen.roomY2 = y;
          bool flag1 = false;
          bool flag2 = false;
          for (int index = -2; index < 3; ++index)
          {
            if (Game1.wallHouse[(int) Game1.tile[x + index, y].wall])
              flag1 = true;
            if (Game1.tile[x + index, y].active && (Game1.tileSolid[(int) Game1.tile[x + index, y].type] || Game1.tile[x + index, y].type == (byte) 11))
              flag1 = true;
            if (Game1.wallHouse[(int) Game1.tile[x, y + index].wall])
              flag2 = true;
            if (Game1.tile[x, y + index].active && (Game1.tileSolid[(int) Game1.tile[x, y + index].type] || Game1.tile[x, y + index].type == (byte) 11))
              flag2 = true;
          }
          if (!flag1 || !flag2)
          {
            WorldGen.canSpawn = false;
          }
          else
          {
            for (int x1 = x - 1; x1 < x + 2; ++x1)
            {
              for (int y1 = y - 1; y1 < y + 2; ++y1)
              {
                if ((x1 != x || y1 != y) && WorldGen.canSpawn)
                  WorldGen.CheckRoom(x1, y1);
              }
            }
          }
        }
      }
    }

    public static void dropMeteor()
    {
      bool flag = true;
      int num = 0;
      if (Game1.netMode == 1)
        return;
      for (int index = 0; index < 8; ++index)
      {
        if (Game1.player[index].active)
        {
          flag = false;
          break;
        }
      }
      while (!flag)
      {
        int i = Game1.rand.Next(50, Game1.maxTilesX - 50);
        for (int j = Game1.rand.Next(100); j < Game1.maxTilesY; ++j)
        {
          if (Game1.tile[i, j].active && Game1.tileSolid[(int) Game1.tile[i, j].type])
          {
            flag = WorldGen.meteor(i, j);
            break;
          }
        }
        ++num;
        if (num >= 100)
          break;
      }
    }

    public static bool meteor(int i, int j)
    {
      if (i < 50 || i > Game1.maxTilesX - 50 || j < 50 || j > Game1.maxTilesY - 50)
        return false;
      int num1 = 25;
      Rectangle rectangle1 = new Rectangle((i - num1) * 16, (j - num1) * 16, num1 * 2 * 16, num1 * 2 * 16);
      for (int index = 0; index < 8; ++index)
      {
        if (Game1.player[index].active)
        {
          Rectangle rectangle2 = new Rectangle((int) ((double) Game1.player[index].position.X + (double) (Game1.player[index].width / 2) - (double) (Game1.screenWidth / 2) - (double) NPC.safeRangeX), (int) ((double) Game1.player[index].position.Y + (double) (Game1.player[index].height / 2) - (double) (Game1.screenHeight / 2) - (double) NPC.safeRangeY), Game1.screenWidth + NPC.safeRangeX * 2, Game1.screenHeight + NPC.safeRangeY * 2);
          if (rectangle1.Intersects(rectangle2))
            return false;
        }
      }
      for (int index = 0; index < 1000; ++index)
      {
        if (Game1.npc[index].active)
        {
          Rectangle rectangle3 = new Rectangle((int) Game1.npc[index].position.X, (int) Game1.npc[index].position.Y, Game1.npc[index].width, Game1.npc[index].height);
          if (rectangle1.Intersects(rectangle3))
            return false;
        }
      }
      for (int index1 = i - num1; index1 < i + num1; ++index1)
      {
        for (int index2 = j - num1; index2 < j + num1; ++index2)
        {
          if (Game1.tile[index1, index2].active && Game1.tile[index1, index2].type == (byte) 21)
            return false;
        }
      }
      WorldGen.stopDrops = true;
      int num2 = 15;
      for (int index3 = i - num2; index3 < i + num2; ++index3)
      {
        for (int index4 = j - num2; index4 < j + num2; ++index4)
        {
          if (index4 > j + Game1.rand.Next(-2, 3) - 5 && (double) (Math.Abs(i - index3) + Math.Abs(j - index4)) < (double) num2 * 1.5 + (double) Game1.rand.Next(-5, 5))
          {
            if (!Game1.tileSolid[(int) Game1.tile[index3, index4].type])
              Game1.tile[index3, index4].active = false;
            Game1.tile[index3, index4].type = (byte) 37;
          }
        }
      }
      int num3 = 10;
      for (int index5 = i - num3; index5 < i + num3; ++index5)
      {
        for (int index6 = j - num3; index6 < j + num3; ++index6)
        {
          if (index6 > j + Game1.rand.Next(-2, 3) - 5 && Math.Abs(i - index5) + Math.Abs(j - index6) < num3 + Game1.rand.Next(-3, 4))
            Game1.tile[index5, index6].active = false;
        }
      }
      int num4 = 16;
      for (int i1 = i - num4; i1 < i + num4; ++i1)
      {
        for (int j1 = j - num4; j1 < j + num4; ++j1)
        {
          if (Game1.tile[i1, j1].type == (byte) 5 || Game1.tile[i1, j1].type == (byte) 32)
            WorldGen.KillTile(i1, j1);
          WorldGen.SquareTileFrame(i1, j1);
          WorldGen.SquareWallFrame(i1, j1);
        }
      }
      int num5 = 23;
      for (int i2 = i - num5; i2 < i + num5; ++i2)
      {
        for (int j2 = j - num5; j2 < j + num5; ++j2)
        {
          if (Game1.tile[i2, j2].active && Game1.rand.Next(10) == 0 && (double) (Math.Abs(i - i2) + Math.Abs(j - j2)) < (double) num5 * 1.3)
          {
            if (Game1.tile[i2, j2].type == (byte) 5 || Game1.tile[i2, j2].type == (byte) 32)
              WorldGen.KillTile(i2, j2);
            Game1.tile[i2, j2].type = (byte) 37;
            WorldGen.SquareTileFrame(i2, j2);
          }
        }
      }
      WorldGen.stopDrops = false;
      if (Game1.netMode == 0)
        Game1.NewText("A meteorite has landed!", (byte) 50, B: (byte) 130);
      else if (Game1.netMode == 2)
        NetMessage.SendData(25, text: "A meteorite has landed!", number: 8, number2: 50f, number3: (float) byte.MaxValue, number4: 130f);
      if (Game1.netMode != 1)
        NetMessage.SendTileSquare(-1, i, j, 30);
      return true;
    }

    public static void setWorldSize()
    {
      Game1.bottomWorld = (float) (Game1.maxTilesY * 16);
      Game1.rightWorld = (float) (Game1.maxTilesX * 16);
      Game1.maxSectionsX = Game1.maxTilesX / 200;
      Game1.maxSectionsY = Game1.maxTilesY / 150;
    }

    public static void worldGenCallBack(object threadContext)
    {
      Game1.PlaySound(10);
      WorldGen.clearWorld();
      WorldGen.generateWorld();
      WorldGen.saveWorld(true);
      Game1.LoadWorlds();
      if (Game1.menuMode == 10)
        Game1.menuMode = 6;
      Game1.PlaySound(10);
    }

     public static void CreateNewWorld()
     {
            //RnD
            //ThreadPool.QueueUserWorkItem(new WaitCallback(WorldGen.worldGenCallBack), (object)1); 
            worldGenCallBack(default);
        }

    public static void SaveAndQuitCallBack(object threadContext)
    {
      Game1.menuMode = 10;
      Game1.gameMenu = true;
      Player.SavePlayer(Game1.player[Game1.myPlayer], Game1.playerPathName);
      if (Game1.netMode == 0)
      {
        WorldGen.saveWorld();
        Game1.PlaySound(10);
      }
      else
      {
        Netplay.disconnect = true;
        Game1.netMode = 0;
      }
      Game1.menuMode = 0;
    }

    public static void SaveAndQuit()
    {
      Game1.PlaySound(11);

        //RnD
        //ThreadPool.QueueUserWorkItem(new WaitCallback(WorldGen.SaveAndQuitCallBack), (object) 1);
        WorldGen.SaveAndQuitCallBack(default);
    }

    public static void playWorldCallBack(object threadContext)
    {
      if (Game1.rand == null)
        Game1.rand = new Random((int) DateTime.Now.Ticks);
      for (int index = 0; index < 8; ++index)
      {
        if (index != Game1.myPlayer)
          Game1.player[index].active = false;
      }
      WorldGen.loadWorld();
      if (WorldGen.loadFailed)
        return;
      WorldGen.EveryTileFrame();
      if (Game1.gameMenu)
        Game1.gameMenu = false;
      Game1.player[Game1.myPlayer].Spawn();
      Game1.player[Game1.myPlayer].UpdatePlayer(Game1.myPlayer);
      Game1.dayTime = WorldGen.tempDayTime;
      Game1.time = WorldGen.tempTime;
      Game1.moonPhase = WorldGen.tempMoonPhase;
      Game1.bloodMoon = WorldGen.tempBloodMoon;
      Game1.PlaySound(11);
      Game1.resetClouds = true;
    }

        public static void playWorld()
        {
            //RnD
            //ThreadPool.QueueUserWorkItem(new WaitCallback(WorldGen.playWorldCallBack), (object)1);
            WorldGen.playWorldCallBack(default);
        }

    public static void saveAndPlayCallBack(object threadContext) => WorldGen.saveWorld();

        public static void saveAndPlay()
        {
            //RnD
            //ThreadPool.QueueUserWorkItem(new WaitCallback(WorldGen.saveAndPlayCallBack), (object)1);
            WorldGen.saveAndPlayCallBack(default);
        }
    public static void serverLoadWorldCallBack(object threadContext)
    {
      WorldGen.loadWorld();
      if (WorldGen.loadFailed)
        return;
      Game1.PlaySound(10);
      Netplay.StartServer();
      Game1.dayTime = WorldGen.tempDayTime;
      Game1.time = WorldGen.tempTime;
      Game1.moonPhase = WorldGen.tempMoonPhase;
      Game1.bloodMoon = WorldGen.tempBloodMoon;
    }

    public static void serverLoadWorld()
    {
        //RnD
        //ThreadPool.QueueUserWorkItem(new WaitCallback(WorldGen.serverLoadWorldCallBack), (object)1);
        WorldGen.serverLoadWorldCallBack(default);
    }

    public static void clearWorld()
    {
      Game1.helpText = 0;
      Game1.dungeonX = 0;
      Game1.dungeonY = 0;
      NPC.downedBoss1 = false;
      NPC.downedBoss2 = false;
      NPC.downedBoss3 = false;
      WorldGen.shadowOrbSmashed = false;
      WorldGen.spawnMeteor = false;
      WorldGen.stopDrops = false;
      Game1.invasionDelay = 0;
      Game1.invasionType = 0;
      Game1.invasionSize = 0;
      Game1.invasionWarn = 0;
      Game1.invasionX = 0.0;
      WorldGen.noLiquidCheck = false;
      Liquid.numLiquid = 0;
      LiquidBuffer.numLiquidBuffer = 0;
      if (Game1.netMode == 1 || WorldGen.lastMaxTilesX > Game1.maxTilesX || WorldGen.lastMaxTilesY > Game1.maxTilesY)
      {
        for (int index1 = 0; index1 < WorldGen.lastMaxTilesX; ++index1)
        {
          Game1.statusText = "Freeing unused resources: " + (object) (int) ((double) ((float) index1 / (float) WorldGen.lastMaxTilesX) * 100.0 + 1.0) + "%";
          for (int index2 = 0; index2 < WorldGen.lastMaxTilesY; ++index2)
            Game1.tile[index1, index2] = (Tile) null;
        }
      }
      WorldGen.lastMaxTilesX = Game1.maxTilesX;
      WorldGen.lastMaxTilesY = Game1.maxTilesY;
      if (Game1.netMode != 1)
      {
        for (int index3 = 0; index3 < Game1.maxTilesX; ++index3)
        {
          Game1.statusText = "Resetting game objects: " + (object) (int) ((double) ((float) index3 / (float) Game1.maxTilesX) * 100.0 + 1.0) + "%";
          for (int index4 = 0; index4 < Game1.maxTilesY; ++index4)
            Game1.tile[index3, index4] = new Tile();
        }
      }
      for (int index = 0; index < 2000; ++index)
        Game1.dust[index] = new Dust();
      for (int index = 0; index < 200; ++index)
        Game1.gore[index] = new Gore();
      for (int index = 0; index < 200; ++index)
        Game1.item[index] = new Item();
      for (int index = 0; index < 1000; ++index)
        Game1.npc[index] = new NPC();
      for (int index = 0; index < 1000; ++index)
        Game1.projectile[index] = new Projectile();
      for (int index = 0; index < 1000; ++index)
        Game1.chest[index] = (Chest) null;
      for (int index = 0; index < 1000; ++index)
        Game1.sign[index] = (Sign) null;
      for (int index = 0; index < Liquid.resLiquid; ++index)
        Game1.liquid[index] = new Liquid();
      for (int index = 0; index < 10000; ++index)
        Game1.liquidBuffer[index] = new LiquidBuffer();
      WorldGen.setWorldSize();
      WorldGen.worldCleared = true;
    }

    public static void saveWorld(bool resetTime = false)
    {
      if (WorldGen.saveLock)
        return;
      WorldGen.saveLock = true;
      if (Game1.skipMenu)
        return;
      bool flag = Game1.dayTime;
      WorldGen.tempTime = Game1.time;
      WorldGen.tempMoonPhase = Game1.moonPhase;
      WorldGen.tempBloodMoon = Game1.bloodMoon;
      if (resetTime)
      {
        flag = true;
        WorldGen.tempTime = 13500.0;
        WorldGen.tempMoonPhase = 0;
        WorldGen.tempBloodMoon = false;
      }
      if (Game1.worldPathName == null)
        return;
      Game1.statusText = "Backing up world file...";
      string destFileName = Game1.worldPathName + ".bak";
      if (File.Exists(Game1.worldPathName))
        File.Copy(Game1.worldPathName, destFileName, true);
      using (FileStream output = new FileStream(Game1.worldPathName, FileMode.Create))
      {
        using (BinaryWriter binaryWriter = new BinaryWriter((Stream) output))
        {
          binaryWriter.Write(Game1.curRelease);
          binaryWriter.Write(Game1.worldName);
          binaryWriter.Write((int) Game1.leftWorld);
          binaryWriter.Write((int) Game1.rightWorld);
          binaryWriter.Write((int) Game1.topWorld);
          binaryWriter.Write((int) Game1.bottomWorld);
          binaryWriter.Write(Game1.maxTilesY);
          binaryWriter.Write(Game1.maxTilesX);
          binaryWriter.Write(Game1.spawnTileX);
          binaryWriter.Write(Game1.spawnTileY);
          binaryWriter.Write(Game1.worldSurface);
          binaryWriter.Write(Game1.rockLayer);
          binaryWriter.Write(WorldGen.tempTime);
          binaryWriter.Write(flag);
          binaryWriter.Write(WorldGen.tempMoonPhase);
          binaryWriter.Write(WorldGen.tempBloodMoon);
          binaryWriter.Write(Game1.dungeonX);
          binaryWriter.Write(Game1.dungeonY);
          binaryWriter.Write(NPC.downedBoss1);
          binaryWriter.Write(NPC.downedBoss2);
          binaryWriter.Write(NPC.downedBoss3);
          binaryWriter.Write(WorldGen.shadowOrbSmashed);
          binaryWriter.Write(WorldGen.spawnMeteor);
          binaryWriter.Write(Game1.invasionDelay);
          binaryWriter.Write(Game1.invasionSize);
          binaryWriter.Write(Game1.invasionType);
          binaryWriter.Write(Game1.invasionX);
          for (int index1 = 0; index1 < Game1.maxTilesX; ++index1)
          {
            Game1.statusText = "Saving world data: " + (object) (int) (
                            (double) ((float) index1 / (float) Game1.maxTilesX) * 100.0 + 1.0) + "%";
            for (int index2 = 0; index2 < Game1.maxTilesY; ++index2)
            {
              binaryWriter.Write(Game1.tile[index1, index2].active);
              if (Game1.tile[index1, index2].active)
              {
                binaryWriter.Write(Game1.tile[index1, index2].type);
                if (Game1.tileFrameImportant[(int) Game1.tile[index1, index2].type])
                {
                  binaryWriter.Write(Game1.tile[index1, index2].frameX);
                  binaryWriter.Write(Game1.tile[index1, index2].frameY);
                }
              }
              binaryWriter.Write(Game1.tile[index1, index2].lighted);
              if (Game1.tile[index1, index2].wall > (byte) 0)
              {
                binaryWriter.Write(true);
                binaryWriter.Write(Game1.tile[index1, index2].wall);
              }
              else
                binaryWriter.Write(false);
              if (Game1.tile[index1, index2].liquid > (byte) 0)
              {
                binaryWriter.Write(true);
                binaryWriter.Write(Game1.tile[index1, index2].liquid);
                binaryWriter.Write(Game1.tile[index1, index2].lava);
              }
              else
                binaryWriter.Write(false);
            }
          }
          for (int index3 = 0; index3 < 1000; ++index3)
          {
            if (Game1.chest[index3] == null)
            {
              binaryWriter.Write(false);
            }
            else
            {
              binaryWriter.Write(true);
              binaryWriter.Write(Game1.chest[index3].x);
              binaryWriter.Write(Game1.chest[index3].y);
              for (int index4 = 0; index4 < Chest.maxItems; ++index4)
              {
                binaryWriter.Write((byte) Game1.chest[index3].item[index4].stack);
                if (Game1.chest[index3].item[index4].stack > 0)
                  binaryWriter.Write(Game1.chest[index3].item[index4].name);
              }
            }
          }
          for (int index = 0; index < 1000; ++index)
          {
            if (Game1.sign[index] == null || Game1.sign[index].text == null)
            {
              binaryWriter.Write(false);
            }
            else
            {
              binaryWriter.Write(true);
              binaryWriter.Write(Game1.sign[index].text);
              binaryWriter.Write(Game1.sign[index].x);
              binaryWriter.Write(Game1.sign[index].y);
            }
          }
          for (int index = 0; index < 1000; ++index)
          {
            lock (Game1.npc[index])
            {
              if (Game1.npc[index].active && Game1.npc[index].townNPC)
              {
                binaryWriter.Write(true);
                binaryWriter.Write(Game1.npc[index].name);
                binaryWriter.Write(Game1.npc[index].position.X);
                binaryWriter.Write(Game1.npc[index].position.Y);
                binaryWriter.Write(Game1.npc[index].homeless);
                binaryWriter.Write(Game1.npc[index].homeTileX);
                binaryWriter.Write(Game1.npc[index].homeTileY);
              }
            }
          }
          binaryWriter.Write(false);
          binaryWriter.Dispose();//.Close();
        }
      }
      WorldGen.saveLock = false;
    }

    public static void loadWorld()
    {
      if (WorldGen.genRand == null)
        WorldGen.genRand = new Random((int) DateTime.Now.Ticks);
      using (FileStream input = new FileStream(Game1.worldPathName, FileMode.Open))
      {
        using (BinaryReader binaryReader = new BinaryReader((Stream) input))
        {
          try
          {
            int num1 = binaryReader.ReadInt32();
            Game1.worldName = binaryReader.ReadString();
            Game1.leftWorld = (float) binaryReader.ReadInt32();
            Game1.rightWorld = (float) binaryReader.ReadInt32();
            Game1.topWorld = (float) binaryReader.ReadInt32();
            Game1.bottomWorld = (float) binaryReader.ReadInt32();
            Game1.maxTilesY = binaryReader.ReadInt32();
            Game1.maxTilesX = binaryReader.ReadInt32();
            WorldGen.clearWorld();
            Game1.spawnTileX = binaryReader.ReadInt32();
            Game1.spawnTileY = binaryReader.ReadInt32();
            Game1.worldSurface = binaryReader.ReadDouble();
            Game1.rockLayer = binaryReader.ReadDouble();
            WorldGen.tempTime = binaryReader.ReadDouble();
            WorldGen.tempDayTime = binaryReader.ReadBoolean();
            WorldGen.tempMoonPhase = binaryReader.ReadInt32();
            WorldGen.tempBloodMoon = binaryReader.ReadBoolean();
            if (num1 >= 28)
            {
              Game1.dungeonX = binaryReader.ReadInt32();
              Game1.dungeonY = binaryReader.ReadInt32();
            }
            if (num1 >= 24)
            {
              NPC.downedBoss1 = binaryReader.ReadBoolean();
              NPC.downedBoss2 = binaryReader.ReadBoolean();
            }
            NPC.downedBoss3 = num1 < 28 || binaryReader.ReadBoolean();
            if (num1 >= 26)
            {
              WorldGen.shadowOrbSmashed = binaryReader.ReadBoolean();
              WorldGen.spawnMeteor = binaryReader.ReadBoolean();
            }
            if (num1 >= 27)
            {
              Game1.invasionDelay = binaryReader.ReadInt32();
              Game1.invasionSize = binaryReader.ReadInt32();
              Game1.invasionType = binaryReader.ReadInt32();
              Game1.invasionX = binaryReader.ReadDouble();
            }
            for (int index1 = 0; index1 < Game1.maxTilesX; ++index1)
            {
              Game1.statusText = "Loading world data: " + (object) (int) ((double) ((float) index1 / (float) Game1.maxTilesX) * 100.0 + 1.0) + "%";
              for (int index2 = 0; index2 < Game1.maxTilesY; ++index2)
              {
                Game1.tile[index1, index2].active = binaryReader.ReadBoolean();
                if (Game1.tile[index1, index2].active)
                {
                  Game1.tile[index1, index2].type = binaryReader.ReadByte();
                  if (Game1.tileFrameImportant[(int) Game1.tile[index1, index2].type])
                  {
                    Game1.tile[index1, index2].frameX = binaryReader.ReadInt16();
                    Game1.tile[index1, index2].frameY = binaryReader.ReadInt16();
                  }
                  else
                  {
                    Game1.tile[index1, index2].frameX = (short) -1;
                    Game1.tile[index1, index2].frameY = (short) -1;
                  }
                }
                Game1.tile[index1, index2].lighted = binaryReader.ReadBoolean();
                if (binaryReader.ReadBoolean())
                  Game1.tile[index1, index2].wall = binaryReader.ReadByte();
                if (num1 >= 34 && binaryReader.ReadBoolean())
                {
                  Game1.tile[index1, index2].liquid = binaryReader.ReadByte();
                  if (num1 >= 35)
                    Game1.tile[index1, index2].lava = binaryReader.ReadBoolean();
                }
              }
            }
            for (int index3 = 0; index3 < 1000; ++index3)
            {
              if (binaryReader.ReadBoolean())
              {
                Game1.chest[index3] = new Chest();
                Game1.chest[index3].x = binaryReader.ReadInt32();
                Game1.chest[index3].y = binaryReader.ReadInt32();
                for (int index4 = 0; index4 < Chest.maxItems; ++index4)
                {
                  Game1.chest[index3].item[index4] = new Item();
                  byte num2 = binaryReader.ReadByte();
                  if (num2 > (byte) 0)
                  {
                    string ItemName = binaryReader.ReadString();
                    Game1.chest[index3].item[index4].SetDefaults(ItemName);
                    Game1.chest[index3].item[index4].stack = (int) num2;
                  }
                }
              }
            }
            if (num1 >= 33)
            {
              for (int index5 = 0; index5 < 1000; ++index5)
              {
                if (binaryReader.ReadBoolean())
                {
                  string str = binaryReader.ReadString();
                  int index6 = binaryReader.ReadInt32();
                  int index7 = binaryReader.ReadInt32();
                  if (Game1.tile[index6, index7].active && Game1.tile[index6, index7].type == (byte) 55)
                  {
                    Game1.sign[index5] = new Sign();
                    Game1.sign[index5].x = index6;
                    Game1.sign[index5].y = index7;
                    Game1.sign[index5].text = str;
                  }
                }
              }
            }
            if (num1 >= 20)
            {
              bool flag = binaryReader.ReadBoolean();
              int index = 0;
              while (flag)
              {
                Game1.npc[index].SetDefaults(binaryReader.ReadString());
                Game1.npc[index].position.X = binaryReader.ReadSingle();
                Game1.npc[index].position.Y = binaryReader.ReadSingle();
                Game1.npc[index].homeless = binaryReader.ReadBoolean();
                Game1.npc[index].homeTileX = binaryReader.ReadInt32();
                Game1.npc[index].homeTileY = binaryReader.ReadInt32();
                flag = binaryReader.ReadBoolean();
                ++index;
              }
            }
            
            binaryReader.Dispose();//.Close();
            WorldGen.gen = true;
            WorldGen.waterLine = Game1.maxTilesY;
            Liquid.QuickWater(2);
            WorldGen.WaterCheck();
            int num3 = 0;
            Liquid.quickSettle = true;
            int num4 = Liquid.numLiquid + LiquidBuffer.numLiquidBuffer;
            float num5 = 0.0f;
            while (Liquid.numLiquid > 0 && num3 < 100000)
            {
              ++num3;
              float num6 = (float) (num4 - (Liquid.numLiquid + LiquidBuffer.numLiquidBuffer)) / (float) num4;
              if (Liquid.numLiquid + LiquidBuffer.numLiquidBuffer > num4)
                num4 = Liquid.numLiquid + LiquidBuffer.numLiquidBuffer;
              if ((double) num6 > (double) num5)
                num5 = num6;
              else
                num6 = num5;
              Game1.statusText = "Settling liquids: " + (object) (int) ((double) num6 * 100.0 / 2.0 + 50.0) + "%";
              Liquid.UpdateLiquid();
            }
            Liquid.quickSettle = false;
            WorldGen.WaterCheck();
            WorldGen.gen = false;
          }
          catch (Exception ex)
          {
            Game1.menuMode = 15;
            Game1.statusText = ex.ToString();
            WorldGen.loadFailed = true;
            try
            {
              binaryReader.Dispose();//.Close();
              return;
            }
            catch
            {
              return;
            }
          }
          WorldGen.loadFailed = false;
        }
      }
    }

    public static void generateWorld(int seed = -1)
    {
      WorldGen.gen = true;
      WorldGen.genRand = seed <= 0 ? new Random((int) DateTime.Now.Ticks) : new Random(seed);
      int num1 = 0;
      int num2 = 0;
      double num3 = (double) Game1.maxTilesY * 0.3 * ((double) WorldGen.genRand.Next(90, 110) * 0.005);
      double num4 = (num3 + (double) Game1.maxTilesY * 0.2) * ((double) WorldGen.genRand.Next(90, 110) * 0.01);
      double num5 = num3;
      double num6 = num3;
      double minValue = num4;
      double num7 = num4;
      int num8 = 0;
      int num9 = WorldGen.genRand.Next(2) != 0 ? 1 : -1;
      for (int index1 = 0; index1 < Game1.maxTilesX; ++index1)
      {
        Game1.statusText = "Generating world terrain: " + (object) (int) ((double) ((float) index1 / (float) Game1.maxTilesX) * 100.0 + 1.0) + "%";
        if (num3 < num5)
          num5 = num3;
        if (num3 > num6)
          num6 = num3;
        if (num4 < minValue)
          minValue = num4;
        if (num4 > num7)
          num7 = num4;
        if (num2 <= 0)
        {
          num1 = WorldGen.genRand.Next(0, 5);
          num2 = WorldGen.genRand.Next(5, 40);
          if (num1 == 0)
            num2 *= (int) ((double) WorldGen.genRand.Next(5, 30) * 0.2);
        }
        --num2;
        switch (num1)
        {
          case 0:
            while (WorldGen.genRand.Next(0, 7) == 0)
              num3 += (double) WorldGen.genRand.Next(-1, 2);
            break;
          case 1:
            while (WorldGen.genRand.Next(0, 4) == 0)
              --num3;
            while (WorldGen.genRand.Next(0, 10) == 0)
              ++num3;
            break;
          case 2:
            while (WorldGen.genRand.Next(0, 4) == 0)
              ++num3;
            while (WorldGen.genRand.Next(0, 10) == 0)
              --num3;
            break;
          case 3:
            while (WorldGen.genRand.Next(0, 2) == 0)
              --num3;
            while (WorldGen.genRand.Next(0, 6) == 0)
              ++num3;
            break;
          case 4:
            while (WorldGen.genRand.Next(0, 2) == 0)
              ++num3;
            while (WorldGen.genRand.Next(0, 5) == 0)
              --num3;
            break;
        }
        if (num3 < (double) Game1.maxTilesY * 0.15)
        {
          num3 = (double) Game1.maxTilesY * 0.15;
          num2 = 0;
        }
        else if (num3 > (double) Game1.maxTilesY * 0.3)
        {
          num3 = (double) Game1.maxTilesY * 0.3;
          num2 = 0;
        }
        while (WorldGen.genRand.Next(0, 3) == 0)
          num4 += (double) WorldGen.genRand.Next(-2, 3);
        if (num4 < num3 + (double) Game1.maxTilesY * 0.05)
          ++num4;
        if (num4 > num3 + (double) Game1.maxTilesY * 0.35)
          --num4;
        for (int index2 = 0; (double) index2 < num3; ++index2)
        {
          Game1.tile[index1, index2].active = false;
          Game1.tile[index1, index2].lighted = true;
          Game1.tile[index1, index2].frameX = (short) -1;
          Game1.tile[index1, index2].frameY = (short) -1;
        }
        for (int index3 = (int) num3; index3 < Game1.maxTilesY; ++index3)
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
      Game1.worldSurface = num6 + 5.0;
      Game1.rockLayer = num7;
      double num10 = (double) ((int) ((Game1.rockLayer - Game1.worldSurface) / 6.0) * 6);
      Game1.rockLayer = Game1.worldSurface + num10;
      WorldGen.waterLine = (int) (Game1.rockLayer + (double) Game1.maxTilesY) / 2;
      WorldGen.waterLine += WorldGen.genRand.Next(-100, 20);
      WorldGen.lavaLine = WorldGen.waterLine + WorldGen.genRand.Next(50, 80);
      int num11 = 0;
      Game1.statusText = "Adding sand...";
      int num12 = WorldGen.genRand.Next((int) ((double) Game1.maxTilesX * 0.0007), (int) ((double) Game1.maxTilesX * 0.003)) + 2;
      for (int index4 = 0; index4 < num12; ++index4)
      {
        int num13 = WorldGen.genRand.Next(Game1.maxTilesX);
        int num14 = WorldGen.genRand.Next(15, 100);
        if (WorldGen.genRand.Next(3) == 0)
          num14 *= 2;
        int num15 = num13 - num14;
        int num16 = WorldGen.genRand.Next(15, 100);
        if (WorldGen.genRand.Next(3) == 0)
          num16 *= 2;
        int num17 = num13 + num16;
        if (num15 < 0)
          num15 = 0;
        if (num17 > Game1.maxTilesX)
          num17 = Game1.maxTilesX;
        switch (index4)
        {
          case 0:
            num15 = 0;
            num17 = WorldGen.genRand.Next(250, 300);
            break;
          case 2:
            num15 = Game1.maxTilesX - WorldGen.genRand.Next(250, 300);
            num17 = Game1.maxTilesX;
            break;
        }
        int num18 = WorldGen.genRand.Next(50, 100);
        for (int index5 = num15; index5 < num17; ++index5)
        {
          if (WorldGen.genRand.Next(2) == 0)
          {
            num18 += WorldGen.genRand.Next(-1, 2);
            if (num18 < 50)
              num18 = 50;
            if (num18 > 100)
              num18 = 100;
          }
          for (int index6 = 0; (double) index6 < Game1.worldSurface; ++index6)
          {
            if (Game1.tile[index5, index6].active)
            {
              int num19 = num18;
              if (index5 - num15 < num19)
                num19 = index5 - num15;
              if (num17 - index5 < num19)
                num19 = num17 - index5;
              int num20 = num19 + WorldGen.genRand.Next(5);
              for (int index7 = index6; index7 < index6 + num20; ++index7)
              {
                if (index5 > num15 + WorldGen.genRand.Next(5) && index5 < num17 - WorldGen.genRand.Next(5))
                  Game1.tile[index5, index7].type = (byte) 53;
              }
              break;
            }
          }
        }
      }
      for (int index = 0; index < (int) ((double) (Game1.maxTilesX * Game1.maxTilesY) * 1E-05); ++index)
        WorldGen.TileRunner(WorldGen.genRand.Next(0, Game1.maxTilesX), WorldGen.genRand.Next((int) Game1.worldSurface, (int) Game1.rockLayer), (double) WorldGen.genRand.Next(15, 70), WorldGen.genRand.Next(20, 130), 53);
      WorldGen.numMCaves = 0;
      Game1.statusText = "Generating hills...";
      for (int index8 = 0; index8 < (int) ((double) Game1.maxTilesX * 0.0008); ++index8)
      {
        int num21 = 0;
        bool flag1 = false;
        bool flag2 = false;
        int i = WorldGen.genRand.Next((int) ((double) Game1.maxTilesX * 0.25), (int) ((double) Game1.maxTilesX * 0.75));
        while (!flag2)
        {
          flag2 = true;
          while (i > Game1.maxTilesX / 2 - 100 && i < Game1.maxTilesX / 2 + 100)
            i = WorldGen.genRand.Next((int) ((double) Game1.maxTilesX * 0.25), (int) ((double) Game1.maxTilesX * 0.75));
          for (int index9 = 0; index9 < WorldGen.numMCaves; ++index9)
          {
            if (i > WorldGen.mCaveX[index9] - 50 && i < WorldGen.mCaveX[index9] + 50)
            {
              ++num21;
              flag2 = false;
              break;
            }
          }
          if (num21 >= 200)
          {
            flag1 = true;
            break;
          }
        }
        if (!flag1)
        {
          for (int j = 0; (double) j < Game1.worldSurface; ++j)
          {
            if (Game1.tile[i, j].active)
            {
              WorldGen.Mountinater(i, j);
              WorldGen.mCaveX[WorldGen.numMCaves] = i;
              WorldGen.mCaveY[WorldGen.numMCaves] = j;
              ++WorldGen.numMCaves;
              break;
            }
          }
        }
      }
      for (int index10 = 1; index10 < Game1.maxTilesX - 1; ++index10)
      {
        Game1.statusText = "Puttin dirt behind dirt: " + (object) (int) ((double) ((float) index10 / (float) Game1.maxTilesX) * 100.0 + 1.0) + "%";
        bool flag = false;
        num11 += WorldGen.genRand.Next(-1, 2);
        if (num11 < 0)
          num11 = 0;
        if (num11 > 10)
          num11 = 10;
        for (int index11 = 0; (double) index11 < Game1.worldSurface + 10.0 && (double) index11 <= Game1.worldSurface + (double) num11; ++index11)
        {
          if (flag)
            Game1.tile[index10, index11].wall = (byte) 2;
          if (Game1.tile[index10, index11].active && Game1.tile[index10 - 1, index11].active && Game1.tile[index10 + 1, index11].active && Game1.tile[index10, index11 + 1].active && Game1.tile[index10 - 1, index11 + 1].active && Game1.tile[index10 + 1, index11 + 1].active)
            flag = true;
        }
      }
      WorldGen.numIslandHouses = 0;
      WorldGen.houseCount = 0;
      Game1.statusText = "Generating floating islands...";
      for (int index12 = 0; index12 < (int) ((double) Game1.maxTilesX * 0.0008); ++index12)
      {
        int num22 = 0;
        bool flag3 = false;
        int index13 = WorldGen.genRand.Next((int) ((double) Game1.maxTilesX * 0.1), (int) ((double) Game1.maxTilesX * 0.9));
        bool flag4 = false;
        while (!flag4)
        {
          flag4 = true;
          while (index13 > Game1.maxTilesX / 2 - 80 && index13 < Game1.maxTilesX / 2 + 80)
            index13 = WorldGen.genRand.Next((int) ((double) Game1.maxTilesX * 0.1), (int) ((double) Game1.maxTilesX * 0.9));
          for (int index14 = 0; index14 < WorldGen.numIslandHouses; ++index14)
          {
            if (index13 > WorldGen.fihX[index14] - 80 && index13 < WorldGen.fihX[index14] + 80)
            {
              ++num22;
              flag4 = false;
              break;
            }
          }
          if (num22 >= 200)
          {
            flag3 = true;
            break;
          }
        }
        if (!flag3)
        {
          for (int index15 = 200; (double) index15 < Game1.worldSurface; ++index15)
          {
            if (Game1.tile[index13, index15].active)
            {
              int i = index13;
              int j = WorldGen.genRand.Next(100, index15 - 100);
              while ((double) j > num5 - 50.0)
                --j;
              WorldGen.FloatingIsland(i, j);
              WorldGen.fihX[WorldGen.numIslandHouses] = i;
              WorldGen.fihY[WorldGen.numIslandHouses] = j;
              ++WorldGen.numIslandHouses;
              break;
            }
          }
        }
      }
      Game1.statusText = "Placing rocks in the dirt...";
      for (int index = 0; index < (int) ((double) (Game1.maxTilesX * Game1.maxTilesY) * 0.0002); ++index)
        WorldGen.TileRunner(WorldGen.genRand.Next(0, Game1.maxTilesX), WorldGen.genRand.Next(0, (int) num5 + 1), (double) WorldGen.genRand.Next(4, 15), WorldGen.genRand.Next(5, 40), 1);
      for (int index = 0; index < (int) ((double) (Game1.maxTilesX * Game1.maxTilesY) * 0.0002); ++index)
        WorldGen.TileRunner(WorldGen.genRand.Next(0, Game1.maxTilesX), WorldGen.genRand.Next((int) num5, (int) num6 + 1), (double) WorldGen.genRand.Next(4, 10), WorldGen.genRand.Next(5, 30), 1);
      for (int index = 0; index < (int) ((double) (Game1.maxTilesX * Game1.maxTilesY) * 0.0045); ++index)
        WorldGen.TileRunner(WorldGen.genRand.Next(0, Game1.maxTilesX), WorldGen.genRand.Next((int) num6, (int) num7 + 1), (double) WorldGen.genRand.Next(2, 7), WorldGen.genRand.Next(2, 23), 1);
      Game1.statusText = "Placing dirt in the rocks...";
      for (int index = 0; index < (int) ((double) (Game1.maxTilesX * Game1.maxTilesY) * 0.005); ++index)
        WorldGen.TileRunner(WorldGen.genRand.Next(0, Game1.maxTilesX), WorldGen.genRand.Next((int) minValue, Game1.maxTilesY), (double) WorldGen.genRand.Next(2, 6), WorldGen.genRand.Next(2, 40), 0);
      Game1.statusText = "Adding clay...";
      for (int index = 0; index < (int) ((double) (Game1.maxTilesX * Game1.maxTilesY) * 2E-05); ++index)
        WorldGen.TileRunner(WorldGen.genRand.Next(0, Game1.maxTilesX), WorldGen.genRand.Next(0, (int) num5), (double) WorldGen.genRand.Next(4, 14), WorldGen.genRand.Next(10, 50), 40);
      for (int index = 0; index < (int) ((double) (Game1.maxTilesX * Game1.maxTilesY) * 5E-05); ++index)
        WorldGen.TileRunner(WorldGen.genRand.Next(0, Game1.maxTilesX), WorldGen.genRand.Next((int) num5, (int) num6 + 1), (double) WorldGen.genRand.Next(8, 14), WorldGen.genRand.Next(15, 45), 40);
      for (int index = 0; index < (int) ((double) (Game1.maxTilesX * Game1.maxTilesY) * 2E-05); ++index)
        WorldGen.TileRunner(WorldGen.genRand.Next(0, Game1.maxTilesX), WorldGen.genRand.Next((int) num6, (int) num7 + 1), (double) WorldGen.genRand.Next(8, 15), WorldGen.genRand.Next(5, 50), 40);
      for (int index16 = 5; index16 < Game1.maxTilesX - 5; ++index16)
      {
        for (int index17 = 1; (double) index17 < Game1.worldSurface - 1.0; ++index17)
        {
          if (Game1.tile[index16, index17].active)
          {
            for (int index18 = index17; index18 < index17 + 5; ++index18)
            {
              if (Game1.tile[index16, index18].type == (byte) 40)
                Game1.tile[index16, index18].type = (byte) 0;
            }
            break;
          }
        }
      }
      for (int index = 0; index < (int) ((double) (Game1.maxTilesX * Game1.maxTilesY) * 0.0015); ++index)
      {
        Game1.statusText = "Making random holes: " + (object) (int) ((double) ((float) index / ((float) (Game1.maxTilesX * Game1.maxTilesY) * 0.0015f)) * 100.0 + 1.0) + "%";
        int type = -1;
        if (WorldGen.genRand.Next(5) == 0)
          type = -2;
        WorldGen.TileRunner(WorldGen.genRand.Next(0, Game1.maxTilesX), WorldGen.genRand.Next((int) num6, Game1.maxTilesY), (double) WorldGen.genRand.Next(2, 5), WorldGen.genRand.Next(2, 20), type);
        WorldGen.TileRunner(WorldGen.genRand.Next(0, Game1.maxTilesX), WorldGen.genRand.Next((int) num6, Game1.maxTilesY), (double) WorldGen.genRand.Next(8, 15), WorldGen.genRand.Next(7, 30), type);
      }
      for (int index = 0; index < (int) ((double) (Game1.maxTilesX * Game1.maxTilesY) * 3E-05); ++index)
      {
        Game1.statusText = "Generating small caves: " + (object) (int) ((double) ((float) index / ((float) (Game1.maxTilesX * Game1.maxTilesY) * 3E-05f)) * 100.0 + 1.0) + "%";
        if (num7 <= (double) Game1.maxTilesY)
        {
          int type = -1;
          if (WorldGen.genRand.Next(5) == 0)
            type = -2;
          WorldGen.TileRunner(WorldGen.genRand.Next(0, Game1.maxTilesX), WorldGen.genRand.Next((int) num5, (int) num7 + 1), (double) WorldGen.genRand.Next(5, 15), WorldGen.genRand.Next(30, 200), type);
        }
      }
      for (int index = 0; index < (int) ((double) (Game1.maxTilesX * Game1.maxTilesY) * 0.00015); ++index)
      {
        Game1.statusText = "Generating large caves: " + (object) (int) ((double) ((float) index / ((float) (Game1.maxTilesX * Game1.maxTilesY) * 0.00015f)) * 100.0 + 1.0) + "%";
        if (num7 <= (double) Game1.maxTilesY)
        {
          int type = -1;
          if (WorldGen.genRand.Next(8) == 0)
            type = -2;
          WorldGen.TileRunner(WorldGen.genRand.Next(0, Game1.maxTilesX), WorldGen.genRand.Next((int) num7, Game1.maxTilesY), (double) WorldGen.genRand.Next(6, 20), WorldGen.genRand.Next(50, 300), type);
        }
      }
      Game1.statusText = "Generating surface caves...";
      for (int index = 0; index < (int) ((double) Game1.maxTilesX * (1.0 / 400.0)); ++index)
      {
        int i = WorldGen.genRand.Next(0, Game1.maxTilesX);
        for (int j = 0; (double) j < num6; ++j)
        {
          if (Game1.tile[i, j].active)
          {
            WorldGen.TileRunner(i, j, (double) WorldGen.genRand.Next(3, 6), WorldGen.genRand.Next(5, 50), -1, speedX: (float) WorldGen.genRand.Next(-10, 11) * 0.1f, speedY: 1f);
            break;
          }
        }
      }
      for (int index = 0; index < (int) ((double) Game1.maxTilesX * 0.0007); ++index)
      {
        int i = WorldGen.genRand.Next(0, Game1.maxTilesX);
        for (int j = 0; (double) j < num6; ++j)
        {
          if (Game1.tile[i, j].active)
          {
            WorldGen.TileRunner(i, j, (double) WorldGen.genRand.Next(10, 15), WorldGen.genRand.Next(50, 130), -1, speedX: (float) WorldGen.genRand.Next(-10, 11) * 0.1f, speedY: 2f);
            break;
          }
        }
      }
      for (int index = 0; index < (int) ((double) Game1.maxTilesX * 0.0003); ++index)
      {
        int i = WorldGen.genRand.Next(0, Game1.maxTilesX);
        for (int j = 0; (double) j < num6; ++j)
        {
          if (Game1.tile[i, j].active)
          {
            WorldGen.TileRunner(i, j, (double) WorldGen.genRand.Next(12, 25), WorldGen.genRand.Next(150, 500), -1, speedX: (float) WorldGen.genRand.Next(-10, 11) * 0.1f, speedY: 4f);
            WorldGen.TileRunner(i, j, (double) WorldGen.genRand.Next(8, 17), WorldGen.genRand.Next(60, 200), -1, speedX: (float) WorldGen.genRand.Next(-10, 11) * 0.1f, speedY: 2f);
            WorldGen.TileRunner(i, j, (double) WorldGen.genRand.Next(5, 13), WorldGen.genRand.Next(40, 170), -1, speedX: (float) WorldGen.genRand.Next(-10, 11) * 0.1f, speedY: 2f);
            break;
          }
        }
      }
      for (int index = 0; index < (int) ((double) Game1.maxTilesX * 0.0004); ++index)
      {
        int i = WorldGen.genRand.Next(0, Game1.maxTilesX);
        for (int j = 0; (double) j < num6; ++j)
        {
          if (Game1.tile[i, j].active)
          {
            WorldGen.TileRunner(i, j, (double) WorldGen.genRand.Next(7, 12), WorldGen.genRand.Next(150, 250), -1, speedY: 1f, noYChange: true);
            break;
          }
        }
      }
      for (int index19 = 0; index19 < (int) ((double) (Game1.maxTilesX * Game1.maxTilesY) * 0.002); ++index19)
      {
        int index20 = WorldGen.genRand.Next(1, Game1.maxTilesX - 1);
        int index21 = WorldGen.genRand.Next((int) num5, (int) num6);
        if (index21 >= Game1.maxTilesY)
          index21 = Game1.maxTilesY - 2;
        if (Game1.tile[index20 - 1, index21].active && Game1.tile[index20 - 1, index21].type == (byte) 0 && Game1.tile[index20 + 1, index21].active && Game1.tile[index20 + 1, index21].type == (byte) 0 && Game1.tile[index20, index21 - 1].active && Game1.tile[index20, index21 - 1].type == (byte) 0 && Game1.tile[index20, index21 + 1].active && Game1.tile[index20, index21 + 1].type == (byte) 0)
        {
          Game1.tile[index20, index21].active = true;
          Game1.tile[index20, index21].type = (byte) 2;
        }
        int index22 = WorldGen.genRand.Next(1, Game1.maxTilesX - 1);
        int index23 = WorldGen.genRand.Next(0, (int) num5);
        if (index23 >= Game1.maxTilesY)
          index23 = Game1.maxTilesY - 2;
        if (Game1.tile[index22 - 1, index23].active && Game1.tile[index22 - 1, index23].type == (byte) 0 && Game1.tile[index22 + 1, index23].active && Game1.tile[index22 + 1, index23].type == (byte) 0 && Game1.tile[index22, index23 - 1].active && Game1.tile[index22, index23 - 1].type == (byte) 0 && Game1.tile[index22, index23 + 1].active && Game1.tile[index22, index23 + 1].type == (byte) 0)
        {
          Game1.tile[index22, index23].active = true;
          Game1.tile[index22, index23].type = (byte) 2;
        }
      }
      Game1.statusText = "Generating underground jungle: 0%";
      float num23 = (float) (Game1.maxTilesX / 4200) * 1.5f;
      int num24 = 0;
      int num25 = num9 != -1 ? (int) ((double) Game1.maxTilesX * 0.20000000298023224) : (int) ((double) Game1.maxTilesX * 0.800000011920929);
      int num26 = (int) ((double) Game1.maxTilesY + Game1.rockLayer) / 2;
      int i1 = num25 + WorldGen.genRand.Next((int) (-100.0 * (double) num23), (int) (101.0 * (double) num23));
      int j1 = num26 + WorldGen.genRand.Next((int) (-100.0 * (double) num23), (int) (101.0 * (double) num23));
      WorldGen.TileRunner(i1, j1, (double) WorldGen.genRand.Next((int) (250.0 * (double) num23), (int) (500.0 * (double) num23)), WorldGen.genRand.Next(50, 150), 59, speedX: (float) (num9 * 3));
      Game1.statusText = "Generating underground jungle: 20%";
      int i2 = i1 + WorldGen.genRand.Next((int) (-250.0 * (double) num23), (int) (251.0 * (double) num23));
      int j2 = j1 + WorldGen.genRand.Next((int) (-150.0 * (double) num23), (int) (151.0 * (double) num23));
      int num27 = i2;
      int num28 = j2;
      WorldGen.TileRunner(i2, j2, (double) WorldGen.genRand.Next((int) (250.0 * (double) num23), (int) (500.0 * (double) num23)), WorldGen.genRand.Next(50, 150), 59);
      Game1.statusText = "Generating underground jungle: 40%";
      WorldGen.TileRunner(i2 + WorldGen.genRand.Next((int) (-400.0 * (double) num23), (int) (401.0 * (double) num23)), j2 + WorldGen.genRand.Next((int) (-150.0 * (double) num23), (int) (151.0 * (double) num23)), (double) WorldGen.genRand.Next((int) (250.0 * (double) num23), (int) (500.0 * (double) num23)), WorldGen.genRand.Next(50, 150), 59, speedX: (float) (num9 * -3));
      Game1.statusText = "Generating underground jungle: 60%";
      int i3 = num27;
      int j3 = num28;
      for (int index = 0; (double) index <= 20.0 * (double) num23; ++index)
      {
        Game1.statusText = "Generating underground jungle: " + (object) (int) (60.0 + (double) index / (double) num23) + "%";
        i3 += WorldGen.genRand.Next((int) (-5.0 * (double) num23), (int) (6.0 * (double) num23));
        j3 += WorldGen.genRand.Next((int) (-5.0 * (double) num23), (int) (6.0 * (double) num23));
        WorldGen.TileRunner(i3, j3, (double) WorldGen.genRand.Next(40, 100), WorldGen.genRand.Next(300, 500), 59);
      }
      for (int index24 = 0; (double) index24 <= 10.0 * (double) num23; ++index24)
      {
        Game1.statusText = "Generating underground jungle: " + (object) (int) (80.0 + (double) index24 / (double) num23 * 2.0) + "%";
        int i4 = num27 + WorldGen.genRand.Next((int) (-600.0 * (double) num23), (int) (600.0 * (double) num23));
        int j4;
        for (j4 = num28 + WorldGen.genRand.Next((int) (-200.0 * (double) num23), (int) (200.0 * (double) num23)); i4 < 1 || i4 >= Game1.maxTilesX - 1 || j4 < 1 || j4 >= Game1.maxTilesY - 1 || Game1.tile[i4, j4].type != (byte) 59; j4 = num28 + WorldGen.genRand.Next((int) (-200.0 * (double) num23), (int) (200.0 * (double) num23)))
          i4 = num27 + WorldGen.genRand.Next((int) (-600.0 * (double) num23), (int) (600.0 * (double) num23));
        for (int index25 = 0; (double) index25 < 8.0 * (double) num23; ++index25)
        {
          i4 += WorldGen.genRand.Next(-30, 31);
          j4 += WorldGen.genRand.Next(-30, 31);
          int type = -1;
          if (WorldGen.genRand.Next(7) == 0)
            type = -2;
          WorldGen.TileRunner(i4, j4, (double) WorldGen.genRand.Next(10, 20), WorldGen.genRand.Next(30, 70), type);
        }
      }
      for (int index = 0; (double) index <= 300.0 * (double) num23; ++index)
      {
        int i5 = num27 + WorldGen.genRand.Next((int) (-600.0 * (double) num23), (int) (600.0 * (double) num23));
        int j5;
        for (j5 = num28 + WorldGen.genRand.Next((int) (-200.0 * (double) num23), (int) (200.0 * (double) num23)); i5 < 1 || i5 >= Game1.maxTilesX - 1 || j5 < 1 || j5 >= Game1.maxTilesY - 1 || Game1.tile[i5, j5].type != (byte) 59; j5 = num28 + WorldGen.genRand.Next((int) (-200.0 * (double) num23), (int) (200.0 * (double) num23)))
          i5 = num27 + WorldGen.genRand.Next((int) (-600.0 * (double) num23), (int) (600.0 * (double) num23));
        WorldGen.TileRunner(i5, j5, (double) WorldGen.genRand.Next(4, 10), WorldGen.genRand.Next(5, 30), 1);
        if (WorldGen.genRand.Next(4) == 0)
        {
          int type = WorldGen.genRand.Next(63, 69);
          WorldGen.TileRunner(i5 + WorldGen.genRand.Next(-1, 2), j5 + WorldGen.genRand.Next(-1, 2), (double) WorldGen.genRand.Next(3, 7), WorldGen.genRand.Next(4, 8), type);
        }
      }
      for (int i6 = 0; i6 < Game1.maxTilesX; ++i6)
      {
        for (int worldSurface = (int) Game1.worldSurface; worldSurface < Game1.maxTilesY; ++worldSurface)
        {
          if (Game1.tile[i6, worldSurface].active)
            WorldGen.SpreadGrass(i6, worldSurface, 59, 60, false);
        }
      }
      Game1.statusText = "Adding mushroom patches...";
      for (int index = 0; index < Game1.maxTilesX / 300; ++index)
        WorldGen.ShroomPatch(WorldGen.genRand.Next((int) ((double) Game1.maxTilesX * 0.3), (int) ((double) Game1.maxTilesX * 0.7)), WorldGen.genRand.Next((int) Game1.rockLayer, Game1.maxTilesY - 300));
      for (int i7 = 0; i7 < Game1.maxTilesX; ++i7)
      {
        for (int worldSurface = (int) Game1.worldSurface; worldSurface < Game1.maxTilesY; ++worldSurface)
        {
          if (Game1.tile[i7, worldSurface].active)
            WorldGen.SpreadGrass(i7, worldSurface, 59, 70, false);
        }
      }
      Game1.statusText = "Placing mud in the dirt...";
      for (int index = 0; index < (int) ((double) (Game1.maxTilesX * Game1.maxTilesY) * 0.001); ++index)
        WorldGen.TileRunner(WorldGen.genRand.Next(0, Game1.maxTilesX), WorldGen.genRand.Next((int) minValue, Game1.maxTilesY), (double) WorldGen.genRand.Next(2, 6), WorldGen.genRand.Next(2, 40), 59);
      Game1.statusText = "Adding shinies...";
      for (int index = 0; index < (int) ((double) (Game1.maxTilesX * Game1.maxTilesY) * 6E-05); ++index)
        WorldGen.TileRunner(WorldGen.genRand.Next(0, Game1.maxTilesX), WorldGen.genRand.Next((int) num5, (int) num6), (double) WorldGen.genRand.Next(3, 6), WorldGen.genRand.Next(2, 6), 7);
      for (int index = 0; index < (int) ((double) (Game1.maxTilesX * Game1.maxTilesY) * 8E-05); ++index)
        WorldGen.TileRunner(WorldGen.genRand.Next(0, Game1.maxTilesX), WorldGen.genRand.Next((int) num6, (int) num7), (double) WorldGen.genRand.Next(3, 7), WorldGen.genRand.Next(3, 7), 7);
      for (int index = 0; index < (int) ((double) (Game1.maxTilesX * Game1.maxTilesY) * 0.0002); ++index)
        WorldGen.TileRunner(WorldGen.genRand.Next(0, Game1.maxTilesX), WorldGen.genRand.Next((int) minValue, Game1.maxTilesY), (double) WorldGen.genRand.Next(4, 9), WorldGen.genRand.Next(4, 8), 7);
      for (int index = 0; index < (int) ((double) (Game1.maxTilesX * Game1.maxTilesY) * 3E-05); ++index)
        WorldGen.TileRunner(WorldGen.genRand.Next(0, Game1.maxTilesX), WorldGen.genRand.Next((int) num5, (int) num6), (double) WorldGen.genRand.Next(3, 7), WorldGen.genRand.Next(2, 5), 6);
      for (int index = 0; index < (int) ((double) (Game1.maxTilesX * Game1.maxTilesY) * 6E-05); ++index)
        WorldGen.TileRunner(WorldGen.genRand.Next(0, Game1.maxTilesX), WorldGen.genRand.Next((int) num6, (int) num7), (double) WorldGen.genRand.Next(3, 6), WorldGen.genRand.Next(3, 6), 6);
      for (int index = 0; index < (int) ((double) (Game1.maxTilesX * Game1.maxTilesY) * 0.0002); ++index)
        WorldGen.TileRunner(WorldGen.genRand.Next(0, Game1.maxTilesX), WorldGen.genRand.Next((int) minValue, Game1.maxTilesY), (double) WorldGen.genRand.Next(4, 9), WorldGen.genRand.Next(4, 8), 6);
      for (int index = 0; index < (int) ((double) (Game1.maxTilesX * Game1.maxTilesY) * 1E-05); ++index)
        WorldGen.TileRunner(WorldGen.genRand.Next(0, Game1.maxTilesX), WorldGen.genRand.Next((int) num5, (int) num6), (double) WorldGen.genRand.Next(3, 7), WorldGen.genRand.Next(2, 5), 9);
      for (int index = 0; index < (int) ((double) (Game1.maxTilesX * Game1.maxTilesY) * 3E-05); ++index)
        WorldGen.TileRunner(WorldGen.genRand.Next(0, Game1.maxTilesX), WorldGen.genRand.Next((int) num6, (int) num7), (double) WorldGen.genRand.Next(3, 6), WorldGen.genRand.Next(3, 6), 9);
      for (int index = 0; index < (int) ((double) (Game1.maxTilesX * Game1.maxTilesY) * 0.00017); ++index)
        WorldGen.TileRunner(WorldGen.genRand.Next(0, Game1.maxTilesX), WorldGen.genRand.Next((int) minValue, Game1.maxTilesY), (double) WorldGen.genRand.Next(4, 9), WorldGen.genRand.Next(4, 8), 9);
      for (int index = 0; index < (int) ((double) (Game1.maxTilesX * Game1.maxTilesY) * 0.00017); ++index)
        WorldGen.TileRunner(WorldGen.genRand.Next(0, Game1.maxTilesX), WorldGen.genRand.Next(0, (int) num5), (double) WorldGen.genRand.Next(4, 9), WorldGen.genRand.Next(4, 8), 9);
      for (int index = 0; index < (int) ((double) (Game1.maxTilesX * Game1.maxTilesY) * 1E-05); ++index)
        WorldGen.TileRunner(WorldGen.genRand.Next(0, Game1.maxTilesX), WorldGen.genRand.Next((int) num6, (int) num7), (double) WorldGen.genRand.Next(3, 6), WorldGen.genRand.Next(2, 6), 8);
      for (int index = 0; index < (int) ((double) (Game1.maxTilesX * Game1.maxTilesY) * 0.00012); ++index)
        WorldGen.TileRunner(WorldGen.genRand.Next(0, Game1.maxTilesX), WorldGen.genRand.Next((int) minValue, Game1.maxTilesY), (double) WorldGen.genRand.Next(4, 8), WorldGen.genRand.Next(4, 8), 8);
      for (int index = 0; index < (int) ((double) (Game1.maxTilesX * Game1.maxTilesY) * 0.00012); ++index)
        WorldGen.TileRunner(WorldGen.genRand.Next(0, Game1.maxTilesX), WorldGen.genRand.Next(0, (int) num5), (double) WorldGen.genRand.Next(4, 8), WorldGen.genRand.Next(4, 8), 8);
      Game1.statusText = "Adding webs...";
      for (int index26 = 0; index26 < (int) ((double) (Game1.maxTilesX * Game1.maxTilesY) * 0.001); ++index26)
      {
        int index27 = WorldGen.genRand.Next(20, Game1.maxTilesX - 20);
        int index28 = WorldGen.genRand.Next((int) num5, Game1.maxTilesY - 20);
        if (index26 < WorldGen.numMCaves)
        {
          index27 = WorldGen.mCaveX[index26];
          index28 = WorldGen.mCaveY[index26];
        }
        if (!Game1.tile[index27, index28].active && ((double) index28 > Game1.worldSurface || Game1.tile[index27, index28].wall > (byte) 0))
        {
          while (!Game1.tile[index27, index28].active && index28 > (int) num5)
            --index28;
          int j6 = index28 + 1;
          int speedX = 1;
          if (WorldGen.genRand.Next(2) == 0)
            speedX = -1;
          while (!Game1.tile[index27, j6].active && index27 > 10 && index27 < Game1.maxTilesX - 10)
            index27 += speedX;
          int i8 = index27 - speedX;
          if ((double) j6 > Game1.worldSurface || Game1.tile[i8, j6].wall > (byte) 0)
            WorldGen.TileRunner(i8, j6, (double) WorldGen.genRand.Next(4, 13), WorldGen.genRand.Next(2, 5), 51, true, (float) speedX, -1f, overRide: false);
        }
      }
      Game1.statusText = "Creating underworld: 0%";
      int num29 = Game1.maxTilesY - WorldGen.genRand.Next(150, 190);
      for (int index29 = 0; index29 < Game1.maxTilesX; ++index29)
      {
        num29 += WorldGen.genRand.Next(-3, 4);
        if (num29 < Game1.maxTilesY - 190)
          num29 = Game1.maxTilesY - 190;
        if (num29 > Game1.maxTilesY - 160)
          num29 = Game1.maxTilesY - 160;
        for (int index30 = num29 - 20 - WorldGen.genRand.Next(3); index30 < Game1.maxTilesY; ++index30)
        {
          if (index30 >= num29)
          {
            Game1.tile[index29, index30].active = false;
            Game1.tile[index29, index30].lava = false;
            Game1.tile[index29, index30].liquid = (byte) 0;
          }
          else
            Game1.tile[index29, index30].type = (byte) 57;
        }
      }
      int num30 = Game1.maxTilesY - WorldGen.genRand.Next(40, 70);
      for (int index31 = 10; index31 < Game1.maxTilesX - 10; ++index31)
      {
        num30 += WorldGen.genRand.Next(-10, 11);
        if (num30 > Game1.maxTilesY - 60)
          num30 = Game1.maxTilesY - 60;
        if (num30 < Game1.maxTilesY - 100)
          num30 = Game1.maxTilesY - 120;
        for (int index32 = num30; index32 < Game1.maxTilesY - 10; ++index32)
        {
          if (!Game1.tile[index31, index32].active)
          {
            Game1.tile[index31, index32].lava = true;
            Game1.tile[index31, index32].liquid = byte.MaxValue;
          }
        }
      }
      for (int index33 = 0; index33 < Game1.maxTilesX; ++index33)
      {
        if (WorldGen.genRand.Next(50) == 0)
        {
          int index34 = Game1.maxTilesY - 65;
          while (!Game1.tile[index33, index34].active && index34 > Game1.maxTilesY - 135)
            --index34;
          WorldGen.TileRunner(WorldGen.genRand.Next(0, Game1.maxTilesX), index34 + WorldGen.genRand.Next(20, 50), (double) WorldGen.genRand.Next(15, 20), 1000, 57, true, speedY: (float) WorldGen.genRand.Next(1, 3), noYChange: true);
        }
      }
      Liquid.QuickWater(-2);
      for (int i9 = 0; i9 < Game1.maxTilesX; ++i9)
      {
        Game1.statusText = "Creating underworld: " + (object) (int) ((double) ((float) i9 / (float) (Game1.maxTilesX - 1)) * 100.0 / 2.0 + 50.0) + "%";
        if (WorldGen.genRand.Next(13) == 0)
        {
          int index = Game1.maxTilesY - 65;
          while ((Game1.tile[i9, index].liquid > (byte) 0 || Game1.tile[i9, index].active) && index > Game1.maxTilesY - 140)
            --index;
          WorldGen.TileRunner(i9, index - WorldGen.genRand.Next(2, 5), (double) WorldGen.genRand.Next(5, 30), 1000, 57, true, speedY: (float) WorldGen.genRand.Next(1, 3), noYChange: true);
          float num31 = (float) WorldGen.genRand.Next(1, 3);
          if (WorldGen.genRand.Next(3) == 0)
            num31 *= 0.5f;
          if (WorldGen.genRand.Next(2) == 0)
            WorldGen.TileRunner(i9, index - WorldGen.genRand.Next(2, 5), (double) (int) ((double) WorldGen.genRand.Next(5, 15) * (double) num31), (int) ((double) WorldGen.genRand.Next(10, 15) * (double) num31), 57, true, 1f, 0.3f);
          if (WorldGen.genRand.Next(2) == 0)
          {
            float num32 = (float) WorldGen.genRand.Next(1, 3);
            WorldGen.TileRunner(i9, index - WorldGen.genRand.Next(2, 5), (double) (int) ((double) WorldGen.genRand.Next(5, 15) * (double) num32), (int) ((double) WorldGen.genRand.Next(10, 15) * (double) num32), 57, true, -1f, 0.3f);
          }
          WorldGen.TileRunner(i9 + WorldGen.genRand.Next(-10, 10), index + WorldGen.genRand.Next(-10, 10), (double) WorldGen.genRand.Next(5, 15), WorldGen.genRand.Next(5, 10), -2, speedX: (float) WorldGen.genRand.Next(-1, 3), speedY: (float) WorldGen.genRand.Next(-1, 3));
          if (WorldGen.genRand.Next(3) == 0)
            WorldGen.TileRunner(i9 + WorldGen.genRand.Next(-10, 10), index + WorldGen.genRand.Next(-10, 10), (double) WorldGen.genRand.Next(10, 30), WorldGen.genRand.Next(10, 20), -2, speedX: (float) WorldGen.genRand.Next(-1, 3), speedY: (float) WorldGen.genRand.Next(-1, 3));
          if (WorldGen.genRand.Next(5) == 0)
            WorldGen.TileRunner(i9 + WorldGen.genRand.Next(-15, 15), index + WorldGen.genRand.Next(-15, 10), (double) WorldGen.genRand.Next(15, 30), WorldGen.genRand.Next(5, 20), -2, speedX: (float) WorldGen.genRand.Next(-1, 3), speedY: (float) WorldGen.genRand.Next(-1, 3));
        }
      }
      for (int index = 0; index < Game1.maxTilesX; ++index)
      {
        if (!Game1.tile[index, Game1.maxTilesY - 145].active)
        {
          Game1.tile[index, Game1.maxTilesY - 145].liquid = byte.MaxValue;
          Game1.tile[index, Game1.maxTilesY - 145].lava = true;
        }
        if (!Game1.tile[index, Game1.maxTilesY - 144].active)
        {
          Game1.tile[index, Game1.maxTilesY - 144].liquid = byte.MaxValue;
          Game1.tile[index, Game1.maxTilesY - 144].lava = true;
        }
      }
      for (int index = 0; index < (int) ((double) (Game1.maxTilesX * Game1.maxTilesY) * 0.002); ++index)
        WorldGen.TileRunner(WorldGen.genRand.Next(0, Game1.maxTilesX), WorldGen.genRand.Next(Game1.maxTilesY - 140, Game1.maxTilesY), (double) WorldGen.genRand.Next(3, 8), WorldGen.genRand.Next(3, 8), 58);
      int num33 = WorldGen.genRand.Next(2, (int) ((double) Game1.maxTilesX * 0.005));
      for (int index = 0; index < num33; ++index)
      {
        Game1.statusText = "Adding water bodies: " + (object) (int) ((double) ((float) index / (float) num33) * 100.0) + "%";
        int i10 = WorldGen.genRand.Next(300, Game1.maxTilesX - 300);
        while (i10 > Game1.maxTilesX / 2 - 50 && i10 < Game1.maxTilesX / 2 + 50)
          i10 = WorldGen.genRand.Next(300, Game1.maxTilesX - 300);
        int j7 = (int) num5 - 20;
        while (!Game1.tile[i10, j7].active)
          ++j7;
        WorldGen.Lakinater(i10, j7);
      }
      num24 = 0;
      int x1;
      if (num9 == -1)
      {
        x1 = WorldGen.genRand.Next((int) ((double) Game1.maxTilesX * 0.05), (int) ((double) Game1.maxTilesX * 0.2));
        num8 = -1;
      }
      else
      {
        x1 = WorldGen.genRand.Next((int) ((double) Game1.maxTilesX * 0.8), (int) ((double) Game1.maxTilesX * 0.95));
        num8 = 1;
      }
      int y1 = (int) ((Game1.rockLayer + (double) Game1.maxTilesY) / 2.0) + WorldGen.genRand.Next(-200, 200);
      WorldGen.MakeDungeon(x1, y1);
      for (int index35 = 0; (double) index35 < (double) Game1.maxTilesX * 0.0004; ++index35)
      {
        Game1.statusText = "Making the world evil: " + (object) (int) ((double) ((float) index35 / ((float) Game1.maxTilesX * 0.0004f)) * 100.0) + "%";
        bool flag5 = false;
        int num34 = 0;
        int num35 = 0;
        int num36 = 0;
        while (!flag5)
        {
          flag5 = true;
          int num37 = Game1.maxTilesX / 2;
          int num38 = 200;
          num34 = WorldGen.genRand.Next(Game1.maxTilesX);
          num35 = num34 - WorldGen.genRand.Next(150) - 175;
          num36 = num34 + WorldGen.genRand.Next(150) + 175;
          if (num35 < 0)
            num35 = 0;
          if (num36 > Game1.maxTilesX)
            num36 = Game1.maxTilesX;
          if (num34 > num37 - num38 && num34 < num37 + num38)
            flag5 = false;
          if (num35 > num37 - num38 && num35 < num37 + num38)
            flag5 = false;
          if (num36 > num37 - num38 && num36 < num37 + num38)
            flag5 = false;
          for (int index36 = num35; index36 < num36; ++index36)
          {
            for (int index37 = 0; index37 < (int) Game1.worldSurface; index37 += 5)
            {
              if (Game1.tile[index36, index37].active && Game1.tileDungeon[(int) Game1.tile[index36, index37].type])
              {
                flag5 = false;
                break;
              }
              if (!flag5)
                break;
            }
          }
        }
        int num39 = 0;
        for (int i11 = num35; i11 < num36; ++i11)
        {
          if (num39 > 0)
            --num39;
          if (i11 == num34 || num39 == 0)
          {
            for (int j8 = (int) num5; (double) j8 < Game1.worldSurface - 1.0; ++j8)
            {
              if (Game1.tile[i11, j8].active || Game1.tile[i11, j8].wall > (byte) 0)
              {
                if (i11 == num34)
                {
                  num39 = 20;
                  WorldGen.ChasmRunner(i11, j8, WorldGen.genRand.Next(150) + 150, true);
                  break;
                }
                if (WorldGen.genRand.Next(30) == 0 && num39 == 0)
                {
                  num39 = 20;
                  bool makeOrb = false;
                  if (WorldGen.genRand.Next(2) == 0)
                    makeOrb = true;
                  WorldGen.ChasmRunner(i11, j8, WorldGen.genRand.Next(50) + 50, makeOrb);
                  break;
                }
                break;
              }
            }
          }
        }
        double num40 = Game1.worldSurface + 40.0;
        for (int index38 = num35; index38 < num36; ++index38)
        {
          num40 += (double) WorldGen.genRand.Next(-2, 3);
          if (num40 < Game1.worldSurface + 30.0)
            num40 = Game1.worldSurface + 30.0;
          if (num40 > Game1.worldSurface + 50.0)
            num40 = Game1.worldSurface + 50.0;
          int i12 = index38;
          bool flag6 = false;
          for (int j9 = (int) num5; (double) j9 < num40; ++j9)
          {
            if (Game1.tile[i12, j9].active)
            {
              if (Game1.tile[i12, j9].type == (byte) 0 && (double) j9 < Game1.worldSurface - 1.0 && !flag6)
                WorldGen.SpreadGrass(i12, j9, grass: 23);
              flag6 = true;
              if (Game1.tile[i12, j9].type == (byte) 1 && i12 >= num35 + WorldGen.genRand.Next(5) && i12 <= num36 - WorldGen.genRand.Next(5))
                Game1.tile[i12, j9].type = (byte) 25;
              if (Game1.tile[i12, j9].type == (byte) 2)
                Game1.tile[i12, j9].type = (byte) 23;
            }
          }
        }
        for (int index39 = num35; index39 < num36; ++index39)
        {
          for (int index40 = 0; index40 < Game1.maxTilesY - 50; ++index40)
          {
            if (Game1.tile[index39, index40].active && Game1.tile[index39, index40].type == (byte) 31)
            {
              int num41 = index39 - 13;
              int num42 = index39 + 13;
              int num43 = index40 - 13;
              int num44 = index40 + 13;
              for (int index41 = num41; index41 < num42; ++index41)
              {
                if (index41 > 10 && index41 < Game1.maxTilesX - 10)
                {
                  for (int index42 = num43; index42 < num44; ++index42)
                  {
                    if (Math.Abs(index41 - index39) + Math.Abs(index42 - index40) < 9 + WorldGen.genRand.Next(11) && WorldGen.genRand.Next(3) != 0 && Game1.tile[index41, index42].type != (byte) 31)
                    {
                      Game1.tile[index41, index42].active = true;
                      Game1.tile[index41, index42].type = (byte) 25;
                      if (Math.Abs(index41 - index39) <= 1 && Math.Abs(index42 - index40) <= 1)
                        Game1.tile[index41, index42].active = false;
                    }
                    if (Game1.tile[index41, index42].type != (byte) 31 && Math.Abs(index41 - index39) <= 2 + WorldGen.genRand.Next(3) && Math.Abs(index42 - index40) <= 2 + WorldGen.genRand.Next(3))
                      Game1.tile[index41, index42].active = false;
                  }
                }
              }
            }
          }
        }
      }
      Game1.statusText = "Generating mountain caves...";
      for (int index = 0; index < WorldGen.numMCaves; ++index)
      {
        int i13 = WorldGen.mCaveX[index];
        int j10 = WorldGen.mCaveY[index];
        WorldGen.CaveOpenater(i13, j10);
        WorldGen.Cavinator(i13, j10, WorldGen.genRand.Next(40, 50));
      }
      Game1.statusText = "Creating beaches...";
      for (int index43 = 0; index43 < 2; ++index43)
      {
        if (index43 == 0)
        {
          int num45 = 0;
          int num46 = WorldGen.genRand.Next(125, 200);
          float num47 = 1f;
          int index44 = 0;
          while (!Game1.tile[num46 - 1, index44].active)
            ++index44;
          for (int index45 = num46 - 1; index45 >= num45; --index45)
          {
            num47 += (float) WorldGen.genRand.Next(10, 20) * 0.05f;
            for (int index46 = 0; (double) index46 < (double) index44 + (double) num47; ++index46)
            {
              if ((double) index46 < (double) index44 + (double) num47 * 0.75 - 3.0)
              {
                Game1.tile[index45, index46].active = false;
                if (index46 > index44)
                  Game1.tile[index45, index46].liquid = byte.MaxValue;
                else if (index46 == index44)
                  Game1.tile[index45, index46].liquid = (byte) 127;
              }
              else if (index46 > index44)
              {
                Game1.tile[index45, index46].type = (byte) 53;
                Game1.tile[index45, index46].active = true;
              }
              Game1.tile[index45, index46].wall = (byte) 0;
            }
          }
        }
        else
        {
          int index47 = Game1.maxTilesX - WorldGen.genRand.Next(125, 200);
          int maxTilesX = Game1.maxTilesX;
          float num48 = 1f;
          int index48 = 0;
          while (!Game1.tile[index47, index48].active)
            ++index48;
          for (int index49 = index47; index49 < maxTilesX; ++index49)
          {
            num48 += (float) WorldGen.genRand.Next(10, 20) * 0.05f;
            for (int index50 = 0; (double) index50 < (double) index48 + (double) num48; ++index50)
            {
              if ((double) index50 < (double) index48 + (double) num48 * 0.75 - 3.0)
              {
                Game1.tile[index49, index50].active = false;
                if (index50 > index48)
                  Game1.tile[index49, index50].liquid = byte.MaxValue;
                else if (index50 == index48)
                  Game1.tile[index49, index50].liquid = (byte) 127;
              }
              else if (index50 > index48)
              {
                Game1.tile[index49, index50].type = (byte) 53;
                Game1.tile[index49, index50].active = true;
              }
              Game1.tile[index49, index50].wall = (byte) 0;
            }
          }
        }
      }
      Game1.statusText = "Adding gems...";
      for (int type = 63; type <= 68; ++type)
      {
        float num49 = 0.0f;
        switch (type)
        {
          case 63:
            num49 = (float) Game1.maxTilesX * 0.3f;
            break;
          case 64:
            num49 = (float) Game1.maxTilesX * 0.1f;
            break;
          case 65:
            num49 = (float) Game1.maxTilesX * 0.25f;
            break;
          case 66:
            num49 = (float) Game1.maxTilesX * 0.45f;
            break;
          case 67:
            num49 = (float) Game1.maxTilesX * 0.5f;
            break;
          case 68:
            num49 = (float) Game1.maxTilesX * 0.05f;
            break;
        }
        float num50 = num49 * 0.2f;
        for (int index = 0; (double) index < (double) num50; ++index)
        {
          int i14 = WorldGen.genRand.Next(0, Game1.maxTilesX);
          int j11;
          for (j11 = WorldGen.genRand.Next((int) Game1.worldSurface, Game1.maxTilesY); Game1.tile[i14, j11].type != (byte) 1; j11 = WorldGen.genRand.Next((int) Game1.worldSurface, Game1.maxTilesY))
            i14 = WorldGen.genRand.Next(0, Game1.maxTilesX);
          WorldGen.TileRunner(i14, j11, (double) WorldGen.genRand.Next(2, 6), WorldGen.genRand.Next(3, 7), type);
        }
      }
      for (int index51 = 0; index51 < Game1.maxTilesX; ++index51)
      {
        Game1.statusText = "Gravitating sand: " + (object) (int) ((double) ((float) index51 / (float) (Game1.maxTilesX - 1)) * 100.0) + "%";
        for (int index52 = Game1.maxTilesY - 5; index52 > 0; --index52)
        {
          if (Game1.tile[index51, index52].active && Game1.tile[index51, index52].type == (byte) 53)
          {
            for (int index53 = index52; !Game1.tile[index51, index53 + 1].active && index53 < Game1.maxTilesY - 5; ++index53)
            {
              Game1.tile[index51, index53 + 1].active = true;
              Game1.tile[index51, index53 + 1].type = (byte) 53;
            }
          }
        }
      }
      for (int index54 = 3; index54 < Game1.maxTilesX - 3; ++index54)
      {
        Game1.statusText = "Cleaning up dirt backgrounds: " + (object) (int) ((double) ((float) index54 / (float) Game1.maxTilesX) * 100.0 + 1.0) + "%";
        for (int index55 = 0; (double) index55 < Game1.worldSurface; ++index55)
        {
          if (Game1.tile[index54, index55].wall == (byte) 2)
            Game1.tile[index54, index55].wall = (byte) 0;
          if (Game1.tile[index54, index55].type != (byte) 53)
          {
            if (Game1.tile[index54 - 1, index55].wall == (byte) 2)
              Game1.tile[index54 - 1, index55].wall = (byte) 0;
            if (Game1.tile[index54 - 2, index55].wall == (byte) 2 && WorldGen.genRand.Next(2) == 0)
              Game1.tile[index54 - 2, index55].wall = (byte) 0;
            if (Game1.tile[index54 - 3, index55].wall == (byte) 2 && WorldGen.genRand.Next(2) == 0)
              Game1.tile[index54 - 3, index55].wall = (byte) 0;
            if (Game1.tile[index54 + 1, index55].wall == (byte) 2)
              Game1.tile[index54 + 1, index55].wall = (byte) 0;
            if (Game1.tile[index54 + 2, index55].wall == (byte) 2 && WorldGen.genRand.Next(2) == 0)
              Game1.tile[index54 + 2, index55].wall = (byte) 0;
            if (Game1.tile[index54 + 3, index55].wall == (byte) 2 && WorldGen.genRand.Next(2) == 0)
              Game1.tile[index54 + 3, index55].wall = (byte) 0;
            if (Game1.tile[index54, index55].active)
              break;
          }
        }
      }
      for (int index = 0; index < (int) ((double) (Game1.maxTilesX * Game1.maxTilesY) * 1E-05); ++index)
      {
        Game1.statusText = "Placing alters: " + (object) (int) ((double) ((float) index / ((float) (Game1.maxTilesX * Game1.maxTilesY) * 1E-05f)) * 100.0 + 1.0) + "%";
        bool flag = false;
        int num51 = 0;
        while (!flag)
        {
          int x2 = WorldGen.genRand.Next(1, Game1.maxTilesX);
          int y2 = (int) (num6 + 20.0);
          WorldGen.Place3x2(x2, y2, 26);
          if (Game1.tile[x2, y2].type == (byte) 26)
          {
            flag = true;
          }
          else
          {
            ++num51;
            if (num51 >= 10000)
              flag = true;
          }
        }
      }
      Liquid.QuickWater(3);
      WorldGen.WaterCheck();
      int num52 = 0;
      Liquid.quickSettle = true;
      int num53;
      while (num52 < 10)
      {
        int num54 = Liquid.numLiquid + LiquidBuffer.numLiquidBuffer;
        ++num52;
        float num55 = 0.0f;
        while (Liquid.numLiquid > 0)
        {
          float num56 = (float) (num54 - (Liquid.numLiquid + LiquidBuffer.numLiquidBuffer)) / (float) num54;
          if (Liquid.numLiquid + LiquidBuffer.numLiquidBuffer > num54)
            num54 = Liquid.numLiquid + LiquidBuffer.numLiquidBuffer;
          if ((double) num56 > (double) num55)
            num55 = num56;
          else
            num56 = num55;
          if (num52 == 1)
            Game1.statusText = "Settling liquids: " + (object) (int) ((double) num56 * 100.0 / 3.0 + 33.0) + "%";
          int num57 = 10;
          if (num52 > num57)
            num53 = num52;
          Liquid.UpdateLiquid();
        }
        WorldGen.WaterCheck();
        Game1.statusText = "Settling liquids: " + (object) (int) ((double) num52 * 10.0 / 3.0 + 66.0) + "%";
      }
      Liquid.quickSettle = false;
      for (int index = 0; index < (int) ((double) (Game1.maxTilesX * Game1.maxTilesY) * 2E-05); ++index)
      {
        Game1.statusText = "Placing life crystals: " + (object) (int) ((double) ((float) index / ((float) (Game1.maxTilesX * Game1.maxTilesY) * 2E-05f)) * 100.0 + 1.0) + "%";
        bool flag = false;
        int num58 = 0;
        while (!flag)
        {
          if (WorldGen.AddLifeCrystal(WorldGen.genRand.Next(1, Game1.maxTilesX), WorldGen.genRand.Next((int) (num6 + 20.0), Game1.maxTilesY)))
          {
            flag = true;
          }
          else
          {
            ++num58;
            if (num58 >= 10000)
              flag = true;
          }
        }
      }
      for (int index = 0; index < (int) ((double) (Game1.maxTilesX * Game1.maxTilesY) * 1.5E-05); ++index)
      {
        Game1.statusText = "Hiding treasure: " + (object) (int) ((double) ((float) index / ((float) (Game1.maxTilesX * Game1.maxTilesY) * 2.5E-05f)) * 100.0 + 1.0) + "%";
        bool flag = false;
        int num59 = 0;
        while (!flag)
        {
          if (WorldGen.AddBuriedChest(WorldGen.genRand.Next(1, Game1.maxTilesX), WorldGen.genRand.Next((int) (num6 + 20.0), Game1.maxTilesY)))
          {
            flag = true;
          }
          else
          {
            ++num59;
            if (num59 >= 10000)
              flag = true;
          }
        }
      }
      float num60 = (float) (Game1.maxTilesX / 4200);
      int num61 = 0;
      for (int index = 0; (double) index < 10.0 * (double) num60; ++index)
      {
        ++num61;
        int contain;
        if (num61 == 1)
        {
          contain = 186;
        }
        else
        {
          contain = 187;
          num61 = 0;
        }
        int i15;
        int j12;
        for (bool flag = false; !flag; flag = WorldGen.AddBuriedChest(i15, j12, contain))
        {
          i15 = WorldGen.genRand.Next(1, Game1.maxTilesX);
          for (j12 = WorldGen.genRand.Next(1, Game1.maxTilesY - 200); Game1.tile[i15, j12].liquid < (byte) 200 || Game1.tile[i15, j12].lava; j12 = WorldGen.genRand.Next(1, Game1.maxTilesY - 200))
            i15 = WorldGen.genRand.Next(1, Game1.maxTilesX);
        }
      }
      for (int index = 0; index < WorldGen.numIslandHouses; ++index)
        WorldGen.IslandHouse(WorldGen.fihX[index], WorldGen.fihY[index]);
      for (int index = 0; index < (int) ((double) (Game1.maxTilesX * Game1.maxTilesY) * 0.0007); ++index)
      {
        float num62 = (float) index / ((float) (Game1.maxTilesX * Game1.maxTilesY) * 0.0007f);
        Game1.statusText = "Placing breakables: " + (object) (int) ((double) num62 * 100.0 + 1.0) + "%";
        bool flag7 = false;
        int num63 = 0;
        while (!flag7)
        {
          int num64 = WorldGen.genRand.Next((int) num6, Game1.maxTilesY - 10);
          if ((double) num62 > 0.8)
            num64 = (int) num5;
          int x3 = WorldGen.genRand.Next(1, Game1.maxTilesX);
          bool flag8 = false;
          for (int y3 = num64; y3 < Game1.maxTilesY; ++y3)
          {
            if (!flag8)
            {
              if (Game1.tile[x3, y3].active && Game1.tileSolid[(int) Game1.tile[x3, y3].type] && !Game1.tile[x3, y3 - 1].lava)
                flag8 = true;
            }
            else
            {
              if (WorldGen.PlacePot(x3, y3))
              {
                flag7 = true;
                break;
              }
              ++num63;
              if (num63 >= 10000)
              {
                flag7 = true;
                break;
              }
            }
          }
        }
      }
      Game1.statusText = "Spreading grass...";
      for (int index = 0; index < Game1.maxTilesX; ++index)
      {
        int i16 = index;
        bool flag = true;
        for (int j13 = 0; (double) j13 < Game1.worldSurface - 1.0; ++j13)
        {
          if (Game1.tile[i16, j13].active)
          {
            if (flag && Game1.tile[i16, j13].type == (byte) 0)
              WorldGen.SpreadGrass(i16, j13);
            if ((double) j13 <= num6)
              flag = false;
            else
              break;
          }
          else if (Game1.tile[i16, j13].wall == (byte) 0)
            flag = true;
        }
      }
      for (int index56 = 0; index56 < Game1.maxTilesY; ++index56)
      {
        int index57 = Game1.maxTilesX / 2;
        if (Game1.tile[index57, index56].active)
        {
          Game1.spawnTileX = index57;
          Game1.spawnTileY = index56;
          Game1.tile[index57, index56 - 1].lighted = true;
          break;
        }
      }
      int index58 = NPC.NewNPC(Game1.spawnTileX * 16, Game1.spawnTileY * 16, 22);
      Game1.npc[index58].homeTileX = Game1.spawnTileX;
      Game1.npc[index58].homeTileY = Game1.spawnTileY;
      Game1.npc[index58].direction = 1;
      Game1.npc[index58].homeless = true;
      Game1.statusText = "Planting sunflowers...";
      for (int index59 = 0; (double) index59 < (double) Game1.maxTilesX * 0.002; ++index59)
      {
        num53 = 0;
        int num65 = Game1.maxTilesX / 2;
        int num66 = WorldGen.genRand.Next(Game1.maxTilesX);
        int num67 = num66 - WorldGen.genRand.Next(10) - 7;
        int num68 = num66 + WorldGen.genRand.Next(10) + 7;
        if (num67 < 0)
          num67 = 0;
        if (num68 > Game1.maxTilesX - 1)
          num68 = Game1.maxTilesX - 1;
        for (int i17 = num67; i17 < num68; ++i17)
        {
          for (int index60 = 1; (double) index60 < Game1.worldSurface - 1.0; ++index60)
          {
            if (Game1.tile[i17, index60].type == (byte) 1 && Game1.tile[i17, index60].active)
              Game1.tile[i17, index60].type = (byte) 2;
            if (Game1.tile[i17 + 1, index60].type == (byte) 1 && Game1.tile[i17 + 1, index60].active)
              Game1.tile[i17 + 1, index60].type = (byte) 2;
            if (Game1.tile[i17, index60].type == (byte) 2 && Game1.tile[i17, index60].active && !Game1.tile[i17, index60 - 1].active)
              WorldGen.PlaceTile(i17, index60 - 1, 27, true);
            if (Game1.tile[i17, index60].active)
              break;
          }
        }
      }
      Game1.statusText = "Planting trees...";
      for (int index61 = 0; (double) index61 < (double) Game1.maxTilesX * 0.003; ++index61)
      {
        int num69 = WorldGen.genRand.Next(50, Game1.maxTilesX - 50);
        int num70 = WorldGen.genRand.Next(25, 50);
        for (int index62 = num69 - num70; index62 < num69 + num70; ++index62)
        {
          for (int index63 = 20; (double) index63 < Game1.worldSurface; ++index63)
          {
            if (Game1.tile[index62, index63].active)
            {
              if (Game1.tile[index62, index63].type == (byte) 1)
                Game1.tile[index62, index63].type = (byte) 2;
              if (Game1.tile[index62, index63 + 1].type == (byte) 1)
              {
                Game1.tile[index62, index63 + 1].type = (byte) 2;
                break;
              }
              break;
            }
          }
        }
        for (int i18 = num69 - num70; i18 < num69 + num70; ++i18)
        {
          for (int y4 = 20; (double) y4 < Game1.worldSurface; ++y4)
            WorldGen.GrowEpicTree(i18, y4);
        }
      }
      WorldGen.AddTrees();
      Game1.statusText = "Planting weeds...";
      WorldGen.AddPlants();
      for (int i19 = 0; i19 < Game1.maxTilesX; ++i19)
      {
        for (int worldSurface = (int) Game1.worldSurface; worldSurface < Game1.maxTilesY; ++worldSurface)
        {
          if (Game1.tile[i19, worldSurface].active)
          {
            if (Game1.tile[i19, worldSurface].type == (byte) 70 && !Game1.tile[i19, worldSurface - 1].active)
            {
              WorldGen.GrowShroom(i19, worldSurface);
              if (!Game1.tile[i19, worldSurface - 1].active)
                WorldGen.PlaceTile(i19, worldSurface - 1, 71, true);
            }
            if (Game1.tile[i19, worldSurface].type == (byte) 60 && !Game1.tile[i19, worldSurface - 1].active)
              WorldGen.PlaceTile(i19, worldSurface - 1, 61, true);
          }
        }
      }
      Game1.statusText = "Growing vines...";
      for (int index64 = 0; index64 < Game1.maxTilesX; ++index64)
      {
        int num71 = 0;
        for (int index65 = 0; (double) index65 < Game1.worldSurface; ++index65)
        {
          if (num71 > 0 && !Game1.tile[index64, index65].active)
          {
            Game1.tile[index64, index65].active = true;
            Game1.tile[index64, index65].type = (byte) 52;
            --num71;
          }
          else
            num71 = 0;
          if (Game1.tile[index64, index65].active && Game1.tile[index64, index65].type == (byte) 2 && WorldGen.genRand.Next(5) < 3)
            num71 = WorldGen.genRand.Next(1, 10);
        }
        int num72 = 0;
        for (int worldSurface = (int) Game1.worldSurface; worldSurface < Game1.maxTilesY; ++worldSurface)
        {
          if (num72 > 0 && !Game1.tile[index64, worldSurface].active)
          {
            Game1.tile[index64, worldSurface].active = true;
            Game1.tile[index64, worldSurface].type = (byte) 62;
            --num72;
          }
          else
            num72 = 0;
          if (Game1.tile[index64, worldSurface].active && Game1.tile[index64, worldSurface].type == (byte) 60 && WorldGen.genRand.Next(5) < 3)
            num72 = WorldGen.genRand.Next(1, 10);
        }
      }
      Game1.statusText = "Planting flowers...";
      for (int index66 = 0; (double) index66 < (double) Game1.maxTilesX * 0.005; ++index66)
      {
        int index67 = WorldGen.genRand.Next(20, Game1.maxTilesX - 20);
        int num73 = WorldGen.genRand.Next(5, 15);
        int num74 = WorldGen.genRand.Next(15, 30);
        for (int index68 = 1; (double) index68 < Game1.worldSurface - 1.0; ++index68)
        {
          if (Game1.tile[index67, index68].active)
          {
            for (int index69 = index67 - num73; index69 < index67 + num73; ++index69)
            {
              for (int index70 = index68 - num74; index70 < index68 + num74; ++index70)
              {
                if (Game1.tile[index69, index70].type == (byte) 3 || Game1.tile[index69, index70].type == (byte) 24)
                  Game1.tile[index69, index70].frameX = (short) (WorldGen.genRand.Next(6, 8) * 18);
              }
            }
            break;
          }
        }
      }
      Game1.statusText = "Planting mushrooms...";
      for (int index71 = 0; (double) index71 < (double) Game1.maxTilesX * 0.002; ++index71)
      {
        int index72 = WorldGen.genRand.Next(20, Game1.maxTilesX - 20);
        int num75 = WorldGen.genRand.Next(4, 10);
        int num76 = WorldGen.genRand.Next(15, 30);
        for (int index73 = 1; (double) index73 < Game1.worldSurface - 1.0; ++index73)
        {
          if (Game1.tile[index72, index73].active)
          {
            for (int index74 = index72 - num75; index74 < index72 + num75; ++index74)
            {
              for (int index75 = index73 - num76; index75 < index73 + num76; ++index75)
              {
                if (Game1.tile[index74, index75].type == (byte) 3 || Game1.tile[index74, index75].type == (byte) 24)
                  Game1.tile[index74, index75].frameX = (short) 144;
              }
            }
            break;
          }
        }
      }
      WorldGen.gen = false;
    }

    public static void GrowEpicTree(int i, int y)
    {
      int index1 = y;
      while (Game1.tile[i, index1].type == (byte) 20)
        ++index1;
      if (!Game1.tile[i, index1].active || Game1.tile[i, index1].type != (byte) 2 || Game1.tile[i, index1 - 1].wall != (byte) 0 || !Game1.tile[i - 1, index1].active || Game1.tile[i - 1, index1].type != (byte) 2 || !Game1.tile[i + 1, index1].active || Game1.tile[i + 1, index1].type != (byte) 2 || !WorldGen.EmptyTileCheck(i - 2, i + 2, index1 - 55, index1 - 1, 20))
        return;
      bool flag1 = false;
      bool flag2 = false;
      int num1 = WorldGen.genRand.Next(20, 50);
      for (int index2 = index1 - num1; index2 < index1; ++index2)
      {
        Game1.tile[i, index2].frameNumber = (byte) WorldGen.genRand.Next(3);
        Game1.tile[i, index2].active = true;
        Game1.tile[i, index2].type = (byte) 5;
        int num2 = WorldGen.genRand.Next(3);
        int num3 = WorldGen.genRand.Next(10);
        if (index2 == index1 - 1 || index2 == index1 - num1)
          num3 = 0;
        while ((num3 == 5 || num3 == 7) && flag1 || (num3 == 6 || num3 == 7) && flag2)
          num3 = WorldGen.genRand.Next(10);
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
              Game1.tile[i, index2].frameX = (short) 0;
              Game1.tile[i, index2].frameY = (short) 66;
            }
            if (num2 == 1)
            {
              Game1.tile[i, index2].frameX = (short) 0;
              Game1.tile[i, index2].frameY = (short) 88;
            }
            if (num2 == 2)
            {
              Game1.tile[i, index2].frameX = (short) 0;
              Game1.tile[i, index2].frameY = (short) 110;
              break;
            }
            break;
          case 2:
            if (num2 == 0)
            {
              Game1.tile[i, index2].frameX = (short) 22;
              Game1.tile[i, index2].frameY = (short) 0;
            }
            if (num2 == 1)
            {
              Game1.tile[i, index2].frameX = (short) 22;
              Game1.tile[i, index2].frameY = (short) 22;
            }
            if (num2 == 2)
            {
              Game1.tile[i, index2].frameX = (short) 22;
              Game1.tile[i, index2].frameY = (short) 44;
              break;
            }
            break;
          case 3:
            if (num2 == 0)
            {
              Game1.tile[i, index2].frameX = (short) 44;
              Game1.tile[i, index2].frameY = (short) 66;
            }
            if (num2 == 1)
            {
              Game1.tile[i, index2].frameX = (short) 44;
              Game1.tile[i, index2].frameY = (short) 88;
            }
            if (num2 == 2)
            {
              Game1.tile[i, index2].frameX = (short) 44;
              Game1.tile[i, index2].frameY = (short) 110;
              break;
            }
            break;
          case 4:
            if (num2 == 0)
            {
              Game1.tile[i, index2].frameX = (short) 22;
              Game1.tile[i, index2].frameY = (short) 66;
            }
            if (num2 == 1)
            {
              Game1.tile[i, index2].frameX = (short) 22;
              Game1.tile[i, index2].frameY = (short) 88;
            }
            if (num2 == 2)
            {
              Game1.tile[i, index2].frameX = (short) 22;
              Game1.tile[i, index2].frameY = (short) 110;
              break;
            }
            break;
          case 5:
            if (num2 == 0)
            {
              Game1.tile[i, index2].frameX = (short) 88;
              Game1.tile[i, index2].frameY = (short) 0;
            }
            if (num2 == 1)
            {
              Game1.tile[i, index2].frameX = (short) 88;
              Game1.tile[i, index2].frameY = (short) 22;
            }
            if (num2 == 2)
            {
              Game1.tile[i, index2].frameX = (short) 88;
              Game1.tile[i, index2].frameY = (short) 44;
              break;
            }
            break;
          case 6:
            if (num2 == 0)
            {
              Game1.tile[i, index2].frameX = (short) 66;
              Game1.tile[i, index2].frameY = (short) 66;
            }
            if (num2 == 1)
            {
              Game1.tile[i, index2].frameX = (short) 66;
              Game1.tile[i, index2].frameY = (short) 88;
            }
            if (num2 == 2)
            {
              Game1.tile[i, index2].frameX = (short) 66;
              Game1.tile[i, index2].frameY = (short) 110;
              break;
            }
            break;
          case 7:
            if (num2 == 0)
            {
              Game1.tile[i, index2].frameX = (short) 110;
              Game1.tile[i, index2].frameY = (short) 66;
            }
            if (num2 == 1)
            {
              Game1.tile[i, index2].frameX = (short) 110;
              Game1.tile[i, index2].frameY = (short) 88;
            }
            if (num2 == 2)
            {
              Game1.tile[i, index2].frameX = (short) 110;
              Game1.tile[i, index2].frameY = (short) 110;
              break;
            }
            break;
          default:
            if (num2 == 0)
            {
              Game1.tile[i, index2].frameX = (short) 0;
              Game1.tile[i, index2].frameY = (short) 0;
            }
            if (num2 == 1)
            {
              Game1.tile[i, index2].frameX = (short) 0;
              Game1.tile[i, index2].frameY = (short) 22;
            }
            if (num2 == 2)
            {
              Game1.tile[i, index2].frameX = (short) 0;
              Game1.tile[i, index2].frameY = (short) 44;
            }
            break;
        }
        if (num3 == 5 || num3 == 7)
        {
          Game1.tile[i - 1, index2].active = true;
          Game1.tile[i - 1, index2].type = (byte) 5;
          int num4 = WorldGen.genRand.Next(3);
          if (WorldGen.genRand.Next(3) < 2)
          {
            if (num4 == 0)
            {
              Game1.tile[i - 1, index2].frameX = (short) 44;
              Game1.tile[i - 1, index2].frameY = (short) 198;
            }
            if (num4 == 1)
            {
              Game1.tile[i - 1, index2].frameX = (short) 44;
              Game1.tile[i - 1, index2].frameY = (short) 220;
            }
            if (num4 == 2)
            {
              Game1.tile[i - 1, index2].frameX = (short) 44;
              Game1.tile[i - 1, index2].frameY = (short) 242;
            }
          }
          else
          {
            if (num4 == 0)
            {
              Game1.tile[i - 1, index2].frameX = (short) 66;
              Game1.tile[i - 1, index2].frameY = (short) 0;
            }
            if (num4 == 1)
            {
              Game1.tile[i - 1, index2].frameX = (short) 66;
              Game1.tile[i - 1, index2].frameY = (short) 22;
            }
            if (num4 == 2)
            {
              Game1.tile[i - 1, index2].frameX = (short) 66;
              Game1.tile[i - 1, index2].frameY = (short) 44;
            }
          }
        }
        if (num3 == 6 || num3 == 7)
        {
          Game1.tile[i + 1, index2].active = true;
          Game1.tile[i + 1, index2].type = (byte) 5;
          int num5 = WorldGen.genRand.Next(3);
          if (WorldGen.genRand.Next(3) < 2)
          {
            if (num5 == 0)
            {
              Game1.tile[i + 1, index2].frameX = (short) 66;
              Game1.tile[i + 1, index2].frameY = (short) 198;
            }
            if (num5 == 1)
            {
              Game1.tile[i + 1, index2].frameX = (short) 66;
              Game1.tile[i + 1, index2].frameY = (short) 220;
            }
            if (num5 == 2)
            {
              Game1.tile[i + 1, index2].frameX = (short) 66;
              Game1.tile[i + 1, index2].frameY = (short) 242;
            }
          }
          else
          {
            if (num5 == 0)
            {
              Game1.tile[i + 1, index2].frameX = (short) 88;
              Game1.tile[i + 1, index2].frameY = (short) 66;
            }
            if (num5 == 1)
            {
              Game1.tile[i + 1, index2].frameX = (short) 88;
              Game1.tile[i + 1, index2].frameY = (short) 88;
            }
            if (num5 == 2)
            {
              Game1.tile[i + 1, index2].frameX = (short) 88;
              Game1.tile[i + 1, index2].frameY = (short) 110;
            }
          }
        }
      }
      int num6 = WorldGen.genRand.Next(3);
      if (num6 == 0 || num6 == 1)
      {
        Game1.tile[i + 1, index1 - 1].active = true;
        Game1.tile[i + 1, index1 - 1].type = (byte) 5;
        int num7 = WorldGen.genRand.Next(3);
        if (num7 == 0)
        {
          Game1.tile[i + 1, index1 - 1].frameX = (short) 22;
          Game1.tile[i + 1, index1 - 1].frameY = (short) 132;
        }
        if (num7 == 1)
        {
          Game1.tile[i + 1, index1 - 1].frameX = (short) 22;
          Game1.tile[i + 1, index1 - 1].frameY = (short) 154;
        }
        if (num7 == 2)
        {
          Game1.tile[i + 1, index1 - 1].frameX = (short) 22;
          Game1.tile[i + 1, index1 - 1].frameY = (short) 176;
        }
      }
      if (num6 == 0 || num6 == 2)
      {
        Game1.tile[i - 1, index1 - 1].active = true;
        Game1.tile[i - 1, index1 - 1].type = (byte) 5;
        int num8 = WorldGen.genRand.Next(3);
        if (num8 == 0)
        {
          Game1.tile[i - 1, index1 - 1].frameX = (short) 44;
          Game1.tile[i - 1, index1 - 1].frameY = (short) 132;
        }
        if (num8 == 1)
        {
          Game1.tile[i - 1, index1 - 1].frameX = (short) 44;
          Game1.tile[i - 1, index1 - 1].frameY = (short) 154;
        }
        if (num8 == 2)
        {
          Game1.tile[i - 1, index1 - 1].frameX = (short) 44;
          Game1.tile[i - 1, index1 - 1].frameY = (short) 176;
        }
      }
      int num9 = WorldGen.genRand.Next(3);
      switch (num6)
      {
        case 0:
          if (num9 == 0)
          {
            Game1.tile[i, index1 - 1].frameX = (short) 88;
            Game1.tile[i, index1 - 1].frameY = (short) 132;
          }
          if (num9 == 1)
          {
            Game1.tile[i, index1 - 1].frameX = (short) 88;
            Game1.tile[i, index1 - 1].frameY = (short) 154;
          }
          if (num9 == 2)
          {
            Game1.tile[i, index1 - 1].frameX = (short) 88;
            Game1.tile[i, index1 - 1].frameY = (short) 176;
            break;
          }
          break;
        case 1:
          if (num9 == 0)
          {
            Game1.tile[i, index1 - 1].frameX = (short) 0;
            Game1.tile[i, index1 - 1].frameY = (short) 132;
          }
          if (num9 == 1)
          {
            Game1.tile[i, index1 - 1].frameX = (short) 0;
            Game1.tile[i, index1 - 1].frameY = (short) 154;
          }
          if (num9 == 2)
          {
            Game1.tile[i, index1 - 1].frameX = (short) 0;
            Game1.tile[i, index1 - 1].frameY = (short) 176;
            break;
          }
          break;
        case 2:
          if (num9 == 0)
          {
            Game1.tile[i, index1 - 1].frameX = (short) 66;
            Game1.tile[i, index1 - 1].frameY = (short) 132;
          }
          if (num9 == 1)
          {
            Game1.tile[i, index1 - 1].frameX = (short) 66;
            Game1.tile[i, index1 - 1].frameY = (short) 154;
          }
          if (num9 == 2)
          {
            Game1.tile[i, index1 - 1].frameX = (short) 66;
            Game1.tile[i, index1 - 1].frameY = (short) 176;
          }
          break;
      }
      if (WorldGen.genRand.Next(3) < 2)
      {
        int num10 = WorldGen.genRand.Next(3);
        if (num10 == 0)
        {
          Game1.tile[i, index1 - num1].frameX = (short) 22;
          Game1.tile[i, index1 - num1].frameY = (short) 198;
        }
        if (num10 == 1)
        {
          Game1.tile[i, index1 - num1].frameX = (short) 22;
          Game1.tile[i, index1 - num1].frameY = (short) 220;
        }
        if (num10 == 2)
        {
          Game1.tile[i, index1 - num1].frameX = (short) 22;
          Game1.tile[i, index1 - num1].frameY = (short) 242;
        }
      }
      else
      {
        int num11 = WorldGen.genRand.Next(3);
        if (num11 == 0)
        {
          Game1.tile[i, index1 - num1].frameX = (short) 0;
          Game1.tile[i, index1 - num1].frameY = (short) 198;
        }
        if (num11 == 1)
        {
          Game1.tile[i, index1 - num1].frameX = (short) 0;
          Game1.tile[i, index1 - num1].frameY = (short) 220;
        }
        if (num11 == 2)
        {
          Game1.tile[i, index1 - num1].frameX = (short) 0;
          Game1.tile[i, index1 - num1].frameY = (short) 242;
        }
      }
      WorldGen.RangeFrame(i - 2, index1 - num1 - 1, i + 2, index1 + 1);
      if (Game1.netMode == 2)
        NetMessage.SendTileSquare(-1, i, (int) ((double) index1 - (double) num1 * 0.5), num1 + 1);
    }

    public static void GrowTree(int i, int y)
    {
      int index1 = y;
      while (Game1.tile[i, index1].type == (byte) 20)
        ++index1;
      if (Game1.tile[i - 1, index1 - 1].liquid != (byte) 0 || Game1.tile[i - 1, index1 - 1].liquid != (byte) 0 || Game1.tile[i + 1, index1 - 1].liquid != (byte) 0 || !Game1.tile[i, index1].active || Game1.tile[i, index1].type != (byte) 2 || Game1.tile[i, index1 - 1].wall != (byte) 0 || !Game1.tile[i - 1, index1].active || Game1.tile[i - 1, index1].type != (byte) 2 || !Game1.tile[i + 1, index1].active || Game1.tile[i + 1, index1].type != (byte) 2 || !WorldGen.EmptyTileCheck(i - 2, i + 2, index1 - 14, index1 - 1, 20))
        return;
      bool flag1 = false;
      bool flag2 = false;
      int num1 = WorldGen.genRand.Next(5, 15);
      for (int index2 = index1 - num1; index2 < index1; ++index2)
      {
        Game1.tile[i, index2].frameNumber = (byte) WorldGen.genRand.Next(3);
        Game1.tile[i, index2].active = true;
        Game1.tile[i, index2].type = (byte) 5;
        int num2 = WorldGen.genRand.Next(3);
        int num3 = WorldGen.genRand.Next(10);
        if (index2 == index1 - 1 || index2 == index1 - num1)
          num3 = 0;
        while ((num3 == 5 || num3 == 7) && flag1 || (num3 == 6 || num3 == 7) && flag2)
          num3 = WorldGen.genRand.Next(10);
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
              Game1.tile[i, index2].frameX = (short) 0;
              Game1.tile[i, index2].frameY = (short) 66;
            }
            if (num2 == 1)
            {
              Game1.tile[i, index2].frameX = (short) 0;
              Game1.tile[i, index2].frameY = (short) 88;
            }
            if (num2 == 2)
            {
              Game1.tile[i, index2].frameX = (short) 0;
              Game1.tile[i, index2].frameY = (short) 110;
              break;
            }
            break;
          case 2:
            if (num2 == 0)
            {
              Game1.tile[i, index2].frameX = (short) 22;
              Game1.tile[i, index2].frameY = (short) 0;
            }
            if (num2 == 1)
            {
              Game1.tile[i, index2].frameX = (short) 22;
              Game1.tile[i, index2].frameY = (short) 22;
            }
            if (num2 == 2)
            {
              Game1.tile[i, index2].frameX = (short) 22;
              Game1.tile[i, index2].frameY = (short) 44;
              break;
            }
            break;
          case 3:
            if (num2 == 0)
            {
              Game1.tile[i, index2].frameX = (short) 44;
              Game1.tile[i, index2].frameY = (short) 66;
            }
            if (num2 == 1)
            {
              Game1.tile[i, index2].frameX = (short) 44;
              Game1.tile[i, index2].frameY = (short) 88;
            }
            if (num2 == 2)
            {
              Game1.tile[i, index2].frameX = (short) 44;
              Game1.tile[i, index2].frameY = (short) 110;
              break;
            }
            break;
          case 4:
            if (num2 == 0)
            {
              Game1.tile[i, index2].frameX = (short) 22;
              Game1.tile[i, index2].frameY = (short) 66;
            }
            if (num2 == 1)
            {
              Game1.tile[i, index2].frameX = (short) 22;
              Game1.tile[i, index2].frameY = (short) 88;
            }
            if (num2 == 2)
            {
              Game1.tile[i, index2].frameX = (short) 22;
              Game1.tile[i, index2].frameY = (short) 110;
              break;
            }
            break;
          case 5:
            if (num2 == 0)
            {
              Game1.tile[i, index2].frameX = (short) 88;
              Game1.tile[i, index2].frameY = (short) 0;
            }
            if (num2 == 1)
            {
              Game1.tile[i, index2].frameX = (short) 88;
              Game1.tile[i, index2].frameY = (short) 22;
            }
            if (num2 == 2)
            {
              Game1.tile[i, index2].frameX = (short) 88;
              Game1.tile[i, index2].frameY = (short) 44;
              break;
            }
            break;
          case 6:
            if (num2 == 0)
            {
              Game1.tile[i, index2].frameX = (short) 66;
              Game1.tile[i, index2].frameY = (short) 66;
            }
            if (num2 == 1)
            {
              Game1.tile[i, index2].frameX = (short) 66;
              Game1.tile[i, index2].frameY = (short) 88;
            }
            if (num2 == 2)
            {
              Game1.tile[i, index2].frameX = (short) 66;
              Game1.tile[i, index2].frameY = (short) 110;
              break;
            }
            break;
          case 7:
            if (num2 == 0)
            {
              Game1.tile[i, index2].frameX = (short) 110;
              Game1.tile[i, index2].frameY = (short) 66;
            }
            if (num2 == 1)
            {
              Game1.tile[i, index2].frameX = (short) 110;
              Game1.tile[i, index2].frameY = (short) 88;
            }
            if (num2 == 2)
            {
              Game1.tile[i, index2].frameX = (short) 110;
              Game1.tile[i, index2].frameY = (short) 110;
              break;
            }
            break;
          default:
            if (num2 == 0)
            {
              Game1.tile[i, index2].frameX = (short) 0;
              Game1.tile[i, index2].frameY = (short) 0;
            }
            if (num2 == 1)
            {
              Game1.tile[i, index2].frameX = (short) 0;
              Game1.tile[i, index2].frameY = (short) 22;
            }
            if (num2 == 2)
            {
              Game1.tile[i, index2].frameX = (short) 0;
              Game1.tile[i, index2].frameY = (short) 44;
            }
            break;
        }
        if (num3 == 5 || num3 == 7)
        {
          Game1.tile[i - 1, index2].active = true;
          Game1.tile[i - 1, index2].type = (byte) 5;
          int num4 = WorldGen.genRand.Next(3);
          if (WorldGen.genRand.Next(3) < 2)
          {
            if (num4 == 0)
            {
              Game1.tile[i - 1, index2].frameX = (short) 44;
              Game1.tile[i - 1, index2].frameY = (short) 198;
            }
            if (num4 == 1)
            {
              Game1.tile[i - 1, index2].frameX = (short) 44;
              Game1.tile[i - 1, index2].frameY = (short) 220;
            }
            if (num4 == 2)
            {
              Game1.tile[i - 1, index2].frameX = (short) 44;
              Game1.tile[i - 1, index2].frameY = (short) 242;
            }
          }
          else
          {
            if (num4 == 0)
            {
              Game1.tile[i - 1, index2].frameX = (short) 66;
              Game1.tile[i - 1, index2].frameY = (short) 0;
            }
            if (num4 == 1)
            {
              Game1.tile[i - 1, index2].frameX = (short) 66;
              Game1.tile[i - 1, index2].frameY = (short) 22;
            }
            if (num4 == 2)
            {
              Game1.tile[i - 1, index2].frameX = (short) 66;
              Game1.tile[i - 1, index2].frameY = (short) 44;
            }
          }
        }
        if (num3 == 6 || num3 == 7)
        {
          Game1.tile[i + 1, index2].active = true;
          Game1.tile[i + 1, index2].type = (byte) 5;
          int num5 = WorldGen.genRand.Next(3);
          if (WorldGen.genRand.Next(3) < 2)
          {
            if (num5 == 0)
            {
              Game1.tile[i + 1, index2].frameX = (short) 66;
              Game1.tile[i + 1, index2].frameY = (short) 198;
            }
            if (num5 == 1)
            {
              Game1.tile[i + 1, index2].frameX = (short) 66;
              Game1.tile[i + 1, index2].frameY = (short) 220;
            }
            if (num5 == 2)
            {
              Game1.tile[i + 1, index2].frameX = (short) 66;
              Game1.tile[i + 1, index2].frameY = (short) 242;
            }
          }
          else
          {
            if (num5 == 0)
            {
              Game1.tile[i + 1, index2].frameX = (short) 88;
              Game1.tile[i + 1, index2].frameY = (short) 66;
            }
            if (num5 == 1)
            {
              Game1.tile[i + 1, index2].frameX = (short) 88;
              Game1.tile[i + 1, index2].frameY = (short) 88;
            }
            if (num5 == 2)
            {
              Game1.tile[i + 1, index2].frameX = (short) 88;
              Game1.tile[i + 1, index2].frameY = (short) 110;
            }
          }
        }
      }
      int num6 = WorldGen.genRand.Next(3);
      if (num6 == 0 || num6 == 1)
      {
        Game1.tile[i + 1, index1 - 1].active = true;
        Game1.tile[i + 1, index1 - 1].type = (byte) 5;
        int num7 = WorldGen.genRand.Next(3);
        if (num7 == 0)
        {
          Game1.tile[i + 1, index1 - 1].frameX = (short) 22;
          Game1.tile[i + 1, index1 - 1].frameY = (short) 132;
        }
        if (num7 == 1)
        {
          Game1.tile[i + 1, index1 - 1].frameX = (short) 22;
          Game1.tile[i + 1, index1 - 1].frameY = (short) 154;
        }
        if (num7 == 2)
        {
          Game1.tile[i + 1, index1 - 1].frameX = (short) 22;
          Game1.tile[i + 1, index1 - 1].frameY = (short) 176;
        }
      }
      if (num6 == 0 || num6 == 2)
      {
        Game1.tile[i - 1, index1 - 1].active = true;
        Game1.tile[i - 1, index1 - 1].type = (byte) 5;
        int num8 = WorldGen.genRand.Next(3);
        if (num8 == 0)
        {
          Game1.tile[i - 1, index1 - 1].frameX = (short) 44;
          Game1.tile[i - 1, index1 - 1].frameY = (short) 132;
        }
        if (num8 == 1)
        {
          Game1.tile[i - 1, index1 - 1].frameX = (short) 44;
          Game1.tile[i - 1, index1 - 1].frameY = (short) 154;
        }
        if (num8 == 2)
        {
          Game1.tile[i - 1, index1 - 1].frameX = (short) 44;
          Game1.tile[i - 1, index1 - 1].frameY = (short) 176;
        }
      }
      int num9 = WorldGen.genRand.Next(3);
      switch (num6)
      {
        case 0:
          if (num9 == 0)
          {
            Game1.tile[i, index1 - 1].frameX = (short) 88;
            Game1.tile[i, index1 - 1].frameY = (short) 132;
          }
          if (num9 == 1)
          {
            Game1.tile[i, index1 - 1].frameX = (short) 88;
            Game1.tile[i, index1 - 1].frameY = (short) 154;
          }
          if (num9 == 2)
          {
            Game1.tile[i, index1 - 1].frameX = (short) 88;
            Game1.tile[i, index1 - 1].frameY = (short) 176;
            break;
          }
          break;
        case 1:
          if (num9 == 0)
          {
            Game1.tile[i, index1 - 1].frameX = (short) 0;
            Game1.tile[i, index1 - 1].frameY = (short) 132;
          }
          if (num9 == 1)
          {
            Game1.tile[i, index1 - 1].frameX = (short) 0;
            Game1.tile[i, index1 - 1].frameY = (short) 154;
          }
          if (num9 == 2)
          {
            Game1.tile[i, index1 - 1].frameX = (short) 0;
            Game1.tile[i, index1 - 1].frameY = (short) 176;
            break;
          }
          break;
        case 2:
          if (num9 == 0)
          {
            Game1.tile[i, index1 - 1].frameX = (short) 66;
            Game1.tile[i, index1 - 1].frameY = (short) 132;
          }
          if (num9 == 1)
          {
            Game1.tile[i, index1 - 1].frameX = (short) 66;
            Game1.tile[i, index1 - 1].frameY = (short) 154;
          }
          if (num9 == 2)
          {
            Game1.tile[i, index1 - 1].frameX = (short) 66;
            Game1.tile[i, index1 - 1].frameY = (short) 176;
          }
          break;
      }
      if (WorldGen.genRand.Next(3) < 2)
      {
        int num10 = WorldGen.genRand.Next(3);
        if (num10 == 0)
        {
          Game1.tile[i, index1 - num1].frameX = (short) 22;
          Game1.tile[i, index1 - num1].frameY = (short) 198;
        }
        if (num10 == 1)
        {
          Game1.tile[i, index1 - num1].frameX = (short) 22;
          Game1.tile[i, index1 - num1].frameY = (short) 220;
        }
        if (num10 == 2)
        {
          Game1.tile[i, index1 - num1].frameX = (short) 22;
          Game1.tile[i, index1 - num1].frameY = (short) 242;
        }
      }
      else
      {
        int num11 = WorldGen.genRand.Next(3);
        if (num11 == 0)
        {
          Game1.tile[i, index1 - num1].frameX = (short) 0;
          Game1.tile[i, index1 - num1].frameY = (short) 198;
        }
        if (num11 == 1)
        {
          Game1.tile[i, index1 - num1].frameX = (short) 0;
          Game1.tile[i, index1 - num1].frameY = (short) 220;
        }
        if (num11 == 2)
        {
          Game1.tile[i, index1 - num1].frameX = (short) 0;
          Game1.tile[i, index1 - num1].frameY = (short) 242;
        }
      }
      WorldGen.RangeFrame(i - 2, index1 - num1 - 1, i + 2, index1 + 1);
      if (Game1.netMode == 2)
        NetMessage.SendTileSquare(-1, i, (int) ((double) index1 - (double) num1 * 0.5), num1 + 1);
    }

    public static void GrowShroom(int i, int y)
    {
      int index1 = y;
      if (Game1.tile[i - 1, index1 - 1].lava || Game1.tile[i - 1, index1 - 1].lava || Game1.tile[i + 1, index1 - 1].lava || !Game1.tile[i, index1].active || Game1.tile[i, index1].type != (byte) 70 || Game1.tile[i, index1 - 1].wall != (byte) 0 || !Game1.tile[i - 1, index1].active || Game1.tile[i - 1, index1].type != (byte) 70 || !Game1.tile[i + 1, index1].active || Game1.tile[i + 1, index1].type != (byte) 70 || !WorldGen.EmptyTileCheck(i - 2, i + 2, index1 - 13, index1 - 1, 71))
        return;
      int num1 = WorldGen.genRand.Next(4, 11);
      for (int index2 = index1 - num1; index2 < index1; ++index2)
      {
        Game1.tile[i, index2].frameNumber = (byte) WorldGen.genRand.Next(3);
        Game1.tile[i, index2].active = true;
        Game1.tile[i, index2].type = (byte) 72;
        int num2 = WorldGen.genRand.Next(3);
        if (num2 == 0)
        {
          Game1.tile[i, index2].frameX = (short) 0;
          Game1.tile[i, index2].frameY = (short) 0;
        }
        if (num2 == 1)
        {
          Game1.tile[i, index2].frameX = (short) 0;
          Game1.tile[i, index2].frameY = (short) 18;
        }
        if (num2 == 2)
        {
          Game1.tile[i, index2].frameX = (short) 0;
          Game1.tile[i, index2].frameY = (short) 36;
        }
      }
      int num3 = WorldGen.genRand.Next(3);
      if (num3 == 0)
      {
        Game1.tile[i, index1 - num1].frameX = (short) 36;
        Game1.tile[i, index1 - num1].frameY = (short) 0;
      }
      if (num3 == 1)
      {
        Game1.tile[i, index1 - num1].frameX = (short) 36;
        Game1.tile[i, index1 - num1].frameY = (short) 18;
      }
      if (num3 == 2)
      {
        Game1.tile[i, index1 - num1].frameX = (short) 36;
        Game1.tile[i, index1 - num1].frameY = (short) 36;
      }
      WorldGen.RangeFrame(i - 2, index1 - num1 - 1, i + 2, index1 + 1);
      if (Game1.netMode == 2)
        NetMessage.SendTileSquare(-1, i, (int) ((double) index1 - (double) num1 * 0.5), num1 + 1);
    }

    public static void AddTrees()
    {
      for (int i = 1; i < Game1.maxTilesX - 1; ++i)
      {
        for (int y = 20; (double) y < Game1.worldSurface; ++y)
          WorldGen.GrowTree(i, y);
      }
    }

    public static bool EmptyTileCheck(
      int startX,
      int endX,
      int startY,
      int endY,
      int ignoreStyle = -1)
    {
      if (startX < 0 || endX >= Game1.maxTilesX || startY < 0 || endY >= Game1.maxTilesY)
        return false;
      for (int index1 = startX; index1 < endX + 1; ++index1)
      {
        for (int index2 = startY; index2 < endY + 1; ++index2)
        {
          if (Game1.tile[index1, index2].active)
          {
            int num;
            switch (ignoreStyle)
            {
              case -1:
                return false;
              case 11:
                num = Game1.tile[index1, index2].type == (byte) 11 ? 1 : 0;
                break;
              default:
                num = 1;
                break;
            }
            if (num == 0 || ignoreStyle == 20 && Game1.tile[index1, index2].type != (byte) 20 && Game1.tile[index1, index2].type != (byte) 3 || ignoreStyle == 71 && Game1.tile[index1, index2].type != (byte) 71)
              return false;
          }
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
        Game1.tile[i, j - 1].frameX = (short) (WorldGen.genRand.Next(3) * 18);
        Game1.tile[i, j].active = true;
        Game1.tile[i, j].type = (byte) 10;
        Game1.tile[i, j].frameY = (short) 18;
        Game1.tile[i, j].frameX = (short) (WorldGen.genRand.Next(3) * 18);
        Game1.tile[i, j + 1].active = true;
        Game1.tile[i, j + 1].type = (byte) 10;
        Game1.tile[i, j + 1].frameY = (short) 36;
        Game1.tile[i, j + 1].frameX = (short) (WorldGen.genRand.Next(3) * 18);
        return true;
      }
      catch
      {
        return false;
      }
    }

    public static bool CloseDoor(int i, int j, bool forced = false)
    {
      int num1 = 0;
      int i1 = i;
      int num2 = j;
      if (Game1.tile[i, j] == null)
        Game1.tile[i, j] = new Tile();
      int frameX = (int) Game1.tile[i, j].frameX;
      int frameY = (int) Game1.tile[i, j].frameY;
      switch (frameX)
      {
        case 0:
          i1 = i;
          num1 = 1;
          break;
        case 18:
          i1 = i - 1;
          num1 = 1;
          break;
        case 36:
          i1 = i + 1;
          num1 = -1;
          break;
        case 54:
          i1 = i;
          num1 = -1;
          break;
      }
      switch (frameY)
      {
        case 0:
          num2 = j;
          break;
        case 18:
          num2 = j - 1;
          break;
        case 36:
          num2 = j - 2;
          break;
      }
      int num3 = i1;
      if (num1 == -1)
        num3 = i1 - 1;
      if (!forced)
      {
        for (int j1 = num2; j1 < num2 + 3; ++j1)
        {
          if (!Collision.EmptyTile(i1, j1, true))
            return false;
        }
      }
      for (int index1 = num3; index1 < num3 + 2; ++index1)
      {
        for (int index2 = num2; index2 < num2 + 3; ++index2)
        {
          if (index1 == i1)
          {
            if (Game1.tile[index1, index2] == null)
              Game1.tile[index1, index2] = new Tile();
            Game1.tile[index1, index2].type = (byte) 10;
            Game1.tile[index1, index2].frameX = (short) (WorldGen.genRand.Next(3) * 18);
          }
          else
          {
            if (Game1.tile[index1, index2] == null)
              Game1.tile[index1, index2] = new Tile();
            Game1.tile[index1, index2].active = false;
          }
        }
      }
      for (int i2 = i1 - 1; i2 <= i1 + 1; ++i2)
      {
        for (int j2 = num2 - 1; j2 <= num2 + 2; ++j2)
          WorldGen.TileFrame(i2, j2);
      }
      Game1.PlaySound(9, i * 16, j * 16);
      return true;
    }

    public static bool AddLifeCrystal(int i, int j)
    {
      for (int index = j; index < Game1.maxTilesY; ++index)
      {
        if (Game1.tile[i, index].active && Game1.tileSolid[(int) Game1.tile[i, index].type])
        {
          int endX = i;
          int endY = index - 1;
          if (Game1.tile[endX, endY - 1].lava || Game1.tile[endX - 1, endY - 1].lava || !WorldGen.EmptyTileCheck(endX - 1, endX, endY - 1, endY))
            return false;
          Game1.tile[endX - 1, endY - 1].active = true;
          Game1.tile[endX - 1, endY - 1].type = (byte) 12;
          Game1.tile[endX - 1, endY - 1].frameX = (short) 0;
          Game1.tile[endX - 1, endY - 1].frameY = (short) 0;
          Game1.tile[endX, endY - 1].active = true;
          Game1.tile[endX, endY - 1].type = (byte) 12;
          Game1.tile[endX, endY - 1].frameX = (short) 18;
          Game1.tile[endX, endY - 1].frameY = (short) 0;
          Game1.tile[endX - 1, endY].active = true;
          Game1.tile[endX - 1, endY].type = (byte) 12;
          Game1.tile[endX - 1, endY].frameX = (short) 0;
          Game1.tile[endX - 1, endY].frameY = (short) 18;
          Game1.tile[endX, endY].active = true;
          Game1.tile[endX, endY].type = (byte) 12;
          Game1.tile[endX, endY].frameX = (short) 18;
          Game1.tile[endX, endY].frameY = (short) 18;
          return true;
        }
      }
      return false;
    }

    public static void AddShadowOrb(int x, int y)
    {
      if (x < 10 || x > Game1.maxTilesX - 10 || y < 10 || y > Game1.maxTilesY - 10)
        return;
      Game1.tile[x - 1, y - 1].active = true;
      Game1.tile[x - 1, y - 1].type = (byte) 31;
      Game1.tile[x - 1, y - 1].frameX = (short) 0;
      Game1.tile[x - 1, y - 1].frameY = (short) 0;
      Game1.tile[x, y - 1].active = true;
      Game1.tile[x, y - 1].type = (byte) 31;
      Game1.tile[x, y - 1].frameX = (short) 18;
      Game1.tile[x, y - 1].frameY = (short) 0;
      Game1.tile[x - 1, y].active = true;
      Game1.tile[x - 1, y].type = (byte) 31;
      Game1.tile[x - 1, y].frameX = (short) 0;
      Game1.tile[x - 1, y].frameY = (short) 18;
      Game1.tile[x, y].active = true;
      Game1.tile[x, y].type = (byte) 31;
      Game1.tile[x, y].frameX = (short) 18;
      Game1.tile[x, y].frameY = (short) 18;
    }

    public static void MakeDungeon(int x, int y, int tileType = 41, int wallType = 7)
    {
      int num1 = WorldGen.genRand.Next(3);
      int num2 = WorldGen.genRand.Next(3);
      switch (num1)
      {
        case 1:
          tileType = 43;
          break;
        case 2:
          tileType = 44;
          break;
      }
      switch (num2)
      {
        case 1:
          wallType = 8;
          break;
        case 2:
          wallType = 9;
          break;
      }
      WorldGen.numDDoors = 0;
      WorldGen.numDPlats = 0;
      WorldGen.numDRooms = 0;
      WorldGen.dungeonX = x;
      WorldGen.dungeonY = y;
      WorldGen.dMinX = x;
      WorldGen.dMaxX = x;
      WorldGen.dMinY = y;
      WorldGen.dMaxY = y;
      WorldGen.dxStrength1 = (double) WorldGen.genRand.Next(25, 30);
      WorldGen.dyStrength1 = (double) WorldGen.genRand.Next(20, 25);
      WorldGen.dxStrength2 = (double) WorldGen.genRand.Next(35, 50);
      WorldGen.dyStrength2 = (double) WorldGen.genRand.Next(10, 15);
      float num3 = (float) (Game1.maxTilesX / 60);
      float num4 = num3 + (float) WorldGen.genRand.Next(0, (int) ((double) num3 / 3.0));
      float num5 = num4;
      int num6 = 5;
      WorldGen.DungeonRoom(WorldGen.dungeonX, WorldGen.dungeonY, tileType, wallType);
      while ((double) num4 > 0.0)
      {
        if (WorldGen.dungeonX < WorldGen.dMinX)
          WorldGen.dMinX = WorldGen.dungeonX;
        if (WorldGen.dungeonX > WorldGen.dMaxX)
          WorldGen.dMaxX = WorldGen.dungeonX;
        if (WorldGen.dungeonY > WorldGen.dMaxY)
          WorldGen.dMaxY = WorldGen.dungeonY;
        --num4;
        Game1.statusText = "Creating dungeon: " + (object) (int) (((double) num5 - (double) num4) / (double) num5 * 60.0) + "%";
        if (num6 > 0)
          --num6;
        if (num6 == 0 & WorldGen.genRand.Next(3) == 0)
        {
          num6 = 5;
          if (WorldGen.genRand.Next(2) == 0)
          {
            int dungeonX = WorldGen.dungeonX;
            int dungeonY = WorldGen.dungeonY;
            WorldGen.DungeonHalls(WorldGen.dungeonX, WorldGen.dungeonY, tileType, wallType);
            if (WorldGen.genRand.Next(2) == 0)
              WorldGen.DungeonHalls(WorldGen.dungeonX, WorldGen.dungeonY, tileType, wallType);
            WorldGen.DungeonRoom(WorldGen.dungeonX, WorldGen.dungeonY, tileType, wallType);
            WorldGen.dungeonX = dungeonX;
            WorldGen.dungeonY = dungeonY;
          }
          else
            WorldGen.DungeonRoom(WorldGen.dungeonX, WorldGen.dungeonY, tileType, wallType);
        }
        else
          WorldGen.DungeonHalls(WorldGen.dungeonX, WorldGen.dungeonY, tileType, wallType);
      }
      WorldGen.DungeonRoom(WorldGen.dungeonX, WorldGen.dungeonY, tileType, wallType);
      int num7 = WorldGen.dRoomX[0];
      int num8 = WorldGen.dRoomY[0];
      for (int index = 0; index < WorldGen.numDRooms; ++index)
      {
        if (WorldGen.dRoomY[index] < num8)
        {
          num7 = WorldGen.dRoomX[index];
          num8 = WorldGen.dRoomY[index];
        }
      }
      WorldGen.dungeonX = num7;
      WorldGen.dungeonY = num8;
      WorldGen.dEnteranceX = num7;
      WorldGen.dSurface = false;
      int num9 = 5;
      while (!WorldGen.dSurface)
      {
        if (num9 > 0)
          --num9;
        if (num9 == 0 & WorldGen.genRand.Next(5) == 0 && (double) WorldGen.dungeonY > Game1.worldSurface + 50.0)
        {
          num9 = 10;
          int dungeonX = WorldGen.dungeonX;
          int dungeonY = WorldGen.dungeonY;
          WorldGen.DungeonHalls(WorldGen.dungeonX, WorldGen.dungeonY, tileType, wallType, true);
          WorldGen.DungeonRoom(WorldGen.dungeonX, WorldGen.dungeonY, tileType, wallType);
          WorldGen.dungeonX = dungeonX;
          WorldGen.dungeonY = dungeonY;
        }
        WorldGen.DungeonStairs(WorldGen.dungeonX, WorldGen.dungeonY, tileType, wallType);
      }
      WorldGen.DungeonEnt(WorldGen.dungeonX, WorldGen.dungeonY, tileType, wallType);
      Game1.statusText = "Creating dungeon: 65%";
      for (int index1 = 0; index1 < WorldGen.numDRooms; ++index1)
      {
        for (int index2 = WorldGen.dRoomL[index1]; index2 <= WorldGen.dRoomR[index1]; ++index2)
        {
          if (!Game1.tile[index2, WorldGen.dRoomT[index1] - 1].active)
          {
            WorldGen.DPlatX[WorldGen.numDPlats] = index2;
            WorldGen.DPlatY[WorldGen.numDPlats] = WorldGen.dRoomT[index1] - 1;
            ++WorldGen.numDPlats;
            break;
          }
        }
        for (int index3 = WorldGen.dRoomL[index1]; index3 <= WorldGen.dRoomR[index1]; ++index3)
        {
          if (!Game1.tile[index3, WorldGen.dRoomB[index1] + 1].active)
          {
            WorldGen.DPlatX[WorldGen.numDPlats] = index3;
            WorldGen.DPlatY[WorldGen.numDPlats] = WorldGen.dRoomB[index1] + 1;
            ++WorldGen.numDPlats;
            break;
          }
        }
        for (int index4 = WorldGen.dRoomT[index1]; index4 <= WorldGen.dRoomB[index1]; ++index4)
        {
          if (!Game1.tile[WorldGen.dRoomL[index1] - 1, index4].active)
          {
            WorldGen.DDoorX[WorldGen.numDDoors] = WorldGen.dRoomL[index1] - 1;
            WorldGen.DDoorY[WorldGen.numDDoors] = index4;
            WorldGen.DDoorPos[WorldGen.numDDoors] = -1;
            ++WorldGen.numDDoors;
            break;
          }
        }
        for (int index5 = WorldGen.dRoomT[index1]; index5 <= WorldGen.dRoomB[index1]; ++index5)
        {
          if (!Game1.tile[WorldGen.dRoomR[index1] + 1, index5].active)
          {
            WorldGen.DDoorX[WorldGen.numDDoors] = WorldGen.dRoomR[index1] + 1;
            WorldGen.DDoorY[WorldGen.numDDoors] = index5;
            WorldGen.DDoorPos[WorldGen.numDDoors] = 1;
            ++WorldGen.numDDoors;
            break;
          }
        }
      }
      Game1.statusText = "Creating dungeon: 70%";
      int num10 = 0;
      int num11 = 1000;
      int num12 = 0;
      while (num12 < Game1.maxTilesX / 125)
      {
        ++num10;
        int index6 = WorldGen.genRand.Next(WorldGen.dMinX, WorldGen.dMaxX);
        int index7 = WorldGen.genRand.Next((int) Game1.worldSurface + 25, WorldGen.dMaxY);
        int num13 = index6;
        if ((int) Game1.tile[index6, index7].wall == wallType && !Game1.tile[index6, index7].active)
        {
          int num14 = 1;
          if (WorldGen.genRand.Next(2) == 0)
            num14 = -1;
          while (!Game1.tile[index6, index7].active)
            index7 += num14;
          if (Game1.tile[index6 - 1, index7].active && Game1.tile[index6 + 1, index7].active && !Game1.tile[index6 - 1, index7 - num14].active && !Game1.tile[index6 + 1, index7 - num14].active)
          {
            ++num12;
            for (int index8 = WorldGen.genRand.Next(5, 10); Game1.tile[index6 - 1, index7].active && Game1.tile[index6, index7 + num14].active && Game1.tile[index6, index7].active && !Game1.tile[index6, index7 - num14].active && index8 > 0; --index8)
            {
              Game1.tile[index6, index7].type = (byte) 48;
              if (!Game1.tile[index6 - 1, index7 - num14].active && !Game1.tile[index6 + 1, index7 - num14].active)
              {
                Game1.tile[index6, index7 - num14].type = (byte) 48;
                Game1.tile[index6, index7 - num14].active = true;
              }
              --index6;
            }
            int num15 = WorldGen.genRand.Next(5, 10);
            for (int index9 = num13 + 1; Game1.tile[index9 + 1, index7].active && Game1.tile[index9, index7 + num14].active && Game1.tile[index9, index7].active && !Game1.tile[index9, index7 - num14].active && num15 > 0; --num15)
            {
              Game1.tile[index9, index7].type = (byte) 48;
              if (!Game1.tile[index9 - 1, index7 - num14].active && !Game1.tile[index9 + 1, index7 - num14].active)
              {
                Game1.tile[index9, index7 - num14].type = (byte) 48;
                Game1.tile[index9, index7 - num14].active = true;
              }
              ++index9;
            }
          }
        }
        if (num10 > num11)
        {
          num10 = 0;
          ++num12;
        }
      }
      int num16 = 0;
      int num17 = 1000;
      int num18 = 0;
      Game1.statusText = "Creating dungeon: 75%";
      while (num18 < Game1.maxTilesX / 125)
      {
        ++num16;
        int index10 = WorldGen.genRand.Next(WorldGen.dMinX, WorldGen.dMaxX);
        int index11 = WorldGen.genRand.Next((int) Game1.worldSurface + 25, WorldGen.dMaxY);
        int num19 = index11;
        if ((int) Game1.tile[index10, index11].wall == wallType && !Game1.tile[index10, index11].active)
        {
          int num20 = 1;
          if (WorldGen.genRand.Next(2) == 0)
            num20 = -1;
          while (index10 > 5 && index10 < Game1.maxTilesX - 5 && !Game1.tile[index10, index11].active)
            index10 += num20;
          if (Game1.tile[index10, index11 - 1].active && Game1.tile[index10, index11 + 1].active && !Game1.tile[index10 - num20, index11 - 1].active && !Game1.tile[index10 - num20, index11 + 1].active)
          {
            ++num18;
            for (int index12 = WorldGen.genRand.Next(5, 10); Game1.tile[index10, index11 - 1].active && Game1.tile[index10 + num20, index11].active && Game1.tile[index10, index11].active && !Game1.tile[index10 - num20, index11].active && index12 > 0; --index12)
            {
              Game1.tile[index10, index11].type = (byte) 48;
              if (!Game1.tile[index10 - num20, index11 - 1].active && !Game1.tile[index10 - num20, index11 + 1].active)
              {
                Game1.tile[index10 - num20, index11].type = (byte) 48;
                Game1.tile[index10 - num20, index11].active = true;
              }
              --index11;
            }
            int num21 = WorldGen.genRand.Next(5, 10);
            for (int index13 = num19 + 1; Game1.tile[index10, index13 + 1].active && Game1.tile[index10 + num20, index13].active && Game1.tile[index10, index13].active && !Game1.tile[index10 - num20, index13].active && num21 > 0; --num21)
            {
              Game1.tile[index10, index13].type = (byte) 48;
              if (!Game1.tile[index10 - num20, index13 - 1].active && !Game1.tile[index10 - num20, index13 + 1].active)
              {
                Game1.tile[index10 - num20, index13].type = (byte) 48;
                Game1.tile[index10 - num20, index13].active = true;
              }
              ++index13;
            }
          }
        }
        if (num16 > num17)
        {
          num16 = 0;
          ++num18;
        }
      }
      Game1.statusText = "Creating dungeon: 80%";
      for (int index14 = 0; index14 < WorldGen.numDDoors; ++index14)
      {
        int num22 = WorldGen.DDoorX[index14] - 10;
        int num23 = WorldGen.DDoorX[index14] + 10;
        int num24 = 100;
        int num25 = 0;
        for (int index15 = num22; index15 < num23; ++index15)
        {
          bool flag1 = true;
          int index16 = WorldGen.DDoorY[index14];
          while (!Game1.tile[index15, index16].active)
            --index16;
          if (!Game1.tileDungeon[(int) Game1.tile[index15, index16].type])
            flag1 = false;
          int num26 = index16;
          int index17 = WorldGen.DDoorY[index14];
          while (!Game1.tile[index15, index17].active)
            ++index17;
          if (!Game1.tileDungeon[(int) Game1.tile[index15, index17].type])
            flag1 = false;
          int num27 = index17;
          if (num27 - num26 >= 3)
          {
            int num28 = index15 - 20;
            int num29 = index15 + 20;
            int num30 = num27 - 10;
            int num31 = num27 + 10;
            for (int index18 = num28; index18 < num29; ++index18)
            {
              for (int index19 = num30; index19 < num31; ++index19)
              {
                if (Game1.tile[index18, index19].active && Game1.tile[index18, index19].type == (byte) 10)
                {
                  flag1 = false;
                  break;
                }
              }
            }
            if (flag1)
            {
              for (int index20 = num27 - 3; index20 < num27; ++index20)
              {
                for (int index21 = index15 - 3; index21 <= index15 + 3; ++index21)
                {
                  if (Game1.tile[index21, index20].active)
                  {
                    flag1 = false;
                    break;
                  }
                }
              }
            }
            if (flag1 && num27 - num26 < 20)
            {
              bool flag2 = false;
              if (WorldGen.DDoorPos[index14] == 0 && num27 - num26 < num24)
                flag2 = true;
              if (WorldGen.DDoorPos[index14] == -1 && index15 > num25)
                flag2 = true;
              if (WorldGen.DDoorPos[index14] == 1 && (index15 < num25 || num25 == 0))
                flag2 = true;
              if (flag2)
              {
                num25 = index15;
                num24 = num27 - num26;
              }
            }
          }
        }
        if (num24 < 20)
        {
          int i = num25;
          int index22 = WorldGen.DDoorY[index14];
          int index23 = index22;
          for (; !Game1.tile[i, index22].active; ++index22)
            Game1.tile[i, index22].active = false;
          while (!Game1.tile[i, index23].active)
            --index23;
          int j = index22 - 1;
          int num32 = index23 + 1;
          for (int index24 = num32; index24 < j - 2; ++index24)
          {
            Game1.tile[i, index24].active = true;
            Game1.tile[i, index24].type = (byte) tileType;
          }
          WorldGen.PlaceTile(i, j, 10, true);
          int index25 = i - 1;
          int index26 = j - 3;
          while (!Game1.tile[index25, index26].active)
            --index26;
          int num33;
          if (j - index26 < j - num32 + 5 && Game1.tileDungeon[(int) Game1.tile[index25, index26].type])
          {
            num33 = index26;
            for (int index27 = j - 4 - WorldGen.genRand.Next(3); index27 > index26; --index27)
            {
              Game1.tile[index25, index27].active = true;
              Game1.tile[index25, index27].type = (byte) tileType;
            }
          }
          int index28 = index25 + 2;
          int index29 = j - 3;
          while (!Game1.tile[index28, index29].active)
            --index29;
          if (j - index29 < j - num32 + 5 && Game1.tileDungeon[(int) Game1.tile[index28, index29].type])
          {
            num33 = index29;
            for (int index30 = j - 4 - WorldGen.genRand.Next(3); index30 > index29; --index30)
            {
              Game1.tile[index28, index30].active = true;
              Game1.tile[index28, index30].type = (byte) tileType;
            }
          }
          int index31 = j + 1;
          int num34 = index28 - 1;
          Game1.tile[num34 - 1, index31].active = true;
          Game1.tile[num34 - 1, index31].type = (byte) tileType;
          Game1.tile[num34 + 1, index31].active = true;
          Game1.tile[num34 + 1, index31].type = (byte) tileType;
        }
      }
      Game1.statusText = "Creating dungeon: 85%";
      for (int index32 = 0; index32 < WorldGen.numDPlats; ++index32)
      {
        int index33 = WorldGen.DPlatX[index32];
        int num35 = WorldGen.DPlatY[index32];
        int num36 = Game1.maxTilesX;
        int num37 = 10;
        for (int index34 = num35 - 5; index34 <= num35 + 5; ++index34)
        {
          int index35 = index33;
          int index36 = index33;
          bool flag3 = false;
          if (Game1.tile[index35, index34].active)
          {
            flag3 = true;
          }
          else
          {
            while (!Game1.tile[index35, index34].active)
            {
              --index35;
              if (!Game1.tileDungeon[(int) Game1.tile[index35, index34].type])
                flag3 = true;
            }
            while (!Game1.tile[index36, index34].active)
            {
              ++index36;
              if (!Game1.tileDungeon[(int) Game1.tile[index36, index34].type])
                flag3 = true;
            }
          }
          if (!flag3 && index36 - index35 <= num37)
          {
            bool flag4 = true;
            int num38 = index33 - num37 / 2 - 2;
            int num39 = index33 + num37 / 2 + 2;
            int num40 = index34 - 5;
            int num41 = index34 + 5;
            for (int index37 = num38; index37 <= num39; ++index37)
            {
              for (int index38 = num40; index38 <= num41; ++index38)
              {
                if (Game1.tile[index37, index38].active && Game1.tile[index37, index38].type == (byte) 19)
                {
                  flag4 = false;
                  break;
                }
              }
            }
            for (int index39 = index34 + 3; index39 >= index34 - 5; --index39)
            {
              if (Game1.tile[index33, index39].active)
              {
                flag4 = false;
                break;
              }
            }
            if (flag4)
            {
              num36 = index34;
              break;
            }
          }
        }
        if (num36 > num35 - 10 && num36 < num35 + 10)
        {
          int index40 = index33;
          int index41 = num36;
          int index42 = index33 + 1;
          for (; !Game1.tile[index40, index41].active; --index40)
          {
            Game1.tile[index40, index41].active = true;
            Game1.tile[index40, index41].type = (byte) 19;
          }
          for (; !Game1.tile[index42, index41].active; ++index42)
          {
            Game1.tile[index42, index41].active = true;
            Game1.tile[index42, index41].type = (byte) 19;
          }
        }
      }
      Game1.statusText = "Creating dungeon: 90%";
      int num42 = 0;
      int num43 = 1000;
      int num44 = 0;
      while (num44 < Game1.maxTilesX / 20)
      {
        ++num42;
        int index43 = WorldGen.genRand.Next(WorldGen.dMinX, WorldGen.dMaxX);
        int index44 = WorldGen.genRand.Next(WorldGen.dMinY, WorldGen.dMaxY);
        bool flag5 = true;
        if ((int) Game1.tile[index43, index44].wall == wallType && !Game1.tile[index43, index44].active)
        {
          int num45 = 1;
          if (WorldGen.genRand.Next(2) == 0)
            num45 = -1;
          while (flag5 && !Game1.tile[index43, index44].active)
          {
            index43 -= num45;
            if (index43 < 5 || index43 > Game1.maxTilesX - 5)
              flag5 = false;
            else if (Game1.tile[index43, index44].active && !Game1.tileDungeon[(int) Game1.tile[index43, index44].type])
              flag5 = false;
          }
          if (flag5 && Game1.tile[index43, index44].active && Game1.tileDungeon[(int) Game1.tile[index43, index44].type] && Game1.tile[index43, index44 - 1].active && Game1.tileDungeon[(int) Game1.tile[index43, index44 - 1].type] && Game1.tile[index43, index44 + 1].active && Game1.tileDungeon[(int) Game1.tile[index43, index44 + 1].type])
          {
            int i1 = index43 + num45;
            for (int index45 = i1 - 3; index45 <= i1 + 3; ++index45)
            {
              for (int index46 = index44 - 3; index46 <= index44 + 3; ++index46)
              {
                if (Game1.tile[index45, index46].active && Game1.tile[index45, index46].type == (byte) 19)
                {
                  flag5 = false;
                  break;
                }
              }
            }
            if (flag5 && !Game1.tile[i1, index44 - 1].active & !Game1.tile[i1, index44 - 2].active & !Game1.tile[i1, index44 - 3].active)
            {
              int index47 = i1;
              int num46 = i1;
              while (index47 > WorldGen.dMinX && index47 < WorldGen.dMaxX && !Game1.tile[index47, index44].active && !Game1.tile[index47, index44 - 1].active && !Game1.tile[index47, index44 + 1].active)
                index47 += num45;
              int num47 = Math.Abs(i1 - index47);
              bool flag6 = false;
              if (WorldGen.genRand.Next(2) == 0)
                flag6 = true;
              if (num47 > 5)
              {
                for (int index48 = WorldGen.genRand.Next(1, 4); index48 > 0; --index48)
                {
                  Game1.tile[i1, index44].active = true;
                  Game1.tile[i1, index44].type = (byte) 19;
                  if (flag6)
                  {
                    WorldGen.PlaceTile(i1, index44 - 1, 50, true);
                    if (WorldGen.genRand.Next(50) == 0 && Game1.tile[i1, index44 - 1].type == (byte) 50)
                      Game1.tile[i1, index44 - 1].frameX = (short) 90;
                  }
                  i1 += num45;
                }
                num42 = 0;
                ++num44;
                if (!flag6 && WorldGen.genRand.Next(2) == 0)
                {
                  int i2 = num46;
                  int j = index44 - 1;
                  int type = WorldGen.genRand.Next(2);
                  switch (type)
                  {
                    case 0:
                      type = 13;
                      break;
                    case 1:
                      type = 49;
                      break;
                  }
                  WorldGen.PlaceTile(i2, j, type, true);
                  if (Game1.tile[i2, j].type == (byte) 13)
                    Game1.tile[i2, j].frameX = WorldGen.genRand.Next(2) != 0 ? (short) 36 : (short) 18;
                }
              }
            }
          }
        }
        if (num42 > num43)
        {
          num42 = 0;
          ++num44;
        }
      }
      Game1.statusText = "Creating dungeon: 95%";
      for (int index = 0; index < WorldGen.numDRooms; ++index)
      {
        int num48 = 0;
        while (num48 < 1000)
        {
          int num49 = (int) ((double) WorldGen.dRoomSize[index] * 0.4);
          int i = WorldGen.dRoomX[index] + WorldGen.genRand.Next(-num49, num49 + 1);
          int j = WorldGen.dRoomY[index] + WorldGen.genRand.Next(-num49, num49 + 1);
          int contain = 0;
          switch (index)
          {
            case 0:
              contain = 113;
              break;
            case 1:
              contain = 155;
              break;
            case 2:
              contain = 156;
              break;
            case 3:
              contain = 157;
              break;
            case 4:
              contain = 163;
              break;
            case 5:
              contain = 164;
              break;
          }
          if (contain == 0 && WorldGen.genRand.Next(2) == 0)
          {
            num48 = 1000;
          }
          else
          {
            if (WorldGen.AddBuriedChest(i, j, contain))
              num48 += 1000;
            ++num48;
          }
        }
      }
      WorldGen.dMinX -= 25;
      WorldGen.dMaxX += 25;
      WorldGen.dMinY -= 25;
      WorldGen.dMaxY += 25;
      if (WorldGen.dMinX < 0)
        WorldGen.dMinX = 0;
      if (WorldGen.dMaxX > Game1.maxTilesX)
        WorldGen.dMaxX = Game1.maxTilesX;
      if (WorldGen.dMinY < 0)
        WorldGen.dMinY = 0;
      if (WorldGen.dMaxY > Game1.maxTilesY)
        WorldGen.dMaxY = Game1.maxTilesY;
      int num50 = 0;
      int num51 = 1000;
      int num52 = 0;
      while (num52 < Game1.maxTilesX / 20)
      {
        ++num50;
        int x1 = WorldGen.genRand.Next(WorldGen.dMinX, WorldGen.dMaxX);
        int index49 = WorldGen.genRand.Next(WorldGen.dMinY, WorldGen.dMaxY);
        if ((int) Game1.tile[x1, index49].wall == wallType)
        {
          for (int y1 = index49; y1 > WorldGen.dMinY; --y1)
          {
            if (Game1.tile[x1, y1 - 1].active && (int) Game1.tile[x1, y1 - 1].type == tileType)
            {
              bool flag = false;
              for (int index50 = x1 - 15; index50 < x1 + 15; ++index50)
              {
                for (int index51 = y1 - 15; index51 < y1 + 15; ++index51)
                {
                  if (index50 > 0 && index50 < Game1.maxTilesX && index51 > 0 && index51 < Game1.maxTilesY && Game1.tile[index50, index51].type == (byte) 42)
                  {
                    flag = true;
                    break;
                  }
                }
              }
              if (Game1.tile[x1 - 1, y1].active || Game1.tile[x1 + 1, y1].active || Game1.tile[x1 - 1, y1 + 1].active || Game1.tile[x1 + 1, y1 + 1].active || Game1.tile[x1, y1 + 2].active)
                flag = true;
              if (!flag)
              {
                WorldGen.Place1x2Top(x1, y1, 42);
                if (Game1.tile[x1, y1].type == (byte) 42)
                {
                  num50 = 0;
                  ++num52;
                }
                break;
              }
              break;
            }
          }
        }
        if (num50 > num51)
        {
          ++num52;
          num50 = 0;
        }
      }
    }

    public static void DungeonStairs(int i, int j, int tileType, int wallType)
    {
      Vector2 vector2_1 = new Vector2();
      double maxValue = (double) WorldGen.genRand.Next(5, 9);
      Vector2 vector2_2;
      vector2_2.X = (float) i;
      vector2_2.Y = (float) j;
      int num1 = WorldGen.genRand.Next(10, 30);
      int num2 = i <= WorldGen.dEnteranceX ? 1 : -1;
      vector2_1.Y = -1f;
      vector2_1.X = (float) num2;
      if (WorldGen.genRand.Next(3) == 0)
        vector2_1.X *= 0.5f;
      else if (WorldGen.genRand.Next(3) == 0)
        vector2_1.Y *= 2f;
      while (num1 > 0)
      {
        --num1;
        int num3 = (int) ((double) vector2_2.X - maxValue - 4.0 - (double) WorldGen.genRand.Next(6));
        int num4 = (int) ((double) vector2_2.X + maxValue + 4.0 + (double) WorldGen.genRand.Next(6));
        int num5 = (int) ((double) vector2_2.Y - maxValue - 4.0);
        int num6 = (int) ((double) vector2_2.Y + maxValue + 4.0 + (double) WorldGen.genRand.Next(6));
        if (num3 < 0)
          num3 = 0;
        if (num4 > Game1.maxTilesX)
          num4 = Game1.maxTilesX;
        if (num5 < 0)
          num5 = 0;
        if (num6 > Game1.maxTilesY)
          num6 = Game1.maxTilesY;
        int num7 = 1;
        if ((double) vector2_2.X > (double) (Game1.maxTilesX / 2))
          num7 = -1;
        int i1 = (int) ((double) vector2_2.X + WorldGen.dxStrength1 * 0.60000002384185791 * (double) num7 + WorldGen.dxStrength2 * (double) num7);
        int num8 = (int) (WorldGen.dyStrength2 * 0.5);
        if ((double) vector2_2.Y < Game1.worldSurface - 5.0 && Game1.tile[i1, (int) ((double) vector2_2.Y - maxValue - 6.0 + (double) num8)].wall == (byte) 0 && Game1.tile[i1, (int) ((double) vector2_2.Y - maxValue - 7.0 + (double) num8)].wall == (byte) 0 && Game1.tile[i1, (int) ((double) vector2_2.Y - maxValue - 8.0 + (double) num8)].wall == (byte) 0)
        {
          WorldGen.dSurface = true;
          WorldGen.TileRunner(i1, (int) ((double) vector2_2.Y - maxValue - 6.0 + (double) num8), (double) WorldGen.genRand.Next(25, 35), WorldGen.genRand.Next(10, 20), -1, speedY: -1f);
        }
        for (int index1 = num3; index1 < num4; ++index1)
        {
          for (int index2 = num5; index2 < num6; ++index2)
          {
            Game1.tile[index1, index2].liquid = (byte) 0;
            if ((int) Game1.tile[index1, index2].wall != wallType)
            {
              Game1.tile[index1, index2].wall = (byte) 0;
              Game1.tile[index1, index2].active = true;
              Game1.tile[index1, index2].type = (byte) tileType;
            }
          }
        }
        for (int i2 = num3 + 1; i2 < num4 - 1; ++i2)
        {
          for (int j1 = num5 + 1; j1 < num6 - 1; ++j1)
            WorldGen.PlaceWall(i2, j1, wallType, true);
        }
        int num9 = 0;
        if (WorldGen.genRand.Next((int) maxValue) == 0)
          num9 = WorldGen.genRand.Next(1, 3);
        int num10 = (int) ((double) vector2_2.X - maxValue * 0.5 - (double) num9);
        int num11 = (int) ((double) vector2_2.X + maxValue * 0.5 + (double) num9);
        int num12 = (int) ((double) vector2_2.Y - maxValue * 0.5 - (double) num9);
        int num13 = (int) ((double) vector2_2.Y + maxValue * 0.5 + (double) num9);
        if (num10 < 0)
          num10 = 0;
        if (num11 > Game1.maxTilesX)
          num11 = Game1.maxTilesX;
        if (num12 < 0)
          num12 = 0;
        if (num13 > Game1.maxTilesY)
          num13 = Game1.maxTilesY;
        for (int i3 = num10; i3 < num11; ++i3)
        {
          for (int j2 = num12; j2 < num13; ++j2)
          {
            Game1.tile[i3, j2].active = false;
            WorldGen.PlaceWall(i3, j2, wallType, true);
          }
        }
        if (WorldGen.dSurface)
          num1 = 0;
        vector2_2 += vector2_1;
      }
      WorldGen.dungeonX = (int) vector2_2.X;
      WorldGen.dungeonY = (int) vector2_2.Y;
    }

    public static void DungeonHalls(int i, int j, int tileType, int wallType, bool forceX = false)
    {
      Vector2 vector2_1 = new Vector2();
      double num1 = (double) WorldGen.genRand.Next(4, 6);
      Vector2 vector2_2 = new Vector2();
      Vector2 vector2_3 = new Vector2();
      Vector2 vector2_4;
      vector2_4.X = (float) i;
      vector2_4.Y = (float) j;
      int num2 = WorldGen.genRand.Next(35, 80);
      if (forceX)
      {
        num2 += 20;
        WorldGen.lastDungeonHall = new Vector2();
      }
      else if (WorldGen.genRand.Next(5) == 0)
      {
        num1 *= 2.0;
        num2 /= 2;
      }
      bool flag1 = false;
      while (!flag1)
      {
        int num3 = WorldGen.genRand.Next(2) != 0 ? 1 : -1;
        bool flag2 = false;
        if (WorldGen.genRand.Next(2) == 0)
          flag2 = true;
        if (forceX)
          flag2 = true;
        if (flag2)
        {
          vector2_2.Y = 0.0f;
          vector2_2.X = (float) num3;
          vector2_3.Y = 0.0f;
          vector2_3.X = (float) -num3;
          vector2_1.Y = 0.0f;
          vector2_1.X = (float) num3;
          if (WorldGen.genRand.Next(3) == 0)
            vector2_1.Y = WorldGen.genRand.Next(2) != 0 ? 0.2f : -0.2f;
        }
        else
        {
          ++num1;
          vector2_1.Y = (float) num3;
          vector2_1.X = 0.0f;
          vector2_2.X = 0.0f;
          vector2_2.Y = (float) num3;
          vector2_3.X = 0.0f;
          vector2_3.Y = (float) -num3;
          if (WorldGen.genRand.Next(2) == 0)
            vector2_1.X = WorldGen.genRand.Next(2) != 0 ? -0.3f : 0.3f;
          else
            num2 /= 2;
        }
        if (WorldGen.lastDungeonHall != vector2_3)
          flag1 = true;
      }
      if (!forceX)
      {
        if ((double) vector2_4.X > (double) (WorldGen.lastMaxTilesX - 200))
        {
          int num4 = -1;
          vector2_2.Y = 0.0f;
          vector2_2.X = (float) num4;
          vector2_1.Y = 0.0f;
          vector2_1.X = (float) num4;
          if (WorldGen.genRand.Next(3) == 0)
            vector2_1.Y = WorldGen.genRand.Next(2) != 0 ? 0.2f : -0.2f;
        }
        else if ((double) vector2_4.X < 200.0)
        {
          int num5 = 1;
          vector2_2.Y = 0.0f;
          vector2_2.X = (float) num5;
          vector2_1.Y = 0.0f;
          vector2_1.X = (float) num5;
          if (WorldGen.genRand.Next(3) == 0)
            vector2_1.Y = WorldGen.genRand.Next(2) != 0 ? 0.2f : -0.2f;
        }
        else if ((double) vector2_4.Y > (double) (WorldGen.lastMaxTilesY + 200))
        {
          int num6 = -1;
          ++num1;
          vector2_1.Y = (float) num6;
          vector2_1.X = 0.0f;
          vector2_2.X = 0.0f;
          vector2_2.Y = (float) num6;
          if (WorldGen.genRand.Next(2) == 0)
            vector2_1.X = WorldGen.genRand.Next(2) != 0 ? -0.3f : 0.3f;
        }
        else if ((double) vector2_4.Y < Game1.rockLayer)
        {
          int num7 = 1;
          ++num1;
          vector2_1.Y = (float) num7;
          vector2_1.X = 0.0f;
          vector2_2.X = 0.0f;
          vector2_2.Y = (float) num7;
          if (WorldGen.genRand.Next(2) == 0)
            vector2_1.X = WorldGen.genRand.Next(2) != 0 ? -0.3f : 0.3f;
        }
        else if ((double) vector2_4.X < (double) (Game1.maxTilesX / 2) && (double) vector2_4.X > (double) Game1.maxTilesX * 0.25)
        {
          int num8 = -1;
          vector2_2.Y = 0.0f;
          vector2_2.X = (float) num8;
          vector2_1.Y = 0.0f;
          vector2_1.X = (float) num8;
          if (WorldGen.genRand.Next(3) == 0)
            vector2_1.Y = WorldGen.genRand.Next(2) != 0 ? 0.2f : -0.2f;
        }
        else if ((double) vector2_4.X > (double) (Game1.maxTilesX / 2) && (double) vector2_4.X < (double) Game1.maxTilesX * 0.75)
        {
          int num9 = 1;
          vector2_2.Y = 0.0f;
          vector2_2.X = (float) num9;
          vector2_1.Y = 0.0f;
          vector2_1.X = (float) num9;
          if (WorldGen.genRand.Next(3) == 0)
            vector2_1.Y = WorldGen.genRand.Next(2) != 0 ? 0.2f : -0.2f;
        }
      }
      if ((double) vector2_2.Y == 0.0)
      {
        WorldGen.DDoorX[WorldGen.numDDoors] = (int) vector2_4.X;
        WorldGen.DDoorY[WorldGen.numDDoors] = (int) vector2_4.Y;
        WorldGen.DDoorPos[WorldGen.numDDoors] = 0;
        ++WorldGen.numDDoors;
      }
      else
      {
        WorldGen.DPlatX[WorldGen.numDPlats] = (int) vector2_4.X;
        WorldGen.DPlatY[WorldGen.numDPlats] = (int) vector2_4.Y;
        ++WorldGen.numDPlats;
      }
      WorldGen.lastDungeonHall = vector2_2;
      while (num2 > 0)
      {
        if ((double) vector2_2.X > 0.0 && (double) vector2_4.X > (double) (Game1.maxTilesX - 100))
          num2 = 0;
        else if ((double) vector2_2.X < 0.0 && (double) vector2_4.X < 100.0)
          num2 = 0;
        else if ((double) vector2_2.Y > 0.0 && (double) vector2_4.Y > (double) (Game1.maxTilesY - 100))
          num2 = 0;
        else if ((double) vector2_2.Y < 0.0 && (double) vector2_4.Y < Game1.rockLayer + 50.0)
          num2 = 0;
        --num2;
        int num10 = (int) ((double) vector2_4.X - num1 - 4.0 - (double) WorldGen.genRand.Next(6));
        int num11 = (int) ((double) vector2_4.X + num1 + 4.0 + (double) WorldGen.genRand.Next(6));
        int num12 = (int) ((double) vector2_4.Y - num1 - 4.0 - (double) WorldGen.genRand.Next(6));
        int num13 = (int) ((double) vector2_4.Y + num1 + 4.0 + (double) WorldGen.genRand.Next(6));
        if (num10 < 0)
          num10 = 0;
        if (num11 > Game1.maxTilesX)
          num11 = Game1.maxTilesX;
        if (num12 < 0)
          num12 = 0;
        if (num13 > Game1.maxTilesY)
          num13 = Game1.maxTilesY;
        for (int index1 = num10; index1 < num11; ++index1)
        {
          for (int index2 = num12; index2 < num13; ++index2)
          {
            Game1.tile[index1, index2].liquid = (byte) 0;
            if (Game1.tile[index1, index2].wall == (byte) 0)
            {
              Game1.tile[index1, index2].active = true;
              Game1.tile[index1, index2].type = (byte) tileType;
            }
          }
        }
        for (int i1 = num10 + 1; i1 < num11 - 1; ++i1)
        {
          for (int j1 = num12 + 1; j1 < num13 - 1; ++j1)
            WorldGen.PlaceWall(i1, j1, wallType, true);
        }
        int num14 = 0;
        if ((double) vector2_1.Y == 0.0 && WorldGen.genRand.Next((int) num1 + 1) == 0)
          num14 = WorldGen.genRand.Next(1, 3);
        else if ((double) vector2_1.X == 0.0 && WorldGen.genRand.Next((int) num1 - 1) == 0)
          num14 = WorldGen.genRand.Next(1, 3);
        else if (WorldGen.genRand.Next((int) num1 * 3) == 0)
          num14 = WorldGen.genRand.Next(1, 3);
        int num15 = (int) ((double) vector2_4.X - num1 * 0.5 - (double) num14);
        int num16 = (int) ((double) vector2_4.X + num1 * 0.5 + (double) num14);
        int num17 = (int) ((double) vector2_4.Y - num1 * 0.5 - (double) num14);
        int num18 = (int) ((double) vector2_4.Y + num1 * 0.5 + (double) num14);
        if (num15 < 0)
          num15 = 0;
        if (num16 > Game1.maxTilesX)
          num16 = Game1.maxTilesX;
        if (num17 < 0)
          num17 = 0;
        if (num18 > Game1.maxTilesY)
          num18 = Game1.maxTilesY;
        for (int index3 = num15; index3 < num16; ++index3)
        {
          for (int index4 = num17; index4 < num18; ++index4)
          {
            Game1.tile[index3, index4].active = false;
            Game1.tile[index3, index4].wall = (byte) wallType;
          }
        }
        vector2_4 += vector2_1;
      }
      WorldGen.dungeonX = (int) vector2_4.X;
      WorldGen.dungeonY = (int) vector2_4.Y;
      if ((double) vector2_2.Y == 0.0)
      {
        WorldGen.DDoorX[WorldGen.numDDoors] = (int) vector2_4.X;
        WorldGen.DDoorY[WorldGen.numDDoors] = (int) vector2_4.Y;
        WorldGen.DDoorPos[WorldGen.numDDoors] = 0;
        ++WorldGen.numDDoors;
      }
      else
      {
        WorldGen.DPlatX[WorldGen.numDPlats] = (int) vector2_4.X;
        WorldGen.DPlatY[WorldGen.numDPlats] = (int) vector2_4.Y;
        ++WorldGen.numDPlats;
      }
    }

    public static void DungeonRoom(int i, int j, int tileType, int wallType)
    {
      double num1 = (double) WorldGen.genRand.Next(15, 30);
      Vector2 vector2_1;
      vector2_1.X = (float) WorldGen.genRand.Next(-10, 11) * 0.1f;
      vector2_1.Y = (float) WorldGen.genRand.Next(-10, 11) * 0.1f;
      Vector2 vector2_2;
      vector2_2.X = (float) i;
      vector2_2.Y = (float) j - (float) num1 / 2f;
      int num2 = WorldGen.genRand.Next(10, 20);
      double num3 = (double) vector2_2.X;
      double num4 = (double) vector2_2.X;
      double num5 = (double) vector2_2.Y;
      double num6 = (double) vector2_2.Y;
      while (num2 > 0)
      {
        --num2;
        int num7 = (int) ((double) vector2_2.X - num1 * 0.800000011920929 - 5.0);
        int num8 = (int) ((double) vector2_2.X + num1 * 0.800000011920929 + 5.0);
        int num9 = (int) ((double) vector2_2.Y - num1 * 0.800000011920929 - 5.0);
        int num10 = (int) ((double) vector2_2.Y + num1 * 0.800000011920929 + 5.0);
        if (num7 < 0)
          num7 = 0;
        if (num8 > Game1.maxTilesX)
          num8 = Game1.maxTilesX;
        if (num9 < 0)
          num9 = 0;
        if (num10 > Game1.maxTilesY)
          num10 = Game1.maxTilesY;
        for (int index1 = num7; index1 < num8; ++index1)
        {
          for (int index2 = num9; index2 < num10; ++index2)
          {
            Game1.tile[index1, index2].liquid = (byte) 0;
            if (Game1.tile[index1, index2].wall == (byte) 0)
            {
              Game1.tile[index1, index2].active = true;
              Game1.tile[index1, index2].type = (byte) tileType;
            }
          }
        }
        for (int i1 = num7 + 1; i1 < num8 - 1; ++i1)
        {
          for (int j1 = num9 + 1; j1 < num10 - 1; ++j1)
            WorldGen.PlaceWall(i1, j1, wallType, true);
        }
        int num11 = (int) ((double) vector2_2.X - num1 * 0.5);
        int num12 = (int) ((double) vector2_2.X + num1 * 0.5);
        int num13 = (int) ((double) vector2_2.Y - num1 * 0.5);
        int num14 = (int) ((double) vector2_2.Y + num1 * 0.5);
        if (num11 < 0)
          num11 = 0;
        if (num12 > Game1.maxTilesX)
          num12 = Game1.maxTilesX;
        if (num13 < 0)
          num13 = 0;
        if (num14 > Game1.maxTilesY)
          num14 = Game1.maxTilesY;
        if ((double) num11 < num3)
          num3 = (double) num11;
        if ((double) num12 > num4)
          num4 = (double) num12;
        if ((double) num13 < num5)
          num5 = (double) num13;
        if ((double) num14 > num6)
          num6 = (double) num14;
        for (int index3 = num11; index3 < num12; ++index3)
        {
          for (int index4 = num13; index4 < num14; ++index4)
          {
            Game1.tile[index3, index4].active = false;
            Game1.tile[index3, index4].wall = (byte) wallType;
          }
        }
        vector2_2 += vector2_1;
        vector2_1.X += (float) WorldGen.genRand.Next(-10, 11) * 0.05f;
        vector2_1.Y += (float) WorldGen.genRand.Next(-10, 11) * 0.05f;
        if ((double) vector2_1.X > 1.0)
          vector2_1.X = 1f;
        if ((double) vector2_1.X < -1.0)
          vector2_1.X = -1f;
        if ((double) vector2_1.Y > 1.0)
          vector2_1.Y = 1f;
        if ((double) vector2_1.Y < -1.0)
          vector2_1.Y = -1f;
      }
      WorldGen.dRoomX[WorldGen.numDRooms] = (int) vector2_2.X;
      WorldGen.dRoomY[WorldGen.numDRooms] = (int) vector2_2.Y;
      WorldGen.dRoomSize[WorldGen.numDRooms] = (int) num1;
      WorldGen.dRoomL[WorldGen.numDRooms] = (int) num3;
      WorldGen.dRoomR[WorldGen.numDRooms] = (int) num4;
      WorldGen.dRoomT[WorldGen.numDRooms] = (int) num5;
      WorldGen.dRoomB[WorldGen.numDRooms] = (int) num6;
      WorldGen.dRoomTreasure[WorldGen.numDRooms] = false;
      ++WorldGen.numDRooms;
    }

    public static void DungeonEnt(int i, int j, int tileType, int wallType)
    {
      double dxStrength1 = WorldGen.dxStrength1;
      double dyStrength1 = WorldGen.dyStrength1;
      Vector2 vector2;
      vector2.X = (float) i;
      vector2.Y = (float) j - (float) dyStrength1 / 2f;
      WorldGen.dMinY = (int) vector2.Y;
      int num1 = 1;
      if (i > Game1.maxTilesX / 2)
        num1 = -1;
      int num2 = (int) ((double) vector2.X - dxStrength1 * 0.60000002384185791 - (double) WorldGen.genRand.Next(2, 5));
      int num3 = (int) ((double) vector2.X + dxStrength1 * 0.60000002384185791 + (double) WorldGen.genRand.Next(2, 5));
      int num4 = (int) ((double) vector2.Y - dyStrength1 * 0.60000002384185791 - (double) WorldGen.genRand.Next(2, 5));
      int num5 = (int) ((double) vector2.Y + dyStrength1 * 0.60000002384185791 + (double) WorldGen.genRand.Next(8, 16));
      if (num2 < 0)
        num2 = 0;
      if (num3 > Game1.maxTilesX)
        num3 = Game1.maxTilesX;
      if (num4 < 0)
        num4 = 0;
      if (num5 > Game1.maxTilesY)
        num5 = Game1.maxTilesY;
      for (int i1 = num2; i1 < num3; ++i1)
      {
        for (int j1 = num4; j1 < num5; ++j1)
        {
          Game1.tile[i1, j1].liquid = (byte) 0;
          if ((int) Game1.tile[i1, j1].wall != wallType)
          {
            Game1.tile[i1, j1].wall = (byte) 0;
            if (i1 > num2 + 1 && i1 < num3 - 2 && j1 > num4 + 1 && j1 < num5 - 2)
              WorldGen.PlaceWall(i1, j1, wallType, true);
            Game1.tile[i1, j1].active = true;
            Game1.tile[i1, j1].type = (byte) tileType;
          }
        }
      }
      int num6 = num2;
      int num7 = num2 + 5 + WorldGen.genRand.Next(4);
      int num8 = num4 - 3 - WorldGen.genRand.Next(3);
      int num9 = num4;
      for (int index1 = num6; index1 < num7; ++index1)
      {
        for (int index2 = num8; index2 < num9; ++index2)
        {
          if ((int) Game1.tile[index1, index2].wall != wallType)
          {
            Game1.tile[index1, index2].active = true;
            Game1.tile[index1, index2].type = (byte) tileType;
          }
        }
      }
      int num10 = num3 - 5 - WorldGen.genRand.Next(4);
      int num11 = num3;
      int num12 = num4 - 3 - WorldGen.genRand.Next(3);
      int num13 = num4;
      for (int index3 = num10; index3 < num11; ++index3)
      {
        for (int index4 = num12; index4 < num13; ++index4)
        {
          if ((int) Game1.tile[index3, index4].wall != wallType)
          {
            Game1.tile[index3, index4].active = true;
            Game1.tile[index3, index4].type = (byte) tileType;
          }
        }
      }
      int num14 = 1 + WorldGen.genRand.Next(2);
      int num15 = 2 + WorldGen.genRand.Next(4);
      int num16 = 0;
      for (int index5 = num2; index5 < num3; ++index5)
      {
        for (int index6 = num4 - num14; index6 < num4; ++index6)
        {
          if ((int) Game1.tile[index5, index6].wall != wallType)
          {
            Game1.tile[index5, index6].active = true;
            Game1.tile[index5, index6].type = (byte) tileType;
          }
        }
        ++num16;
        if (num16 >= num15)
        {
          index5 += num15;
          num16 = 0;
        }
      }
      for (int i2 = num2; i2 < num3; ++i2)
      {
        for (int j2 = num5; j2 < num5 + 100; ++j2)
          WorldGen.PlaceWall(i2, j2, 2, true);
      }
      int num17 = (int) ((double) vector2.X - dxStrength1 * 0.60000002384185791);
      int num18 = (int) ((double) vector2.X + dxStrength1 * 0.60000002384185791);
      int num19 = (int) ((double) vector2.Y - dyStrength1 * 0.60000002384185791);
      int num20 = (int) ((double) vector2.Y + dyStrength1 * 0.60000002384185791);
      if (num17 < 0)
        num17 = 0;
      if (num18 > Game1.maxTilesX)
        num18 = Game1.maxTilesX;
      if (num19 < 0)
        num19 = 0;
      if (num20 > Game1.maxTilesY)
        num20 = Game1.maxTilesY;
      for (int i3 = num17; i3 < num18; ++i3)
      {
        for (int j3 = num19; j3 < num20; ++j3)
          WorldGen.PlaceWall(i3, j3, wallType, true);
      }
      int num21 = (int) ((double) vector2.X - dxStrength1 * 0.6 - 1.0);
      int num22 = (int) ((double) vector2.X + dxStrength1 * 0.6 + 1.0);
      int num23 = (int) ((double) vector2.Y - dyStrength1 * 0.6 - 1.0);
      int num24 = (int) ((double) vector2.Y + dyStrength1 * 0.6 + 1.0);
      if (num21 < 0)
        num21 = 0;
      if (num22 > Game1.maxTilesX)
        num22 = Game1.maxTilesX;
      if (num23 < 0)
        num23 = 0;
      if (num24 > Game1.maxTilesY)
        num24 = Game1.maxTilesY;
      for (int index7 = num21; index7 < num22; ++index7)
      {
        for (int index8 = num23; index8 < num24; ++index8)
          Game1.tile[index7, index8].wall = (byte) wallType;
      }
      int num25 = (int) ((double) vector2.X - dxStrength1 * 0.5);
      int num26 = (int) ((double) vector2.X + dxStrength1 * 0.5);
      int num27 = (int) ((double) vector2.Y - dyStrength1 * 0.5);
      int num28 = (int) ((double) vector2.Y + dyStrength1 * 0.5);
      if (num25 < 0)
        num25 = 0;
      if (num26 > Game1.maxTilesX)
        num26 = Game1.maxTilesX;
      if (num27 < 0)
        num27 = 0;
      if (num28 > Game1.maxTilesY)
        num28 = Game1.maxTilesY;
      for (int index9 = num25; index9 < num26; ++index9)
      {
        for (int index10 = num27; index10 < num28; ++index10)
        {
          Game1.tile[index9, index10].active = false;
          Game1.tile[index9, index10].wall = (byte) wallType;
        }
      }
      WorldGen.DPlatX[WorldGen.numDPlats] = (int) vector2.X;
      WorldGen.DPlatY[WorldGen.numDPlats] = num28;
      ++WorldGen.numDPlats;
      vector2.X += (float) (dxStrength1 * 0.60000002384185791) * (float) num1;
      vector2.Y += (float) dyStrength1 * 0.5f;
      double dxStrength2 = WorldGen.dxStrength2;
      double dyStrength2 = WorldGen.dyStrength2;
      vector2.X += (float) (dxStrength2 * 0.550000011920929) * (float) num1;
      vector2.Y -= (float) dyStrength2 * 0.5f;
      int num29 = (int) ((double) vector2.X - dxStrength2 * 0.60000002384185791 - (double) WorldGen.genRand.Next(1, 3));
      int num30 = (int) ((double) vector2.X + dxStrength2 * 0.60000002384185791 + (double) WorldGen.genRand.Next(1, 3));
      int num31 = (int) ((double) vector2.Y - dyStrength2 * 0.60000002384185791 - (double) WorldGen.genRand.Next(1, 3));
      int num32 = (int) ((double) vector2.Y + dyStrength2 * 0.60000002384185791 + (double) WorldGen.genRand.Next(6, 16));
      if (num29 < 0)
        num29 = 0;
      if (num30 > Game1.maxTilesX)
        num30 = Game1.maxTilesX;
      if (num31 < 0)
        num31 = 0;
      if (num32 > Game1.maxTilesY)
        num32 = Game1.maxTilesY;
      for (int index11 = num29; index11 < num30; ++index11)
      {
        for (int index12 = num31; index12 < num32; ++index12)
        {
          if ((int) Game1.tile[index11, index12].wall != wallType)
          {
            bool flag = true;
            if (num1 < 0)
            {
              if ((double) index11 < (double) vector2.X - dxStrength2 * 0.5)
                flag = false;
            }
            else if ((double) index11 > (double) vector2.X + dxStrength2 * 0.5 - 1.0)
              flag = false;
            if (flag)
            {
              Game1.tile[index11, index12].wall = (byte) 0;
              Game1.tile[index11, index12].active = true;
              Game1.tile[index11, index12].type = (byte) tileType;
            }
          }
        }
      }
      for (int i4 = num29; i4 < num30; ++i4)
      {
        for (int j4 = num32; j4 < num32 + 100; ++j4)
          WorldGen.PlaceWall(i4, j4, 2, true);
      }
      int num33 = (int) ((double) vector2.X - dxStrength2 * 0.5);
      int num34 = (int) ((double) vector2.X + dxStrength2 * 0.5);
      int num35 = num33;
      if (num1 < 0)
        ++num35;
      int num36 = num35 + 5 + WorldGen.genRand.Next(4);
      int num37 = num31 - 3 - WorldGen.genRand.Next(3);
      int num38 = num31;
      for (int index13 = num35; index13 < num36; ++index13)
      {
        for (int index14 = num37; index14 < num38; ++index14)
        {
          if ((int) Game1.tile[index13, index14].wall != wallType)
          {
            Game1.tile[index13, index14].active = true;
            Game1.tile[index13, index14].type = (byte) tileType;
          }
        }
      }
      int num39 = num34 - 5 - WorldGen.genRand.Next(4);
      int num40 = num34;
      int num41 = num31 - 3 - WorldGen.genRand.Next(3);
      int num42 = num31;
      for (int index15 = num39; index15 < num40; ++index15)
      {
        for (int index16 = num41; index16 < num42; ++index16)
        {
          if ((int) Game1.tile[index15, index16].wall != wallType)
          {
            Game1.tile[index15, index16].active = true;
            Game1.tile[index15, index16].type = (byte) tileType;
          }
        }
      }
      int num43 = 1 + WorldGen.genRand.Next(2);
      int num44 = 2 + WorldGen.genRand.Next(4);
      int num45 = 0;
      if (num1 < 0)
        ++num34;
      for (int index17 = num33 + 1; index17 < num34 - 1; ++index17)
      {
        for (int index18 = num31 - num43; index18 < num31; ++index18)
        {
          if ((int) Game1.tile[index17, index18].wall != wallType)
          {
            Game1.tile[index17, index18].active = true;
            Game1.tile[index17, index18].type = (byte) tileType;
          }
        }
        ++num45;
        if (num45 >= num44)
        {
          index17 += num44;
          num45 = 0;
        }
      }
      int num46 = (int) ((double) vector2.X - dxStrength2 * 0.6);
      int num47 = (int) ((double) vector2.X + dxStrength2 * 0.6);
      int num48 = (int) ((double) vector2.Y - dyStrength2 * 0.6);
      int num49 = (int) ((double) vector2.Y + dyStrength2 * 0.6);
      if (num46 < 0)
        num46 = 0;
      if (num47 > Game1.maxTilesX)
        num47 = Game1.maxTilesX;
      if (num48 < 0)
        num48 = 0;
      if (num49 > Game1.maxTilesY)
        num49 = Game1.maxTilesY;
      for (int index19 = num46; index19 < num47; ++index19)
      {
        for (int index20 = num48; index20 < num49; ++index20)
          Game1.tile[index19, index20].wall = (byte) 0;
      }
      int num50 = (int) ((double) vector2.X - dxStrength2 * 0.5);
      int num51 = (int) ((double) vector2.X + dxStrength2 * 0.5);
      int num52 = (int) ((double) vector2.Y - dyStrength2 * 0.5);
      int index21 = (int) ((double) vector2.Y + dyStrength2 * 0.5);
      if (num50 < 0)
        num50 = 0;
      if (num51 > Game1.maxTilesX)
        num51 = Game1.maxTilesX;
      if (num52 < 0)
        num52 = 0;
      if (index21 > Game1.maxTilesY)
        index21 = Game1.maxTilesY;
      for (int index22 = num50; index22 < num51; ++index22)
      {
        for (int index23 = num52; index23 < index21; ++index23)
        {
          Game1.tile[index22, index23].active = false;
          Game1.tile[index22, index23].wall = (byte) 0;
        }
      }
      for (int index24 = num50; index24 < num51; ++index24)
      {
        if (!Game1.tile[index24, index21].active)
        {
          Game1.tile[index24, index21].active = true;
          Game1.tile[index24, index21].type = (byte) 19;
        }
      }
      Game1.dungeonX = (int) vector2.X;
      Game1.dungeonY = index21;
      int index25 = NPC.NewNPC(WorldGen.dungeonX * 16 + 8, WorldGen.dungeonY * 16, 37);
      Game1.npc[index25].homeless = false;
      Game1.npc[index25].homeTileX = Game1.dungeonX;
      Game1.npc[index25].homeTileY = Game1.dungeonY;
      if (num1 == 1)
      {
        int num53 = 0;
        for (int index26 = num51; index26 < num51 + 25; ++index26)
        {
          ++num53;
          for (int index27 = index21 + num53; index27 < index21 + 25; ++index27)
          {
            Game1.tile[index26, index27].active = true;
            Game1.tile[index26, index27].type = (byte) tileType;
          }
        }
      }
      else
      {
        int num54 = 0;
        for (int index28 = num50; index28 > num50 - 25; --index28)
        {
          ++num54;
          for (int index29 = index21 + num54; index29 < index21 + 25; ++index29)
          {
            Game1.tile[index28, index29].active = true;
            Game1.tile[index28, index29].type = (byte) tileType;
          }
        }
      }
      int num55 = 1 + WorldGen.genRand.Next(2);
      int num56 = 2 + WorldGen.genRand.Next(4);
      int num57 = 0;
      int num58 = (int) ((double) vector2.X - dxStrength2 * 0.5);
      int num59 = (int) ((double) vector2.X + dxStrength2 * 0.5);
      int num60 = num58 + 2;
      int num61 = num59 - 2;
      for (int i5 = num60; i5 < num61; ++i5)
      {
        for (int j5 = num52; j5 < index21; ++j5)
          WorldGen.PlaceWall(i5, j5, wallType, true);
        ++num57;
        if (num57 >= num56)
        {
          i5 += num56 * 2;
          num57 = 0;
        }
      }
      vector2.X -= (float) (dxStrength2 * 0.60000002384185791) * (float) num1;
      vector2.Y += (float) dyStrength2 * 0.5f;
      double num62 = 15.0;
      double num63 = 3.0;
      vector2.Y -= (float) num63 * 0.5f;
      int num64 = (int) ((double) vector2.X - num62 * 0.5);
      int num65 = (int) ((double) vector2.X + num62 * 0.5);
      int num66 = (int) ((double) vector2.Y - num63 * 0.5);
      int num67 = (int) ((double) vector2.Y + num63 * 0.5);
      if (num64 < 0)
        num64 = 0;
      if (num65 > Game1.maxTilesX)
        num65 = Game1.maxTilesX;
      if (num66 < 0)
        num66 = 0;
      if (num67 > Game1.maxTilesY)
        num67 = Game1.maxTilesY;
      for (int index30 = num64; index30 < num65; ++index30)
      {
        for (int index31 = num66; index31 < num67; ++index31)
          Game1.tile[index30, index31].active = false;
      }
      if (num1 < 0)
        --vector2.X;
      WorldGen.PlaceTile((int) vector2.X, (int) vector2.Y + 1, 10);
    }

    public static bool AddBuriedChest(int i, int j, int contain = 0)
    {
      if (WorldGen.genRand == null)
        WorldGen.genRand = new Random((int) DateTime.Now.Ticks);
      for (int index1 = j; index1 < Game1.maxTilesY; ++index1)
      {
        if (Game1.tile[i, index1].active && Game1.tileSolid[(int) Game1.tile[i, index1].type])
        {
          int index2 = WorldGen.PlaceChest(i - 1, index1 - 1);
          if (index2 < 0)
            return false;
          int index3 = 0;
          while (index3 == 0)
          {
            if (contain > 0)
            {
              Game1.chest[index2].item[index3].SetDefaults(contain);
              ++index3;
            }
            else
            {
              int num = WorldGen.genRand.Next(7);
              if (num == 0)
                Game1.chest[index2].item[index3].SetDefaults(49);
              if (num == 1)
                Game1.chest[index2].item[index3].SetDefaults(50);
              if (num == 2)
                Game1.chest[index2].item[index3].SetDefaults(52);
              if (num == 3)
                Game1.chest[index2].item[index3].SetDefaults(53);
              if (num == 4)
                Game1.chest[index2].item[index3].SetDefaults(54);
              if (num == 5)
                Game1.chest[index2].item[index3].SetDefaults(55);
              if (num == 6)
              {
                Game1.chest[index2].item[index3].SetDefaults(51);
                Game1.chest[index2].item[index3].stack = WorldGen.genRand.Next(26) + 25;
              }
              ++index3;
            }
            if (WorldGen.genRand.Next(3) == 0)
            {
              Game1.chest[index2].item[index3].SetDefaults(167);
              ++index3;
            }
            if (WorldGen.genRand.Next(2) == 0)
            {
              int num1 = WorldGen.genRand.Next(4);
              int num2 = WorldGen.genRand.Next(8) + 3;
              if (num1 == 0)
                Game1.chest[index2].item[index3].SetDefaults(19);
              if (num1 == 1)
                Game1.chest[index2].item[index3].SetDefaults(20);
              if (num1 == 2)
                Game1.chest[index2].item[index3].SetDefaults(21);
              if (num1 == 3)
                Game1.chest[index2].item[index3].SetDefaults(22);
              Game1.chest[index2].item[index3].stack = num2;
              ++index3;
            }
            if (WorldGen.genRand.Next(2) == 0)
            {
              int num3 = WorldGen.genRand.Next(2);
              int num4 = WorldGen.genRand.Next(26) + 25;
              if (num3 == 0)
                Game1.chest[index2].item[index3].SetDefaults(40);
              if (num3 == 1)
                Game1.chest[index2].item[index3].SetDefaults(42);
              Game1.chest[index2].item[index3].stack = num4;
              ++index3;
            }
            if (WorldGen.genRand.Next(2) == 0)
            {
              int num5 = WorldGen.genRand.Next(1);
              int num6 = WorldGen.genRand.Next(3) + 3;
              if (num5 == 0)
                Game1.chest[index2].item[index3].SetDefaults(28);
              Game1.chest[index2].item[index3].stack = num6;
              ++index3;
            }
            if (WorldGen.genRand.Next(2) == 0)
            {
              int num7 = WorldGen.genRand.Next(2);
              int num8 = WorldGen.genRand.Next(11) + 10;
              if (num7 == 0)
                Game1.chest[index2].item[index3].SetDefaults(8);
              if (num7 == 1)
                Game1.chest[index2].item[index3].SetDefaults(31);
              Game1.chest[index2].item[index3].stack = num8;
              ++index3;
            }
            if (WorldGen.genRand.Next(2) == 0)
            {
              Game1.chest[index2].item[index3].SetDefaults(73);
              Game1.chest[index2].item[index3].stack = WorldGen.genRand.Next(1, 3);
              ++index3;
            }
          }
          return true;
        }
      }
      return false;
    }

    public static bool OpenDoor(int i, int j, int direction)
    {
      if (Game1.tile[i, j - 1] == null)
        Game1.tile[i, j - 1] = new Tile();
      if (Game1.tile[i, j - 2] == null)
        Game1.tile[i, j - 2] = new Tile();
      if (Game1.tile[i, j + 1] == null)
        Game1.tile[i, j + 1] = new Tile();
      if (Game1.tile[i, j] == null)
        Game1.tile[i, j] = new Tile();
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
        if (Game1.tile[i1, j1] == null)
          Game1.tile[i1, j1] = new Tile();
        if (Game1.tile[i1, j1].active)
        {
          if (Game1.tile[i1, j1].type == (byte) 3 || Game1.tile[i1, j1].type == (byte) 24 || Game1.tile[i1, j1].type == (byte) 61 || Game1.tile[i1, j1].type == (byte) 62 || Game1.tile[i1, j1].type == (byte) 69 || Game1.tile[i1, j1].type == (byte) 71 || Game1.tile[i1, j1].type == (byte) 73 || Game1.tile[i1, j1].type == (byte) 74)
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
      if (flag)
      {
        Game1.PlaySound(8, i * 16, j * 16);
        Game1.tile[index2, index1].active = true;
        Game1.tile[index2, index1].type = (byte) 11;
        Game1.tile[index2, index1].frameY = (short) 0;
        Game1.tile[index2, index1].frameX = num;
        if (Game1.tile[index2 + 1, index1] == null)
          Game1.tile[index2 + 1, index1] = new Tile();
        Game1.tile[index2 + 1, index1].active = true;
        Game1.tile[index2 + 1, index1].type = (byte) 11;
        Game1.tile[index2 + 1, index1].frameY = (short) 0;
        Game1.tile[index2 + 1, index1].frameX = (short) ((int) num + 18);
        if (Game1.tile[index2, index1 + 1] == null)
          Game1.tile[index2, index1 + 1] = new Tile();
        Game1.tile[index2, index1 + 1].active = true;
        Game1.tile[index2, index1 + 1].type = (byte) 11;
        Game1.tile[index2, index1 + 1].frameY = (short) 18;
        Game1.tile[index2, index1 + 1].frameX = num;
        if (Game1.tile[index2 + 1, index1 + 1] == null)
          Game1.tile[index2 + 1, index1 + 1] = new Tile();
        Game1.tile[index2 + 1, index1 + 1].active = true;
        Game1.tile[index2 + 1, index1 + 1].type = (byte) 11;
        Game1.tile[index2 + 1, index1 + 1].frameY = (short) 18;
        Game1.tile[index2 + 1, index1 + 1].frameX = (short) ((int) num + 18);
        if (Game1.tile[index2, index1 + 2] == null)
          Game1.tile[index2, index1 + 2] = new Tile();
        Game1.tile[index2, index1 + 2].active = true;
        Game1.tile[index2, index1 + 2].type = (byte) 11;
        Game1.tile[index2, index1 + 2].frameY = (short) 36;
        Game1.tile[index2, index1 + 2].frameX = num;
        if (Game1.tile[index2 + 1, index1 + 2] == null)
          Game1.tile[index2 + 1, index1 + 2] = new Tile();
        Game1.tile[index2 + 1, index1 + 2].active = true;
        Game1.tile[index2 + 1, index1 + 2].type = (byte) 11;
        Game1.tile[index2 + 1, index1 + 2].frameY = (short) 36;
        Game1.tile[index2 + 1, index1 + 2].frameX = (short) ((int) num + 18);
        for (int i2 = index2 - 1; i2 <= index2 + 2; ++i2)
        {
          for (int j2 = index1 - 1; j2 <= index1 + 2; ++j2)
            WorldGen.TileFrame(i2, j2);
        }
      }
      return flag;
    }

    public static void Check1x2(int x, int j, byte type)
    {
      if (WorldGen.destroyObject)
        return;
      int j1 = j;
      bool flag = true;
      if (Game1.tile[x, j1] == null)
        Game1.tile[x, j1] = new Tile();
      if (Game1.tile[x, j1 + 1] == null)
        Game1.tile[x, j1 + 1] = new Tile();
      if (Game1.tile[x, j1].frameY == (short) 18)
        --j1;
      if (Game1.tile[x, j1] == null)
        Game1.tile[x, j1] = new Tile();
      if (Game1.tile[x, j1].frameY == (short) 0 && Game1.tile[x, j1 + 1].frameY == (short) 18 && (int) Game1.tile[x, j1].type == (int) type && (int) Game1.tile[x, j1 + 1].type == (int) type)
        flag = false;
      if (Game1.tile[x, j1 + 2] == null)
        Game1.tile[x, j1 + 2] = new Tile();
      if (!Game1.tile[x, j1 + 2].active || !Game1.tileSolid[(int) Game1.tile[x, j1 + 2].type])
        flag = true;
      if (Game1.tile[x, j1 + 2].type != (byte) 2 && Game1.tile[x, j1].type == (byte) 20)
        flag = true;
      if (!flag)
        return;
      WorldGen.destroyObject = true;
      if ((int) Game1.tile[x, j1].type == (int) type)
        WorldGen.KillTile(x, j1);
      if ((int) Game1.tile[x, j1 + 1].type == (int) type)
        WorldGen.KillTile(x, j1 + 1);
      if (type == (byte) 15)
        Item.NewItem(x * 16, j1 * 16, 32, 32, 34);
      WorldGen.destroyObject = false;
    }

    public static void CheckOnTable1x1(int x, int y, int type)
    {
      if (Game1.tile[x, y + 1] == null || Game1.tile[x, y + 1].active && Game1.tileTable[(int) Game1.tile[x, y + 1].type])
        return;
      WorldGen.KillTile(x, y);
    }

    public static void CheckSign(int x, int y, int type)
    {
      if (WorldGen.destroyObject)
        return;
      int num1 = x - 2;
      int num2 = x + 3;
      int num3 = y - 2;
      int num4 = y + 3;
      if (num1 < 0 || num2 > Game1.maxTilesX || num3 < 0 || num4 > Game1.maxTilesY)
        return;
      bool flag = false;
      for (int index1 = num1; index1 < num2; ++index1)
      {
        for (int index2 = num3; index2 < num4; ++index2)
        {
          if (Game1.tile[index1, index2] == null)
            Game1.tile[index1, index2] = new Tile();
        }
      }
      int num5 = (int) Game1.tile[x, y].frameX / 18;
      int num6 = (int) Game1.tile[x, y].frameY / 18;
      while (num5 > 1)
        num5 -= 2;
      int x1 = x - num5;
      int y1 = y - num6;
      int num7 = (int) Game1.tile[x1, y1].frameX / 18 / 2;
      int num8 = x1;
      int num9 = x1 + 2;
      int num10 = y1;
      int num11 = y1 + 2;
      int num12 = 0;
      for (int index3 = num8; index3 < num9; ++index3)
      {
        int num13 = 0;
        for (int index4 = num10; index4 < num11; ++index4)
        {
          if (!Game1.tile[index3, index4].active || (int) Game1.tile[index3, index4].type != type)
          {
            flag = true;
            break;
          }
          if ((int) Game1.tile[index3, index4].frameX / 18 != num12 + num7 * 2 || (int) Game1.tile[index3, index4].frameY / 18 != num13)
          {
            flag = true;
            break;
          }
          ++num13;
        }
        ++num12;
      }
      if (!flag)
      {
        if (Game1.tile[x1, y1 + 2].active && Game1.tileSolid[(int) Game1.tile[x1, y1 + 2].type] && Game1.tile[x1 + 1, y1 + 2].active && Game1.tileSolid[(int) Game1.tile[x1 + 1, y1 + 2].type])
          num7 = 0;
        else if (Game1.tile[x1, y1 - 1].active && Game1.tileSolid[(int) Game1.tile[x1, y1 - 1].type] && !Game1.tileSolidTop[(int) Game1.tile[x1, y1 - 1].type] && Game1.tile[x1 + 1, y1 - 1].active && Game1.tileSolid[(int) Game1.tile[x1 + 1, y1 - 1].type] && !Game1.tileSolidTop[(int) Game1.tile[x1 + 1, y1 - 1].type])
          num7 = 1;
        else if (Game1.tile[x1 - 1, y1].active && Game1.tileSolid[(int) Game1.tile[x1 - 1, y1].type] && !Game1.tileSolidTop[(int) Game1.tile[x1 - 1, y1].type] && Game1.tile[x1 - 1, y1 + 1].active && Game1.tileSolid[(int) Game1.tile[x1 - 1, y1 + 1].type] && !Game1.tileSolidTop[(int) Game1.tile[x1 - 1, y1 + 1].type])
          num7 = 2;
        else if (Game1.tile[x1 + 2, y1].active && Game1.tileSolid[(int) Game1.tile[x1 + 2, y1].type] && !Game1.tileSolidTop[(int) Game1.tile[x1 + 2, y1].type] && Game1.tile[x1 + 2, y1 + 1].active && Game1.tileSolid[(int) Game1.tile[x1 + 2, y1 + 1].type] && !Game1.tileSolidTop[(int) Game1.tile[x1 + 2, y1 + 1].type])
          num7 = 3;
        else
          flag = true;
      }
      if (flag)
      {
        WorldGen.destroyObject = true;
        for (int i = num8; i < num9; ++i)
        {
          for (int j = num10; j < num11; ++j)
          {
            if ((int) Game1.tile[i, j].type == type)
              WorldGen.KillTile(i, j);
          }
        }
        Sign.KillSign(x1, y1);
        Item.NewItem(x * 16, y * 16, 32, 32, 171);
        WorldGen.destroyObject = false;
      }
      else
      {
        int num14 = 36 * num7;
        for (int index5 = 0; index5 < 2; ++index5)
        {
          for (int index6 = 0; index6 < 2; ++index6)
          {
            Game1.tile[x1 + index5, y1 + index6].active = true;
            Game1.tile[x1 + index5, y1 + index6].type = (byte) type;
            Game1.tile[x1 + index5, y1 + index6].frameX = (short) (num14 + 18 * index5);
            Game1.tile[x1 + index5, y1 + index6].frameY = (short) (18 * index6);
          }
        }
      }
    }

    public static bool PlaceSign(int x, int y, int type)
    {
      int num1 = x - 2;
      int num2 = x + 3;
      int num3 = y - 2;
      int num4 = y + 3;
      if (num1 < 0 || num2 > Game1.maxTilesX || num3 < 0 || num4 > Game1.maxTilesY)
        return false;
      for (int index1 = num1; index1 < num2; ++index1)
      {
        for (int index2 = num3; index2 < num4; ++index2)
        {
          if (Game1.tile[index1, index2] == null)
            Game1.tile[index1, index2] = new Tile();
        }
      }
      int index3 = x;
      int index4 = y;
      int num5;
      if (Game1.tile[x, y + 1].active && Game1.tileSolid[(int) Game1.tile[x, y + 1].type] && Game1.tile[x + 1, y + 1].active && Game1.tileSolid[(int) Game1.tile[x + 1, y + 1].type])
      {
        --index4;
        num5 = 0;
      }
      else if (Game1.tile[x, y - 1].active && Game1.tileSolid[(int) Game1.tile[x, y - 1].type] && !Game1.tileSolidTop[(int) Game1.tile[x, y - 1].type] && Game1.tile[x + 1, y - 1].active && Game1.tileSolid[(int) Game1.tile[x + 1, y - 1].type] && !Game1.tileSolidTop[(int) Game1.tile[x + 1, y - 1].type])
        num5 = 1;
      else if (Game1.tile[x - 1, y].active && Game1.tileSolid[(int) Game1.tile[x - 1, y].type] && !Game1.tileSolidTop[(int) Game1.tile[x - 1, y].type] && Game1.tile[x - 1, y + 1].active && Game1.tileSolid[(int) Game1.tile[x - 1, y + 1].type] && !Game1.tileSolidTop[(int) Game1.tile[x - 1, y + 1].type])
      {
        num5 = 2;
      }
      else
      {
        if (!Game1.tile[x + 1, y].active || !Game1.tileSolid[(int) Game1.tile[x + 1, y].type] || Game1.tileSolidTop[(int) Game1.tile[x + 1, y].type] || !Game1.tile[x + 1, y + 1].active || !Game1.tileSolid[(int) Game1.tile[x + 1, y + 1].type] || Game1.tileSolidTop[(int) Game1.tile[x + 1, y + 1].type])
          return false;
        --index3;
        num5 = 3;
      }
      if (Game1.tile[index3, index4].active || Game1.tile[index3 + 1, index4].active || Game1.tile[index3, index4 + 1].active || Game1.tile[index3 + 1, index4 + 1].active)
        return false;
      int num6 = 36 * num5;
      for (int index5 = 0; index5 < 2; ++index5)
      {
        for (int index6 = 0; index6 < 2; ++index6)
        {
          Game1.tile[index3 + index5, index4 + index6].active = true;
          Game1.tile[index3 + index5, index4 + index6].type = (byte) type;
          Game1.tile[index3 + index5, index4 + index6].frameX = (short) (num6 + 18 * index5);
          Game1.tile[index3 + index5, index4 + index6].frameY = (short) (18 * index6);
        }
      }
      return true;
    }

    public static void PlaceOnTable1x1(int x, int y, int type)
    {
      if (Game1.tile[x, y] == null)
        Game1.tile[x, y] = new Tile();
      if (Game1.tile[x, y + 1] == null)
        Game1.tile[x, y + 1] = new Tile();
      if (Game1.tile[x, y].active || !Game1.tile[x, y + 1].active || !Game1.tileTable[(int) Game1.tile[x, y + 1].type])
        return;
      Game1.tile[x, y].active = true;
      Game1.tile[x, y].frameX = (short) 0;
      Game1.tile[x, y].frameY = (short) 0;
      Game1.tile[x, y].type = (byte) type;
      if (type == 50)
        Game1.tile[x, y].frameX = (short) (18 * WorldGen.genRand.Next(5));
    }

    public static void Place1x2(int x, int y, int type)
    {
      short num = 0;
      if (type == 20)
        num = (short) (WorldGen.genRand.Next(3) * 18);
      if (Game1.tile[x, y - 1] == null)
        Game1.tile[x, y - 1] = new Tile();
      if (Game1.tile[x, y + 1] == null)
        Game1.tile[x, y + 1] = new Tile();
      if (!Game1.tile[x, y + 1].active || !Game1.tileSolid[(int) Game1.tile[x, y + 1].type] || Game1.tile[x, y - 1].active)
        return;
      Game1.tile[x, y - 1].active = true;
      Game1.tile[x, y - 1].frameY = (short) 0;
      Game1.tile[x, y - 1].frameX = num;
      Game1.tile[x, y - 1].type = (byte) type;
      Game1.tile[x, y].active = true;
      Game1.tile[x, y].frameY = (short) 18;
      Game1.tile[x, y].frameX = num;
      Game1.tile[x, y].type = (byte) type;
    }

    public static void Place1x2Top(int x, int y, int type)
    {
      short num = 0;
      if (Game1.tile[x, y - 1] == null)
        Game1.tile[x, y - 1] = new Tile();
      if (Game1.tile[x, y + 1] == null)
        Game1.tile[x, y + 1] = new Tile();
      if (!Game1.tile[x, y - 1].active || !Game1.tileSolid[(int) Game1.tile[x, y - 1].type] || Game1.tileSolidTop[(int) Game1.tile[x, y - 1].type] || Game1.tile[x, y + 1].active)
        return;
      Game1.tile[x, y].active = true;
      Game1.tile[x, y].frameY = (short) 0;
      Game1.tile[x, y].frameX = num;
      Game1.tile[x, y].type = (byte) type;
      Game1.tile[x, y + 1].active = true;
      Game1.tile[x, y + 1].frameY = (short) 18;
      Game1.tile[x, y + 1].frameX = num;
      Game1.tile[x, y + 1].type = (byte) type;
    }

    public static void Check1x2Top(int x, int j, byte type)
    {
      if (WorldGen.destroyObject)
        return;
      int j1 = j;
      bool flag = true;
      if (Game1.tile[x, j1] == null)
        Game1.tile[x, j1] = new Tile();
      if (Game1.tile[x, j1 + 1] == null)
        Game1.tile[x, j1 + 1] = new Tile();
      if (Game1.tile[x, j1].frameY == (short) 18)
        --j1;
      if (Game1.tile[x, j1] == null)
        Game1.tile[x, j1] = new Tile();
      if (Game1.tile[x, j1].frameY == (short) 0 && Game1.tile[x, j1 + 1].frameY == (short) 18 && (int) Game1.tile[x, j1].type == (int) type && (int) Game1.tile[x, j1 + 1].type == (int) type)
        flag = false;
      if (Game1.tile[x, j1 - 1] == null)
        Game1.tile[x, j1 - 1] = new Tile();
      if (!Game1.tile[x, j1 - 1].active || !Game1.tileSolid[(int) Game1.tile[x, j1 - 1].type] || Game1.tileSolidTop[(int) Game1.tile[x, j1 - 1].type])
        flag = true;
      if (!flag)
        return;
      WorldGen.destroyObject = true;
      if ((int) Game1.tile[x, j1].type == (int) type)
        WorldGen.KillTile(x, j1);
      if ((int) Game1.tile[x, j1 + 1].type == (int) type)
        WorldGen.KillTile(x, j1 + 1);
      if (type == (byte) 42)
        Item.NewItem(x * 16, j1 * 16, 32, 32, 136);
      WorldGen.destroyObject = false;
    }

    public static void Check2x1(int i, int y, byte type)
    {
      if (WorldGen.destroyObject)
        return;
      int i1 = i;
      bool flag = true;
      if (Game1.tile[i1, y] == null)
        Game1.tile[i1, y] = new Tile();
      if (Game1.tile[i1 + 1, y] == null)
        Game1.tile[i1 + 1, y] = new Tile();
      if (Game1.tile[i1, y + 1] == null)
        Game1.tile[i1, y + 1] = new Tile();
      if (Game1.tile[i1 + 1, y + 1] == null)
        Game1.tile[i1 + 1, y + 1] = new Tile();
      if (Game1.tile[i1, y].frameX == (short) 18)
        --i1;
      if (Game1.tile[i1, y].frameX == (short) 0 && Game1.tile[i1 + 1, y].frameX == (short) 18 && (int) Game1.tile[i1, y].type == (int) type && (int) Game1.tile[i1 + 1, y].type == (int) type)
        flag = false;
      if (type == (byte) 29)
      {
        if (!Game1.tile[i1, y + 1].active || !Game1.tileTable[(int) Game1.tile[i1, y + 1].type])
          flag = true;
        if (!Game1.tile[i1 + 1, y + 1].active || !Game1.tileTable[(int) Game1.tile[i1 + 1, y + 1].type])
          flag = true;
      }
      else
      {
        if (!Game1.tile[i1, y + 1].active || !Game1.tileSolid[(int) Game1.tile[i1, y + 1].type])
          flag = true;
        if (!Game1.tile[i1 + 1, y + 1].active || !Game1.tileSolid[(int) Game1.tile[i1 + 1, y + 1].type])
          flag = true;
      }
      if (!flag)
        return;
      WorldGen.destroyObject = true;
      if ((int) Game1.tile[i1, y].type == (int) type)
        WorldGen.KillTile(i1, y);
      if ((int) Game1.tile[i1 + 1, y].type == (int) type)
        WorldGen.KillTile(i1 + 1, y);
      if (type == (byte) 16)
        Item.NewItem(i1 * 16, y * 16, 32, 32, 35);
      if (type == (byte) 18)
        Item.NewItem(i1 * 16, y * 16, 32, 32, 36);
      if (type == (byte) 29)
      {
        Item.NewItem(i1 * 16, y * 16, 32, 32, 87);
        Game1.PlaySound(13, i * 16, y * 16);
      }
      WorldGen.destroyObject = false;
    }

    public static void Place2x1(int x, int y, int type)
    {
      if (Game1.tile[x, y] == null)
        Game1.tile[x, y] = new Tile();
      if (Game1.tile[x + 1, y] == null)
        Game1.tile[x + 1, y] = new Tile();
      if (Game1.tile[x, y + 1] == null)
        Game1.tile[x, y + 1] = new Tile();
      if (Game1.tile[x + 1, y + 1] == null)
        Game1.tile[x + 1, y + 1] = new Tile();
      bool flag = false;
      if (type != 29 && Game1.tile[x, y + 1].active && Game1.tile[x + 1, y + 1].active && Game1.tileSolid[(int) Game1.tile[x, y + 1].type] && Game1.tileSolid[(int) Game1.tile[x + 1, y + 1].type] && !Game1.tile[x, y].active && !Game1.tile[x + 1, y].active)
        flag = true;
      else if (type == 29 && Game1.tile[x, y + 1].active && Game1.tile[x + 1, y + 1].active && Game1.tileTable[(int) Game1.tile[x, y + 1].type] && Game1.tileTable[(int) Game1.tile[x + 1, y + 1].type] && !Game1.tile[x, y].active && !Game1.tile[x + 1, y].active)
        flag = true;
      if (!flag)
        return;
      Game1.tile[x, y].active = true;
      Game1.tile[x, y].frameY = (short) 0;
      Game1.tile[x, y].frameX = (short) 0;
      Game1.tile[x, y].type = (byte) type;
      Game1.tile[x + 1, y].active = true;
      Game1.tile[x + 1, y].frameY = (short) 0;
      Game1.tile[x + 1, y].frameX = (short) 18;
      Game1.tile[x + 1, y].type = (byte) type;
    }

    public static void Check3x2(int i, int j, int type)
    {
      if (WorldGen.destroyObject)
        return;
      bool flag = false;
      int num1 = i;
      int num2 = j;
      int num3 = num1 + (int) Game1.tile[i, j].frameX / 18 * -1;
      int num4 = num2 + (int) Game1.tile[i, j].frameY / 18 * -1;
      for (int index1 = num3; index1 < num3 + 3; ++index1)
      {
        for (int index2 = num4; index2 < num4 + 2; ++index2)
        {
          if (Game1.tile[index1, index2] == null)
            Game1.tile[index1, index2] = new Tile();
          if (!Game1.tile[index1, index2].active || (int) Game1.tile[index1, index2].type != type || (int) Game1.tile[index1, index2].frameX != (index1 - num3) * 18 || (int) Game1.tile[index1, index2].frameY != (index2 - num4) * 18)
            flag = true;
        }
        if (Game1.tile[index1, num4 + 2] == null)
          Game1.tile[index1, num4 + 2] = new Tile();
        if (!Game1.tile[index1, num4 + 2].active || !Game1.tileSolid[(int) Game1.tile[index1, num4 + 2].type])
          flag = true;
      }
      if (!flag)
        return;
      WorldGen.destroyObject = true;
      for (int i1 = num3; i1 < num3 + 3; ++i1)
      {
        for (int j1 = num4; j1 < num4 + 3; ++j1)
        {
          if ((int) Game1.tile[i1, j1].type == type && Game1.tile[i1, j1].active)
            WorldGen.KillTile(i1, j1);
        }
      }
      switch (type)
      {
        case 14:
          Item.NewItem(i * 16, j * 16, 32, 32, 32);
          break;
        case 17:
          Item.NewItem(i * 16, j * 16, 32, 32, 33);
          break;
      }
      WorldGen.destroyObject = false;
    }

    public static void Place3x2(int x, int y, int type)
    {
      bool flag = true;
      for (int index1 = x - 1; index1 < x + 2; ++index1)
      {
        for (int index2 = y - 1; index2 < y + 1; ++index2)
        {
          if (Game1.tile[index1, index2] == null)
            Game1.tile[index1, index2] = new Tile();
          if (Game1.tile[index1, index2].active)
            flag = false;
        }
        if (Game1.tile[index1, y + 1] == null)
          Game1.tile[index1, y + 1] = new Tile();
        if (!Game1.tile[index1, y + 1].active || !Game1.tileSolid[(int) Game1.tile[index1, y + 1].type])
          flag = false;
      }
      if (!flag)
        return;
      Game1.tile[x - 1, y - 1].active = true;
      Game1.tile[x - 1, y - 1].frameY = (short) 0;
      Game1.tile[x - 1, y - 1].frameX = (short) 0;
      Game1.tile[x - 1, y - 1].type = (byte) type;
      Game1.tile[x, y - 1].active = true;
      Game1.tile[x, y - 1].frameY = (short) 0;
      Game1.tile[x, y - 1].frameX = (short) 18;
      Game1.tile[x, y - 1].type = (byte) type;
      Game1.tile[x + 1, y - 1].active = true;
      Game1.tile[x + 1, y - 1].frameY = (short) 0;
      Game1.tile[x + 1, y - 1].frameX = (short) 36;
      Game1.tile[x + 1, y - 1].type = (byte) type;
      Game1.tile[x - 1, y].active = true;
      Game1.tile[x - 1, y].frameY = (short) 18;
      Game1.tile[x - 1, y].frameX = (short) 0;
      Game1.tile[x - 1, y].type = (byte) type;
      Game1.tile[x, y].active = true;
      Game1.tile[x, y].frameY = (short) 18;
      Game1.tile[x, y].frameX = (short) 18;
      Game1.tile[x, y].type = (byte) type;
      Game1.tile[x + 1, y].active = true;
      Game1.tile[x + 1, y].frameY = (short) 18;
      Game1.tile[x + 1, y].frameX = (short) 36;
      Game1.tile[x + 1, y].type = (byte) type;
    }

    public static void Check3x3(int i, int j, int type)
    {
      if (WorldGen.destroyObject)
        return;
      bool flag = false;
      int num1 = i;
      int num2 = j;
      int num3 = num1 + (int) Game1.tile[i, j].frameX / 18 * -1;
      int num4 = num2 + (int) Game1.tile[i, j].frameY / 18 * -1;
      for (int index1 = num3; index1 < num3 + 3; ++index1)
      {
        for (int index2 = num4; index2 < num4 + 3; ++index2)
        {
          if (Game1.tile[index1, index2] == null)
            Game1.tile[index1, index2] = new Tile();
          if (!Game1.tile[index1, index2].active || (int) Game1.tile[index1, index2].type != type || (int) Game1.tile[index1, index2].frameX != (index1 - num3) * 18 || (int) Game1.tile[index1, index2].frameY != (index2 - num4) * 18)
            flag = true;
        }
      }
      if (Game1.tile[num3 + 1, num4 - 1] == null)
        Game1.tile[num3 + 1, num4 - 1] = new Tile();
      if (!Game1.tile[num3 + 1, num4 - 1].active || !Game1.tileSolid[(int) Game1.tile[num3 + 1, num4 - 1].type] || Game1.tileSolidTop[(int) Game1.tile[num3 + 1, num4 - 1].type])
        flag = true;
      if (!flag)
        return;
      WorldGen.destroyObject = true;
      for (int i1 = num3; i1 < num3 + 3; ++i1)
      {
        for (int j1 = num4; j1 < num4 + 3; ++j1)
        {
          if ((int) Game1.tile[i1, j1].type == type && Game1.tile[i1, j1].active)
            WorldGen.KillTile(i1, j1);
        }
      }
      switch (type)
      {
        case 34:
          Item.NewItem(i * 16, j * 16, 32, 32, 106);
          break;
        case 35:
          Item.NewItem(i * 16, j * 16, 32, 32, 107);
          break;
        case 36:
          Item.NewItem(i * 16, j * 16, 32, 32, 108);
          break;
      }
      WorldGen.destroyObject = false;
    }

    public static void Place3x3(int x, int y, int type)
    {
      bool flag = true;
      for (int index1 = x - 1; index1 < x + 2; ++index1)
      {
        for (int index2 = y; index2 < y + 3; ++index2)
        {
          if (Game1.tile[index1, index2] == null)
            Game1.tile[index1, index2] = new Tile();
          if (Game1.tile[index1, index2].active)
            flag = false;
        }
      }
      if (Game1.tile[x, y - 1] == null)
        Game1.tile[x, y - 1] = new Tile();
      if (!Game1.tile[x, y - 1].active || !Game1.tileSolid[(int) Game1.tile[x, y - 1].type] || Game1.tileSolidTop[(int) Game1.tile[x, y - 1].type])
        flag = false;
      if (!flag)
        return;
      Game1.tile[x - 1, y].active = true;
      Game1.tile[x - 1, y].frameY = (short) 0;
      Game1.tile[x - 1, y].frameX = (short) 0;
      Game1.tile[x - 1, y].type = (byte) type;
      Game1.tile[x, y].active = true;
      Game1.tile[x, y].frameY = (short) 0;
      Game1.tile[x, y].frameX = (short) 18;
      Game1.tile[x, y].type = (byte) type;
      Game1.tile[x + 1, y].active = true;
      Game1.tile[x + 1, y].frameY = (short) 0;
      Game1.tile[x + 1, y].frameX = (short) 36;
      Game1.tile[x + 1, y].type = (byte) type;
      Game1.tile[x - 1, y + 1].active = true;
      Game1.tile[x - 1, y + 1].frameY = (short) 18;
      Game1.tile[x - 1, y + 1].frameX = (short) 0;
      Game1.tile[x - 1, y + 1].type = (byte) type;
      Game1.tile[x, y + 1].active = true;
      Game1.tile[x, y + 1].frameY = (short) 18;
      Game1.tile[x, y + 1].frameX = (short) 18;
      Game1.tile[x, y + 1].type = (byte) type;
      Game1.tile[x + 1, y + 1].active = true;
      Game1.tile[x + 1, y + 1].frameY = (short) 18;
      Game1.tile[x + 1, y + 1].frameX = (short) 36;
      Game1.tile[x + 1, y + 1].type = (byte) type;
      Game1.tile[x - 1, y + 2].active = true;
      Game1.tile[x - 1, y + 2].frameY = (short) 36;
      Game1.tile[x - 1, y + 2].frameX = (short) 0;
      Game1.tile[x - 1, y + 2].type = (byte) type;
      Game1.tile[x, y + 2].active = true;
      Game1.tile[x, y + 2].frameY = (short) 36;
      Game1.tile[x, y + 2].frameX = (short) 18;
      Game1.tile[x, y + 2].type = (byte) type;
      Game1.tile[x + 1, y + 2].active = true;
      Game1.tile[x + 1, y + 2].frameY = (short) 36;
      Game1.tile[x + 1, y + 2].frameX = (short) 36;
      Game1.tile[x + 1, y + 2].type = (byte) type;
    }

    public static void PlaceSunflower(int x, int y, int type = 27)
    {
      if ((double) y > Game1.worldSurface - 1.0)
        return;
      bool flag = true;
      for (int index1 = x; index1 < x + 2; ++index1)
      {
        for (int index2 = y - 3; index2 < y + 1; ++index2)
        {
          if (Game1.tile[index1, index2] == null)
            Game1.tile[index1, index2] = new Tile();
          if (Game1.tile[index1, index2].active || Game1.tile[index1, index2].wall > (byte) 0)
            flag = false;
        }
        if (Game1.tile[index1, y + 1] == null)
          Game1.tile[index1, y + 1] = new Tile();
        if (!Game1.tile[index1, y + 1].active || Game1.tile[index1, y + 1].type != (byte) 2)
          flag = false;
      }
      if (!flag)
        return;
      for (int index3 = 0; index3 < 2; ++index3)
      {
        for (int index4 = -3; index4 < 1; ++index4)
        {
          int num1 = index3 * 18 + WorldGen.genRand.Next(3) * 36;
          int num2 = (index4 + 3) * 18;
          Game1.tile[x + index3, y + index4].active = true;
          Game1.tile[x + index3, y + index4].frameX = (short) num1;
          Game1.tile[x + index3, y + index4].frameY = (short) num2;
          Game1.tile[x + index3, y + index4].type = (byte) type;
        }
      }
    }

    public static void CheckSunflower(int i, int j, int type = 27)
    {
      if (WorldGen.destroyObject)
        return;
      bool flag = false;
      int num1 = 0;
      int num2 = j;
      int num3 = num1 + (int) Game1.tile[i, j].frameX / 18;
      int num4 = num2 + (int) Game1.tile[i, j].frameY / 18 * -1;
      while (num3 > 1)
        num3 -= 2;
      int num5 = num3 * -1 + i;
      for (int index1 = num5; index1 < num5 + 2; ++index1)
      {
        for (int index2 = num4; index2 < num4 + 4; ++index2)
        {
          if (Game1.tile[index1, index2] == null)
            Game1.tile[index1, index2] = new Tile();
          int num6 = (int) Game1.tile[index1, index2].frameX / 18;
          while (num6 > 1)
            num6 -= 2;
          if (!Game1.tile[index1, index2].active || (int) Game1.tile[index1, index2].type != type || num6 != index1 - num5 || (int) Game1.tile[index1, index2].frameY != (index2 - num4) * 18)
            flag = true;
        }
        if (Game1.tile[index1, num4 + 4] == null)
          Game1.tile[index1, num4 + 4] = new Tile();
        if (!Game1.tile[index1, num4 + 4].active || Game1.tile[index1, num4 + 4].type != (byte) 2)
          flag = true;
      }
      if (!flag)
        return;
      WorldGen.destroyObject = true;
      for (int i1 = num5; i1 < num5 + 2; ++i1)
      {
        for (int j1 = num4; j1 < num4 + 4; ++j1)
        {
          if ((int) Game1.tile[i1, j1].type == type && Game1.tile[i1, j1].active)
            WorldGen.KillTile(i1, j1);
        }
      }
      Item.NewItem(i * 16, j * 16, 32, 32, 63);
      WorldGen.destroyObject = false;
    }

    public static bool PlacePot(int x, int y, int type = 28)
    {
      bool flag = true;
      for (int index1 = x; index1 < x + 2; ++index1)
      {
        for (int index2 = y - 1; index2 < y + 1; ++index2)
        {
          if (Game1.tile[index1, index2] == null)
            Game1.tile[index1, index2] = new Tile();
          if (Game1.tile[index1, index2].active)
            flag = false;
        }
        if (Game1.tile[index1, y + 1] == null)
          Game1.tile[index1, y + 1] = new Tile();
        if (!Game1.tile[index1, y + 1].active || !Game1.tileSolid[(int) Game1.tile[index1, y + 1].type])
          flag = false;
      }
      if (!flag)
        return false;
      for (int index3 = 0; index3 < 2; ++index3)
      {
        for (int index4 = -1; index4 < 1; ++index4)
        {
          int num1 = index3 * 18 + WorldGen.genRand.Next(3) * 36;
          int num2 = (index4 + 1) * 18;
          Game1.tile[x + index3, y + index4].active = true;
          Game1.tile[x + index3, y + index4].frameX = (short) num1;
          Game1.tile[x + index3, y + index4].frameY = (short) num2;
          Game1.tile[x + index3, y + index4].type = (byte) type;
        }
      }
      return true;
    }

    public static void CheckPot(int i, int j, int type = 28)
    {
      if (WorldGen.destroyObject)
        return;
      bool flag = false;
      int num1 = 0;
      int num2 = j;
      int num3 = num1 + (int) Game1.tile[i, j].frameX / 18;
      int num4 = num2 + (int) Game1.tile[i, j].frameY / 18 * -1;
      while (num3 > 1)
        num3 -= 2;
      int num5 = num3 * -1 + i;
      for (int index1 = num5; index1 < num5 + 2; ++index1)
      {
        for (int index2 = num4; index2 < num4 + 2; ++index2)
        {
          if (Game1.tile[index1, index2] == null)
            Game1.tile[index1, index2] = new Tile();
          int num6 = (int) Game1.tile[index1, index2].frameX / 18;
          while (num6 > 1)
            num6 -= 2;
          if (!Game1.tile[index1, index2].active || (int) Game1.tile[index1, index2].type != type || num6 != index1 - num5 || (int) Game1.tile[index1, index2].frameY != (index2 - num4) * 18)
            flag = true;
        }
        if (Game1.tile[index1, num4 + 2] == null)
          Game1.tile[index1, num4 + 2] = new Tile();
        if (!Game1.tile[index1, num4 + 2].active || !Game1.tileSolid[(int) Game1.tile[index1, num4 + 2].type])
          flag = true;
      }
      if (!flag)
        return;
      WorldGen.destroyObject = true;
      Game1.PlaySound(13, i * 16, j * 16);
      for (int i1 = num5; i1 < num5 + 2; ++i1)
      {
        for (int j1 = num4; j1 < num4 + 2; ++j1)
        {
          if ((int) Game1.tile[i1, j1].type == type && Game1.tile[i1, j1].active)
            WorldGen.KillTile(i1, j1);
        }
      }
      Gore.NewGore(new Vector2((float) (i * 16), (float) (j * 16)), new Vector2(), 51);
      Gore.NewGore(new Vector2((float) (i * 16), (float) (j * 16)), new Vector2(), 52);
      Gore.NewGore(new Vector2((float) (i * 16), (float) (j * 16)), new Vector2(), 53);
      int num7 = Game1.rand.Next(10);
      if (num7 == 0)
        Item.NewItem(i * 16, j * 16, 16, 16, 58);
      else if (num7 == 0)
        Item.NewItem(i * 16, j * 16, 16, 16, 184);
      else if (num7 == 2)
      {
        int Stack = Game1.rand.Next(3) + 1;
        Item.NewItem(i * 16, j * 16, 16, 16, 8, Stack);
      }
      else if (num7 == 3)
      {
        int Stack = Game1.rand.Next(8) + 3;
        Item.NewItem(i * 16, j * 16, 16, 16, 40, Stack);
      }
      else if (num7 == 4)
      {
        int Stack = Game1.rand.Next(6) + 5;
        Item.NewItem(i * 16, j * 16, 16, 16, 42, Stack);
      }
      else if (num7 == 5)
        Item.NewItem(i * 16, j * 16, 16, 16, 28);
      else if (num7 == 6)
      {
        int Stack = Game1.rand.Next(4) + 1;
        Item.NewItem(i * 16, j * 16, 16, 16, 166, Stack);
      }
      else
      {
        float num8 = (float) (200 + WorldGen.genRand.Next(-100, 101)) * (float) (1.0 + (double) Game1.rand.Next(-20, 21) * 0.0099999997764825821);
        if (Game1.rand.Next(5) == 0)
          num8 *= (float) (1.0 + (double) Game1.rand.Next(5, 11) * 0.0099999997764825821);
        if (Game1.rand.Next(10) == 0)
          num8 *= (float) (1.0 + (double) Game1.rand.Next(10, 21) * 0.0099999997764825821);
        if (Game1.rand.Next(15) == 0)
          num8 *= (float) (1.0 + (double) Game1.rand.Next(20, 41) * 0.0099999997764825821);
        if (Game1.rand.Next(20) == 0)
          num8 *= (float) (1.0 + (double) Game1.rand.Next(40, 81) * 0.0099999997764825821);
        if (Game1.rand.Next(25) == 0)
          num8 *= (float) (1.0 + (double) Game1.rand.Next(50, 101) * 0.0099999997764825821);
        while ((int) num8 > 0)
        {
          if ((double) num8 > 1000000.0)
          {
            int Stack = (int) ((double) num8 / 1000000.0);
            if (Stack > 50 && Game1.rand.Next(2) == 0)
              Stack /= Game1.rand.Next(3) + 1;
            if (Game1.rand.Next(2) == 0)
              Stack /= Game1.rand.Next(3) + 1;
            num8 -= (float) (1000000 * Stack);
            Item.NewItem(i * 16, j * 16, 16, 16, 74, Stack);
          }
          else if ((double) num8 > 10000.0)
          {
            int Stack = (int) ((double) num8 / 10000.0);
            if (Stack > 50 && Game1.rand.Next(2) == 0)
              Stack /= Game1.rand.Next(3) + 1;
            if (Game1.rand.Next(2) == 0)
              Stack /= Game1.rand.Next(3) + 1;
            num8 -= (float) (10000 * Stack);
            Item.NewItem(i * 16, j * 16, 16, 16, 73, Stack);
          }
          else if ((double) num8 > 100.0)
          {
            int Stack = (int) ((double) num8 / 100.0);
            if (Stack > 50 && Game1.rand.Next(2) == 0)
              Stack /= Game1.rand.Next(3) + 1;
            if (Game1.rand.Next(2) == 0)
              Stack /= Game1.rand.Next(3) + 1;
            num8 -= (float) (100 * Stack);
            Item.NewItem(i * 16, j * 16, 16, 16, 72, Stack);
          }
          else
          {
            int Stack = (int) num8;
            if (Stack > 50 && Game1.rand.Next(2) == 0)
              Stack /= Game1.rand.Next(3) + 1;
            if (Game1.rand.Next(2) == 0)
              Stack /= Game1.rand.Next(4) + 1;
            if (Stack < 1)
              Stack = 1;
            num8 -= (float) Stack;
            Item.NewItem(i * 16, j * 16, 16, 16, 71, Stack);
          }
        }
      }
      WorldGen.destroyObject = false;
    }

    public static int PlaceChest(int x, int y, int type = 21)
    {
      bool flag = true;
      int num = -1;
      for (int index1 = x; index1 < x + 2; ++index1)
      {
        for (int index2 = y - 1; index2 < y + 1; ++index2)
        {
          if (Game1.tile[index1, index2] == null)
            Game1.tile[index1, index2] = new Tile();
          if (Game1.tile[index1, index2].active)
            flag = false;
          if (Game1.tile[index1, index2].lava)
            flag = false;
        }
        if (Game1.tile[index1, y + 1] == null)
          Game1.tile[index1, y + 1] = new Tile();
        if (!Game1.tile[index1, y + 1].active || !Game1.tileSolid[(int) Game1.tile[index1, y + 1].type])
          flag = false;
      }
      if (flag)
      {
        num = Chest.CreateChest(x, y - 1);
        if (num == -1)
          flag = false;
      }
      if (flag)
      {
        Game1.tile[x, y - 1].active = true;
        Game1.tile[x, y - 1].frameY = (short) 0;
        Game1.tile[x, y - 1].frameX = (short) 0;
        Game1.tile[x, y - 1].type = (byte) type;
        Game1.tile[x + 1, y - 1].active = true;
        Game1.tile[x + 1, y - 1].frameY = (short) 0;
        Game1.tile[x + 1, y - 1].frameX = (short) 18;
        Game1.tile[x + 1, y - 1].type = (byte) type;
        Game1.tile[x, y].active = true;
        Game1.tile[x, y].frameY = (short) 18;
        Game1.tile[x, y].frameX = (short) 0;
        Game1.tile[x, y].type = (byte) type;
        Game1.tile[x + 1, y].active = true;
        Game1.tile[x + 1, y].frameY = (short) 18;
        Game1.tile[x + 1, y].frameX = (short) 18;
        Game1.tile[x + 1, y].type = (byte) type;
      }
      return num;
    }

    public static void CheckChest(int i, int j, int type)
    {
      if (WorldGen.destroyObject)
        return;
      bool flag = false;
      int num1 = i;
      int num2 = j;
      int num3 = num1 + (int) Game1.tile[i, j].frameX / 18 * -1;
      int num4 = num2 + (int) Game1.tile[i, j].frameY / 18 * -1;
      for (int index1 = num3; index1 < num3 + 2; ++index1)
      {
        for (int index2 = num4; index2 < num4 + 2; ++index2)
        {
          if (Game1.tile[index1, index2] == null)
            Game1.tile[index1, index2] = new Tile();
          if (!Game1.tile[index1, index2].active || (int) Game1.tile[index1, index2].type != type || (int) Game1.tile[index1, index2].frameX != (index1 - num3) * 18 || (int) Game1.tile[index1, index2].frameY != (index2 - num4) * 18)
            flag = true;
        }
        if (Game1.tile[index1, num4 + 2] == null)
          Game1.tile[index1, num4 + 2] = new Tile();
        if (!Game1.tile[index1, num4 + 2].active || !Game1.tileSolid[(int) Game1.tile[index1, num4 + 2].type])
          flag = true;
      }
      if (!flag)
        return;
      WorldGen.destroyObject = true;
      for (int i1 = num3; i1 < num3 + 2; ++i1)
      {
        for (int j1 = num4; j1 < num4 + 3; ++j1)
        {
          if ((int) Game1.tile[i1, j1].type == type && Game1.tile[i1, j1].active)
            WorldGen.KillTile(i1, j1);
        }
      }
      Item.NewItem(i * 16, j * 16, 32, 32, 48);
      WorldGen.destroyObject = false;
    }

    public static bool PlaceTile(int i, int j, int type, bool mute = false, bool forced = false)
    {
      bool flag = false;
      if (i >= 0 && j >= 0 && i < Game1.maxTilesX && j < Game1.maxTilesY)
      {
        if (Game1.tile[i, j] == null)
          Game1.tile[i, j] = new Tile();
        if (forced || Collision.EmptyTile(i, j) || !Game1.tileSolid[type] || type == 23 && Game1.tile[i, j].type == (byte) 0 && Game1.tile[i, j].active || type == 2 && Game1.tile[i, j].type == (byte) 0 && Game1.tile[i, j].active)
        {
          if (type == 23 && (Game1.tile[i, j].type != (byte) 0 || !Game1.tile[i, j].active) || type == 2 && (Game1.tile[i, j].type != (byte) 0 || !Game1.tile[i, j].active) || type == 60 && (Game1.tile[i, j].type != (byte) 59 || !Game1.tile[i, j].active) || Game1.tile[i, j].liquid > (byte) 0 && (type == 3 || type == 4 || type == 20 || type == 24 || type == 27 || type == 32 || type == 51 || type == 61 || type == 69 || type == 72 || type == 73))
            return false;
          Game1.tile[i, j].frameY = (short) 0;
          Game1.tile[i, j].frameX = (short) 0;
          if (type == 3 || type == 24)
          {
            if (j + 1 < Game1.maxTilesY && Game1.tile[i, j + 1].active && (Game1.tile[i, j + 1].type == (byte) 2 && type == 3 || Game1.tile[i, j + 1].type == (byte) 23 && type == 24))
            {
              if (type == 24 && WorldGen.genRand.Next(15) == 0)
              {
                Game1.tile[i, j].active = true;
                Game1.tile[i, j].type = (byte) 32;
                WorldGen.SquareTileFrame(i, j);
              }
              else if (Game1.tile[i, j].wall == (byte) 0 && Game1.tile[i, j + 1].wall == (byte) 0)
              {
                if (WorldGen.genRand.Next(50) == 0)
                {
                  Game1.tile[i, j].active = true;
                  Game1.tile[i, j].type = (byte) type;
                  Game1.tile[i, j].frameX = (short) 144;
                }
                else if (WorldGen.genRand.Next(35) == 0)
                {
                  Game1.tile[i, j].active = true;
                  Game1.tile[i, j].type = (byte) type;
                  Game1.tile[i, j].frameX = (short) (WorldGen.genRand.Next(2) * 18 + 108);
                }
                else
                {
                  Game1.tile[i, j].active = true;
                  Game1.tile[i, j].type = (byte) type;
                  Game1.tile[i, j].frameX = (short) (WorldGen.genRand.Next(6) * 18);
                }
              }
            }
          }
          else
          {
            int num1;
            switch (type)
            {
              case 4:
                if (Game1.tile[i - 1, j] == null)
                  Game1.tile[i - 1, j] = new Tile();
                if (Game1.tile[i + 1, j] == null)
                  Game1.tile[i + 1, j] = new Tile();
                if (Game1.tile[i, j + 1] == null)
                  Game1.tile[i, j + 1] = new Tile();
                if (Game1.tile[i - 1, j].active && (Game1.tileSolid[(int) Game1.tile[i - 1, j].type] || Game1.tile[i - 1, j].type == (byte) 5 && Game1.tile[i - 1, j - 1].type == (byte) 5 && Game1.tile[i - 1, j + 1].type == (byte) 5) || Game1.tile[i + 1, j].active && (Game1.tileSolid[(int) Game1.tile[i + 1, j].type] || Game1.tile[i + 1, j].type == (byte) 5 && Game1.tile[i + 1, j - 1].type == (byte) 5 && Game1.tile[i + 1, j + 1].type == (byte) 5) || Game1.tile[i, j + 1].active && Game1.tileSolid[(int) Game1.tile[i, j + 1].type])
                {
                  Game1.tile[i, j].active = true;
                  Game1.tile[i, j].type = (byte) type;
                  WorldGen.SquareTileFrame(i, j);
                  goto label_83;
                }
                else
                  goto label_83;
              case 10:
                if (Game1.tile[i, j - 1] == null)
                  Game1.tile[i, j - 1] = new Tile();
                if (Game1.tile[i, j - 2] == null)
                  Game1.tile[i, j - 2] = new Tile();
                if (Game1.tile[i, j - 3] == null)
                  Game1.tile[i, j - 3] = new Tile();
                if (Game1.tile[i, j + 1] == null)
                  Game1.tile[i, j + 1] = new Tile();
                if (Game1.tile[i, j + 2] == null)
                  Game1.tile[i, j + 2] = new Tile();
                if (Game1.tile[i, j + 3] == null)
                  Game1.tile[i, j + 3] = new Tile();
                if (!Game1.tile[i, j - 1].active && !Game1.tile[i, j - 2].active && Game1.tile[i, j - 3].active && Game1.tileSolid[(int) Game1.tile[i, j - 3].type])
                {
                  WorldGen.PlaceDoor(i, j - 1, type);
                  WorldGen.SquareTileFrame(i, j);
                  goto label_83;
                }
                else
                {
                  if (Game1.tile[i, j + 1].active || Game1.tile[i, j + 2].active || !Game1.tile[i, j + 3].active || !Game1.tileSolid[(int) Game1.tile[i, j + 3].type])
                    return false;
                  WorldGen.PlaceDoor(i, j + 1, type);
                  WorldGen.SquareTileFrame(i, j);
                  goto label_83;
                }
              case 34:
              case 35:
                num1 = 0;
                break;
              case 61:
                if (j + 1 < Game1.maxTilesY && Game1.tile[i, j + 1].active && Game1.tile[i, j + 1].type == (byte) 60)
                {
                  if (WorldGen.genRand.Next(10) == 0)
                  {
                    Game1.tile[i, j].active = true;
                    Game1.tile[i, j].type = (byte) 69;
                    WorldGen.SquareTileFrame(i, j);
                  }
                  else if (WorldGen.genRand.Next(15) == 0)
                  {
                    Game1.tile[i, j].active = true;
                    Game1.tile[i, j].type = (byte) type;
                    Game1.tile[i, j].frameX = (short) 144;
                  }
                  else
                  {
                    Game1.tile[i, j].active = true;
                    Game1.tile[i, j].type = (byte) type;
                    Game1.tile[i, j].frameX = (short) (WorldGen.genRand.Next(8) * 18);
                  }
                  goto label_83;
                }
                else
                  goto label_83;
              case 71:
                if (j + 1 < Game1.maxTilesY && Game1.tile[i, j + 1].active && Game1.tile[i, j + 1].type == (byte) 70)
                {
                  Game1.tile[i, j].active = true;
                  Game1.tile[i, j].type = (byte) type;
                  Game1.tile[i, j].frameX = (short) (WorldGen.genRand.Next(5) * 18);
                  goto label_83;
                }
                else
                  goto label_83;
              default:
                num1 = type != 36 ? 1 : 0;
                break;
            }
            if (num1 == 0)
            {
              WorldGen.Place3x3(i, j, type);
              WorldGen.SquareTileFrame(i, j);
            }
            else if (type == 13 || type == 33 || type == 49 || type == 50)
            {
              WorldGen.PlaceOnTable1x1(i, j, type);
              WorldGen.SquareTileFrame(i, j);
            }
            else if (type == 14 || type == 26)
            {
              WorldGen.Place3x2(i, j, type);
              WorldGen.SquareTileFrame(i, j);
            }
            else
            {
              int num2;
              switch (type)
              {
                case 15:
                  if (Game1.tile[i, j - 1] == null)
                    Game1.tile[i, j - 1] = new Tile();
                  if (Game1.tile[i, j] == null)
                    Game1.tile[i, j] = new Tile();
                  WorldGen.Place1x2(i, j, type);
                  WorldGen.SquareTileFrame(i, j);
                  goto label_83;
                case 16:
                case 18:
                  num2 = 0;
                  break;
                case 20:
                  if (Game1.tile[i, j + 1] == null)
                    Game1.tile[i, j + 1] = new Tile();
                  if (Game1.tile[i, j + 1].active && Game1.tile[i, j + 1].type == (byte) 2)
                  {
                    WorldGen.Place1x2(i, j, type);
                    WorldGen.SquareTileFrame(i, j);
                    goto label_83;
                  }
                  else
                    goto label_83;
                default:
                  num2 = type != 29 ? 1 : 0;
                  break;
              }
              if (num2 == 0)
              {
                WorldGen.Place2x1(i, j, type);
                WorldGen.SquareTileFrame(i, j);
              }
              else
              {
                switch (type)
                {
                  case 17:
                    WorldGen.Place3x2(i, j, type);
                    WorldGen.SquareTileFrame(i, j);
                    break;
                  case 21:
                    WorldGen.PlaceChest(i, j, type);
                    WorldGen.SquareTileFrame(i, j);
                    break;
                  case 27:
                    WorldGen.PlaceSunflower(i, j);
                    WorldGen.SquareTileFrame(i, j);
                    break;
                  case 28:
                    WorldGen.PlacePot(i, j);
                    WorldGen.SquareTileFrame(i, j);
                    break;
                  case 42:
                    WorldGen.Place1x2Top(i, j, type);
                    WorldGen.SquareTileFrame(i, j);
                    break;
                  case 55:
                    WorldGen.PlaceSign(i, j, type);
                    break;
                  default:
                    Game1.tile[i, j].active = true;
                    Game1.tile[i, j].type = (byte) type;
                    break;
                }
              }
            }
          }
label_83:
          if (Game1.tile[i, j].active && !mute)
          {
            WorldGen.SquareTileFrame(i, j);
            flag = true;
            Game1.PlaySound(0, i * 16, j * 16);
            if (type == 22)
            {
              for (int index = 0; index < 3; ++index)
                Dust.NewDust(new Vector2((float) (i * 16), (float) (j * 16)), 16, 16, 14);
            }
          }
        }
      }
      return flag;
    }

    public static void KillWall(int i, int j, bool fail = false)
    {
      if (i < 0 || j < 0 || i >= Game1.maxTilesX || j >= Game1.maxTilesY)
        return;
      if (Game1.tile[i, j] == null)
        Game1.tile[i, j] = new Tile();
      if (Game1.tile[i, j].wall > (byte) 0)
      {
        WorldGen.genRand.Next(3);
        Game1.PlaySound(0, i * 16, j * 16);
        int num = 10;
        if (fail)
          num = 3;
        for (int index = 0; index < num; ++index)
        {
          int Type = 0;
          if (Game1.tile[i, j].wall == (byte) 1 || Game1.tile[i, j].wall == (byte) 5 || Game1.tile[i, j].wall == (byte) 6 || Game1.tile[i, j].wall == (byte) 7 || Game1.tile[i, j].wall == (byte) 8 || Game1.tile[i, j].wall == (byte) 9)
            Type = 1;
          if (Game1.tile[i, j].wall == (byte) 3)
            Type = WorldGen.genRand.Next(2) != 0 ? 1 : 14;
          if (Game1.tile[i, j].wall == (byte) 4)
            Type = 7;
          if (Game1.tile[i, j].wall == (byte) 12)
            Type = 9;
          if (Game1.tile[i, j].wall == (byte) 10)
            Type = 10;
          if (Game1.tile[i, j].wall == (byte) 11)
            Type = 11;
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
          if (Game1.tile[i, j].wall == (byte) 4)
            Type = 93;
          if (Game1.tile[i, j].wall == (byte) 5)
            Type = 130;
          if (Game1.tile[i, j].wall == (byte) 6)
            Type = 132;
          if (Game1.tile[i, j].wall == (byte) 7)
            Type = 135;
          if (Game1.tile[i, j].wall == (byte) 8)
            Type = 138;
          if (Game1.tile[i, j].wall == (byte) 9)
            Type = 140;
          if (Game1.tile[i, j].wall == (byte) 10)
            Type = 142;
          if (Game1.tile[i, j].wall == (byte) 11)
            Type = 144;
          if (Game1.tile[i, j].wall == (byte) 12)
            Type = 146;
          if (Type > 0)
            Item.NewItem(i * 16, j * 16, 16, 16, Type);
          Game1.tile[i, j].wall = (byte) 0;
          WorldGen.SquareWallFrame(i, j);
        }
      }
    }

    public static void KillTile(int i, int j, bool fail = false, bool effectOnly = false, bool noItem = false)
    {
      if (i < 0 || j < 0 || i >= Game1.maxTilesX || j >= Game1.maxTilesY)
        return;
      if (Game1.tile[i, j] == null)
        Game1.tile[i, j] = new Tile();
      if (Game1.tile[i, j].active)
      {
        if (j >= 1 && Game1.tile[i, j - 1] == null)
          Game1.tile[i, j - 1] = new Tile();
        if (j >= 1 && Game1.tile[i, j - 1].active && (Game1.tile[i, j - 1].type == (byte) 5 && Game1.tile[i, j].type != (byte) 5 || Game1.tile[i, j - 1].type == (byte) 21 && Game1.tile[i, j].type != (byte) 21 || Game1.tile[i, j - 1].type == (byte) 26 && Game1.tile[i, j].type != (byte) 26 || Game1.tile[i, j - 1].type == (byte) 72 && Game1.tile[i, j].type != (byte) 72) && (Game1.tile[i, j - 1].type != (byte) 5 || (Game1.tile[i, j - 1].frameX != (short) 66 || Game1.tile[i, j - 1].frameY < (short) 0 || Game1.tile[i, j - 1].frameY > (short) 44) && (Game1.tile[i, j - 1].frameX != (short) 88 || Game1.tile[i, j - 1].frameY < (short) 66 || Game1.tile[i, j - 1].frameY > (short) 110) && Game1.tile[i, j - 1].frameY < (short) 198))
          return;
        if (!effectOnly && !WorldGen.stopDrops)
        {
          if (Game1.tile[i, j].type == (byte) 3)
          {
            Game1.PlaySound(6, i * 16, j * 16);
            if (Game1.tile[i, j].frameX == (short) 144)
              Item.NewItem(i * 16, j * 16, 16, 16, 5);
          }
          else if (Game1.tile[i, j].type == (byte) 24)
          {
            Game1.PlaySound(6, i * 16, j * 16);
            if (Game1.tile[i, j].frameX == (short) 144)
              Item.NewItem(i * 16, j * 16, 16, 16, 60);
          }
          else if (Game1.tile[i, j].type == (byte) 32 || Game1.tile[i, j].type == (byte) 51 || Game1.tile[i, j].type == (byte) 52 || Game1.tile[i, j].type == (byte) 61 || Game1.tile[i, j].type == (byte) 62 || Game1.tile[i, j].type == (byte) 69 || Game1.tile[i, j].type == (byte) 71 || Game1.tile[i, j].type == (byte) 73 || Game1.tile[i, j].type == (byte) 74)
            Game1.PlaySound(6, i * 16, j * 16);
          else
            Game1.PlaySound(0, i * 16, j * 16);
        }
        int num = 10;
        if (fail)
          num = 3;
        for (int index = 0; index < num; ++index)
        {
          int Type = 0;
          if (Game1.tile[i, j].type == (byte) 0)
            Type = 0;
          if (Game1.tile[i, j].type == (byte) 1 || Game1.tile[i, j].type == (byte) 16 || Game1.tile[i, j].type == (byte) 17 || Game1.tile[i, j].type == (byte) 38 || Game1.tile[i, j].type == (byte) 39 || Game1.tile[i, j].type == (byte) 41 || Game1.tile[i, j].type == (byte) 43 || Game1.tile[i, j].type == (byte) 44 || Game1.tile[i, j].type == (byte) 48 || Game1.tileStone[(int) Game1.tile[i, j].type])
            Type = 1;
          if (Game1.tile[i, j].type == (byte) 4 || Game1.tile[i, j].type == (byte) 33)
            Type = 6;
          if (Game1.tile[i, j].type == (byte) 5 || Game1.tile[i, j].type == (byte) 10 || Game1.tile[i, j].type == (byte) 11 || Game1.tile[i, j].type == (byte) 14 || Game1.tile[i, j].type == (byte) 15 || Game1.tile[i, j].type == (byte) 19 || Game1.tile[i, j].type == (byte) 21 || Game1.tile[i, j].type == (byte) 30)
            Type = 7;
          if (Game1.tile[i, j].type == (byte) 2)
            Type = WorldGen.genRand.Next(2) != 0 ? 2 : 0;
          if (Game1.tile[i, j].type == (byte) 6 || Game1.tile[i, j].type == (byte) 26)
            Type = 8;
          if (Game1.tile[i, j].type == (byte) 7 || Game1.tile[i, j].type == (byte) 34 || Game1.tile[i, j].type == (byte) 47)
            Type = 9;
          if (Game1.tile[i, j].type == (byte) 8 || Game1.tile[i, j].type == (byte) 36 || Game1.tile[i, j].type == (byte) 45)
            Type = 10;
          if (Game1.tile[i, j].type == (byte) 9 || Game1.tile[i, j].type == (byte) 35 || Game1.tile[i, j].type == (byte) 42 || Game1.tile[i, j].type == (byte) 46)
            Type = 11;
          if (Game1.tile[i, j].type == (byte) 12)
            Type = 12;
          if (Game1.tile[i, j].type == (byte) 3 || Game1.tile[i, j].type == (byte) 73)
            Type = 3;
          if (Game1.tile[i, j].type == (byte) 13 || Game1.tile[i, j].type == (byte) 54)
            Type = 13;
          if (Game1.tile[i, j].type == (byte) 22)
            Type = 14;
          if (Game1.tile[i, j].type == (byte) 28)
            Type = 22;
          if (Game1.tile[i, j].type == (byte) 29)
            Type = 23;
          if (Game1.tile[i, j].type == (byte) 40)
            Type = 28;
          if (Game1.tile[i, j].type == (byte) 49)
            Type = 29;
          if (Game1.tile[i, j].type == (byte) 50)
            Type = 22;
          if (Game1.tile[i, j].type == (byte) 51)
            Type = 30;
          if (Game1.tile[i, j].type == (byte) 52)
            Type = 3;
          if (Game1.tile[i, j].type == (byte) 53)
            Type = 32;
          if (Game1.tile[i, j].type == (byte) 56 || Game1.tile[i, j].type == (byte) 75)
            Type = 37;
          if (Game1.tile[i, j].type == (byte) 57)
            Type = 36;
          if (Game1.tile[i, j].type == (byte) 59)
            Type = 38;
          if (Game1.tile[i, j].type == (byte) 61 || Game1.tile[i, j].type == (byte) 62 || Game1.tile[i, j].type == (byte) 74)
            Type = 40;
          if (Game1.tile[i, j].type == (byte) 69)
            Type = 7;
          if (Game1.tile[i, j].type == (byte) 71 || Game1.tile[i, j].type == (byte) 72)
            Type = 26;
          if (Game1.tile[i, j].type == (byte) 70)
            Type = 17;
          if (Game1.tile[i, j].type == (byte) 2)
            Type = WorldGen.genRand.Next(2) != 0 ? 39 : 38;
          if (Game1.tile[i, j].type == (byte) 58)
            Type = WorldGen.genRand.Next(2) != 0 ? 25 : 6;
          if (Game1.tile[i, j].type == (byte) 37)
            Type = WorldGen.genRand.Next(2) != 0 ? 23 : 6;
          if (Game1.tile[i, j].type == (byte) 32)
            Type = WorldGen.genRand.Next(2) != 0 ? 24 : 14;
          if (Game1.tile[i, j].type == (byte) 23 || Game1.tile[i, j].type == (byte) 24)
            Type = WorldGen.genRand.Next(2) != 0 ? 17 : 14;
          if (Game1.tile[i, j].type == (byte) 25 || Game1.tile[i, j].type == (byte) 31)
            Type = WorldGen.genRand.Next(2) != 0 ? 1 : 14;
          if (Game1.tile[i, j].type == (byte) 20)
            Type = WorldGen.genRand.Next(2) != 0 ? 2 : 7;
          if (Game1.tile[i, j].type == (byte) 27)
            Type = WorldGen.genRand.Next(2) != 0 ? 19 : 3;
          if ((Game1.tile[i, j].type == (byte) 34 || Game1.tile[i, j].type == (byte) 35 || Game1.tile[i, j].type == (byte) 36 || Game1.tile[i, j].type == (byte) 42) && Game1.rand.Next(2) == 0)
            Type = 6;
          if (Type >= 0)
            Dust.NewDust(new Vector2((float) (i * 16), (float) (j * 16)), 16, 16, Type);
        }
        if (effectOnly)
          return;
        if (fail)
        {
          if (Game1.tile[i, j].type == (byte) 2 || Game1.tile[i, j].type == (byte) 23)
            Game1.tile[i, j].type = (byte) 0;
          if (Game1.tile[i, j].type == (byte) 60)
            Game1.tile[i, j].type = (byte) 59;
          WorldGen.SquareTileFrame(i, j);
        }
        else
        {
          if (Game1.tile[i, j].type == (byte) 21 && Game1.netMode != 1 && !Chest.DestroyChest(i - (int) Game1.tile[i, j].frameX / 18, j - (int) Game1.tile[i, j].frameY / 18))
            return;
          if (!noItem && !WorldGen.stopDrops)
          {
            int Type = 0;
            if (Game1.tile[i, j].type == (byte) 0 || Game1.tile[i, j].type == (byte) 2)
              Type = 2;
            else if (Game1.tile[i, j].type == (byte) 1)
              Type = 3;
            else if (Game1.tile[i, j].type == (byte) 4)
              Type = 8;
            else if (Game1.tile[i, j].type == (byte) 5)
              Type = Game1.tile[i, j].frameX < (short) 22 || Game1.tile[i, j].frameY < (short) 198 ? 9 : 27;
            else if (Game1.tile[i, j].type == (byte) 6)
              Type = 11;
            else if (Game1.tile[i, j].type == (byte) 7)
              Type = 12;
            else if (Game1.tile[i, j].type == (byte) 8)
              Type = 13;
            else if (Game1.tile[i, j].type == (byte) 9)
              Type = 14;
            else if (Game1.tile[i, j].type == (byte) 13)
            {
              Game1.PlaySound(13, i * 16, j * 16);
              Type = Game1.tile[i, j].frameX != (short) 18 ? (Game1.tile[i, j].frameX != (short) 36 ? 31 : 110) : 28;
            }
            else if (Game1.tile[i, j].type == (byte) 19)
              Type = 94;
            else if (Game1.tile[i, j].type == (byte) 22)
              Type = 56;
            else if (Game1.tile[i, j].type == (byte) 23)
              Type = 2;
            else if (Game1.tile[i, j].type == (byte) 25)
              Type = 61;
            else if (Game1.tile[i, j].type == (byte) 30)
              Type = 9;
            else if (Game1.tile[i, j].type == (byte) 33)
              Type = 105;
            else if (Game1.tile[i, j].type == (byte) 37)
              Type = 116;
            else if (Game1.tile[i, j].type == (byte) 38)
              Type = 129;
            else if (Game1.tile[i, j].type == (byte) 39)
              Type = 131;
            else if (Game1.tile[i, j].type == (byte) 40)
              Type = 133;
            else if (Game1.tile[i, j].type == (byte) 41)
              Type = 134;
            else if (Game1.tile[i, j].type == (byte) 43)
              Type = 137;
            else if (Game1.tile[i, j].type == (byte) 44)
              Type = 139;
            else if (Game1.tile[i, j].type == (byte) 45)
              Type = 141;
            else if (Game1.tile[i, j].type == (byte) 46)
              Type = 143;
            else if (Game1.tile[i, j].type == (byte) 47)
              Type = 145;
            else if (Game1.tile[i, j].type == (byte) 48)
              Type = 147;
            else if (Game1.tile[i, j].type == (byte) 49)
              Type = 148;
            else if (Game1.tile[i, j].type == (byte) 51)
              Type = 150;
            else if (Game1.tile[i, j].type == (byte) 53)
              Type = 169;
            else if (Game1.tile[i, j].type == (byte) 54)
              Game1.PlaySound(13, i * 16, j * 16);
            else if (Game1.tile[i, j].type == (byte) 56)
              Type = 173;
            else if (Game1.tile[i, j].type == (byte) 57)
              Type = 172;
            else if (Game1.tile[i, j].type == (byte) 58)
              Type = 174;
            else if (Game1.tile[i, j].type == (byte) 75)
              Type = 192;
            else if (Game1.tile[i, j].type == (byte) 59 || Game1.tile[i, j].type == (byte) 60)
              Type = 176;
            else if (Game1.tile[i, j].type == (byte) 71 || Game1.tile[i, j].type == (byte) 72)
              Type = 183;
            else if (Game1.tile[i, j].type >= (byte) 63 && Game1.tile[i, j].type <= (byte) 68)
              Type = (int) Game1.tile[i, j].type - 63 + 177;
            else if (Game1.tile[i, j].type == (byte) 50)
              Type = Game1.tile[i, j].frameX != (short) 90 ? 149 : 165;
            if (Type > 0)
              Item.NewItem(i * 16, j * 16, 16, 16, Type);
          }
          Game1.tile[i, j].active = false;
          if (Game1.tileSolid[(int) Game1.tile[i, j].type])
            Game1.tile[i, j].lighted = false;
          Game1.tile[i, j].frameX = (short) -1;
          Game1.tile[i, j].frameY = (short) -1;
          Game1.tile[i, j].frameNumber = (byte) 0;
          Game1.tile[i, j].type = (byte) 0;
          WorldGen.SquareTileFrame(i, j);
        }
      }
    }

    public static bool PlayerLOS(int x, int y)
    {
      Rectangle rectangle1 = new Rectangle(x * 16, y * 16, 16, 16);
      for (int index = 0; index < 8; ++index)
      {
        if (Game1.player[index].active)
        {
          Rectangle rectangle2 = new Rectangle((int) ((double) Game1.player[index].position.X + (double) Game1.player[index].width * 0.5 - (double) Game1.screenWidth * 0.6), (int) ((double) Game1.player[index].position.Y + (double) Game1.player[index].height * 0.5 - (double) Game1.screenHeight * 0.6), (int) ((double) Game1.screenWidth * 1.2), (int) ((double) Game1.screenHeight * 1.2));
          if (rectangle1.Intersects(rectangle2))
            return true;
        }
      }
      return false;
    }

    public static void UpdateWorld()
    {
      ++Liquid.skipCount;
      if (Liquid.skipCount > 1)
      {
        Liquid.UpdateLiquid();
        Liquid.skipCount = 0;
      }
      float num1 = 5E-05f;
      float num2 = 2.5E-05f;
      bool flag1 = false;
      ++WorldGen.spawnDelay;
      if (Game1.invasionType > 0)
        WorldGen.spawnDelay = 0;
      if (WorldGen.spawnDelay >= 10)
      {
        flag1 = true;
        WorldGen.spawnDelay = 0;
        for (int index = 0; index < 1000; ++index)
        {
          if (Game1.npc[index].active && Game1.npc[index].homeless && Game1.npc[index].townNPC)
          {
            WorldGen.spawnNPC = Game1.npc[index].type;
            break;
          }
        }
      }
      for (int index1 = 0; (double) index1 < (double) (Game1.maxTilesX * Game1.maxTilesY) * (double) num1; ++index1)
      {
        int index2 = WorldGen.genRand.Next(10, Game1.maxTilesX - 10);
        int index3 = WorldGen.genRand.Next(10, (int) Game1.worldSurface - 1);
        int num3 = index2 - 1;
        int num4 = index2 + 2;
        int index4 = index3 - 1;
        int num5 = index3 + 2;
        if (num3 < 10)
          num3 = 10;
        if (num4 > Game1.maxTilesX - 10)
          num4 = Game1.maxTilesX - 10;
        if (index4 < 10)
          index4 = 10;
        if (num5 > Game1.maxTilesY - 10)
          num5 = Game1.maxTilesY - 10;
        if (Game1.tile[index2, index3] != null)
        {
          if (Game1.tile[index2, index3].liquid > (byte) 32)
          {
            if (Game1.tile[index2, index3].active && (Game1.tile[index2, index3].type == (byte) 3 || Game1.tile[index2, index3].type == (byte) 20 || Game1.tile[index2, index3].type == (byte) 24 || Game1.tile[index2, index3].type == (byte) 27 || Game1.tile[index2, index3].type == (byte) 73))
            {
              WorldGen.KillTile(index2, index3);
              if (Game1.netMode == 2)
                NetMessage.SendData(17, number2: (float) index2, number3: (float) index3);
            }
          }
          else if (Game1.tile[index2, index3].active)
          {
            if (Game1.tile[index2, index3].type == (byte) 2 || Game1.tile[index2, index3].type == (byte) 23 || Game1.tile[index2, index3].type == (byte) 32)
            {
              int grass = (int) Game1.tile[index2, index3].type;
              if (!Game1.tile[index2, index4].active && WorldGen.genRand.Next(10) == 0 && grass == 2)
              {
                WorldGen.PlaceTile(index2, index4, 3, true);
                if (Game1.netMode == 2 && Game1.tile[index2, index4].active)
                  NetMessage.SendTileSquare(-1, index2, index4, 1);
              }
              if (!Game1.tile[index2, index4].active && WorldGen.genRand.Next(10) == 0 && grass == 23)
              {
                WorldGen.PlaceTile(index2, index4, 24, true);
                if (Game1.netMode == 2 && Game1.tile[index2, index4].active)
                  NetMessage.SendTileSquare(-1, index2, index4, 1);
              }
              bool flag2 = false;
              for (int i = num3; i < num4; ++i)
              {
                for (int j = index4; j < num5; ++j)
                {
                  if ((index2 != i || index3 != j) && Game1.tile[i, j].active)
                  {
                    if (grass == 32)
                      grass = 23;
                    if (Game1.tile[i, j].type == (byte) 0 || grass == 23 && Game1.tile[i, j].type == (byte) 2)
                    {
                      WorldGen.SpreadGrass(i, j, grass: grass, repeat: false);
                      if (grass == 23)
                        WorldGen.SpreadGrass(i, j, 2, grass, false);
                      if ((int) Game1.tile[i, j].type == grass)
                      {
                        WorldGen.SquareTileFrame(i, j);
                        flag2 = true;
                      }
                    }
                  }
                }
              }
              if (Game1.netMode == 2 && flag2)
                NetMessage.SendTileSquare(-1, index2, index3, 3);
            }
            else if (Game1.tile[index2, index3].type == (byte) 20 && !WorldGen.PlayerLOS(index2, index3) && WorldGen.genRand.Next(5) == 0)
              WorldGen.GrowTree(index2, index3);
            if (Game1.tile[index2, index3].type == (byte) 3 && WorldGen.genRand.Next(10) == 0 && Game1.tile[index2, index3].frameX < (short) 144)
            {
              Game1.tile[index2, index3].type = (byte) 73;
              if (Game1.netMode == 2)
                NetMessage.SendTileSquare(-1, index2, index3, 3);
            }
            if (Game1.tile[index2, index3].type == (byte) 32 && WorldGen.genRand.Next(3) == 0)
            {
              int index5 = index2;
              int index6 = index3;
              int num6 = 0;
              if (Game1.tile[index5 + 1, index6].active && Game1.tile[index5 + 1, index6].type == (byte) 32)
                ++num6;
              if (Game1.tile[index5 - 1, index6].active && Game1.tile[index5 - 1, index6].type == (byte) 32)
                ++num6;
              if (Game1.tile[index5, index6 + 1].active && Game1.tile[index5, index6 + 1].type == (byte) 32)
                ++num6;
              if (Game1.tile[index5, index6 - 1].active && Game1.tile[index5, index6 - 1].type == (byte) 32)
                ++num6;
              if (num6 < 3 || Game1.tile[index2, index3].type == (byte) 23)
              {
                switch (WorldGen.genRand.Next(4))
                {
                  case 0:
                    --index6;
                    break;
                  case 1:
                    ++index6;
                    break;
                  case 2:
                    --index5;
                    break;
                  case 3:
                    ++index5;
                    break;
                }
                if (!Game1.tile[index5, index6].active)
                {
                  int num7 = 0;
                  if (Game1.tile[index5 + 1, index6].active && Game1.tile[index5 + 1, index6].type == (byte) 32)
                    ++num7;
                  if (Game1.tile[index5 - 1, index6].active && Game1.tile[index5 - 1, index6].type == (byte) 32)
                    ++num7;
                  if (Game1.tile[index5, index6 + 1].active && Game1.tile[index5, index6 + 1].type == (byte) 32)
                    ++num7;
                  if (Game1.tile[index5, index6 - 1].active && Game1.tile[index5, index6 - 1].type == (byte) 32)
                    ++num7;
                  if (num7 < 2)
                  {
                    int num8 = 7;
                    int num9 = index5 - num8;
                    int num10 = index5 + num8;
                    int num11 = index6 - num8;
                    int num12 = index6 + num8;
                    bool flag3 = false;
                    for (int index7 = num9; index7 < num10; ++index7)
                    {
                      for (int index8 = num11; index8 < num12; ++index8)
                      {
                        if (Math.Abs(index7 - index5) * 2 + Math.Abs(index8 - index6) < 9 && Game1.tile[index7, index8].active && Game1.tile[index7, index8].type == (byte) 23 && Game1.tile[index7, index8 - 1].active && Game1.tile[index7, index8 - 1].type == (byte) 32 && Game1.tile[index7, index8 - 1].liquid == (byte) 0)
                        {
                          flag3 = true;
                          break;
                        }
                      }
                    }
                    if (flag3)
                    {
                      Game1.tile[index5, index6].type = (byte) 32;
                      Game1.tile[index5, index6].active = true;
                      WorldGen.SquareTileFrame(index5, index6);
                      if (Game1.netMode == 2)
                        NetMessage.SendTileSquare(-1, index5, index6, 3);
                    }
                  }
                }
              }
            }
            if ((Game1.tile[index2, index3].type == (byte) 2 || Game1.tile[index2, index3].type == (byte) 52) && WorldGen.genRand.Next(5) == 0 && !Game1.tile[index2, index3 + 1].active && !Game1.tile[index2, index3 + 1].lava)
            {
              bool flag4 = false;
              for (int index9 = index3; index9 > index3 - 10; --index9)
              {
                if (Game1.tile[index2, index9].active && Game1.tile[index2, index9].type == (byte) 2)
                {
                  flag4 = true;
                  break;
                }
              }
              if (flag4)
              {
                int index10 = index2;
                int index11 = index3 + 1;
                Game1.tile[index10, index11].type = (byte) 52;
                Game1.tile[index10, index11].active = true;
                WorldGen.SquareTileFrame(index10, index11);
                if (Game1.netMode == 2)
                  NetMessage.SendTileSquare(-1, index10, index11, 3);
              }
            }
          }
          else if (flag1 && WorldGen.spawnNPC > 0)
            WorldGen.SpawnNPC(index2, index3);
        }
      }
      for (int index12 = 0; (double) index12 < (double) (Game1.maxTilesX * Game1.maxTilesY) * (double) num2; ++index12)
      {
        int index13 = WorldGen.genRand.Next(10, Game1.maxTilesX - 10);
        int index14 = WorldGen.genRand.Next((int) Game1.worldSurface + 10, Game1.maxTilesY - 200);
        int num13 = index13 - 1;
        int num14 = index13 + 2;
        int index15 = index14 - 1;
        int num15 = index14 + 2;
        if (num13 < 10)
          num13 = 10;
        if (num14 > Game1.maxTilesX - 10)
          num14 = Game1.maxTilesX - 10;
        if (index15 < 10)
          index15 = 10;
        if (num15 > Game1.maxTilesY - 10)
          num15 = Game1.maxTilesY - 10;
        if (Game1.tile[index13, index14] != null)
        {
          if (Game1.tile[index13, index14].liquid > (byte) 32)
          {
            if (Game1.tile[index13, index14].active && (Game1.tile[index13, index14].type == (byte) 61 || Game1.tile[index13, index14].type == (byte) 74))
            {
              WorldGen.KillTile(index13, index14);
              if (Game1.netMode == 2)
                NetMessage.SendData(17, number2: (float) index13, number3: (float) index14);
            }
          }
          else if (Game1.tile[index13, index14].active)
          {
            if (Game1.tile[index13, index14].type == (byte) 60)
            {
              int type = (int) Game1.tile[index13, index14].type;
              if (!Game1.tile[index13, index15].active && WorldGen.genRand.Next(10) == 0)
              {
                WorldGen.PlaceTile(index13, index15, 61, true);
                if (Game1.netMode == 2 && Game1.tile[index13, index15].active)
                  NetMessage.SendTileSquare(-1, index13, index15, 1);
              }
              bool flag5 = false;
              for (int i = num13; i < num14; ++i)
              {
                for (int j = index15; j < num15; ++j)
                {
                  if ((index13 != i || index14 != j) && Game1.tile[i, j].active && Game1.tile[i, j].type == (byte) 59)
                  {
                    WorldGen.SpreadGrass(i, j, 59, type, false);
                    if ((int) Game1.tile[i, j].type == type)
                    {
                      WorldGen.SquareTileFrame(i, j);
                      flag5 = true;
                    }
                  }
                }
              }
              if (Game1.netMode == 2 && flag5)
                NetMessage.SendTileSquare(-1, index13, index14, 3);
            }
            if (Game1.tile[index13, index14].type == (byte) 61 && WorldGen.genRand.Next(3) == 0 && Game1.tile[index13, index14].frameX < (short) 144)
            {
              Game1.tile[index13, index14].type = (byte) 74;
              if (Game1.netMode == 2)
                NetMessage.SendTileSquare(-1, index13, index14, 3);
            }
            if ((Game1.tile[index13, index14].type == (byte) 60 || Game1.tile[index13, index14].type == (byte) 62) && WorldGen.genRand.Next(5) == 0 && !Game1.tile[index13, index14 + 1].active && !Game1.tile[index13, index14 + 1].lava)
            {
              bool flag6 = false;
              for (int index16 = index14; index16 > index14 - 10; --index16)
              {
                if (Game1.tile[index13, index16].active && Game1.tile[index13, index16].type == (byte) 60)
                {
                  flag6 = true;
                  break;
                }
              }
              if (flag6)
              {
                int index17 = index13;
                int index18 = index14 + 1;
                Game1.tile[index17, index18].type = (byte) 62;
                Game1.tile[index17, index18].active = true;
                WorldGen.SquareTileFrame(index17, index18);
                if (Game1.netMode == 2)
                  NetMessage.SendTileSquare(-1, index17, index18, 3);
              }
            }
            if (Game1.tile[index13, index14].type == (byte) 69 && WorldGen.genRand.Next(3) == 0)
            {
              int index19 = index13;
              int index20 = index14;
              int num16 = 0;
              if (Game1.tile[index19 + 1, index20].active && Game1.tile[index19 + 1, index20].type == (byte) 69)
                ++num16;
              if (Game1.tile[index19 - 1, index20].active && Game1.tile[index19 - 1, index20].type == (byte) 69)
                ++num16;
              if (Game1.tile[index19, index20 + 1].active && Game1.tile[index19, index20 + 1].type == (byte) 69)
                ++num16;
              if (Game1.tile[index19, index20 - 1].active && Game1.tile[index19, index20 - 1].type == (byte) 69)
                ++num16;
              if (num16 < 3 || Game1.tile[index13, index14].type == (byte) 60)
              {
                switch (WorldGen.genRand.Next(4))
                {
                  case 0:
                    --index20;
                    break;
                  case 1:
                    ++index20;
                    break;
                  case 2:
                    --index19;
                    break;
                  case 3:
                    ++index19;
                    break;
                }
                if (!Game1.tile[index19, index20].active)
                {
                  int num17 = 0;
                  if (Game1.tile[index19 + 1, index20].active && Game1.tile[index19 + 1, index20].type == (byte) 69)
                    ++num17;
                  if (Game1.tile[index19 - 1, index20].active && Game1.tile[index19 - 1, index20].type == (byte) 69)
                    ++num17;
                  if (Game1.tile[index19, index20 + 1].active && Game1.tile[index19, index20 + 1].type == (byte) 69)
                    ++num17;
                  if (Game1.tile[index19, index20 - 1].active && Game1.tile[index19, index20 - 1].type == (byte) 69)
                    ++num17;
                  if (num17 < 2)
                  {
                    int num18 = 7;
                    int num19 = index19 - num18;
                    int num20 = index19 + num18;
                    int num21 = index20 - num18;
                    int num22 = index20 + num18;
                    bool flag7 = false;
                    for (int index21 = num19; index21 < num20; ++index21)
                    {
                      for (int index22 = num21; index22 < num22; ++index22)
                      {
                        if (Math.Abs(index21 - index19) * 2 + Math.Abs(index22 - index20) < 9 && Game1.tile[index21, index22].active && Game1.tile[index21, index22].type == (byte) 60 && Game1.tile[index21, index22 - 1].active && Game1.tile[index21, index22 - 1].type == (byte) 69 && Game1.tile[index21, index22 - 1].liquid == (byte) 0)
                        {
                          flag7 = true;
                          break;
                        }
                      }
                    }
                    if (flag7)
                    {
                      Game1.tile[index19, index20].type = (byte) 69;
                      Game1.tile[index19, index20].active = true;
                      WorldGen.SquareTileFrame(index19, index20);
                      if (Game1.netMode == 2)
                        NetMessage.SendTileSquare(-1, index19, index20, 3);
                    }
                  }
                }
              }
            }
            if (Game1.tile[index13, index14].type == (byte) 70)
            {
              int type = (int) Game1.tile[index13, index14].type;
              if (!Game1.tile[index13, index15].active && WorldGen.genRand.Next(10) == 0)
              {
                WorldGen.PlaceTile(index13, index15, 71, true);
                if (Game1.netMode == 2 && Game1.tile[index13, index15].active)
                  NetMessage.SendTileSquare(-1, index13, index15, 1);
              }
              bool flag8 = false;
              for (int i = num13; i < num14; ++i)
              {
                for (int j = index15; j < num15; ++j)
                {
                  if ((index13 != i || index14 != j) && Game1.tile[i, j].active && Game1.tile[i, j].type == (byte) 59)
                  {
                    WorldGen.SpreadGrass(i, j, 59, type, false);
                    if ((int) Game1.tile[i, j].type == type)
                    {
                      WorldGen.SquareTileFrame(i, j);
                      flag8 = true;
                    }
                  }
                }
              }
              if (Game1.netMode == 2 && flag8)
                NetMessage.SendTileSquare(-1, index13, index14, 3);
            }
          }
        }
      }
      if (Game1.dayTime)
        return;
      float num23 = (float) (4200 / Game1.maxTilesX);
      if ((double) Game1.rand.Next(10000) < 10.0 * (double) num23)
      {
        int num24 = 12;
        Vector2 vector2 = new Vector2((float) ((Game1.rand.Next(Game1.maxTilesX - 50) + 100) * 16), (float) (Game1.rand.Next((int) ((double) Game1.maxTilesY * 0.05)) * 16));
        float num25 = (float) Game1.rand.Next(-100, 101);
        float num26 = (float) (Game1.rand.Next(200) + 100);
        float num27 = (float) Math.Sqrt((double) num25 * (double) num25 + (double) num26 * (double) num26);
        float num28 = (float) num24 / num27;
        float SpeedX = num25 * num28;
        float SpeedY = num26 * num28;
        Projectile.NewProjectile(vector2.X, vector2.Y, SpeedX, SpeedY, 12, 1000, 10f, Game1.myPlayer);
      }
    }

    public static void PlaceWall(int i, int j, int type, bool mute = false)
    {
      if (Game1.tile[i, j] == null)
        Game1.tile[i, j] = new Tile();
      if ((int) Game1.tile[i, j].wall == type)
        return;
      for (int index1 = i - 1; index1 < i + 2; ++index1)
      {
        for (int index2 = j - 1; index2 < j + 2; ++index2)
        {
          if (Game1.tile[index1, index2] == null)
            Game1.tile[index1, index2] = new Tile();
          if (Game1.tile[index1, index2].wall > (byte) 0 && (int) Game1.tile[index1, index2].wall != type)
            return;
        }
      }
      Game1.tile[i, j].wall = (byte) type;
      WorldGen.SquareWallFrame(i, j);
      if (!mute)
        Game1.PlaySound(0, i * 16, j * 16);
    }

    public static void AddPlants()
    {
      for (int i = 0; i < Game1.maxTilesX; ++i)
      {
        for (int index = 1; index < Game1.maxTilesY; ++index)
        {
          if (Game1.tile[i, index].type == (byte) 2 && Game1.tile[i, index].active)
          {
            if (!Game1.tile[i, index - 1].active)
              WorldGen.PlaceTile(i, index - 1, 3, true);
          }
          else if (Game1.tile[i, index].type == (byte) 23 && Game1.tile[i, index].active && !Game1.tile[i, index - 1].active)
            WorldGen.PlaceTile(i, index - 1, 24, true);
        }
      }
    }

    public static void SpreadGrass(int i, int j, int dirt = 0, int grass = 2, bool repeat = true)
    {
      if ((int) Game1.tile[i, j].type != dirt || !Game1.tile[i, j].active || (double) j >= Game1.worldSurface && dirt != 59)
        return;
      int num1 = i - 1;
      int num2 = i + 2;
      int num3 = j - 1;
      int num4 = j + 2;
      if (num1 < 0)
        num1 = 0;
      if (num2 > Game1.maxTilesX)
        num2 = Game1.maxTilesX;
      if (num3 < 0)
        num3 = 0;
      if (num4 > Game1.maxTilesY)
        num4 = Game1.maxTilesY;
      bool flag = true;
      for (int index1 = num1; index1 < num2; ++index1)
      {
        for (int index2 = num3; index2 < num4; ++index2)
        {
          if (!Game1.tile[index1, index2].active || !Game1.tileSolid[(int) Game1.tile[index1, index2].type])
          {
            flag = false;
            break;
          }
        }
      }
      if (flag || grass == 23 && Game1.tile[i, j - 1].type == (byte) 27)
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

    public static void ChasmRunner(int i, int j, int steps, bool makeOrb = false)
    {
      bool flag = false;
      if (!makeOrb)
        flag = true;
      float num1 = (float) steps;
      Vector2 vector2_1;
      vector2_1.X = (float) i;
      vector2_1.Y = (float) j;
      Vector2 vector2_2;
      vector2_2.X = (float) WorldGen.genRand.Next(-10, 11) * 0.1f;
      vector2_2.Y = (float) ((double) WorldGen.genRand.Next(11) * 0.20000000298023224 + 0.5);
      int num2 = 5;
      double num3 = (double) (WorldGen.genRand.Next(5) + 7);
      while (num3 > 0.0)
      {
        if ((double) num1 > 0.0)
        {
          num3 = num3 + (double) WorldGen.genRand.Next(3) - (double) WorldGen.genRand.Next(3);
          if (num3 < 7.0)
            num3 = 7.0;
          if (num3 > 20.0)
            num3 = 20.0;
          if ((double) num1 == 1.0 && num3 < 10.0)
            num3 = 10.0;
        }
        else
          num3 -= (double) WorldGen.genRand.Next(4);
        if ((double) vector2_1.Y > Game1.rockLayer && (double) num1 > 0.0)
          num1 = 0.0f;
        --num1;
        if ((double) num1 > (double) num2)
        {
          int num4 = (int) ((double) vector2_1.X - num3 * 0.5);
          int num5 = (int) ((double) vector2_1.X + num3 * 0.5);
          int num6 = (int) ((double) vector2_1.Y - num3 * 0.5);
          int num7 = (int) ((double) vector2_1.Y + num3 * 0.5);
          if (num4 < 0)
            num4 = 0;
          if (num5 > Game1.maxTilesX - 1)
            num5 = Game1.maxTilesX - 1;
          if (num6 < 0)
            num6 = 0;
          if (num7 > Game1.maxTilesY)
            num7 = Game1.maxTilesY;
          for (int index1 = num4; index1 < num5; ++index1)
          {
            for (int index2 = num6; index2 < num7; ++index2)
            {
              if ((double) Math.Abs((float) index1 - vector2_1.X) + (double) Math.Abs((float) index2 - vector2_1.Y) < num3 * 0.5 * (1.0 + (double) WorldGen.genRand.Next(-10, 11) * 0.015))
                Game1.tile[index1, index2].active = false;
            }
          }
        }
        if (!flag && (double) num1 <= 0.0)
        {
          flag = true;
          WorldGen.AddShadowOrb((int) vector2_1.X, (int) vector2_1.Y);
        }
        vector2_1 += vector2_2;
        vector2_2.X += (float) WorldGen.genRand.Next(-10, 11) * 0.01f;
        if ((double) vector2_2.X > 0.3)
          vector2_2.X = 0.3f;
        if ((double) vector2_2.X < -0.3)
          vector2_2.X = -0.3f;
        int num8 = (int) ((double) vector2_1.X - num3 * 1.1);
        int num9 = (int) ((double) vector2_1.X + num3 * 1.1);
        int num10 = (int) ((double) vector2_1.Y - num3 * 1.1);
        int num11 = (int) ((double) vector2_1.Y + num3 * 1.1);
        if (num8 < 1)
          num8 = 1;
        if (num9 > Game1.maxTilesX - 1)
          num9 = Game1.maxTilesX - 1;
        if (num10 < 0)
          num10 = 0;
        if (num11 > Game1.maxTilesY)
          num11 = Game1.maxTilesY;
        for (int index3 = num8; index3 < num9; ++index3)
        {
          for (int index4 = num10; index4 < num11; ++index4)
          {
            if ((double) Math.Abs((float) index3 - vector2_1.X) + (double) Math.Abs((float) index4 - vector2_1.Y) < num3 * 1.1 * (1.0 + (double) WorldGen.genRand.Next(-10, 11) * 0.015))
            {
              if (Game1.tile[index3, index4].type != (byte) 25 && index4 > j + WorldGen.genRand.Next(3, 20))
                Game1.tile[index3, index4].active = true;
              if (steps <= num2)
                Game1.tile[index3, index4].active = true;
              if (Game1.tile[index3, index4].type != (byte) 31)
                Game1.tile[index3, index4].type = (byte) 25;
              if (Game1.tile[index3, index4].wall == (byte) 2)
                Game1.tile[index3, index4].wall = (byte) 0;
            }
          }
        }
        for (int i1 = num8; i1 < num9; ++i1)
        {
          for (int j1 = num10; j1 < num11; ++j1)
          {
            if ((double) Math.Abs((float) i1 - vector2_1.X) + (double) Math.Abs((float) j1 - vector2_1.Y) < num3 * 1.1 * (1.0 + (double) WorldGen.genRand.Next(-10, 11) * 0.015))
            {
              if (Game1.tile[i1, j1].type != (byte) 31)
                Game1.tile[i1, j1].type = (byte) 25;
              if (steps <= num2)
                Game1.tile[i1, j1].active = true;
              if (j1 > j + WorldGen.genRand.Next(3, 20))
                WorldGen.PlaceWall(i1, j1, 3, true);
            }
          }
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
      float speedY = 0.0f,
      bool noYChange = false,
      bool overRide = true)
    {
      double num1 = strength;
      float num2 = (float) steps;
      Vector2 vector2_1;
      vector2_1.X = (float) i;
      vector2_1.Y = (float) j;
      Vector2 vector2_2;
      vector2_2.X = (float) WorldGen.genRand.Next(-10, 11) * 0.1f;
      vector2_2.Y = (float) WorldGen.genRand.Next(-10, 11) * 0.1f;
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
        if (num4 > Game1.maxTilesX)
          num4 = Game1.maxTilesX;
        if (num5 < 0)
          num5 = 0;
        if (num6 > Game1.maxTilesY)
          num6 = Game1.maxTilesY;
        for (int index1 = num3; index1 < num4; ++index1)
        {
          for (int index2 = num5; index2 < num6; ++index2)
          {
            if ((double) Math.Abs((float) index1 - vector2_1.X) + (double) Math.Abs((float) index2 - vector2_1.Y) < strength * 0.5 * (1.0 + (double) WorldGen.genRand.Next(-10, 11) * 0.015))
            {
              if (type < 0)
              {
                if (type == -2 && Game1.tile[index1, index2].active && (index2 < WorldGen.waterLine || index2 > WorldGen.lavaLine))
                {
                  Game1.tile[index1, index2].liquid = byte.MaxValue;
                  if (index2 > WorldGen.lavaLine)
                    Game1.tile[index1, index2].lava = true;
                }
                Game1.tile[index1, index2].active = false;
              }
              else
              {
                if ((overRide || !Game1.tile[index1, index2].active) && (type != 40 || Game1.tile[index1, index2].type != (byte) 53) && (!Game1.tileStone[type] || Game1.tile[index1, index2].type == (byte) 1))
                  Game1.tile[index1, index2].type = (byte) type;
                if (addTile)
                {
                  Game1.tile[index1, index2].active = true;
                  Game1.tile[index1, index2].liquid = (byte) 0;
                  Game1.tile[index1, index2].lava = false;
                }
                if (noYChange && (double) index2 < Game1.worldSurface)
                  Game1.tile[index1, index2].wall = (byte) 2;
                if (type == 59 && index2 > WorldGen.waterLine && Game1.tile[index1, index2].liquid > (byte) 0)
                {
                  Game1.tile[index1, index2].lava = false;
                  Game1.tile[index1, index2].liquid = (byte) 0;
                }
              }
            }
          }
        }
        vector2_1 += vector2_2;
        if (num1 > 50.0)
        {
          vector2_1 += vector2_2;
          --num2;
          vector2_2.Y += (float) WorldGen.genRand.Next(-10, 11) * 0.05f;
          vector2_2.X += (float) WorldGen.genRand.Next(-10, 11) * 0.05f;
          if (num1 > 100.0)
          {
            vector2_1 += vector2_2;
            --num2;
            vector2_2.Y += (float) WorldGen.genRand.Next(-10, 11) * 0.05f;
            vector2_2.X += (float) WorldGen.genRand.Next(-10, 11) * 0.05f;
            if (num1 > 150.0)
            {
              vector2_1 += vector2_2;
              --num2;
              vector2_2.Y += (float) WorldGen.genRand.Next(-10, 11) * 0.05f;
              vector2_2.X += (float) WorldGen.genRand.Next(-10, 11) * 0.05f;
              if (num1 > 200.0)
              {
                vector2_1 += vector2_2;
                --num2;
                vector2_2.Y += (float) WorldGen.genRand.Next(-10, 11) * 0.05f;
                vector2_2.X += (float) WorldGen.genRand.Next(-10, 11) * 0.05f;
                if (num1 > 250.0)
                {
                  vector2_1 += vector2_2;
                  --num2;
                  vector2_2.Y += (float) WorldGen.genRand.Next(-10, 11) * 0.05f;
                  vector2_2.X += (float) WorldGen.genRand.Next(-10, 11) * 0.05f;
                  if (num1 > 300.0)
                  {
                    vector2_1 += vector2_2;
                    --num2;
                    vector2_2.Y += (float) WorldGen.genRand.Next(-10, 11) * 0.05f;
                    vector2_2.X += (float) WorldGen.genRand.Next(-10, 11) * 0.05f;
                    if (num1 > 400.0)
                    {
                      vector2_1 += vector2_2;
                      --num2;
                      vector2_2.Y += (float) WorldGen.genRand.Next(-10, 11) * 0.05f;
                      vector2_2.X += (float) WorldGen.genRand.Next(-10, 11) * 0.05f;
                      if (num1 > 500.0)
                      {
                        vector2_1 += vector2_2;
                        --num2;
                        vector2_2.Y += (float) WorldGen.genRand.Next(-10, 11) * 0.05f;
                        vector2_2.X += (float) WorldGen.genRand.Next(-10, 11) * 0.05f;
                        if (num1 > 600.0)
                        {
                          vector2_1 += vector2_2;
                          --num2;
                          vector2_2.Y += (float) WorldGen.genRand.Next(-10, 11) * 0.05f;
                          vector2_2.X += (float) WorldGen.genRand.Next(-10, 11) * 0.05f;
                          if (num1 > 700.0)
                          {
                            vector2_1 += vector2_2;
                            --num2;
                            vector2_2.Y += (float) WorldGen.genRand.Next(-10, 11) * 0.05f;
                            vector2_2.X += (float) WorldGen.genRand.Next(-10, 11) * 0.05f;
                            if (num1 > 800.0)
                            {
                              vector2_1 += vector2_2;
                              --num2;
                              vector2_2.Y += (float) WorldGen.genRand.Next(-10, 11) * 0.05f;
                              vector2_2.X += (float) WorldGen.genRand.Next(-10, 11) * 0.05f;
                              if (num1 > 900.0)
                              {
                                vector2_1 += vector2_2;
                                --num2;
                                vector2_2.Y += (float) WorldGen.genRand.Next(-10, 11) * 0.05f;
                                vector2_2.X += (float) WorldGen.genRand.Next(-10, 11) * 0.05f;
                              }
                            }
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
        vector2_2.X += (float) WorldGen.genRand.Next(-10, 11) * 0.05f;
        if ((double) vector2_2.X > 1.0)
          vector2_2.X = 1f;
        if ((double) vector2_2.X < -1.0)
          vector2_2.X = -1f;
        if (!noYChange)
        {
          vector2_2.Y += (float) WorldGen.genRand.Next(-10, 11) * 0.05f;
          if ((double) vector2_2.Y > 1.0)
            vector2_2.Y = 1f;
          if ((double) vector2_2.Y < -1.0)
            vector2_2.Y = -1f;
        }
        else if (num1 < 3.0)
        {
          if ((double) vector2_2.Y > 1.0)
            vector2_2.Y = 1f;
          if ((double) vector2_2.Y < -1.0)
            vector2_2.Y = -1f;
        }
        if (type == 59)
        {
          if ((double) vector2_2.Y > 0.5)
            vector2_2.Y = 0.5f;
          if ((double) vector2_2.Y < -0.5)
            vector2_2.Y = -0.5f;
          if ((double) vector2_1.Y < Game1.rockLayer + 100.0)
            vector2_2.Y = 1f;
          if ((double) vector2_1.Y > (double) (Game1.maxTilesY - 300))
            vector2_2.Y = -1f;
        }
      }
    }

    public static void FloatingIsland(int i, int j)
    {
      double num1 = (double) WorldGen.genRand.Next(80, 120);
      float num2 = (float) WorldGen.genRand.Next(20, 25);
      Vector2 vector2_1;
      vector2_1.X = (float) i;
      vector2_1.Y = (float) j;
      Vector2 vector2_2;
      vector2_2.X = (float) WorldGen.genRand.Next(-20, 21) * 0.2f;
      while ((double) vector2_2.X > -2.0 && (double) vector2_2.X < 2.0)
        vector2_2.X = (float) WorldGen.genRand.Next(-20, 21) * 0.2f;
      vector2_2.Y = (float) WorldGen.genRand.Next(-20, -10) * 0.02f;
      while (num1 > 0.0 && (double) num2 > 0.0)
      {
        num1 -= (double) WorldGen.genRand.Next(4);
        --num2;
        int num3 = (int) ((double) vector2_1.X - num1 * 0.5);
        int num4 = (int) ((double) vector2_1.X + num1 * 0.5);
        int num5 = (int) ((double) vector2_1.Y - num1 * 0.5);
        int num6 = (int) ((double) vector2_1.Y + num1 * 0.5);
        if (num3 < 0)
          num3 = 0;
        if (num4 > Game1.maxTilesX)
          num4 = Game1.maxTilesX;
        if (num5 < 0)
          num5 = 0;
        if (num6 > Game1.maxTilesY)
          num6 = Game1.maxTilesY;
        double num7 = num1 * (double) WorldGen.genRand.Next(80, 120) * 0.01;
        float num8 = vector2_1.Y + 1f;
        for (int index1 = num3; index1 < num4; ++index1)
        {
          if (WorldGen.genRand.Next(2) == 0)
            num8 += (float) WorldGen.genRand.Next(-1, 2);
          if ((double) num8 < (double) vector2_1.Y)
            num8 = vector2_1.Y;
          if ((double) num8 > (double) vector2_1.Y + 2.0)
            num8 = vector2_1.Y + 2f;
          for (int index2 = num5; index2 < num6; ++index2)
          {
            if ((double) index2 > (double) num8)
            {
              float num9 = Math.Abs((float) index1 - vector2_1.X);
              float num10 = Math.Abs((float) index2 - vector2_1.Y) * 2f;
              if (Math.Sqrt((double) num9 * (double) num9 + (double) num10 * (double) num10) < num7 * 0.4)
                Game1.tile[index1, index2].active = true;
            }
          }
        }
        WorldGen.TileRunner(WorldGen.genRand.Next(num3 + 10, num4 - 10), (int) ((double) vector2_1.Y + num7 * 0.1 + 5.0), (double) WorldGen.genRand.Next(5, 10), WorldGen.genRand.Next(10, 15), 0, true, speedY: 2f, noYChange: true);
        int num11 = (int) ((double) vector2_1.X - num1 * 0.4);
        int num12 = (int) ((double) vector2_1.X + num1 * 0.4);
        int num13 = (int) ((double) vector2_1.Y - num1 * 0.4);
        int num14 = (int) ((double) vector2_1.Y + num1 * 0.4);
        if (num11 < 0)
          num11 = 0;
        if (num12 > Game1.maxTilesX)
          num12 = Game1.maxTilesX;
        if (num13 < 0)
          num13 = 0;
        if (num14 > Game1.maxTilesY)
          num14 = Game1.maxTilesY;
        double num15 = num1 * (double) WorldGen.genRand.Next(80, 120) * 0.01;
        for (int index3 = num11; index3 < num12; ++index3)
        {
          for (int index4 = num13; index4 < num14; ++index4)
          {
            if ((double) index4 > (double) vector2_1.Y + 2.0)
            {
              float num16 = Math.Abs((float) index3 - vector2_1.X);
              float num17 = Math.Abs((float) index4 - vector2_1.Y) * 2f;
              if (Math.Sqrt((double) num16 * (double) num16 + (double) num17 * (double) num17) < num15 * 0.4)
                Game1.tile[index3, index4].wall = (byte) 2;
            }
          }
        }
        vector2_1 += vector2_2;
        vector2_2.Y += (float) WorldGen.genRand.Next(-10, 11) * 0.05f;
        if ((double) vector2_2.X > 1.0)
          vector2_2.X = 1f;
        if ((double) vector2_2.X < -1.0)
          vector2_2.X = -1f;
        if ((double) vector2_2.Y > 0.2)
          vector2_2.Y = -0.2f;
        if ((double) vector2_2.Y < -0.2)
          vector2_2.Y = -0.2f;
      }
    }

    public static void IslandHouse(int i, int j)
    {
      byte num1 = (byte) WorldGen.genRand.Next(45, 48);
      byte num2 = (byte) WorldGen.genRand.Next(10, 13);
      Vector2 vector2 = new Vector2((float) i, (float) j);
      int num3 = 1;
      if (WorldGen.genRand.Next(2) == 0)
        num3 = -1;
      int num4 = WorldGen.genRand.Next(7, 12);
      int num5 = WorldGen.genRand.Next(5, 7);
      vector2.X = (float) (i + (num4 + 2) * num3);
      for (int index = j - 15; index < j + 30; ++index)
      {
        if (Game1.tile[(int) vector2.X, index].active)
        {
          vector2.Y = (float) (index - 1);
          break;
        }
      }
      vector2.X = (float) i;
      int num6 = (int) ((double) vector2.X - (double) num4 - 2.0);
      int num7 = (int) ((double) vector2.X + (double) num4 + 2.0);
      int num8 = (int) ((double) vector2.Y - (double) num5 - 2.0);
      int num9 = (int) ((double) vector2.Y + 2.0 + (double) WorldGen.genRand.Next(3, 5));
      if (num6 < 0)
        num6 = 0;
      if (num7 > Game1.maxTilesX)
        num7 = Game1.maxTilesX;
      if (num8 < 0)
        num8 = 0;
      if (num9 > Game1.maxTilesY)
        num9 = Game1.maxTilesY;
      for (int index1 = num6; index1 <= num7; ++index1)
      {
        for (int index2 = num8; index2 < num9; ++index2)
        {
          Game1.tile[index1, index2].active = true;
          Game1.tile[index1, index2].type = num1;
          Game1.tile[index1, index2].wall = (byte) 0;
        }
      }
      int num10 = (int) ((double) vector2.X - (double) num4);
      int num11 = (int) ((double) vector2.X + (double) num4);
      int num12 = (int) ((double) vector2.Y - (double) num5);
      int num13 = (int) ((double) vector2.Y + 1.0);
      if (num10 < 0)
        num10 = 0;
      if (num11 > Game1.maxTilesX)
        num11 = Game1.maxTilesX;
      if (num12 < 0)
        num12 = 0;
      if (num13 > Game1.maxTilesY)
        num13 = Game1.maxTilesY;
      for (int index3 = num10; index3 <= num11; ++index3)
      {
        for (int index4 = num12; index4 < num13; ++index4)
        {
          if (Game1.tile[index3, index4].wall == (byte) 0)
          {
            Game1.tile[index3, index4].active = false;
            Game1.tile[index3, index4].wall = num2;
          }
        }
      }
      int i1 = i + (num4 + 1) * num3;
      int y = (int) vector2.Y;
      for (int index = i1 - 2; index <= i1 + 2; ++index)
      {
        Game1.tile[index, y].active = false;
        Game1.tile[index, y - 1].active = false;
        Game1.tile[index, y - 2].active = false;
      }
      WorldGen.PlaceTile(i1, y, 10, true);
      int contain = 0;
      int num14 = WorldGen.houseCount;
      if (num14 > 2)
        num14 = WorldGen.genRand.Next(3);
      switch (num14)
      {
        case 0:
          contain = 159;
          break;
        case 1:
          contain = 65;
          break;
        case 2:
          contain = 158;
          break;
      }
      WorldGen.AddBuriedChest(i, y - 3, contain);
      ++WorldGen.houseCount;
    }

    public static void Mountinater(int i, int j)
    {
      double num1 = (double) WorldGen.genRand.Next(80, 120);
      float num2 = (float) WorldGen.genRand.Next(40, 55);
      Vector2 vector2_1;
      vector2_1.X = (float) i;
      vector2_1.Y = (float) j + num2 / 2f;
      Vector2 vector2_2;
      vector2_2.X = (float) WorldGen.genRand.Next(-10, 11) * 0.1f;
      vector2_2.Y = (float) WorldGen.genRand.Next(-20, -10) * 0.1f;
      while (num1 > 0.0 && (double) num2 > 0.0)
      {
        num1 -= (double) WorldGen.genRand.Next(4);
        --num2;
        int num3 = (int) ((double) vector2_1.X - num1 * 0.5);
        int num4 = (int) ((double) vector2_1.X + num1 * 0.5);
        int num5 = (int) ((double) vector2_1.Y - num1 * 0.5);
        int num6 = (int) ((double) vector2_1.Y + num1 * 0.5);
        if (num3 < 0)
          num3 = 0;
        if (num4 > Game1.maxTilesX)
          num4 = Game1.maxTilesX;
        if (num5 < 0)
          num5 = 0;
        if (num6 > Game1.maxTilesY)
          num6 = Game1.maxTilesY;
        double num7 = num1 * (double) WorldGen.genRand.Next(80, 120) * 0.01;
        for (int index1 = num3; index1 < num4; ++index1)
        {
          for (int index2 = num5; index2 < num6; ++index2)
          {
            float num8 = Math.Abs((float) index1 - vector2_1.X);
            float num9 = Math.Abs((float) index2 - vector2_1.Y);
            if (Math.Sqrt((double) num8 * (double) num8 + (double) num9 * (double) num9) < num7 * 0.4 && !Game1.tile[index1, index2].active)
            {
              Game1.tile[index1, index2].active = true;
              Game1.tile[index1, index2].type = (byte) 0;
            }
          }
        }
        vector2_1 += vector2_2;
        vector2_2.X += (float) WorldGen.genRand.Next(-10, 11) * 0.05f;
        vector2_2.Y += (float) WorldGen.genRand.Next(-10, 11) * 0.05f;
        if ((double) vector2_2.X > 0.5)
          vector2_2.X = 0.5f;
        if ((double) vector2_2.X < -0.5)
          vector2_2.X = -0.5f;
        if ((double) vector2_2.Y > -0.5)
          vector2_2.Y = -0.5f;
        if ((double) vector2_2.Y < -1.5)
          vector2_2.Y = -1.5f;
      }
    }

    public static void Lakinater(int i, int j)
    {
      double num1 = (double) WorldGen.genRand.Next(25, 50);
      double num2 = num1;
      float num3 = (float) WorldGen.genRand.Next(30, 80);
      if (WorldGen.genRand.Next(5) == 0)
      {
        num1 *= 1.5;
        num2 *= 1.5;
        num3 *= 1.2f;
      }
      Vector2 vector2_1;
      vector2_1.X = (float) i;
      vector2_1.Y = (float) j - num3 * 0.3f;
      Vector2 vector2_2;
      vector2_2.X = (float) WorldGen.genRand.Next(-10, 11) * 0.1f;
      vector2_2.Y = (float) WorldGen.genRand.Next(-20, -10) * 0.1f;
      while (num1 > 0.0 && (double) num3 > 0.0)
      {
        if ((double) vector2_1.Y + num2 * 0.5 > Game1.worldSurface)
          num3 = 0.0f;
        num1 -= (double) WorldGen.genRand.Next(3);
        --num3;
        int num4 = (int) ((double) vector2_1.X - num1 * 0.5);
        int num5 = (int) ((double) vector2_1.X + num1 * 0.5);
        int num6 = (int) ((double) vector2_1.Y - num1 * 0.5);
        int num7 = (int) ((double) vector2_1.Y + num1 * 0.5);
        if (num4 < 0)
          num4 = 0;
        if (num5 > Game1.maxTilesX)
          num5 = Game1.maxTilesX;
        if (num6 < 0)
          num6 = 0;
        if (num7 > Game1.maxTilesY)
          num7 = Game1.maxTilesY;
        num2 = num1 * (double) WorldGen.genRand.Next(80, 120) * 0.01;
        for (int index1 = num4; index1 < num5; ++index1)
        {
          for (int index2 = num6; index2 < num7; ++index2)
          {
            float num8 = Math.Abs((float) index1 - vector2_1.X);
            float num9 = Math.Abs((float) index2 - vector2_1.Y);
            if (Math.Sqrt((double) num8 * (double) num8 + (double) num9 * (double) num9) < num2 * 0.4)
            {
              if (Game1.tile[index1, index2].active)
                Game1.tile[index1, index2].liquid = byte.MaxValue;
              Game1.tile[index1, index2].active = false;
            }
          }
        }
        vector2_1 += vector2_2;
        vector2_2.X += (float) WorldGen.genRand.Next(-10, 11) * 0.05f;
        vector2_2.Y += (float) WorldGen.genRand.Next(-10, 11) * 0.05f;
        if ((double) vector2_2.X > 0.5)
          vector2_2.X = 0.5f;
        if ((double) vector2_2.X < -0.5)
          vector2_2.X = -0.5f;
        if ((double) vector2_2.Y > 1.5)
          vector2_2.Y = 1.5f;
        if ((double) vector2_2.Y < 0.5)
          vector2_2.Y = 0.5f;
      }
    }

    public static void ShroomPatch(int i, int j)
    {
      double num1 = (double) WorldGen.genRand.Next(40, 70);
      double num2 = num1;
      float num3 = (float) WorldGen.genRand.Next(10, 20);
      if (WorldGen.genRand.Next(5) == 0)
      {
        num1 *= 1.5;
        double num4 = num2 * 1.5;
        num3 *= 1.2f;
      }
      Vector2 vector2_1;
      vector2_1.X = (float) i;
      vector2_1.Y = (float) j - num3 * 0.3f;
      Vector2 vector2_2;
      vector2_2.X = (float) WorldGen.genRand.Next(-10, 11) * 0.1f;
      vector2_2.Y = (float) WorldGen.genRand.Next(-20, -10) * 0.1f;
      while (num1 > 0.0 && (double) num3 > 0.0)
      {
        num1 -= (double) WorldGen.genRand.Next(3);
        --num3;
        int num5 = (int) ((double) vector2_1.X - num1 * 0.5);
        int num6 = (int) ((double) vector2_1.X + num1 * 0.5);
        int num7 = (int) ((double) vector2_1.Y - num1 * 0.5);
        int num8 = (int) ((double) vector2_1.Y + num1 * 0.5);
        if (num5 < 0)
          num5 = 0;
        if (num6 > Game1.maxTilesX)
          num6 = Game1.maxTilesX;
        if (num7 < 0)
          num7 = 0;
        if (num8 > Game1.maxTilesY)
          num8 = Game1.maxTilesY;
        double num9 = num1 * (double) WorldGen.genRand.Next(80, 120) * 0.01;
        for (int index1 = num5; index1 < num6; ++index1)
        {
          for (int index2 = num7; index2 < num8; ++index2)
          {
            float num10 = Math.Abs((float) index1 - vector2_1.X);
            float num11 = Math.Abs((float) (((double) index2 - (double) vector2_1.Y) * 2.2999999523162842));
            if (Math.Sqrt((double) num10 * (double) num10 + (double) num11 * (double) num11) < num9 * 0.4)
            {
              if ((double) index2 < (double) vector2_1.Y + num9 * 0.05)
              {
                if (Game1.tile[index1, index2].type != (byte) 59)
                  Game1.tile[index1, index2].active = false;
              }
              else
                Game1.tile[index1, index2].type = (byte) 59;
              Game1.tile[index1, index2].liquid = (byte) 0;
              Game1.tile[index1, index2].lava = false;
            }
          }
        }
        vector2_1 += vector2_2;
        vector2_2.X += (float) WorldGen.genRand.Next(-10, 11) * 0.05f;
        vector2_2.Y += (float) WorldGen.genRand.Next(-10, 11) * 0.05f;
        if ((double) vector2_2.X > 1.0)
          vector2_2.X = 0.1f;
        if ((double) vector2_2.X < -1.0)
          vector2_2.X = -1f;
        if ((double) vector2_2.Y > 1.0)
          vector2_2.Y = 1f;
        if ((double) vector2_2.Y < -1.0)
          vector2_2.Y = -1f;
      }
    }

    public static void Cavinator(int i, int j, int steps)
    {
      double num1 = (double) WorldGen.genRand.Next(7, 15);
      int num2 = 1;
      if (WorldGen.genRand.Next(2) == 0)
        num2 = -1;
      Vector2 vector2_1;
      vector2_1.X = (float) i;
      vector2_1.Y = (float) j;
      int num3 = WorldGen.genRand.Next(20, 40);
      Vector2 vector2_2;
      vector2_2.Y = (float) WorldGen.genRand.Next(10, 20) * 0.01f;
      vector2_2.X = (float) num2;
      while (num3 > 0)
      {
        --num3;
        int num4 = (int) ((double) vector2_1.X - num1 * 0.5);
        int num5 = (int) ((double) vector2_1.X + num1 * 0.5);
        int num6 = (int) ((double) vector2_1.Y - num1 * 0.5);
        int num7 = (int) ((double) vector2_1.Y + num1 * 0.5);
        if (num4 < 0)
          num4 = 0;
        if (num5 > Game1.maxTilesX)
          num5 = Game1.maxTilesX;
        if (num6 < 0)
          num6 = 0;
        if (num7 > Game1.maxTilesY)
          num7 = Game1.maxTilesY;
        double num8 = num1 * (double) WorldGen.genRand.Next(80, 120) * 0.01;
        for (int index1 = num4; index1 < num5; ++index1)
        {
          for (int index2 = num6; index2 < num7; ++index2)
          {
            float num9 = Math.Abs((float) index1 - vector2_1.X);
            float num10 = Math.Abs((float) index2 - vector2_1.Y);
            if (Math.Sqrt((double) num9 * (double) num9 + (double) num10 * (double) num10) < num8 * 0.4)
              Game1.tile[index1, index2].active = false;
          }
        }
        vector2_1 += vector2_2;
        vector2_2.X += (float) WorldGen.genRand.Next(-10, 11) * 0.05f;
        vector2_2.Y += (float) WorldGen.genRand.Next(-10, 11) * 0.05f;
        if ((double) vector2_2.X > (double) num2 + 0.5)
          vector2_2.X = (float) num2 + 0.5f;
        if ((double) vector2_2.X < (double) num2 - 0.5)
          vector2_2.X = (float) num2 - 0.5f;
        if ((double) vector2_2.Y > 2.0)
          vector2_2.Y = 2f;
        if ((double) vector2_2.Y < 0.0)
          vector2_2.Y = 0.0f;
      }
      if (steps <= 0 || (double) (int) vector2_1.Y >= Game1.rockLayer + 50.0)
        return;
      WorldGen.Cavinator((int) vector2_1.X, (int) vector2_1.Y, steps - 1);
    }

    public static void CaveOpenater(int i, int j)
    {
      double num1 = (double) WorldGen.genRand.Next(7, 12);
      int num2 = 1;
      if (WorldGen.genRand.Next(2) == 0)
        num2 = -1;
      Vector2 vector2_1;
      vector2_1.X = (float) i;
      vector2_1.Y = (float) j;
      int num3 = 100;
      Vector2 vector2_2;
      vector2_2.Y = 0.0f;
      vector2_2.X = (float) num2;
      while (num3 > 0)
      {
        if (Game1.tile[(int) vector2_1.X, (int) vector2_1.Y].wall == (byte) 0)
          num3 = 0;
        --num3;
        int num4 = (int) ((double) vector2_1.X - num1 * 0.5);
        int num5 = (int) ((double) vector2_1.X + num1 * 0.5);
        int num6 = (int) ((double) vector2_1.Y - num1 * 0.5);
        int num7 = (int) ((double) vector2_1.Y + num1 * 0.5);
        if (num4 < 0)
          num4 = 0;
        if (num5 > Game1.maxTilesX)
          num5 = Game1.maxTilesX;
        if (num6 < 0)
          num6 = 0;
        if (num7 > Game1.maxTilesY)
          num7 = Game1.maxTilesY;
        double num8 = num1 * (double) WorldGen.genRand.Next(80, 120) * 0.01;
        for (int index1 = num4; index1 < num5; ++index1)
        {
          for (int index2 = num6; index2 < num7; ++index2)
          {
            float num9 = Math.Abs((float) index1 - vector2_1.X);
            float num10 = Math.Abs((float) index2 - vector2_1.Y);
            if (Math.Sqrt((double) num9 * (double) num9 + (double) num10 * (double) num10) < num8 * 0.4)
              Game1.tile[index1, index2].active = false;
          }
        }
        vector2_1 += vector2_2;
        vector2_2.X += (float) WorldGen.genRand.Next(-10, 11) * 0.05f;
        vector2_2.Y += (float) WorldGen.genRand.Next(-10, 11) * 0.05f;
        if ((double) vector2_2.X > (double) num2 + 0.5)
          vector2_2.X = (float) num2 + 0.5f;
        if ((double) vector2_2.X < (double) num2 - 0.5)
          vector2_2.X = (float) num2 - 0.5f;
        if ((double) vector2_2.Y > 0.0)
          vector2_2.Y = 0.0f;
        if ((double) vector2_2.Y < -0.5)
          vector2_2.Y = -0.5f;
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

    public static void SectionTileFrame(int startX, int startY, int endX, int endY)
    {
      int num1 = startX * 200;
      int num2 = (endX + 1) * 200;
      int num3 = startY * 150;
      int num4 = (endY + 1) * 150;
      if (num1 < 1)
        num1 = 1;
      if (num3 < 1)
        num3 = 1;
      if (num1 > Game1.maxTilesX - 2)
        num1 = Game1.maxTilesX - 2;
      if (num3 > Game1.maxTilesY - 2)
        num3 = Game1.maxTilesY - 2;
      for (int i = num1 - 1; i < num2 + 1; ++i)
      {
        for (int j = num3 - 1; j < num4 + 1; ++j)
        {
          if (Game1.tile[i, j] == null)
            Game1.tile[i, j] = new Tile();
          WorldGen.TileFrame(i, j, true, true);
          WorldGen.WallFrame(i, j, true);
        }
      }
    }

    public static void RangeFrame(int startX, int startY, int endX, int endY)
    {
      int num1 = startX;
      int num2 = endX + 1;
      int num3 = startY;
      int num4 = endY + 1;
      for (int i = num1 - 1; i < num2 + 1; ++i)
      {
        for (int j = num3 - 1; j < num4 + 1; ++j)
        {
          WorldGen.TileFrame(i, j);
          WorldGen.WallFrame(i, j);
        }
      }
    }

    public static void WaterCheck()
    {
      Liquid.numLiquid = 0;
      LiquidBuffer.numLiquidBuffer = 0;
      for (int index1 = 1; index1 < Game1.maxTilesX - 1; ++index1)
      {
        for (int index2 = Game1.maxTilesY - 2; index2 > 0; --index2)
        {
          Game1.tile[index1, index2].checkingLiquid = false;
          if (Game1.tile[index1, index2].liquid > (byte) 0 && Game1.tile[index1, index2].active && Game1.tileSolid[(int) Game1.tile[index1, index2].type] && !Game1.tileSolidTop[(int) Game1.tile[index1, index2].type])
            Game1.tile[index1, index2].liquid = (byte) 0;
          else if (Game1.tile[index1, index2].liquid > (byte) 0)
          {
            if (Game1.tile[index1, index2].active)
            {
              if (Game1.tileWaterDeath[(int) Game1.tile[index1, index2].type])
                WorldGen.KillTile(index1, index2);
              if (Game1.tile[index1, index2].lava && Game1.tileLavaDeath[(int) Game1.tile[index1, index2].type])
                WorldGen.KillTile(index1, index2);
            }
            if ((!Game1.tile[index1, index2 + 1].active || !Game1.tileSolid[(int) Game1.tile[index1, index2 + 1].type] || Game1.tileSolidTop[(int) Game1.tile[index1, index2 + 1].type]) && Game1.tile[index1, index2 + 1].liquid < byte.MaxValue)
            {
              if (Game1.tile[index1, index2 + 1].liquid > (byte) 250)
                Game1.tile[index1, index2 + 1].liquid = byte.MaxValue;
              else
                Liquid.AddWater(index1, index2);
            }
            if ((!Game1.tile[index1 - 1, index2].active || !Game1.tileSolid[(int) Game1.tile[index1 - 1, index2].type] || Game1.tileSolidTop[(int) Game1.tile[index1 - 1, index2].type]) && (int) Game1.tile[index1 - 1, index2].liquid != (int) Game1.tile[index1, index2].liquid)
              Liquid.AddWater(index1, index2);
            else if ((!Game1.tile[index1 + 1, index2].active || !Game1.tileSolid[(int) Game1.tile[index1 + 1, index2].type] || Game1.tileSolidTop[(int) Game1.tile[index1 + 1, index2].type]) && (int) Game1.tile[index1 + 1, index2].liquid != (int) Game1.tile[index1, index2].liquid)
              Liquid.AddWater(index1, index2);
            if (Game1.tile[index1, index2].lava)
            {
              if (Game1.tile[index1 - 1, index2].liquid > (byte) 0 && !Game1.tile[index1 - 1, index2].lava)
                Liquid.AddWater(index1, index2);
              else if (Game1.tile[index1 + 1, index2].liquid > (byte) 0 && !Game1.tile[index1 + 1, index2].lava)
                Liquid.AddWater(index1, index2);
              else if (Game1.tile[index1, index2 - 1].liquid > (byte) 0 && !Game1.tile[index1, index2 - 1].lava)
                Liquid.AddWater(index1, index2);
              else if (Game1.tile[index1, index2 + 1].liquid > (byte) 0 && !Game1.tile[index1, index2 + 1].lava)
                Liquid.AddWater(index1, index2);
            }
          }
        }
      }
    }

    public static void EveryTileFrame()
    {
      WorldGen.noLiquidCheck = true;
      for (int i = 0; i < Game1.maxTilesX; ++i)
      {
        Game1.statusText = "Finding tile frames: " + (object) (int) ((double) ((float) i / (float) Game1.maxTilesX) * 100.0 + 1.0) + "%";
        for (int j = 0; j < Game1.maxTilesY; ++j)
        {
          WorldGen.TileFrame(i, j, true);
          WorldGen.WallFrame(i, j, true);
        }
      }
      WorldGen.noLiquidCheck = false;
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
      if (i + 1 >= Game1.maxTilesX)
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
      if (j + 1 >= Game1.maxTilesY)
      {
        num6 = type;
        num7 = type;
        num8 = type;
      }
      if (i - 1 >= 0 && Game1.tile[i - 1, j] != null && Game1.tile[i - 1, j].active)
        num4 = (int) Game1.tile[i - 1, j].type;
      if (i + 1 < Game1.maxTilesX && Game1.tile[i + 1, j] != null && Game1.tile[i + 1, j].active)
        num5 = (int) Game1.tile[i + 1, j].type;
      if (j - 1 >= 0 && Game1.tile[i, j - 1] != null && Game1.tile[i, j - 1].active)
        num2 = (int) Game1.tile[i, j - 1].type;
      if (j + 1 < Game1.maxTilesY && Game1.tile[i, j + 1] != null && Game1.tile[i, j + 1].active)
        num7 = (int) Game1.tile[i, j + 1].type;
      if (i - 1 >= 0 && j - 1 >= 0 && Game1.tile[i - 1, j - 1] != null && Game1.tile[i - 1, j - 1].active)
        num1 = (int) Game1.tile[i - 1, j - 1].type;
      if (i + 1 < Game1.maxTilesX && j - 1 >= 0 && Game1.tile[i + 1, j - 1] != null && Game1.tile[i + 1, j - 1].active)
        num3 = (int) Game1.tile[i + 1, j - 1].type;
      if (i - 1 >= 0 && j + 1 < Game1.maxTilesY && Game1.tile[i - 1, j + 1] != null && Game1.tile[i - 1, j + 1].active)
        num6 = (int) Game1.tile[i - 1, j + 1].type;
      if (i + 1 < Game1.maxTilesX && j + 1 < Game1.maxTilesY && Game1.tile[i + 1, j + 1] != null && Game1.tile[i + 1, j + 1].active)
        num8 = (int) Game1.tile[i + 1, j + 1].type;
      if ((type != 3 || num7 == 2) && (type != 24 || num7 == 23) && (type != 61 || num7 == 60) && (type != 71 || num7 == 70) && (type != 73 || num7 == 2) && (type != 74 || num7 == 60))
        return;
      WorldGen.KillTile(i, j);
    }

    public static void WallFrame(int i, int j, bool resetFrame = false)
    {
      if (i < 0 || j < 0 || i >= Game1.maxTilesX || j >= Game1.maxTilesY || Game1.tile[i, j] == null || Game1.tile[i, j].wall <= (byte) 0)
        return;
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
      if (i + 1 >= Game1.maxTilesX)
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
      if (j + 1 >= Game1.maxTilesY)
      {
        num6 = wall;
        num7 = wall;
        num8 = wall;
      }
      if (i - 1 >= 0 && Game1.tile[i - 1, j] != null)
        num4 = (int) Game1.tile[i - 1, j].wall;
      if (i + 1 < Game1.maxTilesX && Game1.tile[i + 1, j] != null)
        num5 = (int) Game1.tile[i + 1, j].wall;
      if (j - 1 >= 0 && Game1.tile[i, j - 1] != null)
        num2 = (int) Game1.tile[i, j - 1].wall;
      if (j + 1 < Game1.maxTilesY && Game1.tile[i, j + 1] != null)
        num7 = (int) Game1.tile[i, j + 1].wall;
      if (i - 1 >= 0 && j - 1 >= 0 && Game1.tile[i - 1, j - 1] != null)
        num1 = (int) Game1.tile[i - 1, j - 1].wall;
      if (i + 1 < Game1.maxTilesX && j - 1 >= 0 && Game1.tile[i + 1, j - 1] != null)
        num3 = (int) Game1.tile[i + 1, j - 1].wall;
      if (i - 1 >= 0 && j + 1 < Game1.maxTilesY && Game1.tile[i - 1, j + 1] != null)
        num6 = (int) Game1.tile[i - 1, j + 1].wall;
      if (i + 1 < Game1.maxTilesX && j + 1 < Game1.maxTilesY && Game1.tile[i + 1, j + 1] != null)
        num8 = (int) Game1.tile[i + 1, j + 1].wall;
      if (wall == 2)
      {
        if (j == (int) Game1.worldSurface)
        {
          num7 = wall;
          num6 = wall;
          num8 = wall;
        }
        else if (j >= (int) Game1.worldSurface)
        {
          num7 = wall;
          num6 = wall;
          num8 = wall;
          num2 = wall;
          num1 = wall;
          num3 = wall;
          num4 = wall;
          num5 = wall;
        }
      }
      int num9;
      if (resetFrame)
      {
        num9 = WorldGen.genRand.Next(0, 3);
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
    }

    public static void TileFrame(int i, int j, bool resetFrame = false, bool noBreak = false)
    {
      if (i < 0 || j < 0 || i >= Game1.maxTilesX || j >= Game1.maxTilesY || Game1.tile[i, j] == null)
        return;
      if (Game1.tile[i, j].liquid > (byte) 0 && Game1.netMode != 1 && !WorldGen.noLiquidCheck)
        Liquid.AddWater(i, j);
      if (Game1.tile[i, j].active && (!noBreak || !Game1.tileFrameImportant[(int) Game1.tile[i, j].type]))
      {
        int index1 = -1;
        int index2 = -1;
        int index3 = -1;
        int index4 = -1;
        int index5 = -1;
        int index6 = -1;
        int index7 = -1;
        int index8 = -1;
        int type = (int) Game1.tile[i, j].type;
        if (Game1.tileStone[type])
          type = 1;
        int frameX1 = (int) Game1.tile[i, j].frameX;
        int frameY1 = (int) Game1.tile[i, j].frameY;
        Rectangle rectangle;
        rectangle.X = -1;
        rectangle.Y = -1;
        if (type == 3 || type == 24 || type == 61 || type == 71 || type == 73 || type == 74)
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
            index1 = type;
            index4 = type;
            index6 = type;
          }
          if (i + 1 >= Game1.maxTilesX)
          {
            index3 = type;
            index5 = type;
            index8 = type;
          }
          if (j - 1 < 0)
          {
            index1 = type;
            index2 = type;
            index3 = type;
          }
          if (j + 1 >= Game1.maxTilesY)
          {
            index6 = type;
            index7 = type;
            index8 = type;
          }
          if (i - 1 >= 0 && Game1.tile[i - 1, j] != null && Game1.tile[i - 1, j].active)
            index4 = (int) Game1.tile[i - 1, j].type;
          if (i + 1 < Game1.maxTilesX && Game1.tile[i + 1, j] != null && Game1.tile[i + 1, j].active)
            index5 = (int) Game1.tile[i + 1, j].type;
          if (j - 1 >= 0 && Game1.tile[i, j - 1] != null && Game1.tile[i, j - 1].active)
            index2 = (int) Game1.tile[i, j - 1].type;
          if (j + 1 < Game1.maxTilesY && Game1.tile[i, j + 1] != null && Game1.tile[i, j + 1].active)
            index7 = (int) Game1.tile[i, j + 1].type;
          if (i - 1 >= 0 && j - 1 >= 0 && Game1.tile[i - 1, j - 1] != null && Game1.tile[i - 1, j - 1].active)
            index1 = (int) Game1.tile[i - 1, j - 1].type;
          if (i + 1 < Game1.maxTilesX && j - 1 >= 0 && Game1.tile[i + 1, j - 1] != null && Game1.tile[i + 1, j - 1].active)
            index3 = (int) Game1.tile[i + 1, j - 1].type;
          if (i - 1 >= 0 && j + 1 < Game1.maxTilesY && Game1.tile[i - 1, j + 1] != null && Game1.tile[i - 1, j + 1].active)
            index6 = (int) Game1.tile[i - 1, j + 1].type;
          if (i + 1 < Game1.maxTilesX && j + 1 < Game1.maxTilesY && Game1.tile[i + 1, j + 1] != null && Game1.tile[i + 1, j + 1].active)
            index8 = (int) Game1.tile[i + 1, j + 1].type;
          if (index4 >= 0 && Game1.tileStone[index4])
            index4 = 1;
          if (index5 >= 0 && Game1.tileStone[index5])
            index5 = 1;
          if (index2 >= 0 && Game1.tileStone[index2])
            index2 = 1;
          if (index7 >= 0 && Game1.tileStone[index7])
            index7 = 1;
          if (index1 >= 0 && Game1.tileStone[index1])
            index1 = 1;
          if (index3 >= 0 && Game1.tileStone[index3])
            index3 = 1;
          if (index6 >= 0 && Game1.tileStone[index6])
            index6 = 1;
          if (index8 >= 0 && Game1.tileStone[index8])
            index8 = 1;
          int num1;
          switch (type)
          {
            case 4:
              if (index7 >= 0 && Game1.tileSolid[index7] && !Game1.tileNoAttach[index7])
              {
                Game1.tile[i, j].frameX = (short) 0;
                return;
              }
              if (index4 >= 0 && Game1.tileSolid[index4] && !Game1.tileNoAttach[index4] || index4 == 5 && index1 == 5 && index6 == 5)
              {
                Game1.tile[i, j].frameX = (short) 22;
                return;
              }
              if (index5 >= 0 && Game1.tileSolid[index5] && !Game1.tileNoAttach[index5] || index5 == 5 && index3 == 5 && index8 == 5)
              {
                Game1.tile[i, j].frameX = (short) 44;
                return;
              }
              WorldGen.KillTile(i, j);
              return;
            case 12:
              num1 = 0;
              break;
            default:
              num1 = type != 31 ? 1 : 0;
              break;
          }
          if (num1 == 0)
          {
            if (WorldGen.destroyObject)
              return;
            int i1 = Game1.tile[i, j].frameX != (short) 0 ? i - 1 : i;
            int j1 = Game1.tile[i, j].frameY != (short) 0 ? j - 1 : j;
            if (Game1.tile[i1, j1] != null && Game1.tile[i1 + 1, j1] != null && Game1.tile[i1, j1 + 1] != null && Game1.tile[i1 + 1, j1 + 1] != null && (!Game1.tile[i1, j1].active || (int) Game1.tile[i1, j1].type != type || !Game1.tile[i1 + 1, j1].active || (int) Game1.tile[i1 + 1, j1].type != type || !Game1.tile[i1, j1 + 1].active || (int) Game1.tile[i1, j1 + 1].type != type || !Game1.tile[i1 + 1, j1 + 1].active || (int) Game1.tile[i1 + 1, j1 + 1].type != type))
            {
              WorldGen.destroyObject = true;
              if ((int) Game1.tile[i1, j1].type == type)
                WorldGen.KillTile(i1, j1);
              if ((int) Game1.tile[i1 + 1, j1].type == type)
                WorldGen.KillTile(i1 + 1, j1);
              if ((int) Game1.tile[i1, j1 + 1].type == type)
                WorldGen.KillTile(i1, j1 + 1);
              if ((int) Game1.tile[i1 + 1, j1 + 1].type == type)
                WorldGen.KillTile(i1 + 1, j1 + 1);
              switch (type)
              {
                case 12:
                  Item.NewItem(i1 * 16, j1 * 16, 32, 32, 29);
                  break;
                case 31:
                  WorldGen.spawnMeteor = true;
                  int num2 = Game1.rand.Next(5);
                  if (!WorldGen.shadowOrbSmashed)
                    num2 = 0;
                  switch (num2)
                  {
                    case 0:
                      Item.NewItem(i1 * 16, j1 * 16, 32, 32, 96);
                      int Stack = WorldGen.genRand.Next(25, 51);
                      Item.NewItem(i1 * 16, j1 * 16, 32, 32, 97, Stack);
                      break;
                    case 1:
                      Item.NewItem(i1 * 16, j1 * 16, 32, 32, 64);
                      break;
                    case 2:
                      Item.NewItem(i1 * 16, j1 * 16, 32, 32, 162);
                      break;
                    case 3:
                      Item.NewItem(i1 * 16, j1 * 16, 32, 32, 115);
                      break;
                    case 4:
                      Item.NewItem(i1 * 16, j1 * 16, 32, 32, 111);
                      break;
                  }
                  WorldGen.shadowOrbSmashed = true;
                  break;
              }
              Game1.PlaySound(13, i * 16, j * 16);
              WorldGen.destroyObject = false;
            }
          }
          else
          {
            int num3;
            switch (type)
            {
              case 10:
                if (WorldGen.destroyObject)
                  return;
                int frameY2 = (int) Game1.tile[i, j].frameY;
                int j2 = j;
                bool flag1 = false;
                if (frameY2 == 0)
                  j2 = j;
                if (frameY2 == 18)
                  j2 = j - 1;
                if (frameY2 == 36)
                  j2 = j - 2;
                if (Game1.tile[i, j2 - 1] == null)
                  Game1.tile[i, j2 - 1] = new Tile();
                if (Game1.tile[i, j2 + 3] == null)
                  Game1.tile[i, j2 + 3] = new Tile();
                if (Game1.tile[i, j2 + 2] == null)
                  Game1.tile[i, j2 + 2] = new Tile();
                if (Game1.tile[i, j2 + 1] == null)
                  Game1.tile[i, j2 + 1] = new Tile();
                if (Game1.tile[i, j2] == null)
                  Game1.tile[i, j2] = new Tile();
                if (!Game1.tile[i, j2 - 1].active || !Game1.tileSolid[(int) Game1.tile[i, j2 - 1].type])
                  flag1 = true;
                if (!Game1.tile[i, j2 + 3].active || !Game1.tileSolid[(int) Game1.tile[i, j2 + 3].type])
                  flag1 = true;
                if (!Game1.tile[i, j2].active || (int) Game1.tile[i, j2].type != type)
                  flag1 = true;
                if (!Game1.tile[i, j2 + 1].active || (int) Game1.tile[i, j2 + 1].type != type)
                  flag1 = true;
                if (!Game1.tile[i, j2 + 2].active || (int) Game1.tile[i, j2 + 2].type != type)
                  flag1 = true;
                if (flag1)
                {
                  WorldGen.destroyObject = true;
                  WorldGen.KillTile(i, j2);
                  WorldGen.KillTile(i, j2 + 1);
                  WorldGen.KillTile(i, j2 + 2);
                  Item.NewItem(i * 16, j * 16, 16, 16, 25);
                }
                WorldGen.destroyObject = false;
                return;
              case 11:
                if (WorldGen.destroyObject)
                  return;
                int num4 = 0;
                int index9 = i;
                int num5 = j;
                int frameX2 = (int) Game1.tile[i, j].frameX;
                int frameY3 = (int) Game1.tile[i, j].frameY;
                bool flag2 = false;
                switch (frameX2)
                {
                  case 0:
                    index9 = i;
                    num4 = 1;
                    break;
                  case 18:
                    index9 = i - 1;
                    num4 = 1;
                    break;
                  case 36:
                    index9 = i + 1;
                    num4 = -1;
                    break;
                  case 54:
                    index9 = i;
                    num4 = -1;
                    break;
                }
                switch (frameY3)
                {
                  case 0:
                    num5 = j;
                    break;
                  case 18:
                    num5 = j - 1;
                    break;
                  case 36:
                    num5 = j - 2;
                    break;
                }
                if (Game1.tile[index9, num5 + 3] == null)
                  Game1.tile[index9, num5 + 3] = new Tile();
                if (Game1.tile[index9, num5 - 1] == null)
                  Game1.tile[index9, num5 - 1] = new Tile();
                if (!Game1.tile[index9, num5 - 1].active || !Game1.tileSolid[(int) Game1.tile[index9, num5 - 1].type] || !Game1.tile[index9, num5 + 3].active || !Game1.tileSolid[(int) Game1.tile[index9, num5 + 3].type])
                {
                  flag2 = true;
                  WorldGen.destroyObject = true;
                  Item.NewItem(i * 16, j * 16, 16, 16, 25);
                }
                int num6 = index9;
                if (num4 == -1)
                  num6 = index9 - 1;
                for (int i2 = num6; i2 < num6 + 2; ++i2)
                {
                  for (int j3 = num5; j3 < num5 + 3; ++j3)
                  {
                    if (!flag2 && (Game1.tile[i2, j3].type != (byte) 11 || !Game1.tile[i2, j3].active))
                    {
                      WorldGen.destroyObject = true;
                      Item.NewItem(i * 16, j * 16, 16, 16, 25);
                      flag2 = true;
                      i2 = num6;
                      j3 = num5;
                    }
                    if (flag2)
                      WorldGen.KillTile(i2, j3);
                  }
                }
                WorldGen.destroyObject = false;
                return;
              case 19:
                if (index4 == type && index5 == type)
                {
                  if (Game1.tile[i, j].frameNumber == (byte) 0)
                  {
                    rectangle.X = 0;
                    rectangle.Y = 0;
                  }
                  if (Game1.tile[i, j].frameNumber == (byte) 1)
                  {
                    rectangle.X = 0;
                    rectangle.Y = 18;
                  }
                  if (Game1.tile[i, j].frameNumber == (byte) 2)
                  {
                    rectangle.X = 0;
                    rectangle.Y = 36;
                    goto label_220;
                  }
                  else
                    goto label_220;
                }
                else if (index4 == type && index5 == -1)
                {
                  if (Game1.tile[i, j].frameNumber == (byte) 0)
                  {
                    rectangle.X = 18;
                    rectangle.Y = 0;
                  }
                  if (Game1.tile[i, j].frameNumber == (byte) 1)
                  {
                    rectangle.X = 18;
                    rectangle.Y = 18;
                  }
                  if (Game1.tile[i, j].frameNumber == (byte) 2)
                  {
                    rectangle.X = 18;
                    rectangle.Y = 36;
                    goto label_220;
                  }
                  else
                    goto label_220;
                }
                else if (index4 == -1 && index5 == type)
                {
                  if (Game1.tile[i, j].frameNumber == (byte) 0)
                  {
                    rectangle.X = 36;
                    rectangle.Y = 0;
                  }
                  if (Game1.tile[i, j].frameNumber == (byte) 1)
                  {
                    rectangle.X = 36;
                    rectangle.Y = 18;
                  }
                  if (Game1.tile[i, j].frameNumber == (byte) 2)
                  {
                    rectangle.X = 36;
                    rectangle.Y = 36;
                    goto label_220;
                  }
                  else
                    goto label_220;
                }
                else if (index4 != type && index5 == type)
                {
                  if (Game1.tile[i, j].frameNumber == (byte) 0)
                  {
                    rectangle.X = 54;
                    rectangle.Y = 0;
                  }
                  if (Game1.tile[i, j].frameNumber == (byte) 1)
                  {
                    rectangle.X = 54;
                    rectangle.Y = 18;
                  }
                  if (Game1.tile[i, j].frameNumber == (byte) 2)
                  {
                    rectangle.X = 54;
                    rectangle.Y = 36;
                    goto label_220;
                  }
                  else
                    goto label_220;
                }
                else if (index4 == type && index5 != type)
                {
                  if (Game1.tile[i, j].frameNumber == (byte) 0)
                  {
                    rectangle.X = 72;
                    rectangle.Y = 0;
                  }
                  if (Game1.tile[i, j].frameNumber == (byte) 1)
                  {
                    rectangle.X = 72;
                    rectangle.Y = 18;
                  }
                  if (Game1.tile[i, j].frameNumber == (byte) 2)
                  {
                    rectangle.X = 72;
                    rectangle.Y = 36;
                    goto label_220;
                  }
                  else
                    goto label_220;
                }
                else if (index4 != type && index4 != -1 && index5 == -1)
                {
                  if (Game1.tile[i, j].frameNumber == (byte) 0)
                  {
                    rectangle.X = 108;
                    rectangle.Y = 0;
                  }
                  if (Game1.tile[i, j].frameNumber == (byte) 1)
                  {
                    rectangle.X = 108;
                    rectangle.Y = 18;
                  }
                  if (Game1.tile[i, j].frameNumber == (byte) 2)
                  {
                    rectangle.X = 108;
                    rectangle.Y = 36;
                    goto label_220;
                  }
                  else
                    goto label_220;
                }
                else if (index4 == -1 && index5 != type && index5 != -1)
                {
                  if (Game1.tile[i, j].frameNumber == (byte) 0)
                  {
                    rectangle.X = 126;
                    rectangle.Y = 0;
                  }
                  if (Game1.tile[i, j].frameNumber == (byte) 1)
                  {
                    rectangle.X = 126;
                    rectangle.Y = 18;
                  }
                  if (Game1.tile[i, j].frameNumber == (byte) 2)
                  {
                    rectangle.X = 126;
                    rectangle.Y = 36;
                    goto label_220;
                  }
                  else
                    goto label_220;
                }
                else
                {
                  if (Game1.tile[i, j].frameNumber == (byte) 0)
                  {
                    rectangle.X = 90;
                    rectangle.Y = 0;
                  }
                  if (Game1.tile[i, j].frameNumber == (byte) 1)
                  {
                    rectangle.X = 90;
                    rectangle.Y = 18;
                  }
                  if (Game1.tile[i, j].frameNumber == (byte) 2)
                  {
                    rectangle.X = 90;
                    rectangle.Y = 36;
                  }
                  goto label_220;
                }
              case 34:
              case 35:
                num3 = 0;
                break;
              default:
                num3 = type != 36 ? 1 : 0;
                break;
            }
            if (num3 == 0)
            {
              WorldGen.Check3x3(i, j, (int) (byte) type);
              return;
            }
            if (type == 15 || type == 20)
            {
              WorldGen.Check1x2(i, j, (byte) type);
              return;
            }
            if (type == 14 || type == 17 || type == 26)
            {
              WorldGen.Check3x2(i, j, (int) (byte) type);
              return;
            }
            if (type == 16 || type == 18 || type == 29)
            {
              WorldGen.Check2x1(i, j, (byte) type);
              return;
            }
            if (type == 13 || type == 33 || type == 49 || type == 50)
            {
              WorldGen.CheckOnTable1x1(i, j, (int) (byte) type);
              return;
            }
            if (type == 21)
            {
              WorldGen.CheckChest(i, j, (int) (byte) type);
              return;
            }
            if (type == 27)
            {
              WorldGen.CheckSunflower(i, j);
              return;
            }
            if (type == 28)
            {
              WorldGen.CheckPot(i, j);
              return;
            }
            if (type == 42)
            {
              WorldGen.Check1x2Top(i, j, (byte) type);
              return;
            }
            if (type == 55)
            {
              WorldGen.CheckSign(i, j, type);
              return;
            }
label_220:
            if (type == 72)
            {
              if (index7 != type && index7 != 70)
                WorldGen.KillTile(i, j);
              else if (index2 != type && Game1.tile[i, j].frameX == (short) 0)
              {
                Game1.tile[i, j].frameNumber = (byte) WorldGen.genRand.Next(3);
                if (Game1.tile[i, j].frameNumber == (byte) 0)
                {
                  Game1.tile[i, j].frameX = (short) 18;
                  Game1.tile[i, j].frameY = (short) 0;
                }
                if (Game1.tile[i, j].frameNumber == (byte) 1)
                {
                  Game1.tile[i, j].frameX = (short) 18;
                  Game1.tile[i, j].frameY = (short) 18;
                }
                if (Game1.tile[i, j].frameNumber == (byte) 2)
                {
                  Game1.tile[i, j].frameX = (short) 18;
                  Game1.tile[i, j].frameY = (short) 36;
                }
              }
            }
            if (type == 5)
            {
              if (Game1.tile[i, j].frameX >= (short) 22 && Game1.tile[i, j].frameX <= (short) 44 && Game1.tile[i, j].frameY >= (short) 132 && Game1.tile[i, j].frameY <= (short) 176)
              {
                if (index4 != type && index5 != type || index7 != 2)
                  WorldGen.KillTile(i, j);
              }
              else if (Game1.tile[i, j].frameX == (short) 88 && Game1.tile[i, j].frameY >= (short) 0 && Game1.tile[i, j].frameY <= (short) 44 || Game1.tile[i, j].frameX == (short) 66 && Game1.tile[i, j].frameY >= (short) 66 && Game1.tile[i, j].frameY <= (short) 130 || Game1.tile[i, j].frameX == (short) 110 && Game1.tile[i, j].frameY >= (short) 66 && Game1.tile[i, j].frameY <= (short) 110 || Game1.tile[i, j].frameX == (short) 132 && Game1.tile[i, j].frameY >= (short) 0 && Game1.tile[i, j].frameY <= (short) 176)
              {
                if (index4 == type && index5 == type)
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
                else if (index4 == type)
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
                else if (index5 == type)
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
              }
              if (Game1.tile[i, j].frameY >= (short) 132 && Game1.tile[i, j].frameY <= (short) 176 && (Game1.tile[i, j].frameX == (short) 0 || Game1.tile[i, j].frameX == (short) 66 || Game1.tile[i, j].frameX == (short) 88))
              {
                if (index7 != 2)
                  WorldGen.KillTile(i, j);
                if (index4 != type && index5 != type)
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
                else if (index4 != type)
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
                else if (index5 != type)
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
                if (index4 != type && index5 != type)
                  WorldGen.KillTile(i, j);
              }
              else if (index7 == -1 || index7 == 23)
                WorldGen.KillTile(i, j);
              else if (index2 != type && Game1.tile[i, j].frameY < (short) 198 && (Game1.tile[i, j].frameX != (short) 22 && Game1.tile[i, j].frameX != (short) 44 || Game1.tile[i, j].frameY < (short) 132))
              {
                if (index4 == type || index5 == type)
                {
                  if (index7 == type)
                  {
                    if (index4 == type && index5 == type)
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
                    else if (index4 == type)
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
                    else if (index5 == type)
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
                  else if (index4 == type && index5 == type)
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
                  else if (index4 == type)
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
                  else if (index5 == type)
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
            }
            if (Game1.tileFrameImportant[(int) Game1.tile[i, j].type])
              return;
            int num7;
            if (resetFrame)
            {
              num7 = WorldGen.genRand.Next(0, 3);
              Game1.tile[i, j].frameNumber = (byte) num7;
            }
            else
              num7 = (int) Game1.tile[i, j].frameNumber;
            switch (type)
            {
              case 0:
                for (int index10 = 0; index10 < 76; ++index10)
                {
                  if (index10 == 1 || index10 == 6 || index10 == 7 || index10 == 8 || index10 == 9 || index10 == 22 || index10 == 25 || index10 == 37 || index10 == 40 || index10 == 53 || index10 == 56)
                  {
                    if (index2 == index10)
                    {
                      WorldGen.TileFrame(i, j - 1);
                      if (WorldGen.mergeDown)
                        index2 = type;
                    }
                    if (index7 == index10)
                    {
                      WorldGen.TileFrame(i, j + 1);
                      if (WorldGen.mergeUp)
                        index7 = type;
                    }
                    if (index4 == index10)
                    {
                      WorldGen.TileFrame(i - 1, j);
                      if (WorldGen.mergeRight)
                        index4 = type;
                    }
                    if (index5 == index10)
                    {
                      WorldGen.TileFrame(i + 1, j);
                      if (WorldGen.mergeLeft)
                        index5 = type;
                    }
                    if (index1 == index10)
                      index1 = type;
                    if (index3 == index10)
                      index3 = type;
                    if (index6 == index10)
                      index6 = type;
                    if (index8 == index10)
                      index8 = type;
                  }
                }
                if (index2 == 2)
                  index2 = type;
                if (index7 == 2)
                  index7 = type;
                if (index4 == 2)
                  index4 = type;
                if (index5 == 2)
                  index5 = type;
                if (index1 == 2)
                  index1 = type;
                if (index3 == 2)
                  index3 = type;
                if (index6 == 2)
                  index6 = type;
                if (index8 == 2)
                  index8 = type;
                if (index2 == 23)
                  index2 = type;
                if (index7 == 23)
                  index7 = type;
                if (index4 == 23)
                  index4 = type;
                if (index5 == 23)
                  index5 = type;
                if (index1 == 23)
                  index1 = type;
                if (index3 == 23)
                  index3 = type;
                if (index6 == 23)
                  index6 = type;
                if (index8 == 23)
                {
                  index8 = type;
                  break;
                }
                break;
              case 1:
                if (index2 == 59)
                {
                  WorldGen.TileFrame(i, j - 1);
                  if (WorldGen.mergeDown)
                    index2 = type;
                }
                if (index7 == 59)
                {
                  WorldGen.TileFrame(i, j + 1);
                  if (WorldGen.mergeUp)
                    index7 = type;
                }
                if (index4 == 59)
                {
                  WorldGen.TileFrame(i - 1, j);
                  if (WorldGen.mergeRight)
                    index4 = type;
                }
                if (index5 == 59)
                {
                  WorldGen.TileFrame(i + 1, j);
                  if (WorldGen.mergeLeft)
                    index5 = type;
                }
                if (index1 == 59)
                  index1 = type;
                if (index3 == 59)
                  index3 = type;
                if (index6 == 59)
                  index6 = type;
                if (index8 == 59)
                  index8 = type;
                break;
              case 57:
                if (index2 == 58)
                {
                  WorldGen.TileFrame(i, j - 1);
                  if (WorldGen.mergeDown)
                    index2 = type;
                }
                if (index7 == 58)
                {
                  WorldGen.TileFrame(i, j + 1);
                  if (WorldGen.mergeUp)
                    index7 = type;
                }
                if (index4 == 58)
                {
                  WorldGen.TileFrame(i - 1, j);
                  if (WorldGen.mergeRight)
                    index4 = type;
                }
                if (index5 == 58)
                {
                  WorldGen.TileFrame(i + 1, j);
                  if (WorldGen.mergeLeft)
                    index5 = type;
                }
                if (index1 == 58)
                  index1 = type;
                if (index3 == 58)
                  index3 = type;
                if (index6 == 58)
                  index6 = type;
                if (index8 == 58)
                {
                  index8 = type;
                  break;
                }
                break;
              case 59:
                if (index2 == 60)
                  index2 = type;
                if (index7 == 60)
                  index7 = type;
                if (index4 == 60)
                  index4 = type;
                if (index5 == 60)
                  index5 = type;
                if (index1 == 60)
                  index1 = type;
                if (index3 == 60)
                  index3 = type;
                if (index6 == 60)
                  index6 = type;
                if (index8 == 60)
                  index8 = type;
                if (index2 == 70)
                  index2 = type;
                if (index7 == 70)
                  index7 = type;
                if (index4 == 70)
                  index4 = type;
                if (index5 == 70)
                  index5 = type;
                if (index1 == 70)
                  index1 = type;
                if (index3 == 70)
                  index3 = type;
                if (index6 == 70)
                  index6 = type;
                if (index8 == 70)
                {
                  index8 = type;
                  break;
                }
                break;
            }
            if (type == 1 || type == 6 || type == 7 || type == 8 || type == 9 || type == 22 || type == 25 || type == 37 || type == 40 || type == 53 || type == 56)
            {
              for (int index11 = 0; index11 < 76; ++index11)
              {
                if (index11 == 1 || index11 == 6 || index11 == 7 || index11 == 8 || index11 == 9 || index11 == 22 || index11 == 25 || index11 == 37 || index11 == 40 || index11 == 53 || index11 == 56)
                {
                  if (index2 == 0)
                    index2 = -2;
                  if (index7 == 0)
                    index7 = -2;
                  if (index4 == 0)
                    index4 = -2;
                  if (index5 == 0)
                    index5 = -2;
                  if (index1 == 0)
                    index1 = -2;
                  if (index3 == 0)
                    index3 = -2;
                  if (index6 == 0)
                    index6 = -2;
                  if (index8 == 0)
                    index8 = -2;
                }
              }
            }
            else
            {
              switch (type)
              {
                case 58:
                  if (index2 == 57)
                    index2 = -2;
                  if (index7 == 57)
                    index7 = -2;
                  if (index4 == 57)
                    index4 = -2;
                  if (index5 == 57)
                    index5 = -2;
                  if (index1 == 57)
                    index1 = -2;
                  if (index3 == 57)
                    index3 = -2;
                  if (index6 == 57)
                    index6 = -2;
                  if (index8 == 57)
                  {
                    index8 = -2;
                    break;
                  }
                  break;
                case 59:
                  if (index2 == 1)
                    index2 = -2;
                  if (index7 == 1)
                    index7 = -2;
                  if (index4 == 1)
                    index4 = -2;
                  if (index5 == 1)
                    index5 = -2;
                  if (index1 == 1)
                    index1 = -2;
                  if (index3 == 1)
                    index3 = -2;
                  if (index6 == 1)
                    index6 = -2;
                  if (index8 == 1)
                    index8 = -2;
                  break;
              }
            }
            if (type == 32 && index7 == 23)
              index7 = type;
            if (type == 69 && index7 == 60)
              index7 = type;
            if (type == 51)
            {
              if (index2 > -1 && !Game1.tileNoAttach[index2])
                index2 = type;
              if (index7 > -1 && !Game1.tileNoAttach[index7])
                index7 = type;
              if (index4 > -1 && !Game1.tileNoAttach[index4])
                index4 = type;
              if (index5 > -1 && !Game1.tileNoAttach[index5])
                index5 = type;
              if (index1 > -1 && !Game1.tileNoAttach[index1])
                index1 = type;
              if (index3 > -1 && !Game1.tileNoAttach[index3])
                index3 = type;
              if (index6 > -1 && !Game1.tileNoAttach[index6])
                index6 = type;
              if (index8 > -1 && !Game1.tileNoAttach[index8])
                index8 = type;
            }
            if (index2 > -1 && !Game1.tileSolid[index2] && index2 != type)
              index2 = -1;
            if (index7 > -1 && !Game1.tileSolid[index7] && index7 != type)
              index7 = -1;
            if (index4 > -1 && !Game1.tileSolid[index4] && index4 != type)
              index4 = -1;
            if (index5 > -1 && !Game1.tileSolid[index5] && index5 != type)
              index5 = -1;
            if (index1 > -1 && !Game1.tileSolid[index1] && index1 != type)
              index1 = -1;
            if (index3 > -1 && !Game1.tileSolid[index3] && index3 != type)
              index3 = -1;
            if (index6 > -1 && !Game1.tileSolid[index6] && index6 != type)
              index6 = -1;
            if (index8 > -1 && !Game1.tileSolid[index8] && index8 != type)
              index8 = -1;
            if (type == 2 || type == 23 || type == 60 || type == 70)
            {
              int num8 = 0;
              if (type == 60 || type == 70)
                num8 = 59;
              else if (type == 2)
              {
                if (index2 == 23)
                  index2 = num8;
                if (index7 == 23)
                  index7 = num8;
                if (index4 == 23)
                  index4 = num8;
                if (index5 == 23)
                  index5 = num8;
                if (index1 == 23)
                  index1 = num8;
                if (index3 == 23)
                  index3 = num8;
                if (index6 == 23)
                  index6 = num8;
                if (index8 == 23)
                  index8 = num8;
              }
              else if (type == 23)
              {
                if (index2 == 2)
                  index2 = num8;
                if (index7 == 2)
                  index7 = num8;
                if (index4 == 2)
                  index4 = num8;
                if (index5 == 2)
                  index5 = num8;
                if (index1 == 2)
                  index1 = num8;
                if (index3 == 2)
                  index3 = num8;
                if (index6 == 2)
                  index6 = num8;
                if (index8 == 2)
                  index8 = num8;
              }
              if (index2 != type && index2 != num8 && (index7 == type || index7 == num8))
              {
                if (index4 == num8 && index5 == type)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 0;
                    rectangle.Y = 198;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 18;
                    rectangle.Y = 198;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 36;
                    rectangle.Y = 198;
                  }
                }
                else if (index4 == type && index5 == num8)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 54;
                    rectangle.Y = 198;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 72;
                    rectangle.Y = 198;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 90;
                    rectangle.Y = 198;
                  }
                }
              }
              else if (index7 != type && index7 != num8 && (index2 == type || index2 == num8))
              {
                if (index4 == num8 && index5 == type)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 0;
                    rectangle.Y = 216;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 18;
                    rectangle.Y = 216;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 36;
                    rectangle.Y = 216;
                  }
                }
                else if (index4 == type && index5 == num8)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 54;
                    rectangle.Y = 216;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 72;
                    rectangle.Y = 216;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 90;
                    rectangle.Y = 216;
                  }
                }
              }
              else if (index4 != type && index4 != num8 && (index5 == type || index5 == num8))
              {
                if (index2 == num8 && index7 == type)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 72;
                    rectangle.Y = 144;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 72;
                    rectangle.Y = 162;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 72;
                    rectangle.Y = 180;
                  }
                }
                else if (index7 == type && index5 == index2)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 72;
                    rectangle.Y = 90;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 72;
                    rectangle.Y = 108;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 72;
                    rectangle.Y = 126;
                  }
                }
              }
              else if (index5 != type && index5 != num8 && (index4 == type || index4 == num8))
              {
                if (index2 == num8 && index7 == type)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 90;
                    rectangle.Y = 144;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 90;
                    rectangle.Y = 162;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 90;
                    rectangle.Y = 180;
                  }
                }
                else if (index7 == type && index5 == index2)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 90;
                    rectangle.Y = 90;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 90;
                    rectangle.Y = 108;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 90;
                    rectangle.Y = 126;
                  }
                }
              }
              else if (index2 == type && index7 == type && index4 == type && index5 == type)
              {
                if (index1 != type && index3 != type && index6 != type && index8 != type)
                {
                  if (index8 == num8)
                  {
                    if (num7 == 0)
                    {
                      rectangle.X = 108;
                      rectangle.Y = 324;
                    }
                    if (num7 == 1)
                    {
                      rectangle.X = 126;
                      rectangle.Y = 324;
                    }
                    if (num7 == 2)
                    {
                      rectangle.X = 144;
                      rectangle.Y = 324;
                    }
                  }
                  else if (index3 == num8)
                  {
                    if (num7 == 0)
                    {
                      rectangle.X = 108;
                      rectangle.Y = 342;
                    }
                    if (num7 == 1)
                    {
                      rectangle.X = 126;
                      rectangle.Y = 342;
                    }
                    if (num7 == 2)
                    {
                      rectangle.X = 144;
                      rectangle.Y = 342;
                    }
                  }
                  else if (index6 == num8)
                  {
                    if (num7 == 0)
                    {
                      rectangle.X = 108;
                      rectangle.Y = 360;
                    }
                    if (num7 == 1)
                    {
                      rectangle.X = 126;
                      rectangle.Y = 360;
                    }
                    if (num7 == 2)
                    {
                      rectangle.X = 144;
                      rectangle.Y = 360;
                    }
                  }
                  else if (index1 == num8)
                  {
                    if (num7 == 0)
                    {
                      rectangle.X = 108;
                      rectangle.Y = 378;
                    }
                    if (num7 == 1)
                    {
                      rectangle.X = 126;
                      rectangle.Y = 378;
                    }
                    if (num7 == 2)
                    {
                      rectangle.X = 144;
                      rectangle.Y = 378;
                    }
                  }
                  else
                  {
                    if (num7 == 0)
                    {
                      rectangle.X = 144;
                      rectangle.Y = 234;
                    }
                    if (num7 == 1)
                    {
                      rectangle.X = 198;
                      rectangle.Y = 234;
                    }
                    if (num7 == 2)
                    {
                      rectangle.X = 252;
                      rectangle.Y = 234;
                    }
                  }
                }
                else if (index1 != type && index8 != type)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 36;
                    rectangle.Y = 306;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 54;
                    rectangle.Y = 306;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 72;
                    rectangle.Y = 306;
                  }
                }
                else if (index3 != type && index6 != type)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 90;
                    rectangle.Y = 306;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 108;
                    rectangle.Y = 306;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 126;
                    rectangle.Y = 306;
                  }
                }
                else if (index1 != type && index3 == type && index6 == type && index8 == type)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 54;
                    rectangle.Y = 108;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 54;
                    rectangle.Y = 144;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 54;
                    rectangle.Y = 180;
                  }
                }
                else if (index1 == type && index3 != type && index6 == type && index8 == type)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 36;
                    rectangle.Y = 108;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 36;
                    rectangle.Y = 144;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 36;
                    rectangle.Y = 180;
                  }
                }
                else if (index1 == type && index3 == type && index6 != type && index8 == type)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 54;
                    rectangle.Y = 90;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 54;
                    rectangle.Y = 126;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 54;
                    rectangle.Y = 162;
                  }
                }
                else if (index1 == type && index3 == type && index6 == type && index8 != type)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 36;
                    rectangle.Y = 90;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 36;
                    rectangle.Y = 126;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 36;
                    rectangle.Y = 162;
                  }
                }
              }
              else if (index2 == type && index7 == num8 && index4 == type && index5 == type && index1 == -1 && index3 == -1)
              {
                if (num7 == 0)
                {
                  rectangle.X = 108;
                  rectangle.Y = 18;
                }
                if (num7 == 1)
                {
                  rectangle.X = 126;
                  rectangle.Y = 18;
                }
                if (num7 == 2)
                {
                  rectangle.X = 144;
                  rectangle.Y = 18;
                }
              }
              else if (index2 == num8 && index7 == type && index4 == type && index5 == type && index6 == -1 && index8 == -1)
              {
                if (num7 == 0)
                {
                  rectangle.X = 108;
                  rectangle.Y = 36;
                }
                if (num7 == 1)
                {
                  rectangle.X = 126;
                  rectangle.Y = 36;
                }
                if (num7 == 2)
                {
                  rectangle.X = 144;
                  rectangle.Y = 36;
                }
              }
              else if (index2 == type && index7 == type && index4 == num8 && index5 == type && index3 == -1 && index8 == -1)
              {
                if (num7 == 0)
                {
                  rectangle.X = 198;
                  rectangle.Y = 0;
                }
                if (num7 == 1)
                {
                  rectangle.X = 198;
                  rectangle.Y = 18;
                }
                if (num7 == 2)
                {
                  rectangle.X = 198;
                  rectangle.Y = 36;
                }
              }
              else if (index2 == type && index7 == type && index4 == type && index5 == num8 && index1 == -1 && index6 == -1)
              {
                if (num7 == 0)
                {
                  rectangle.X = 180;
                  rectangle.Y = 0;
                }
                if (num7 == 1)
                {
                  rectangle.X = 180;
                  rectangle.Y = 18;
                }
                if (num7 == 2)
                {
                  rectangle.X = 180;
                  rectangle.Y = 36;
                }
              }
              else if (index2 == type && index7 == num8 && index4 == type && index5 == type)
              {
                if (index3 != -1)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 54;
                    rectangle.Y = 108;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 54;
                    rectangle.Y = 144;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 54;
                    rectangle.Y = 180;
                  }
                }
                else if (index1 != -1)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 36;
                    rectangle.Y = 108;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 36;
                    rectangle.Y = 144;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 36;
                    rectangle.Y = 180;
                  }
                }
              }
              else if (index2 == num8 && index7 == type && index4 == type && index5 == type)
              {
                if (index8 != -1)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 54;
                    rectangle.Y = 90;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 54;
                    rectangle.Y = 126;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 54;
                    rectangle.Y = 162;
                  }
                }
                else if (index6 != -1)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 36;
                    rectangle.Y = 90;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 36;
                    rectangle.Y = 126;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 36;
                    rectangle.Y = 162;
                  }
                }
              }
              else if (index2 == type && index7 == type && index4 == type && index5 == num8)
              {
                if (index1 != -1)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 54;
                    rectangle.Y = 90;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 54;
                    rectangle.Y = 126;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 54;
                    rectangle.Y = 162;
                  }
                }
                else if (index6 != -1)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 54;
                    rectangle.Y = 108;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 54;
                    rectangle.Y = 144;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 54;
                    rectangle.Y = 180;
                  }
                }
              }
              else if (index2 == type && index7 == type && index4 == num8 && index5 == type)
              {
                if (index3 != -1)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 36;
                    rectangle.Y = 90;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 36;
                    rectangle.Y = 126;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 36;
                    rectangle.Y = 162;
                  }
                }
                else if (index8 != -1)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 36;
                    rectangle.Y = 108;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 36;
                    rectangle.Y = 144;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 36;
                    rectangle.Y = 180;
                  }
                }
              }
              else if (index2 == num8 && index7 == type && index4 == type && index5 == type || index2 == type && index7 == num8 && index4 == type && index5 == type || index2 == type && index7 == type && index4 == num8 && index5 == type || index2 == type && index7 == type && index4 == type && index5 == num8)
              {
                if (num7 == 0)
                {
                  rectangle.X = 18;
                  rectangle.Y = 18;
                }
                if (num7 == 1)
                {
                  rectangle.X = 36;
                  rectangle.Y = 18;
                }
                if (num7 == 2)
                {
                  rectangle.X = 54;
                  rectangle.Y = 18;
                }
              }
              if ((index2 == type || index2 == num8) && (index7 == type || index7 == num8) && (index4 == type || index4 == num8) && (index5 == type || index5 == num8))
              {
                if (index1 != type && index1 != num8 && (index3 == type || index3 == num8) && (index6 == type || index6 == num8) && (index8 == type || index8 == num8))
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 54;
                    rectangle.Y = 108;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 54;
                    rectangle.Y = 144;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 54;
                    rectangle.Y = 180;
                  }
                }
                else if (index3 != type && index3 != num8 && (index1 == type || index1 == num8) && (index6 == type || index6 == num8) && (index8 == type || index8 == num8))
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 36;
                    rectangle.Y = 108;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 36;
                    rectangle.Y = 144;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 36;
                    rectangle.Y = 180;
                  }
                }
                else if (index6 != type && index6 != num8 && (index1 == type || index1 == num8) && (index3 == type || index3 == num8) && (index8 == type || index8 == num8))
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 54;
                    rectangle.Y = 90;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 54;
                    rectangle.Y = 126;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 54;
                    rectangle.Y = 162;
                  }
                }
                else if (index8 != type && index8 != num8 && (index1 == type || index1 == num8) && (index6 == type || index6 == num8) && (index3 == type || index3 == num8))
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 36;
                    rectangle.Y = 90;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 36;
                    rectangle.Y = 126;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 36;
                    rectangle.Y = 162;
                  }
                }
              }
              if (index2 != num8 && index2 != type && index7 == type && index4 != num8 && index4 != type && index5 == type && index8 != num8 && index8 != type)
              {
                if (num7 == 0)
                {
                  rectangle.X = 90;
                  rectangle.Y = 270;
                }
                if (num7 == 1)
                {
                  rectangle.X = 108;
                  rectangle.Y = 270;
                }
                if (num7 == 2)
                {
                  rectangle.X = 126;
                  rectangle.Y = 270;
                }
              }
              else if (index2 != num8 && index2 != type && index7 == type && index4 == type && index5 != num8 && index5 != type && index6 != num8 && index6 != type)
              {
                if (num7 == 0)
                {
                  rectangle.X = 144;
                  rectangle.Y = 270;
                }
                if (num7 == 1)
                {
                  rectangle.X = 162;
                  rectangle.Y = 270;
                }
                if (num7 == 2)
                {
                  rectangle.X = 180;
                  rectangle.Y = 270;
                }
              }
              else if (index7 != num8 && index7 != type && index2 == type && index4 != num8 && index4 != type && index5 == type && index3 != num8 && index3 != type)
              {
                if (num7 == 0)
                {
                  rectangle.X = 90;
                  rectangle.Y = 288;
                }
                if (num7 == 1)
                {
                  rectangle.X = 108;
                  rectangle.Y = 288;
                }
                if (num7 == 2)
                {
                  rectangle.X = 126;
                  rectangle.Y = 288;
                }
              }
              else if (index7 != num8 && index7 != type && index2 == type && index4 == type && index5 != num8 && index5 != type && index1 != num8 && index1 != type)
              {
                if (num7 == 0)
                {
                  rectangle.X = 144;
                  rectangle.Y = 288;
                }
                if (num7 == 1)
                {
                  rectangle.X = 162;
                  rectangle.Y = 288;
                }
                if (num7 == 2)
                {
                  rectangle.X = 180;
                  rectangle.Y = 288;
                }
              }
              else if (index2 != type && index2 != num8 && index7 == type && index4 == type && index5 == type && index6 != type && index6 != num8 && index8 != type && index8 != num8)
              {
                if (num7 == 0)
                {
                  rectangle.X = 144;
                  rectangle.Y = 216;
                }
                if (num7 == 1)
                {
                  rectangle.X = 198;
                  rectangle.Y = 216;
                }
                if (num7 == 2)
                {
                  rectangle.X = 252;
                  rectangle.Y = 216;
                }
              }
              else if (index7 != type && index7 != num8 && index2 == type && index4 == type && index5 == type && index1 != type && index1 != num8 && index3 != type && index3 != num8)
              {
                if (num7 == 0)
                {
                  rectangle.X = 144;
                  rectangle.Y = 252;
                }
                if (num7 == 1)
                {
                  rectangle.X = 198;
                  rectangle.Y = 252;
                }
                if (num7 == 2)
                {
                  rectangle.X = 252;
                  rectangle.Y = 252;
                }
              }
              else if (index4 != type && index4 != num8 && index7 == type && index2 == type && index5 == type && index3 != type && index3 != num8 && index8 != type && index8 != num8)
              {
                if (num7 == 0)
                {
                  rectangle.X = 126;
                  rectangle.Y = 234;
                }
                if (num7 == 1)
                {
                  rectangle.X = 180;
                  rectangle.Y = 234;
                }
                if (num7 == 2)
                {
                  rectangle.X = 234;
                  rectangle.Y = 234;
                }
              }
              else if (index5 != type && index5 != num8 && index7 == type && index2 == type && index4 == type && index1 != type && index1 != num8 && index6 != type && index6 != num8)
              {
                if (num7 == 0)
                {
                  rectangle.X = 162;
                  rectangle.Y = 234;
                }
                if (num7 == 1)
                {
                  rectangle.X = 216;
                  rectangle.Y = 234;
                }
                if (num7 == 2)
                {
                  rectangle.X = 270;
                  rectangle.Y = 234;
                }
              }
              else if (index2 != num8 && index2 != type && (index7 == num8 || index7 == type) && index4 == num8 && index5 == num8)
              {
                if (num7 == 0)
                {
                  rectangle.X = 36;
                  rectangle.Y = 270;
                }
                if (num7 == 1)
                {
                  rectangle.X = 54;
                  rectangle.Y = 270;
                }
                if (num7 == 2)
                {
                  rectangle.X = 72;
                  rectangle.Y = 270;
                }
              }
              else if (index7 != num8 && index7 != type && (index2 == num8 || index2 == type) && index4 == num8 && index5 == num8)
              {
                if (num7 == 0)
                {
                  rectangle.X = 36;
                  rectangle.Y = 288;
                }
                if (num7 == 1)
                {
                  rectangle.X = 54;
                  rectangle.Y = 288;
                }
                if (num7 == 2)
                {
                  rectangle.X = 72;
                  rectangle.Y = 288;
                }
              }
              else if (index4 != num8 && index4 != type && (index5 == num8 || index5 == type) && index2 == num8 && index7 == num8)
              {
                if (num7 == 0)
                {
                  rectangle.X = 0;
                  rectangle.Y = 270;
                }
                if (num7 == 1)
                {
                  rectangle.X = 0;
                  rectangle.Y = 288;
                }
                if (num7 == 2)
                {
                  rectangle.X = 0;
                  rectangle.Y = 306;
                }
              }
              else if (index5 != num8 && index5 != type && (index4 == num8 || index4 == type) && index2 == num8 && index7 == num8)
              {
                if (num7 == 0)
                {
                  rectangle.X = 18;
                  rectangle.Y = 270;
                }
                if (num7 == 1)
                {
                  rectangle.X = 18;
                  rectangle.Y = 288;
                }
                if (num7 == 2)
                {
                  rectangle.X = 18;
                  rectangle.Y = 306;
                }
              }
              else if (index2 == type && index7 == num8 && index4 == num8 && index5 == num8)
              {
                if (num7 == 0)
                {
                  rectangle.X = 198;
                  rectangle.Y = 288;
                }
                if (num7 == 1)
                {
                  rectangle.X = 216;
                  rectangle.Y = 288;
                }
                if (num7 == 2)
                {
                  rectangle.X = 234;
                  rectangle.Y = 288;
                }
              }
              else if (index2 == num8 && index7 == type && index4 == num8 && index5 == num8)
              {
                if (num7 == 0)
                {
                  rectangle.X = 198;
                  rectangle.Y = 270;
                }
                if (num7 == 1)
                {
                  rectangle.X = 216;
                  rectangle.Y = 270;
                }
                if (num7 == 2)
                {
                  rectangle.X = 234;
                  rectangle.Y = 270;
                }
              }
              else if (index2 == num8 && index7 == num8 && index4 == type && index5 == num8)
              {
                if (num7 == 0)
                {
                  rectangle.X = 198;
                  rectangle.Y = 306;
                }
                if (num7 == 1)
                {
                  rectangle.X = 216;
                  rectangle.Y = 306;
                }
                if (num7 == 2)
                {
                  rectangle.X = 234;
                  rectangle.Y = 306;
                }
              }
              else if (index2 == num8 && index7 == num8 && index4 == num8 && index5 == type)
              {
                if (num7 == 0)
                {
                  rectangle.X = 144;
                  rectangle.Y = 306;
                }
                if (num7 == 1)
                {
                  rectangle.X = 162;
                  rectangle.Y = 306;
                }
                if (num7 == 2)
                {
                  rectangle.X = 180;
                  rectangle.Y = 306;
                }
              }
              if (index2 != type && index2 != num8 && index7 == type && index4 == type && index5 == type)
              {
                if ((index6 == num8 || index6 == type) && index8 != num8 && index8 != type)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 0;
                    rectangle.Y = 324;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 18;
                    rectangle.Y = 324;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 36;
                    rectangle.Y = 324;
                  }
                }
                else if ((index8 == num8 || index8 == type) && index6 != num8 && index6 != type)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 54;
                    rectangle.Y = 324;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 72;
                    rectangle.Y = 324;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 90;
                    rectangle.Y = 324;
                  }
                }
              }
              else if (index7 != type && index7 != num8 && index2 == type && index4 == type && index5 == type)
              {
                if ((index1 == num8 || index1 == type) && index3 != num8 && index3 != type)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 0;
                    rectangle.Y = 342;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 18;
                    rectangle.Y = 342;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 36;
                    rectangle.Y = 342;
                  }
                }
                else if ((index3 == num8 || index3 == type) && index1 != num8 && index1 != type)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 54;
                    rectangle.Y = 342;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 72;
                    rectangle.Y = 342;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 90;
                    rectangle.Y = 342;
                  }
                }
              }
              else if (index4 != type && index4 != num8 && index2 == type && index7 == type && index5 == type)
              {
                if ((index3 == num8 || index3 == type) && index8 != num8 && index8 != type)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 54;
                    rectangle.Y = 360;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 72;
                    rectangle.Y = 360;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 90;
                    rectangle.Y = 360;
                  }
                }
                else if ((index8 == num8 || index8 == type) && index3 != num8 && index3 != type)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 0;
                    rectangle.Y = 360;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 18;
                    rectangle.Y = 360;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 36;
                    rectangle.Y = 360;
                  }
                }
              }
              else if (index5 != type && index5 != num8 && index2 == type && index7 == type && index4 == type)
              {
                if ((index1 == num8 || index1 == type) && index6 != num8 && index6 != type)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 0;
                    rectangle.Y = 378;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 18;
                    rectangle.Y = 378;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 36;
                    rectangle.Y = 378;
                  }
                }
                else if ((index6 == num8 || index6 == type) && index1 != num8 && index1 != type)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 54;
                    rectangle.Y = 378;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 72;
                    rectangle.Y = 378;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 90;
                    rectangle.Y = 378;
                  }
                }
              }
              if ((index2 == type || index2 == num8) && (index7 == type || index7 == num8) && (index4 == type || index4 == num8) && (index5 == type || index5 == num8) && index1 != -1 && index3 != -1 && index6 != -1 && index8 != -1)
              {
                if (num7 == 0)
                {
                  rectangle.X = 18;
                  rectangle.Y = 18;
                }
                if (num7 == 1)
                {
                  rectangle.X = 36;
                  rectangle.Y = 18;
                }
                if (num7 == 2)
                {
                  rectangle.X = 54;
                  rectangle.Y = 18;
                }
              }
              if (index2 == num8)
                index2 = -2;
              if (index7 == num8)
                index7 = -2;
              if (index4 == num8)
                index4 = -2;
              if (index5 == num8)
                index5 = -2;
              if (index1 == num8)
                index1 = -2;
              if (index3 == num8)
                index3 = -2;
              if (index6 == num8)
                index6 = -2;
              if (index8 == num8)
                index8 = -2;
            }
            if ((type == 1 || type == 2 || type == 6 || type == 7 || type == 8 || type == 9 || type == 22 || type == 23 || type == 25 || type == 37 || type == 40 || type == 53 || type == 56 || type == 58 || type == 59 || type == 60 || type == 70) && rectangle.X == -1 && rectangle.Y == -1)
            {
              if (index2 >= 0 && index2 != type)
                index2 = -1;
              if (index7 >= 0 && index7 != type)
                index7 = -1;
              if (index4 >= 0 && index4 != type)
                index4 = -1;
              if (index5 >= 0 && index5 != type)
                index5 = -1;
              if (index2 != -1 && index7 != -1 && index4 != -1 && index5 != -1)
              {
                if (index2 == -2 && index7 == type && index4 == type && index5 == type)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 144;
                    rectangle.Y = 108;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 162;
                    rectangle.Y = 108;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 180;
                    rectangle.Y = 108;
                  }
                  WorldGen.mergeUp = true;
                }
                else if (index2 == type && index7 == -2 && index4 == type && index5 == type)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 144;
                    rectangle.Y = 90;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 162;
                    rectangle.Y = 90;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 180;
                    rectangle.Y = 90;
                  }
                  WorldGen.mergeDown = true;
                }
                else if (index2 == type && index7 == type && index4 == -2 && index5 == type)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 162;
                    rectangle.Y = 126;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 162;
                    rectangle.Y = 144;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 162;
                    rectangle.Y = 162;
                  }
                  WorldGen.mergeLeft = true;
                }
                else if (index2 == type && index7 == type && index4 == type && index5 == -2)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 144;
                    rectangle.Y = 126;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 144;
                    rectangle.Y = 144;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 144;
                    rectangle.Y = 162;
                  }
                  WorldGen.mergeRight = true;
                }
                else if (index2 == -2 && index7 == type && index4 == -2 && index5 == type)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 36;
                    rectangle.Y = 90;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 36;
                    rectangle.Y = 126;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 36;
                    rectangle.Y = 162;
                  }
                  WorldGen.mergeUp = true;
                  WorldGen.mergeLeft = true;
                }
                else if (index2 == -2 && index7 == type && index4 == type && index5 == -2)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 54;
                    rectangle.Y = 90;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 54;
                    rectangle.Y = 126;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 54;
                    rectangle.Y = 162;
                  }
                  WorldGen.mergeUp = true;
                  WorldGen.mergeRight = true;
                }
                else if (index2 == type && index7 == -2 && index4 == -2 && index5 == type)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 36;
                    rectangle.Y = 108;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 36;
                    rectangle.Y = 144;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 36;
                    rectangle.Y = 180;
                  }
                  WorldGen.mergeDown = true;
                  WorldGen.mergeLeft = true;
                }
                else if (index2 == type && index7 == -2 && index4 == type && index5 == -2)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 54;
                    rectangle.Y = 108;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 54;
                    rectangle.Y = 144;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 54;
                    rectangle.Y = 180;
                  }
                  WorldGen.mergeDown = true;
                  WorldGen.mergeRight = true;
                }
                else if (index2 == type && index7 == type && index4 == -2 && index5 == -2)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 180;
                    rectangle.Y = 126;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 180;
                    rectangle.Y = 144;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 180;
                    rectangle.Y = 162;
                  }
                  WorldGen.mergeLeft = true;
                  WorldGen.mergeRight = true;
                }
                else if (index2 == -2 && index7 == -2 && index4 == type && index5 == type)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 144;
                    rectangle.Y = 180;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 162;
                    rectangle.Y = 180;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 180;
                    rectangle.Y = 180;
                  }
                  WorldGen.mergeUp = true;
                  WorldGen.mergeDown = true;
                }
                else if (index2 == -2 && index7 == type && index4 == -2 && index5 == -2)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 198;
                    rectangle.Y = 90;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 198;
                    rectangle.Y = 108;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 198;
                    rectangle.Y = 126;
                  }
                  WorldGen.mergeUp = true;
                  WorldGen.mergeLeft = true;
                  WorldGen.mergeRight = true;
                }
                else if (index2 == type && index7 == -2 && index4 == -2 && index5 == -2)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 198;
                    rectangle.Y = 144;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 198;
                    rectangle.Y = 162;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 198;
                    rectangle.Y = 180;
                  }
                  WorldGen.mergeDown = true;
                  WorldGen.mergeLeft = true;
                  WorldGen.mergeRight = true;
                }
                else if (index2 == -2 && index7 == -2 && index4 == type && index5 == -2)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 216;
                    rectangle.Y = 144;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 216;
                    rectangle.Y = 162;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 216;
                    rectangle.Y = 180;
                  }
                  WorldGen.mergeUp = true;
                  WorldGen.mergeDown = true;
                  WorldGen.mergeRight = true;
                }
                else if (index2 == -2 && index7 == -2 && index4 == -2 && index5 == type)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 216;
                    rectangle.Y = 90;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 216;
                    rectangle.Y = 108;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 216;
                    rectangle.Y = 126;
                  }
                  WorldGen.mergeUp = true;
                  WorldGen.mergeDown = true;
                  WorldGen.mergeLeft = true;
                }
                else if (index2 == -2 && index7 == -2 && index4 == -2 && index5 == -2)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 108;
                    rectangle.Y = 198;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 126;
                    rectangle.Y = 198;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 144;
                    rectangle.Y = 198;
                  }
                  WorldGen.mergeUp = true;
                  WorldGen.mergeDown = true;
                  WorldGen.mergeLeft = true;
                  WorldGen.mergeRight = true;
                }
                else if (index2 == type && index7 == type && index4 == type && index5 == type)
                {
                  if (index1 == -2)
                  {
                    if (num7 == 0)
                    {
                      rectangle.X = 18;
                      rectangle.Y = 108;
                    }
                    if (num7 == 1)
                    {
                      rectangle.X = 18;
                      rectangle.Y = 144;
                    }
                    if (num7 == 2)
                    {
                      rectangle.X = 18;
                      rectangle.Y = 180;
                    }
                  }
                  if (index3 == -2)
                  {
                    if (num7 == 0)
                    {
                      rectangle.X = 0;
                      rectangle.Y = 108;
                    }
                    if (num7 == 1)
                    {
                      rectangle.X = 0;
                      rectangle.Y = 144;
                    }
                    if (num7 == 2)
                    {
                      rectangle.X = 0;
                      rectangle.Y = 180;
                    }
                  }
                  if (index6 == -2)
                  {
                    if (num7 == 0)
                    {
                      rectangle.X = 18;
                      rectangle.Y = 90;
                    }
                    if (num7 == 1)
                    {
                      rectangle.X = 18;
                      rectangle.Y = 126;
                    }
                    if (num7 == 2)
                    {
                      rectangle.X = 18;
                      rectangle.Y = 162;
                    }
                  }
                  if (index8 == -2)
                  {
                    if (num7 == 0)
                    {
                      rectangle.X = 0;
                      rectangle.Y = 90;
                    }
                    if (num7 == 1)
                    {
                      rectangle.X = 0;
                      rectangle.Y = 126;
                    }
                    if (num7 == 2)
                    {
                      rectangle.X = 0;
                      rectangle.Y = 162;
                    }
                  }
                }
              }
              else
              {
                if (type != 2 && type != 23 && type != 60 && type != 70)
                {
                  if (index2 == -1 && index7 == -2 && index4 == type && index5 == type)
                  {
                    if (num7 == 0)
                    {
                      rectangle.X = 234;
                      rectangle.Y = 0;
                    }
                    if (num7 == 1)
                    {
                      rectangle.X = 252;
                      rectangle.Y = 0;
                    }
                    if (num7 == 2)
                    {
                      rectangle.X = 270;
                      rectangle.Y = 0;
                    }
                    WorldGen.mergeDown = true;
                  }
                  else if (index2 == -2 && index7 == -1 && index4 == type && index5 == type)
                  {
                    if (num7 == 0)
                    {
                      rectangle.X = 234;
                      rectangle.Y = 18;
                    }
                    if (num7 == 1)
                    {
                      rectangle.X = 252;
                      rectangle.Y = 18;
                    }
                    if (num7 == 2)
                    {
                      rectangle.X = 270;
                      rectangle.Y = 18;
                    }
                    WorldGen.mergeUp = true;
                  }
                  else if (index2 == type && index7 == type && index4 == -1 && index5 == -2)
                  {
                    if (num7 == 0)
                    {
                      rectangle.X = 234;
                      rectangle.Y = 36;
                    }
                    if (num7 == 1)
                    {
                      rectangle.X = 252;
                      rectangle.Y = 36;
                    }
                    if (num7 == 2)
                    {
                      rectangle.X = 270;
                      rectangle.Y = 36;
                    }
                    WorldGen.mergeRight = true;
                  }
                  else if (index2 == type && index7 == type && index4 == -2 && index5 == -1)
                  {
                    if (num7 == 0)
                    {
                      rectangle.X = 234;
                      rectangle.Y = 54;
                    }
                    if (num7 == 1)
                    {
                      rectangle.X = 252;
                      rectangle.Y = 54;
                    }
                    if (num7 == 2)
                    {
                      rectangle.X = 270;
                      rectangle.Y = 54;
                    }
                    WorldGen.mergeLeft = true;
                  }
                }
                if (index2 != -1 && index7 != -1 && index4 == -1 && index5 == type)
                {
                  if (index2 == -2 && index7 == type)
                  {
                    if (num7 == 0)
                    {
                      rectangle.X = 72;
                      rectangle.Y = 144;
                    }
                    if (num7 == 1)
                    {
                      rectangle.X = 72;
                      rectangle.Y = 162;
                    }
                    if (num7 == 2)
                    {
                      rectangle.X = 72;
                      rectangle.Y = 180;
                    }
                    WorldGen.mergeUp = true;
                  }
                  else if (index7 == -2 && index2 == type)
                  {
                    if (num7 == 0)
                    {
                      rectangle.X = 72;
                      rectangle.Y = 90;
                    }
                    if (num7 == 1)
                    {
                      rectangle.X = 72;
                      rectangle.Y = 108;
                    }
                    if (num7 == 2)
                    {
                      rectangle.X = 72;
                      rectangle.Y = 126;
                    }
                    WorldGen.mergeDown = true;
                  }
                }
                else if (index2 != -1 && index7 != -1 && index4 == type && index5 == -1)
                {
                  if (index2 == -2 && index7 == type)
                  {
                    if (num7 == 0)
                    {
                      rectangle.X = 90;
                      rectangle.Y = 144;
                    }
                    if (num7 == 1)
                    {
                      rectangle.X = 90;
                      rectangle.Y = 162;
                    }
                    if (num7 == 2)
                    {
                      rectangle.X = 90;
                      rectangle.Y = 180;
                    }
                    WorldGen.mergeUp = true;
                  }
                  else if (index7 == -2 && index2 == type)
                  {
                    if (num7 == 0)
                    {
                      rectangle.X = 90;
                      rectangle.Y = 90;
                    }
                    if (num7 == 1)
                    {
                      rectangle.X = 90;
                      rectangle.Y = 108;
                    }
                    if (num7 == 2)
                    {
                      rectangle.X = 90;
                      rectangle.Y = 126;
                    }
                    WorldGen.mergeDown = true;
                  }
                }
                else if (index2 == -1 && index7 == type && index4 != -1 && index5 != -1)
                {
                  if (index4 == -2 && index5 == type)
                  {
                    if (num7 == 0)
                    {
                      rectangle.X = 0;
                      rectangle.Y = 198;
                    }
                    if (num7 == 1)
                    {
                      rectangle.X = 18;
                      rectangle.Y = 198;
                    }
                    if (num7 == 2)
                    {
                      rectangle.X = 36;
                      rectangle.Y = 198;
                    }
                    WorldGen.mergeLeft = true;
                  }
                  else if (index5 == -2 && index4 == type)
                  {
                    if (num7 == 0)
                    {
                      rectangle.X = 54;
                      rectangle.Y = 198;
                    }
                    if (num7 == 1)
                    {
                      rectangle.X = 72;
                      rectangle.Y = 198;
                    }
                    if (num7 == 2)
                    {
                      rectangle.X = 90;
                      rectangle.Y = 198;
                    }
                    WorldGen.mergeRight = true;
                  }
                }
                else if (index2 == type && index7 == -1 && index4 != -1 && index5 != -1)
                {
                  if (index4 == -2 && index5 == type)
                  {
                    if (num7 == 0)
                    {
                      rectangle.X = 0;
                      rectangle.Y = 216;
                    }
                    if (num7 == 1)
                    {
                      rectangle.X = 18;
                      rectangle.Y = 216;
                    }
                    if (num7 == 2)
                    {
                      rectangle.X = 36;
                      rectangle.Y = 216;
                    }
                    WorldGen.mergeLeft = true;
                  }
                  else if (index5 == -2 && index4 == type)
                  {
                    if (num7 == 0)
                    {
                      rectangle.X = 54;
                      rectangle.Y = 216;
                    }
                    if (num7 == 1)
                    {
                      rectangle.X = 72;
                      rectangle.Y = 216;
                    }
                    if (num7 == 2)
                    {
                      rectangle.X = 90;
                      rectangle.Y = 216;
                    }
                    WorldGen.mergeRight = true;
                  }
                }
                else if (index2 != -1 && index7 != -1 && index4 == -1 && index5 == -1)
                {
                  if (index2 == -2 && index7 == -2)
                  {
                    if (num7 == 0)
                    {
                      rectangle.X = 108;
                      rectangle.Y = 216;
                    }
                    if (num7 == 1)
                    {
                      rectangle.X = 108;
                      rectangle.Y = 234;
                    }
                    if (num7 == 2)
                    {
                      rectangle.X = 108;
                      rectangle.Y = 252;
                    }
                    WorldGen.mergeUp = true;
                    WorldGen.mergeDown = true;
                  }
                  else if (index2 == -2)
                  {
                    if (num7 == 0)
                    {
                      rectangle.X = 126;
                      rectangle.Y = 144;
                    }
                    if (num7 == 1)
                    {
                      rectangle.X = 126;
                      rectangle.Y = 162;
                    }
                    if (num7 == 2)
                    {
                      rectangle.X = 126;
                      rectangle.Y = 180;
                    }
                    WorldGen.mergeUp = true;
                  }
                  else if (index7 == -2)
                  {
                    if (num7 == 0)
                    {
                      rectangle.X = 126;
                      rectangle.Y = 90;
                    }
                    if (num7 == 1)
                    {
                      rectangle.X = 126;
                      rectangle.Y = 108;
                    }
                    if (num7 == 2)
                    {
                      rectangle.X = 126;
                      rectangle.Y = 126;
                    }
                    WorldGen.mergeDown = true;
                  }
                }
                else if (index2 == -1 && index7 == -1 && index4 != -1 && index5 != -1)
                {
                  if (index4 == -2 && index5 == -2)
                  {
                    if (num7 == 0)
                    {
                      rectangle.X = 162;
                      rectangle.Y = 198;
                    }
                    if (num7 == 1)
                    {
                      rectangle.X = 180;
                      rectangle.Y = 198;
                    }
                    if (num7 == 2)
                    {
                      rectangle.X = 198;
                      rectangle.Y = 198;
                    }
                    WorldGen.mergeLeft = true;
                    WorldGen.mergeRight = true;
                  }
                  else if (index4 == -2)
                  {
                    if (num7 == 0)
                    {
                      rectangle.X = 0;
                      rectangle.Y = 252;
                    }
                    if (num7 == 1)
                    {
                      rectangle.X = 18;
                      rectangle.Y = 252;
                    }
                    if (num7 == 2)
                    {
                      rectangle.X = 36;
                      rectangle.Y = 252;
                    }
                    WorldGen.mergeLeft = true;
                  }
                  else if (index5 == -2)
                  {
                    if (num7 == 0)
                    {
                      rectangle.X = 54;
                      rectangle.Y = 252;
                    }
                    if (num7 == 1)
                    {
                      rectangle.X = 72;
                      rectangle.Y = 252;
                    }
                    if (num7 == 2)
                    {
                      rectangle.X = 90;
                      rectangle.Y = 252;
                    }
                    WorldGen.mergeRight = true;
                  }
                }
                else if (index2 == -2 && index7 == -1 && index4 == -1 && index5 == -1)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 108;
                    rectangle.Y = 144;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 108;
                    rectangle.Y = 162;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 108;
                    rectangle.Y = 180;
                  }
                  WorldGen.mergeUp = true;
                }
                else if (index2 == -1 && index7 == -2 && index4 == -1 && index5 == -1)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 108;
                    rectangle.Y = 90;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 108;
                    rectangle.Y = 108;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 108;
                    rectangle.Y = 126;
                  }
                  WorldGen.mergeDown = true;
                }
                else if (index2 == -1 && index7 == -1 && index4 == -2 && index5 == -1)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 0;
                    rectangle.Y = 234;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 18;
                    rectangle.Y = 234;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 36;
                    rectangle.Y = 234;
                  }
                  WorldGen.mergeLeft = true;
                }
                else if (index2 == -1 && index7 == -1 && index4 == -1 && index5 == -2)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 54;
                    rectangle.Y = 234;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 72;
                    rectangle.Y = 234;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 90;
                    rectangle.Y = 234;
                  }
                  WorldGen.mergeRight = true;
                }
              }
            }
            if (rectangle.X < 0 || rectangle.Y < 0)
            {
              if (type == 2 || type == 23 || type == 60 || type == 70)
              {
                if (index2 == -2)
                  index2 = type;
                if (index7 == -2)
                  index7 = type;
                if (index4 == -2)
                  index4 = type;
                if (index5 == -2)
                  index5 = type;
                if (index1 == -2)
                  index1 = type;
                if (index3 == -2)
                  index3 = type;
                if (index6 == -2)
                  index6 = type;
                if (index8 == -2)
                  index8 = type;
              }
              if (index2 == type && index7 == type && index4 == type & index5 == type)
              {
                if (index1 != type && index3 != type)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 108;
                    rectangle.Y = 18;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 126;
                    rectangle.Y = 18;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 144;
                    rectangle.Y = 18;
                  }
                }
                else if (index6 != type && index8 != type)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 108;
                    rectangle.Y = 36;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 126;
                    rectangle.Y = 36;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 144;
                    rectangle.Y = 36;
                  }
                }
                else if (index1 != type && index6 != type)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 180;
                    rectangle.Y = 0;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 180;
                    rectangle.Y = 18;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 180;
                    rectangle.Y = 36;
                  }
                }
                else if (index3 != type && index8 != type)
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 198;
                    rectangle.Y = 0;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 198;
                    rectangle.Y = 18;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 198;
                    rectangle.Y = 36;
                  }
                }
                else
                {
                  if (num7 == 0)
                  {
                    rectangle.X = 18;
                    rectangle.Y = 18;
                  }
                  if (num7 == 1)
                  {
                    rectangle.X = 36;
                    rectangle.Y = 18;
                  }
                  if (num7 == 2)
                  {
                    rectangle.X = 54;
                    rectangle.Y = 18;
                  }
                }
              }
              else if (index2 != type && index7 == type && index4 == type & index5 == type)
              {
                if (num7 == 0)
                {
                  rectangle.X = 18;
                  rectangle.Y = 0;
                }
                if (num7 == 1)
                {
                  rectangle.X = 36;
                  rectangle.Y = 0;
                }
                if (num7 == 2)
                {
                  rectangle.X = 54;
                  rectangle.Y = 0;
                }
              }
              else if (index2 == type && index7 != type && index4 == type & index5 == type)
              {
                if (num7 == 0)
                {
                  rectangle.X = 18;
                  rectangle.Y = 36;
                }
                if (num7 == 1)
                {
                  rectangle.X = 36;
                  rectangle.Y = 36;
                }
                if (num7 == 2)
                {
                  rectangle.X = 54;
                  rectangle.Y = 36;
                }
              }
              else if (index2 == type && index7 == type && index4 != type & index5 == type)
              {
                if (num7 == 0)
                {
                  rectangle.X = 0;
                  rectangle.Y = 0;
                }
                if (num7 == 1)
                {
                  rectangle.X = 0;
                  rectangle.Y = 18;
                }
                if (num7 == 2)
                {
                  rectangle.X = 0;
                  rectangle.Y = 36;
                }
              }
              else if (index2 == type && index7 == type && index4 == type & index5 != type)
              {
                if (num7 == 0)
                {
                  rectangle.X = 72;
                  rectangle.Y = 0;
                }
                if (num7 == 1)
                {
                  rectangle.X = 72;
                  rectangle.Y = 18;
                }
                if (num7 == 2)
                {
                  rectangle.X = 72;
                  rectangle.Y = 36;
                }
              }
              else if (index2 != type && index7 == type && index4 != type & index5 == type)
              {
                if (num7 == 0)
                {
                  rectangle.X = 0;
                  rectangle.Y = 54;
                }
                if (num7 == 1)
                {
                  rectangle.X = 36;
                  rectangle.Y = 54;
                }
                if (num7 == 2)
                {
                  rectangle.X = 72;
                  rectangle.Y = 54;
                }
              }
              else if (index2 != type && index7 == type && index4 == type & index5 != type)
              {
                if (num7 == 0)
                {
                  rectangle.X = 18;
                  rectangle.Y = 54;
                }
                if (num7 == 1)
                {
                  rectangle.X = 54;
                  rectangle.Y = 54;
                }
                if (num7 == 2)
                {
                  rectangle.X = 90;
                  rectangle.Y = 54;
                }
              }
              else if (index2 == type && index7 != type && index4 != type & index5 == type)
              {
                if (num7 == 0)
                {
                  rectangle.X = 0;
                  rectangle.Y = 72;
                }
                if (num7 == 1)
                {
                  rectangle.X = 36;
                  rectangle.Y = 72;
                }
                if (num7 == 2)
                {
                  rectangle.X = 72;
                  rectangle.Y = 72;
                }
              }
              else if (index2 == type && index7 != type && index4 == type & index5 != type)
              {
                if (num7 == 0)
                {
                  rectangle.X = 18;
                  rectangle.Y = 72;
                }
                if (num7 == 1)
                {
                  rectangle.X = 54;
                  rectangle.Y = 72;
                }
                if (num7 == 2)
                {
                  rectangle.X = 90;
                  rectangle.Y = 72;
                }
              }
              else if (index2 == type && index7 == type && index4 != type & index5 != type)
              {
                if (num7 == 0)
                {
                  rectangle.X = 90;
                  rectangle.Y = 0;
                }
                if (num7 == 1)
                {
                  rectangle.X = 90;
                  rectangle.Y = 18;
                }
                if (num7 == 2)
                {
                  rectangle.X = 90;
                  rectangle.Y = 36;
                }
              }
              else if (index2 != type && index7 != type && index4 == type & index5 == type)
              {
                if (num7 == 0)
                {
                  rectangle.X = 108;
                  rectangle.Y = 72;
                }
                if (num7 == 1)
                {
                  rectangle.X = 126;
                  rectangle.Y = 72;
                }
                if (num7 == 2)
                {
                  rectangle.X = 144;
                  rectangle.Y = 72;
                }
              }
              else if (index2 != type && index7 == type && index4 != type & index5 != type)
              {
                if (num7 == 0)
                {
                  rectangle.X = 108;
                  rectangle.Y = 0;
                }
                if (num7 == 1)
                {
                  rectangle.X = 126;
                  rectangle.Y = 0;
                }
                if (num7 == 2)
                {
                  rectangle.X = 144;
                  rectangle.Y = 0;
                }
              }
              else if (index2 == type && index7 != type && index4 != type & index5 != type)
              {
                if (num7 == 0)
                {
                  rectangle.X = 108;
                  rectangle.Y = 54;
                }
                if (num7 == 1)
                {
                  rectangle.X = 126;
                  rectangle.Y = 54;
                }
                if (num7 == 2)
                {
                  rectangle.X = 144;
                  rectangle.Y = 54;
                }
              }
              else if (index2 != type && index7 != type && index4 != type & index5 == type)
              {
                if (num7 == 0)
                {
                  rectangle.X = 162;
                  rectangle.Y = 0;
                }
                if (num7 == 1)
                {
                  rectangle.X = 162;
                  rectangle.Y = 18;
                }
                if (num7 == 2)
                {
                  rectangle.X = 162;
                  rectangle.Y = 36;
                }
              }
              else if (index2 != type && index7 != type && index4 == type & index5 != type)
              {
                if (num7 == 0)
                {
                  rectangle.X = 216;
                  rectangle.Y = 0;
                }
                if (num7 == 1)
                {
                  rectangle.X = 216;
                  rectangle.Y = 18;
                }
                if (num7 == 2)
                {
                  rectangle.X = 216;
                  rectangle.Y = 36;
                }
              }
              else if (index2 != type && index7 != type && index4 != type & index5 != type)
              {
                if (num7 == 0)
                {
                  rectangle.X = 162;
                  rectangle.Y = 54;
                }
                if (num7 == 1)
                {
                  rectangle.X = 180;
                  rectangle.Y = 54;
                }
                if (num7 == 2)
                {
                  rectangle.X = 198;
                  rectangle.Y = 54;
                }
              }
            }
            if (rectangle.X <= -1 || rectangle.Y <= -1)
            {
              if (num7 <= 0)
              {
                rectangle.X = 18;
                rectangle.Y = 18;
              }
              if (num7 == 1)
              {
                rectangle.X = 36;
                rectangle.Y = 18;
              }
              if (num7 >= 2)
              {
                rectangle.X = 54;
                rectangle.Y = 18;
              }
            }
            Game1.tile[i, j].frameX = (short) rectangle.X;
            Game1.tile[i, j].frameY = (short) rectangle.Y;
            if (type == 52 || type == 62)
            {
              int num9 = Game1.tile[i, j - 1] == null ? type : (Game1.tile[i, j - 1].active ? (int) Game1.tile[i, j - 1].type : -1);
              if (num9 != type && num9 != 2 && num9 != 60)
                WorldGen.KillTile(i, j);
            }
            if (type == 53)
            {
              if (Game1.netMode == 0)
              {
                if (Game1.tile[i, j + 1] != null && !Game1.tile[i, j + 1].active)
                {
                  Game1.tile[i, j].active = false;
                  Projectile.NewProjectile((float) (i * 16 + 8), (float) (j * 16 + 8), 0.0f, 0.41f, 31, 10, 0.0f, Game1.myPlayer);
                  WorldGen.SquareTileFrame(i, j);
                }
              }
              else if (Game1.netMode == 2 && Game1.tile[i, j + 1] != null && !Game1.tile[i, j + 1].active)
              {
                Game1.tile[i, j].active = false;
                int index12 = Projectile.NewProjectile((float) (i * 16 + 8), (float) (j * 16 + 8), 0.0f, 0.41f, 31, 10, 0.0f, Game1.myPlayer);
                Game1.projectile[index12].velocity.Y = 0.5f;
                Game1.projectile[index12].position.Y += 2f;
                NetMessage.SendTileSquare(-1, i, j, 1);
                WorldGen.SquareTileFrame(i, j);
              }
            }
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
  }
}
