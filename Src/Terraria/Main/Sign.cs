/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

namespace GameManager
{
    public class Sign
    {
        public const int maxSigns = 1000;
        public int x;
        public int y;
        public string text;

        public static void KillSign(int x, int y)
        {
            for (int index = 0; index < 1000; ++index)
            {
                if (Game1.sign[index] != null && Game1.sign[index].x == x && Game1.sign[index].y == y)
                    Game1.sign[index] = (Sign)null;
            }
        }

        public static int ReadSign(int i, int j, bool CreateIfMissing = true)
        {
            int num1 = (int)Game1.tile[i, j].frameX / 18;
            int num2 = (int)Game1.tile[i, j].frameY / 18;
            int num3 = num1 % 2;
            int x = i - num3;
            int y = j - num2;
            if (!Game1.tileSign[(int)Game1.tile[x, y].type])
            {
                Sign.KillSign(x, y);
                return -1;
            }
            int num4 = -1;
            for (int index = 0; index < 1000; ++index)
            {
                if (Game1.sign[index] != null && Game1.sign[index].x == x && Game1.sign[index].y == y)
                {
                    num4 = index;
                    break;
                }
            }
            if (num4 < 0 && CreateIfMissing)
            {
                for (int index = 0; index < 1000; ++index)
                {
                    if (Game1.sign[index] == null)
                    {
                        num4 = index;
                        Game1.sign[index] = new Sign();
                        Game1.sign[index].x = x;
                        Game1.sign[index].y = y;
                        Game1.sign[index].text = "";
                        break;
                    }
                }
            }
            return num4;
        }

        public static void TextSign(int i, string text)
        {
            if (Game1.tile[Game1.sign[i].x, Game1.sign[i].y] == null || !Game1.tile[Game1.sign[i].x, Game1.sign[i].y].active() || (int)Game1.tile[Game1.sign[i].x, Game1.sign[i].y].type != 55 && (int)Game1.tile[Game1.sign[i].x, Game1.sign[i].y].type != 85)
                Game1.sign[i] = (Sign)null;
            else
                Game1.sign[i].text = text;
        }

        public override string ToString()
        {
            return "x" + (object)this.x + "\ty" + (string)(object)this.y + "\t" + this.text;
        }
    }
}
