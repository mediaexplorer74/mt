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
using System;
using System.Collections.Generic;
using GameManager;
using GameManager.ID;

namespace GameManager.GameContent.UI
{
    public class EmoteBubble
    {
        private static int[] CountNPCs = new int[540];
        public static Dictionary<int, EmoteBubble> byID = new Dictionary<int, EmoteBubble>();
        private static List<int> toClean = new List<int>();
        private const int frameSpeed = 8;
        public static int NextID;
        public int ID;
        public WorldUIAnchor anchor;
        public int lifeTime;
        public int lifeTimeStart;
        public int emote;
        public int metadata;
        public int frameCounter;
        public int frame;

        public EmoteBubble(int emotion, WorldUIAnchor bubbleAnchor, int time = 180)
        {
            anchor = bubbleAnchor;
            emote = emotion;
            lifeTime = time;
            lifeTimeStart = time;
        }

        public static void UpdateAll()
        {
            lock (EmoteBubble.byID)
            {
                toClean.Clear();
                foreach (KeyValuePair<int, EmoteBubble> item_0 in byID)
                {
                    item_0.Value.Update();
                    if (item_0.Value.lifeTime <= 0)
                        toClean.Add(item_0.Key);
                }

                foreach (int item_1 in EmoteBubble.toClean)
                    byID.Remove(item_1);
                toClean.Clear();
            }
        }

        public static void DrawAll(SpriteBatch sb)
        {
            lock (byID)
            {
                foreach (KeyValuePair<int, EmoteBubble> item_0 in byID)
                    item_0.Value.Draw(sb);
            }
        }

        public static Tuple<int, int> SerializeNetAnchor(WorldUIAnchor anch)
        {
            if (anch.type != WorldUIAnchor.AnchorType.Entity)
                return Tuple.Create<int, int>(0, 0);
            int num = 0;
            if (anch.entity is NPC)
                num = 0;
            else if (anch.entity is Player)
                num = 1;
            else if (anch.entity is Projectile)
                num = 2;
            return Tuple.Create<int, int>(num, anch.entity.whoAmI);
        }

        public static WorldUIAnchor DeserializeNetAnchor(int type, int meta)
        {
            if (type == 0)
                return new WorldUIAnchor(Game1.npc[meta]);
            if (type == 1)
                return new WorldUIAnchor(Game1.player[meta]);
            if (type == 2)
                return new WorldUIAnchor(Game1.projectile[meta]);
            throw new Exception("How did you end up getting this?");
        }

        public static int AssignNewID()
        {
            return NextID++;
        }

        public static int NewBubble(int emoticon, WorldUIAnchor bubbleAnchor, int time)
        {
            EmoteBubble emoteBubble = new EmoteBubble(emoticon, bubbleAnchor, time);
            emoteBubble.ID = AssignNewID();
            byID[emoteBubble.ID] = emoteBubble;
            if (Game1.netMode == 2)
            {
                Tuple<int, int> tuple = SerializeNetAnchor(bubbleAnchor);
                NetMessage.SendData(91, -1, -1, "", emoteBubble.ID, (float)tuple.Item1, (float)tuple.Item2, (float)time, emoticon, 0, 0);
            }
            return emoteBubble.ID;
        }

        public static int NewBubbleNPC(WorldUIAnchor bubbleAnchor, int time, WorldUIAnchor other = null)
        {
            EmoteBubble emoteBubble = new EmoteBubble(0, bubbleAnchor, time);
            emoteBubble.ID = AssignNewID();
            byID[emoteBubble.ID] = emoteBubble;
            emoteBubble.PickNPCEmote(other);
            if (Game1.netMode == 2)
            {
                Tuple<int, int> tuple = SerializeNetAnchor(bubbleAnchor);
                NetMessage.SendData(91, -1, -1, "", emoteBubble.ID, (float)tuple.Item1, (float)tuple.Item2, (float)time, emoteBubble.emote, emoteBubble.metadata, 0);
            }
            return emoteBubble.ID;
        }

