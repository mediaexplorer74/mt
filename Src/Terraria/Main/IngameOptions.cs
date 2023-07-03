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
using Microsoft.Xna.Framework.Input;
using System;
using GameManager.GameContent;
using GameManager.UI;

namespace GameManager
{
    public static class IngameOptions
    {
        public static float[] leftScale = new float[8]
    {
      0.7f,
      0.7f,
      0.7f,
      0.7f,
      0.7f,
      0.7f,
      0.7f,
      0.7f
    };
        public static float[] rightScale = new float[15]
    {
      0.7f,
      0.7f,
      0.7f,
      0.7f,
      0.7f,
      0.7f,
      0.7f,
      0.7f,
      0.7f,
      0.7f,
      0.7f,
      0.7f,
      0.7f,
      0.7f,
      0.7f
    };
        public static int leftHover = -1;
        public static int rightHover = -1;
        public static int oldLeftHover = -1;
        public static int oldRightHover = -1;
        public static int rightLock = -1;
        public static bool inBar = false;
        public static bool notBar = false;
        public static bool noSound = false;
        private static Rectangle _GUIHover = new Rectangle();
        public static int category = 0;
        public static Vector2 valuePosition = Vector2.Zero;
        public const int width = 670;
        public const int height = 480;

        public static void Open()
        {
            Main.playerInventory = false;
            Main.editChest = false;
            Main.npcChatText = "";
            Main.PlaySound(10, -1, -1, 1);
            Main.ingameOptionsWindow = true;
            IngameOptions.category = 0;
            for (int index = 0; index < IngameOptions.leftScale.Length; ++index)
                IngameOptions.leftScale[index] = 0.0f;
            for (int index = 0; index < IngameOptions.rightScale.Length; ++index)
                IngameOptions.rightScale[index] = 0.0f;
            IngameOptions.leftHover = -1;
            IngameOptions.rightHover = -1;
            IngameOptions.oldLeftHover = -1;
            IngameOptions.oldRightHover = -1;
            IngameOptions.rightLock = -1;
            IngameOptions.inBar = false;
            IngameOptions.notBar = false;
            IngameOptions.noSound = false;
        }

        public static void Close()
        {
            if (Main.setKey != -1)
                return;
            Main.SaveSettings();
            Main.ingameOptionsWindow = false;
            Main.PlaySound(11, -1, -1, 1);
            Recipe.FindRecipes();
            Main.playerInventory = true;
        }

