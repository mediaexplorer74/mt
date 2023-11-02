// Decompiled with JetBrains decompiler
// Type: GameManager.LiquidBuffer
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 09A1FDD7-68B3-40FF-A030-32890DF6B0E7
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager v1 Beta\GameManager.exe

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
      if (LiquidBuffer.numLiquidBuffer == 9999 || Game1.tile[x, y].checkingLiquid)
        return;
      Game1.tile[x, y].checkingLiquid = true;
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