        private void Update()
        {
            if (--lifeTime <= 0 || ++frameCounter < 8)
                return;
            this.frameCounter = 0;
            if (++frame < 2)
                return;
            frame = 0;
        }

        private void Draw(SpriteBatch sb)
        {
            Texture2D texture2D = Game1.extraTexture[48];
            SpriteEffects effect = SpriteEffects.None;
            Vector2 vector2 = GetPosition(out effect);
            bool flag = lifeTime < 6 || lifeTimeStart - lifeTime < 6;
            Rectangle rectangle = Utils.Frame(texture2D, 8, 33, flag ? 0 : 1, 0);
            Vector2 origin = new Vector2((float)(rectangle.Width / 2), (float)rectangle.Height);
            if (Game1.player[Game1.myPlayer].gravDir == -1.0)
            {
                origin.Y = 0.0f;
                effect |= SpriteEffects.FlipVertically;
                vector2 = Game1.ReverseGravitySupport(vector2, 0.0f);
            }

            sb.Draw(texture2D, vector2, new Rectangle?(rectangle), Color.White, 0.0f, origin, 1f, effect, 0.0f);
            if (flag)
                return;

            if (emote >= 0)
            {
                if (emote == 87)
                    effect = SpriteEffects.None;
                sb.Draw(texture2D, vector2, new Rectangle?(Utils.Frame(texture2D, 8, 33, emote * 2 % 8 + frame, 1 + emote / 4)), Color.White, 0.0f, origin, 1f, effect, 0.0f);
            }
            else
            {
                if (this.emote != -1)
                    return;
                Texture2D texture = Game1.npcHeadTexture[metadata];
                float scale = 1f;
                if (texture.Width / 22.0 > 1.0)
                    scale = 22f / (float)texture.Width;
                if (texture.Height / 16.0 > 1.0 / scale)
                    scale = 16f / (float)texture.Height;
                sb.Draw(texture, vector2 + new Vector2(effect.HasFlag(SpriteEffects.FlipHorizontally) ? 1f : -1f, (float)(-rectangle.Height + 3)),
                    new Rectangle?(), Color.White, 0.0f, new Vector2((float)(texture.Width / 2), 0.0f), scale, effect, 0.0f);
            }
        }

        private Vector2 GetPosition(out SpriteEffects effect)
        {
            switch (this.anchor.type)
            {
                case WorldUIAnchor.AnchorType.Entity:
                    effect = anchor.entity.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                    return anchor.entity.Top + new Vector2((float)(-anchor.entity.direction * anchor.entity.width) * 0.75f, 2f) - Game1.screenPosition;
                case WorldUIAnchor.AnchorType.Tile:
                    effect = SpriteEffects.None;
                    return anchor.pos - Game1.screenPosition + new Vector2(0.0f, (float)(-anchor.size.Y / 2.0));
                case WorldUIAnchor.AnchorType.Pos:
                    effect = SpriteEffects.None;
                    return anchor.pos - Game1.screenPosition;
                default:
                    effect = SpriteEffects.None;
                    return new Vector2((float)Game1.screenWidth, (float)Game1.screenHeight) / 2f;
            }
        }

