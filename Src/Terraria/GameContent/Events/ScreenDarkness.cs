﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Events.ScreenDarkness
// Assembly: Terraria, Version=1.3.5.3, Culture=neutral, PublicKeyToken=null
// MVID: 68659D26-2BE6-448F-8663-74FA559E6F08
// Assembly location: H:\Steam\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.GameContent.Events
{
  public class ScreenDarkness
  {
    public static float screenObstruction;

    public static void Update()
    {
      float num1 = 0.0f;
      float num2 = 0.1f;
      Vector2 mountedCenter = Main.player[Main.myPlayer].MountedCenter;
      for (int index = 0; index < 200; ++index)
      {
        if (Main.npc[index].active && Main.npc[index].type == 370 && (double) Main.npc[index].Distance(mountedCenter) < 3000.0 && ((double) Main.npc[index].ai[0] >= 10.0 || (double) Main.npc[index].ai[0] == 9.0 && (double) Main.npc[index].ai[2] > 120.0))
        {
          num1 = 0.95f;
          num2 = 0.03f;
        }
      }
      ScreenDarkness.screenObstruction = MathHelper.Lerp(ScreenDarkness.screenObstruction, num1, num2);
    }

    public static void DrawBack(SpriteBatch spriteBatch)
    {
      if ((double) ScreenDarkness.screenObstruction == 0.0)
        return;
      Color color = Color.op_Multiply(Color.get_Black(), ScreenDarkness.screenObstruction);
      spriteBatch.Draw(Main.magicPixel, new Rectangle(-2, -2, Main.screenWidth + 4, Main.screenHeight + 4), new Rectangle?(new Rectangle(0, 0, 1, 1)), color);
    }

    public static void DrawFront(SpriteBatch spriteBatch)
    {
      if ((double) ScreenDarkness.screenObstruction == 0.0)
        return;
      Color color = Color.op_Multiply(Color.op_Multiply(new Color(0, 0, 120), ScreenDarkness.screenObstruction), 0.3f);
      spriteBatch.Draw(Main.magicPixel, new Rectangle(-2, -2, Main.screenWidth + 4, Main.screenHeight + 4), new Rectangle?(new Rectangle(0, 0, 1, 1)), color);
    }
  }
}
