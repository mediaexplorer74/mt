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
using GameManager;
using GameManager.GameContent.Achievements;
using GameManager.GameContent.UI.Chat;
using GameManager.ID;
using GameManager.UI.Chat;

namespace GameManager.UI
{
    public class ItemSlot
    {
        private static Item[] singleSlotArray = new Item[1];
        private static bool[] canFavoriteAt = new bool[23];
        private static int dyeSlotCount = 0;
        private static int accSlotCount = 0;

        static ItemSlot()
        {
            ItemSlot.canFavoriteAt[0] = true;
            ItemSlot.canFavoriteAt[1] = true;
            ItemSlot.canFavoriteAt[2] = true;
        }

        public static void Handle(ref Item inv, int context = 0)
        {
            ItemSlot.singleSlotArray[0] = inv;
            ItemSlot.Handle(ItemSlot.singleSlotArray, context, 0);
            inv = ItemSlot.singleSlotArray[0];
            Recipe.FindRecipes();
        }

        public static void Handle(Item[] inv, int context = 0, int slot = 0)
        {
            ItemSlot.OverrideHover(inv, context, slot);
            if (Game1.mouseLeftRelease && Game1.mouseLeft)
            {
                ItemSlot.LeftClick(inv, context, slot);
                Recipe.FindRecipes();
            }
            else
                ItemSlot.RightClick(inv, context, slot);
            ItemSlot.MouseHover(inv, context, slot);
        }

        public static void OverrideHover(Item[] inv, int context = 0, int slot = 0)
        {
            Item obj = inv[slot];
            if (Game1.keyState.IsKeyDown(Keys.LeftShift) && obj.itemId > 0 && (obj.stack > 0 && !inv[slot].favorited))
            {
                switch (context)
                {
                    case 0:
                    case 1:
                    case 2:
                        if (Game1.npcShop > 0 && !obj.favorited)
                        {
                            Game1.cursorOverride = 10;
                            break;
                        }
                        if (Game1.player[Game1.myPlayer].chest != -1)
                        {
                            if (ChestUI.TryPlacingInChest(obj, true))
                            {
                                Game1.cursorOverride = 9;
                                break;
                            }
                            break;
                        }
                        Game1.cursorOverride = 6;
                        break;
                    case 3:
                    case 4:
                        if (Game1.player[Game1.myPlayer].ItemSpace(obj))
                        {
                            Game1.cursorOverride = 8;
                            break;
                        }
                        break;
                    case 5:
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                    case 12:
                    case 16:
                    case 17:
                    case 18:
                    case 19:
                    case 20:
                        if (Game1.player[Game1.myPlayer].ItemSpace(inv[slot]))
                        {
                            Game1.cursorOverride = 7;
                            break;
                        }
                        break;
                }
            }
            if (!Game1.keyState.IsKeyDown(Keys.LeftAlt) || !ItemSlot.canFavoriteAt[context])
                return;
            if (obj.itemId > 0 && obj.stack > 0 && Game1.chatMode)
            {
                Game1.cursorOverride = 2;
            }
            else
            {
                if (obj.itemId <= 0 || obj.stack <= 0)
                    return;
                Game1.cursorOverride = 3;
            }
        }

        private static bool OverrideLeftClick(Item[] inv, int context = 0, int slot = 0)
        {
            Item I = inv[slot];
            if (Game1.cursorOverride == 2)
            {
                if (ChatManager.AddChatText(Game1.fontMouseText, ItemTagHandler.GenerateTag(I), Vector2.One))
                    Game1.PlaySound(12, -1, -1, 1);
                return true;
            }
            if (Game1.cursorOverride == 3)
            {
                if (!ItemSlot.canFavoriteAt[context])
                    return false;
                I.favorited = !I.favorited;
                Game1.PlaySound(12, -1, -1, 1);
                return true;
            }
            if (Game1.cursorOverride == 7)
            {
                inv[slot] = Game1.player[Game1.myPlayer].GetItem(Game1.myPlayer, inv[slot], false, true);
                Game1.PlaySound(12, -1, -1, 1);
                return true;
            }
            if (Game1.cursorOverride == 8)
            {
                inv[slot] = Game1.player[Game1.myPlayer].GetItem(Game1.myPlayer, inv[slot], false, true);
                if (Game1.player[Game1.myPlayer].chest > -1)
                    NetMessage.SendData(32, -1, -1, "", Game1.player[Game1.myPlayer].chest, (float)slot, 0.0f, 0.0f, 0, 0, 0);
                return true;
            }
            if (Game1.cursorOverride != 9)
                return false;
            ChestUI.TryPlacingInChest(inv[slot], false);
            return true;
        }

        public static void LeftClick(ref Item inv, int context = 0)
        {
            ItemSlot.singleSlotArray[0] = inv;
            ItemSlot.LeftClick(ItemSlot.singleSlotArray, context, 0);
            inv = ItemSlot.singleSlotArray[0];
        }