        public void PickNPCEmote(WorldUIAnchor other = null)
        {
            Player plr = Game1.player[(int)Player.FindClosest(anchor.entity.Center, 0, 0)];
            List<int> list = new List<int>();
            bool flag = false;
            for (int index = 0; index < 200; ++index)
            {
                if (Game1.npc[index].active && Game1.npc[index].boss)
                    flag = true;
            }

            if (!flag)
            {
                if (Game1.rand.Next(3) == 0)
                    ProbeTownNPCs(list);
                if (Game1.rand.Next(3) == 0)
                    ProbeEmotions(list);
                if (Game1.rand.Next(3) == 0)
                    ProbeBiomes(list, plr);
                if (Game1.rand.Next(2) == 0)
                    ProbeCritters(list);
                if (Game1.rand.Next(2) == 0)
                    ProbeItems(list, plr);
                if (Game1.rand.Next(5) == 0)
                    ProbeBosses(list);
                if (Game1.rand.Next(2) == 0)
                    ProbeDebuffs(list, plr);
                if (Game1.rand.Next(2) == 0)
                    ProbeEvents(list);
                if (Game1.rand.Next(2) == 0)
                    ProbeWeather(list, plr);
                ProbeExceptions(list, plr, other);
            }
            else
                ProbeCombat(list);

            if (list.Count <= 0)
                return;
            emote = list[Game1.rand.Next(list.Count)];
        }

        private void ProbeCombat(List<int> list)
        {
            list.Add(16);
            list.Add(1);
            list.Add(2);
            list.Add(91);
            list.Add(93);
            list.Add(84);
            list.Add(84);
        }

        private void ProbeWeather(List<int> list, Player plr)
        {
            if (Game1.cloudBGActive > 0.0)
                list.Add(96);
            if (Game1.cloudAlpha > 0.0)
            {
                if (!Game1.dayTime)
                    list.Add(5);
                list.Add(4);
                if (plr.ZoneSnow)
                    list.Add(98);
                if (plr.position.X < 4000.0 || plr.position.X > (Game1.maxTilesX * 16 - 4000) && plr.position.Y < Game1.worldSurface / 16.0)
                    list.Add(97);
            }
            else
                list.Add(95);

            if (!plr.ZoneHoly)
                return;
            list.Add(6);
        }

        private void ProbeEvents(List<int> list)
        {
            if (Game1.bloodMoon || !Game1.dayTime && Game1.rand.Next(4) == 0)
                list.Add(18);
            if (Game1.eclipse || Game1.hardMode && Game1.rand.Next(4) == 0)
                list.Add(19);
            if ((!Game1.dayTime || WorldGen.spawnMeteor) && WorldGen.shadowOrbSmashed)
                list.Add(99);
            if (Game1.pumpkinMoon || (NPC.downedHalloweenKing || NPC.downedHalloweenTree) && !Game1.dayTime)
                list.Add(20);
            if (!Game1.snowMoon && (!NPC.downedChristmasIceQueen && !NPC.downedChristmasSantank && !NPC.downedChristmasTree || Game1.dayTime))
                return;
            list.Add(21);
        }

        private void ProbeDebuffs(List<int> list, Player plr)
        {
            if (plr.Center.Y > (Game1.maxTilesY * 16 - 3200) || plr.onFire || (((NPC)anchor.entity).onFire || plr.onFire2))
                list.Add(9);
            if (Game1.rand.Next(2) == 0)
                list.Add(11);
            if (plr.poisoned || ((NPC)anchor.entity).poisoned || plr.ZoneJungle)
                list.Add(8);
            if (plr.inventory[plr.selectedItem].itemId != 215 && Game1.rand.Next(3) != 0)
                return;
            list.Add(10);
        }

        private void ProbeItems(List<int> list, Player plr)
        {
            list.Add(7);
            list.Add(73);
            list.Add(74);
            list.Add(75);
            list.Add(78);
            list.Add(90);
            if (plr.statLife >= plr.statLifeMax2 / 2)
                return;
            list.Add(84);
        }

        private void ProbeTownNPCs(List<int> list)
        {
            for (int index = 0; index < 540; ++index)
                EmoteBubble.CountNPCs[index] = 0;
            for (int index = 0; index < 200; ++index)
            {
                if (Game1.npc[index].active)
                    ++EmoteBubble.CountNPCs[Game1.npc[index].type];
            }

            int num = ((NPC)anchor.entity).type;
            for (int index = 0; index < 540; ++index)
            {
                if (NPCID.Sets.FaceEmote[index] > 0 && EmoteBubble.CountNPCs[index] > 0 && index != num)
                    list.Add(NPCID.Sets.FaceEmote[index]);
            }
        }

