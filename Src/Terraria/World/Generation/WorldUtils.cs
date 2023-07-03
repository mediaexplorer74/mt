﻿/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using Microsoft.Xna.Framework;
using GameManager;

namespace GameManager.World.Generation
{
    internal static class WorldUtils
    {
        public static bool Gen(Point origin, GenShape shape, GenAction action)
        {
            return shape.Perform(origin, action);
        }

        public static bool Find(Point origin, GenSearch search, out Point result)
        {
            result = search.Find(origin);
            return !(result == GenSearch.NOT_FOUND);
        }

        public static void ClearTile(int x, int y, bool frameNeighbors = false)
        {
            Main.tile[x, y].ClearTile();
            if (!frameNeighbors)
                return;
            WorldGen.TileFrame(x + 1, y, false, false);
            WorldGen.TileFrame(x - 1, y, false, false);
            WorldGen.TileFrame(x, y + 1, false, false);
            WorldGen.TileFrame(x, y - 1, false, false);
        }

        public static void ClearWall(int x, int y, bool frameNeighbors = false)
        {
            Main.tile[x, y].wall = (byte)0;
            if (!frameNeighbors)
                return;
            WorldGen.SquareWallFrame(x + 1, y, true);
            WorldGen.SquareWallFrame(x - 1, y, true);
            WorldGen.SquareWallFrame(x, y + 1, true);
            WorldGen.SquareWallFrame(x, y - 1, true);
        }

        public static void TileFrame(int x, int y, bool frameNeighbors = false)
        {
            WorldGen.TileFrame(x, y, true, false);
            if (!frameNeighbors)
                return;
            WorldGen.TileFrame(x + 1, y, true, false);
            WorldGen.TileFrame(x - 1, y, true, false);
            WorldGen.TileFrame(x, y + 1, true, false);
            WorldGen.TileFrame(x, y - 1, true, false);
        }

        public static void ClearChestLocation(int x, int y)
        {
            WorldUtils.ClearTile(x, y, true);
            WorldUtils.ClearTile(x - 1, y, true);
            WorldUtils.ClearTile(x, y - 1, true);
            WorldUtils.ClearTile(x - 1, y - 1, true);
        }

        public static void WireLine(Point start, Point end)
        {
            Point point1 = start;
            Point point2 = end;
            if (end.X < start.X)
                Utils.Swap<int>(ref end.X, ref start.X);
            if (end.Y < start.Y)
                Utils.Swap<int>(ref end.Y, ref start.Y);
            for (int i = start.X; i <= end.X; ++i)
                WorldGen.PlaceWire(i, point1.Y, k_WireFlags.WIRE_RED);
            for (int j = start.Y; j <= end.Y; ++j)
                WorldGen.PlaceWire(point2.X, j, k_WireFlags.WIRE_RED);
        }

        public static void DebugRegen()
        {
            WorldGen.clearWorld();
            WorldGen.generateWorld(-1, (GenerationProgress)null);
            Main.NewText("World Regen Complete.", byte.MaxValue, byte.MaxValue, byte.MaxValue, false);
        }

        public static void DebugRotate()
        {
            int num1 = 0;
            int num2 = 0;
            int num3 = Main.maxTilesY;
            for (int index1 = 0; index1 < Main.maxTilesX / Main.maxTilesY; ++index1)
            {
                for (int index2 = 0; index2 < num3 / 2; ++index2)
                {
                    for (int index3 = index2; index3 < num3 - index2; ++index3)
                    {
                        Tile tile = Main.tile[index3 + num1, index2 + num2];
                        Main.tile[index3 + num1, index2 + num2] = Main.tile[index2 + num1, num3 - index3 + num2];
                        Main.tile[index2 + num1, num3 - index3 + num2] = Main.tile[num3 - index3 + num1, num3 - index2 + num2];
                        Main.tile[num3 - index3 + num1, num3 - index2 + num2] = Main.tile[num3 - index2 + num1, index3 + num2];
                        Main.tile[num3 - index2 + num1, index3 + num2] = tile;
                    }
                }
                num1 += num3;
            }
        }
    }
}