        public static void LeftClick(Item[] inv, int context = 0, int slot = 0)
        {
            if (ItemSlot.OverrideLeftClick(inv, context, slot))
                return;
            inv[slot].newAndShiny = false;
            Player player = Game1.player[Game1.myPlayer];
            bool flag = false;
            switch (context)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                    flag = player.chest == -1;
                    break;
            }
            if (Game1.keyState.IsKeyDown(Keys.LeftShift) && flag)
            {
                if (inv[slot].itemId <= 0)
                    return;
                if (Game1.npcShop > 0 && !inv[slot].favorited)
                {
                    Chest chest = Game1.instance.shop[Game1.npcShop];
                    if (inv[slot].itemId >= 71 && inv[slot].itemId <= 74)
                        return;
                    if (player.SellItem(inv[slot].value, inv[slot].stack))
                    {
                        chest.AddShop(inv[slot]);
                        inv[slot].SetDefaults(0, false);
                        Game1.PlaySound(18, -1, -1, 1);
                        Recipe.FindRecipes();
                    }
                    else
                    {
                        if (inv[slot].value != 0)
                            return;
                        chest.AddShop(inv[slot]);
                        inv[slot].SetDefaults(0, false);
                        Game1.PlaySound(7, -1, -1, 1);
                        Recipe.FindRecipes();
                    }
                }
                else
                {
                    if (inv[slot].favorited || ItemSlot.Options.DisableLeftShiftTrashCan)
                        return;
                    Game1.PlaySound(7, -1, -1, 1);
                    player.trashItem = inv[slot].Clone();
                    inv[slot].SetDefaults(0, false);
                    if (context == 3 && Game1.netMode == 1)
                        NetMessage.SendData(32, -1, -1, "", player.chest, (float)slot, 0.0f, 0.0f, 0, 0, 0);
                    Recipe.FindRecipes();
                }
            }
            else
            {
                if (player.selectedItem == slot && player.itemAnimation > 0 || player.itemTime != 0)
                    return;
                switch (ItemSlot.PickItemMovementAction(inv, context, slot, Game1.mouseItem))
                {
                    case 0:
                        if (context == 6 && Game1.mouseItem.itemId != 0)
                            inv[slot].SetDefaults(0, false);
                        Utils.Swap<Item>(ref inv[slot], ref Game1.mouseItem);
                        if (inv[slot].stack > 0)
                        {
                            switch (context)
                            {
                                case 0:
                                    AchievementsHelper.NotifyItemPickup(player, inv[slot]);
                                    break;
                                case 8:
                                case 9:
                                case 10:
                                case 11:
                                case 12:
                                case 16:
                                case 17:
                                    AchievementsHelper.HandleOnEquip(player, inv[slot], context);
                                    break;
                            }
                        }
                        if (inv[slot].itemId == 0 || inv[slot].stack < 1)
                            inv[slot] = new Item();
                        if (Game1.mouseItem.IsTheSameAs(inv[slot]))
                        {
                            Utils.Swap<bool>(ref inv[slot].favorited, ref Game1.mouseItem.favorited);
                            if (inv[slot].stack != inv[slot].maxStack && Game1.mouseItem.stack != Game1.mouseItem.maxStack)
                            {
                                if (Game1.mouseItem.stack + inv[slot].stack <= Game1.mouseItem.maxStack)
                                {
                                    inv[slot].stack += Game1.mouseItem.stack;
                                    Game1.mouseItem.stack = 0;
                                }
                                else
                                {
                                    int num = Game1.mouseItem.maxStack - inv[slot].stack;
                                    inv[slot].stack += num;
                                    Game1.mouseItem.stack -= num;
                                }
                            }
                        }
                        if (Game1.mouseItem.itemId == 0 || Game1.mouseItem.stack < 1)
                            Game1.mouseItem = new Item();
                        if (Game1.mouseItem.itemId > 0 || inv[slot].itemId > 0)
                        {
                            Recipe.FindRecipes();
                            Game1.PlaySound(7, -1, -1, 1);
                        }
                        if (context == 3 && Game1.netMode == 1)
                        {
                            NetMessage.SendData(32, -1, -1, "", player.chest, (float)slot, 0.0f, 0.0f, 0, 0, 0);
                            break;
                        }
                        break;
                    case 1:
                        if (Game1.mouseItem.stack == 1 && Game1.mouseItem.itemId > 0 && (inv[slot].itemId > 0 && inv[slot].IsNotTheSameAs(Game1.mouseItem)))
                        {
                            Utils.Swap<Item>(ref inv[slot], ref Game1.mouseItem);
                            Game1.PlaySound(7, -1, -1, 1);
                            if (inv[slot].stack > 0)
                            {
                                switch (context)
                                {
                                    case 0:
                                        AchievementsHelper.NotifyItemPickup(player, inv[slot]);
                                        break;
                                    case 8:
                                    case 9:
                                    case 10:
                                    case 11:
                                    case 12:
                                    case 16:
                                    case 17:
                                        AchievementsHelper.HandleOnEquip(player, inv[slot], context);
                                        break;
                                }
                            }
                            else
                                break;
                        }
                        else
                        {
                            if (Game1.mouseItem.itemId == 0 && inv[slot].itemId > 0)
                            {
                                Utils.Swap<Item>(ref inv[slot], ref Game1.mouseItem);
                                if (inv[slot].itemId == 0 || inv[slot].stack < 1)
                                    inv[slot] = new Item();
                                if (Game1.mouseItem.itemId == 0 || Game1.mouseItem.stack < 1)
                                    Game1.mouseItem = new Item();
                                if (Game1.mouseItem.itemId > 0 || inv[slot].itemId > 0)
                                {
                                    Recipe.FindRecipes();
                                    Game1.PlaySound(7, -1, -1, 1);
                                    break;
                                }
                                break;
                            }
                            if (Game1.mouseItem.itemId > 0 && inv[slot].itemId == 0)
                            {
                                if (Game1.mouseItem.stack == 1)
                                {
                                    Utils.Swap<Item>(ref inv[slot], ref Game1.mouseItem);
                                    if (inv[slot].itemId == 0 || inv[slot].stack < 1)
                                        inv[slot] = new Item();
                                    if (Game1.mouseItem.itemId == 0 || Game1.mouseItem.stack < 1)
                                        Game1.mouseItem = new Item();
                                    if (Game1.mouseItem.itemId > 0 || inv[slot].itemId > 0)
                                    {
                                        Recipe.FindRecipes();
                                        Game1.PlaySound(7, -1, -1, 1);
                                    }
                                }
                                else
                                {
                                    --Game1.mouseItem.stack;
                                    inv[slot].SetDefaults(Game1.mouseItem.itemId, false);
                                    Recipe.FindRecipes();
                                    Game1.PlaySound(7, -1, -1, 1);
                                }
                                if (inv[slot].stack > 0)
                                {
                                    switch (context)
                                    {
                                        case 0:
                                            AchievementsHelper.NotifyItemPickup(player, inv[slot]);
                                            break;
                                        case 8:
                                        case 9:
                                        case 10:
                                        case 11:
                                        case 12:
                                        case 16:
                                        case 17:
                                            AchievementsHelper.HandleOnEquip(player, inv[slot], context);
                                            break;
                                    }
                                }
                                else
                                    break;
                            }
                            else
                                break;
                        }
                        break;
                    case 2:
                        if (Game1.mouseItem.stack == 1 && (int)Game1.mouseItem.dye > 0 && (inv[slot].itemId > 0 && inv[slot].itemId != Game1.mouseItem.itemId))
                        {
                            Utils.Swap<Item>(ref inv[slot], ref Game1.mouseItem);
                            Game1.PlaySound(7, -1, -1, 1);
                            if (inv[slot].stack > 0)
                            {
                                switch (context)
                                {
                                    case 0:
                                        AchievementsHelper.NotifyItemPickup(player, inv[slot]);
                                        break;
                                    case 8:
                                    case 9:
                                    case 10:
                                    case 11:
                                    case 12:
                                    case 16:
                                    case 17:
                                        AchievementsHelper.HandleOnEquip(player, inv[slot], context);
                                        break;
                                }
                            }
                            else
                                break;
                        }
                        else
                        {
                            if (Game1.mouseItem.itemId == 0 && inv[slot].itemId > 0)
                            {
                                Utils.Swap<Item>(ref inv[slot], ref Game1.mouseItem);
                                if (inv[slot].itemId == 0 || inv[slot].stack < 1)
                                    inv[slot] = new Item();
                                if (Game1.mouseItem.itemId == 0 || Game1.mouseItem.stack < 1)
                                    Game1.mouseItem = new Item();
                                if (Game1.mouseItem.itemId > 0 || inv[slot].itemId > 0)
                                {
                                    Recipe.FindRecipes();
                                    Game1.PlaySound(7, -1, -1, 1);
                                    break;
                                }
                                break;
                            }
                            if ((int)Game1.mouseItem.dye > 0 && inv[slot].itemId == 0)
                            {
                                if (Game1.mouseItem.stack == 1)
                                {
                                    Utils.Swap<Item>(ref inv[slot], ref Game1.mouseItem);
                                    if (inv[slot].itemId == 0 || inv[slot].stack < 1)
                                        inv[slot] = new Item();
                                    if (Game1.mouseItem.itemId == 0 || Game1.mouseItem.stack < 1)
                                        Game1.mouseItem = new Item();
                                    if (Game1.mouseItem.itemId > 0 || inv[slot].itemId > 0)
                                    {
                                        Recipe.FindRecipes();
                                        Game1.PlaySound(7, -1, -1, 1);
                                    }
                                }
                                else
                                {
                                    --Game1.mouseItem.stack;
                                    inv[slot].SetDefaults(Game1.mouseItem.itemId, false);
                                    Recipe.FindRecipes();
                                    Game1.PlaySound(7, -1, -1, 1);
                                }
                                if (inv[slot].stack > 0)
                                {
                                    switch (context)
                                    {
                                        case 0:
                                            AchievementsHelper.NotifyItemPickup(player, inv[slot]);
                                            break;
                                        case 8:
                                        case 9:
                                        case 10:
                                        case 11:
                                        case 12:
                                        case 16:
                                        case 17:
                                            AchievementsHelper.HandleOnEquip(player, inv[slot], context);
                                            break;
                                    }
                                }
                                else
                                    break;
                            }
                            else
                                break;
                        }
                        break;
                    case 3:
                        Game1.mouseItem.netDefaults(inv[slot].netID);
                        if (inv[slot].buyOnce)
                            Game1.mouseItem.Prefix((int)inv[slot].prefix);
                        else
                            Game1.mouseItem.Prefix(-1);
                        Game1.mouseItem.position = player.Center - new Vector2((float)Game1.mouseItem.width, (float)Game1.mouseItem.headSlot) / 2f;
                        ItemText.NewText(Game1.mouseItem, Game1.mouseItem.stack, false, false);
                        if (inv[slot].buyOnce && --inv[slot].stack <= 0)
                            inv[slot].SetDefaults(0, false);
                        if (inv[slot].value > 0)
                        {
                            Game1.PlaySound(18, -1, -1, 1);
                            break;
                        }
                        Game1.PlaySound(7, -1, -1, 1);
                        break;
                    case 4:
                        Chest chest = Game1.instance.shop[Game1.npcShop];
                        if (player.SellItem(Game1.mouseItem.value, Game1.mouseItem.stack))
                        {
                            chest.AddShop(Game1.mouseItem);
                            Game1.mouseItem.SetDefaults(0, false);
                            Game1.PlaySound(18, -1, -1, 1);
                        }
                        else if (Game1.mouseItem.value == 0)
                        {
                            chest.AddShop(Game1.mouseItem);
                            Game1.mouseItem.SetDefaults(0, false);
                            Game1.PlaySound(7, -1, -1, 1);
                        }
                        Recipe.FindRecipes();
                        break;
                }
                switch (context)
                {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 5:
                        break;
                    default:
                        inv[slot].favorited = false;
                        break;
                }
            }
        }