        private void ProbeBiomes(List<int> list, Player plr)
        {
            if (plr.position.Y / 16.0 < Game1.worldSurface * 0.45)
                list.Add(22);
            else if (plr.position.Y / 16.0 > Game1.rockLayer + (Game1.maxTilesY / 2) - 100.0)
                list.Add(31);
            else if (plr.position.Y / 16.0 > Game1.rockLayer)
                list.Add(30);
            else if (plr.ZoneHoly)
                list.Add(27);
            else if (plr.ZoneCorrupt)
                list.Add(26);
            else if (plr.ZoneCrimson)
                list.Add(25);
            else if (plr.ZoneJungle)
                list.Add(24);
            else if (plr.ZoneSnow)
                list.Add(32);
            else if (plr.position.Y / 16.0 < Game1.worldSurface && (plr.position.X < 4000.0 || plr.position.X > (16 * (Game1.maxTilesX - 250))))
                list.Add(29);
            else if (plr.ZoneDesert)
                list.Add(28);
            else
                list.Add(23);
        }

        private void ProbeCritters(List<int> list)
        {
            Vector2 center = this.anchor.entity.Center;
            float num1 = 1f;
            float num2 = 1f;
            if (center.Y < Game1.rockLayer * 16.0)
                num2 = 0.2f;
            else
                num1 = 0.2f;

            if (Utils.NextFloat(Game1.rand) <= num1)
            {
                if (Game1.dayTime)
                {
                    list.Add(13);
                    list.Add(12);
                    list.Add(68);
                    list.Add(62);
                    list.Add(63);
                    list.Add(69);
                    list.Add(70);
                }

                if (!Game1.dayTime || Game1.dayTime && (Game1.time < 5400.0 || Game1.time > 48600.0))
                    list.Add(61);
                if (NPC.downedGoblins)
                    list.Add(64);
                if (NPC.downedFrost)
                    list.Add(66);
                if (NPC.downedPirates)
                    list.Add(65);
                if (NPC.downedMartians)
                    list.Add(71);
                if (WorldGen.crimson)
                    list.Add(67);
            }

            if (Utils.NextFloat(Game1.rand) > num2)
                return;
            list.Add(72);
            list.Add(69);
        }

        private void ProbeEmotions(List<int> list)
        {
            list.Add(0);
            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(15);
            list.Add(16);
            list.Add(17);
            list.Add(87);
            list.Add(91);
            if (!Game1.bloodMoon || Game1.dayTime)
                return;
            int num = Utils.SelectRandom<int>(Game1.rand, 16, 1);
            list.Add(num);
            list.Add(num);
            list.Add(num);
        }

