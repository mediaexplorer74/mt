// AchievementsHelper

using System;
using GameManager;

namespace GameManager.GameContent.Achievements
{
    public class AchievementsHelper
    {
        private static bool _isMining = false;
        private static bool mayhemOK;
        private static bool mayhem1down;
        private static bool mayhem2down;
        private static bool mayhem3down;

        public static bool CurrentlyMining
        {
            get { return _isMining; }
            set { _isMining = value; }
        }

        public delegate void ItemPickupEvent(Player player, short itemId, int count);
        public delegate void ItemCraftEvent(short itemId, int count);
        public delegate void TileDestroyedEvent(Player player, ushort itemId);
        public delegate void NPCKilledEvent(Player player, short npcId);
        public delegate void ProgressionEventEvent(int eventID);
        public static event ItemPickupEvent OnItemPickup;
        public static event ItemCraftEvent OnItemCraft;
        public static event TileDestroyedEvent OnTileDestroyed;
        public static event NPCKilledEvent OnNPCKilled;
        public static event ProgressionEventEvent OnProgressionEvent;

        public static void NotifyTileDestroyed(Player player, ushort tile)
        {
            if (Game1.gameMenu || !_isMining || OnTileDestroyed == null)
                return;

            OnTileDestroyed(player, tile);
        }

        public static void NotifyItemPickup(Player player, Item item)
        {
            if (OnItemPickup == null)
                return;

            OnItemPickup(player, (short)item.netID, item.stack);
        }

        public static void NotifyItemPickup(Player player, Item item, int customStack)
        {
            if (OnItemPickup == null)
                return;

            OnItemPickup(player, (short)item.netID, customStack);
        }

        public static void NotifyItemCraft(Recipe recipe)
        {
            if (OnItemCraft == null)
                return;

            OnItemCraft((short)recipe.createItem.netID, recipe.createItem.stack);
        }

        public static void Initialize()
        {
            Player.OnEnterWorld += new Action<Player>(OnPlayerEnteredWorld);
        }

        private static void OnPlayerEnteredWorld(Player player)
        {
            if (OnItemPickup != null)
            {
                for (int index = 0; index < 58; ++index)
                    OnItemPickup(player, (short)player.inventory[index].itemId, player.inventory[index].stack);
                for (int index = 0; index < player.armor.Length; ++index)
                    OnItemPickup(player, (short)player.armor[index].itemId, player.armor[index].stack);
                for (int index = 0; index < player.dye.Length; ++index)
                    OnItemPickup(player, (short)player.dye[index].itemId, player.dye[index].stack);
                for (int index = 0; index < player.miscEquips.Length; ++index)
                    OnItemPickup(player, (short)player.miscEquips[index].itemId, player.miscEquips[index].stack);
                for (int index = 0; index < player.miscDyes.Length; ++index)
                    OnItemPickup(player, (short)player.miscDyes[index].itemId, player.miscDyes[index].stack);
                for (int index = 0; index < player.bank.item.Length; ++index)
                    OnItemPickup(player, (short)player.bank.item[index].itemId, player.bank.item[index].stack);
                for (int index = 0; index < player.bank2.item.Length; ++index)
                    OnItemPickup(player, (short)player.bank2.item[index].itemId, player.bank2.item[index].stack);
            }

            if (player.statManaMax > 20)
                Game1.Achievements.GetCondition("STAR_POWER", "Use").Complete();
            if (player.statLifeMax == 500 && player.statManaMax == 200)
                Game1.Achievements.GetCondition("TOPPED_OFF", "Use").Complete();
            if (player.miscEquips[4].itemId > 0)
                Game1.Achievements.GetCondition("HOLD_ON_TIGHT", "Equip").Complete();
            if (player.miscEquips[3].itemId > 0)
                Game1.Achievements.GetCondition("THE_CAVALRY", "Equip").Complete();

            for (int index = 0; index < player.armor.Length; ++index)
            {
                if ((int)player.armor[index].wingSlot > 0)
                {
                    Game1.Achievements.GetCondition("HEAD_IN_THE_CLOUDS", "Equip").Complete();
                    break;
                }
            }

            if (player.armor[0].stack > 0 && player.armor[1].stack > 0 && player.armor[2].stack > 0)
                Game1.Achievements.GetCondition("MATCHING_ATTIRE", "Equip").Complete();
            if (player.armor[10].stack > 0 && player.armor[11].stack > 0 && player.armor[12].stack > 0)
                Game1.Achievements.GetCondition("FASHION_STATEMENT", "Equip").Complete();

            bool flag = true;
            for (int index = 0; index < player.extraAccessorySlots + 3 + 5; ++index)
            {
                if (player.dye[index].itemId < 1 || player.dye[index].stack < 1)
                    flag = false;
            }

            if (!flag)
                return;

            Game1.Achievements.GetCondition("DYE_HARD", "Equip").Complete();
        }