        public static void Draw(Main mainInstance, SpriteBatch sb)
        {
            if (Main.player[Main.myPlayer].dead && !Main.player[Main.myPlayer].ghost)
            {
                Main.setKey = -1;
                IngameOptions.Close();
                Main.playerInventory = false;
            }
            else
            {
                Vector2 vector2_1 = new Vector2((float)Main.mouseX, (float)Main.mouseY);
                bool flag1 = Main.mouseLeft && Main.mouseLeftRelease;
                Vector2 vector2_2 = new Vector2((float)Main.screenWidth, (float)Main.screenHeight);
                Vector2 vector2_3 = new Vector2(670f, 480f);
                Vector2 vector2_4 = vector2_2 / 2f - vector2_3 / 2f;
                int num1 = 20;
                IngameOptions._GUIHover = new Rectangle((int)((double)vector2_4.X - (double)num1), (int)((double)vector2_4.Y - (double)num1), (int)((double)vector2_3.X + (double)(num1 * 2)), (int)((double)vector2_3.Y + (double)(num1 * 2)));
                Utils.DrawInvBG(sb, vector2_4.X - (float)num1, vector2_4.Y - (float)num1, vector2_3.X + (float)(num1 * 2), vector2_3.Y + (float)(num1 * 2), new Color(33, 15, 91, (int)byte.MaxValue) * 0.685f);
                if (new Rectangle((int)vector2_4.X - num1, (int)vector2_4.Y - num1, (int)vector2_3.X + num1 * 2, (int)vector2_3.Y + num1 * 2).Contains(new Point(Main.mouseX, Main.mouseY)))
                    Main.player[Main.myPlayer].mouseInterface = true;
                Utils.DrawInvBG(sb, vector2_4.X + (float)(num1 / 2), vector2_4.Y + (float)(num1 * 5 / 2), vector2_3.X / 2f - (float)num1, vector2_3.Y - (float)(num1 * 3), new Color());
                Utils.DrawInvBG(sb, vector2_4.X + vector2_3.X / 2f + (float)num1, vector2_4.Y + (float)(num1 * 5 / 2), vector2_3.X / 2f - (float)(num1 * 3 / 2), vector2_3.Y - (float)(num1 * 3), new Color());
                Utils.DrawBorderString(sb, "Settings Menu", vector2_4 + vector2_3 * new Vector2(0.5f, 0.0f), Color.White, 1f, 0.5f, 0.0f, -1);
                float num2 = 0.7f;
                float scale = 0.8f;
                float num3 = 0.01f;
                if (IngameOptions.oldLeftHover != IngameOptions.leftHover && IngameOptions.leftHover != -1)
                    Main.PlaySound(12, -1, -1, 1);
                if (IngameOptions.oldRightHover != IngameOptions.rightHover && IngameOptions.rightHover != -1)
                    Main.PlaySound(12, -1, -1, 1);
                if (flag1 && IngameOptions.rightHover != -1 && !IngameOptions.noSound)
                    Main.PlaySound(12, -1, -1, 1);
                IngameOptions.oldLeftHover = IngameOptions.leftHover;
                IngameOptions.oldRightHover = IngameOptions.rightHover;
                IngameOptions.noSound = false;
                bool flag2 = false;
                int num4 = flag2 ? 1 : 0;
                int num5 = 6 + num4;
                Vector2 anchor1 = new Vector2(vector2_4.X + vector2_3.X / 4f, vector2_4.Y + (float)(num1 * 5 / 2));
                Vector2 offset1 = new Vector2(0.0f, vector2_3.Y - (float)(num1 * 5)) / (float)(num5 + 1);
                for (int index = 0; index <= num5; ++index)
                {
                    if (IngameOptions.leftHover == index || index == IngameOptions.category)
                        IngameOptions.leftScale[index] += num3;
                    else
                        IngameOptions.leftScale[index] -= num3;
                    if ((double)IngameOptions.leftScale[index] < (double)num2)
                        IngameOptions.leftScale[index] = num2;
                    if ((double)IngameOptions.leftScale[index] > (double)scale)
                        IngameOptions.leftScale[index] = scale;
                }
                IngameOptions.leftHover = -1;
                int num6 = IngameOptions.category;
                if (IngameOptions.DrawLeftSide(sb, Lang.menu[114], 0, anchor1, offset1, IngameOptions.leftScale, 0.7f, 0.8f, 0.01f))
                {
                    IngameOptions.leftHover = 0;
                    if (flag1)
                    {
                        IngameOptions.category = 0;
                        Main.PlaySound(10, -1, -1, 1);
                    }
                }
                if (IngameOptions.DrawLeftSide(sb, Lang.menu[63], 1, anchor1, offset1, IngameOptions.leftScale, 0.7f, 0.8f, 0.01f))
                {
                    IngameOptions.leftHover = 1;
                    if (flag1)
                    {
                        IngameOptions.category = 1;
                        Main.PlaySound(10, -1, -1, 1);
                    }
                }
                if (IngameOptions.DrawLeftSide(sb, Lang.menu[66], 2, anchor1, offset1, IngameOptions.leftScale, 0.7f, 0.8f, 0.01f))
                {
                    IngameOptions.leftHover = 2;
                    if (flag1)
                    {
                        IngameOptions.category = 2;
                        Main.PlaySound(10, -1, -1, 1);
                    }
                }
                if (IngameOptions.DrawLeftSide(sb, Lang.menu[115], 3, anchor1, offset1, IngameOptions.leftScale, 0.7f, 0.8f, 0.01f))
                {
                    IngameOptions.leftHover = 3;
                    if (flag1)
                    {
                        IngameOptions.category = 3;
                        Main.PlaySound(10, -1, -1, 1);
                    }
                }
                if (flag2 && IngameOptions.DrawLeftSide(sb, Lang.menu[141], 4, anchor1, offset1, IngameOptions.leftScale, 0.7f, 0.8f, 0.01f))
                {
                    IngameOptions.leftHover = 4;
                    if (flag1)
                    {
                        IngameOptions.Close();
                    }
                }
                if (IngameOptions.DrawLeftSide(sb, Lang.menu[131], 4 + num4, anchor1, offset1, IngameOptions.leftScale, 0.7f, 0.8f, 0.01f))
                {
                    IngameOptions.leftHover = 4 + num4;
                    if (flag1)
                    {
                        IngameOptions.Close();
                        AchievementsUI.Open();
                    }
                }
                if (IngameOptions.DrawLeftSide(sb, Lang.menu[118], 5 + num4, anchor1, offset1, IngameOptions.leftScale, 0.7f, 0.8f, 0.01f))
                {
                    IngameOptions.leftHover = 5 + num4;
                    if (flag1)
                        IngameOptions.Close();
                }
                if (IngameOptions.DrawLeftSide(sb, Lang.inter[35], 6 + num4, anchor1, offset1, IngameOptions.leftScale, 0.7f, 0.8f, 0.01f))
                {
                    IngameOptions.leftHover = 6 + num4;
                    if (flag1)
                    {
                        IngameOptions.Close();
                        Main.menuMode = 10;
                        WorldGen.SaveAndQuit((Action)null);
                    }
                }
                if (num6 != IngameOptions.category)
                {
                    for (int index = 0; index < IngameOptions.rightScale.Length; ++index)
                        IngameOptions.rightScale[index] = 0.0f;
                }
                int num7 = 0;
                switch (IngameOptions.category)
                {
                    case 0:
                        num7 = 11;
                        num2 = 1f;
                        scale = 1.001f;
                        num3 = 1.0f / 1000.0f;
                        break;
                    case 1:
                        num7 = 8;
                        num2 = 1f;
                        scale = 1.001f;
                        num3 = 1.0f / 1000.0f;
                        break;
                    case 2:
                        num7 = 14;
                        num2 = 0.8f;
                        scale = 0.801f;
                        num3 = 1.0f / 1000.0f;
                        break;
                    case 3:
                        num7 = 7;
                        num2 = 0.8f;
                        scale = 0.801f;
                        num3 = 1.0f / 1000.0f;
                        break;
                }
                Vector2 anchor2 = new Vector2(vector2_4.X + (float)((double)vector2_3.X * 3.0 / 4.0), vector2_4.Y + (float)(num1 * 5 / 2));
                Vector2 offset2 = new Vector2(0.0f, vector2_3.Y - (float)(num1 * 3)) / (float)(num7 + 1);
                if (IngameOptions.category == 2)
                    offset2.Y -= 2f;
                for (int index = 0; index < 15; ++index)
                {
                    if (IngameOptions.rightLock == index || IngameOptions.rightHover == index && IngameOptions.rightLock == -1)
                        IngameOptions.rightScale[index] += num3;
                    else
                        IngameOptions.rightScale[index] -= num3;
                    if ((double)IngameOptions.rightScale[index] < (double)num2)
                        IngameOptions.rightScale[index] = num2;
                    if ((double)IngameOptions.rightScale[index] > (double)scale)
                        IngameOptions.rightScale[index] = scale;
                }
                IngameOptions.inBar = false;
                IngameOptions.rightHover = -1;
                if (!Main.mouseLeft)
                    IngameOptions.rightLock = -1;
                if (IngameOptions.rightLock == -1)
                    IngameOptions.notBar = false;
                if (IngameOptions.category == 0)
                {
                    int i1 = 0;
                    anchor2.X -= 70f;
                    if (IngameOptions.DrawRightSide(sb, string.Concat(new object[4]
          {
            (object) Lang.menu[99],
            (object) " ",
            (object) Math.Round((double) Main.musicVolume * 100.0),
            (object) "%"
          }), i1, anchor2, offset2, IngameOptions.rightScale[i1], (float)(((double)IngameOptions.rightScale[i1] - (double)num2) / ((double)scale - (double)num2)), new Color()))
                    {
                        if (IngameOptions.rightLock == -1)
                            IngameOptions.notBar = true;
                        IngameOptions.noSound = true;
                        IngameOptions.rightHover = i1;
                    }
                    IngameOptions.valuePosition.X = (float)((double)vector2_4.X + (double)vector2_3.X - (double)(num1 / 2) - 20.0);
                    IngameOptions.valuePosition.Y -= 3f;
                    float num8 = IngameOptions.DrawValueBar(sb, 0.75f, Main.musicVolume);
                    if ((IngameOptions.inBar || IngameOptions.rightLock == i1) && !IngameOptions.notBar)
                    {
                        IngameOptions.rightHover = i1;
                        if (Main.mouseLeft && IngameOptions.rightLock == i1)
                            Main.musicVolume = num8;
                    }
                    if ((double)Main.mouseX > (double)vector2_4.X + (double)vector2_3.X * 2.0 / 3.0 + (double)num1 && (double)Main.mouseX < (double)IngameOptions.valuePosition.X + 3.75 && ((double)Main.mouseY > (double)IngameOptions.valuePosition.Y - 10.0 && (double)Main.mouseY <= (double)IngameOptions.valuePosition.Y + 10.0))
                    {
                        if (IngameOptions.rightLock == -1)
                            IngameOptions.notBar = true;
                        IngameOptions.rightHover = i1;
                    }
                    int i2 = i1 + 1;
                    if (IngameOptions.DrawRightSide(sb, string.Concat(new object[4]
          {
            (object) Lang.menu[98],
            (object) " ",
            (object) Math.Round((double) Main.soundVolume * 100.0),
            (object) "%"
          }), i2, anchor2, offset2, IngameOptions.rightScale[i2], (float)(((double)IngameOptions.rightScale[i2] - (double)num2) / ((double)scale - (double)num2)), new Color()))
                    {
                        if (IngameOptions.rightLock == -1)
                            IngameOptions.notBar = true;
                        IngameOptions.rightHover = i2;
                    }
                    IngameOptions.valuePosition.X = (float)((double)vector2_4.X + (double)vector2_3.X - (double)(num1 / 2) - 20.0);
                    IngameOptions.valuePosition.Y -= 3f;
                    float num9 = IngameOptions.DrawValueBar(sb, 0.75f, Main.soundVolume);
                    if ((IngameOptions.inBar || IngameOptions.rightLock == i2) && !IngameOptions.notBar)
                    {
                        IngameOptions.rightHover = i2;
                        if (Main.mouseLeft && IngameOptions.rightLock == i2)
                        {
                            Main.soundVolume = num9;
                            IngameOptions.noSound = true;
                        }
                    }
                    if ((double)Main.mouseX > (double)vector2_4.X + (double)vector2_3.X * 2.0 / 3.0 + (double)num1 && (double)Main.mouseX < (double)IngameOptions.valuePosition.X + 3.75 && ((double)Main.mouseY > (double)IngameOptions.valuePosition.Y - 10.0 && (double)Main.mouseY <= (double)IngameOptions.valuePosition.Y + 10.0))
                    {
                        if (IngameOptions.rightLock == -1)
                            IngameOptions.notBar = true;
                        IngameOptions.rightHover = i2;
                    }
                    int i3 = i2 + 1;
                    if (IngameOptions.DrawRightSide(sb, string.Concat(new object[4]
          {
            (object) Lang.menu[119],
            (object) " ",
            (object) Math.Round((double) Main.ambientVolume * 100.0),
            (object) "%"
          }), i3, anchor2, offset2, IngameOptions.rightScale[i3], (float)(((double)IngameOptions.rightScale[i3] - (double)num2) / ((double)scale - (double)num2)), new Color()))
                    {
                        if (IngameOptions.rightLock == -1)
                            IngameOptions.notBar = true;
                        IngameOptions.rightHover = i3;
                    }
                    IngameOptions.valuePosition.X = (float)((double)vector2_4.X + (double)vector2_3.X - (double)(num1 / 2) - 20.0);
                    IngameOptions.valuePosition.Y -= 3f;
                    float num10 = IngameOptions.DrawValueBar(sb, 0.75f, Main.ambientVolume);
                    if ((IngameOptions.inBar || IngameOptions.rightLock == i3) && !IngameOptions.notBar)
                    {
                        IngameOptions.rightHover = i3;
                        if (Main.mouseLeft && IngameOptions.rightLock == i3)
                        {
                            Main.ambientVolume = num10;
                            IngameOptions.noSound = true;
                        }
                    }
                    if ((double)Main.mouseX > (double)vector2_4.X + (double)vector2_3.X * 2.0 / 3.0 + (double)num1 && (double)Main.mouseX < (double)IngameOptions.valuePosition.X + 3.75 && ((double)Main.mouseY > (double)IngameOptions.valuePosition.Y - 10.0 && (double)Main.mouseY <= (double)IngameOptions.valuePosition.Y + 10.0))
                    {
                        if (IngameOptions.rightLock == -1)
                            IngameOptions.notBar = true;
                        IngameOptions.rightHover = i3;
                    }
                    int i4 = i3 + 1;
                    anchor2.X += 70f;
                    if (IngameOptions.DrawRightSide(sb, Main.autoSave ? Lang.menu[67] : Lang.menu[68], i4, anchor2, offset2, IngameOptions.rightScale[i4], (float)(((double)IngameOptions.rightScale[i4] - (double)num2) / ((double)scale - (double)num2)), new Color()))
                    {
                        IngameOptions.rightHover = i4;
                        if (flag1)
                            Main.autoSave = !Main.autoSave;
                    }
                    int i5 = i4 + 1;
                    if (IngameOptions.DrawRightSide(sb, Main.autoPause ? Lang.menu[69] : Lang.menu[70], i5, anchor2, offset2, IngameOptions.rightScale[i5], (float)(((double)IngameOptions.rightScale[i5] - (double)num2) / ((double)scale - (double)num2)), new Color()))
                    {
                        IngameOptions.rightHover = i5;
                        if (flag1)
                            Main.autoPause = !Main.autoPause;
                    }
                    int i6 = i5 + 1;
                    if (IngameOptions.DrawRightSide(sb, Main.showItemText ? Lang.menu[71] : Lang.menu[72], i6, anchor2, offset2, IngameOptions.rightScale[i6], (float)(((double)IngameOptions.rightScale[i6] - (double)num2) / ((double)scale - (double)num2)), new Color()))
                    {
                        IngameOptions.rightHover = i6;
                        if (flag1)
                            Main.showItemText = !Main.showItemText;
                    }
                    int i7 = i6 + 1;
                    if (IngameOptions.DrawRightSide(sb, Main.cSmartToggle ? Lang.menu[121] : Lang.menu[122], i7, anchor2, offset2, IngameOptions.rightScale[i7], (float)(((double)IngameOptions.rightScale[i7] - (double)num2) / ((double)scale - (double)num2)), new Color()))
                    {
                        IngameOptions.rightHover = i7;
                        if (flag1)
                            Main.cSmartToggle = !Main.cSmartToggle;
                    }
                    int i8 = i7 + 1;
                    if (IngameOptions.DrawRightSide(sb, Lang.menu[123] + " " + Lang.menu[124 + Main.invasionProgressMode], i8, anchor2, offset2, IngameOptions.rightScale[i8], (float)(((double)IngameOptions.rightScale[i8] - (double)num2) / ((double)scale - (double)num2)), new Color()))
                    {
                        IngameOptions.rightHover = i8;
                        if (flag1)
                        {
                            ++Main.invasionProgressMode;
                            if (Main.invasionProgressMode >= 3)
                                Main.invasionProgressMode = 0;
                        }
                    }
                    int i9 = i8 + 1;
                    if (IngameOptions.DrawRightSide(sb, Main.placementPreview ? Lang.menu[128] : Lang.menu[129], i9, anchor2, offset2, IngameOptions.rightScale[i9], (float)(((double)IngameOptions.rightScale[i9] - (double)num2) / ((double)scale - (double)num2)), new Color()))
                    {
                        IngameOptions.rightHover = i9;
                        if (flag1)
                            Main.placementPreview = !Main.placementPreview;
                    }
                    int i10 = i9 + 1;
                    if (IngameOptions.DrawRightSide(sb, ChildSafety.Disabled ? Lang.menu[132] : Lang.menu[133], i10, anchor2, offset2, IngameOptions.rightScale[i10], (float)(((double)IngameOptions.rightScale[i10] - (double)num2) / ((double)scale - (double)num2)), new Color()))
                    {
                        IngameOptions.rightHover = i10;
                        if (flag1)
                            ChildSafety.Disabled = !ChildSafety.Disabled;
                    }
                    int i11 = i10 + 1;
                    if (IngameOptions.DrawRightSide(sb, ItemSlot.Options.HighlightNewItems ? Lang.inter[117] : Lang.inter[116], i11, anchor2, offset2, IngameOptions.rightScale[i11], (float)(((double)IngameOptions.rightScale[i11] - (double)num2) / ((double)scale - (double)num2)), new Color()))
                    {
                        IngameOptions.rightHover = i11;
                        if (flag1)
                            ItemSlot.Options.HighlightNewItems = !ItemSlot.Options.HighlightNewItems;
                    }
                    int num11 = i11 + 1;
                }
                if (IngameOptions.category == 1)
                {
                    int i1 = 0;
                    if (IngameOptions.DrawRightSide(sb, Main.graphics.IsFullScreen ? Lang.menu[49] : Lang.menu[50], i1, anchor2, offset2, IngameOptions.rightScale[i1], (float)(((double)IngameOptions.rightScale[i1] - (double)num2) / ((double)scale - (double)num2)), new Color()))
                    {
                        IngameOptions.rightHover = i1;
                        if (flag1)
                            Main.ToggleFullScreen();
                    }
                    int i2 = i1 + 1;
                    if (IngameOptions.DrawRightSide(sb, Lang.menu[51] + (object)": " + (string)(object)Main.PendingResolutionWidth + "x" + (string)(object)Main.PendingResolutionHeight, i2, anchor2, offset2, IngameOptions.rightScale[i2], (float)(((double)IngameOptions.rightScale[i2] - (double)num2) / ((double)scale - (double)num2)), new Color()))
                    {
                        IngameOptions.rightHover = i2;
                        if (flag1)
                        {
                            int num8 = 0;
                            for (int index = 0; index < Main.numDisplayModes; ++index)
                            {
                                if (Main.displayWidth[index] == Main.PendingResolutionWidth && Main.displayHeight[index] == Main.PendingResolutionHeight)
                                {
                                    num8 = index;
                                    break;
                                }
                            }
                            int index1 = num8 + 1;
                            if (index1 >= Main.numDisplayModes)
                                index1 = 0;
                            Main.PendingResolutionWidth = Main.displayWidth[index1];
                            Main.PendingResolutionHeight = Main.displayHeight[index1];
                        }
                    }
                    int i3 = i2 + 1;
                    anchor2.X -= 70f;
                    if (IngameOptions.DrawRightSide(sb, string.Concat(new object[4]
          {
            (object) Lang.menu[52],
            (object) ": ",
            (object) Main.bgScroll,
            (object) "%"
          }), i3, anchor2, offset2, IngameOptions.rightScale[i3], (float)(((double)IngameOptions.rightScale[i3] - (double)num2) / ((double)scale - (double)num2)), new Color()))
                    {
                        if (IngameOptions.rightLock == -1)
                            IngameOptions.notBar = true;
                        IngameOptions.noSound = true;
                        IngameOptions.rightHover = i3;
                    }
                    IngameOptions.valuePosition.X = (float)((double)vector2_4.X + (double)vector2_3.X - (double)(num1 / 2) - 20.0);
                    IngameOptions.valuePosition.Y -= 3f;
                    float num9 = IngameOptions.DrawValueBar(sb, 0.75f, (float)Main.bgScroll / 100f);
                    if ((IngameOptions.inBar || IngameOptions.rightLock == i3) && !IngameOptions.notBar)
                    {
                        IngameOptions.rightHover = i3;
                        if (Main.mouseLeft && IngameOptions.rightLock == i3)
                        {
                            Main.bgScroll = (int)((double)num9 * 100.0);
                            Main.caveParallax = (float)(1.0 - (double)Main.bgScroll / 500.0);
                        }
                    }
                    if ((double)Main.mouseX > (double)vector2_4.X + (double)vector2_3.X * 2.0 / 3.0 + (double)num1 && (double)Main.mouseX < (double)IngameOptions.valuePosition.X + 3.75 && ((double)Main.mouseY > (double)IngameOptions.valuePosition.Y - 10.0 && (double)Main.mouseY <= (double)IngameOptions.valuePosition.Y + 10.0))
                    {
                        if (IngameOptions.rightLock == -1)
                            IngameOptions.notBar = true;
                        IngameOptions.rightHover = i3;
                    }
                    int i4 = i3 + 1;
                    anchor2.X += 70f;
                    if (IngameOptions.DrawRightSide(sb, Main.terrariasFixedTiming ? Lang.menu[53] : Lang.menu[54], i4, anchor2, offset2, IngameOptions.rightScale[i4], (float)(((double)IngameOptions.rightScale[i4] - (double)num2) / ((double)scale - (double)num2)), new Color()))
                    {
                        IngameOptions.rightHover = i4;
                        if (flag1)
                            Main.terrariasFixedTiming = !Main.terrariasFixedTiming;
                    }
                    int i5 = i4 + 1;
                    if (IngameOptions.DrawRightSide(sb, Lang.menu[55 + Lighting.lightMode], i5, anchor2, offset2, IngameOptions.rightScale[i5], (float)(((double)IngameOptions.rightScale[i5] - (double)num2) / ((double)scale - (double)num2)), new Color()))
                    {
                        IngameOptions.rightHover = i5;
                        if (flag1)
                            Lighting.NextLightMode();
                    }
                    int i6 = i5 + 1;
                    if (IngameOptions.DrawRightSide(sb, Lang.menu[116] + " " + (Lighting.LightingThreads > 0 ? string.Concat((object)(Lighting.LightingThreads + 1)) : Lang.menu[117]), i6, anchor2, offset2, IngameOptions.rightScale[i6], (float)(((double)IngameOptions.rightScale[i6] - (double)num2) / ((double)scale - (double)num2)), new Color()))
                    {
                        IngameOptions.rightHover = i6;
                        if (flag1)
                        {
                            ++Lighting.LightingThreads;
                            if (Lighting.LightingThreads > Environment.ProcessorCount - 1)
                                Lighting.LightingThreads = 0;
                        }
                    }
                    int i7 = i6 + 1;
                    if (IngameOptions.DrawRightSide(sb, Lang.menu[59 + Main.qaStyle], i7, anchor2, offset2, IngameOptions.rightScale[i7], (float)(((double)IngameOptions.rightScale[i7] - (double)num2) / ((double)scale - (double)num2)), new Color()))
                    {
                        IngameOptions.rightHover = i7;
                        if (flag1)
                        {
                            ++Main.qaStyle;
                            if (Main.qaStyle > 3)
                                Main.qaStyle = 0;
                        }
                    }
                    int i8 = i7 + 1;
                    if (IngameOptions.DrawRightSide(sb, Main.owBack ? Lang.menu[100] : Lang.menu[101], i8, anchor2, offset2, IngameOptions.rightScale[i8], (float)(((double)IngameOptions.rightScale[i8] - (double)num2) / ((double)scale - (double)num2)), new Color()))
                    {
                        IngameOptions.rightHover = i8;
                        if (flag1)
                            Main.owBack = !Main.owBack;
                    }
                    int num10 = i8 + 1;
                }
                if (IngameOptions.category == 2)
                {
                    int i1 = 0;
                    anchor2.X -= 30f;
                    int num8 = 0;
                    if (IngameOptions.DrawRightSide(sb, Lang.menu[74 + num8], i1, anchor2, offset2, IngameOptions.rightScale[i1], (float)(((double)IngameOptions.rightScale[i1] - (double)num2) / ((double)scale - (double)num2)), Main.setKey == num8 ? Color.Gold : new Color()))
                    {
                        IngameOptions.rightHover = i1;
                        if (flag1)
                            Main.setKey = num8;
                    }
                    IngameOptions.valuePosition.X += 10f;
                    if (IngameOptions.DrawValue(sb, Main.setKey == num8 ? "_" : Main.cUp, i1, scale, Main.setKey == num8 ? Color.Gold : (IngameOptions.rightHover == i1 ? Color.White : new Color())))
                    {
                        IngameOptions.rightHover = i1;
                        if (flag1)
                            Main.setKey = num8;
                    }
                    int i2 = i1 + 1;
                    int num9 = 1;
                    if (IngameOptions.DrawRightSide(sb, Lang.menu[74 + num9], i2, anchor2, offset2, IngameOptions.rightScale[i2], (float)(((double)IngameOptions.rightScale[i2] - (double)num2) / ((double)scale - (double)num2)), Main.setKey == num9 ? Color.Gold : new Color()))
                    {
                        IngameOptions.rightHover = i2;
                        if (flag1)
                            Main.setKey = num9;
                    }
                    IngameOptions.valuePosition.X += 10f;
                    if (IngameOptions.DrawValue(sb, Main.setKey == num9 ? "_" : Main.cDown, i2, scale, Main.setKey == num9 ? Color.Gold : (IngameOptions.rightHover == i2 ? Color.White : new Color())))
                    {
                        IngameOptions.rightHover = i2;
                        if (flag1)
                            Main.setKey = num9;
                    }
                    int i3 = i2 + 1;
                    int num10 = 2;
                    if (IngameOptions.DrawRightSide(sb, Lang.menu[74 + num10], i3, anchor2, offset2, IngameOptions.rightScale[i3], (float)(((double)IngameOptions.rightScale[i3] - (double)num2) / ((double)scale - (double)num2)), Main.setKey == num10 ? Color.Gold : new Color()))
                    {
                        IngameOptions.rightHover = i3;
                        if (flag1)
                            Main.setKey = num10;
                    }
                    IngameOptions.valuePosition.X += 10f;
                    if (IngameOptions.DrawValue(sb, Main.setKey == num10 ? "_" : Main.cLeft, i3, scale, Main.setKey == num10 ? Color.Gold : (IngameOptions.rightHover == i3 ? Color.White : new Color())))
                    {
                        IngameOptions.rightHover = i3;
                        if (flag1)
                            Main.setKey = num10;
                    }
                    int i4 = i3 + 1;
                    int num11 = 3;
                    if (IngameOptions.DrawRightSide(sb, Lang.menu[74 + num11], i4, anchor2, offset2, IngameOptions.rightScale[i4], (float)(((double)IngameOptions.rightScale[i4] - (double)num2) / ((double)scale - (double)num2)), Main.setKey == num11 ? Color.Gold : new Color()))
                    {
                        IngameOptions.rightHover = i4;
                        if (flag1)
                            Main.setKey = num11;
                    }
                    IngameOptions.valuePosition.X += 10f;
                    if (IngameOptions.DrawValue(sb, Main.setKey == num11 ? "_" : Main.cRight, i4, scale, Main.setKey == num11 ? Color.Gold : (IngameOptions.rightHover == i4 ? Color.White : new Color())))
                    {
                        IngameOptions.rightHover = i4;
                        if (flag1)
                            Main.setKey = num11;
                    }
                    int i5 = i4 + 1;
                    int num12 = 4;
                    if (IngameOptions.DrawRightSide(sb, Lang.menu[74 + num12], i5, anchor2, offset2, IngameOptions.rightScale[i5], (float)(((double)IngameOptions.rightScale[i5] - (double)num2) / ((double)scale - (double)num2)), Main.setKey == num12 ? Color.Gold : new Color()))
                    {
                        IngameOptions.rightHover = i5;
                        if (flag1)
                            Main.setKey = num12;
                    }
                    IngameOptions.valuePosition.X += 10f;
                    if (IngameOptions.DrawValue(sb, Main.setKey == num12 ? "_" : Main.cJump, i5, scale, Main.setKey == num12 ? Color.Gold : (IngameOptions.rightHover == i5 ? Color.White : new Color())))
                    {
                        IngameOptions.rightHover = i5;
                        if (flag1)
                            Main.setKey = num12;
                    }
                    int i6 = i5 + 1;
                    int num13 = 5;
                    if (IngameOptions.DrawRightSide(sb, Lang.menu[74 + num13], i6, anchor2, offset2, IngameOptions.rightScale[i6], (float)(((double)IngameOptions.rightScale[i6] - (double)num2) / ((double)scale - (double)num2)), Main.setKey == num13 ? Color.Gold : new Color()))
                    {
                        IngameOptions.rightHover = i6;
                        if (flag1)
                            Main.setKey = num13;
                    }
                    IngameOptions.valuePosition.X += 10f;
                    if (IngameOptions.DrawValue(sb, Main.setKey == num13 ? "_" : Main.cThrowItem, i6, scale, Main.setKey == num13 ? Color.Gold : (IngameOptions.rightHover == i6 ? Color.White : new Color())))
                    {
                        IngameOptions.rightHover = i6;
                        if (flag1)
                            Main.setKey = num13;
                    }
                    int i7 = i6 + 1;
                    int num14 = 6;
                    if (IngameOptions.DrawRightSide(sb, Lang.menu[74 + num14], i7, anchor2, offset2, IngameOptions.rightScale[i7], (float)(((double)IngameOptions.rightScale[i7] - (double)num2) / ((double)scale - (double)num2)), Main.setKey == num14 ? Color.Gold : new Color()))
                    {
                        IngameOptions.rightHover = i7;
                        if (flag1)
                            Main.setKey = num14;
                    }
                    IngameOptions.valuePosition.X += 10f;
                    if (IngameOptions.DrawValue(sb, Main.setKey == num14 ? "_" : Main.cInv, i7, scale, Main.setKey == num14 ? Color.Gold : (IngameOptions.rightHover == i7 ? Color.White : new Color())))
                    {
                        IngameOptions.rightHover = i7;
                        if (flag1)
                            Main.setKey = num14;
                    }
                    int i8 = i7 + 1;
                    int num15 = 7;
                    if (IngameOptions.DrawRightSide(sb, Lang.menu[74 + num15], i8, anchor2, offset2, IngameOptions.rightScale[i8], (float)(((double)IngameOptions.rightScale[i8] - (double)num2) / ((double)scale - (double)num2)), Main.setKey == num15 ? Color.Gold : new Color()))
                    {
                        IngameOptions.rightHover = i8;
                        if (flag1)
                            Main.setKey = num15;
                    }
                    IngameOptions.valuePosition.X += 10f;
                    if (IngameOptions.DrawValue(sb, Main.setKey == num15 ? "_" : Main.cHeal, i8, scale, Main.setKey == num15 ? Color.Gold : (IngameOptions.rightHover == i8 ? Color.White : new Color())))
                    {
                        IngameOptions.rightHover = i8;
                        if (flag1)
                            Main.setKey = num15;
                    }
                    int i9 = i8 + 1;
                    int num16 = 8;
                    if (IngameOptions.DrawRightSide(sb, Lang.menu[74 + num16], i9, anchor2, offset2, IngameOptions.rightScale[i9], (float)(((double)IngameOptions.rightScale[i9] - (double)num2) / ((double)scale - (double)num2)), Main.setKey == num16 ? Color.Gold : new Color()))
                    {
                        IngameOptions.rightHover = i9;
                        if (flag1)
                            Main.setKey = num16;
                    }
                    IngameOptions.valuePosition.X += 10f;
                    if (IngameOptions.DrawValue(sb, Main.setKey == num16 ? "_" : Main.cMana, i9, scale, Main.setKey == num16 ? Color.Gold : (IngameOptions.rightHover == i9 ? Color.White : new Color())))
                    {
                        IngameOptions.rightHover = i9;
                        if (flag1)
                            Main.setKey = num16;
                    }
                    int i10 = i9 + 1;
                    int num17 = 9;
                    if (IngameOptions.DrawRightSide(sb, Lang.menu[74 + num17], i10, anchor2, offset2, IngameOptions.rightScale[i10], (float)(((double)IngameOptions.rightScale[i10] - (double)num2) / ((double)scale - (double)num2)), Main.setKey == num17 ? Color.Gold : new Color()))
                    {
                        IngameOptions.rightHover = i10;
                        if (flag1)
                            Main.setKey = num17;
                    }
                    IngameOptions.valuePosition.X += 10f;
                    if (IngameOptions.DrawValue(sb, Main.setKey == num17 ? "_" : Main.cBuff, i10, scale, Main.setKey == num17 ? Color.Gold : (IngameOptions.rightHover == i10 ? Color.White : new Color())))
                    {
                        IngameOptions.rightHover = i10;
                        if (flag1)
                            Main.setKey = num17;
                    }
                    int i11 = i10 + 1;
                    int num18 = 10;
                    if (IngameOptions.DrawRightSide(sb, Lang.menu[74 + num18], i11, anchor2, offset2, IngameOptions.rightScale[i11], (float)(((double)IngameOptions.rightScale[i11] - (double)num2) / ((double)scale - (double)num2)), Main.setKey == num18 ? Color.Gold : new Color()))
                    {
                        IngameOptions.rightHover = i11;
                        if (flag1)
                            Main.setKey = num18;
                    }
                    IngameOptions.valuePosition.X += 10f;
                    if (IngameOptions.DrawValue(sb, Main.setKey == num18 ? "_" : Main.cHook, i11, scale, Main.setKey == num18 ? Color.Gold : (IngameOptions.rightHover == i11 ? Color.White : new Color())))
                    {
                        IngameOptions.rightHover = i11;
                        if (flag1)
                            Main.setKey = num18;
                    }
                    int i12 = i11 + 1;
                    int num19 = 11;
                    if (IngameOptions.DrawRightSide(sb, Lang.menu[74 + num19], i12, anchor2, offset2, IngameOptions.rightScale[i12], (float)(((double)IngameOptions.rightScale[i12] - (double)num2) / ((double)scale - (double)num2)), Main.setKey == num19 ? Color.Gold : new Color()))
                    {
                        IngameOptions.rightHover = i12;
                        if (flag1)
                            Main.setKey = num19;
                    }
                    IngameOptions.valuePosition.X += 10f;
                    if (IngameOptions.DrawValue(sb, Main.setKey == num19 ? "_" : Main.cTorch, i12, scale, Main.setKey == num19 ? Color.Gold : (IngameOptions.rightHover == i12 ? Color.White : new Color())))
                    {
                        IngameOptions.rightHover = i12;
                        if (flag1)
                            Main.setKey = num19;
                    }
                    int i13 = i12 + 1;
                    int num20 = 12;
                    if (IngameOptions.DrawRightSide(sb, Lang.menu[120], i13, anchor2, offset2, IngameOptions.rightScale[i13], (float)(((double)IngameOptions.rightScale[i13] - (double)num2) / ((double)scale - (double)num2)), Main.setKey == num20 ? Color.Gold : new Color()))
                    {
                        IngameOptions.rightHover = i13;
                        if (flag1)
                            Main.setKey = num20;
                    }
                    IngameOptions.valuePosition.X += 10f;
                    if (IngameOptions.DrawValue(sb, Main.setKey == num20 ? "_" : Main.cSmart, i13, scale, Main.setKey == num20 ? Color.Gold : (IngameOptions.rightHover == i13 ? Color.White : new Color())))
                    {
                        IngameOptions.rightHover = i13;
                        if (flag1)
                            Main.setKey = num20;
                    }
                    int i14 = i13 + 1;
                    int num21 = 13;
                    if (IngameOptions.DrawRightSide(sb, Lang.menu[130], i14, anchor2, offset2, IngameOptions.rightScale[i14], (float)(((double)IngameOptions.rightScale[i14] - (double)num2) / ((double)scale - (double)num2)), Main.setKey == num21 ? Color.Gold : new Color()))
                    {
                        IngameOptions.rightHover = i14;
                        if (flag1)
                            Main.setKey = num21;
                    }
                    IngameOptions.valuePosition.X += 10f;
                    if (IngameOptions.DrawValue(sb, Main.setKey == num21 ? "_" : Main.cMount, i14, scale, Main.setKey == num21 ? Color.Gold : (IngameOptions.rightHover == i14 ? Color.White : new Color())))
                    {
                        IngameOptions.rightHover = i14;
                        if (flag1)
                            Main.setKey = num21;
                    }
                    int i15 = i14 + 1;
                    anchor2.X += 30f;
                    if (IngameOptions.DrawRightSide(sb, Lang.menu[86], i15, anchor2, offset2, IngameOptions.rightScale[i15], (float)(((double)IngameOptions.rightScale[i15] - (double)num2) / ((double)scale - (double)num2)), new Color()))
                    {
                        IngameOptions.rightHover = i15;
                        if (flag1)
                        {
                            Main.ResetKeyBindings();
                            Main.setKey = -1;
                        }
                    }
                    int num22 = i15 + 1;
                    if (Main.setKey >= 0)
                    {
                        Main.blockInput = true;
                        Keys[] pressedKeys = Main.keyState.GetPressedKeys();
                        if (pressedKeys.Length > 0)
                        {
                            string str = string.Concat((object)pressedKeys[0]);
                            if (str != "None")
                            {
                                if (Main.setKey == 0)
                                    Main.cUp = str;
                                if (Main.setKey == 1)
                                    Main.cDown = str;
                                if (Main.setKey == 2)
                                    Main.cLeft = str;
                                if (Main.setKey == 3)
                                    Main.cRight = str;
                                if (Main.setKey == 4)
                                    Main.cJump = str;
                                if (Main.setKey == 5)
                                    Main.cThrowItem = str;
                                if (Main.setKey == 6)
                                    Main.cInv = str;
                                if (Main.setKey == 7)
                                    Main.cHeal = str;
                                if (Main.setKey == 8)
                                    Main.cMana = str;
                                if (Main.setKey == 9)
                                    Main.cBuff = str;
                                if (Main.setKey == 10)
                                    Main.cHook = str;
                                if (Main.setKey == 11)
                                    Main.cTorch = str;
                                if (Main.setKey == 12)
                                    Main.cSmart = str;
                                if (Main.setKey == 13)
                                    Main.cMount = str;
                                Main.blockKey = pressedKeys[0];
                                Main.blockInput = false;
                                Main.setKey = -1;
                            }
                        }
                    }
                }
                if (IngameOptions.category == 3)
                {
                    int i1 = 0;
                    anchor2.X -= 30f;
                    int num8 = 0;
                    if (IngameOptions.DrawRightSide(sb, Lang.menu[106 + num8], i1, anchor2, offset2, IngameOptions.rightScale[i1], (float)(((double)IngameOptions.rightScale[i1] - (double)num2) / ((double)scale - (double)num2)), Main.setKey == num8 ? Color.Gold : new Color()))
                    {
                        IngameOptions.rightHover = i1;
                        if (flag1)
                            Main.setKey = num8;
                    }
                    IngameOptions.valuePosition.X += 10f;
                    if (IngameOptions.DrawValue(sb, Main.setKey == num8 ? "_" : Main.cMapStyle, i1, scale, Main.setKey == num8 ? Color.Gold : (IngameOptions.rightHover == i1 ? Color.White : new Color())))
                    {
                        IngameOptions.rightHover = i1;
                        if (flag1)
                            Main.setKey = num8;
                    }
                    int i2 = i1 + 1;
                    int num9 = 1;
                    if (IngameOptions.DrawRightSide(sb, Lang.menu[106 + num9], i2, anchor2, offset2, IngameOptions.rightScale[i2], (float)(((double)IngameOptions.rightScale[i2] - (double)num2) / ((double)scale - (double)num2)), Main.setKey == num9 ? Color.Gold : new Color()))
                    {
                        IngameOptions.rightHover = i2;
                        if (flag1)
                            Main.setKey = num9;
                    }
                    IngameOptions.valuePosition.X += 10f;
                    if (IngameOptions.DrawValue(sb, Main.setKey == num9 ? "_" : Main.cMapFull, i2, scale, Main.setKey == num9 ? Color.Gold : (IngameOptions.rightHover == i2 ? Color.White : new Color())))
                    {
                        IngameOptions.rightHover = i2;
                        if (flag1)
                            Main.setKey = num9;
                    }
                    int i3 = i2 + 1;
                    int num10 = 2;
                    if (IngameOptions.DrawRightSide(sb, Lang.menu[106 + num10], i3, anchor2, offset2, IngameOptions.rightScale[i3], (float)(((double)IngameOptions.rightScale[i3] - (double)num2) / ((double)scale - (double)num2)), Main.setKey == num10 ? Color.Gold : new Color()))
                    {
                        IngameOptions.rightHover = i3;
                        if (flag1)
                            Main.setKey = num10;
                    }
                    IngameOptions.valuePosition.X += 10f;
                    if (IngameOptions.DrawValue(sb, Main.setKey == num10 ? "_" : Main.cMapZoomIn, i3, scale, Main.setKey == num10 ? Color.Gold : (IngameOptions.rightHover == i3 ? Color.White : new Color())))
                    {
                        IngameOptions.rightHover = i3;
                        if (flag1)
                            Main.setKey = num10;
                    }
                    int i4 = i3 + 1;
                    int num11 = 3;
                    if (IngameOptions.DrawRightSide(sb, Lang.menu[106 + num11], i4, anchor2, offset2, IngameOptions.rightScale[i4], (float)(((double)IngameOptions.rightScale[i4] - (double)num2) / ((double)scale - (double)num2)), Main.setKey == num11 ? Color.Gold : new Color()))
                    {
                        IngameOptions.rightHover = i4;
                        if (flag1)
                            Main.setKey = num11;
                    }
                    IngameOptions.valuePosition.X += 10f;
                    if (IngameOptions.DrawValue(sb, Main.setKey == num11 ? "_" : Main.cMapZoomOut, i4, scale, Main.setKey == num11 ? Color.Gold : (IngameOptions.rightHover == i4 ? Color.White : new Color())))
                    {
                        IngameOptions.rightHover = i4;
                        if (flag1)
                            Main.setKey = num11;
                    }
                    int i5 = i4 + 1;
                    int num12 = 4;
                    if (IngameOptions.DrawRightSide(sb, Lang.menu[106 + num12], i5, anchor2, offset2, IngameOptions.rightScale[i5], (float)(((double)IngameOptions.rightScale[i5] - (double)num2) / ((double)scale - (double)num2)), Main.setKey == num12 ? Color.Gold : new Color()))
                    {
                        IngameOptions.rightHover = i5;
                        if (flag1)
                            Main.setKey = num12;
                    }
                    IngameOptions.valuePosition.X += 10f;
                    if (IngameOptions.DrawValue(sb, Main.setKey == num12 ? "_" : Main.cMapAlphaUp, i5, scale, Main.setKey == num12 ? Color.Gold : (IngameOptions.rightHover == i5 ? Color.White : new Color())))
                    {
                        IngameOptions.rightHover = i5;
                        if (flag1)
                            Main.setKey = num12;
                    }
                    int i6 = i5 + 1;
                    int num13 = 5;
                    if (IngameOptions.DrawRightSide(sb, Lang.menu[106 + num13], i6, anchor2, offset2, IngameOptions.rightScale[i6], (float)(((double)IngameOptions.rightScale[i6] - (double)num2) / ((double)scale - (double)num2)), Main.setKey == num13 ? Color.Gold : new Color()))
                    {
                        IngameOptions.rightHover = i6;
                        if (flag1)
                            Main.setKey = num13;
                    }
                    IngameOptions.valuePosition.X += 10f;
                    if (IngameOptions.DrawValue(sb, Main.setKey == num13 ? "_" : Main.cMapAlphaDown, i6, scale, Main.setKey == num13 ? Color.Gold : (IngameOptions.rightHover == i6 ? Color.White : new Color())))
                    {
                        IngameOptions.rightHover = i6;
                        if (flag1)
                            Main.setKey = num13;
                    }
                    int i7 = i6 + 1;
                    anchor2.X += 30f;
                    if (IngameOptions.DrawRightSide(sb, Lang.menu[86], i7, anchor2, offset2, IngameOptions.rightScale[i7], (float)(((double)IngameOptions.rightScale[i7] - (double)num2) / ((double)scale - (double)num2)), new Color()))
                    {
                        IngameOptions.rightHover = i7;
                        if (flag1)
                        {
                            Main.cMapStyle = "Tab";
                            Main.cMapFull = "M";
                            Main.cMapZoomIn = "Add";
                            Main.cMapZoomOut = "Subtract";
                            Main.cMapAlphaUp = "PageUp";
                            Main.cMapAlphaDown = "PageDown";
                            Main.setKey = -1;
                        }
                    }
                    int num14 = i7 + 1;
                    if (Main.setKey >= 0)
                    {
                        Main.blockInput = true;
                        Keys[] pressedKeys = Main.keyState.GetPressedKeys();
                        if (pressedKeys.Length > 0)
                        {
                            string str = string.Concat((object)pressedKeys[0]);
                            if (str != "None")
                            {
                                if (Main.setKey == 0)
                                    Main.cMapStyle = str;
                                if (Main.setKey == 1)
                                    Main.cMapFull = str;
                                if (Main.setKey == 2)
                                    Main.cMapZoomIn = str;
                                if (Main.setKey == 3)
                                    Main.cMapZoomOut = str;
                                if (Main.setKey == 4)
                                    Main.cMapAlphaUp = str;
                                if (Main.setKey == 5)
                                    Main.cMapAlphaDown = str;
                                Main.setKey = -1;
                                Main.blockKey = pressedKeys[0];
                                Main.blockInput = false;
                            }
                        }
                    }
                }
                if (IngameOptions.rightHover != -1 && IngameOptions.rightLock == -1)
                    IngameOptions.rightLock = IngameOptions.rightHover;
                Main.mouseText = false;
                Main.instance.GUIBarsDraw();
                Main.instance.DrawMouseOver();
                Main.DrawThickCursor(false);
                sb.Draw(Main.cursorTextures[0], new Vector2((float)(Main.mouseX + 1), (float)(Main.mouseY + 1)), new Rectangle?(), new Color((int)((double)Main.cursorColor.R * 0.200000002980232), (int)((double)Main.cursorColor.G * 0.200000002980232), (int)((double)Main.cursorColor.B * 0.200000002980232), (int)((double)Main.cursorColor.A * 0.5)), 0.0f, new Vector2(), Main.cursorScale * 1.1f, SpriteEffects.None, 0.0f);
                sb.Draw(Main.cursorTextures[0], new Vector2((float)Main.mouseX, (float)Main.mouseY), new Rectangle?(), Main.cursorColor, 0.0f, new Vector2(), Main.cursorScale, SpriteEffects.None, 0.0f);
            }
        }

