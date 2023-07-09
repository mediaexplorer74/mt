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
using System;
using System.Collections.Generic;
using System.IO;
using GameManager.DataStructures;
using GameManager.GameContent;
using GameManager.GameContent.Achievements;
using GameManager.GameContent.Tile_Entities;
using GameManager.GameContent.UI;
using GameManager.GameContent.UI.Chat;
using GameManager.ID;
using GameManager.Net;
using GameManager.UI;
using System.Diagnostics;

namespace GameManager
{
    public class MessageBuffer
    {
        public byte[] readBuffer = new byte[131070];
        public byte[] writeBuffer = new byte[131070];
        public const int readBufferMax = 131070;
        public const int writeBufferMax = 131070;
        public bool broadcast;
        public bool writeLocked;
        public int messageLength;
        public int totalData;
        public int whoAmI;
        public int spamCount;
        public int maxSpam;
        public bool checkBytes;
        public MemoryStream readerStream;
        public MemoryStream writerStream;
        public BinaryReader reader;
        public BinaryWriter writer;

        public void Reset()
        {
            this.readBuffer = new byte[131070];
            this.writeBuffer = new byte[131070];
            this.writeLocked = false;
            this.messageLength = 0;
            this.totalData = 0;
            this.spamCount = 0;
            this.broadcast = false;
            this.checkBytes = false;
            this.ResetReader();
            this.ResetWriter();
        }

        public void ResetReader()
        {
            if (this.readerStream != null)
            {
                //this.readerStream.Close();
                this.readerStream.Dispose();
            }

            this.readerStream = new MemoryStream(this.readBuffer);
            this.reader = new BinaryReader((Stream)this.readerStream);
        }

        public void ResetWriter()
        {
            if (this.writerStream != null)
            {
                //this.writerStream.Close();
                this.writerStream.Dispose();
            }

            this.writerStream = new MemoryStream(this.writeBuffer);
            this.writer = new BinaryWriter((Stream)this.writerStream);
        }

