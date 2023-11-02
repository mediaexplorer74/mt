// Decompiled with JetBrains decompiler
// Type: GameManager.Sign
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 09A1FDD7-68B3-40FF-A030-32890DF6B0E7
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager v1 Beta\GameManager.exe

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
          Game1.sign[index] = (Sign) null;
      }
    }

    public static int ReadSign(int i, int j)
    {
      int num1 = (int) Game1.tile[i, j].frameX / 18;
      int num2 = (int) Game1.tile[i, j].frameY / 18;
      while (num1 > 1)
        num1 -= 2;
      int x = i - num1;
      int y = j - num2;
      if (Game1.tile[x, y].type != (byte) 55)
      {
        Sign.KillSign(x, y);
        return -1;
      }
      int num3 = -1;
      for (int index = 0; index < 1000; ++index)
      {
        if (Game1.sign[index] != null && Game1.sign[index].x == x && Game1.sign[index].y == y)
        {
          num3 = index;
          break;
        }
      }
      if (num3 < 0)
      {
        for (int index = 0; index < 1000; ++index)
        {
          if (Game1.sign[index] == null)
          {
            num3 = index;
            Game1.sign[index] = new Sign();
            Game1.sign[index].x = x;
            Game1.sign[index].y = y;
            Game1.sign[index].text = "";
            break;
          }
        }
      }
      return num3;
    }

    public static void TextSign(int i, string text)
    {
      if (Game1.tile[Game1.sign[i].x, Game1.sign[i].y] == null || !Game1.tile[Game1.sign[i].x, Game1.sign[i].y].active || Game1.tile[Game1.sign[i].x, Game1.sign[i].y].type != (byte) 55)
        Game1.sign[i] = (Sign) null;
      else
        Game1.sign[i].text = text;
    }
  }
}
