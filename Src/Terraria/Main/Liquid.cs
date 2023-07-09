﻿/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using System;
using System.Collections.Generic;
using GameManager.GameContent.NetModules;
using GameManager.Net;
using GameManager;
using System.Diagnostics;

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
        public static bool stuck = false;
        public static bool quickFall = false;
        public static bool quickSettle = false;
        public static int panicCounter = 0;
        public static bool panicMode = false;
        public static int panicY = 0;
        private static HashSet<int> _netChangeSet = new HashSet<int>();
        private static HashSet<int> _swapNetChangeSet = new HashSet<int>();
        public static int numLiquid;
        private static int wetCounter;
        public int x;
        public int y;
        public int kill;
        public int delay;

        public static void NetSendLiquid(int x, int y)
        {
            lock (Liquid._netChangeSet)
                Liquid._netChangeSet.Add((x & (int)ushort.MaxValue) << 16 | y & (int)ushort.MaxValue);
        }

        public static double QuickWater(int verbose = 0, int minY = -1, int maxY = -1)
        {
            Game1.tileSolid[379] = true;
            int num1 = 0;
            if (minY == -1)
                minY = 3;
            if (maxY == -1)
                maxY = Game1.maxTilesY - 3;
            for (int index1 = maxY; index1 >= minY; --index1)
            {
                if (verbose > 0)
                {
                    float num2 = (float)(maxY - index1) / (float)(maxY - minY + 1) / (float)verbose;
                    Game1.statusText = string.Concat(new object[4]
          {
            (object) Lang.gen[27],
            (object) " ",
            (object) (int) ((double) num2 * 100.0 + 1.0),
            (object) "%"
          });
                }
                else if (verbose < 0)
                {
                    float num2 = (float)(maxY - index1) / (float)(maxY - minY + 1) / (float)-verbose;
                    Game1.statusText = string.Concat(new object[4]
          {
            (object) Lang.gen[18],
            (object) " ",
            (object) (int) ((double) num2 * 100.0 + 1.0),
            (object) "%"
          });
                }
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
                    int index3 = num2;
                    while (index3 != num3)
                    {
                        Tile tile = Game1.tile[index3, index1];
                        if ((int)tile.liquid > 0)
                        {
                            int num5 = -num4;
                            bool flag1 = false;
                            int x = index3;
                            int y = index1;
                            byte num6 = tile.liquidType();
                            bool flag2 = tile.lava();
                            bool flag3 = tile.honey();
                            byte num7 = tile.liquid;
                            tile.liquid = (byte)0;
                            bool flag4 = true;
                            int num8 = 0;
                            while (flag4 && x > 3 && (x < Game1.maxTilesX - 3 && y < Game1.maxTilesY - 3))
                            {
                                flag4 = false;
                                while ((int)Game1.tile[x, y + 1].liquid == 0 && y < Game1.maxTilesY - 5 && (!Game1.tile[x, y + 1].nactive() || !Game1.tileSolid[(int)Game1.tile[x, y + 1].type] || Game1.tileSolidTop[(int)Game1.tile[x, y + 1].type]))
                                {
                                    flag1 = true;
                                    num5 = num4;
                                    num8 = 0;
                                    flag4 = true;
                                    ++y;
                                    if (y > WorldGen.waterLine && WorldGen.gen && !flag3)
                                        num6 = (byte)1;
                                }
                                if ((int)Game1.tile[x, y + 1].liquid > 0 && (int)Game1.tile[x, y + 1].liquid < (int)byte.MaxValue && (int)Game1.tile[x, y + 1].liquidType() == (int)num6)
                                {
                                    int num9 = (int)byte.MaxValue - (int)Game1.tile[x, y + 1].liquid;
                                    if (num9 > (int)num7)
                                        num9 = (int)num7;
                                    Game1.tile[x, y + 1].liquid += (byte)num9;
                                    num7 -= (byte)num9;
                                    if ((int)num7 <= 0)
                                    {
                                        ++num1;
                                        break;
                                    }
                                }
                                if (num8 == 0)
                                {
                                    if ((int)Game1.tile[x + num5, y].liquid == 0 && (!Game1.tile[x + num5, y].nactive() || !Game1.tileSolid[(int)Game1.tile[x + num5, y].type] || Game1.tileSolidTop[(int)Game1.tile[x + num5, y].type]))
                                        num8 = num5;
                                    else if ((int)Game1.tile[x - num5, y].liquid == 0 && (!Game1.tile[x - num5, y].nactive() || !Game1.tileSolid[(int)Game1.tile[x - num5, y].type] || Game1.tileSolidTop[(int)Game1.tile[x - num5, y].type]))
                                        num8 = -num5;
                                }
                                if (num8 != 0 && (int)Game1.tile[x + num8, y].liquid == 0 && (!Game1.tile[x + num8, y].nactive() || !Game1.tileSolid[(int)Game1.tile[x + num8, y].type] || Game1.tileSolidTop[(int)Game1.tile[x + num8, y].type]))
                                {
                                    flag4 = true;
                                    x += num8;
                                }
                                if (flag1 && !flag4)
                                {
                                    flag1 = false;
                                    flag4 = true;
                                    num5 = -num4;
                                    num8 = 0;
                                }
                            }
                            if (index3 != x && index1 != y)
                                ++num1;
                            Game1.tile[x, y].liquid = num7;
                            Game1.tile[x, y].liquidType((int)num6);
                            if ((int)Game1.tile[x - 1, y].liquid > 0 && Game1.tile[x - 1, y].lava() != flag2)
                            {
                                if (flag2)
                                    Liquid.LavaCheck(x, y);
                                else
                                    Liquid.LavaCheck(x - 1, y);
                            }
                            else if ((int)Game1.tile[x + 1, y].liquid > 0 && Game1.tile[x + 1, y].lava() != flag2)
                            {
                                if (flag2)
                                    Liquid.LavaCheck(x, y);
                                else
                                    Liquid.LavaCheck(x + 1, y);
                            }
                            else if ((int)Game1.tile[x, y - 1].liquid > 0 && Game1.tile[x, y - 1].lava() != flag2)
                            {
                                if (flag2)
                                    Liquid.LavaCheck(x, y);
                                else
                                    Liquid.LavaCheck(x, y - 1);
                            }
                            else if ((int)Game1.tile[x, y + 1].liquid > 0 && Game1.tile[x, y + 1].lava() != flag2)
                            {
                                if (flag2)
                                    Liquid.LavaCheck(x, y);
                                else
                                    Liquid.LavaCheck(x, y + 1);
                            }
                            if ((int)Game1.tile[x, y].liquid > 0)
                            {
                                if ((int)Game1.tile[x - 1, y].liquid > 0 && Game1.tile[x - 1, y].honey() != flag3)
                                {
                                    if (flag3)
                                        Liquid.HoneyCheck(x, y);
                                    else
                                        Liquid.HoneyCheck(x - 1, y);
                                }
                                else if ((int)Game1.tile[x + 1, y].liquid > 0 && Game1.tile[x + 1, y].honey() != flag3)
                                {
                                    if (flag3)
                                        Liquid.HoneyCheck(x, y);
                                    else
                                        Liquid.HoneyCheck(x + 1, y);
                                }
                                else if ((int)Game1.tile[x, y - 1].liquid > 0 && Game1.tile[x, y - 1].honey() != flag3)
                                {
                                    if (flag3)
                                        Liquid.HoneyCheck(x, y);
                                    else
                                        Liquid.HoneyCheck(x, y - 1);
                                }
                                else if ((int)Game1.tile[x, y + 1].liquid > 0 && Game1.tile[x, y + 1].honey() != flag3)
                                {
                                    if (flag3)
                                        Liquid.HoneyCheck(x, y);
                                    else
                                        Liquid.HoneyCheck(x, y + 1);
                                }
                            }
                        }
                        index3 += num4;
                    }
                }
            }
            return (double)num1;
        }

        public void Update()
        {
            Game1.tileSolid[379] = true;
            Tile tile1 = Game1.tile[this.x - 1, this.y];
            Tile tile2 = Game1.tile[this.x + 1, this.y];
            Tile tile3 = Game1.tile[this.x, this.y - 1];
            Tile tile4 = Game1.tile[this.x, this.y + 1];
            Tile tile5 = Game1.tile[this.x, this.y];
            if (tile5.nactive() && Game1.tileSolid[(int)tile5.type] && !Game1.tileSolidTop[(int)tile5.type])
            {
                int num = (int)tile5.type;
                this.kill = 9;
            }
            else
            {
                byte num1 = tile5.liquid;
                if (this.y > Game1.maxTilesY - 200 && (int)tile5.liquidType() == 0 && (int)tile5.liquid > 0)
                {
                    byte num2 = (byte)2;
                    if ((int)tile5.liquid < (int)num2)
                        num2 = tile5.liquid;
                    tile5.liquid -= num2;
                }
                if ((int)tile5.liquid == 0)
                {
                    this.kill = 9;
                }
                else
                {
                    if (tile5.lava())
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
                        if (tile1.lava())
                            Liquid.AddWater(this.x - 1, this.y);
                        if (tile2.lava())
                            Liquid.AddWater(this.x + 1, this.y);
                        if (tile3.lava())
                            Liquid.AddWater(this.x, this.y - 1);
                        if (tile4.lava())
                            Liquid.AddWater(this.x, this.y + 1);
                        if (tile5.honey())
                        {
                            Liquid.HoneyCheck(this.x, this.y);
                            if (!Liquid.quickFall)
                            {
                                if (this.delay < 10)
                                {
                                    ++this.delay;
                                    return;
                                }
                                this.delay = 0;
                            }
                        }
                        else
                        {
                            if (tile1.honey())
                                Liquid.AddWater(this.x - 1, this.y);
                            if (tile2.honey())
                                Liquid.AddWater(this.x + 1, this.y);
                            if (tile3.honey())
                                Liquid.AddWater(this.x, this.y - 1);
                            if (tile4.honey())
                                Liquid.AddWater(this.x, this.y + 1);
                        }
                    }
                    if ((!tile4.nactive() || !Game1.tileSolid[(int)tile4.type] || Game1.tileSolidTop[(int)tile4.type]) && (((int)tile4.liquid <= 0 || (int)tile4.liquidType() == (int)tile5.liquidType()) && (int)tile4.liquid < (int)byte.MaxValue))
                    {
                        float num2 = (float)((int)byte.MaxValue - (int)tile4.liquid);
                        if ((double)num2 > (double)tile5.liquid)
                            num2 = (float)tile5.liquid;
                        tile5.liquid -= (byte)num2;
                        tile4.liquid += (byte)num2;
                        tile4.liquidType((int)tile5.liquidType());
                        Liquid.AddWater(this.x, this.y + 1);
                        tile4.skipLiquid(true);
                        tile5.skipLiquid(true);
                        if ((int)tile5.liquid > 250)
                        {
                            tile5.liquid = byte.MaxValue;
                        }
                        else
                        {
                            Liquid.AddWater(this.x - 1, this.y);
                            Liquid.AddWater(this.x + 1, this.y);
                        }
                    }
                    if ((int)tile5.liquid > 0)
                    {
                        bool flag1 = true;
                        bool flag2 = true;
                        bool flag3 = true;
                        bool flag4 = true;
                        if (tile1.nactive() && Game1.tileSolid[(int)tile1.type] && !Game1.tileSolidTop[(int)tile1.type])
                            flag1 = false;
                        else if ((int)tile1.liquid > 0 && (int)tile1.liquidType() != (int)tile5.liquidType())
                            flag1 = false;
                        else if (Game1.tile[this.x - 2, this.y].nactive() && Game1.tileSolid[(int)Game1.tile[this.x - 2, this.y].type] && !Game1.tileSolidTop[(int)Game1.tile[this.x - 2, this.y].type])
                            flag3 = false;
                        else if ((int)Game1.tile[this.x - 2, this.y].liquid == 0)
                            flag3 = false;
                        else if ((int)Game1.tile[this.x - 2, this.y].liquid > 0 && (int)Game1.tile[this.x - 2, this.y].liquidType() != (int)tile5.liquidType())
                            flag3 = false;
                        if (tile2.nactive() && Game1.tileSolid[(int)tile2.type] && !Game1.tileSolidTop[(int)tile2.type])
                            flag2 = false;
                        else if ((int)tile2.liquid > 0 && (int)tile2.liquidType() != (int)tile5.liquidType())
                            flag2 = false;
                        else if (Game1.tile[this.x + 2, this.y].nactive() && Game1.tileSolid[(int)Game1.tile[this.x + 2, this.y].type] && !Game1.tileSolidTop[(int)Game1.tile[this.x + 2, this.y].type])
                            flag4 = false;
                        else if ((int)Game1.tile[this.x + 2, this.y].liquid == 0)
                            flag4 = false;
                        else if ((int)Game1.tile[this.x + 2, this.y].liquid > 0 && (int)Game1.tile[this.x + 2, this.y].liquidType() != (int)tile5.liquidType())
                            flag4 = false;
                        int num2 = 0;
                        if ((int)tile5.liquid < 3)
                            num2 = -1;
                        if (flag1 && flag2)
                        {
                            if (flag3 && flag4)
                            {
                                bool flag5 = true;
                                bool flag6 = true;
                                if (Game1.tile[this.x - 3, this.y].nactive() && Game1.tileSolid[(int)Game1.tile[this.x - 3, this.y].type] && !Game1.tileSolidTop[(int)Game1.tile[this.x - 3, this.y].type])
                                    flag5 = false;
                                else if ((int)Game1.tile[this.x - 3, this.y].liquid == 0)
                                    flag5 = false;
                                else if ((int)Game1.tile[this.x - 3, this.y].liquidType() != (int)tile5.liquidType())
                                    flag5 = false;
                                if (Game1.tile[this.x + 3, this.y].nactive() && Game1.tileSolid[(int)Game1.tile[this.x + 3, this.y].type] && !Game1.tileSolidTop[(int)Game1.tile[this.x + 3, this.y].type])
                                    flag6 = false;
                                else if ((int)Game1.tile[this.x + 3, this.y].liquid == 0)
                                    flag6 = false;
                                else if ((int)Game1.tile[this.x + 3, this.y].liquidType() != (int)tile5.liquidType())
                                    flag6 = false;
                                if (flag5 && flag6)
                                {
                                    float num3 = (float)Math.Round((double)((int)tile1.liquid + (int)tile2.liquid + (int)Game1.tile[this.x - 2, this.y].liquid + (int)Game1.tile[this.x + 2, this.y].liquid + (int)Game1.tile[this.x - 3, this.y].liquid + (int)Game1.tile[this.x + 3, this.y].liquid + (int)tile5.liquid + num2) / 7.0);
                                    int num4 = 0;
                                    tile1.liquidType((int)tile5.liquidType());
                                    if ((int)tile1.liquid != (int)(byte)num3)
                                    {
                                        tile1.liquid = (byte)num3;
                                        Liquid.AddWater(this.x - 1, this.y);
                                    }
                                    else
                                        ++num4;
                                    tile2.liquidType((int)tile5.liquidType());
                                    if ((int)tile2.liquid != (int)(byte)num3)
                                    {
                                        tile2.liquid = (byte)num3;
                                        Liquid.AddWater(this.x + 1, this.y);
                                    }
                                    else
                                        ++num4;
                                    Game1.tile[this.x - 2, this.y].liquidType((int)tile5.liquidType());
                                    if ((int)Game1.tile[this.x - 2, this.y].liquid != (int)(byte)num3)
                                    {
                                        Game1.tile[this.x - 2, this.y].liquid = (byte)num3;
                                        Liquid.AddWater(this.x - 2, this.y);
                                    }
                                    else
                                        ++num4;
                                    Game1.tile[this.x + 2, this.y].liquidType((int)tile5.liquidType());
                                    if ((int)Game1.tile[this.x + 2, this.y].liquid != (int)(byte)num3)
                                    {
                                        Game1.tile[this.x + 2, this.y].liquid = (byte)num3;
                                        Liquid.AddWater(this.x + 2, this.y);
                                    }
                                    else
                                        ++num4;
                                    Game1.tile[this.x - 3, this.y].liquidType((int)tile5.liquidType());
                                    if ((int)Game1.tile[this.x - 3, this.y].liquid != (int)(byte)num3)
                                    {
                                        Game1.tile[this.x - 3, this.y].liquid = (byte)num3;
                                        Liquid.AddWater(this.x - 3, this.y);
                                    }
                                    else
                                        ++num4;
                                    Game1.tile[this.x + 3, this.y].liquidType((int)tile5.liquidType());
                                    if ((int)Game1.tile[this.x + 3, this.y].liquid != (int)(byte)num3)
                                    {
                                        Game1.tile[this.x + 3, this.y].liquid = (byte)num3;
                                        Liquid.AddWater(this.x + 3, this.y);
                                    }
                                    else
                                        ++num4;
                                    if ((int)tile1.liquid != (int)(byte)num3 || (int)tile5.liquid != (int)(byte)num3)
                                        Liquid.AddWater(this.x - 1, this.y);
                                    if ((int)tile2.liquid != (int)(byte)num3 || (int)tile5.liquid != (int)(byte)num3)
                                        Liquid.AddWater(this.x + 1, this.y);
                                    if ((int)Game1.tile[this.x - 2, this.y].liquid != (int)(byte)num3 || (int)tile5.liquid != (int)(byte)num3)
                                        Liquid.AddWater(this.x - 2, this.y);
                                    if ((int)Game1.tile[this.x + 2, this.y].liquid != (int)(byte)num3 || (int)tile5.liquid != (int)(byte)num3)
                                        Liquid.AddWater(this.x + 2, this.y);
                                    if ((int)Game1.tile[this.x - 3, this.y].liquid != (int)(byte)num3 || (int)tile5.liquid != (int)(byte)num3)
                                        Liquid.AddWater(this.x - 3, this.y);
                                    if ((int)Game1.tile[this.x + 3, this.y].liquid != (int)(byte)num3 || (int)tile5.liquid != (int)(byte)num3)
                                        Liquid.AddWater(this.x + 3, this.y);
                                    if (num4 != 6 || (int)tile3.liquid <= 0)
                                        tile5.liquid = (byte)num3;
                                }
                                else
                                {
                                    int num3 = 0;
                                    float num4 = (float)Math.Round((double)((int)tile1.liquid + (int)tile2.liquid + (int)Game1.tile[this.x - 2, this.y].liquid + (int)Game1.tile[this.x + 2, this.y].liquid + (int)tile5.liquid + num2) / 5.0);
                                    tile1.liquidType((int)tile5.liquidType());
                                    if ((int)tile1.liquid != (int)(byte)num4)
                                    {
                                        tile1.liquid = (byte)num4;
                                        Liquid.AddWater(this.x - 1, this.y);
                                    }
                                    else
                                        ++num3;
                                    tile2.liquidType((int)tile5.liquidType());
                                    if ((int)tile2.liquid != (int)(byte)num4)
                                    {
                                        tile2.liquid = (byte)num4;
                                        Liquid.AddWater(this.x + 1, this.y);
                                    }
                                    else
                                        ++num3;
                                    Game1.tile[this.x - 2, this.y].liquidType((int)tile5.liquidType());
                                    if ((int)Game1.tile[this.x - 2, this.y].liquid != (int)(byte)num4)
                                    {
                                        Game1.tile[this.x - 2, this.y].liquid = (byte)num4;
                                        Liquid.AddWater(this.x - 2, this.y);
                                    }
                                    else
                                        ++num3;
                                    Game1.tile[this.x + 2, this.y].liquidType((int)tile5.liquidType());
                                    if ((int)Game1.tile[this.x + 2, this.y].liquid != (int)(byte)num4)
                                    {
                                        Game1.tile[this.x + 2, this.y].liquid = (byte)num4;
                                        Liquid.AddWater(this.x + 2, this.y);
                                    }
                                    else
                                        ++num3;
                                    if ((int)tile1.liquid != (int)(byte)num4 || (int)tile5.liquid != (int)(byte)num4)
                                        Liquid.AddWater(this.x - 1, this.y);
                                    if ((int)tile2.liquid != (int)(byte)num4 || (int)tile5.liquid != (int)(byte)num4)
                                        Liquid.AddWater(this.x + 1, this.y);
                                    if ((int)Game1.tile[this.x - 2, this.y].liquid != (int)(byte)num4 || (int)tile5.liquid != (int)(byte)num4)
                                        Liquid.AddWater(this.x - 2, this.y);
                                    if ((int)Game1.tile[this.x + 2, this.y].liquid != (int)(byte)num4 || (int)tile5.liquid != (int)(byte)num4)
                                        Liquid.AddWater(this.x + 2, this.y);
                                    if (num3 != 4 || (int)tile3.liquid <= 0)
                                        tile5.liquid = (byte)num4;
                                }
                            }
                            else if (flag3)
                            {
                                float num3 = (float)Math.Round((double)((int)tile1.liquid + (int)tile2.liquid + (int)Game1.tile[this.x - 2, this.y].liquid + (int)tile5.liquid + num2) / 4.0 + 0.001);
                                tile1.liquidType((int)tile5.liquidType());
                                if ((int)tile1.liquid != (int)(byte)num3 || (int)tile5.liquid != (int)(byte)num3)
                                {
                                    tile1.liquid = (byte)num3;
                                    Liquid.AddWater(this.x - 1, this.y);
                                }
                                tile2.liquidType((int)tile5.liquidType());
                                if ((int)tile2.liquid != (int)(byte)num3 || (int)tile5.liquid != (int)(byte)num3)
                                {
                                    tile2.liquid = (byte)num3;
                                    Liquid.AddWater(this.x + 1, this.y);
                                }
                                Game1.tile[this.x - 2, this.y].liquidType((int)tile5.liquidType());
                                if ((int)Game1.tile[this.x - 2, this.y].liquid != (int)(byte)num3 || (int)tile5.liquid != (int)(byte)num3)
                                {
                                    Game1.tile[this.x - 2, this.y].liquid = (byte)num3;
                                    Liquid.AddWater(this.x - 2, this.y);
                                }
                                tile5.liquid = (byte)num3;
                            }
                            else if (flag4)
                            {
                                float num3 = (float)Math.Round((double)((int)tile1.liquid + (int)tile2.liquid + (int)Game1.tile[this.x + 2, this.y].liquid + (int)tile5.liquid + num2) / 4.0 + 0.001);
                                tile1.liquidType((int)tile5.liquidType());
                                if ((int)tile1.liquid != (int)(byte)num3 || (int)tile5.liquid != (int)(byte)num3)
                                {
                                    tile1.liquid = (byte)num3;
                                    Liquid.AddWater(this.x - 1, this.y);
                                }
                                tile2.liquidType((int)tile5.liquidType());
                                if ((int)tile2.liquid != (int)(byte)num3 || (int)tile5.liquid != (int)(byte)num3)
                                {
                                    tile2.liquid = (byte)num3;
                                    Liquid.AddWater(this.x + 1, this.y);
                                }
                                Game1.tile[this.x + 2, this.y].liquidType((int)tile5.liquidType());
                                if ((int)Game1.tile[this.x + 2, this.y].liquid != (int)(byte)num3 || (int)tile5.liquid != (int)(byte)num3)
                                {
                                    Game1.tile[this.x + 2, this.y].liquid = (byte)num3;
                                    Liquid.AddWater(this.x + 2, this.y);
                                }
                                tile5.liquid = (byte)num3;
                            }
                            else
                            {
                                float num3 = (float)Math.Round((double)((int)tile1.liquid + (int)tile2.liquid + (int)tile5.liquid + num2) / 3.0 + 0.001);
                                tile1.liquidType((int)tile5.liquidType());
                                if ((int)tile1.liquid != (int)(byte)num3)
                                    tile1.liquid = (byte)num3;
                                if ((int)tile5.liquid != (int)(byte)num3 || (int)tile1.liquid != (int)(byte)num3)
                                    Liquid.AddWater(this.x - 1, this.y);
                                tile2.liquidType((int)tile5.liquidType());
                                if ((int)tile2.liquid != (int)(byte)num3)
                                    tile2.liquid = (byte)num3;
                                if ((int)tile5.liquid != (int)(byte)num3 || (int)tile2.liquid != (int)(byte)num3)
                                    Liquid.AddWater(this.x + 1, this.y);
                                tile5.liquid = (byte)num3;
                            }
                        }
                        else if (flag1)
                        {
                            float num3 = (float)Math.Round((double)((int)tile1.liquid + (int)tile5.liquid + num2) / 2.0 + 0.001);
                            if ((int)tile1.liquid != (int)(byte)num3)
                                tile1.liquid = (byte)num3;
                            tile1.liquidType((int)tile5.liquidType());
                            if ((int)tile5.liquid != (int)(byte)num3 || (int)tile1.liquid != (int)(byte)num3)
                                Liquid.AddWater(this.x - 1, this.y);
                            tile5.liquid = (byte)num3;
                        }
                        else if (flag2)
                        {
                            float num3 = (float)Math.Round((double)((int)tile2.liquid + (int)tile5.liquid + num2) / 2.0 + 0.001);
                            if ((int)tile2.liquid != (int)(byte)num3)
                                tile2.liquid = (byte)num3;
                            tile2.liquidType((int)tile5.liquidType());
                            if ((int)tile5.liquid != (int)(byte)num3 || (int)tile2.liquid != (int)(byte)num3)
                                Liquid.AddWater(this.x + 1, this.y);
                            tile5.liquid = (byte)num3;
                        }
                    }
                    if ((int)tile5.liquid != (int)num1)
                    {
                        if ((int)tile5.liquid == 254 && (int)num1 == (int)byte.MaxValue)
                        {
                            tile5.liquid = byte.MaxValue;
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

        public static void StartPanic()
        {
            if (Liquid.panicMode)
                return;
            WorldGen.waterLine = Game1.maxTilesY;
            Liquid.numLiquid = 0;
            LiquidBuffer.numLiquidBuffer = 0;
            Liquid.panicCounter = 0;
            Liquid.panicMode = true;
            Liquid.panicY = Game1.maxTilesY - 3;

            if (!Game1.dedServ)
                return;

            Debug.WriteLine("Forcing water to settle.");
        }

        public static void UpdateLiquid()
        {
            int num1 = Game1.netMode;
            if (!WorldGen.gen)
            {
                if (!Liquid.panicMode)
                {
                    if (Liquid.numLiquid + LiquidBuffer.numLiquidBuffer > 4000)
                    {
                        ++Liquid.panicCounter;
                        if (Liquid.panicCounter > 1800 || Liquid.numLiquid + LiquidBuffer.numLiquidBuffer > 13500)
                            Liquid.StartPanic();
                    }
                    else
                        Liquid.panicCounter = 0;
                }
                if (Liquid.panicMode)
                {
                    int num2 = 0;
                    while (Liquid.panicY >= 3 && num2 < 5)
                    {
                        ++num2;
                        Liquid.QuickWater(0, Liquid.panicY, Liquid.panicY);
                        --Liquid.panicY;
                        if (Liquid.panicY < 3)
                        {
                            Debug.WriteLine("Water has been settled.");

                            Liquid.panicCounter = 0;
                            Liquid.panicMode = false;
                            WorldGen.WaterCheck();
                            if (Game1.netMode == 2)
                            {
                                for (int index1 = 0; index1 < (int)byte.MaxValue; ++index1)
                                {
                                    for (int index2 = 0; index2 < Game1.maxSectionsX; ++index2)
                                    {
                                        for (int index3 = 0; index3 < Game1.maxSectionsY; ++index3)
                                            Netplay.Clients[index1].TileSections[index2, index3] = false;
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
            int num3 = Liquid.maxLiquid / Liquid.cycles;
            int num4 = num3 * (Liquid.wetCounter - 1);
            int num5 = num3 * Liquid.wetCounter;
            if (Liquid.wetCounter == Liquid.cycles)
                num5 = Liquid.numLiquid;
            if (num5 > Liquid.numLiquid)
            {
                num5 = Liquid.numLiquid;
                int num2 = Game1.netMode;
                Liquid.wetCounter = Liquid.cycles;
            }
            if (Liquid.quickFall)
            {
                for (int index = num4; index < num5; ++index)
                {
                    Game1.liquid[index].delay = 10;
                    Game1.liquid[index].Update();
                    Game1.tile[Game1.liquid[index].x, Game1.liquid[index].y].skipLiquid(false);
                }
            }
            else
            {
                for (int index = num4; index < num5; ++index)
                {
                    if (!Game1.tile[Game1.liquid[index].x, Game1.liquid[index].y].skipLiquid())
                        Game1.liquid[index].Update();
                    else
                        Game1.tile[Game1.liquid[index].x, Game1.liquid[index].y].skipLiquid(false);
                }
            }
            if (Liquid.wetCounter >= Liquid.cycles)
            {
                Liquid.wetCounter = 0;
                for (int l = Liquid.numLiquid - 1; l >= 0; --l)
                {
                    if (Game1.liquid[l].kill > 4)
                        Liquid.DelWater(l);
                }
                int num2 = Liquid.maxLiquid - (Liquid.maxLiquid - Liquid.numLiquid);
                if (num2 > LiquidBuffer.numLiquidBuffer)
                    num2 = LiquidBuffer.numLiquidBuffer;
                for (int index = 0; index < num2; ++index)
                {
                    Game1.tile[Game1.liquidBuffer[0].x, Game1.liquidBuffer[0].y].checkingLiquid(false);
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
            if (Game1.netMode != 2 || Liquid._netChangeSet.Count <= 0)
                return;
            Utils.Swap<HashSet<int>>(ref Liquid._netChangeSet, ref Liquid._swapNetChangeSet);
            NetManager.Instance.Broadcast(NetLiquidModule.Serialize(Liquid._swapNetChangeSet), -1);
            Liquid._swapNetChangeSet.Clear();
        }

        public static void AddWater(int x, int y)
        {
            Tile checkTile = Game1.tile[x, y];
            if (Game1.tile[x, y] == null || checkTile.checkingLiquid() || (x >= Game1.maxTilesX - 5 || y >= Game1.maxTilesY - 5) || (x < 5 || y < 5 || (int)checkTile.liquid == 0))
                return;
            if (Liquid.numLiquid >= Liquid.maxLiquid - 1)
            {
                LiquidBuffer.AddBuffer(x, y);
            }
            else
            {
                checkTile.checkingLiquid(true);
                Game1.liquid[Liquid.numLiquid].kill = 0;
                Game1.liquid[Liquid.numLiquid].x = x;
                Game1.liquid[Liquid.numLiquid].y = y;
                Game1.liquid[Liquid.numLiquid].delay = 0;
                checkTile.skipLiquid(false);
                ++Liquid.numLiquid;
                if (Game1.netMode == 2)
                    Liquid.NetSendLiquid(x, y);
                if (!checkTile.active() || WorldGen.gen)
                    return;
                bool flag = false;
                if (checkTile.lava())
                {
                    if (TileObjectData.CheckLavaDeath(checkTile))
                        flag = true;
                }
                else if (TileObjectData.CheckWaterDeath(checkTile))
                    flag = true;
                if (!flag)
                    return;
                WorldGen.KillTile(x, y, false, false, false);
                if (Game1.netMode != 2)
                    return;
                NetMessage.SendData(17, -1, -1, "", 0, (float)x, (float)y, 0.0f, 0, 0, 0);
            }
        }

        public static void LavaCheck(int x, int y)
        {
            Tile tile1 = Game1.tile[x - 1, y];
            Tile tile2 = Game1.tile[x + 1, y];
            Tile tile3 = Game1.tile[x, y - 1];
            Tile tile4 = Game1.tile[x, y + 1];
            Tile tile5 = Game1.tile[x, y];
            if ((int)tile1.liquid > 0 && !tile1.lava() || (int)tile2.liquid > 0 && !tile2.lava() || (int)tile3.liquid > 0 && !tile3.lava())
            {
                int num = 0;
                int type = 56;
                if (!tile1.lava())
                {
                    num += (int)tile1.liquid;
                    tile1.liquid = (byte)0;
                }
                if (!tile2.lava())
                {
                    num += (int)tile2.liquid;
                    tile2.liquid = (byte)0;
                }
                if (!tile3.lava())
                {
                    num += (int)tile3.liquid;
                    tile3.liquid = (byte)0;
                }
                if (tile1.honey() || tile2.honey() || tile3.honey())
                    type = 230;
                if (num < 24)
                    return;
                if (tile5.active() && Game1.tileObsidianKill[(int)tile5.type])
                {
                    WorldGen.KillTile(x, y, false, false, false);
                    if (Game1.netMode == 2)
                        NetMessage.SendData(17, -1, -1, "", 0, (float)x, (float)y, 0.0f, 0, 0, 0);
                }
                if (tile5.active())
                    return;
                tile5.liquid = (byte)0;
                tile5.lava(false);
                WorldGen.PlaceTile(x, y, type, true, true, -1, 0);
                WorldGen.SquareTileFrame(x, y, true);
                if (Game1.netMode != 2)
                    return;
                NetMessage.SendTileSquare(-1, x - 1, y - 1, 3);
            }
            else
            {
                if ((int)tile4.liquid <= 0 || tile4.lava())
                    return;
                if (Game1.tileCut[(int)tile4.type])
                {
                    WorldGen.KillTile(x, y + 1, false, false, false);
                    if (Game1.netMode == 2)
                        NetMessage.SendData(17, -1, -1, "", 0, (float)x, (float)(y + 1), 0.0f, 0, 0, 0);
                }
                else if (tile4.active() && Game1.tileObsidianKill[(int)tile4.type])
                {
                    WorldGen.KillTile(x, y + 1, false, false, false);
                    if (Game1.netMode == 2)
                        NetMessage.SendData(17, -1, -1, "", 0, (float)x, (float)(y + 1), 0.0f, 0, 0, 0);
                }
                if (tile4.active())
                    return;
                if ((int)tile5.liquid < 24)
                {
                    tile5.liquid = (byte)0;
                    tile5.liquidType(0);
                    if (Game1.netMode != 2)
                        return;
                    NetMessage.SendTileSquare(-1, x - 1, y, 3);
                }
                else
                {
                    int type = 56;
                    if (tile4.honey())
                        type = 230;
                    tile5.liquid = (byte)0;
                    tile5.lava(false);
                    tile4.liquid = (byte)0;
                    WorldGen.PlaceTile(x, y + 1, type, true, true, -1, 0);
                    WorldGen.SquareTileFrame(x, y + 1, true);
                    if (Game1.netMode != 2)
                        return;
                    NetMessage.SendTileSquare(-1, x - 1, y, 3);
                }
            }
        }

        public static void HoneyCheck(int x, int y)
        {
            Tile tile1 = Game1.tile[x - 1, y];
            Tile tile2 = Game1.tile[x + 1, y];
            Tile tile3 = Game1.tile[x, y - 1];
            Tile tile4 = Game1.tile[x, y + 1];
            Tile tile5 = Game1.tile[x, y];
            if ((int)tile1.liquid > 0 && (int)tile1.liquidType() == 0 || (int)tile2.liquid > 0 && (int)tile2.liquidType() == 0 || (int)tile3.liquid > 0 && (int)tile3.liquidType() == 0)
            {
                int num = 0;
                if ((int)tile1.liquidType() == 0)
                {
                    num += (int)tile1.liquid;
                    tile1.liquid = (byte)0;
                }
                if ((int)tile2.liquidType() == 0)
                {
                    num += (int)tile2.liquid;
                    tile2.liquid = (byte)0;
                }
                if ((int)tile3.liquidType() == 0)
                {
                    num += (int)tile3.liquid;
                    tile3.liquid = (byte)0;
                }
                if (num < 32)
                    return;
                if (tile5.active() && Game1.tileObsidianKill[(int)tile5.type])
                {
                    WorldGen.KillTile(x, y, false, false, false);
                    if (Game1.netMode == 2)
                        NetMessage.SendData(17, -1, -1, "", 0, (float)x, (float)y, 0.0f, 0, 0, 0);
                }
                if (tile5.active())
                    return;
                tile5.liquid = (byte)0;
                tile5.liquidType(0);
                WorldGen.PlaceTile(x, y, 229, true, true, -1, 0);
                WorldGen.SquareTileFrame(x, y, true);
                if (Game1.netMode != 2)
                    return;
                NetMessage.SendTileSquare(-1, x - 1, y - 1, 3);
            }
            else
            {
                if ((int)tile4.liquid <= 0 || (int)tile4.liquidType() != 0)
                    return;
                if (Game1.tileCut[(int)tile4.type])
                {
                    WorldGen.KillTile(x, y + 1, false, false, false);
                    if (Game1.netMode == 2)
                        NetMessage.SendData(17, -1, -1, "", 0, (float)x, (float)(y + 1), 0.0f, 0, 0, 0);
                }
                else if (tile4.active() && Game1.tileObsidianKill[(int)tile4.type])
                {
                    WorldGen.KillTile(x, y + 1, false, false, false);
                    if (Game1.netMode == 2)
                        NetMessage.SendData(17, -1, -1, "", 0, (float)x, (float)(y + 1), 0.0f, 0, 0, 0);
                }
                if (tile4.active())
                    return;
                if ((int)tile5.liquid < 32)
                {
                    tile5.liquid = (byte)0;
                    tile5.liquidType(0);
                    if (Game1.netMode != 2)
                        return;
                    NetMessage.SendTileSquare(-1, x - 1, y, 3);
                }
                else
                {
                    tile5.liquid = (byte)0;
                    tile5.liquidType(0);
                    tile4.liquid = (byte)0;
                    tile4.liquidType(0);
                    WorldGen.PlaceTile(x, y + 1, 229, true, true, -1, 0);
                    WorldGen.SquareTileFrame(x, y + 1, true);
                    if (Game1.netMode != 2)
                        return;
                    NetMessage.SendTileSquare(-1, x - 1, y, 3);
                }
            }
        }

        public static void DelWater(int l)
        {
            int index1 = Game1.liquid[l].x;
            int index2 = Game1.liquid[l].y;
            Tile tile1 = Game1.tile[index1 - 1, index2];
            Tile tile2 = Game1.tile[index1 + 1, index2];
            Tile tile3 = Game1.tile[index1, index2 + 1];
            Tile tile4 = Game1.tile[index1, index2];
            byte num = (byte)2;
            if ((int)tile4.liquid < (int)num)
            {
                tile4.liquid = (byte)0;
                if ((int)tile1.liquid < (int)num)
                    tile1.liquid = (byte)0;
                else
                    Liquid.AddWater(index1 - 1, index2);
                if ((int)tile2.liquid < (int)num)
                    tile2.liquid = (byte)0;
                else
                    Liquid.AddWater(index1 + 1, index2);
            }
            else if ((int)tile4.liquid < 20)
            {
                if ((int)tile1.liquid < (int)tile4.liquid && (!tile1.nactive() || !Game1.tileSolid[(int)tile1.type] || Game1.tileSolidTop[(int)tile1.type]) || (int)tile2.liquid < (int)tile4.liquid && (!tile2.nactive() || !Game1.tileSolid[(int)tile2.type] || Game1.tileSolidTop[(int)tile2.type]) || (int)tile3.liquid < (int)byte.MaxValue && (!tile3.nactive() || !Game1.tileSolid[(int)tile3.type] || Game1.tileSolidTop[(int)tile3.type]))
                    tile4.liquid = (byte)0;
            }
            else if ((int)tile3.liquid < (int)byte.MaxValue && (!tile3.nactive() || !Game1.tileSolid[(int)tile3.type] || Game1.tileSolidTop[(int)tile3.type]) && !Liquid.stuck)
            {
                Game1.liquid[l].kill = 0;
                return;
            }
            if ((int)tile4.liquid < 250 && (int)Game1.tile[index1, index2 - 1].liquid > 0)
                Liquid.AddWater(index1, index2 - 1);
            if ((int)tile4.liquid == 0)
            {
                tile4.liquidType(0);
            }
            else
            {
                if ((int)tile2.liquid > 0 && (int)Game1.tile[index1 + 1, index2 + 1].liquid < 250 && !Game1.tile[index1 + 1, index2 + 1].active() || (int)tile1.liquid > 0 && (int)Game1.tile[index1 - 1, index2 + 1].liquid < 250 && !Game1.tile[index1 - 1, index2 + 1].active())
                {
                    Liquid.AddWater(index1 - 1, index2);
                    Liquid.AddWater(index1 + 1, index2);
                }
                if (tile4.lava())
                {
                    Liquid.LavaCheck(index1, index2);
                    for (int i = index1 - 1; i <= index1 + 1; ++i)
                    {
                        for (int j = index2 - 1; j <= index2 + 1; ++j)
                        {
                            Tile tile5 = Game1.tile[i, j];
                            if (tile5.active())
                            {
                                if ((int)tile5.type == 2 || (int)tile5.type == 23 || ((int)tile5.type == 109 || (int)tile5.type == 199))
                                {
                                    tile5.type = (ushort)0;
                                    WorldGen.SquareTileFrame(i, j, true);
                                    if (Game1.netMode == 2)
                                        NetMessage.SendTileSquare(-1, index1, index2, 3);
                                }
                                else if ((int)tile5.type == 60 || (int)tile5.type == 70)
                                {
                                    tile5.type = (ushort)59;
                                    WorldGen.SquareTileFrame(i, j, true);
                                    if (Game1.netMode == 2)
                                        NetMessage.SendTileSquare(-1, index1, index2, 3);
                                }
                            }
                        }
                    }
                }
                else if (tile4.honey())
                    Liquid.HoneyCheck(index1, index2);
            }
            if (Game1.netMode == 2)
                Liquid.NetSendLiquid(index1, index2);
            --Liquid.numLiquid;
            Game1.tile[Game1.liquid[l].x, Game1.liquid[l].y].checkingLiquid(false);
            Game1.liquid[l].x = Game1.liquid[Liquid.numLiquid].x;
            Game1.liquid[l].y = Game1.liquid[Liquid.numLiquid].y;
            Game1.liquid[l].kill = Game1.liquid[Liquid.numLiquid].kill;
            if (!Game1.tileAlch[(int)tile4.type])
                return;
            WorldGen.CheckAlch(index1, index2);
        }
    }
}