        public static void NotifyNPCKilled(NPC npc)
        {
            if (Game1.netMode == 0)
            {
                if (!npc.playerInteraction[Game1.myPlayer])
                    return;

                NotifyNPCKilledDirect(Game1.player[Game1.myPlayer], npc.netID);
            }
            else
            {
                for (int remoteClient = 0; remoteClient < (int)byte.MaxValue; ++remoteClient)
                {
                    if (npc.playerInteraction[remoteClient])
                        NetMessage.SendData(97, remoteClient, -1, "", npc.netID, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                }
            }
        }

        public static void NotifyNPCKilledDirect(Player player, int npcNetID)
        {
            if (OnNPCKilled == null)
                return;

            OnNPCKilled(player, (short)npcNetID);
        }

        public static void NotifyProgressionEvent(int eventID)
        {
            if (OnProgressionEvent == null)
                return;

            if (Game1.netMode == 2)
                NetMessage.SendData(98, -1, -1, "", eventID, 0.0f, 0.0f, 0.0f, 0, 0, 0);
            else
                OnProgressionEvent(eventID);
        }

        public static void HandleOnEquip(Player player, Item item, int context)
        {
            if (context == 16)
                Game1.Achievements.GetCondition("HOLD_ON_TIGHT", "Equip").Complete();
            if (context == 17)
                Game1.Achievements.GetCondition("THE_CAVALRY", "Equip").Complete();
            if ((context == 10 || context == 11) && (int)item.wingSlot > 0)
                Game1.Achievements.GetCondition("HEAD_IN_THE_CLOUDS", "Equip").Complete();
            if (context == 8 && player.armor[0].stack > 0 && (player.armor[1].stack > 0 && player.armor[2].stack > 0))
                Game1.Achievements.GetCondition("MATCHING_ATTIRE", "Equip").Complete();
            if (context == 9 && player.armor[10].stack > 0 && (player.armor[11].stack > 0 && player.armor[12].stack > 0))
                Game1.Achievements.GetCondition("FASHION_STATEMENT", "Equip").Complete();

            if (context != 12)
                return;

            for (int index = 0; index < player.extraAccessorySlots + 3 + 5; ++index)
            {
                if (player.dye[index].itemId < 1 || player.dye[index].stack < 1)
                    return;
            }

            Game1.Achievements.GetCondition("DYE_HARD", "Equip").Complete();
        }

        public static void HandleSpecialEvent(Player player, int eventID)
        {
            if (player.whoAmI != Game1.myPlayer)
                return;
            switch (eventID)
            {
                case 1:
                    Game1.Achievements.GetCondition("STAR_POWER", "Use").Complete();
                    if (player.statLifeMax != 500 || player.statManaMax != 200)
                        break;
                    Game1.Achievements.GetCondition("TOPPED_OFF", "Use").Complete();
                    break;
                case 2:
                    Game1.Achievements.GetCondition("GET_A_LIFE", "Use").Complete();
                    if (player.statLifeMax != 500 || player.statManaMax != 200)
                        break;
                    Game1.Achievements.GetCondition("TOPPED_OFF", "Use").Complete();
                    break;
                case 3:
                    Game1.Achievements.GetCondition("NOT_THE_BEES", "Use").Complete();
                    break;
                case 4:
                    Game1.Achievements.GetCondition("WATCH_YOUR_STEP", "Hit").Complete();
                    break;
                case 5:
                    Game1.Achievements.GetCondition("RAINBOWS_AND_UNICORNS", "Use").Complete();
                    break;
                case 6:
                    Game1.Achievements.GetCondition("YOU_AND_WHAT_ARMY", "Spawn").Complete();
                    break;
                case 7:
                    Game1.Achievements.GetCondition("THROWING_LINES", "Use").Complete();
                    break;
                case 8:
                    Game1.Achievements.GetCondition("LUCKY_BREAK", "Hit").Complete();
                    break;
                case 9:
                    Game1.Achievements.GetCondition("VEHICULAR_MANSLAUGHTER", "Hit").Complete();
                    break;
                case 10:
                    Game1.Achievements.GetCondition("ROCK_BOTTOM", "Reach").Complete();
                    break;
                case 11:
                    Game1.Achievements.GetCondition("INTO_ORBIT", "Reach").Complete();
                    break;
                case 12:
                    Game1.Achievements.GetCondition("WHERES_MY_HONEY", "Reach").Complete();
                    break;
                case 13:
                    Game1.Achievements.GetCondition("JEEPERS_CREEPERS", "Reach").Complete();
                    break;
                case 14:
                    Game1.Achievements.GetCondition("ITS_GETTING_HOT_IN_HERE", "Reach").Complete();
                    break;
                case 15:
                    Game1.Achievements.GetCondition("FUNKYTOWN", "Reach").Complete();
                    break;
                case 16:
                    Game1.Achievements.GetCondition("I_AM_LOOT", "Peek").Complete();
                    break;
            }
        }

        public static void HandleNurseService(int coinsSpent)
        {
            ((CustomFloatCondition)Game1.Achievements.GetCondition("FREQUENT_FLYER", "Pay")).Value += (float)coinsSpent;
        }

        public static void HandleAnglerService()
        {
            Game1.Achievements.GetCondition("SERVANT_IN_TRAINING", "Finish").Complete();
            ++((CustomFloatCondition)Game1.Achievements.GetCondition("GOOD_LITTLE_SLAVE", "Finish")).Value;
            ++((CustomFloatCondition)Game1.Achievements.GetCondition("TROUT_MONKEY", "Finish")).Value;
            ++((CustomFloatCondition)Game1.Achievements.GetCondition("FAST_AND_FISHIOUS", "Finish")).Value;
            ++((CustomFloatCondition)Game1.Achievements.GetCondition("SUPREME_HELPER_MINION", "Finish")).Value;
        }

        public static void HandleRunning(float pixelsMoved)
        {
            ((CustomFloatCondition)Game1.Achievements.GetCondition("MARATHON_MEDALIST", "Move")).Value += pixelsMoved;
        }

        public static void HandleMining()
        {
            ++((CustomIntCondition)Game1.Achievements.GetCondition("BULLDOZER", "Pick")).Value;
        }

        public static void CheckMechaMayhem(int justKilled = -1)
        {
            if (!mayhemOK)
            {
                if (!NPC.AnyNPCs(127) || !NPC.AnyNPCs(134) || (!NPC.AnyNPCs(126) || !NPC.AnyNPCs(125)))
                    return;

                mayhemOK = true;
                mayhem1down = false;
                mayhem2down = false;
                mayhem3down = false;
            }
            else
            {
                if (justKilled == 125 || justKilled == 126)
                    mayhem1down = true;
                else if (!NPC.AnyNPCs(125) && !NPC.AnyNPCs(126) && !mayhem1down)
                {
                    mayhemOK = false;
                    return;
                }

                if (justKilled == 134)
                    mayhem2down = true;
                else if (!NPC.AnyNPCs(134) && !mayhem2down)
                {
                    mayhemOK = false;
                    return;
                }

                if (justKilled == 127)
                    mayhem3down = true;
                else if (!NPC.AnyNPCs(127) && !mayhem3down)
                {
                    mayhemOK = false;
                    return;
                }

                if (!mayhem1down || !mayhem2down || !mayhem3down)
                    return;

                NotifyProgressionEvent(21);
            }
        }
    }
}
