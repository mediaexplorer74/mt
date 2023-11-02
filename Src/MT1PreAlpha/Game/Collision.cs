
using Microsoft.Xna.Framework;

namespace GameManager
{
  public class Collision
  {
    public static bool EmptyTile(int i, int j)
    {
      Rectangle rectangle = new Rectangle(i * 16, j * 16, 16, 16);
      if (Game1.tile[i, j].active)
        return false;
      for (int index = 0; index < 16; ++index)
      {
        if (Game1.player[index].active && rectangle.Intersects(new Rectangle((int) Game1.player[index].position.X, (int) Game1.player[index].position.Y, Game1.player[index].width, Game1.player[index].height)))
          return false;
      }
      for (int index = 0; index < 1000; ++index)
      {
        if (Game1.item[index].active && rectangle.Intersects(new Rectangle((int) Game1.item[index].position.X, (int) Game1.item[index].position.Y, Game1.item[index].width, Game1.item[index].height)))
          return false;
      }
      for (int index = 0; index < 1000; ++index)
      {
        if (Game1.npc[index].active && rectangle.Intersects(new Rectangle((int) Game1.npc[index].position.X, (int) Game1.npc[index].position.Y, Game1.npc[index].width, Game1.npc[index].height)))
          return false;
      }
      return true;
    }

    public static Vector2 TileCollision(Vector2 Position, Vector2 Velocity, int Width, int Height)
    {
      Vector2 vector2_1 = Velocity;
      Vector2 vector2_2 = Velocity;
      Vector2 vector2_3 = Position + Velocity;
      Vector2 vector2_4 = Position;
      int num1 = (int) ((double) Position.X / 16.0) - 1;
      int num2 = (int) (((double) Position.X + (double) Width) / 16.0) + 2;
      int num3 = (int) ((double) Position.Y / 16.0) - 1;
      int num4 = (int) (((double) Position.Y + (double) Height) / 16.0) + 2;
      int num5 = -1;
      int num6 = -1;
      int num7 = -1;
      int num8 = -1;
      if (num1 < 0)
        num1 = 0;
      if (num2 > 5001)
        num2 = 5001;
      if (num3 < 0)
        num3 = 0;
      if (num4 > 2501)
        num4 = 2501;
      for (int index1 = num1; index1 < num2; ++index1)
      {
        for (int index2 = num3; index2 < num4; ++index2)
        {
          if (Game1.tile[index1, index2].active 
                        && Game1.tileSolid[(int) Game1.tile[index1, index2].type])
          {
            Vector2 vector2_5;
            vector2_5.X = (float) (index1 * 16);
            vector2_5.Y = (float) (index2 * 16);
            if ((double) vector2_3.X + (double) Width > (double) vector2_5.X 
                            && (double) vector2_3.X < (double) vector2_5.X + 16.0 && (double) vector2_3.Y + (double) Height > (double) vector2_5.Y && (double) vector2_3.Y < (double) vector2_5.Y + 16.0)
            {
              if ((double) vector2_4.Y + (double) Height <= (double) vector2_5.Y)
              {
                num7 = index1;
                num8 = index2;
                if (num7 != num5)
                  vector2_1.Y = vector2_5.Y - (vector2_4.Y + (float) Height);
              }
              else if ((double) vector2_4.X + (double) Width <= (double) vector2_5.X)
              {
                num5 = index1;
                num6 = index2;
                if (num6 != num8)
                  vector2_1.X = vector2_5.X - (vector2_4.X + (float) Width);
                if (num7 == num5)
                  vector2_1.Y = vector2_2.Y;
              }
              else if ((double) vector2_4.X >= (double) vector2_5.X + 16.0)
              {
                num5 = index1;
                num6 = index2;
                if (num6 != num8)
                  vector2_1.X = vector2_5.X + 16f - vector2_4.X;
                if (num7 == num5)
                  vector2_1.Y = vector2_2.Y;
              }
              else if ((double) vector2_4.Y >= (double) vector2_5.Y + 16.0)
              {
                num7 = index1;
                num8 = index2;
                vector2_1.Y = vector2_5.Y + 16f - vector2_4.Y;
                if (num8 == num6)
                  vector2_1.X = vector2_2.X + 0.01f;
              }
            }
          }
        }
      }
      return vector2_1;
    }
  }
}
