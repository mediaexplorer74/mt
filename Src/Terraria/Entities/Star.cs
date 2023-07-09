// Star

using Microsoft.Xna.Framework;

namespace GameManager
{
    public class Star
    {
        public Vector2 position;
        public float scale;
        public float rotation;
        public int type;
        public float twinkle;
        public float twinkleSpeed;
        public float rotationSpeed;

        public static void SpawnStars()
        {
            Game1.numStars = Game1.rand.Next(65, 130);
            Game1.numStars = 130;
            for (int index = 0; index < Game1.numStars; ++index)
            {
                Game1.star[index] = new Star();
                Game1.star[index].position.X = Game1.rand.Next(-12, Game1.screenWidth + 1);
                Game1.star[index].position.Y = Game1.rand.Next(-12, Game1.screenHeight);
                Game1.star[index].rotation = Game1.rand.Next(628) * 0.01f;
                Game1.star[index].scale = Game1.rand.Next(50, 120) * 0.01f;
                Game1.star[index].type = Game1.rand.Next(0, 5);
                Game1.star[index].twinkle = Game1.rand.Next(101) * 0.01f;
                Game1.star[index].twinkleSpeed = Game1.rand.Next(40, 100) * 0.0001f;
                if (Game1.rand.Next(2) == 0)
                    Game1.star[index].twinkleSpeed *= -1f;
                Game1.star[index].rotationSpeed = Game1.rand.Next(10, 40) * 0.0001f;
                if (Game1.rand.Next(2) == 0)
                    Game1.star[index].rotationSpeed *= -1f;
            }
        }

        public static void UpdateStars()
        {
            for (int index = 0; index < Game1.numStars; ++index)
            {
                Game1.star[index].twinkle += Game1.star[index].twinkleSpeed;
                if (Game1.star[index].twinkle > 1.0)
                {
                    Game1.star[index].twinkle = 1f;
                    Game1.star[index].twinkleSpeed *= -1f;
                }
                else if (Game1.star[index].twinkle < 0.5)
                {
                    Game1.star[index].twinkle = 0.5f;
                    Game1.star[index].twinkleSpeed *= -1f;
                }
                Game1.star[index].rotation += Game1.star[index].rotationSpeed;
                if (Game1.star[index].rotation > 6.28)
                    Game1.star[index].rotation -= 6.28f;
                if (Game1.star[index].rotation < 0.0)
                    Game1.star[index].rotation += 6.28f;
            }
        }
    }
}