        private void ProbeBosses(List<int> list)
        {
            int num = 0;
            if (!NPC.downedBoss1 && !Game1.dayTime || NPC.downedBoss1)
                num = 1;
            if (NPC.downedBoss2)
                num = 2;
            if (NPC.downedQueenBee || NPC.downedBoss3)
                num = 3;
            if (Game1.hardMode)
                num = 4;
            if (NPC.downedMechBossAny)
                num = 5;
            if (NPC.downedPlantBoss)
                num = 6;
            if (NPC.downedGolemBoss)
                num = 7;
            if (NPC.downedAncientCultist)
                num = 8;
            int maxValue = 10;
            if (NPC.downedMoonlord)
                maxValue = 1;
            if (num >= 1 && num <= 2 || num >= 1 && Game1.rand.Next(maxValue) == 0)
            {
                list.Add(39);
                if (WorldGen.crimson)
                    list.Add(41);
                else
                    list.Add(40);
                list.Add(51);
            }

            if (num >= 2 && num <= 3 || num >= 2 && Game1.rand.Next(maxValue) == 0)
            {
                list.Add(43);
                list.Add(42);
            }

            if (num >= 4 && num <= 5 || num >= 4 && Game1.rand.Next(maxValue) == 0)
            {
                list.Add(44);
                list.Add(47);
                list.Add(45);
                list.Add(46);
            }

            if (num >= 5 && num <= 6 || num >= 5 && Game1.rand.Next(maxValue) == 0)
            {
                if (!NPC.downedMechBoss1)
                    list.Add(47);
                if (!NPC.downedMechBoss2)
                    list.Add(45);
                if (!NPC.downedMechBoss3)
                    list.Add(46);
                list.Add(48);
            }

            if (num == 6 || num >= 6 && Game1.rand.Next(maxValue) == 0)
            {
                list.Add(48);
                list.Add(49);
                list.Add(50);
            }

            if (num == 7 || num >= 7 && Game1.rand.Next(maxValue) == 0)
            {
                list.Add(49);
                list.Add(50);
                list.Add(52);
            }

            if (num == 8 || num >= 8 && Game1.rand.Next(maxValue) == 0)
            {
                list.Add(52);
                list.Add(53);
            }

            if (NPC.downedPirates && Game1.expertMode)
                list.Add(59);
            if (NPC.downedMartians)
                list.Add(60);
            if (NPC.downedChristmasIceQueen)
                list.Add(57);
            if (NPC.downedChristmasSantank)
                list.Add(58);
            if (NPC.downedChristmasTree)
                list.Add(56);
            if (NPC.downedHalloweenKing)
                list.Add(55);
            if (!NPC.downedHalloweenTree)
                return;
            list.Add(54);
        }

