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
using GameManager.ID;

namespace GameManager
{
    public static class DelegateMethods
    {
        public static Vector3 v3_1 = Vector3.Zero;
        public static float f_1 = 0.0f;
        public static Color c_1 = Color.Transparent;
        public static int i_1 = 0;

        public static bool TestDust(int x, int y)
        {
            if (x < 0 || x >= Game1.maxTilesX || (y < 0 || y >= Game1.maxTilesY))
                return false;
            int index = Dust.NewDust(new Vector2(x, y) * 16f + new Vector2(8f), 0, 0, 6, 0.0f, 0.0f, 0, new Color(), 1f);
            Game1.dust[index].noGravity = true;
            Game1.dust[index].noLight = true;
            return true;
        }

        public static bool CastLight(int x, int y)
        {
            if (x < 0 || x >= Game1.maxTilesX || (y < 0 || y >= Game1.maxTilesY) || Game1.tile[x, y] == null)
                return false;
            Lighting.AddLight(x, y, v3_1.X, v3_1.Y, v3_1.Z);
            return true;
        }

        public static bool CastLightOpen(int x, int y)
        {
            if (x < 0 || x >= Game1.maxTilesX || (y < 0 || y >= Game1.maxTilesY) || Game1.tile[x, y] == null)
                return false;
            if (!Game1.tile[x, y].active() || Game1.tile[x, y].inActive() || (Game1.tileSolidTop[Game1.tile[x, y].type] || !Game1.tileSolid[Game1.tile[x, y].type]))
                Lighting.AddLight(x, y, v3_1.X, v3_1.Y, v3_1.Z);
            return true;
        }

        public static bool NotDoorStand(int x, int y)
        {
            if (Game1.tile[x, y] == null || !Game1.tile[x, y].active() || Game1.tile[x, y].type != 11)
                return true;
            if (Game1.tile[x, y].frameX >= 18)
                return Game1.tile[x, y].frameX < 54;
            return false;
        }

        public static bool CutTiles(int x, int y)
        {
            if (!WorldGen.InWorld(x, y, 1) || Game1.tile[x, y] == null)
                return false;
            if (!Game1.tileCut[Game1.tile[x, y].type] || Game1.tile[x, y + 1] == null || (Game1.tile[x, y + 1].type == 78 || Game1.tile[x, y + 1].type == 380))
                return true;
            WorldGen.KillTile(x, y, false, false, false);
            if (Game1.netMode != 0)
                NetMessage.SendData(17, -1, -1, "", 0, x, y, 0.0f, 0, 0, 0);
            return true;
        }

        public static bool SearchAvoidedByNPCs(int x, int y)
        {
            return WorldGen.InWorld(x, y, 1) && Game1.tile[x, y] != null && (!Game1.tile[x, y].active() || !TileID.Sets.AvoidedByNPCs[Game1.tile[x, y].type]);
        }

        public static void RainbowLaserDraw(int stage, Vector2 currentPosition, float distanceLeft, Rectangle lastFrame, out float distCovered, out Rectangle frame, out Vector2 origin, out Color color)
        {
            color = c_1;
            if (stage == 0)
            {
                distCovered = 33f;
                frame = new Rectangle(0, 0, 26, 22);
                origin = Utils.Size(frame) / 2f;
            }
            else if (stage == 1)
            {
                frame = new Rectangle(0, 25, 26, 28);
                distCovered = frame.Height;
                origin = new Vector2(frame.Width / 2, 0.0f);
            }
            else if (stage == 2)
            {
                distCovered = 22f;
                frame = new Rectangle(0, 56, 26, 22);
                origin = new Vector2(frame.Width / 2, 1f);
            }
            else
            {
                distCovered = 9999f;
                frame = Rectangle.Empty;
                origin = Vector2.Zero;
                color = Color.Transparent;
            }
        }

