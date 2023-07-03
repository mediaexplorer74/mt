﻿/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameManager;

namespace GameManager.GameContent.Events
{
    internal class ScreenDarkness
    {
        public static float screenObstruction;

        public static void Update()
        {
            float num = 0.0f;
            float amount = 0.1f;
            Vector2 mountedCenter = Main.player[Main.myPlayer].MountedCenter;
            for (int index = 0; index < 200; ++index)
            {
                if (Main.npc[index].active && Main.npc[index].type == 370 && Main.npc[index].Distance(mountedCenter) < 3000.0 && (Main.npc[index].ai[0] >= 10.0 ||
                    Main.npc[index].ai[0] == 9.0 && Main.npc[index].ai[2] > 120.0))
                {
                    num = 0.95f;
                    amount = 0.03f;
                }
            }

            screenObstruction = MathHelper.Lerp(screenObstruction, num, amount);
        }

        public static void DrawBack(SpriteBatch spriteBatch)
        {
            if (screenObstruction == 0.0)
                return;

            Color color = Color.Black * screenObstruction;
            spriteBatch.Draw(Main.magicPixel, new Rectangle(-2, -2, Main.screenWidth + 4, Main.screenHeight + 4), new Rectangle?(new Rectangle(0, 0, 1, 1)), color);
        }

        public static void DrawFront(SpriteBatch spriteBatch)
        {
            if (screenObstruction == 0.0)
                return;

            Color color = new Color(0, 0, 120) * screenObstruction * 0.3f;
            spriteBatch.Draw(Main.magicPixel, new Rectangle(-2, -2, Main.screenWidth + 4, Main.screenHeight + 4), new Rectangle?(new Rectangle(0, 0, 1, 1)), color);
        }
    }
}