        public void GetData(int start, int length, out int messageType)
        {
            if (this.whoAmI < 256)
                Netplay.Clients[this.whoAmI].TimeOutTimer = 0;
            else
                Netplay.Connection.TimeOutTimer = 0;
            int bufferStart = start + 1;
            byte num1 = this.readBuffer[start];
            messageType = (int)num1;
            if ((int)num1 >= 105)
                return;
            ++Game1.rxMsg;
            Game1.rxData += length;
            ++Game1.rxMsgType[(int)num1];
            Game1.rxDataType[(int)num1] += length;
            if (Game1.netMode == 1 && Netplay.Connection.StatusMax > 0)
                ++Netplay.Connection.StatusCount;
            if (Game1.verboseNetplay)
            {
                int num2 = start;
                while (num2 < start + length)
                    ++num2;
                for (int index = start; index < start + length; ++index)
                {
                    int num3 = (int)this.readBuffer[index];
                }
            }
            if (Game1.netMode == 2 && (int)num1 != 38 && Netplay.Clients[this.whoAmI].State == -1)
            {
                NetMessage.SendData(2, this.whoAmI, -1, Lang.mp[1], 0, 0.0f, 0.0f, 0.0f, 0, 0, 0);
            }
            else
            {
                if (Game1.netMode == 2 && Netplay.Clients[this.whoAmI].State < 10 && ((int)num1 > 12 && (int)num1 != 93) && ((int)num1 != 16 && (int)num1 != 42 && ((int)num1 != 50 && (int)num1 != 38)) && (int)num1 != 68)
                    NetMessage.BootPlayer(this.whoAmI, Lang.mp[2]);
                if (this.reader == null)
                    this.ResetReader();
                this.reader.BaseStream.Position = (long)bufferStart;
                switch (num1)
                {
                    case (byte)1:
                        if (Game1.netMode != 2)
                            break;
                        if (Game1.dedServ && Netplay.IsBanned(Netplay.Clients[this.whoAmI].Socket.GetRemoteAddress()))
                        {
                            NetMessage.SendData(2, this.whoAmI, -1, Lang.mp[3], 0, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                            break;
                        }
                        if (Netplay.Clients[this.whoAmI].State != 0)
                            break;
                        if (this.reader.ReadString() == "Terraria" + (object)Game1.curRelease)
                        {
                            if (string.IsNullOrEmpty(Netplay.ServerPassword))
                            {
                                Netplay.Clients[this.whoAmI].State = 1;
                                NetMessage.SendData(3, this.whoAmI, -1, "", 0, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                                break;
                            }
                            Netplay.Clients[this.whoAmI].State = -1;
                            NetMessage.SendData(37, this.whoAmI, -1, "", 0, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                            break;
                        }
                        NetMessage.SendData(2, this.whoAmI, -1, Lang.mp[4], 0, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        break;
                    case (byte)2:
                        if (Game1.netMode != 1)
                            break;
                        Netplay.disconnect = true;
                        Game1.statusText = this.reader.ReadString();
                        break;
                    case (byte)3:
                        if (Game1.netMode != 1)
                            break;
                        if (Netplay.Connection.State == 1)
                            Netplay.Connection.State = 2;
                        int number1 = (int)this.reader.ReadByte();
                        if (number1 != Game1.myPlayer)
                        {
                            Game1.player[number1] = Game1.ActivePlayerFileData.Player;
                            Game1.player[Game1.myPlayer] = new Player();
                        }
                        Game1.player[number1].whoAmI = number1;
                        Game1.myPlayer = number1;
                        Player player1 = Game1.player[number1];
                        NetMessage.SendData(4, -1, -1, player1.name, number1, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        NetMessage.SendData(68, -1, -1, "", number1, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        NetMessage.SendData(16, -1, -1, "", number1, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        NetMessage.SendData(42, -1, -1, "", number1, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        NetMessage.SendData(50, -1, -1, "", number1, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        for (int index = 0; index < 59; ++index)
                            NetMessage.SendData(5, -1, -1, player1.inventory[index].name, number1, (float)index, (float)player1.inventory[index].prefix, 0.0f, 0, 0, 0);
                        for (int index = 0; index < player1.armor.Length; ++index)
                            NetMessage.SendData(5, -1, -1, player1.armor[index].name, number1, (float)(59 + index), (float)player1.armor[index].prefix, 0.0f, 0, 0, 0);
                        for (int index = 0; index < player1.dye.Length; ++index)
                            NetMessage.SendData(5, -1, -1, player1.dye[index].name, number1, (float)(58 + player1.armor.Length + 1 + index), (float)player1.dye[index].prefix, 0.0f, 0, 0, 0);
                        for (int index = 0; index < player1.miscEquips.Length; ++index)
                            NetMessage.SendData(5, -1, -1, "", number1, (float)(58 + player1.armor.Length + player1.dye.Length + 1 + index), (float)player1.miscEquips[index].prefix, 0.0f, 0, 0, 0);
                        for (int index = 0; index < player1.miscDyes.Length; ++index)
                            NetMessage.SendData(5, -1, -1, "", number1, (float)(58 + player1.armor.Length + player1.dye.Length + player1.miscEquips.Length + 1 + index), (float)player1.miscDyes[index].prefix, 0.0f, 0, 0, 0);
                        for (int index = 0; index < player1.bank.item.Length; ++index)
                            NetMessage.SendData(5, -1, -1, "", number1, (float)(58 + player1.armor.Length + player1.dye.Length + player1.miscEquips.Length + player1.miscDyes.Length + 1 + index), (float)player1.bank.item[index].prefix, 0.0f, 0, 0, 0);
                        for (int index = 0; index < player1.bank2.item.Length; ++index)
                            NetMessage.SendData(5, -1, -1, "", number1, (float)(58 + player1.armor.Length + player1.dye.Length + player1.miscEquips.Length + player1.miscDyes.Length + player1.bank.item.Length + 1 + index), (float)player1.bank2.item[index].prefix, 0.0f, 0, 0, 0);
                        NetMessage.SendData(5, -1, -1, "", number1, (float)(58 + player1.armor.Length + player1.dye.Length + player1.miscEquips.Length + player1.miscDyes.Length + player1.bank.item.Length + player1.bank2.item.Length + 1), (float)player1.trashItem.prefix, 0.0f, 0, 0, 0);
                        NetMessage.SendData(6, -1, -1, "", 0, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        if (Netplay.Connection.State != 2)
                            break;
                        Netplay.Connection.State = 3;
                        break;
                    case (byte)4:
                        int number2 = (int)this.reader.ReadByte();
                        if (Game1.netMode == 2)
                            number2 = this.whoAmI;
                        if (number2 == Game1.myPlayer && !Game1.ServerSideCharacter)
                            break;
                        Player player2 = Game1.player[number2];
                        player2.whoAmI = number2;
                        player2.skinVariant = (int)this.reader.ReadByte();
                        player2.skinVariant = (int)MathHelper.Clamp((float)player2.skinVariant, 0.0f, 7f);
                        player2.hair = (int)this.reader.ReadByte();
                        if (player2.hair >= 134)
                            player2.hair = 0;
                        player2.name = this.reader.ReadString().Trim().Trim();
                        player2.hairDye = this.reader.ReadByte();
                        BitsByte bitsByte1 = (BitsByte)this.reader.ReadByte();
                        for (int index = 0; index < 8; ++index)
                            player2.hideVisual[index] = bitsByte1[index];
                        bitsByte1 = (BitsByte)this.reader.ReadByte();
                        for (int index = 0; index < 2; ++index)
                            player2.hideVisual[index + 8] = bitsByte1[index];
                        player2.hideMisc = (BitsByte)this.reader.ReadByte();
                        player2.hairColor = Utils.ReadRGB(this.reader);
                        player2.skinColor = Utils.ReadRGB(this.reader);
                        player2.eyeColor = Utils.ReadRGB(this.reader);
                        player2.shirtColor = Utils.ReadRGB(this.reader);
                        player2.underShirtColor = Utils.ReadRGB(this.reader);
                        player2.pantsColor = Utils.ReadRGB(this.reader);
                        player2.shoeColor = Utils.ReadRGB(this.reader);
                        BitsByte bitsByte2 = (BitsByte)this.reader.ReadByte();
                        player2.difficulty = (byte)0;
                        if (bitsByte2[0])
                            ++player2.difficulty;
                        if (bitsByte2[1])
                            player2.difficulty += (byte)2;
                        if ((int)player2.difficulty > 2)
                            player2.difficulty = (byte)2;
                        player2.extraAccessory = bitsByte2[2];
                        if (Game1.netMode != 2)
                            break;
                        bool flag1 = false;
                        if (Netplay.Clients[this.whoAmI].State < 10)
                        {
                            for (int index = 0; index < (int)byte.MaxValue; ++index)
                            {
                                if (index != number2 && player2.name == Game1.player[index].name && Netplay.Clients[index].IsActive)
                                    flag1 = true;
                            }
                        }
                        if (flag1)
                        {
                            NetMessage.SendData(2, this.whoAmI, -1, player2.name + " " + Lang.mp[5], 0, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                            break;
                        }
                        if (player2.name.Length > Player.nameLen)
                        {
                            NetMessage.SendData(2, this.whoAmI, -1, "Name is too long.", 0, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                            break;
                        }
                        if (player2.name == "")
                        {
                            NetMessage.SendData(2, this.whoAmI, -1, "Empty name.", 0, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                            break;
                        }
                        Netplay.Clients[this.whoAmI].Name = player2.name;
                        Netplay.Clients[this.whoAmI].Name = player2.name;
                        NetMessage.SendData(4, -1, this.whoAmI, player2.name, number2, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        break;
                    case (byte)5:
                        int number3 = (int)this.reader.ReadByte();
                        if (Game1.netMode == 2)
                            number3 = this.whoAmI;
                        if (number3 == Game1.myPlayer && !Game1.ServerSideCharacter && !Game1.player[number3].IsStackingItems())
                            break;
                        Player player3 = Game1.player[number3];
                        lock (player3)
                        {
                            int local_24 = (int)this.reader.ReadByte();
                            int local_25 = (int)this.reader.ReadInt16();
                            int local_26 = (int)this.reader.ReadByte();
                            int local_27 = (int)this.reader.ReadInt16();
                            Item[] local_28 = (Item[])null;
                            int local_29 = 0;
                            bool local_30 = false;
                            if (local_24 > 58 + player3.armor.Length + player3.dye.Length + player3.miscEquips.Length + player3.miscDyes.Length + player3.bank.item.Length + player3.bank2.item.Length)
                                local_30 = true;
                            else if (local_24 > 58 + player3.armor.Length + player3.dye.Length + player3.miscEquips.Length + player3.miscDyes.Length + player3.bank.item.Length)
                            {
                                local_29 = local_24 - 58 - (player3.armor.Length + player3.dye.Length + player3.miscEquips.Length + player3.miscDyes.Length + player3.bank.item.Length) - 1;
                                local_28 = player3.bank2.item;
                            }
                            else if (local_24 > 58 + player3.armor.Length + player3.dye.Length + player3.miscEquips.Length + player3.miscDyes.Length)
                            {
                                local_29 = local_24 - 58 - (player3.armor.Length + player3.dye.Length + player3.miscEquips.Length + player3.miscDyes.Length) - 1;
                                local_28 = player3.bank.item;
                            }
                            else if (local_24 > 58 + player3.armor.Length + player3.dye.Length + player3.miscEquips.Length)
                            {
                                local_29 = local_24 - 58 - (player3.armor.Length + player3.dye.Length + player3.miscEquips.Length) - 1;
                                local_28 = player3.miscDyes;
                            }
                            else if (local_24 > 58 + player3.armor.Length + player3.dye.Length)
                            {
                                local_29 = local_24 - 58 - (player3.armor.Length + player3.dye.Length) - 1;
                                local_28 = player3.miscEquips;
                            }
                            else if (local_24 > 58 + player3.armor.Length)
                            {
                                local_29 = local_24 - 58 - player3.armor.Length - 1;
                                local_28 = player3.dye;
                            }
                            else if (local_24 > 58)
                            {
                                local_29 = local_24 - 58 - 1;
                                local_28 = player3.armor;
                            }
                            else
                            {
                                local_29 = local_24;
                                local_28 = player3.inventory;
                            }
                            if (local_30)
                            {
                                player3.trashItem = new Item();
                                player3.trashItem.netDefaults(local_27);
                                player3.trashItem.stack = local_25;
                                player3.trashItem.Prefix(local_26);
                            }
                            else if (local_24 <= 58)
                            {
                                int local_31 = local_28[local_29].itemId;
                                int local_32 = local_28[local_29].stack;
                                local_28[local_29] = new Item();
                                local_28[local_29].netDefaults(local_27);
                                local_28[local_29].stack = local_25;
                                local_28[local_29].Prefix(local_26);
                                if (number3 == Game1.myPlayer && local_29 == 58)
                                    Game1.mouseItem = local_28[local_29].Clone();
                                if (number3 == Game1.myPlayer && Game1.netMode == 1)
                                {
                                    Game1.player[number3].inventoryChestStack[local_24] = false;
                                    if (local_28[local_29].stack != local_32 || local_28[local_29].itemId != local_31)
                                        Game1.PlaySound(7, -1, -1, 1);
                                }
                            }
                            else
                            {
                                local_28[local_29] = new Item();
                                local_28[local_29].netDefaults(local_27);
                                local_28[local_29].stack = local_25;
                                local_28[local_29].Prefix(local_26);
                            }
                            if (Game1.netMode != 2 || number3 != this.whoAmI)
                                break;
                            NetMessage.SendData(5, -1, this.whoAmI, "", number3, (float)local_24, (float)local_26, 0.0f, 0, 0, 0);
                            break;
                        }
                    case (byte)6:
                        if (Game1.netMode != 2)
                            break;
                        if (Netplay.Clients[this.whoAmI].State == 1)
                        {
                            Netplay.Clients[this.whoAmI].State = 2;
                            Netplay.Clients[this.whoAmI].ResetSections();
                        }
                        NetMessage.SendData(7, this.whoAmI, -1, "", 0, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        Game1.SyncAnInvasion(this.whoAmI);
                        break;
                    case (byte)7:
                        if (Game1.netMode != 1)
                            break;
                        Game1.time = (double)this.reader.ReadInt32();
                        BitsByte bitsByte3 = (BitsByte)this.reader.ReadByte();
                        Game1.dayTime = bitsByte3[0];
                        Game1.bloodMoon = bitsByte3[1];
                        Game1.eclipse = bitsByte3[2];
                        Game1.moonPhase = (int)this.reader.ReadByte();
                        Game1.maxTilesX = (int)this.reader.ReadInt16();
                        Game1.maxTilesY = (int)this.reader.ReadInt16();
                        Game1.spawnTileX = (int)this.reader.ReadInt16();
                        Game1.spawnTileY = (int)this.reader.ReadInt16();
                        Game1.worldSurface = (double)this.reader.ReadInt16();
                        Game1.rockLayer = (double)this.reader.ReadInt16();
                        Game1.worldID = this.reader.ReadInt32();
                        Game1.worldName = this.reader.ReadString();
                        Game1.moonType = (int)this.reader.ReadByte();
                        WorldGen.setBG(0, (int)this.reader.ReadByte());
                        WorldGen.setBG(1, (int)this.reader.ReadByte());
                        WorldGen.setBG(2, (int)this.reader.ReadByte());
                        WorldGen.setBG(3, (int)this.reader.ReadByte());
                        WorldGen.setBG(4, (int)this.reader.ReadByte());
                        WorldGen.setBG(5, (int)this.reader.ReadByte());
                        WorldGen.setBG(6, (int)this.reader.ReadByte());
                        WorldGen.setBG(7, (int)this.reader.ReadByte());
                        Game1.iceBackStyle = (int)this.reader.ReadByte();
                        Game1.jungleBackStyle = (int)this.reader.ReadByte();
                        Game1.hellBackStyle = (int)this.reader.ReadByte();
                        Game1.windSpeedSet = this.reader.ReadSingle();
                        Game1.numClouds = (int)this.reader.ReadByte();
                        for (int index = 0; index < 3; ++index)
                            Game1.treeX[index] = this.reader.ReadInt32();
                        for (int index = 0; index < 4; ++index)
                            Game1.treeStyle[index] = (int)this.reader.ReadByte();
                        for (int index = 0; index < 3; ++index)
                            Game1.caveBackX[index] = this.reader.ReadInt32();
                        for (int index = 0; index < 4; ++index)
                            Game1.caveBackStyle[index] = (int)this.reader.ReadByte();
                        Game1.maxRaining = this.reader.ReadSingle();
                        Game1.raining = (double)Game1.maxRaining > 0.0;
                        BitsByte bitsByte4 = (BitsByte)this.reader.ReadByte();
                        WorldGen.shadowOrbSmashed = bitsByte4[0];
                        NPC.downedBoss1 = bitsByte4[1];
                        NPC.downedBoss2 = bitsByte4[2];
                        NPC.downedBoss3 = bitsByte4[3];
                        Game1.hardMode = bitsByte4[4];
                        NPC.downedClown = bitsByte4[5];
                        Game1.ServerSideCharacter = bitsByte4[6];
                        NPC.downedPlantBoss = bitsByte4[7];
                        BitsByte bitsByte5 = (BitsByte)this.reader.ReadByte();
                        NPC.downedMechBoss1 = bitsByte5[0];
                        NPC.downedMechBoss2 = bitsByte5[1];
                        NPC.downedMechBoss3 = bitsByte5[2];
                        NPC.downedMechBossAny = bitsByte5[3];
                        Game1.cloudBGActive = bitsByte5[4] ? 1f : 0.0f;
                        WorldGen.crimson = bitsByte5[5];
                        Game1.pumpkinMoon = bitsByte5[6];
                        Game1.snowMoon = bitsByte5[7];
                        BitsByte bitsByte6 = (BitsByte)this.reader.ReadByte();
                        Game1.expertMode = bitsByte6[0];
                        Game1.fastForwardTime = bitsByte6[1];
                        Game1.UpdateSundial();
                        bool flag2 = bitsByte6[2];
                        NPC.downedSlimeKing = bitsByte6[3];
                        NPC.downedQueenBee = bitsByte6[4];
                        NPC.downedFishron = bitsByte6[5];
                        NPC.downedMartians = bitsByte6[6];
                        NPC.downedAncientCultist = bitsByte6[7];
                        BitsByte bitsByte7 = (BitsByte)this.reader.ReadByte();
                        NPC.downedMoonlord = bitsByte7[0];
                        NPC.downedHalloweenKing = bitsByte7[1];
                        NPC.downedHalloweenTree = bitsByte7[2];
                        NPC.downedChristmasIceQueen = bitsByte7[3];
                        NPC.downedChristmasSantank = bitsByte7[4];
                        NPC.downedChristmasTree = bitsByte7[5];
                        if (flag2)
                            Game1.StartSlimeRain(true);
                        else
                            Game1.StopSlimeRain(true);
                        Game1.invasionType = (int)this.reader.ReadSByte();
                        Game1.LobbyId = this.reader.ReadUInt64();
                        if (Netplay.Connection.State != 3)
                            break;
                        Netplay.Connection.State = 4;
                        break;
                    case (byte)8:
                        if (Game1.netMode != 2)
                            break;
                        int num2 = this.reader.ReadInt32();
                        int y1 = this.reader.ReadInt32();
                        bool flag3 = true;
                        if (num2 == -1 || y1 == -1)
                            flag3 = false;
                        else if (num2 < 10 || num2 > Game1.maxTilesX - 10)
                            flag3 = false;
                        else if (y1 < 10 || y1 > Game1.maxTilesY - 10)
                            flag3 = false;
                        int number4 = Netplay.GetSectionX(Game1.spawnTileX) - 2;
                        int num3 = Netplay.GetSectionY(Game1.spawnTileY) - 1;
                        int num4 = number4 + 5;
                        int num5 = num3 + 3;
                        if (number4 < 0)
                            number4 = 0;
                        if (num4 >= Game1.maxSectionsX)
                            num4 = Game1.maxSectionsX - 1;
                        if (num3 < 0)
                            num3 = 0;
                        if (num5 >= Game1.maxSectionsY)
                            num5 = Game1.maxSectionsY - 1;
                        int num6 = (num4 - number4) * (num5 - num3);
                        List<Point> dontInclude = new List<Point>();
                        for (int x = number4; x < num4; ++x)
                        {
                            for (int y2 = num3; y2 < num5; ++y2)
                                dontInclude.Add(new Point(x, y2));
                        }
                        int num7 = -1;
                        int num8 = -1;
                        if (flag3)
                        {
                            num2 = Netplay.GetSectionX(num2) - 2;
                            y1 = Netplay.GetSectionY(y1) - 1;
                            num7 = num2 + 5;
                            num8 = y1 + 3;
                            if (num2 < 0)
                                num2 = 0;
                            if (num7 >= Game1.maxSectionsX)
                                num7 = Game1.maxSectionsX - 1;
                            if (y1 < 0)
                                y1 = 0;
                            if (num8 >= Game1.maxSectionsY)
                                num8 = Game1.maxSectionsY - 1;
                            for (int x = num2; x < num7; ++x)
                            {
                                for (int y2 = y1; y2 < num8; ++y2)
                                {
                                    if (x < number4 || x >= num4 || (y2 < num3 || y2 >= num5))
                                    {
                                        dontInclude.Add(new Point(x, y2));
                                        ++num6;
                                    }
                                }
                            }
                        }
                        int num9 = 1;
                        List<Point> portals;
                        List<Point> portalCenters;
                        PortalHelper.SyncPortalsOnPlayerJoin(this.whoAmI, 1, dontInclude, out portals, out portalCenters);
                        int number5 = num6 + portals.Count;
                        if (Netplay.Clients[this.whoAmI].State == 2)
                            Netplay.Clients[this.whoAmI].State = 3;
                        NetMessage.SendData(9, this.whoAmI, -1, Lang.inter[44], number5, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        Netplay.Clients[this.whoAmI].StatusText2 = "is receiving tile data";
                        Netplay.Clients[this.whoAmI].StatusMax += number5;
                        for (int sectionX = number4; sectionX < num4; ++sectionX)
                        {
                            for (int sectionY = num3; sectionY < num5; ++sectionY)
                                NetMessage.SendSection(this.whoAmI, sectionX, sectionY, false);
                        }
                        NetMessage.SendData(11, this.whoAmI, -1, "", number4, (float)num3, (float)(num4 - 1), (float)(num5 - 1), 0, 0, 0);
                        if (flag3)
                        {
                            for (int sectionX = num2; sectionX < num7; ++sectionX)
                            {
                                for (int sectionY = y1; sectionY < num8; ++sectionY)
                                    NetMessage.SendSection(this.whoAmI, sectionX, sectionY, true);
                            }
                            NetMessage.SendData(11, this.whoAmI, -1, "", num2, (float)y1, (float)(num7 - 1), (float)(num8 - 1), 0, 0, 0);
                        }
                        for (int index = 0; index < portals.Count; ++index)
                            NetMessage.SendSection(this.whoAmI, portals[index].X, portals[index].Y, true);
                        for (int index = 0; index < portalCenters.Count; ++index)
                            NetMessage.SendData(11, this.whoAmI, -1, "", portalCenters[index].X - num9, (float)(portalCenters[index].Y - num9), (float)(portalCenters[index].X + num9 + 1), (float)(portalCenters[index].Y + num9 + 1), 0, 0, 0);
                        for (int number6 = 0; number6 < 400; ++number6)
                        {
                            if (Game1.item[number6].active)
                            {
                                NetMessage.SendData(21, this.whoAmI, -1, "", number6, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                                NetMessage.SendData(22, this.whoAmI, -1, "", number6, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                            }
                        }
                        for (int number6 = 0; number6 < 200; ++number6)
                        {
                            if (Game1.npc[number6].active)
                                NetMessage.SendData(23, this.whoAmI, -1, "", number6, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        }
                        for (int number6 = 0; number6 < 1000; ++number6)
                        {
                            if (Game1.projectile[number6].active && (Game1.projPet[Game1.projectile[number6].type] || Game1.projectile[number6].netImportant))
                                NetMessage.SendData(27, this.whoAmI, -1, "", number6, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        }
                        for (int number6 = 0; number6 < 251; ++number6)
                            NetMessage.SendData(83, this.whoAmI, -1, "", number6, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        NetMessage.SendData(49, this.whoAmI, -1, "", 0, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        NetMessage.SendData(57, this.whoAmI, -1, "", 0, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        NetMessage.SendData(7, this.whoAmI, -1, "", 0, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        NetMessage.SendData(103, -1, -1, "", NPC.MoonLordCountdown, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        NetMessage.SendData(101, this.whoAmI, -1, "", 0, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        break;
                    case (byte)9:
                        if (Game1.netMode != 1)
                            break;
                        Netplay.Connection.StatusMax += this.reader.ReadInt32();
                        Netplay.Connection.StatusText = this.reader.ReadString();
                        break;
                    case (byte)10:
                        if (Game1.netMode != 1)
                            break;
                        NetMessage.DecompressTileBlock(this.readBuffer, bufferStart, length);
                        break;
                    case (byte)11:
                        if (Game1.netMode != 1)
                            break;
                        WorldGen.SectionTileFrame((int)this.reader.ReadInt16(), (int)this.reader.ReadInt16(), (int)this.reader.ReadInt16(), (int)this.reader.ReadInt16());
                        break;
                    case (byte)12:
                        int index1 = (int)this.reader.ReadByte();
                        if (Game1.netMode == 2)
                            index1 = this.whoAmI;
                        Player player4 = Game1.player[index1];
                        player4.SpawnX = (int)this.reader.ReadInt16();
                        player4.SpawnY = (int)this.reader.ReadInt16();
                        player4.Spawn();
                        if (index1 == Game1.myPlayer && Game1.netMode != 2)
                        {
                            Game1.ActivePlayerFileData.StartPlayTimer();
                            Player.EnterWorld(Game1.player[Game1.myPlayer]);
                        }
                        if (Game1.netMode != 2 || Netplay.Clients[this.whoAmI].State < 3)
                            break;
                        if (Netplay.Clients[this.whoAmI].State == 3)
                        {
                            Netplay.Clients[this.whoAmI].State = 10;
                            NetMessage.greetPlayer(this.whoAmI);
                            NetMessage.buffer[this.whoAmI].broadcast = true;
                            NetMessage.syncPlayers();
                            NetMessage.SendData(12, -1, this.whoAmI, "", this.whoAmI, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                            NetMessage.SendData(74, this.whoAmI, -1, Game1.player[this.whoAmI].name, Game1.anglerQuest, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                            break;
                        }
                        NetMessage.SendData(12, -1, this.whoAmI, "", this.whoAmI, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        break;
                    case (byte)13:
                        int number7 = (int)this.reader.ReadByte();
                        if (number7 == Game1.myPlayer && !Game1.ServerSideCharacter)
                            break;
                        if (Game1.netMode == 2)
                            number7 = this.whoAmI;
                        Player player5 = Game1.player[number7];
                        BitsByte bitsByte8 = (BitsByte)this.reader.ReadByte();
                        player5.controlUp = bitsByte8[0];
                        player5.controlDown = bitsByte8[1];
                        player5.controlLeft = bitsByte8[2];
                        player5.controlRight = bitsByte8[3];
                        player5.controlJump = bitsByte8[4];
                        player5.controlUseItem = bitsByte8[5];
                        player5.direction = bitsByte8[6] ? 1 : -1;
                        BitsByte bitsByte9 = (BitsByte)this.reader.ReadByte();
                        if (bitsByte9[0])
                        {
                            player5.pulley = true;
                            player5.pulleyDir = bitsByte9[1] ? (byte)2 : (byte)1;
                        }
                        else
                            player5.pulley = false;
                        player5.selectedItem = (int)this.reader.ReadByte();
                        player5.position = Utils.ReadVector2(this.reader);
                        if (bitsByte9[2])
                            player5.velocity = Utils.ReadVector2(this.reader);
                        player5.vortexStealthActive = bitsByte9[3];
                        player5.gravDir = bitsByte9[4] ? 1f : -1f;
                        if (Game1.netMode != 2 || Netplay.Clients[this.whoAmI].State != 10)
                            break;
                        NetMessage.SendData(13, -1, this.whoAmI, "", number7, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        break;
                    case (byte)14:
                        if (Game1.netMode != 1)
                            break;
                        int index2 = (int)this.reader.ReadByte();
                        if ((int)this.reader.ReadByte() == 1)
                        {
                            if (!Game1.player[index2].active)
                                Game1.player[index2] = new Player();
                            Game1.player[index2].active = true;
                            break;
                        }
                        Game1.player[index2].active = false;
                        break;
                    case (byte)16:
                        int number8 = (int)this.reader.ReadByte();
                        if (number8 == Game1.myPlayer && !Game1.ServerSideCharacter)
                            break;
                        if (Game1.netMode == 2)
                            number8 = this.whoAmI;
                        Player player6 = Game1.player[number8];
                        player6.statLife = (int)this.reader.ReadInt16();
                        player6.statLifeMax = (int)this.reader.ReadInt16();
                        if (player6.statLifeMax < 100)
                            player6.statLifeMax = 100;
                        player6.dead = player6.statLife <= 0;
                        if (Game1.netMode != 2)
                            break;
                        NetMessage.SendData(16, -1, this.whoAmI, "", number8, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        break;
                    case (byte)17:
                        byte num10 = this.reader.ReadByte();
                        int index3 = (int)this.reader.ReadInt16();
                        int index4 = (int)this.reader.ReadInt16();
                        short num11 = this.reader.ReadInt16();
                        int num12 = (int)this.reader.ReadByte();
                        bool fail = (int)num11 == 1;
                        if (!WorldGen.InWorld(index3, index4, 3))
                            break;
                        if (Game1.tile[index3, index4] == null)
                            Game1.tile[index3, index4] = new Tile();
                        if (Game1.netMode == 2)
                        {
                            if (!fail)
                            {
                                if ((int)num10 == 0 || (int)num10 == 2 || (int)num10 == 4)
                                    ++Netplay.Clients[this.whoAmI].SpamDeleteBlock;
                                if ((int)num10 == 1 || (int)num10 == 3)
                                    ++Netplay.Clients[this.whoAmI].SpamAddBlock;
                            }
                            if (!Netplay.Clients[this.whoAmI].TileSections[Netplay.GetSectionX(index3), Netplay.GetSectionY(index4)])
                                fail = true;
                        }
                        if ((int)num10 == 0)
                            WorldGen.KillTile(index3, index4, fail, false, false);
                        if ((int)num10 == 1)
                            WorldGen.PlaceTile(index3, index4, (int)num11, false, true, -1, num12);
                        if ((int)num10 == 2)
                            WorldGen.KillWall(index3, index4, fail);
                        if ((int)num10 == 3)
                            WorldGen.PlaceWall(index3, index4, (int)num11, false);
                        if ((int)num10 == 4)
                            WorldGen.KillTile(index3, index4, fail, false, true);
                        if (num10 == 5 || num10 == 8 || num10 == 10 || num10 == 12)
                            WorldGen.PlaceWire(index3, index4, Tile.k_HACK_GetNetworkWireType(num10));
                        if (num10 == 6 || num10 == 9 || num10 == 11 || num10 == 13)
                            WorldGen.KillWire(index3, index4, Tile.k_HACK_GetNetworkWireType(num10));
                        if ((int)num10 == 7)
                            WorldGen.PoundTile(index3, index4);
                        if ((int)num10 == 14)
                            WorldGen.SlopeTile(index3, index4, (int)num11);
                        if ((int)num10 == 15)
                            Minecart.FrameTrack(index3, index4, true, false);
                        if (Game1.netMode != 2)
                            break;
                        NetMessage.SendData(17, -1, this.whoAmI, "", (int)num10, (float)index3, (float)index4, (float)num11, num12, 0, 0);
                        if ((int)num10 != 1 || (int)num11 != 53)
                            break;
                        NetMessage.SendTileSquare(-1, index3, index4, 1);
                        break;
                    case (byte)18:
                        if (Game1.netMode != 1)
                            break;
                        Game1.dayTime = (int)this.reader.ReadByte() == 1;
                        Game1.time = (double)this.reader.ReadInt32();
                        Game1.sunModY = this.reader.ReadInt16();
                        Game1.moonModY = this.reader.ReadInt16();
                        break;
                    case (byte)19:
                        byte num13 = this.reader.ReadByte();
                        int num14 = (int)this.reader.ReadInt16();
                        int num15 = (int)this.reader.ReadInt16();
                        if (!WorldGen.InWorld(num14, num15, 3))
                            break;
                        int direction1 = (int)this.reader.ReadByte() == 0 ? -1 : 1;
                        if ((int)num13 == 0)
                            WorldGen.OpenDoor(num14, num15, direction1);
                        else if ((int)num13 == 1)
                            WorldGen.CloseDoor(num14, num15, true);
                        else if ((int)num13 == 2)
                            WorldGen.ShiftTrapdoor(num14, num15, direction1 == 1, 1);
                        else if ((int)num13 == 3)
                            WorldGen.ShiftTrapdoor(num14, num15, direction1 == 1, 0);
                        else if ((int)num13 == 4)
                            WorldGen.ShiftTallGate(num14, num15, false);
                        else if ((int)num13 == 5)
                            WorldGen.ShiftTallGate(num14, num15, true);
                        if (Game1.netMode != 2)
                            break;
                        NetMessage.SendData(19, -1, this.whoAmI, "", (int)num13, (float)num14, (float)num15, direction1 == 1 ? 1f : 0.0f, 0, 0, 0);
                        break;
                    case (byte)20:
                        short num16 = this.reader.ReadInt16();
                        int num17 = (int)this.reader.ReadInt16();
                        int num18 = (int)this.reader.ReadInt16();
                        if (!WorldGen.InWorld(num17, num18, 3))
                            break;
                        BitsByte bitsByte10 = (BitsByte)(byte)0;
                        BitsByte bitsByte11 = (BitsByte)(byte)0;
                        for (int index5 = num17; index5 < num17 + (int)num16; ++index5)
                        {
                            for (int index6 = num18; index6 < num18 + (int)num16; ++index6)
                            {
                                if (Game1.tile[index5, index6] == null)
                                    Game1.tile[index5, index6] = new Tile();
                                Tile tile = Game1.tile[index5, index6];
                                bool flag4 = tile.active();
                                BitsByte bitsByte12 = (BitsByte)this.reader.ReadByte();
                                BitsByte bitsByte13 = (BitsByte)this.reader.ReadByte();
                                tile.active(bitsByte12[0]);
                                tile.wall = bitsByte12[2] ? (byte)1 : (byte)0;
                                bool flag5 = bitsByte12[3];
                                if (Game1.netMode != 2)
                                    tile.liquid = flag5 ? (byte)1 : (byte)0;
								tile.k_SetWireFlags(k_WireFlags.WIRE_RED, bitsByte12[4]);
                                tile.halfBrick(bitsByte12[5]);
								tile.k_SetWireFlags(k_WireFlags.WIRE_ACTUATOR, bitsByte12[6]);
								tile.inActive(bitsByte12[7]);
								tile.k_SetWireFlags(k_WireFlags.WIRE_GREEN, bitsByte13[0]);
								tile.k_SetWireFlags(k_WireFlags.WIRE_BLUE, bitsByte13[1]);
                                if (bitsByte13[2])
                                    tile.color(this.reader.ReadByte());
                                if (bitsByte13[3])
                                    tile.wallColor(this.reader.ReadByte());
                                if (tile.active())
                                {
                                    int num19 = (int)tile.type;
                                    tile.type = this.reader.ReadUInt16();
                                    if (Game1.tileFrameImportant[(int)tile.type])
                                    {
                                        tile.frameX = this.reader.ReadInt16();
                                        tile.frameY = this.reader.ReadInt16();
                                    }
                                    else if (!flag4 || (int)tile.type != num19)
                                    {
                                        tile.frameX = (short)-1;
                                        tile.frameY = (short)-1;
                                    }
                                    byte slope = (byte)0;
                                    if (bitsByte13[4])
                                        ++slope;
                                    if (bitsByte13[5])
                                        slope += (byte)2;
                                    if (bitsByte13[6])
                                        slope += (byte)4;
                                    tile.slope(slope);
                                }
                                if ((int)tile.wall > 0)
                                    tile.wall = this.reader.ReadByte();
                                if (flag5)
                                {
                                    tile.liquid = this.reader.ReadByte();
                                    tile.liquidType((int)this.reader.ReadByte());
                                }
                            }
                        }
                        WorldGen.RangeFrame(num17, num18, num17 + (int)num16, num18 + (int)num16);
                        if (Game1.netMode != 2)
                            break;
                        NetMessage.SendData((int)num1, -1, this.whoAmI, "", (int)num16, (float)num17, (float)num18, 0.0f, 0, 0, 0);
                        break;
                    case (byte)21:
                    case (byte)90:
                        int index7 = (int)this.reader.ReadInt16();
                        Vector2 vector2_1 = Utils.ReadVector2(this.reader);
                        Vector2 vector2_2 = Utils.ReadVector2(this.reader);
                        int Stack = (int)this.reader.ReadInt16();
                        int pre1 = (int)this.reader.ReadByte();
                        int num20 = (int)this.reader.ReadByte();
                        int type1 = (int)this.reader.ReadInt16();
                        if (Game1.netMode == 1)
                        {
                            if (type1 == 0)
                            {
                                Game1.item[index7].active = false;
                                break;
                            }
                            Item obj = Game1.item[index7];
                            bool flag4 = (obj.newAndShiny || obj.netID != type1) && ItemSlot.Options.HighlightNewItems;
                            obj.netDefaults(type1);
                            obj.newAndShiny = flag4;
                            obj.Prefix(pre1);
                            obj.stack = Stack;
                            obj.position = vector2_1;
                            obj.velocity = vector2_2;
                            obj.active = true;
                            if ((int)num1 == 90)
                            {
                                obj.instanced = true;
                                obj.owner = Game1.myPlayer;
                                obj.keepTime = 600;
                            }
                            obj.wet = Collision.WetCollision(obj.position, obj.width, obj.height);
                            break;
                        }
                        if (Game1.itemLockoutTime[index7] > 0)
                            break;
                        if (type1 == 0)
                        {
                            if (index7 >= 400)
                                break;
                            Game1.item[index7].active = false;
                            NetMessage.SendData(21, -1, -1, "", index7, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                            break;
                        }
                        bool flag6 = false;
                        if (index7 == 400)
                            flag6 = true;
                        if (flag6)
                        {
                            Item obj = new Item();
                            obj.netDefaults(type1);
                            index7 = Item.NewItem((int)vector2_1.X, (int)vector2_1.Y, obj.width, obj.height, obj.itemId, Stack, true, 0, false);
                        }
                        Item obj1 = Game1.item[index7];
                        obj1.netDefaults(type1);
                        obj1.Prefix(pre1);
                        obj1.stack = Stack;
                        obj1.position = vector2_1;
                        obj1.velocity = vector2_2;
                        obj1.active = true;
                        obj1.owner = Game1.myPlayer;
                        if (flag6)
                        {
                            NetMessage.SendData(21, -1, -1, "", index7, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                            if (num20 == 0)
                            {
                                Game1.item[index7].ownIgnore = this.whoAmI;
                                Game1.item[index7].ownTime = 100;
                            }
                            Game1.item[index7].FindOwner(index7);
                            break;
                        }
                        NetMessage.SendData(21, -1, this.whoAmI, "", index7, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        break;
                    case (byte)22:
                        int number9 = (int)this.reader.ReadInt16();
                        int num21 = (int)this.reader.ReadByte();
                        if (Game1.netMode == 2 && Game1.item[number9].owner != this.whoAmI)
                            break;
                        Game1.item[number9].owner = num21;
                        Game1.item[number9].keepTime = num21 != Game1.myPlayer ? 0 : 15;
                        if (Game1.netMode != 2)
                            break;
                        Game1.item[number9].owner = (int)byte.MaxValue;
                        Game1.item[number9].keepTime = 15;
                        NetMessage.SendData(22, -1, -1, "", number9, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        break;
                    case (byte)23:
                        if (Game1.netMode != 1)
                            break;
                        int index8 = (int)this.reader.ReadInt16();
                        Vector2 vector2_3 = Utils.ReadVector2(this.reader);
                        Vector2 vector2_4 = Utils.ReadVector2(this.reader);
                        int num22 = (int)this.reader.ReadByte();
                        BitsByte bitsByte14 = (BitsByte)this.reader.ReadByte();
                        float[] numArray1 = new float[NPC.maxAI];
                        for (int index5 = 0; index5 < NPC.maxAI; ++index5)
                            numArray1[index5] = !bitsByte14[index5 + 2] ? 0.0f : this.reader.ReadSingle();
                        int type2 = (int)this.reader.ReadInt16();
                        int num23 = 0;
                        if (!bitsByte14[7])
                        {
                            switch (this.reader.ReadByte())
                            {
                                case (byte)2:
                                    num23 = (int)this.reader.ReadInt16();
                                    break;
                                case (byte)4:
                                    num23 = this.reader.ReadInt32();
                                    break;
                                default:
                                    num23 = (int)this.reader.ReadSByte();
                                    break;
                            }
                        }
                        int oldType = -1;
                        NPC npc1 = Game1.npc[index8];
                        if (!npc1.active || npc1.netID != type2)
                        {
                            if (npc1.active)
                                oldType = npc1.type;
                            npc1.active = true;
                            npc1.netDefaults(type2);
                        }
                        npc1.position = vector2_3;
                        npc1.velocity = vector2_4;
                        npc1.target = num22;
                        npc1.direction = bitsByte14[0] ? 1 : -1;
                        npc1.directionY = bitsByte14[1] ? 1 : -1;
                        npc1.spriteDirection = bitsByte14[6] ? 1 : -1;
                        if (bitsByte14[7])
                            num23 = npc1.life = npc1.lifeMax;
                        else
                            npc1.life = num23;
                        if (num23 <= 0)
                            npc1.active = false;
                        for (int index5 = 0; index5 < NPC.maxAI; ++index5)
                            npc1.ai[index5] = numArray1[index5];
                        if (oldType > -1 && oldType != npc1.type)
                            npc1.TransformVisuals(oldType, npc1.type);
                        if (type2 == 262)
                            NPC.plantBoss = index8;
                        if (type2 == 245)
                            NPC.golemBoss = index8;
                        if (!Game1.npcCatchable[npc1.type])
                            break;
                        npc1.releaseOwner = (short)this.reader.ReadByte();
                        break;
                    case (byte)24:
                        int number10 = (int)this.reader.ReadInt16();
                        int index9 = (int)this.reader.ReadByte();
                        if (Game1.netMode == 2)
                            index9 = this.whoAmI;
                        Player player7 = Game1.player[index9];
                        Game1.npc[number10].StrikeNPC(player7.inventory[player7.selectedItem].damage, player7.inventory[player7.selectedItem].knockBack, player7.direction, false, false, false);
                        if (Game1.netMode != 2)
                            break;
                        NetMessage.SendData(24, -1, this.whoAmI, "", number10, (float)index9, 0.0f, 0.0f, 0, 0, 0);
                        NetMessage.SendData(23, -1, -1, "", number10, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        break;
                    case (byte)25:
                        int number11 = (int)this.reader.ReadByte();
                        if (Game1.netMode == 2)
                            number11 = this.whoAmI;
                        Color color1 = Utils.ReadRGB(this.reader);
                        if (Game1.netMode == 2)
                            color1 = new Color((int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue);
                        string str1 = this.reader.ReadString();
                        if (Game1.netMode == 1)
                        {
                            string newText = str1;
                            if (number11 < (int)byte.MaxValue)
                            {
                                newText = NameTagHandler.GenerateTag(Game1.player[number11].name) + " " + str1;
                                Game1.player[number11].chatOverhead.NewMessage(str1, Game1.chatLength / 2);
                            }
                            Game1.NewText(newText, color1.R, color1.G, color1.B, false);
                            break;
                        }
                        if (Game1.netMode != 2)
                            break;
                        string str2 = str1.ToLower();
                        if (str2 == Lang.mp[6] || str2 == Lang.mp[21])
                        {
                            string str3 = "";
                            for (int index5 = 0; index5 < (int)byte.MaxValue; ++index5)
                            {
                                if (Game1.player[index5].active)
                                    str3 = !(str3 == "") ? str3 + ", " + Game1.player[index5].name : Game1.player[index5].name;
                            }
                            NetMessage.SendData(25, this.whoAmI, -1, Lang.mp[7] + " " + str3 + ".", (int)byte.MaxValue, (float)byte.MaxValue, 240f, 20f, 0, 0, 0);
                            break;
                        }
                        if (str2.StartsWith("/me "))
                        {
                            NetMessage.SendData(25, -1, -1, "*" + Game1.player[this.whoAmI].name + " " + str1.Substring(4), (int)byte.MaxValue, 200f, 100f, 0.0f, 0, 0, 0);
                            break;
                        }
                        if (str2 == Lang.mp[8])
                        {
                            NetMessage.SendData(25, -1, -1, "*" + (object)Game1.player[this.whoAmI].name + " " + Lang.mp[9] + " " + (string)(object)Game1.rand.Next(1, 101), (int)byte.MaxValue, (float)byte.MaxValue, 240f, 20f, 0, 0, 0);
                            break;
                        }
                        if (str2.StartsWith("/p "))
                        {
                            int index5 = Game1.player[this.whoAmI].team;
                            Color color2 = Game1.teamColor[index5];
                            if (index5 != 0)
                            {
                                for (int remoteClient = 0; remoteClient < (int)byte.MaxValue; ++remoteClient)
                                {
                                    if (Game1.player[remoteClient].team == index5)
                                        NetMessage.SendData(25, remoteClient, -1, str1.Substring(3), number11, (float)color2.R, (float)color2.G, (float)color2.B, 0, 0, 0);
                                }
                                break;
                            }
                            NetMessage.SendData(25, this.whoAmI, -1, Lang.mp[10], (int)byte.MaxValue, (float)byte.MaxValue, 240f, 20f, 0, 0, 0);
                            break;
                        }
                        if ((int)Game1.player[this.whoAmI].difficulty == 2)
                            color1 = Game1.hcColor;
                        else if ((int)Game1.player[this.whoAmI].difficulty == 1)
                            color1 = Game1.mcColor;
                        NetMessage.SendData(25, -1, -1, str1, number11, (float)color1.R, (float)color1.G, (float)color1.B, 0, 0, 0);
                        if (!Game1.dedServ)
                            break;
                        Debug.WriteLine("<" + Game1.player[this.whoAmI].name + "> " + str1);
                        break;
                    case (byte)26:
                        int number12 = (int)this.reader.ReadByte();
                        if (Game1.netMode == 2 && this.whoAmI != number12 && (!Game1.player[number12].hostile || !Game1.player[this.whoAmI].hostile))
                            break;
                        int hitDirection1 = (int)this.reader.ReadByte() - 1;
                        int Damage1 = (int)this.reader.ReadInt16();
                        string str4 = this.reader.ReadString();
                        BitsByte bitsByte15 = (BitsByte)this.reader.ReadByte();
                        bool pvp = bitsByte15[0];
                        bool Crit = bitsByte15[1];
                        Game1.player[number12].Hurt(Damage1, hitDirection1, pvp, true, str4, Crit);
                        if (Game1.netMode != 2)
                            break;
                        NetMessage.SendData(26, -1, this.whoAmI, str4, number12, (float)hitDirection1, (float)Damage1, pvp ? 1f : 0.0f, Crit ? 1 : 0, 0, 0);
                        break;
                    case (byte)27:
                        int num24 = (int)this.reader.ReadInt16();
                        Vector2 vector2_5 = Utils.ReadVector2(this.reader);
                        Vector2 vector2_6 = Utils.ReadVector2(this.reader);
                        float num25 = this.reader.ReadSingle();
                        int num26 = (int)this.reader.ReadInt16();
                        int own = (int)this.reader.ReadByte();
                        int Type1 = (int)this.reader.ReadInt16();
                        BitsByte bitsByte16 = (BitsByte)this.reader.ReadByte();
                        float[] numArray2 = new float[Projectile.maxAI];
                        for (int index5 = 0; index5 < Projectile.maxAI; ++index5)
                            numArray2[index5] = !bitsByte16[index5] ? 0.0f : this.reader.ReadSingle();
                        if (Game1.netMode == 2)
                        {
                            own = this.whoAmI;
                            if (Game1.projHostile[Type1])
                                break;
                        }
                        int number13 = 1000;
                        for (int index5 = 0; index5 < 1000; ++index5)
                        {
                            if (Game1.projectile[index5].owner == own && Game1.projectile[index5].identity == num24 && Game1.projectile[index5].active)
                            {
                                number13 = index5;
                                break;
                            }
                        }
                        if (number13 == 1000)
                        {
                            for (int index5 = 0; index5 < 1000; ++index5)
                            {
                                if (!Game1.projectile[index5].active)
                                {
                                    number13 = index5;
                                    break;
                                }
                            }
                        }
                        Projectile projectile1 = Game1.projectile[number13];
                        if (!projectile1.active || projectile1.type != Type1)
                        {
                            projectile1.SetDefaults(Type1);
                            if (Game1.netMode == 2)
                                ++Netplay.Clients[this.whoAmI].SpamProjectile;
                        }
                        projectile1.identity = num24;
                        projectile1.position = vector2_5;
                        projectile1.velocity = vector2_6;
                        projectile1.type = Type1;
                        projectile1.damage = num26;
                        projectile1.knockBack = num25;
                        projectile1.owner = own;
                        for (int index5 = 0; index5 < Projectile.maxAI; ++index5)
                            projectile1.ai[index5] = numArray2[index5];
                        projectile1.ProjectileFixDesperation(own);
                        if (Game1.netMode != 2)
                            break;
                        NetMessage.SendData(27, -1, this.whoAmI, "", number13, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        break;
                    case (byte)28:
                        int number14 = (int)this.reader.ReadInt16();
                        int Damage2 = (int)this.reader.ReadInt16();
                        float num27 = this.reader.ReadSingle();
                        int hitDirection2 = (int)this.reader.ReadByte() - 1;
                        byte num28 = this.reader.ReadByte();
                        if (Game1.netMode == 2)
                            Game1.npc[number14].PlayerInteraction(this.whoAmI);
                        if (Damage2 >= 0)
                        {
                            Game1.npc[number14].StrikeNPC(Damage2, num27, hitDirection2, (int)num28 == 1, false, true);
                        }
                        else
                        {
                            Game1.npc[number14].life = 0;
                            Game1.npc[number14].HitEffect(0, 10.0);
                            Game1.npc[number14].active = false;
                        }
                        if (Game1.netMode != 2)
                            break;
                        NetMessage.SendData(28, -1, this.whoAmI, "", number14, (float)Damage2, num27, (float)hitDirection2, (int)num28, 0, 0);
                        if (Game1.npc[number14].life <= 0)
                        {
                            NetMessage.SendData(23, -1, -1, "", number14, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                            break;
                        }
                        Game1.npc[number14].netUpdate = true;
                        break;
                    case (byte)29:
                        int number15 = (int)this.reader.ReadInt16();
                        int num29 = (int)this.reader.ReadByte();
                        if (Game1.netMode == 2)
                            num29 = this.whoAmI;
                        for (int index5 = 0; index5 < 1000; ++index5)
                        {
                            if (Game1.projectile[index5].owner == num29 && Game1.projectile[index5].identity == number15 && Game1.projectile[index5].active)
                            {
                                Game1.projectile[index5].Kill();
                                break;
                            }
                        }
                        if (Game1.netMode != 2)
                            break;
                        NetMessage.SendData(29, -1, this.whoAmI, "", number15, (float)num29, 0.0f, 0.0f, 0, 0, 0);
                        break;
                    case (byte)30:
                        int number16 = (int)this.reader.ReadByte();
                        if (Game1.netMode == 2)
                            number16 = this.whoAmI;
                        bool flag7 = this.reader.ReadBoolean();
                        Game1.player[number16].hostile = flag7;
                        if (Game1.netMode != 2)
                            break;
                        NetMessage.SendData(30, -1, this.whoAmI, "", number16, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        string str5 = " " + Lang.mp[flag7 ? 11 : 12];
                        Color color3 = Game1.teamColor[Game1.player[number16].team];
                        NetMessage.SendData(25, -1, -1, Game1.player[number16].name + str5, (int)byte.MaxValue, (float)color3.R, (float)color3.G, (float)color3.B, 0, 0, 0);
                        break;
                    case (byte)31:
                        if (Game1.netMode != 2)
                            break;
                        int X1 = (int)this.reader.ReadInt16();
                        int Y1 = (int)this.reader.ReadInt16();
                        int chest1 = Chest.FindChest(X1, Y1);
                        if (chest1 <= -1 || Chest.UsingChest(chest1) != -1)
                            break;
                        for (int index5 = 0; index5 < 40; ++index5)
                            NetMessage.SendData(32, this.whoAmI, -1, "", chest1, (float)index5, 0.0f, 0.0f, 0, 0, 0);
                        NetMessage.SendData(33, this.whoAmI, -1, "", chest1, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        Game1.player[this.whoAmI].chest = chest1;
                        if (Game1.myPlayer == this.whoAmI)
                            Game1.recBigList = false;
                        Recipe.FindRecipes();
                        NetMessage.SendData(80, -1, this.whoAmI, "", this.whoAmI, (float)chest1, 0.0f, 0.0f, 0, 0, 0);
                        if ((int)Game1.tile[X1, Y1].frameX < 36 || (int)Game1.tile[X1, Y1].frameX >= 72)
                            break;
                        AchievementsHelper.HandleSpecialEvent(Game1.player[this.whoAmI], 16);
                        break;
                    case (byte)32:
                        int index10 = (int)this.reader.ReadInt16();
                        int index11 = (int)this.reader.ReadByte();
                        int num30 = (int)this.reader.ReadInt16();
                        int pre2 = (int)this.reader.ReadByte();
                        int type3 = (int)this.reader.ReadInt16();
                        if (Game1.chest[index10] == null)
                            Game1.chest[index10] = new Chest(false);
                        if (Game1.chest[index10].item[index11] == null)
                            Game1.chest[index10].item[index11] = new Item();
                        Game1.chest[index10].item[index11].netDefaults(type3);
                        Game1.chest[index10].item[index11].Prefix(pre2);
                        Game1.chest[index10].item[index11].stack = num30;
                        Recipe.FindRecipes();
                        break;
                    case (byte)33:
                        int num31 = (int)this.reader.ReadInt16();
                        int num32 = (int)this.reader.ReadInt16();
                        int num33 = (int)this.reader.ReadInt16();
                        int num34 = (int)this.reader.ReadByte();
                        string text1 = string.Empty;
                        if (num34 != 0)
                        {
                            if (num34 <= 20)
                                text1 = this.reader.ReadString();
                            else if (num34 != (int)byte.MaxValue)
                                num34 = 0;
                        }
                        if (Game1.netMode == 1)
                        {
                            Player player8 = Game1.player[Game1.myPlayer];
                            if (player8.chest == -1)
                            {
                                Game1.playerInventory = true;
                                Game1.PlaySound(10, -1, -1, 1);
                            }
                            else if (player8.chest != num31 && num31 != -1)
                            {
                                Game1.playerInventory = true;
                                Game1.PlaySound(12, -1, -1, 1);
                                Game1.recBigList = false;
                            }
                            else if (player8.chest != -1 && num31 == -1)
                            {
                                Game1.PlaySound(11, -1, -1, 1);
                                Game1.recBigList = false;
                            }
                            player8.chest = num31;
                            player8.chestX = num32;
                            player8.chestY = num33;
                            Recipe.FindRecipes();
                            break;
                        }
                        if (num34 != 0)
                        {
                            int number6 = Game1.player[this.whoAmI].chest;
                            Chest chest2 = Game1.chest[number6];
                            chest2.name = text1;
                            NetMessage.SendData(69, -1, this.whoAmI, text1, number6, (float)chest2.x, (float)chest2.y, 0.0f, 0, 0, 0);
                        }
                        Game1.player[this.whoAmI].chest = num31;
                        Recipe.FindRecipes();
                        NetMessage.SendData(80, -1, this.whoAmI, "", this.whoAmI, (float)num31, 0.0f, 0.0f, 0, 0, 0);
                        break;
                    case (byte)34:
                        byte num35 = this.reader.ReadByte();
                        int index12 = (int)this.reader.ReadInt16();
                        int index13 = (int)this.reader.ReadInt16();
                        int style1 = (int)this.reader.ReadInt16();
                        if (Game1.netMode == 2)
                        {
                            if ((int)num35 == 0)
                            {
                                int number5_1 = WorldGen.PlaceChest(index12, index13, (ushort)21, false, style1);
                                if (number5_1 == -1)
                                {
                                    NetMessage.SendData(34, this.whoAmI, -1, "", (int)num35, (float)index12, (float)index13, (float)style1, number5_1, 0, 0);
                                    Item.NewItem(index12 * 16, index13 * 16, 32, 32, Chest.chestItemSpawn[style1], 1, true, 0, false);
                                    break;
                                }
                                NetMessage.SendData(34, -1, -1, "", (int)num35, (float)index12, (float)index13, (float)style1, number5_1, 0, 0);
                                break;
                            }
                            if ((int)num35 == 2)
                            {
                                int number5_1 = WorldGen.PlaceChest(index12, index13, (ushort)88, false, style1);
                                if (number5_1 == -1)
                                {
                                    NetMessage.SendData(34, this.whoAmI, -1, "", (int)num35, (float)index12, (float)index13, (float)style1, number5_1, 0, 0);
                                    Item.NewItem(index12 * 16, index13 * 16, 32, 32, Chest.dresserItemSpawn[style1], 1, true, 0, false);
                                    break;
                                }
                                NetMessage.SendData(34, -1, -1, "", (int)num35, (float)index12, (float)index13, (float)style1, number5_1, 0, 0);
                                break;
                            }
                            Tile tile = Game1.tile[index12, index13];
                            if ((int)tile.type == 21 && (int)num35 == 1)
                            {
                                if ((int)tile.frameX % 36 != 0)
                                    --index12;
                                if ((int)tile.frameY % 36 != 0)
                                    --index13;
                                int chest2 = Chest.FindChest(index12, index13);
                                WorldGen.KillTile(index12, index13, false, false, false);
                                if (tile.active())
                                    break;
                                NetMessage.SendData(34, -1, -1, "", (int)num35, (float)index12, (float)index13, 0.0f, chest2, 0, 0);
                                break;
                            }
                            if ((int)tile.type != 88 || (int)num35 != 3)
                                break;
                            int num19 = index12 - (int)tile.frameX % 54 / 18;
                            if ((int)tile.frameY % 36 != 0)
                                --index13;
                            int chest3 = Chest.FindChest(num19, index13);
                            WorldGen.KillTile(num19, index13, false, false, false);
                            if (tile.active())
                                break;
                            NetMessage.SendData(34, -1, -1, "", (int)num35, (float)num19, (float)index13, 0.0f, chest3, 0, 0);
                            break;
                        }
                        int id = (int)this.reader.ReadInt16();
                        if ((int)num35 == 0)
                        {
                            if (id == -1)
                            {
                                WorldGen.KillTile(index12, index13, false, false, false);
                                break;
                            }
                            WorldGen.PlaceChestDirect(index12, index13, (ushort)21, style1, id);
                            break;
                        }
                        if ((int)num35 == 2)
                        {
                            if (id == -1)
                            {
                                WorldGen.KillTile(index12, index13, false, false, false);
                                break;
                            }
                            WorldGen.PlaceDresserDirect(index12, index13, (ushort)88, style1, id);
                            break;
                        }
                        Chest.DestroyChestDirect(index12, index13, id);
                        WorldGen.KillTile(index12, index13, false, false, false);
                        break;
                    case (byte)35:
                        int number17 = (int)this.reader.ReadByte();
                        if (Game1.netMode == 2)
                            number17 = this.whoAmI;
                        int healAmount1 = (int)this.reader.ReadInt16();
                        if (number17 != Game1.myPlayer || Game1.ServerSideCharacter)
                            Game1.player[number17].HealEffect(healAmount1, true);
                        if (Game1.netMode != 2)
                            break;
                        NetMessage.SendData(35, -1, this.whoAmI, "", number17, (float)healAmount1, 0.0f, 0.0f, 0, 0, 0);
                        break;
                    case (byte)36:
                        int number18 = (int)this.reader.ReadByte();
                        if (Game1.netMode == 2)
                            number18 = this.whoAmI;
                        Player player9 = Game1.player[number18];
                        player9.zone1 = (BitsByte)this.reader.ReadByte();
                        player9.zone2 = (BitsByte)this.reader.ReadByte();
                        if (Game1.netMode != 2)
                            break;
                        NetMessage.SendData(36, -1, this.whoAmI, "", number18, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        break;
                    case (byte)37:
                        if (Game1.netMode != 1)
                            break;
                        if (Game1.autoPass)
                        {
                            NetMessage.SendData(38, -1, -1, Netplay.ServerPassword, 0, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                            Game1.autoPass = false;
                            break;
                        }
                        Netplay.ServerPassword = "";
                        Game1.menuMode = 31;
                        break;
                    case (byte)38:
                        if (Game1.netMode != 2)
                            break;
                        if (this.reader.ReadString() == Netplay.ServerPassword)
                        {
                            Netplay.Clients[this.whoAmI].State = 1;
                            NetMessage.SendData(3, this.whoAmI, -1, "", 0, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                            break;
                        }
                        NetMessage.SendData(2, this.whoAmI, -1, Lang.mp[1], 0, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        break;
                    case (byte)39:
                        if (Game1.netMode != 1)
                            break;
                        int number19 = (int)this.reader.ReadInt16();
                        Game1.item[number19].owner = (int)byte.MaxValue;
                        NetMessage.SendData(22, -1, -1, "", number19, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        break;
                    case (byte)40:
                        int number20 = (int)this.reader.ReadByte();
                        if (Game1.netMode == 2)
                            number20 = this.whoAmI;
                        int num36 = (int)this.reader.ReadInt16();
                        Game1.player[number20].talkNPC = num36;
                        if (Game1.netMode != 2)
                            break;
                        NetMessage.SendData(40, -1, this.whoAmI, "", number20, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        break;
                    case (byte)41:
                        int number21 = (int)this.reader.ReadByte();
                        if (Game1.netMode == 2)
                            number21 = this.whoAmI;
                        Player player10 = Game1.player[number21];
                        float num37 = this.reader.ReadSingle();
                        int num38 = (int)this.reader.ReadInt16();
                        player10.itemRotation = num37;
                        player10.itemAnimation = num38;
                        player10.channel = player10.inventory[player10.selectedItem].channel;
                        if (Game1.netMode != 2)
                            break;
                        NetMessage.SendData(41, -1, this.whoAmI, "", number21, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        break;
                    case (byte)42:
                        int index14 = (int)this.reader.ReadByte();
                        if (Game1.netMode == 2)
                            index14 = this.whoAmI;
                        else if (Game1.myPlayer == index14 && !Game1.ServerSideCharacter)
                            break;
                        int num39 = (int)this.reader.ReadInt16();
                        int num40 = (int)this.reader.ReadInt16();
                        Game1.player[index14].statMana = num39;
                        Game1.player[index14].statManaMax = num40;
                        break;
                    case (byte)43:
                        int number22 = (int)this.reader.ReadByte();
                        if (Game1.netMode == 2)
                            number22 = this.whoAmI;
                        int manaAmount = (int)this.reader.ReadInt16();
                        if (number22 != Game1.myPlayer)
                            Game1.player[number22].ManaEffect(manaAmount);
                        if (Game1.netMode != 2)
                            break;
                        NetMessage.SendData(43, -1, this.whoAmI, "", number22, (float)manaAmount, 0.0f, 0.0f, 0, 0, 0);
                        break;
                    case (byte)44:
                        int number23 = (int)this.reader.ReadByte();
                        if (Game1.netMode == 2)
                            number23 = this.whoAmI;
                        int hitDirection3 = (int)this.reader.ReadByte() - 1;
                        int num41 = (int)this.reader.ReadInt16();
                        byte num42 = this.reader.ReadByte();
                        string str6 = this.reader.ReadString();
                        Game1.player[number23].KillMe((double)num41, hitDirection3, (int)num42 == 1, str6);
                        if (Game1.netMode != 2)
                            break;
                        NetMessage.SendData(44, -1, this.whoAmI, str6, number23, (float)hitDirection3, (float)num41, (float)num42, 0, 0, 0);
                        break;
                    case (byte)45:
                        int number24 = (int)this.reader.ReadByte();
                        if (Game1.netMode == 2)
                            number24 = this.whoAmI;
                        int index15 = (int)this.reader.ReadByte();
                        Player player11 = Game1.player[number24];
                        int num43 = player11.team;
                        player11.team = index15;
                        Color color4 = Game1.teamColor[index15];
                        if (Game1.netMode != 2)
                            break;
                        NetMessage.SendData(45, -1, this.whoAmI, "", number24, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        string str7 = " " + Lang.mp[13 + index15];
                        if (index15 == 5)
                            str7 = " " + Lang.mp[22];
                        for (int remoteClient = 0; remoteClient < (int)byte.MaxValue; ++remoteClient)
                        {
                            if (remoteClient == this.whoAmI || num43 > 0 && Game1.player[remoteClient].team == num43 || index15 > 0 && Game1.player[remoteClient].team == index15)
                                NetMessage.SendData(25, remoteClient, -1, player11.name + str7, (int)byte.MaxValue, (float)color4.R, (float)color4.G, (float)color4.B, 0, 0, 0);
                        }
                        break;
                    case (byte)46:
                        if (Game1.netMode != 2)
                            break;
                        int number25 = Sign.ReadSign((int)this.reader.ReadInt16(), (int)this.reader.ReadInt16(), true);
                        if (number25 < 0)
                            break;
                        NetMessage.SendData(47, this.whoAmI, -1, "", number25, (float)this.whoAmI, 0.0f, 0.0f, 0, 0, 0);
                        break;
                    case (byte)47:
                        int index16 = (int)this.reader.ReadInt16();
                        int num44 = (int)this.reader.ReadInt16();
                        int num45 = (int)this.reader.ReadInt16();
                        string text2 = this.reader.ReadString();
                        string str8 = (string)null;
                        if (Game1.sign[index16] != null)
                            str8 = Game1.sign[index16].text;
                        Game1.sign[index16] = new Sign();
                        Game1.sign[index16].x = num44;
                        Game1.sign[index16].y = num45;
                        Sign.TextSign(index16, text2);
                        int num46 = (int)this.reader.ReadByte();
                        if (Game1.netMode == 2 && str8 != text2)
                        {
                            num46 = this.whoAmI;
                            NetMessage.SendData(47, -1, this.whoAmI, "", index16, (float)num46, 0.0f, 0.0f, 0, 0, 0);
                        }
                        if (Game1.netMode != 1 || num46 != Game1.myPlayer || Game1.sign[index16] == null)
                            break;
                        Game1.playerInventory = false;
                        Game1.player[Game1.myPlayer].talkNPC = -1;
                        Game1.npcChatCornerItem = 0;
                        Game1.editSign = false;
                        Game1.PlaySound(10, -1, -1, 1);
                        Game1.player[Game1.myPlayer].sign = index16;
                        Game1.npcChatText = Game1.sign[index16].text;
                        break;
                    case (byte)48:
                        int i1 = (int)this.reader.ReadInt16();
                        int j1 = (int)this.reader.ReadInt16();
                        byte num47 = this.reader.ReadByte();
                        byte num48 = this.reader.ReadByte();
                        if (Game1.netMode == 2 && Netplay.spamCheck)
                        {
                            int index5 = this.whoAmI;
                            int num19 = (int)((double)Game1.player[index5].position.X + (double)(Game1.player[index5].width / 2));
                            int num49 = (int)((double)Game1.player[index5].position.Y + (double)(Game1.player[index5].height / 2));
                            int num50 = 10;
                            int num51 = num19 - num50;
                            int num52 = num19 + num50;
                            int num53 = num49 - num50;
                            int num54 = num49 + num50;
                            if (i1 < num51 || i1 > num52 || (j1 < num53 || j1 > num54))
                            {
                                NetMessage.BootPlayer(this.whoAmI, "Cheating attempt detected: Liquid spam");
                                break;
                            }
                        }
                        if (Game1.tile[i1, j1] == null)
                            Game1.tile[i1, j1] = new Tile();
                        lock (Game1.tile[i1, j1])
                        {
                            Game1.tile[i1, j1].liquid = num47;
                            Game1.tile[i1, j1].liquidType((int)num48);
                            if (Game1.netMode != 2)
                                break;
                            WorldGen.SquareTileFrame(i1, j1, true);
                            break;
                        }
                    case (byte)49:
                        if (Netplay.Connection.State != 6)
                            break;
                        Netplay.Connection.State = 10;
                        Game1.ActivePlayerFileData.StartPlayTimer();
                        Player.EnterWorld(Game1.player[Game1.myPlayer]);
                        Game1.player[Game1.myPlayer].Spawn();
                        break;
                    case (byte)50:
                        int number26 = (int)this.reader.ReadByte();
                        if (Game1.netMode == 2)
                            number26 = this.whoAmI;
                        else if (number26 == Game1.myPlayer && !Game1.ServerSideCharacter)
                            break;
                        Player player12 = Game1.player[number26];
                        for (int index5 = 0; index5 < 22; ++index5)
                        {
                            player12.buffType[index5] = (int)this.reader.ReadByte();
                            player12.buffTime[index5] = player12.buffType[index5] <= 0 ? 0 : 60;
                        }
                        if (Game1.netMode != 2)
                            break;
                        NetMessage.SendData(50, -1, this.whoAmI, "", number26, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        break;
                    case (byte)51:
                        byte num55 = this.reader.ReadByte();
                        byte num56 = this.reader.ReadByte();
                        switch (num56)
                        {
                            case (byte)1:
                                NPC.SpawnSkeletron();
                                return;
                            case (byte)2:
                                if (Game1.netMode == 2)
                                {
                                    NetMessage.SendData(51, -1, this.whoAmI, "", (int)num55, (float)num56, 0.0f, 0.0f, 0, 0, 0);
                                    return;
                                }
                                Game1.PlaySound(2, (int)Game1.player[(int)num55].position.X, (int)Game1.player[(int)num55].position.Y, 1);
                                return;
                            case (byte)3:
                                if (Game1.netMode != 2)
                                    return;
                                Game1.Sundialing();
                                return;
                            case (byte)4:
                                Game1.npc[(int)num55].BigMimicSpawnSmoke();
                                return;
                            default:
                                return;
                        }
                    case (byte)52:
                        int num57 = (int)this.reader.ReadByte();
                        int num58 = (int)this.reader.ReadInt16();
                        int num59 = (int)this.reader.ReadInt16();
                        if (num57 == 1)
                        {
                            Chest.Unlock(num58, num59);
                            if (Game1.netMode == 2)
                            {
                                NetMessage.SendData(52, -1, this.whoAmI, "", 0, (float)num57, (float)num58, (float)num59, 0, 0, 0);
                                NetMessage.SendTileSquare(-1, num58, num59, 2);
                            }
                        }
                        if (num57 != 2)
                            break;
                        WorldGen.UnlockDoor(num58, num59);
                        if (Game1.netMode != 2)
                            break;
                        NetMessage.SendData(52, -1, this.whoAmI, "", 0, (float)num57, (float)num58, (float)num59, 0, 0, 0);
                        NetMessage.SendTileSquare(-1, num58, num59, 2);
                        break;
                    case (byte)53:
                        int number27 = (int)this.reader.ReadInt16();
                        int type4 = (int)this.reader.ReadByte();
                        int time1 = (int)this.reader.ReadInt16();
                        Game1.npc[number27].AddBuff(type4, time1, true);
                        if (Game1.netMode != 2)
                            break;
                        NetMessage.SendData(54, -1, -1, "", number27, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        break;
                    case (byte)54:
                        if (Game1.netMode != 1)
                            break;
                        int index17 = (int)this.reader.ReadInt16();
                        NPC npc2 = Game1.npc[index17];
                        for (int index5 = 0; index5 < 5; ++index5)
                        {
                            npc2.buffType[index5] = (int)this.reader.ReadByte();
                            npc2.buffTime[index5] = (int)this.reader.ReadInt16();
                        }
                        break;
                    case (byte)55:
                        int index18 = (int)this.reader.ReadByte();
                        int type5 = (int)this.reader.ReadByte();
                        int time1_1 = (int)this.reader.ReadInt16();
                        if (Game1.netMode == 2 && index18 != this.whoAmI && !Game1.pvpBuff[type5])
                            break;
                        if (Game1.netMode == 1 && index18 == Game1.myPlayer)
                        {
                            Game1.player[index18].AddBuff(type5, time1_1, true);
                            break;
                        }
                        if (Game1.netMode != 2)
                            break;
                        NetMessage.SendData(55, index18, -1, "", index18, (float)type5, (float)time1_1, 0.0f, 0, 0, 0);
                        break;
                    case (byte)56:
                        int number28 = (int)this.reader.ReadInt16();
                        if (number28 < 0 || number28 >= 200)
                            break;
                        string str9 = this.reader.ReadString();
                        if (Game1.netMode == 1)
                        {
                            Game1.npc[number28].displayName = str9;
                            break;
                        }
                        if (Game1.netMode != 2)
                            break;
                        NetMessage.SendData(56, this.whoAmI, -1, Game1.npc[number28].displayName, number28, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        break;
                    case (byte)57:
                        if (Game1.netMode != 1)
                            break;
                        WorldGen.tGood = this.reader.ReadByte();
                        WorldGen.tEvil = this.reader.ReadByte();
                        WorldGen.tBlood = this.reader.ReadByte();
                        break;
                    case (byte)58:
                        int index19 = (int)this.reader.ReadByte();
                        if (Game1.netMode == 2)
                            index19 = this.whoAmI;
                        float number2_1 = this.reader.ReadSingle();
                        if (Game1.netMode == 2)
                        {
                            NetMessage.SendData(58, -1, this.whoAmI, "", this.whoAmI, number2_1, 0.0f, 0.0f, 0, 0, 0);
                            break;
                        }
                        Player player13 = Game1.player[index19];
                        Game1.harpNote = number2_1;
                        int Style = 26;
                        if (player13.inventory[player13.selectedItem].itemId == 507)
                            Style = 35;
                        Game1.PlaySound(2, (int)player13.position.X, (int)player13.position.Y, Style);
                        break;
                    case (byte)59:
                        int num60 = (int)this.reader.ReadInt16();
                        int j2 = (int)this.reader.ReadInt16();
                        Wiring.HitSwitch(num60, j2);
                        if (Game1.netMode != 2)
                            break;
                        NetMessage.SendData(59, -1, this.whoAmI, "", num60, (float)j2, 0.0f, 0.0f, 0, 0, 0);
                        break;
                    case (byte)60:
                        int n = (int)this.reader.ReadInt16();
                        int x1 = (int)this.reader.ReadInt16();
                        int y3 = (int)this.reader.ReadInt16();
                        byte num61 = this.reader.ReadByte();
                        if (n >= 200)
                        {
                            NetMessage.BootPlayer(this.whoAmI, "cheating attempt detected: Invalid kick-out");
                            break;
                        }
                        if (Game1.netMode == 1)
                        {
                            Game1.npc[n].homeless = (int)num61 == 1;
                            Game1.npc[n].homeTileX = x1;
                            Game1.npc[n].homeTileY = y3;
                            break;
                        }
                        if ((int)num61 == 0)
                        {
                            WorldGen.kickOut(n);
                            break;
                        }
                        WorldGen.moveRoom(x1, y3, n);
                        break;
                    case (byte)61:
                        int plr = this.reader.ReadInt32();
                        int Type2 = this.reader.ReadInt32();
                        if (Game1.netMode != 2)
                            break;
                        if (Type2 >= 0 && Type2 < 540 && NPCID.Sets.MPAllowedEnemies[Type2])
                        {
                            if (NPC.AnyNPCs(Type2))
                                break;
                            NPC.SpawnOnPlayer(plr, Type2);
                            break;
                        }
                        if (Type2 == -4)
                        {
                            if (Game1.dayTime)
                                break;
                            NetMessage.SendData(25, -1, -1, Lang.misc[31], (int)byte.MaxValue, 50f, (float)byte.MaxValue, 130f, 0, 0, 0);
                            Game1.startPumpkinMoon();
                            NetMessage.SendData(7, -1, -1, "", 0, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                            NetMessage.SendData(78, -1, -1, "", 0, 1f, 2f, 1f, 0, 0, 0);
                            break;
                        }
                        if (Type2 == -5)
                        {
                            if (Game1.dayTime)
                                break;
                            NetMessage.SendData(25, -1, -1, Lang.misc[34], (int)byte.MaxValue, 50f, (float)byte.MaxValue, 130f, 0, 0, 0);
                            Game1.startSnowMoon();
                            NetMessage.SendData(7, -1, -1, "", 0, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                            NetMessage.SendData(78, -1, -1, "", 0, 1f, 1f, 1f, 0, 0, 0);
                            break;
                        }
                        if (Type2 == -6)
                        {
                            if (!Game1.dayTime || Game1.eclipse)
                                break;
                            NetMessage.SendData(25, -1, -1, Lang.misc[20], (int)byte.MaxValue, 50f, (float)byte.MaxValue, 130f, 0, 0, 0);
                            Game1.eclipse = true;
                            NetMessage.SendData(7, -1, -1, "", 0, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                            break;
                        }
                        if (Type2 == -7)
                        {
                            NetMessage.SendData(25, -1, -1, "martian moon toggled", (int)byte.MaxValue, 50f, (float)byte.MaxValue, 130f, 0, 0, 0);
                            Game1.invasionDelay = 0;
                            Game1.StartInvasion(4);
                            NetMessage.SendData(7, -1, -1, "", 0, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                            NetMessage.SendData(78, -1, -1, "", 0, 1f, (float)(Game1.invasionType + 2), 0.0f, 0, 0, 0);
                            break;
                        }
                        if (Type2 >= 0)
                            break;
                        int type6 = 1;
                        if (Type2 > -5)
                            type6 = -Type2;
                        if (type6 > 0 && Game1.invasionType == 0)
                        {
                            Game1.invasionDelay = 0;
                            Game1.StartInvasion(type6);
                        }
                        NetMessage.SendData(78, -1, -1, "", 0, 1f, (float)(Game1.invasionType + 2), 0.0f, 0, 0, 0);
                        break;
                    case (byte)62:
                        int number29 = (int)this.reader.ReadByte();
                        int num62 = (int)this.reader.ReadByte();
                        if (Game1.netMode == 2)
                            number29 = this.whoAmI;
                        if (num62 == 1)
                            Game1.player[number29].NinjaDodge();
                        if (num62 == 2)
                            Game1.player[number29].ShadowDodge();
                        if (Game1.netMode != 2)
                            break;
                        NetMessage.SendData(62, -1, this.whoAmI, "", number29, (float)num62, 0.0f, 0.0f, 0, 0, 0);
                        break;
                    case (byte)63:
                        int num63 = (int)this.reader.ReadInt16();
                        int y4 = (int)this.reader.ReadInt16();
                        byte color5 = this.reader.ReadByte();
                        WorldGen.paintTile(num63, y4, color5, false);
                        if (Game1.netMode != 2)
                            break;
                        NetMessage.SendData(63, -1, this.whoAmI, "", num63, (float)y4, (float)color5, 0.0f, 0, 0, 0);
                        break;
                    case (byte)64:
                        int num64 = (int)this.reader.ReadInt16();
                        int y5 = (int)this.reader.ReadInt16();
                        byte color6 = this.reader.ReadByte();
                        WorldGen.paintWall(num64, y5, color6, false);
                        if (Game1.netMode != 2)
                            break;
                        NetMessage.SendData(64, -1, this.whoAmI, "", num64, (float)y5, (float)color6, 0.0f, 0, 0, 0);
                        break;
                    case (byte)65:
                        BitsByte bitsByte17 = (BitsByte)this.reader.ReadByte();
                        int index20 = (int)this.reader.ReadInt16();
                        if (Game1.netMode == 2)
                            index20 = this.whoAmI;
                        Vector2 vector2_7 = Utils.ReadVector2(this.reader);
                        int num65 = 0;
                        int num66 = 0;
                        if (bitsByte17[0])
                            ++num65;
                        if (bitsByte17[1])
                            num65 += 2;
                        if (bitsByte17[2])
                            ++num66;
                        if (bitsByte17[3])
                            num66 += 2;
                        if (num65 == 0)
                            Game1.player[index20].Teleport(vector2_7, num66, 0);
                        else if (num65 == 1)
                            Game1.npc[index20].Teleport(vector2_7, num66, 0);
                        else if (num65 == 2)
                        {
                            Game1.player[index20].Teleport(vector2_7, num66, 0);
                            if (Game1.netMode == 2)
                            {
                                RemoteClient.CheckSection(this.whoAmI, vector2_7, 1);
                                NetMessage.SendData(65, -1, -1, "", 0, (float)index20, vector2_7.X, vector2_7.Y, num66, 0, 0);
                                int index5 = -1;
                                float num19 = 9999f;
                                for (int index6 = 0; index6 < (int)byte.MaxValue; ++index6)
                                {
                                    if (Game1.player[index6].active && index6 != this.whoAmI)
                                    {
                                        Vector2 vector2_8 = Game1.player[index6].position - Game1.player[this.whoAmI].position;
                                        if ((double)vector2_8.Length() < (double)num19)
                                        {
                                            num19 = vector2_8.Length();
                                            index5 = index6;
                                        }
                                    }
                                }
                                if (index5 >= 0)
                                    NetMessage.SendData(25, -1, -1, Game1.player[this.whoAmI].name + " has teleported to " + Game1.player[index5].name, (int)byte.MaxValue, 250f, 250f, 0.0f, 0, 0, 0);
                            }
                        }
                        if (Game1.netMode != 2 || num65 != 0)
                            break;
                        NetMessage.SendData(65, -1, this.whoAmI, "", 0, (float)index20, vector2_7.X, vector2_7.Y, num66, 0, 0);
                        break;
                    case (byte)66:
                        int number30 = (int)this.reader.ReadByte();
                        int healAmount2 = (int)this.reader.ReadInt16();
                        if (healAmount2 <= 0)
                            break;
                        Player player14 = Game1.player[number30];
                        player14.statLife += healAmount2;
                        if (player14.statLife > player14.statLifeMax2)
                            player14.statLife = player14.statLifeMax2;
                        player14.HealEffect(healAmount2, false);
                        if (Game1.netMode != 2)
                            break;
                        NetMessage.SendData(66, -1, this.whoAmI, "", number30, (float)healAmount2, 0.0f, 0.0f, 0, 0, 0);
                        break;
                    case (byte)68:
                        this.reader.ReadString();
                        break;
                    case (byte)69:
                        int number31 = (int)this.reader.ReadInt16();
                        int X2 = (int)this.reader.ReadInt16();
                        int Y2 = (int)this.reader.ReadInt16();
                        if (Game1.netMode == 1)
                        {
                            if (number31 < 0 || number31 >= 1000)
                                break;
                            Chest chest2 = Game1.chest[number31];
                            if (chest2 == null)
                            {
                                chest2 = new Chest(false);
                                chest2.x = X2;
                                chest2.y = Y2;
                                Game1.chest[number31] = chest2;
                            }
                            else if (chest2.x != X2 || chest2.y != Y2)
                                break;
                            chest2.name = this.reader.ReadString();
                            break;
                        }
                        if (number31 < -1 || number31 >= 1000)
                            break;
                        if (number31 == -1)
                        {
                            number31 = Chest.FindChest(X2, Y2);
                            if (number31 == -1)
                                break;
                        }
                        Chest chest4 = Game1.chest[number31];
                        if (chest4.x != X2 || chest4.y != Y2)
                            break;
                        NetMessage.SendData(69, this.whoAmI, -1, chest4.name, number31, (float)X2, (float)Y2, 0.0f, 0, 0, 0);
                        break;
                    case (byte)70:
                        if (Game1.netMode != 2)
                            break;
                        int i2 = (int)this.reader.ReadInt16();
                        int who = (int)this.reader.ReadByte();
                        if (Game1.netMode == 2)
                            who = this.whoAmI;
                        if (i2 >= 200 || i2 < 0)
                            break;
                        NPC.CatchNPC(i2, who);
                        break;
                    case (byte)71:
                        if (Game1.netMode != 2)
                            break;
                        NPC.ReleaseNPC(this.reader.ReadInt32(), this.reader.ReadInt32(), (int)this.reader.ReadInt16(), (int)this.reader.ReadByte(), this.whoAmI);
                        break;
                    case (byte)72:
                        if (Game1.netMode != 1)
                            break;
                        for (int index5 = 0; index5 < 40; ++index5)
                            Game1.travelShop[index5] = (int)this.reader.ReadInt16();
                        break;
                    case (byte)73:
                        Game1.player[this.whoAmI].TeleportationPotion();
                        break;
                    case (byte)74:
                        if (Game1.netMode != 1)
                            break;
                        Game1.anglerQuest = (int)this.reader.ReadByte();
                        Game1.anglerQuestFinished = this.reader.ReadBoolean();
                        break;
                    case (byte)75:
                        if (Game1.netMode != 2)
                            break;
                        string str10 = Game1.player[this.whoAmI].name;
                        if (Game1.anglerWhoFinishedToday.Contains(str10))
                            break;
                        Game1.anglerWhoFinishedToday.Add(str10);
                        break;
                    case (byte)76:
                        int number32 = (int)this.reader.ReadByte();
                        if (number32 == Game1.myPlayer && !Game1.ServerSideCharacter)
                            break;
                        if (Game1.netMode == 2)
                            number32 = this.whoAmI;
                        Game1.player[number32].anglerQuestsFinished = this.reader.ReadInt32();
                        if (Game1.netMode != 2)
                            break;
                        NetMessage.SendData(76, -1, this.whoAmI, "", number32, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        break;
                    case (byte)77:
                        Animation.NewTemporaryAnimation((int)this.reader.ReadInt16(), this.reader.ReadUInt16(), (int)this.reader.ReadInt16(), (int)this.reader.ReadInt16());
                        break;
                    case (byte)78:
                        if (Game1.netMode != 1)
                            break;
                        Game1.ReportInvasionProgress((int)this.reader.ReadInt16(), (int)this.reader.ReadInt16(), (int)this.reader.ReadSByte(), (int)this.reader.ReadSByte());
                        break;
                    case (byte)79:
                        int x2 = (int)this.reader.ReadInt16();
                        int y6 = (int)this.reader.ReadInt16();
                        short num67 = this.reader.ReadInt16();
                        int style2 = (int)this.reader.ReadByte();
                        int num68 = (int)this.reader.ReadByte();
                        int random = (int)this.reader.ReadSByte();
                        int direction2 = !this.reader.ReadBoolean() ? -1 : 1;
                        if (Game1.netMode == 2)
                        {
                            ++Netplay.Clients[this.whoAmI].SpamAddBlock;
                            if (!WorldGen.InWorld(x2, y6, 10) || !Netplay.Clients[this.whoAmI].TileSections[Netplay.GetSectionX(x2), Netplay.GetSectionY(y6)])
                                break;
                        }
                        WorldGen.PlaceObject(x2, y6, (int)num67, false, style2, num68, random, direction2);
                        if (Game1.netMode != 2)
                            break;
                        NetMessage.SendObjectPlacment(this.whoAmI, x2, y6, (int)num67, style2, num68, random, direction2);
                        break;
                    case (byte)80:
                        if (Game1.netMode != 1)
                            break;
                        int index21 = (int)this.reader.ReadByte();
                        int num69 = (int)this.reader.ReadInt16();
                        if (num69 < -3 || num69 >= 1000)
                            break;
                        Game1.player[index21].chest = num69;
                        Recipe.FindRecipes();
                        break;
                    case (byte)81:
                        if (Game1.netMode != 1)
                            break;
                        CombatText.NewText(new Rectangle((int)this.reader.ReadSingle(), (int)this.reader.ReadSingle(), 0, 0), Utils.ReadRGB(this.reader), this.reader.ReadString(), false, false);
                        break;
                    case (byte)82:
                        NetManager.Instance.Read(this.reader, this.whoAmI);
                        break;
                    case (byte)83:
                        if (Game1.netMode != 1)
                            break;
                        int index22 = (int)this.reader.ReadInt16();
                        int num70 = this.reader.ReadInt32();
                        if (index22 < 0 || index22 >= 251)
                            break;
                        NPC.killCount[index22] = num70;
                        break;
                    case (byte)84:
                        byte num71 = this.reader.ReadByte();
                        float num72 = this.reader.ReadSingle();
                        Game1.player[(int)num71].stealth = num72;
                        if (Game1.netMode != 2)
                            break;
                        NetMessage.SendData(84, -1, this.whoAmI, "", (int)num71, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        break;
                    case (byte)85:
                        int num73 = this.whoAmI;
                        byte num74 = this.reader.ReadByte();
                        if (Game1.netMode != 2 || num73 >= (int)byte.MaxValue || (int)num74 >= 58)
                            break;
                        Chest.ServerPlaceItem(this.whoAmI, (int)num74);
                        break;
                    case (byte)86:
                        if (Game1.netMode != 1)
                            break;
                        int key1 = this.reader.ReadInt32();
                        if (!this.reader.ReadBoolean())
                        {
                            TileEntity tileEntity;
                            if (!TileEntity.ByID.TryGetValue(key1, out tileEntity) || !(tileEntity is TETrainingDummy) && !(tileEntity is TEItemFrame))
                                break;
                            TileEntity.ByID.Remove(key1);
                            TileEntity.ByPosition.Remove(tileEntity.Position);
                            break;
                        }
                        TileEntity tileEntity1 = TileEntity.Read(this.reader);
                        TileEntity.ByID[tileEntity1.ID] = tileEntity1;
                        TileEntity.ByPosition[tileEntity1.Position] = tileEntity1;
                        break;
                    case (byte)87:
                        if (Game1.netMode != 2)
                            break;
                        int num75 = (int)this.reader.ReadInt16();
                        int num76 = (int)this.reader.ReadInt16();
                        int num77 = (int)this.reader.ReadByte();
                        if (num75 < 0 || num75 >= Game1.maxTilesX || (num76 < 0 || num76 >= Game1.maxTilesY) || TileEntity.ByPosition.ContainsKey(new Point16(num75, num76)))
                            break;
                        switch (num77)
                        {
                            case 0:
                                if (!TETrainingDummy.ValidTile(num75, num76))
                                    return;
                                TETrainingDummy.Place(num75, num76);
                                return;
                            case 1:
                                if (!TEItemFrame.ValidTile(num75, num76))
                                    return;
                                NetMessage.SendData(86, -1, -1, "", TEItemFrame.Place(num75, num76), (float)num75, (float)num76, 0.0f, 0, 0, 0);
                                return;
                            default:
                                return;
                        }
                    case (byte)88:
                        if (Game1.netMode != 1)
                            break;
                        int index23 = (int)this.reader.ReadInt16();
                        if (index23 < 0 || index23 > 400)
                            break;
                        Item obj2 = Game1.item[index23];
                        BitsByte bitsByte18 = (BitsByte)this.reader.ReadByte();
                        if (bitsByte18[0])
                            obj2.color.PackedValue = this.reader.ReadUInt32();
                        if (bitsByte18[1])
                            obj2.damage = (int)this.reader.ReadUInt16();
                        if (bitsByte18[2])
                            obj2.knockBack = this.reader.ReadSingle();
                        if (bitsByte18[3])
                            obj2.useAnimation = (int)this.reader.ReadUInt16();
                        if (bitsByte18[4])
                            obj2.useTime = (int)this.reader.ReadUInt16();
                        if (bitsByte18[5])
                            obj2.shoot = (int)this.reader.ReadInt16();
                        if (bitsByte18[6])
                            obj2.shootSpeed = this.reader.ReadSingle();
                        if (!bitsByte18[7])
                            break;
                        bitsByte18 = (BitsByte)this.reader.ReadByte();
                        if (bitsByte18[0])
                            obj2.width = (int)this.reader.ReadInt16();
                        if (bitsByte18[1])
                            obj2.height = (int)this.reader.ReadInt16();
                        if (!bitsByte18[2])
                            break;
                        obj2.scale = this.reader.ReadSingle();
                        break;
                    case (byte)89:
                        if (Game1.netMode != 2)
                            break;
                        TEItemFrame.TryPlacing((int)this.reader.ReadInt16(), (int)this.reader.ReadInt16(), (int)this.reader.ReadInt16(), (int)this.reader.ReadByte(), (int)this.reader.ReadInt16());
                        break;
                    case (byte)91:
                        if (Game1.netMode != 1)
                            break;
                        int key2 = this.reader.ReadInt32();
                        int type7 = (int)this.reader.ReadByte();
                        if (type7 == (int)byte.MaxValue)
                        {
                            if (!EmoteBubble.byID.ContainsKey(key2))
                                break;
                            EmoteBubble.byID.Remove(key2);
                            break;
                        }
                        int meta = (int)this.reader.ReadUInt16();
                        int time2 = (int)this.reader.ReadByte();
                        int emotion = (int)this.reader.ReadByte();
                        int num78 = 0;
                        if (emotion < 0)
                            num78 = (int)this.reader.ReadInt16();
                        WorldUIAnchor bubbleAnchor = EmoteBubble.DeserializeNetAnchor(type7, meta);
                        lock (EmoteBubble.byID)
                        {
                            if (!EmoteBubble.byID.ContainsKey(key2))
                            {
                                EmoteBubble.byID[key2] = new EmoteBubble(emotion, bubbleAnchor, time2);
                            }
                            else
                            {
                                EmoteBubble.byID[key2].lifeTime = time2;
                                EmoteBubble.byID[key2].lifeTimeStart = time2;
                                EmoteBubble.byID[key2].emote = emotion;
                                EmoteBubble.byID[key2].anchor = bubbleAnchor;
                            }
                            EmoteBubble.byID[key2].ID = key2;
                            EmoteBubble.byID[key2].metadata = num78;
                            break;
                        }
                    case (byte)92:
                        int number33 = (int)this.reader.ReadInt16();
                        float num79 = this.reader.ReadSingle();
                        float num80 = this.reader.ReadSingle();
                        float num81 = this.reader.ReadSingle();
                        if (Game1.netMode == 1)
                        {
                            Game1.npc[number33].moneyPing(new Vector2(num80, num81));
                            Game1.npc[number33].extraValue = num79;
                            break;
                        }
                        Game1.npc[number33].extraValue += num79;
                        NetMessage.SendData(92, -1, -1, "", number33, Game1.npc[number33].extraValue, num80, num81, 0, 0, 0);
                        break;
                    case (byte)95:
                        if (Game1.netMode != 2)
                            break;
                        ushort num82 = this.reader.ReadUInt16();
                        if ((int)num82 < 0 || (int)num82 >= 1000)
                            break;
                        Projectile projectile2 = Game1.projectile[(int)num82];
                        if (projectile2.type != 602)
                            break;
                        projectile2.Kill();
                        if (Game1.netMode == 0)
                            break;
                        NetMessage.SendData(29, -1, -1, "", projectile2.whoAmI, (float)projectile2.owner, 0.0f, 0.0f, 0, 0, 0);
                        break;
                    case (byte)96:
                        int index24 = (int)this.reader.ReadByte();
                        Player player15 = Game1.player[index24];
                        int extraInfo1 = (int)this.reader.ReadInt16();
                        Vector2 newPos1 = Utils.ReadVector2(this.reader);
                        Vector2 vector2_9 = Utils.ReadVector2(this.reader);
                        int num83 = extraInfo1 + (extraInfo1 % 2 == 0 ? 1 : -1);
                        player15.lastPortalColorIndex = num83;
                        player15.Teleport(newPos1, 4, extraInfo1);
                        player15.velocity = vector2_9;
                        break;
                    case (byte)97:
                        if (Game1.netMode != 1)
                            break;
                        AchievementsHelper.NotifyNPCKilledDirect(Game1.player[Game1.myPlayer], (int)this.reader.ReadInt16());
                        break;
                    case (byte)98:
                        if (Game1.netMode != 1)
                            break;
                        AchievementsHelper.NotifyProgressionEvent((int)this.reader.ReadInt16());
                        break;
                    case (byte)99:
                        int number34 = (int)this.reader.ReadByte();
                        if (Game1.netMode == 2)
                            number34 = this.whoAmI;
                        Game1.player[number34].MinionTargetPoint = Utils.ReadVector2(this.reader);
                        if (Game1.netMode != 2)
                            break;
                        NetMessage.SendData(99, -1, this.whoAmI, "", number34, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        break;
                    case (byte)100:
                        int index25 = (int)this.reader.ReadUInt16();
                        NPC npc3 = Game1.npc[index25];
                        int extraInfo2 = (int)this.reader.ReadInt16();
                        Vector2 newPos2 = Utils.ReadVector2(this.reader);
                        Vector2 vector2_10 = Utils.ReadVector2(this.reader);
                        int num84 = extraInfo2 + (extraInfo2 % 2 == 0 ? 1 : -1);
                        npc3.lastPortalColorIndex = num84;
                        npc3.Teleport(newPos2, 4, extraInfo2);
                        npc3.velocity = vector2_10;
                        break;
                    case (byte)101:
                        if (Game1.netMode == 2)
                            break;
                        NPC.ShieldStrengthTowerSolar = (int)this.reader.ReadUInt16();
                        NPC.ShieldStrengthTowerVortex = (int)this.reader.ReadUInt16();
                        NPC.ShieldStrengthTowerNebula = (int)this.reader.ReadUInt16();
                        NPC.ShieldStrengthTowerStardust = (int)this.reader.ReadUInt16();
                        if (NPC.ShieldStrengthTowerSolar < 0)
                            NPC.ShieldStrengthTowerSolar = 0;
                        if (NPC.ShieldStrengthTowerVortex < 0)
                            NPC.ShieldStrengthTowerVortex = 0;
                        if (NPC.ShieldStrengthTowerNebula < 0)
                            NPC.ShieldStrengthTowerNebula = 0;
                        if (NPC.ShieldStrengthTowerStardust < 0)
                            NPC.ShieldStrengthTowerStardust = 0;
                        if (NPC.ShieldStrengthTowerSolar > NPC.LunarShieldPowerExpert)
                            NPC.ShieldStrengthTowerSolar = NPC.LunarShieldPowerExpert;
                        if (NPC.ShieldStrengthTowerVortex > NPC.LunarShieldPowerExpert)
                            NPC.ShieldStrengthTowerVortex = NPC.LunarShieldPowerExpert;
                        if (NPC.ShieldStrengthTowerNebula > NPC.LunarShieldPowerExpert)
                            NPC.ShieldStrengthTowerNebula = NPC.LunarShieldPowerExpert;
                        if (NPC.ShieldStrengthTowerStardust <= NPC.LunarShieldPowerExpert)
                            break;
                        NPC.ShieldStrengthTowerStardust = NPC.LunarShieldPowerExpert;
                        break;
                    case (byte)102:
                        int index26 = (int)this.reader.ReadByte();
                        byte num85 = this.reader.ReadByte();
                        Vector2 Other = Utils.ReadVector2(this.reader);
                        if (Game1.netMode == 2)
                        {
                            NetMessage.SendData(102, -1, -1, "", this.whoAmI, (float)num85, Other.X, Other.Y, 0, 0, 0);
                            break;
                        }
                        Player player16 = Game1.player[index26];
                        for (int index5 = 0; index5 < (int)byte.MaxValue; ++index5)
                        {
                            Player player8 = Game1.player[index5];
                            if (player8.active && !player8.dead && (player16.team == 0 || player16.team == player8.team) && (double)player8.Distance(Other) < 700.0)
                            {
                                Vector2 vector2_8 = player16.Center - player8.Center;
                                Vector2 vec = Vector2.Normalize(vector2_8);
                                if (!Utils.HasNaNs(vec))
                                {
                                    int Type3 = 90;
                                    float num19 = 0.0f;
                                    float num49 = 0.2094395f;
                                    Vector2 spinningpoint = new Vector2(0.0f, -8f);
                                    Vector2 vector2_11 = new Vector2(-3f);
                                    float num50 = 0.0f;
                                    float num51 = 0.005f;
                                    switch (num85)
                                    {
                                        case (byte)173:
                                            Type3 = 90;
                                            break;
                                        case (byte)176:
                                            Type3 = 88;
                                            break;
                                        case (byte)179:
                                            Type3 = 86;
                                            break;
                                    }
                                    for (int index6 = 0; (double)index6 < (double)vector2_8.Length() / 6.0; ++index6)
                                    {
                                        Vector2 Position = player8.Center + 6f * (float)index6 * vec + Utils.RotatedBy(spinningpoint, (double)num19, new Vector2()) + vector2_11;
                                        num19 += num49;
                                        int index27 = Dust.NewDust(Position, 6, 6, Type3, 0.0f, 0.0f, 100, new Color(), 1.5f);
                                        Game1.dust[index27].noGravity = true;
                                        Game1.dust[index27].velocity = Vector2.Zero;
                                        Game1.dust[index27].fadeIn = (num50 += num51);
                                        Game1.dust[index27].velocity += vec * 1.5f;
                                    }
                                }
                                player8.NebulaLevelup((int)num85);
                            }
                        }
                        break;
                    case (byte)103:
                        if (Game1.netMode != 1)
                            break;
                        NPC.MoonLordCountdown = this.reader.ReadInt32();
                        break;
                    case (byte)104:
                        if (Game1.netMode != 1 || Game1.npcShop <= 0)
                            break;
                        Item[] objArray = Game1.instance.shop[Game1.npcShop].item;
                        int index28 = (int)this.reader.ReadByte();
                        int type8 = (int)this.reader.ReadInt16();
                        int num86 = (int)this.reader.ReadInt16();
                        int pre3 = (int)this.reader.ReadByte();
                        int num87 = this.reader.ReadInt32();
                        BitsByte bitsByte19 = (BitsByte)this.reader.ReadByte();
                        if (index28 >= objArray.Length)
                            break;
                        objArray[index28] = new Item();
                        objArray[index28].netDefaults(type8);
                        objArray[index28].stack = num86;
                        objArray[index28].Prefix(pre3);
                        objArray[index28].value = num87;
                        objArray[index28].buyOnce = bitsByte19[0];
                        break;
                }
            }
        }
    }
}
