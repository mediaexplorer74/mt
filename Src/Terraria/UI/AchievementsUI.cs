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
using Microsoft.Xna.Framework.Graphics;
using GameManager;
using GameManager.Achievements;

namespace GameManager.UI
{
    public class AchievementsUI
    {
        public static void Open()
        {
            Game1.playerInventory = false;
            Game1.editChest = false;
            Game1.npcChatText = "";
            Game1.achievementsWindow = true;
            Game1.InGameUI.SetState((UIState)Game1.AchievementsMenu);
        }

        public static void OpenAndGoto(Achievement achievement)
        {
            AchievementsUI.Open();
            Game1.AchievementsMenu.GotoAchievement(achievement);
        }

        public static void Close()
        {
            Game1.achievementsWindow = false;
            Game1.PlaySound(11, -1, -1, 1);
            if (!Game1.gameMenu)
                Game1.playerInventory = true;
            Game1.InGameUI.SetState((UIState)null);
        }

        public static void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (!Game1.gameMenu && Game1.player[Game1.myPlayer].dead && !Game1.player[Game1.myPlayer].ghost)
            {
                AchievementsUI.Close();
                Game1.playerInventory = false;
            }
            else
            {
                if (Game1.gameMenu)
                    return;
                Game1.mouseText = false;
                Game1.instance.GUIBarsDraw();
                if (!Game1.achievementsWindow)
                    Game1.InGameUI.SetState((UIState)null);
                Game1.instance.DrawMouseOver();
                Game1.DrawThickCursor(false);
                spriteBatch.Draw(Game1.cursorTextures[0], new Vector2((float)(Game1.mouseX + 1), (float)(Game1.mouseY + 1)), new Rectangle?(), new Color((int)((double)Game1.cursorColor.R * 0.200000002980232), (int)((double)Game1.cursorColor.G * 0.200000002980232), (int)((double)Game1.cursorColor.B * 0.200000002980232), (int)((double)Game1.cursorColor.A * 0.5)), 0.0f, new Vector2(), Game1.cursorScale * 1.1f, SpriteEffects.None, 0.0f);
                spriteBatch.Draw(Game1.cursorTextures[0], new Vector2((float)Game1.mouseX, (float)Game1.mouseY), new Rectangle?(), Game1.cursorColor, 0.0f, new Vector2(), Game1.cursorScale, SpriteEffects.None, 0.0f);
            }
        }

        public static void MouseOver()
        {
            if (!Game1.achievementsWindow || !Game1.InGameUI.IsElementUnderMouse())
                return;
            Game1.mouseText = true;
        }
    }
}