        private void ProbeExceptions(List<int> list, Player plr, WorldUIAnchor other)
        {
            NPC npc = (NPC)anchor.entity;
            if (npc.type == 17)
            {
                list.Add(80);
                list.Add(85);
                list.Add(85);
                list.Add(85);
                list.Add(85);
            }
            else if (npc.type == 18)
            {
                list.Add(73);
                list.Add(73);
                list.Add(84);
                list.Add(75);
            }
            else if (npc.type == 19)
            {
                if (other != null && ((NPC)other.entity).type == 22)
                {
                    list.Add(1);
                    list.Add(1);
                    list.Add(93);
                    list.Add(92);
                }
                else if (other != null && ((NPC)other.entity).type == 22)
                {
                    list.Add(1);
                    list.Add(1);
                    list.Add(93);
                    list.Add(92);
                }
                else
                {
                    list.Add(82);
                    list.Add(82);
                    list.Add(85);
                    list.Add(85);
                    list.Add(77);
                    list.Add(93);
                }
            }
            else if (npc.type == 20)
            {
                if (list.Contains(121))
                {
                    list.Add(121);
                    list.Add(121);
                }
                list.Add(14);
                list.Add(14);
            }
            else if (npc.type == 22)
            {
                if (!Game1.bloodMoon)
                {
                    if (other != null && ((NPC)other.entity).type == 19)
                    {
                        list.Add(1);
                        list.Add(1);
                        list.Add(93);
                        list.Add(92);
                    }
                    else
                        list.Add(79);
                }
                if (!Game1.dayTime)
                {
                    list.Add(16);
                    list.Add(16);
                    list.Add(16);
                }
            }
            else if (npc.type == 37)
            {
                list.Add(43);
                list.Add(43);
                list.Add(43);
                list.Add(72);
                list.Add(72);
            }
            else if (npc.type == 38)
            {
                if (Game1.bloodMoon)
                {
                    list.Add(77);
                    list.Add(77);
                    list.Add(77);
                    list.Add(81);
                }
                else
                {
                    list.Add(77);
                    list.Add(77);
                    list.Add(81);
                    list.Add(81);
                    list.Add(81);
                    list.Add(90);
                    list.Add(90);
                }
            }
            else if (npc.type == 54)
            {
                if (Game1.bloodMoon)
                {
                    list.Add(43);
                    list.Add(72);
                    list.Add(1);
                }
                else
                {
                    if (list.Contains(111))
                        list.Add(111);
                    list.Add(17);
                }
            }
            else if (npc.type == 107)
            {
                if (other != null && ((NPC)other.entity).type == 124)
                {
                    list.Remove(111);
                    list.Add(0);
                    list.Add(0);
                    list.Add(0);
                    list.Add(17);
                    list.Add(17);
                    list.Add(86);
                    list.Add(88);
                    list.Add(88);
                }
                else
                {
                    if (list.Contains(111))
                    {
                        list.Add(111);
                        list.Add(111);
                        list.Add(111);
                    }
                    list.Add(91);
                    list.Add(92);
                    list.Add(91);
                    list.Add(92);
                }
            }
            else if (npc.type == 108)
            {
                list.Add(100);
                list.Add(89);
                list.Add(11);
            }

            if (npc.type == 124)
            {
                if (other != null && ((NPC)other.entity).type == 107)
                {
                    list.Remove(111);
                    list.Add(0);
                    list.Add(0);
                    list.Add(0);
                    list.Add(17);
                    list.Add(17);
                    list.Add(88);
                    list.Add(88);
                }
                else
                {
                    if (list.Contains(109))
                    {
                        list.Add(109);
                        list.Add(109);
                        list.Add(109);
                    }
                    if (list.Contains(108))
                    {
                        list.Remove(108);
                        if (Game1.hardMode)
                        {
                            list.Add(108);
                            list.Add(108);
                        }
                        else
                        {
                            list.Add(106);
                            list.Add(106);
                        }
                    }
                    list.Add(43);
                    list.Add(2);
                }
            }
            else if (npc.type == 142)
            {
                list.Add(32);
                list.Add(66);
                list.Add(17);
                list.Add(15);
                list.Add(15);
            }
            else if (npc.type == 160)
            {
                list.Add(10);
                list.Add(89);
                list.Add(94);
                list.Add(8);
            }
            else if (npc.type == 178)
            {
                list.Add(83);
                list.Add(83);
            }
            else if (npc.type == 207)
            {
                list.Add(28);
                list.Add(95);
                list.Add(93);
            }
            else if (npc.type == 208)
            {
                list.Add(94);
                list.Add(17);
                list.Add(3);
                list.Add(77);
            }
            else if (npc.type == 209)
            {
                list.Add(48);
                list.Add(83);
                list.Add(5);
                list.Add(5);
            }
            else if (npc.type == 227)
            {
                list.Add(63);
                list.Add(68);
            }
            else if (npc.type == 228)
            {
                list.Add(24);
                list.Add(24);
                list.Add(95);
                list.Add(8);
            }
            else if (npc.type == 229)
            {
                list.Add(93);
                list.Add(9);
                list.Add(65);
                list.Add(120);
                list.Add(59);
            }
            else if (npc.type == 353)
            {
                if (list.Contains(104))
                {
                    list.Add(104);
                    list.Add(104);
                }
                if (list.Contains(111))
                {
                    list.Add(111);
                    list.Add(111);
                }
                list.Add(67);
            }
            else if (npc.type == 368)
            {
                list.Add(85);
                list.Add(7);
                list.Add(79);
            }
            else if (npc.type == 369)
            {
                if (Game1.bloodMoon)
                    return;
                list.Add(70);
                list.Add(70);
                list.Add(76);
                list.Add(76);
                list.Add(79);
                list.Add(79);
                if (npc.position.Y >= Game1.worldSurface)
                    return;
                list.Add(29);
            }
            else if (npc.type == 453)
            {
                list.Add(72);
                list.Add(69);
                list.Add(87);
                list.Add(3);
            }
            else
            {
                if (npc.type != 441)
                    return;
                list.Add(100);
                list.Add(100);
                list.Add(1);
                list.Add(1);
                list.Add(1);
                list.Add(87);
            }
        }
    }
}