        public static int PickItemMovementAction(Item[] inv, int context, int slot, Item checkItem)
        {
            Player player = Game1.player[Game1.myPlayer];
            int num = -1;
            if (context == 0)
                num = 0;
            else if (context == 1)
            {
                if (checkItem.itemId == 0 || checkItem.itemId == 71 || (checkItem.itemId == 72 || checkItem.itemId == 73) || checkItem.itemId == 74)
                    num = 0;
            }
            else if (context == 2)
            {
                if ((checkItem.itemId == 0 || checkItem.ammo > 0 || checkItem.bait > 0) && !checkItem.notAmmo || checkItem.itemId == 530)
                    num = 0;
            }
            else if (context == 3)
                num = 0;
            else if (context == 4)
                num = 0;
            else if (context == 5)
            {
                if (checkItem.Prefix(-3) || checkItem.itemId == 0)
                    num = 0;
            }
            else if (context == 6)
                num = 0;
            else if (context == 7)
            {
                if (checkItem.material || checkItem.itemId == 0)
                    num = 0;
            }
            else if (context == 8)
            {
                if (checkItem.itemId == 0 || checkItem.headSlot > -1 && slot == 0 || (checkItem.bodySlot > -1 && slot == 1 || checkItem.legSlot > -1 && slot == 2))
                    num = 1;
            }
            else if (context == 9)
            {
                if (checkItem.itemId == 0 || checkItem.headSlot > -1 && slot == 10 || (checkItem.bodySlot > -1 && slot == 11 || checkItem.legSlot > -1 && slot == 12))
                    num = 1;
            }
            else if (context == 10)
            {
                if (checkItem.itemId == 0 || checkItem.accessory && !ItemSlot.AccCheck(checkItem, slot))
                    num = 1;
            }
            else if (context == 11)
            {
                if (checkItem.itemId == 0 || checkItem.accessory && !ItemSlot.AccCheck(checkItem, slot))
                    num = 1;
            }
            else if (context == 12)
                num = 2;
            else if (context == 15)
            {
                if (checkItem.itemId == 0 && inv[slot].itemId > 0)
                {
                    if (player.BuyItem(inv[slot].value))
                        num = 3;
                }
                else if (inv[slot].itemId == 0 && checkItem.itemId > 0 && (checkItem.itemId < 71 || checkItem.itemId > 74))
                    num = 4;
            }
            else if (context == 16)
            {
                if (checkItem.itemId == 0 || Game1.projHook[checkItem.shoot])
                    num = 1;
            }
            else if (context == 17)
            {
                if (checkItem.itemId == 0 || checkItem.mountType != -1 && !MountID.Sets.Cart[checkItem.mountType])
                    num = 1;
            }
            else if (context == 19)
            {
                if (checkItem.itemId == 0 || checkItem.buffType > 0 && Game1.vanityPet[checkItem.buffType] && !Game1.lightPet[checkItem.buffType])
                    num = 1;
            }
            else if (context == 18)
            {
                if (checkItem.itemId == 0 || checkItem.mountType != -1 && MountID.Sets.Cart[checkItem.mountType])
                    num = 1;
            }
            else if (context == 20 && (checkItem.itemId == 0 || checkItem.buffType > 0 && Game1.lightPet[checkItem.buffType]))
                num = 1;
            return num;
        }

        public static void RightClick(ref Item inv, int context = 0)
        {
            ItemSlot.singleSlotArray[0] = inv;
            ItemSlot.RightClick(ItemSlot.singleSlotArray, context, 0);
            inv = ItemSlot.singleSlotArray[0];
        }