        public static void MouseOver()
        {
            if (!Main.ingameOptionsWindow || !IngameOptions._GUIHover.Contains(Utils.ToPoint(Main.MouseScreen)))
                return;
            Main.mouseText = true;
        }

        public static bool DrawLeftSide(SpriteBatch sb, string txt, int i, Vector2 anchor, Vector2 offset, float[] scales, float minscale = 0.7f, float maxscale = 0.8f, float scalespeed = 0.01f)
        {
            bool flag = i == IngameOptions.category;
            Color color = Color.Lerp(Color.Gray, Color.White, (float)(((double)scales[i] - (double)minscale) / ((double)maxscale - (double)minscale)));
            if (flag)
                color = Color.Gold;
            Vector2 vector2 = Utils.DrawBorderStringBig(sb, txt, anchor + offset * (float)(1 + i), color, scales[i], 0.5f, 0.5f, -1);
            return new Rectangle((int)anchor.X - (int)vector2.X / 2, (int)anchor.Y + (int)((double)offset.Y * (double)(1 + i)) - (int)vector2.Y / 2, (int)vector2.X, (int)vector2.Y).Contains(new Point(Main.mouseX, Main.mouseY));
        }

        public static bool DrawRightSide(SpriteBatch sb, string txt, int i, Vector2 anchor, Vector2 offset, float scale, float colorScale, Color over = default(Color))
        {
            Color color = Color.Lerp(Color.Gray, Color.White, colorScale);
            if (over != new Color())
                color = over;
            Vector2 vector2 = Utils.DrawBorderString(sb, txt, anchor + offset * (float)(1 + i), color, scale, 0.5f, 0.5f, -1);
            IngameOptions.valuePosition = anchor + offset * (float)(1 + i) + vector2 * new Vector2(0.5f, 0.0f);
            return new Rectangle((int)anchor.X - (int)vector2.X / 2, (int)anchor.Y + (int)((double)offset.Y * (double)(1 + i)) - (int)vector2.Y / 2, (int)vector2.X, (int)vector2.Y).Contains(new Point(Main.mouseX, Main.mouseY));
        }

