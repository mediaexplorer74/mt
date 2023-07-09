/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using Microsoft.Xna.Framework;
using System;

namespace GameManager
{
    public class Rain
    {
        public Vector2 position;
        public Vector2 velocity;
        public float scale;
        public float rotation;
        public int alpha;
        public bool active;
        public byte type;

        public static void MakeRain()
        {
            if ((double)Game1.screenPosition.Y > Game1.worldSurface * 16.0 || Game1.gameMenu)
                return;
            float num1 = (float)Game1.screenWidth / 1920f * 25f * (float)(0.25 + 1.0 * (double)Game1.cloudAlpha);
            for (int index = 0; (double)index < (double)num1; ++index)
            {
                int num2 = 600;
                if ((double)Game1.player[Game1.myPlayer].velocity.Y < 0.0)
                    num2 += (int)((double)Math.Abs(Game1.player[Game1.myPlayer].velocity.Y) * 30.0);
                Vector2 Position;
                Position.X = (float)Game1.rand.Next((int)Game1.screenPosition.X - num2, (int)Game1.screenPosition.X + Game1.screenWidth + num2);
                Position.Y = Game1.screenPosition.Y - (float)Game1.rand.Next(20, 100);
                Position.X -= (float)((double)Game1.windSpeed * 15.0 * 40.0);
                Position.X += Game1.player[Game1.myPlayer].velocity.X * 40f;
                if ((double)Position.X < 0.0)
                    Position.X = 0.0f;
                if ((double)Position.X > (double)((Game1.maxTilesX - 1) * 16))
                    Position.X = (float)((Game1.maxTilesX - 1) * 16);
                int i = (int)Position.X / 16;
                int j = (int)Position.Y / 16;
                if (i < 0)
                    i = 0;
                if (i > Game1.maxTilesX - 1)
                    i = Game1.maxTilesX - 1;
                if (Game1.gameMenu || !WorldGen.SolidTile(i, j) && (int)Game1.tile[i, j].wall <= 0)
                {
                    Vector2 Velocity = new Vector2(Game1.windSpeed * 12f, 14f);
                    Rain.NewRain(Position, Velocity);
                }
            }
        }

        public void Update()
        {
            this.position += this.velocity;
            if (!Collision.SolidCollision(this.position, 2, 2) && (double)this.position.Y <= (double)Game1.screenPosition.Y + (double)Game1.screenHeight + 100.0 && !Collision.WetCollision(this.position, 2, 2))
                return;
            this.active = false;
            if ((double)Game1.rand.Next(100) >= (double)Game1.gfxQuality * 100.0)
                return;
            int Type = 154;
            if ((int)this.type == 3 || (int)this.type == 4 || (int)this.type == 5)
                Type = 218;
            int index = Dust.NewDust(this.position - this.velocity, 2, 2, Type, 0.0f, 0.0f, 0, new Color(), 1f);
            Game1.dust[index].position.X -= 2f;
            Game1.dust[index].alpha = 38;
            Game1.dust[index].velocity *= 0.1f;
            Game1.dust[index].velocity += -this.velocity * 0.025f;
            Game1.dust[index].scale = 0.75f;
        }

        public static int NewRain(Vector2 Position, Vector2 Velocity)
        {
            int index1 = -1;
            int num1 = (int)((double)Game1.maxRain * (double)Game1.cloudAlpha);
            if (num1 > Game1.maxRain)
                num1 = Game1.maxRain;
            float num2 = (float)((1.0 + (double)Game1.gfxQuality) / 2.0);
            if ((double)num2 < 0.9)
                num1 = (int)((double)num1 * (double)num2);
            float num3 = (float)(800 - Game1.snowTiles);
            if ((double)num3 < 0.0)
                num3 = 0.0f;
            float num4 = num3 / 800f;
            int num5 = (int)((double)num1 * (double)num4);
            for (int index2 = 0; index2 < num5; ++index2)
            {
                if (!Game1.rain[index2].active)
                {
                    index1 = index2;
                    break;
                }
            }
            if (index1 == -1)
                return Game1.maxRain;
            Rain rain = Game1.rain[index1];
            rain.active = true;
            rain.position = Position;
            rain.scale = (float)(1.0 + (double)Game1.rand.Next(-20, 21) * 0.00999999977648258);
            rain.velocity = Velocity * rain.scale;
            rain.rotation = (float)Math.Atan2((double)rain.velocity.X, -(double)rain.velocity.Y);
            rain.type = (byte)Game1.rand.Next(3);
            if (Game1.bloodMoon)
                rain.type += (byte)3;
            return index1;
        }
    }
}