        public static void RightClick(Item[] inv, int context = 0, int slot = 0)
        {
            Player player = Game1.player[Game1.myPlayer];
            inv[slot].newAndShiny = false;
            if (player.itemAnimation > 0)
                return;
            bool flag1 = false;
            if (context == 0)
            {
                flag1 = true;
                if (Game1.mouseRight && inv[slot].itemId >= 3318 && inv[slot].itemId <= 3332)
                {
                    if (Game1.mouseRightRelease)
                    {
                        player.OpenBossBag(inv[slot].itemId);
                        --inv[slot].stack;
                        if (inv[slot].stack == 0)
                            inv[slot].SetDefaults(0, false);
                        Game1.PlaySound(7, -1, -1, 1);
                        Game1.stackSplit = 30;
                        Game1.mouseRightRelease = false;
                        Recipe.FindRecipes();
                    }
                }
                else if (Game1.mouseRight && (inv[slot].itemId >= 2334 && inv[slot].itemId <= 2336 || inv[slot].itemId >= 3203 && inv[slot].itemId <= 3208))
                {
                    if (Game1.mouseRightRelease)
                    {
                        player.openCrate(inv[slot].itemId);
                        --inv[slot].stack;
                        if (inv[slot].stack == 0)
                            inv[slot].SetDefaults(0, false);
                        Game1.PlaySound(7, -1, -1, 1);
                        Game1.stackSplit = 30;
                        Game1.mouseRightRelease = false;
                        Recipe.FindRecipes();
                    }
                }
                else if (Game1.mouseRight && inv[slot].itemId == 3093)
                {
                    if (Game1.mouseRightRelease)
                    {
                        player.openHerbBag();
                        --inv[slot].stack;
                        if (inv[slot].stack == 0)
                            inv[slot].SetDefaults(0, false);
                        Game1.PlaySound(7, -1, -1, 1);
                        Game1.stackSplit = 30;
                        Game1.mouseRightRelease = false;
                        Recipe.FindRecipes();
                    }
                }
                else if (Game1.mouseRight && inv[slot].itemId == 1774)
                {
                    if (Game1.mouseRightRelease)
                    {
                        --inv[slot].stack;
                        if (inv[slot].stack == 0)
                            inv[slot].SetDefaults(0, false);
                        Game1.PlaySound(7, -1, -1, 1);
                        Game1.stackSplit = 30;
                        Game1.mouseRightRelease = false;
                        player.openGoodieBag();
                        Recipe.FindRecipes();
                    }
                }
                else if (Game1.mouseRight && inv[slot].itemId == 3085)
                {
                    if (Game1.mouseRightRelease && player.consumeItem(327))
                    {
                        --inv[slot].stack;
                        if (inv[slot].stack == 0)
                            inv[slot].SetDefaults(0, false);
                        Game1.PlaySound(7, -1, -1, 1);
                        Game1.stackSplit = 30;
                        Game1.mouseRightRelease = false;
                        player.openLockBox();
                        Recipe.FindRecipes();
                    }
                }
                else if (Game1.mouseRight && inv[slot].itemId == 1869)
                {
                    if (Game1.mouseRightRelease)
                    {
                        --inv[slot].stack;
                        if (inv[slot].stack == 0)
                            inv[slot].SetDefaults(0, false);
                        Game1.PlaySound(7, -1, -1, 1);
                        Game1.stackSplit = 30;
                        Game1.mouseRightRelease = false;
                        player.openPresent();
                        Recipe.FindRecipes();
                    }
                }
                else if (Game1.mouseRight && Game1.mouseRightRelease && (inv[slot].itemId == 599 || inv[slot].itemId == 600 || inv[slot].itemId == 601))
                {
                    Game1.PlaySound(7, -1, -1, 1);
                    Game1.stackSplit = 30;
                    Game1.mouseRightRelease = false;
                    int num = Game1.rand.Next(14);
                    if (num == 0 && Game1.hardMode)
                        inv[slot].SetDefaults(602, false);
                    else if (num <= 7)
                    {
                        inv[slot].SetDefaults(586, false);
                        inv[slot].stack = Game1.rand.Next(20, 50);
                    }
                    else
                    {
                        inv[slot].SetDefaults(591, false);
                        inv[slot].stack = Game1.rand.Next(20, 50);
                    }
                    Recipe.FindRecipes();
                }
                else
                    flag1 = false;
            }
            else if (context == 9 || context == 11)
            {
                flag1 = true;
                if (Game1.mouseRight && Game1.mouseRightRelease && (inv[slot].itemId > 0 && inv[slot].stack > 0 || inv[slot - 10].itemId > 0 && inv[slot - 10].stack > 0))
                {
                    bool flag2 = true;
                    if (flag2 && context == 11 && (int)inv[slot].wingSlot > 0)
                    {
                        for (int index = 3; index < 10; ++index)
                        {
                            if ((int)inv[index].wingSlot > 0 && index != slot - 10)
                                flag2 = false;
                        }
                    }
                    if (flag2)
                    {
                        Utils.Swap<Item>(ref inv[slot], ref inv[slot - 10]);
                        Game1.PlaySound(7, -1, -1, 1);
                        Recipe.FindRecipes();
                        if (inv[slot].stack > 0)
                        {
                            switch (context)
                            {
                                case 0:
                                    AchievementsHelper.NotifyItemPickup(player, inv[slot]);
                                    break;
                                case 8:
                                case 9:
                                case 10:
                                case 11:
                                case 12:
                                case 16:
                                case 17:
                                    AchievementsHelper.HandleOnEquip(player, inv[slot], context);
                                    break;
                            }
                        }
                    }
                }
            }
            else if (context == 12)
            {
                flag1 = true;
                if (Game1.mouseRight && Game1.mouseRightRelease && (Game1.mouseItem.stack < Game1.mouseItem.maxStack && Game1.mouseItem.itemId > 0) && (inv[slot].itemId > 0 && Game1.mouseItem.itemId == inv[slot].itemId))
                {
                    ++Game1.mouseItem.stack;
                    inv[slot].SetDefaults(0, false);
                    Game1.PlaySound(7, -1, -1, 1);
                }
            }
            else if (context == 15)
            {
                flag1 = true;
                Chest chest = Game1.instance.shop[Game1.npcShop];
                if (Game1.stackSplit <= 1 && Game1.mouseRight && inv[slot].itemId > 0 && (Game1.mouseItem.IsTheSameAs(inv[slot]) || Game1.mouseItem.itemId == 0))
                {
                    int num = Game1.superFastStack + 1;
                    for (int index = 0; index < num; ++index)
                    {
                        if ((Game1.mouseItem.stack < Game1.mouseItem.maxStack || Game1.mouseItem.itemId == 0) && (player.BuyItem(inv[slot].value) && inv[slot].stack > 0))
                        {
                            if (index == 0)
                                Game1.PlaySound(18, -1, -1, 1);
                            if (Game1.mouseItem.itemId == 0)
                            {
                                Game1.mouseItem.netDefaults(inv[slot].netID);
                                if ((int)inv[slot].prefix != 0)
                                    Game1.mouseItem.Prefix((int)inv[slot].prefix);
                                Game1.mouseItem.stack = 0;
                            }
                            ++Game1.mouseItem.stack;
                            Game1.stackSplit = Game1.stackSplit != 0 ? Game1.stackDelay : 15;
                            if (inv[slot].buyOnce && --inv[slot].stack <= 0)
                                inv[slot].SetDefaults(0, false);
                        }
                    }
                }
            }
            if (flag1)
                return;
            if ((context == 0 || context == 4 || context == 3) && (Game1.mouseRight && Game1.mouseRightRelease && inv[slot].maxStack == 1))
            {
                if ((int)inv[slot].dye > 0)
                {
                    bool success;
                    inv[slot] = ItemSlot.DyeSwap(inv[slot], out success);
                    if (success)
                    {
                        Game1.EquipPageSelected = 0;
                        AchievementsHelper.HandleOnEquip(player, inv[slot], 12);
                    }
                }
                else if (Game1.projHook[inv[slot].shoot])
                {
                    bool success;
                    inv[slot] = ItemSlot.EquipSwap(inv[slot], player.miscEquips, 4, out success);
                    if (success)
                    {
                        Game1.EquipPageSelected = 2;
                        AchievementsHelper.HandleOnEquip(player, inv[slot], 16);
                    }
                }
                else if (inv[slot].mountType != -1 && !MountID.Sets.Cart[inv[slot].mountType])
                {
                    bool success;
                    inv[slot] = ItemSlot.EquipSwap(inv[slot], player.miscEquips, 3, out success);
                    if (success)
                    {
                        Game1.EquipPageSelected = 2;
                        AchievementsHelper.HandleOnEquip(player, inv[slot], 17);
                    }
                }
                else if (inv[slot].mountType != -1 && MountID.Sets.Cart[inv[slot].mountType])
                {
                    bool success;
                    inv[slot] = ItemSlot.EquipSwap(inv[slot], player.miscEquips, 2, out success);
                    if (success)
                        Game1.EquipPageSelected = 2;
                }
                else if (inv[slot].buffType > 0 && Game1.lightPet[inv[slot].buffType])
                {
                    bool success;
                    inv[slot] = ItemSlot.EquipSwap(inv[slot], player.miscEquips, 1, out success);
                    if (success)
                        Game1.EquipPageSelected = 2;
                }
                else if (inv[slot].buffType > 0 && Game1.vanityPet[inv[slot].buffType])
                {
                    bool success;
                    inv[slot] = ItemSlot.EquipSwap(inv[slot], player.miscEquips, 0, out success);
                    if (success)
                        Game1.EquipPageSelected = 2;
                }
                else
                {
                    bool success;
                    inv[slot] = ItemSlot.ArmorSwap(inv[slot], out success);
                    if (success)
                    {
                        Game1.EquipPageSelected = 0;
                        AchievementsHelper.HandleOnEquip(player, inv[slot], 8);
                    }
                }
                Recipe.FindRecipes();
                if (context != 3 || Game1.netMode != 1)
                    return;
                NetMessage.SendData(32, -1, -1, "", player.chest, (float)slot, 0.0f, 0.0f, 0, 0, 0);
            }
            else
            {
                if (Game1.stackSplit > 1 || !Game1.mouseRight)
                    return;
                bool flag2 = true;
                if (context == 0 && inv[slot].maxStack <= 1)
                    flag2 = false;
                if (context == 3 && inv[slot].maxStack <= 1)
                    flag2 = false;
                if (context == 4 && inv[slot].maxStack <= 1)
                    flag2 = false;
                if (!flag2 || !Game1.mouseItem.IsTheSameAs(inv[slot]) && Game1.mouseItem.itemId != 0 || Game1.mouseItem.stack >= Game1.mouseItem.maxStack && Game1.mouseItem.itemId != 0)
                    return;
                if (Game1.mouseItem.itemId == 0)
                {
                    Game1.mouseItem = inv[slot].Clone();
                    Game1.mouseItem.stack = 0;
                    Game1.mouseItem.favorited = inv[slot].favorited && inv[slot].maxStack == 1;
                }
                ++Game1.mouseItem.stack;
                --inv[slot].stack;
                if (inv[slot].stack <= 0)
                    inv[slot] = new Item();
                Recipe.FindRecipes();
                Game1.soundInstanceMenuTick.Stop();
                Game1.soundInstanceMenuTick = Game1.soundMenuTick.CreateInstance();
                Game1.PlaySound(12, -1, -1, 1);
                Game1.stackSplit = Game1.stackSplit != 0 ? Game1.stackDelay : 15;
                if (context != 3 || Game1.netMode != 1)
                    return;
                NetMessage.SendData(32, -1, -1, "", player.chest, (float)slot, 0.0f, 0.0f, 0, 0, 0);
            }
        }