        public static void TurretLaserDraw(int stage, Vector2 currentPosition, float distanceLeft, Rectangle lastFrame, out float distCovered, out Rectangle frame, out Vector2 origin, out Color color)
        {
            color = c_1;
            if (stage == 0)
            {
                distCovered = 32f;
                frame = new Rectangle(0, 0, 22, 20);
                origin = Utils.Size(frame) / 2f;
            }
            else if (stage == 1)
            {
                ++i_1;
                int num = i_1 % 5;
                frame = new Rectangle(0, 22 * (num + 1), 22, 20);
                distCovered = frame.Height - 1;
                origin = new Vector2(frame.Width / 2, 0.0f);
            }
            else if (stage == 2)
            {
                frame = new Rectangle(0, 154, 22, 30);
                distCovered = frame.Height;
                origin = new Vector2(frame.Width / 2, 1f);
            }
            else
            {
                distCovered = 9999f;
                frame = Rectangle.Empty;
                origin = Vector2.Zero;
                color = Color.Transparent;
            }
        }

        public static void LightningLaserDraw(int stage, Vector2 currentPosition, float distanceLeft, Rectangle lastFrame, out float distCovered, out Rectangle frame, out Vector2 origin, out Color color)
        {
            color = c_1 * f_1;
            if (stage == 0)
            {
                distCovered = 0.0f;
                frame = new Rectangle(0, 0, 21, 8);
                origin = Utils.Size(frame) / 2f;
            }
            else if (stage == 1)
            {
                frame = new Rectangle(0, 8, 21, 6);
                distCovered = frame.Height;
                origin = new Vector2(frame.Width / 2, 0.0f);
            }
            else if (stage == 2)
            {
                distCovered = 8f;
                frame = new Rectangle(0, 14, 21, 8);
                origin = new Vector2(frame.Width / 2, 2f);
            }
            else
            {
                distCovered = 9999f;
                frame = Rectangle.Empty;
                origin = Vector2.Zero;
                color = Color.Transparent;
            }
        }

        public static int CompareYReverse(Point a, Point b)
        {
            return b.Y.CompareTo(a.Y);
        }

        public static class Minecart
        {
            public static Vector2 rotationOrigin;
            public static float rotation;

            public static void Sparks(Vector2 dustPosition)
            {
                dustPosition += Utils.RotatedBy(new Vector2(Game1.rand.Next(2) == 0 ? 13f : -13f, 0.0f), rotation, new Vector2());
                int index = Dust.NewDust(dustPosition, 1, 1, 213, Game1.rand.Next(-2, 3), Game1.rand.Next(-2, 3), 0, new Color(), 1f);
                Game1.dust[index].noGravity = true;
                Game1.dust[index].fadeIn = (float)(Game1.dust[index].scale + 1.0 + 0.00999999977648258 * Game1.rand.Next(0, 51));
                Game1.dust[index].noGravity = true;
                Game1.dust[index].velocity *= Game1.rand.Next(15, 51) * 0.01f;
                Game1.dust[index].velocity.X *= Game1.rand.Next(25, 101) * 0.01f;
                Game1.dust[index].velocity.Y -= Game1.rand.Next(15, 31) * 0.1f;
                Game1.dust[index].position.Y -= 4f;
                if (Game1.rand.Next(3) != 0)
                    Game1.dust[index].noGravity = false;
                else
                    Game1.dust[index].scale *= 0.6f;
            }

            public static void SparksMech(Vector2 dustPosition)
            {
                dustPosition += Utils.RotatedBy(new Vector2(Game1.rand.Next(2) == 0 ? 13f : -13f, 0.0f), rotation, new Vector2());
                int index = Dust.NewDust(dustPosition, 1, 1, 260, Game1.rand.Next(-2, 3), Game1.rand.Next(-2, 3), 0, new Color(), 1f);
                Game1.dust[index].noGravity = true;
                Game1.dust[index].fadeIn = (float)(Game1.dust[index].scale + 0.5 + 0.00999999977648258 * Game1.rand.Next(0, 51));
                Game1.dust[index].noGravity = true;
                Game1.dust[index].velocity *= Game1.rand.Next(15, 51) * 0.01f;
                Game1.dust[index].velocity.X *= Game1.rand.Next(25, 101) * 0.01f;
                Game1.dust[index].velocity.Y -= Game1.rand.Next(15, 31) * 0.1f;
                Game1.dust[index].position.Y -= 4f;
                if (Game1.rand.Next(3) != 0)
                    Game1.dust[index].noGravity = false;
                else
                    Game1.dust[index].scale *= 0.6f;
            }
        }
    }
}
