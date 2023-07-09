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
    public class LiquidBuffer
    {
        public const int maxLiquidBuffer = 10000;
        public static int numLiquidBuffer;
        public int x;
        public int y;

        public static void AddBuffer(int x, int y)
        {
            if (LiquidBuffer.numLiquidBuffer == 9999 || Game1.tile[x, y].checkingLiquid())
                return;
            Game1.tile[x, y].checkingLiquid(true);
            Game1.liquidBuffer[LiquidBuffer.numLiquidBuffer].x = x;
            Game1.liquidBuffer[LiquidBuffer.numLiquidBuffer].y = y;
            ++LiquidBuffer.numLiquidBuffer;
        }

        public static void DelBuffer(int l)
        {
            --LiquidBuffer.numLiquidBuffer;
            Game1.liquidBuffer[l].x = Game1.liquidBuffer[LiquidBuffer.numLiquidBuffer].x;
            Game1.liquidBuffer[l].y = Game1.liquidBuffer[LiquidBuffer.numLiquidBuffer].y;
        }
    }
}
