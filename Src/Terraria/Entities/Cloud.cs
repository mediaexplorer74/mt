// Cloud

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
            if (Game1.dedServ || Game1.cloudLimit < 10)
                return;
            Game1.windSpeed = Game1.windSpeedSet;
            for (int index = 0; index < 200; ++index)
                Game1.cloud[index].active = false;
            for (int index = 0; index < Game1.numClouds; ++index)
            {
                addCloud();
                Game1.cloud[index].Alpha = 1f;
            }
            for (int index = 0; index < 200; ++index)
                Game1.cloud[index].Alpha = 1f;
        }

        public static void addCloud()
        {
            if (Game1.netMode == 2)
                return;
            int index1 = -1;
            for (int index2 = 0; index2 < 200; ++index2)
            {
                if (!Game1.cloud[index2].active)
                {
                    index1 = index2;
                    break;
                }
            }
            if (index1 < 0)
                return;
            Game1.cloud[index1].kill = false;
            Game1.cloud[index1].rSpeed = 0.0f;
            Game1.cloud[index1].sSpeed = 0.0f;
            Game1.cloud[index1].scale = rand.Next(70, 131) * 0.01f;
            Game1.cloud[index1].rotation = rand.Next(-10, 11) * 0.01f;
            Game1.cloud[index1].width = (int)(Game1.cloudTexture[Game1.cloud[index1].type].Width * (double)Game1.cloud[index1].scale);
            Game1.cloud[index1].height = (int)(Game1.cloudTexture[Game1.cloud[index1].type].Height * (double)Game1.cloud[index1].scale);
            Game1.cloud[index1].Alpha = 0.0f;
            Game1.cloud[index1].spriteDir = SpriteEffects.None;
            if (rand.Next(2) == 0)
                Game1.cloud[index1].spriteDir = SpriteEffects.FlipHorizontally;
            float num1 = Game1.windSpeed;
            if (!Game1.gameMenu)
                num1 = Game1.windSpeed - Game1.player[Game1.myPlayer].velocity.X * 0.1f;
            int num2 = 0;
            int num3 = 0;
            if (num1 > 0.0)
                num2 -= 200;
            if (num1 < 0.0)
                num3 += 200;
            int num4 = 300;
            float num5 = WorldGen.genRand.Next(num2 - num4, Game1.screenWidth + num3 + num4);
            Game1.cloud[index1].Alpha = 0.0f;
            Game1.cloud[index1].position.Y = rand.Next((int)(-Game1.screenHeight * 0.25), (int)(Game1.screenHeight * 0.25));
            Game1.cloud[index1].position.Y -= rand.Next((int)(Game1.screenHeight * 0.150000005960464));
            Game1.cloud[index1].position.Y -= rand.Next((int)(Game1.screenHeight * 0.150000005960464));
            Game1.cloud[index1].type = rand.Next(4);
            if (Game1.rand == null)
                Game1.rand = new Random();
            if (Game1.cloudAlpha > 0.0 && rand.Next(4) != 0 || Game1.cloudBGActive >= 1.0 && Game1.rand.Next(2) == 0)
            {
                Game1.cloud[index1].type = rand.Next(18, 22);
                if (Game1.cloud[index1].scale >= 1.15)
                    Game1.cloud[index1].position.Y -= 150f;
                if (Game1.cloud[index1].scale >= 1.0)
                    Game1.cloud[index1].position.Y -= 150f;
            }
            else if ((Game1.cloudBGActive <= 0.0 && Game1.cloudAlpha == 0.0 && (Game1.cloud[index1].scale < 1.0 && Game1.cloud[index1].position.Y < -Game1.screenHeight * 0.200000002980232) || Game1.cloud[index1].position.Y < -Game1.screenHeight * 0.200000002980232) && Game1.numClouds < 50.0)
                Game1.cloud[index1].type = rand.Next(9, 14);
            else if ((Game1.cloud[index1].scale < 1.15 && Game1.cloud[index1].position.Y < -Game1.screenHeight * 0.300000011920929 || Game1.cloud[index1].scale < 0.85 && Game1.cloud[index1].position.Y < Game1.screenHeight * 0.150000005960464) && (Game1.numClouds > 70.0 || Game1.cloudBGActive >= 1.0))
                Game1.cloud[index1].type = rand.Next(4, 9);
            else if (Game1.cloud[index1].position.Y > -Game1.screenHeight * 0.150000005960464 && rand.Next(2) == 0 && Game1.numClouds > 20.0)
                Game1.cloud[index1].type = rand.Next(14, 18);
            if (Game1.cloud[index1].scale > 1.2)
                Game1.cloud[index1].position.Y += 100f;
            if (Game1.cloud[index1].scale > 1.3)
                Game1.cloud[index1].scale = 1.3f;
            if (Game1.cloud[index1].scale < 0.7)
                Game1.cloud[index1].scale = 0.7f;
            Game1.cloud[index1].active = true;
            Game1.cloud[index1].position.X = num5;
            if (Game1.cloud[index1].position.X > (Game1.screenWidth + 100))
                Game1.cloud[index1].Alpha = 1f;
            if (Game1.cloud[index1].position.X + Game1.cloudTexture[Game1.cloud[index1].type].Width * (double)Game1.cloud[index1].scale < -100.0)
                Game1.cloud[index1].Alpha = 1f;
            Rectangle rectangle1 = new Rectangle((int)Game1.cloud[index1].position.X, (int)Game1.cloud[index1].position.Y, Game1.cloud[index1].width, Game1.cloud[index1].height);
            for (int index2 = 0; index2 < 200; ++index2)
            {
                if (index1 != index2 && Game1.cloud[index2].active)
                {
                    Rectangle rectangle2 = new Rectangle((int)Game1.cloud[index2].position.X, (int)Game1.cloud[index2].position.Y, Game1.cloud[index2].width, Game1.cloud[index2].height);
                    if (rectangle1.Intersects(rectangle2))
                        Game1.cloud[index1].active = false;
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
            if (Game1.netMode == 2)
                return;
            int maxValue = 0;
            for (int index = 0; index < 200; ++index)
            {
                if (Game1.cloud[index].active)
                {
                    Game1.cloud[index].Update();
                    if (!Game1.cloud[index].kill)
                        ++maxValue;
                }
            }
            for (int index = 0; index < 200; ++index)
            {
                if (Game1.cloud[index].active)
                {
                    if (index > 1 && (!Game1.cloud[index - 1].active || Game1.cloud[index - 1].scale > Game1.cloud[index].scale + 0.02))
                    {
                        Cloud cloud = (Cloud)Game1.cloud[index - 1].Clone();
                        Game1.cloud[index - 1] = (Cloud)Game1.cloud[index].Clone();
                        Game1.cloud[index] = cloud;
                    }
                    if (index < 199 && (!Game1.cloud[index].active || Game1.cloud[index + 1].scale < Game1.cloud[index].scale - 0.02))
                    {
                        Cloud cloud = (Cloud)Game1.cloud[index + 1].Clone();
                        Game1.cloud[index + 1] = (Cloud)Game1.cloud[index].Clone();
                        Game1.cloud[index] = cloud;
                    }
                }
            }
            if (maxValue < Game1.numClouds)
            {
                addCloud();
            }
            else
            {
                if (maxValue <= Game1.numClouds)
                    return;
                int index1 = Game1.rand.Next(maxValue);
                for (int index2 = 0; Game1.cloud[index1].kill && index2 < 100; index1 = Game1.rand.Next(maxValue))
                    ++index2;
                Game1.cloud[index1].kill = true;
            }
        }

        public void Update()
        {
            if (Game1.gameMenu)
            {
                position.X += (float)(Game1.windSpeed * scale * 3.0);
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
                position.X += (float)(Game1.windSpeed * (double)num1 * 5.0) * Game1.dayRate;
                position.X -= (Game1.screenPosition.X - Game1.screenLastPosition.X) * num1;
            }
            float num = 600f;
            if (!kill)
            {
                if (Alpha < 1.0)
                {
                    Alpha += 1.0f / 1000.0f * Game1.dayRate;
                    if (Alpha > 1.0)
                        Alpha = 1f;
                }
            }
            else
            {
                Alpha -= 1.0f / 1000.0f * Game1.dayRate;
                if (Alpha <= 0.0)
                    active = false;
            }
            if (position.X + Game1.cloudTexture[type].Width * (double)scale < -num || position.X > Game1.screenWidth + (double)num)
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
            width = (int)(Game1.cloudTexture[type].Width * (double)scale);
            height = (int)(Game1.cloudTexture[type].Height * (double)scale);
            if (type < 9 || type > 13 || Game1.cloudAlpha <= 0.0 && Game1.cloudBGActive < 1.0)
                return;
            kill = true;
        }
    }
}