        public static bool DrawValue(SpriteBatch sb, string txt, int i, float scale, Color over = default(Color))
        {
            Color color = Color.Gray;
            Vector2 vector2 = Main.fontMouseText.MeasureString(txt) * scale;
            bool flag = new Rectangle((int)IngameOptions.valuePosition.X, (int)IngameOptions.valuePosition.Y - (int)vector2.Y / 2, (int)vector2.X, (int)vector2.Y).Contains(new Point(Main.mouseX, Main.mouseY));
            if (flag)
                color = Color.White;
            if (over != new Color())
                color = over;
            Utils.DrawBorderString(sb, txt, IngameOptions.valuePosition, color, scale, 0.0f, 0.5f, -1);
            IngameOptions.valuePosition.X += vector2.X;
            return flag;
        }

        public static float DrawValueBar(SpriteBatch sb, float scale, float perc)
        {
            Texture2D texture = Main.colorBarTexture;
            Vector2 vector2 = new Vector2((float)texture.Width, (float)texture.Height) * scale;
            IngameOptions.valuePosition.X -= (float)(int)vector2.X;
            Rectangle destinationRectangle = new Rectangle((int)IngameOptions.valuePosition.X, (int)IngameOptions.valuePosition.Y - (int)vector2.Y / 2, (int)vector2.X, (int)vector2.Y);
            sb.Draw(texture, destinationRectangle, Color.White);
            int num1 = 167;
            float num2 = (float)destinationRectangle.X + 5f * scale;
            float y = (float)destinationRectangle.Y + 4f * scale;
            for (float num3 = 0.0f; (double)num3 < (double)num1; ++num3)
            {
                float amount = num3 / (float)num1;
                sb.Draw(Main.colorBlipTexture, new Vector2(num2 + num3 * scale, y), new Rectangle?(), Color.Lerp(Color.Black, Color.White, amount), 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
            }
            sb.Draw(Main.colorSliderTexture, new Vector2(num2 + 167f * scale * perc, y + 4f * scale), new Rectangle?(), Color.White, 0.0f, new Vector2(0.5f * (float)Main.colorSliderTexture.Width, 0.5f * (float)Main.colorSliderTexture.Height), scale, SpriteEffects.None, 0.0f);
            destinationRectangle.X = (int)num2;
            destinationRectangle.Y = (int)y;
            bool flag = destinationRectangle.Contains(new Point(Main.mouseX, Main.mouseY));
            if (Main.mouseX >= destinationRectangle.X && Main.mouseX <= destinationRectangle.X + destinationRectangle.Width)
            {
                IngameOptions.inBar = flag;
                return (float)(Main.mouseX - destinationRectangle.X) / (float)destinationRectangle.Width;
            }
            IngameOptions.inBar = false;
            return destinationRectangle.X >= Main.mouseX ? 0.0f : 1f;
        }
    }
}
