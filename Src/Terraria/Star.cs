﻿// Decompiled with JetBrains decompiler
// Type: Terraria.Star
// Assembly: Terraria, Version=1.3.5.3, Culture=neutral, PublicKeyToken=null
// MVID: 68659D26-2BE6-448F-8663-74FA559E6F08
// Assembly location: H:\Steam\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;

namespace Terraria
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
      Main.numStars = Main.rand.Next(65, 130);
      Main.numStars = 130;
      for (int index = 0; index < Main.numStars; ++index)
      {
        Main.star[index] = new Star();
        Main.star[index].position.X = (__Null) (double) Main.rand.Next(-12, Main.screenWidth + 1);
        Main.star[index].position.Y = (__Null) (double) Main.rand.Next(-12, Main.screenHeight);
        Main.star[index].rotation = (float) Main.rand.Next(628) * 0.01f;
        Main.star[index].scale = (float) Main.rand.Next(50, 120) * 0.01f;
        Main.star[index].type = Main.rand.Next(0, 5);
        Main.star[index].twinkle = (float) Main.rand.Next(101) * 0.01f;
        Main.star[index].twinkleSpeed = (float) Main.rand.Next(40, 100) * 0.0001f;
        if (Main.rand.Next(2) == 0)
          Main.star[index].twinkleSpeed *= -1f;
        Main.star[index].rotationSpeed = (float) Main.rand.Next(10, 40) * 0.0001f;
        if (Main.rand.Next(2) == 0)
          Main.star[index].rotationSpeed *= -1f;
      }
    }

    public static void UpdateStars()
    {
      for (int index = 0; index < Main.numStars; ++index)
      {
        Main.star[index].twinkle += Main.star[index].twinkleSpeed;
        if ((double) Main.star[index].twinkle > 1.0)
        {
          Main.star[index].twinkle = 1f;
          Main.star[index].twinkleSpeed *= -1f;
        }
        else if ((double) Main.star[index].twinkle < 0.5)
        {
          Main.star[index].twinkle = 0.5f;
          Main.star[index].twinkleSpeed *= -1f;
        }
        Main.star[index].rotation += Main.star[index].rotationSpeed;
        if ((double) Main.star[index].rotation > 6.28)
          Main.star[index].rotation -= 6.28f;
        if ((double) Main.star[index].rotation < 0.0)
          Main.star[index].rotation += 6.28f;
      }
    }
  }
}
