﻿// Cloud

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameManager
{
    public class Cloud
    {
        private static Random rand = new Random();
        public Vector2 position;
        public float scale;
        public float rotation;
        public float rSpeed;
        public float sSpeed;
        public bool active;
        public SpriteEffects spriteDir;
        public int type;
        public int width;
        public int height;
        public float Alpha;
        public bool kill;

        public static void resetClouds()
        {
            if (Main.dedServ || Main.cloudLimit < 10)
                return;
            Main.windSpeed = Main.windSpeedSet;
            for (int index = 0; index < 200; ++index)
                Main.cloud[index].active = false;
            for (int index = 0; index < Main.numClouds; ++index)
            {
                addCloud();
                Main.cloud[index].Alpha = 1f;
            }
            for (int index = 0; index < 200; ++index)
                Main.cloud[index].Alpha = 1f;
        }

        public static void addCloud()
        {
            if (Main.netMode == 2)
                return;
            int index1 = -1;
            for (int index2 = 0; index2 < 200; ++index2)
            {
                if (!Main.cloud[index2].active)
                {
                    index1 = index2;
                    break;
                }
            }
            if (index1 < 0)
                return;
            Main.cloud[index1].kill = false;
            Main.cloud[index1].rSpeed = 0.0f;
            Main.cloud[index1].sSpeed = 0.0f;
            Main.cloud[index1].scale = rand.Next(70, 131) * 0.01f;
            Main.cloud[index1].rotation = rand.Next(-10, 11) * 0.01f;
            Main.cloud[index1].width = (int)(Main.cloudTexture[Main.cloud[index1].type].Width * (double)Main.cloud[index1].scale);
            Main.cloud[index1].height = (int)(Main.cloudTexture[Main.cloud[index1].type].Height * (double)Main.cloud[index1].scale);
            Main.cloud[index1].Alpha = 0.0f;
            Main.cloud[index1].spriteDir = SpriteEffects.None;
            if (rand.Next(2) == 0)
                Main.cloud[index1].spriteDir = SpriteEffects.FlipHorizontally;
            float num1 = Main.windSpeed;
            if (!Main.gameMenu)
                num1 = Main.windSpeed - Main.player[Main.myPlayer].velocity.X * 0.1f;
            int num2 = 0;
            int num3 = 0;
            if (num1 > 0.0)
                num2 -= 200;
            if (num1 < 0.0)
                num3 += 200;
            int num4 = 300;
            float num5 = WorldGen.genRand.Next(num2 - num4, Main.screenWidth + num3 + num4);
            Main.cloud[index1].Alpha = 0.0f;
            Main.cloud[index1].position.Y = rand.Next((int)(-Main.screenHeight * 0.25), (int)(Main.screenHeight * 0.25));
            Main.cloud[index1].position.Y -= rand.Next((int)(Main.screenHeight * 0.150000005960464));
            Main.cloud[index1].position.Y -= rand.Next((int)(Main.screenHeight * 0.150000005960464));
            Main.cloud[index1].type = rand.Next(4);
            if (Main.rand == null)
                Main.rand = new Random();
            if (Main.cloudAlpha > 0.0 && rand.Next(4) != 0 || Main.cloudBGActive >= 1.0 && Main.rand.Next(2) == 0)
            {
                Main.cloud[index1].type = rand.Next(18, 22);
                if (Main.cloud[index1].scale >= 1.15)
                    Main.cloud[index1].position.Y -= 150f;
                if (Main.cloud[index1].scale >= 1.0)
                    Main.cloud[index1].position.Y -= 150f;
            }
            else if ((Main.cloudBGActive <= 0.0 && Main.cloudAlpha == 0.0 && (Main.cloud[index1].scale < 1.0 && Main.cloud[index1].position.Y < -Main.screenHeight * 0.200000002980232) || Main.cloud[index1].position.Y < -Main.screenHeight * 0.200000002980232) && Main.numClouds < 50.0)
                Main.cloud[index1].type = rand.Next(9, 14);
            else if ((Main.cloud[index1].scale < 1.15 && Main.cloud[index1].position.Y < -Main.screenHeight * 0.300000011920929 || Main.cloud[index1].scale < 0.85 && Main.cloud[index1].position.Y < Main.screenHeight * 0.150000005960464) && (Main.numClouds > 70.0 || Main.cloudBGActive >= 1.0))
                Main.cloud[index1].type = rand.Next(4, 9);
            else if (Main.cloud[index1].position.Y > -Main.screenHeight * 0.150000005960464 && rand.Next(2) == 0 && Main.numClouds > 20.0)
                Main.cloud[index1].type = rand.Next(14, 18);
            if (Main.cloud[index1].scale > 1.2)
                Main.cloud[index1].position.Y += 100f;
            if (Main.cloud[index1].scale > 1.3)
                Main.cloud[index1].scale = 1.3f;
            if (Main.cloud[index1].scale < 0.7)
                Main.cloud[index1].scale = 0.7f;
            Main.cloud[index1].active = true;
            Main.cloud[index1].position.X = num5;
            if (Main.cloud[index1].position.X > (Main.screenWidth + 100))
                Main.cloud[index1].Alpha = 1f;
            if (Main.cloud[index1].position.X + Main.cloudTexture[Main.cloud[index1].type].Width * (double)Main.cloud[index1].scale < -100.0)
                Main.cloud[index1].Alpha = 1f;
            Rectangle rectangle1 = new Rectangle((int)Main.cloud[index1].position.X, (int)Main.cloud[index1].position.Y, Main.cloud[index1].width, Main.cloud[index1].height);
            for (int index2 = 0; index2 < 200; ++index2)
            {
                if (index1 != index2 && Main.cloud[index2].active)
                {
                    Rectangle rectangle2 = new Rectangle((int)Main.cloud[index2].position.X, (int)Main.cloud[index2].position.Y, Main.cloud[index2].width, Main.cloud[index2].height);
                    if (rectangle1.Intersects(rectangle2))
                        Main.cloud[index1].active = false;
                }
            }
        }

        public Color cloudColor(Color bgColor)
        {
            float num = scale * Alpha;
            if (num > 1.0)
                num = 1f;
            return new Color((byte)(float)(int)(bgColor.R * (double)num), (byte)(float)(int)(bgColor.G * (double)num), (byte)(float)(int)(bgColor.B * (double)num), (byte)(float)(int)(bgColor.A * (double)num));
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public static void UpdateClouds()
        {
            if (Main.netMode == 2)
                return;
            int maxValue = 0;
            for (int index = 0; index < 200; ++index)
            {
                if (Main.cloud[index].active)
                {
                    Main.cloud[index].Update();
                    if (!Main.cloud[index].kill)
                        ++maxValue;
                }
            }
            for (int index = 0; index < 200; ++index)
            {
                if (Main.cloud[index].active)
                {
                    if (index > 1 && (!Main.cloud[index - 1].active || Main.cloud[index - 1].scale > Main.cloud[index].scale + 0.02))
                    {
                        Cloud cloud = (Cloud)Main.cloud[index - 1].Clone();
                        Main.cloud[index - 1] = (Cloud)Main.cloud[index].Clone();
                        Main.cloud[index] = cloud;
                    }
                    if (index < 199 && (!Main.cloud[index].active || Main.cloud[index + 1].scale < Main.cloud[index].scale - 0.02))
                    {
                        Cloud cloud = (Cloud)Main.cloud[index + 1].Clone();
                        Main.cloud[index + 1] = (Cloud)Main.cloud[index].Clone();
                        Main.cloud[index] = cloud;
                    }
                }
            }
            if (maxValue < Main.numClouds)
            {
                addCloud();
            }
            else
            {
                if (maxValue <= Main.numClouds)
                    return;
                int index1 = Main.rand.Next(maxValue);
                for (int index2 = 0; Main.cloud[index1].kill && index2 < 100; index1 = Main.rand.Next(maxValue))
                    ++index2;
                Main.cloud[index1].kill = true;
            }
        }

        public void Update()
        {
            if (Main.gameMenu)
            {
                position.X += (float)(Main.windSpeed * scale * 3.0);
            }
            else
            {
                if (scale == 1.0)
                    scale -= 0.0001f;
                if (scale == 1.15)
                    scale -= 0.0001f;
                float num1;
                if (scale < 1.0)
                {
                    float num2 = 0.07f;
                    float num3 = (float)(((scale + 0.15f) + 1.0) / 2.0);
                    float num4 = num3 * num3;
                    num1 = num2 * num4;
                }
                else if (scale <= 1.15)
                {
                    float num2 = 0.19f;
                    float num3 = scale - 0.075f;
                    float num4 = num3 * num3;
                    num1 = num2 * num4;
                }
                else
                {
                    float num2 = 0.23f;
                    float num3 = (float)(scale - 0.150000005960464 - 0.0750000029802322);
                    float num4 = num3 * num3;
                    num1 = num2 * num4;
                }
                position.X += (float)(Main.windSpeed * (double)num1 * 5.0) * Main.dayRate;
                position.X -= (Main.screenPosition.X - Main.screenLastPosition.X) * num1;
            }
            float num = 600f;
            if (!kill)
            {
                if (Alpha < 1.0)
                {
                    Alpha += 1.0f / 1000.0f * Main.dayRate;
                    if (Alpha > 1.0)
                        Alpha = 1f;
                }
            }
            else
            {
                Alpha -= 1.0f / 1000.0f * Main.dayRate;
                if (Alpha <= 0.0)
                    active = false;
            }
            if (position.X + Main.cloudTexture[type].Width * (double)scale < -num || position.X > Main.screenWidth + (double)num)
                active = false;
            rSpeed += rand.Next(-10, 11) * 2E-05f;
            if (rSpeed > 0.0002)
                rSpeed = 0.0002f;
            if (rSpeed < -0.0002)
                rSpeed = -0.0002f;
            if (rotation > 0.02)
                rotation = 0.02f;
            if (rotation < -0.02)
                rotation = -0.02f;
            rotation += rSpeed;
            width = (int)(Main.cloudTexture[type].Width * (double)scale);
            height = (int)(Main.cloudTexture[type].Height * (double)scale);
            if (type < 9 || type > 13 || Main.cloudAlpha <= 0.0 && Main.cloudBGActive < 1.0)
                return;
            kill = true;
        }
    }
}