        public static void Draw(SpriteBatch spriteBatch, ref Item inv, int context, Vector2 position, Color lightColor = default(Color))
        {
            ItemSlot.singleSlotArray[0] = inv;
            ItemSlot.Draw(spriteBatch, ItemSlot.singleSlotArray, context, 0, position, lightColor);
            inv = ItemSlot.singleSlotArray[0];
        }

        public static void Draw(SpriteBatch spriteBatch, Item[] inv, int context, int slot, Vector2 position, Color lightColor = default(Color))
        {
            Player player = Game1.player[Game1.myPlayer];
            Item obj = inv[slot];
            float num1 = Game1.inventoryScale;
            Color color1 = Color.White;
            if (lightColor != Color.Transparent)
                color1 = lightColor;
            Texture2D texture2D1 = Game1.inventoryBackTexture;
            Color color2 = Game1.inventoryBack;
            bool flag = false;
            if (obj.itemId > 0 && obj.stack > 0 && (obj.favorited && context != 13) && (context != 21 && context != 22))
                texture2D1 = Game1.inventoryBack10Texture;
            else if (obj.itemId > 0 && obj.stack > 0 && (ItemSlot.Options.HighlightNewItems && obj.newAndShiny) && (context != 13 && context != 21 && context != 22))
            {
                texture2D1 = Game1.inventoryBack15Texture;
                float num2 = (float)((double)((float)Game1.mouseTextColor / (float)byte.MaxValue) * 0.200000002980232 + 0.800000011920929);
                color2 = Utils.MultiplyRGBA(color2, new Color(num2, num2, num2));
            }
            else if (context == 0 && slot < 10)
                texture2D1 = Game1.inventoryBack9Texture;
            else if (context == 10 || context == 8 || (context == 16 || context == 17) || (context == 19 || context == 18 || context == 20))
                texture2D1 = Game1.inventoryBack3Texture;
            else if (context == 11 || context == 9)
                texture2D1 = Game1.inventoryBack8Texture;
            else if (context == 12)
                texture2D1 = Game1.inventoryBack12Texture;
            else if (context == 3)
                texture2D1 = Game1.inventoryBack5Texture;
            else if (context == 4)
                texture2D1 = Game1.inventoryBack2Texture;
            else if (context == 7 || context == 5)
                texture2D1 = Game1.inventoryBack4Texture;
            else if (context == 6)
                texture2D1 = Game1.inventoryBack7Texture;
            else if (context == 13)
            {
                byte num2 = (byte)200;
                if (slot == Game1.player[Game1.myPlayer].selectedItem)
                {
                    texture2D1 = Game1.inventoryBack14Texture;
                    num2 = byte.MaxValue;
                }
                color2 = new Color((int)num2, (int)num2, (int)num2, (int)num2);
            }
            else if (context == 14 || context == 21)
                flag = true;
            else if (context == 15)
                texture2D1 = Game1.inventoryBack6Texture;
            else if (context == 22)
                texture2D1 = Game1.inventoryBack4Texture;
            if (!flag)
                spriteBatch.Draw(texture2D1, position, new Rectangle?(), color2, 0.0f, new Vector2(), num1, SpriteEffects.None, 0.0f);
            int num3 = -1;
            switch (context)
            {
                case 8:
                    if (slot == 0)
                        num3 = 0;
                    if (slot == 1)
                        num3 = 6;
                    if (slot == 2)
                    {
                        num3 = 12;
                        break;
                    }
                    break;
                case 9:
                    if (slot == 10)
                        num3 = 3;
                    if (slot == 11)
                        num3 = 9;
                    if (slot == 12)
                    {
                        num3 = 15;
                        break;
                    }
                    break;
                case 10:
                    num3 = 11;
                    break;
                case 11:
                    num3 = 2;
                    break;
                case 12:
                    num3 = 1;
                    break;
                case 16:
                    num3 = 4;
                    break;
                case 17:
                    num3 = 13;
                    break;
                case 18:
                    num3 = 7;
                    break;
                case 19:
                    num3 = 10;
                    break;
                case 20:
                    num3 = 17;
                    break;
            }
            if ((obj.itemId <= 0 || obj.stack <= 0) && num3 != -1)
            {
                Texture2D texture2D2 = Game1.extraTexture[54];
                Rectangle r = Utils.Frame(texture2D2, 3, 6, num3 % 3, num3 / 3);
                r.Width -= 2;
                r.Height -= 2;
                spriteBatch.Draw(texture2D2, position + Utils.Size(texture2D1) / 2f * num1, new Rectangle?(r), Color.White * 0.35f, 0.0f, Utils.Size(r) / 2f, num1, SpriteEffects.None, 0.0f);
            }
            if (obj.itemId > 0 && obj.stack > 0)
            {
                Texture2D texture2D2 = Game1.itemTexture[obj.itemId];
                Rectangle r = Game1.itemAnimations[obj.itemId] == null ? Utils.Frame(texture2D2, 1, 1, 0, 0) : Game1.itemAnimations[obj.itemId].GetFrame(texture2D2);
                Color currentColor = color1;
                float scale1 = 1f;
                ItemSlot.GetItemLight(ref currentColor, ref scale1, obj, false);
                float num2 = 1f;
                if (r.Width > 32 || r.Height > 32)
                    num2 = r.Width <= r.Height ? 32f / (float)r.Height : 32f / (float)r.Width;
                float scale2 = num2 * num1;
                Vector2 position1 = position + Utils.Size(texture2D1) * num1 / 2f - Utils.Size(r) * scale2 / 2f;
                Vector2 origin = Utils.Size(r) * (float)((double)scale1 / 2.0 - 0.5);
                spriteBatch.Draw(texture2D2, position1, new Rectangle?(r), obj.GetAlpha(currentColor), 0.0f, origin, scale2 * scale1, SpriteEffects.None, 0.0f);
                if (obj.color != Color.Transparent)
                    spriteBatch.Draw(texture2D2, position1, new Rectangle?(r), obj.GetColor(color1), 0.0f, origin, scale2 * scale1, SpriteEffects.None, 0.0f);
                if (obj.stack > 1)
                    ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Game1.fontItemStack, obj.stack.ToString(), position + new Vector2(10f, 26f) * num1, color1, 0.0f, Vector2.Zero, new Vector2(num1), -1f, num1);
                int num4 = -1;
                if (context == 13)
                {
                    if (obj.useAmmo > 0)
                    {
                        int num5 = obj.useAmmo;
                        num4 = 0;
                        for (int index = 0; index < 58; ++index)
                        {
                            if (inv[index].ammo == num5)
                                num4 += inv[index].stack;
                        }
                    }
                    if (obj.fishingPole > 0)
                    {
                        num4 = 0;
                        for (int index = 0; index < 58; ++index)
                        {
                            if (inv[index].bait > 0)
                                num4 += inv[index].stack;
                        }
                    }
                    if (obj.tileWand > 0)
                    {
                        int num5 = obj.tileWand;
                        num4 = 0;
                        for (int index = 0; index < 58; ++index)
                        {
                            if (inv[index].itemId == num5)
                                num4 += inv[index].stack;
                        }
                    }
                    if (obj.itemId == 509 || obj.itemId == 851 || obj.itemId == 850)
                    {
                        num4 = 0;
                        for (int index = 0; index < 58; ++index)
                        {
                            if (inv[index].itemId == 530)
                                num4 += inv[index].stack;
                        }
                    }
                }
                if (num4 != -1)
                    ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Game1.fontItemStack, num4.ToString(), position + new Vector2(8f, 30f) * num1, color1, 0.0f, Vector2.Zero, new Vector2(num1 * 0.8f), -1f, num1);
                if (context == 13)
                {
                    string text = string.Concat((object)(slot + 1));
                    if (text == "10")
                        text = "0";
                    ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Game1.fontItemStack, text, position + new Vector2(8f, 4f) * num1, color1, 0.0f, Vector2.Zero, new Vector2(num1), -1f, num1);
                }
                if (context == 13 && obj.potion)
                {
                    Vector2 position2 = position + Utils.Size(texture2D1) * num1 / 2f - Utils.Size(Game1.cdTexture) * num1 / 2f;
                    Color color3 = obj.GetAlpha(color1) * ((float)player.potionDelay / (float)player.potionDelayTime);
                    spriteBatch.Draw(Game1.cdTexture, position2, new Rectangle?(), color3, 0.0f, new Vector2(), scale2, SpriteEffects.None, 0.0f);
                }
                if ((context == 10 || context == 18) && (obj.expertOnly && !Game1.expertMode))
                {
                    Vector2 position2 = position + Utils.Size(texture2D1) * num1 / 2f - Utils.Size(Game1.cdTexture) * num1 / 2f;
                    Color white = Color.White;
                    spriteBatch.Draw(Game1.cdTexture, position2, new Rectangle?(), white, 0.0f, new Vector2(), scale2, SpriteEffects.None, 0.0f);
                }
            }
            else if (context == 6)
            {
                Texture2D texture2D2 = Game1.trashTexture;
                Vector2 position1 = position + Utils.Size(texture2D1) * num1 / 2f - Utils.Size(texture2D2) * num1 / 2f;
                spriteBatch.Draw(texture2D2, position1, new Rectangle?(), new Color(100, 100, 100, 100), 0.0f, new Vector2(), num1, SpriteEffects.None, 0.0f);
            }
            if (context != 0 || slot >= 10)
                return;
            float num6 = num1;
            string text1 = string.Concat((object)(slot + 1));
            if (text1 == "10")
                text1 = "0";
            Color baseColor = Game1.inventoryBack;
            int num7 = 0;
            if (Game1.player[Game1.myPlayer].selectedItem == slot)
            {
                num7 -= 3;
                baseColor.R = byte.MaxValue;
                baseColor.B = (byte)0;
                baseColor.G = (byte)210;
                baseColor.A = (byte)100;
                float num2 = num6 * 1.4f;
            }
            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Game1.fontItemStack, text1, position + new Vector2(6f, (float)(4 + num7)) * num1, baseColor, 0.0f, Vector2.Zero, new Vector2(num1), -1f, num1);
        }

        public static void MouseHover(ref Item inv, int context = 0)
        {
            ItemSlot.singleSlotArray[0] = inv;
            ItemSlot.MouseHover(ItemSlot.singleSlotArray, context, 0);
            inv = ItemSlot.singleSlotArray[0];
        }

        public static void MouseHover(Item[] inv, int context = 0, int slot = 0)
        {
            if (context == 6 && Game1.hoverItemName == null)
                Game1.hoverItemName = Lang.inter[3];
            if (inv[slot].itemId > 0 && inv[slot].stack > 0)
            {
                Game1.hoverItemName = inv[slot].name;
                if (inv[slot].stack > 1)
                    Game1.hoverItemName = string.Concat(new object[4]
          {
            (object) Game1.hoverItemName,
            (object) " (",
            (object) inv[slot].stack,
            (object) ")"
          });
                Game1.toolTip = inv[slot].Clone();
                if (context == 8 && slot <= 2)
                    Game1.toolTip.wornArmor = true;
                if (context == 11 || context == 9)
                    Game1.toolTip.social = true;
                if (context != 15)
                    return;
                Game1.toolTip.buy = true;
            }
            else
            {
                if (context == 10 || context == 11)
                    Game1.hoverItemName = Lang.inter[9];
                if (context == 11)
                    Game1.hoverItemName = Lang.inter[11] + " " + Game1.hoverItemName;
                if (context == 8 || context == 9)
                {
                    if (slot == 0 || slot == 10)
                        Game1.hoverItemName = Lang.inter[12];
                    if (slot == 1 || slot == 11)
                        Game1.hoverItemName = Lang.inter[13];
                    if (slot == 2 || slot == 12)
                        Game1.hoverItemName = Lang.inter[14];
                    if (slot >= 10)
                        Game1.hoverItemName = Lang.inter[11] + " " + Game1.hoverItemName;
                }
                if (context == 12)
                    Game1.hoverItemName = Lang.inter[57];
                if (context == 16)
                    Game1.hoverItemName = Lang.inter[90];
                if (context == 17)
                    Game1.hoverItemName = Lang.inter[91];
                if (context == 19)
                    Game1.hoverItemName = Lang.inter[92];
                if (context == 18)
                    Game1.hoverItemName = Lang.inter[93];
                if (context != 20)
                    return;
                Game1.hoverItemName = Lang.inter[94];
            }
        }

        private static bool AccCheck(Item item, int slot)
        {
            Player player = Game1.player[Game1.myPlayer];
            if (slot != -1 && (player.armor[slot].IsTheSameAs(item) || (int)player.armor[slot].wingSlot > 0 && (int)item.wingSlot > 0))
                return false;
            for (int index = 0; index < player.armor.Length; ++index)
            {
                if (slot < 10 && index < 10 && ((int)item.wingSlot > 0 && (int)player.armor[index].wingSlot > 0 || slot >= 10 && index >= 10 && ((int)item.wingSlot > 0 && (int)player.armor[index].wingSlot > 0)) || item.IsTheSameAs(player.armor[index]))
                    return true;
            }
            return false;
        }

        private static Item DyeSwap(Item item, out bool success)
        {
            success = false;
            if ((int)item.dye <= 0)
                return item;
            Player player = Game1.player[Game1.myPlayer];
            for (int index = 0; index < 10; ++index)
            {
                if (player.dye[index].itemId == 0)
                {
                    ItemSlot.dyeSlotCount = index;
                    break;
                }
            }
            if (ItemSlot.dyeSlotCount >= 10)
                ItemSlot.dyeSlotCount = 0;
            if (ItemSlot.dyeSlotCount < 0)
                ItemSlot.dyeSlotCount = 9;
            Item obj = player.dye[ItemSlot.dyeSlotCount].Clone();
            player.dye[ItemSlot.dyeSlotCount] = item.Clone();
            ++ItemSlot.dyeSlotCount;
            if (ItemSlot.dyeSlotCount >= 10)
                ItemSlot.accSlotCount = 0;
            Game1.PlaySound(7, -1, -1, 1);
            Recipe.FindRecipes();
            success = true;
            return obj;
        }

        private static Item ArmorSwap(Item item, out bool success)
        {
            success = false;
            if (item.headSlot == -1 && item.bodySlot == -1 && (item.legSlot == -1 && !item.accessory))
                return item;
            Player player = Game1.player[Game1.myPlayer];
            int index1 = !item.vanity || item.accessory ? 0 : 10;
            item.favorited = false;
            Item obj = item;
            if (item.headSlot != -1)
            {
                obj = player.armor[index1].Clone();
                player.armor[index1] = item.Clone();
            }
            else if (item.bodySlot != -1)
            {
                obj = player.armor[index1 + 1].Clone();
                player.armor[index1 + 1] = item.Clone();
            }
            else if (item.legSlot != -1)
            {
                obj = player.armor[index1 + 2].Clone();
                player.armor[index1 + 2] = item.Clone();
            }
            else if (item.accessory)
            {
                int num = 5 + Game1.player[Game1.myPlayer].extraAccessorySlots;
                for (int index2 = 3; index2 < 3 + num; ++index2)
                {
                    if (player.armor[index2].itemId == 0)
                    {
                        ItemSlot.accSlotCount = index2 - 3;
                        break;
                    }
                }
                for (int index2 = 0; index2 < player.armor.Length; ++index2)
                {
                    if (item.IsTheSameAs(player.armor[index2]))
                        ItemSlot.accSlotCount = index2 - 3;
                    if (index2 < 10 && (int)item.wingSlot > 0 && (int)player.armor[index2].wingSlot > 0)
                        ItemSlot.accSlotCount = index2 - 3;
                }
                if (ItemSlot.accSlotCount >= num)
                    ItemSlot.accSlotCount = 0;
                if (ItemSlot.accSlotCount < 0)
                    ItemSlot.accSlotCount = num - 1;
                int index3 = 3 + ItemSlot.accSlotCount;
                for (int index2 = 0; index2 < player.armor.Length; ++index2)
                {
                    if (item.IsTheSameAs(player.armor[index2]))
                        index3 = index2;
                }
                obj = player.armor[index3].Clone();
                player.armor[index3] = item.Clone();
                ++ItemSlot.accSlotCount;
                if (ItemSlot.accSlotCount >= num)
                    ItemSlot.accSlotCount = 0;
            }
            Game1.PlaySound(7, -1, -1, 1);
            Recipe.FindRecipes();
            success = true;
            return obj;
        }

        private static Item EquipSwap(Item item, Item[] inv, int slot, out bool success)
        {
            success = false;
            Player player = Game1.player[Game1.myPlayer];
            item.favorited = false;
            Item obj = inv[slot].Clone();
            inv[slot] = item.Clone();
            Game1.PlaySound(7, -1, -1, 1);
            Recipe.FindRecipes();
            success = true;
            return obj;
        }

        public static void EquipPage(Item item)
        {
            Game1.EquipPage = -1;
            if (Game1.projHook[item.shoot])
                Game1.EquipPage = 2;
            else if (item.mountType != -1)
                Game1.EquipPage = 2;
            else if ((int)item.dye > 0 && Game1.EquipPageSelected == 1)
            {
                Game1.EquipPage = 0;
            }
            else
            {
                if (item.legSlot == -1 && item.headSlot == -1 && (item.bodySlot == -1 && !item.accessory))
                    return;
                Game1.EquipPage = 0;
            }
        }

        public static void DrawMoney(SpriteBatch sb, string text, float shopx, float shopy, int[] coinsArray, bool horizontal = false)
        {
            Utils.DrawBorderStringFourWay(sb, Game1.fontMouseText, text, shopx, shopy + 40f, Color.White * ((float)Game1.mouseTextColor / (float)byte.MaxValue), Color.Black, Vector2.Zero, 1f);
            if (horizontal)
            {
                for (int index = 0; index < 4; ++index)
                {
                    if (index == 0)
                    {
                        int num = coinsArray[3 - index];
                    }
                    Vector2 position = new Vector2((float)((double)shopx + (double)ChatManager.GetStringSize(Game1.fontMouseText, text, Vector2.One, -1f).X + (double)(24 * index) + 45.0), shopy + 50f);
                    sb.Draw(Game1.itemTexture[74 - index], position, new Rectangle?(), Color.White, 0.0f, Utils.Size(Game1.itemTexture[74 - index]) / 2f, 1f, SpriteEffects.None, 0.0f);
                    Utils.DrawBorderStringFourWay(sb, Game1.fontItemStack, coinsArray[3 - index].ToString(), position.X - 11f, position.Y, Color.White, Color.Black, new Vector2(0.3f), 0.75f);
                }
            }
            else
            {
                for (int index = 0; index < 4; ++index)
                {
                    int num = index != 0 || coinsArray[3 - index] <= 99 ? 0 : -6;
                    sb.Draw(Game1.itemTexture[74 - index], new Vector2(shopx + 11f + (float)(24 * index), shopy + 75f), new Rectangle?(), Color.White, 0.0f, Utils.Size(Game1.itemTexture[74 - index]) / 2f, 1f, SpriteEffects.None, 0.0f);
                    Utils.DrawBorderStringFourWay(sb, Game1.fontItemStack, coinsArray[3 - index].ToString(), shopx + (float)(24 * index) + (float)num, shopy + 75f, Color.White, Color.Black, new Vector2(0.3f), 0.75f);
                }
            }
        }

        public static void DrawSavings(SpriteBatch sb, float shopx, float shopy, bool horizontal = false)
        {
            Player player = Game1.player[Game1.myPlayer];
            bool overFlowing;
            long num1 = Utils.CoinsCount(out overFlowing, player.bank.item);
            long num2 = Utils.CoinsCount(out overFlowing, player.bank2.item);
            long count = Utils.CoinsCombineStacks(out overFlowing, num1, num2);
            if (count <= 0L)
                return;
            if (num2 > 0L)
                sb.Draw(Game1.itemTexture[346], Utils.CenteredRectangle(new Vector2(shopx + 80f, shopy + 50f), Utils.Size(Game1.itemTexture[346]) * 0.65f), new Rectangle?(), Color.White);
            if (num1 > 0L)
                sb.Draw(Game1.itemTexture[87], Utils.CenteredRectangle(new Vector2(shopx + 70f, shopy + 60f), Utils.Size(Game1.itemTexture[87]) * 0.65f), new Rectangle?(), Color.White);
            ItemSlot.DrawMoney(sb, Lang.inter[66], shopx, shopy, Utils.CoinsSplit(count), horizontal);
        }

        public static void GetItemLight(ref Color currentColor, Item item, bool outInTheWorld = false)
        {
            float scale = 1f;
            ItemSlot.GetItemLight(ref currentColor, ref scale, item, outInTheWorld);
        }

        public static void GetItemLight(ref Color currentColor, int type, bool outInTheWorld = false)
        {
            float scale = 1f;
            ItemSlot.GetItemLight(ref currentColor, ref scale, type, outInTheWorld);
        }

        public static void GetItemLight(ref Color currentColor, ref float scale, Item item, bool outInTheWorld = false)
        {
            ItemSlot.GetItemLight(ref currentColor, ref scale, item.itemId, outInTheWorld);
        }

        public static Color GetItemLight(ref Color currentColor, ref float scale, int type, bool outInTheWorld = false)
        {
            if (type < 0 || type > 3601)
                return currentColor;
            if (type == 662 || type == 663)
            {
                currentColor.R = (byte)Game1.DiscoR;
                currentColor.G = (byte)Game1.DiscoG;
                currentColor.B = (byte)Game1.DiscoB;
                currentColor.A = byte.MaxValue;
            }
            else if (ItemID.Sets.ItemIconPulse[type])
            {
                scale = Game1.essScale;
                currentColor.R = (byte)((double)currentColor.R * (double)scale);
                currentColor.G = (byte)((double)currentColor.G * (double)scale);
                currentColor.B = (byte)((double)currentColor.B * (double)scale);
                currentColor.A = (byte)((double)currentColor.A * (double)scale);
            }
            else if (type == 58 || type == 184)
            {
                scale = (float)((double)Game1.essScale * 0.25 + 0.75);
                currentColor.R = (byte)((double)currentColor.R * (double)scale);
                currentColor.G = (byte)((double)currentColor.G * (double)scale);
                currentColor.B = (byte)((double)currentColor.B * (double)scale);
                currentColor.A = (byte)((double)currentColor.A * (double)scale);
            }
            return currentColor;
        }

        public class Options
        {
            public static bool DisableLeftShiftTrashCan = false;
            public static bool HighlightNewItems = true;
        }

        public class Context
        {
            public const int InventoryItem = 0;
            public const int InventoryCoin = 1;
            public const int InventoryAmmo = 2;
            public const int ChestItem = 3;
            public const int BankItem = 4;
            public const int PrefixItem = 5;
            public const int TrashItem = 6;
            public const int GuideItem = 7;
            public const int EquipArmor = 8;
            public const int EquipArmorVanity = 9;
            public const int EquipAccessory = 10;
            public const int EquipAccessoryVanity = 11;
            public const int EquipDye = 12;
            public const int HotbarItem = 13;
            public const int ChatItem = 14;
            public const int ShopItem = 15;
            public const int EquipGrapple = 16;
            public const int EquipMount = 17;
            public const int EquipMinecart = 18;
            public const int EquipPet = 19;
            public const int EquipLight = 20;
            public const int MouseItem = 21;
            public const int CraftingMaterial = 22;
            public const int Count = 23;
        }
    }
}
