// Decompiled with JetBrains decompiler
// Type: GameManager.messageBuffer
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 09A1FDD7-68B3-40FF-A030-32890DF6B0E7
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager v1 Beta\GameManager.exe

using System;
using System.Diagnostics;
using System.Text;

namespace GameManager
{
  public class messageBuffer
  {
    public const int readBufferMax = 65535;
    public const int writeBufferMax = 65535;
    public bool broadcast = false;
    public byte[] readBuffer = new byte[(int) ushort.MaxValue];
    public byte[] writeBuffer = new byte[(int) ushort.MaxValue];
    public bool writeLocked = false;
    public int messageLength = 0;
    public int totalData = 0;
    public int whoAmI;
    public int spamCount = 0;
    public int maxSpam;
    public bool checkBytes = false;

    public void Reset()
    {
      byte[] numArray = new byte[(int) ushort.MaxValue];
      this.writeBuffer = new byte[(int) ushort.MaxValue];
      this.writeLocked = false;
      this.messageLength = 0;
      this.totalData = 0;
      this.spamCount = 0;
      this.broadcast = false;
      this.checkBytes = false;
    }

    public void GetData(int start, int length)
    {
      if (this.whoAmI < 9)
        Netplay.serverSock[this.whoAmI].timeOut = 0;
      else
        Netplay.clientSock.timeOut = 0;
      int num1 = 0;
      int index1 = start + 1;
      byte msgType = this.readBuffer[start];
      if (Game1.netMode == 1 && Netplay.clientSock.statusMax > 0)
        ++Netplay.clientSock.statusCount;
      if (Game1.verboseNetplay)
      {
        Debug.WriteLine(Game1.myPlayer.ToString() + " Recieve:");
        for (int index2 = start; index2 < start + length; ++index2)
          Debug.Write(this.readBuffer[index2].ToString() + " ");
        Debug.WriteLine("");
        for (int index3 = start; index3 < start + length; ++index3)
          Debug.Write((object) (char) this.readBuffer[index3]);
        Debug.WriteLine("");
        Debug.WriteLine("");
      }
      if (Game1.netMode == 2 && msgType != (byte) 38 && Netplay.serverSock[this.whoAmI].state == -1)
        NetMessage.SendData(2, this.whoAmI, text: "Incorrect password.");
      else if (msgType == (byte) 1 && Game1.netMode == 2)
      {
        if (Netplay.serverSock[this.whoAmI].state != 0)
          return;
        if (Encoding.ASCII.GetString(this.readBuffer, start + 1, length - 1) == "Terraria" + (object) Game1.curRelease)
        {
          if (Netplay.password == null || Netplay.password == "")
          {
            Netplay.serverSock[this.whoAmI].state = 1;
            NetMessage.SendData(3, this.whoAmI);
          }
          else
          {
            Netplay.serverSock[this.whoAmI].state = -1;
            NetMessage.SendData(37, this.whoAmI);
          }
        }
        else
          NetMessage.SendData(2, this.whoAmI, text: "You are not using the same version as this server.");
      }
      else if (msgType == (byte) 2 && Game1.netMode == 1)
      {
        Netplay.disconnect = true;
        Game1.statusText = Encoding.ASCII.GetString(this.readBuffer, start + 1, length - 1);
      }
      else if (msgType == (byte) 3 && Game1.netMode == 1)
      {
        if (Netplay.clientSock.state == 1)
          Netplay.clientSock.state = 2;
        int index4 = (int) this.readBuffer[start + 1];
        if (index4 != Game1.myPlayer)
        {
          Game1.player[index4] = (Player) Game1.player[Game1.myPlayer].Clone();
          Game1.player[Game1.myPlayer] = new Player();
          Game1.player[index4].whoAmi = index4;
          Game1.myPlayer = index4;
        }
        NetMessage.SendData(4, text: Game1.player[Game1.myPlayer].name, number: Game1.myPlayer);
        NetMessage.SendData(16, number: Game1.myPlayer);
        NetMessage.SendData(42, number: Game1.myPlayer);
        for (int number2 = 0; number2 < 44; ++number2)
          NetMessage.SendData(5, text: Game1.player[Game1.myPlayer].inventory[number2].name, number: Game1.myPlayer, number2: (float) number2);
        NetMessage.SendData(5, text: Game1.player[Game1.myPlayer].armor[0].name, number: Game1.myPlayer, number2: 44f);
        NetMessage.SendData(5, text: Game1.player[Game1.myPlayer].armor[1].name, number: Game1.myPlayer, number2: 45f);
        NetMessage.SendData(5, text: Game1.player[Game1.myPlayer].armor[2].name, number: Game1.myPlayer, number2: 46f);
        NetMessage.SendData(5, text: Game1.player[Game1.myPlayer].armor[3].name, number: Game1.myPlayer, number2: 47f);
        NetMessage.SendData(5, text: Game1.player[Game1.myPlayer].armor[4].name, number: Game1.myPlayer, number2: 48f);
        NetMessage.SendData(5, text: Game1.player[Game1.myPlayer].armor[5].name, number: Game1.myPlayer, number2: 49f);
        NetMessage.SendData(5, text: Game1.player[Game1.myPlayer].armor[6].name, number: Game1.myPlayer, number2: 50f);
        NetMessage.SendData(5, text: Game1.player[Game1.myPlayer].armor[7].name, number: Game1.myPlayer, number2: 51f);
        NetMessage.SendData(6);
        if (Netplay.clientSock.state != 2)
          return;
        Netplay.clientSock.state = 3;
      }
      else
      {
        int num2;
        switch (msgType)
        {
          case 4:
            bool flag1 = false;
            int whoAmI1 = (int) this.readBuffer[start + 1];
            int num3 = (int) this.readBuffer[start + 2];
            if (Game1.netMode == 2)
              whoAmI1 = this.whoAmI;
            Game1.player[whoAmI1].hair = num3;
            Game1.player[whoAmI1].whoAmi = whoAmI1;
            int index5 = index1 + 2;
            Game1.player[whoAmI1].hairColor.R = this.readBuffer[index5];
            int index6 = index5 + 1;
            Game1.player[whoAmI1].hairColor.G = this.readBuffer[index6];
            int index7 = index6 + 1;
            Game1.player[whoAmI1].hairColor.B = this.readBuffer[index7];
            int index8 = index7 + 1;
            Game1.player[whoAmI1].skinColor.R = this.readBuffer[index8];
            int index9 = index8 + 1;
            Game1.player[whoAmI1].skinColor.G = this.readBuffer[index9];
            int index10 = index9 + 1;
            Game1.player[whoAmI1].skinColor.B = this.readBuffer[index10];
            int index11 = index10 + 1;
            Game1.player[whoAmI1].eyeColor.R = this.readBuffer[index11];
            int index12 = index11 + 1;
            Game1.player[whoAmI1].eyeColor.G = this.readBuffer[index12];
            int index13 = index12 + 1;
            Game1.player[whoAmI1].eyeColor.B = this.readBuffer[index13];
            int index14 = index13 + 1;
            Game1.player[whoAmI1].shirtColor.R = this.readBuffer[index14];
            int index15 = index14 + 1;
            Game1.player[whoAmI1].shirtColor.G = this.readBuffer[index15];
            int index16 = index15 + 1;
            Game1.player[whoAmI1].shirtColor.B = this.readBuffer[index16];
            int index17 = index16 + 1;
            Game1.player[whoAmI1].underShirtColor.R = this.readBuffer[index17];
            int index18 = index17 + 1;
            Game1.player[whoAmI1].underShirtColor.G = this.readBuffer[index18];
            int index19 = index18 + 1;
            Game1.player[whoAmI1].underShirtColor.B = this.readBuffer[index19];
            int index20 = index19 + 1;
            Game1.player[whoAmI1].pantsColor.R = this.readBuffer[index20];
            int index21 = index20 + 1;
            Game1.player[whoAmI1].pantsColor.G = this.readBuffer[index21];
            int index22 = index21 + 1;
            Game1.player[whoAmI1].pantsColor.B = this.readBuffer[index22];
            int index23 = index22 + 1;
            Game1.player[whoAmI1].shoeColor.R = this.readBuffer[index23];
            int index24 = index23 + 1;
            Game1.player[whoAmI1].shoeColor.G = this.readBuffer[index24];
            int index25 = index24 + 1;
            Game1.player[whoAmI1].shoeColor.B = this.readBuffer[index25];
            int index26 = index25 + 1;
            string text1 = Encoding.ASCII.GetString(this.readBuffer, index26, length - index26 + start);
            Game1.player[whoAmI1].name = text1;
            if (Game1.netMode != 2)
              return;
            if (Netplay.serverSock[this.whoAmI].state < 10)
            {
              for (int index27 = 0; index27 < 8; ++index27)
              {
                if (index27 != whoAmI1 && text1 == Game1.player[index27].name && Netplay.serverSock[index27].active)
                  flag1 = true;
              }
            }
            if (flag1)
            {
              NetMessage.SendData(2, this.whoAmI, text: text1 + " is already on this server.");
            }
            else
            {
              Netplay.serverSock[this.whoAmI].oldName = text1;
              Netplay.serverSock[this.whoAmI].name = text1;
              NetMessage.SendData(4, ignoreClient: this.whoAmI, text: text1, number: whoAmI1);
            }
            return;
          case 5:
            int whoAmI2 = (int) this.readBuffer[start + 1];
            if (Game1.netMode == 2)
              whoAmI2 = this.whoAmI;
            int number2_1 = (int) this.readBuffer[start + 2];
            int num4 = (int) this.readBuffer[start + 3];
            string str1 = Encoding.ASCII.GetString(this.readBuffer, start + 4, length - 4);
            if (number2_1 < 44)
            {
              Game1.player[whoAmI2].inventory[number2_1] = new Item();
              Game1.player[whoAmI2].inventory[number2_1].SetDefaults(str1);
              Game1.player[whoAmI2].inventory[number2_1].stack = num4;
            }
            else
            {
              Game1.player[whoAmI2].armor[number2_1 - 44] = new Item();
              Game1.player[whoAmI2].armor[number2_1 - 44].SetDefaults(str1);
              Game1.player[whoAmI2].armor[number2_1 - 44].stack = num4;
            }
            if (Game1.netMode != 2 || whoAmI2 != this.whoAmI)
              return;
            if (number2_1 < 44 && Game1.player[whoAmI2].inventory[number2_1].name != "" && Game1.player[whoAmI2].inventory[number2_1].type > 0 && Game1.player[whoAmI2].inventory[number2_1].maxStack > 0 && Game1.player[whoAmI2].inventory[number2_1].stack > Game1.player[whoAmI2].inventory[number2_1].maxStack)
              NetMessage.SendData(2, this.whoAmI, text: "Cheating attempt detected.");
            NetMessage.SendData(5, ignoreClient: this.whoAmI, text: str1, number: whoAmI2, number2: (float) number2_1);
            return;
          case 6:
            if (Game1.netMode != 2)
              return;
            if (Netplay.serverSock[this.whoAmI].state == 1)
              Netplay.serverSock[this.whoAmI].state = 2;
            NetMessage.SendData(7, this.whoAmI);
            return;
          case 7:
            if (Game1.netMode != 1)
              return;
            Game1.time = (double) BitConverter.ToInt32(this.readBuffer, index1);
            int index28 = index1 + 4;
            Game1.dayTime = false;
            if (this.readBuffer[index28] == (byte) 1)
              Game1.dayTime = true;
            int index29 = index28 + 1;
            Game1.moonPhase = (int) this.readBuffer[index29];
            int index30 = index29 + 1;
            int num5 = (int) this.readBuffer[index30];
            int startIndex1 = index30 + 1;
            Game1.bloodMoon = num5 == 1;
            Game1.maxTilesX = BitConverter.ToInt32(this.readBuffer, startIndex1);
            int startIndex2 = startIndex1 + 4;
            Game1.maxTilesY = BitConverter.ToInt32(this.readBuffer, startIndex2);
            int startIndex3 = startIndex2 + 4;
            Game1.spawnTileX = BitConverter.ToInt32(this.readBuffer, startIndex3);
            int startIndex4 = startIndex3 + 4;
            Game1.spawnTileY = BitConverter.ToInt32(this.readBuffer, startIndex4);
            int startIndex5 = startIndex4 + 4;
            Game1.worldSurface = (double) BitConverter.ToInt32(this.readBuffer, startIndex5);
            int startIndex6 = startIndex5 + 4;
            Game1.rockLayer = (double) BitConverter.ToInt32(this.readBuffer, startIndex6);
            int index31 = startIndex6 + 4;
            Game1.worldName = Encoding.ASCII.GetString(this.readBuffer, index31, length - index31 + start);
            if (Netplay.clientSock.state == 3)
              Netplay.clientSock.state = 4;
            return;
          case 8:
            if (Game1.netMode != 2)
              return;
            int number1 = 1350;
            if (Netplay.serverSock[this.whoAmI].state == 2)
              Netplay.serverSock[this.whoAmI].state = 3;
            NetMessage.SendData(9, this.whoAmI, text: "Recieving tile data", number: number1);
            Netplay.serverSock[this.whoAmI].statusText2 = "is recieving tile data";
            Netplay.serverSock[this.whoAmI].statusMax += number1;
            int sectionX1 = Netplay.GetSectionX(Game1.spawnTileX);
            int sectionY1 = Netplay.GetSectionY(Game1.spawnTileY);
            for (int sectionX2 = sectionX1 - 2; sectionX2 < sectionX1 + 3; ++sectionX2)
            {
              for (int sectionY2 = sectionY1 - 1; sectionY2 < sectionY1 + 2; ++sectionY2)
                NetMessage.SendSection(this.whoAmI, sectionX2, sectionY2);
            }
            NetMessage.SendData(11, this.whoAmI, number: sectionX1 - 2, number2: (float) (sectionY1 - 1), number3: (float) (sectionX1 + 2), number4: (float) (sectionY1 + 1));
            for (int number2 = 0; number2 < 200; ++number2)
            {
              if (Game1.item[number2].active)
              {
                NetMessage.SendData(21, this.whoAmI, number: number2);
                NetMessage.SendData(22, this.whoAmI, number: number2);
              }
            }
            for (int number3 = 0; number3 < 1000; ++number3)
            {
              if (Game1.npc[number3].active)
                NetMessage.SendData(23, this.whoAmI, number: number3);
            }
            return;
          case 9:
            if (Game1.netMode != 1)
              return;
            int int32_1 = BitConverter.ToInt32(this.readBuffer, start + 1);
            string str2 = Encoding.ASCII.GetString(this.readBuffer, start + 5, length - 5);
            Netplay.clientSock.statusMax += int32_1;
            Netplay.clientSock.statusText = str2;
            return;
          case 10:
            short int16_1 = BitConverter.ToInt16(this.readBuffer, start + 1);
            int int32_2 = BitConverter.ToInt32(this.readBuffer, start + 3);
            int int32_3 = BitConverter.ToInt32(this.readBuffer, start + 7);
            int startIndex7 = start + 11;
            for (int index32 = int32_2; index32 < int32_2 + (int) int16_1; ++index32)
            {
              if (Game1.tile[index32, int32_3] == null)
                Game1.tile[index32, int32_3] = new Tile();
              byte num6 = this.readBuffer[startIndex7];
              ++startIndex7;
              bool active = Game1.tile[index32, int32_3].active;
              Game1.tile[index32, int32_3].active = ((int) num6 & 1) == 1;
              if (((int) num6 & 2) == 2)
                Game1.tile[index32, int32_3].lighted = true;
              Game1.tile[index32, int32_3].wall = ((int) num6 & 4) != 4 ? (byte) 0 : (byte) 1;
              Game1.tile[index32, int32_3].liquid = ((int) num6 & 8) != 8 ? (byte) 0 : (byte) 1;
              if (Game1.tile[index32, int32_3].active)
              {
                int type = (int) Game1.tile[index32, int32_3].type;
                Game1.tile[index32, int32_3].type = this.readBuffer[startIndex7];
                ++startIndex7;
                if (Game1.tileFrameImportant[(int) Game1.tile[index32, int32_3].type])
                {
                  Game1.tile[index32, int32_3].frameX = BitConverter.ToInt16(this.readBuffer, startIndex7);
                  int startIndex8 = startIndex7 + 2;
                  Game1.tile[index32, int32_3].frameY = BitConverter.ToInt16(this.readBuffer, startIndex8);
                  startIndex7 = startIndex8 + 2;
                }
                else if (!active || (int) Game1.tile[index32, int32_3].type != type)
                {
                  Game1.tile[index32, int32_3].frameX = (short) -1;
                  Game1.tile[index32, int32_3].frameY = (short) -1;
                }
              }
              if (Game1.tile[index32, int32_3].wall > (byte) 0)
              {
                Game1.tile[index32, int32_3].wall = this.readBuffer[startIndex7];
                ++startIndex7;
              }
              if (Game1.tile[index32, int32_3].liquid > (byte) 0)
              {
                Game1.tile[index32, int32_3].liquid = this.readBuffer[startIndex7];
                int index33 = startIndex7 + 1;
                byte num7 = this.readBuffer[index33];
                startIndex7 = index33 + 1;
                Game1.tile[index32, int32_3].lava = num7 == (byte) 1;
              }
            }
            if (Game1.netMode != 2)
              return;
            NetMessage.SendData((int) msgType, ignoreClient: this.whoAmI, number: (int) int16_1, number2: (float) int32_2, number3: (float) int32_3);
            return;
          case 11:
            if (Game1.netMode != 1)
              return;
            int int16_2 = (int) BitConverter.ToInt16(this.readBuffer, index1);
            int startIndex9 = index1 + 4;
            int int16_3 = (int) BitConverter.ToInt16(this.readBuffer, startIndex9);
            int startIndex10 = startIndex9 + 4;
            int int16_4 = (int) BitConverter.ToInt16(this.readBuffer, startIndex10);
            int startIndex11 = startIndex10 + 4;
            int int16_5 = (int) BitConverter.ToInt16(this.readBuffer, startIndex11);
            num1 = startIndex11 + 4;
            WorldGen.SectionTileFrame(int16_2, int16_3, int16_4, int16_5);
            if (Netplay.clientSock.state == 6)
            {
              Netplay.clientSock.state = 10;
              Game1.player[Game1.myPlayer].Spawn();
            }
            return;
          case 12:
            int index34 = (int) this.readBuffer[index1];
            Game1.player[index34].Spawn();
            if (Game1.netMode != 2 || Netplay.serverSock[this.whoAmI].state < 3)
              return;
            NetMessage.buffer[this.whoAmI].broadcast = true;
            NetMessage.SendData(12, ignoreClient: this.whoAmI, number: this.whoAmI);
            if (Netplay.serverSock[this.whoAmI].state == 3)
            {
              Netplay.serverSock[this.whoAmI].state = 10;
              NetMessage.greetPlayer(this.whoAmI);
              NetMessage.syncPlayers();
            }
            return;
          case 13:
            int number4 = (int) this.readBuffer[index1];
            if (Game1.netMode == 1 && !Game1.player[number4].active)
              NetMessage.SendData(15);
            int index35 = index1 + 1;
            int num8 = (int) this.readBuffer[index35];
            int index36 = index35 + 1;
            int num9 = (int) this.readBuffer[index36];
            int startIndex12 = index36 + 1;
            float single1 = BitConverter.ToSingle(this.readBuffer, startIndex12);
            int startIndex13 = startIndex12 + 4;
            float single2 = BitConverter.ToSingle(this.readBuffer, startIndex13);
            int startIndex14 = startIndex13 + 4;
            float single3 = BitConverter.ToSingle(this.readBuffer, startIndex14);
            int startIndex15 = startIndex14 + 4;
            float single4 = BitConverter.ToSingle(this.readBuffer, startIndex15);
            num1 = startIndex15 + 4;
            Game1.player[number4].selectedItem = num9;
            Game1.player[number4].position.X = single1;
            Game1.player[number4].position.Y = single2;
            Game1.player[number4].velocity.X = single3;
            Game1.player[number4].velocity.Y = single4;
            Game1.player[number4].oldVelocity = Game1.player[number4].velocity;
            Game1.player[number4].fallStart = (int) ((double) single2 / 16.0);
            Game1.player[number4].controlUp = false;
            Game1.player[number4].controlDown = false;
            Game1.player[number4].controlLeft = false;
            Game1.player[number4].controlRight = false;
            Game1.player[number4].controlJump = false;
            Game1.player[number4].controlUseItem = false;
            Game1.player[number4].direction = -1;
            if ((num8 & 1) == 1)
              Game1.player[number4].controlUp = true;
            if ((num8 & 2) == 2)
              Game1.player[number4].controlDown = true;
            if ((num8 & 4) == 4)
              Game1.player[number4].controlLeft = true;
            if ((num8 & 8) == 8)
              Game1.player[number4].controlRight = true;
            if ((num8 & 16) == 16)
              Game1.player[number4].controlJump = true;
            if ((num8 & 32) == 32)
              Game1.player[number4].controlUseItem = true;
            if ((num8 & 64) == 64)
              Game1.player[number4].direction = 1;
            if (Game1.netMode != 2 || Netplay.serverSock[this.whoAmI].state != 10)
              return;
            NetMessage.SendData(13, ignoreClient: this.whoAmI, number: number4);
            return;
          case 14:
            if (Game1.netMode != 1)
              return;
            int index37 = (int) this.readBuffer[index1];
            if (this.readBuffer[index1 + 1] == (byte) 1)
            {
              if (Game1.player[index37].active)
                Game1.player[index37] = new Player();
              Game1.player[index37].active = true;
            }
            else
              Game1.player[index37].active = false;
            return;
          case 15:
            if (Game1.netMode != 2)
              return;
            NetMessage.syncPlayers();
            return;
          case 16:
            int whoAmI3 = (int) this.readBuffer[index1];
            int startIndex16 = index1 + 1;
            int int16_6 = (int) BitConverter.ToInt16(this.readBuffer, startIndex16);
            int int16_7 = (int) BitConverter.ToInt16(this.readBuffer, startIndex16 + 2);
            if (Game1.netMode == 2)
              whoAmI3 = this.whoAmI;
            Game1.player[whoAmI3].statLife = int16_6;
            Game1.player[whoAmI3].statLifeMax = int16_7;
            if (Game1.player[whoAmI3].statLife <= 0)
              Game1.player[whoAmI3].dead = true;
            if (Game1.netMode != 2)
              return;
            if (Game1.player[this.whoAmI].statLifeMax > 400 || Game1.player[this.whoAmI].statLife > Game1.player[this.whoAmI].statLifeMax)
              NetMessage.SendData(2, this.whoAmI, text: "Cheating attempt detected.");
            NetMessage.SendData(16, ignoreClient: this.whoAmI, number: whoAmI3);
            return;
          case 17:
            byte number5 = this.readBuffer[index1];
            int startIndex17 = index1 + 1;
            int int32_4 = BitConverter.ToInt32(this.readBuffer, startIndex17);
            int startIndex18 = startIndex17 + 4;
            int int32_5 = BitConverter.ToInt32(this.readBuffer, startIndex18);
            byte num10 = this.readBuffer[startIndex18 + 4];
            bool fail = false;
            if (num10 == (byte) 1)
              fail = true;
            if (Game1.tile[int32_4, int32_5] == null)
              Game1.tile[int32_4, int32_5] = new Tile();
            switch (number5)
            {
              case 0:
                WorldGen.KillTile(int32_4, int32_5, fail);
                break;
              case 1:
                WorldGen.PlaceTile(int32_4, int32_5, (int) num10, forced: true);
                break;
              case 2:
                WorldGen.KillWall(int32_4, int32_5, fail);
                break;
              case 3:
                WorldGen.PlaceWall(int32_4, int32_5, (int) num10);
                break;
              case 4:
                WorldGen.KillTile(int32_4, int32_5, fail, noItem: true);
                break;
            }
            if (Game1.netMode != 2)
              return;
            NetMessage.SendData(17, ignoreClient: this.whoAmI, number: (int) number5, number2: (float) int32_4, number3: (float) int32_5, number4: (float) num10);
            if (number5 == (byte) 1 && num10 == (byte) 53)
              NetMessage.SendTileSquare(-1, int32_4, int32_5, 1);
            return;
          case 18:
            byte num11 = this.readBuffer[index1];
            int startIndex19 = index1 + 1;
            int int32_6 = BitConverter.ToInt32(this.readBuffer, startIndex19);
            int startIndex20 = startIndex19 + 4;
            short int16_8 = BitConverter.ToInt16(this.readBuffer, startIndex20);
            int startIndex21 = startIndex20 + 2;
            short int16_9 = BitConverter.ToInt16(this.readBuffer, startIndex21);
            num1 = startIndex21 + 2;
            Game1.dayTime = num11 == (byte) 1;
            Game1.time = (double) int32_6;
            Game1.sunModY = int16_8;
            Game1.moonModY = int16_9;
            if (Game1.netMode != 2)
              return;
            NetMessage.SendData(18, ignoreClient: this.whoAmI);
            return;
          case 19:
            byte number6 = this.readBuffer[index1];
            int startIndex22 = index1 + 1;
            int int32_7 = BitConverter.ToInt32(this.readBuffer, startIndex22);
            int startIndex23 = startIndex22 + 4;
            int int32_8 = BitConverter.ToInt32(this.readBuffer, startIndex23);
            int number4_1 = (int) this.readBuffer[startIndex23 + 4];
            int direction = 0;
            if (number4_1 == 0)
              direction = -1;
            switch (number6)
            {
              case 0:
                WorldGen.OpenDoor(int32_7, int32_8, direction);
                break;
              case 1:
                WorldGen.CloseDoor(int32_7, int32_8, true);
                break;
            }
            if (Game1.netMode != 2)
              return;
            NetMessage.SendData(19, ignoreClient: this.whoAmI, number: (int) number6, number2: (float) int32_7, number3: (float) int32_8, number4: (float) number4_1);
            return;
          case 20:
            short int16_10 = BitConverter.ToInt16(this.readBuffer, start + 1);
            int int32_9 = BitConverter.ToInt32(this.readBuffer, start + 3);
            int int32_10 = BitConverter.ToInt32(this.readBuffer, start + 7);
            int startIndex24 = start + 11;
            for (int index38 = int32_9; index38 < int32_9 + (int) int16_10; ++index38)
            {
              for (int index39 = int32_10; index39 < int32_10 + (int) int16_10; ++index39)
              {
                if (Game1.tile[index38, index39] == null)
                  Game1.tile[index38, index39] = new Tile();
                byte num12 = this.readBuffer[startIndex24];
                ++startIndex24;
                bool active = Game1.tile[index38, index39].active;
                Game1.tile[index38, index39].active = ((int) num12 & 1) == 1;
                if (((int) num12 & 2) == 2)
                  Game1.tile[index38, index39].lighted = true;
                Game1.tile[index38, index39].wall = ((int) num12 & 4) != 4 ? (byte) 0 : (byte) 1;
                Game1.tile[index38, index39].liquid = ((int) num12 & 8) != 8 ? (byte) 0 : (byte) 1;
                if (Game1.tile[index38, index39].active)
                {
                  int type = (int) Game1.tile[index38, index39].type;
                  Game1.tile[index38, index39].type = this.readBuffer[startIndex24];
                  ++startIndex24;
                  if (Game1.tileFrameImportant[(int) Game1.tile[index38, index39].type])
                  {
                    Game1.tile[index38, index39].frameX = BitConverter.ToInt16(this.readBuffer, startIndex24);
                    int startIndex25 = startIndex24 + 2;
                    Game1.tile[index38, index39].frameY = BitConverter.ToInt16(this.readBuffer, startIndex25);
                    startIndex24 = startIndex25 + 2;
                  }
                  else if (!active || (int) Game1.tile[index38, index39].type != type)
                  {
                    Game1.tile[index38, index39].frameX = (short) -1;
                    Game1.tile[index38, index39].frameY = (short) -1;
                  }
                }
                if (Game1.tile[index38, index39].wall > (byte) 0)
                {
                  Game1.tile[index38, index39].wall = this.readBuffer[startIndex24];
                  ++startIndex24;
                }
                if (Game1.tile[index38, index39].liquid > (byte) 0)
                {
                  Game1.tile[index38, index39].liquid = this.readBuffer[startIndex24];
                  int index40 = startIndex24 + 1;
                  byte num13 = this.readBuffer[index40];
                  startIndex24 = index40 + 1;
                  Game1.tile[index38, index39].lava = num13 == (byte) 1;
                }
              }
            }
            WorldGen.RangeFrame(int32_9, int32_10, int32_9 + (int) int16_10, int32_10 + (int) int16_10);
            if (Game1.netMode != 2)
              return;
            NetMessage.SendData((int) msgType, ignoreClient: this.whoAmI, number: (int) int16_10, number2: (float) int32_9, number3: (float) int32_10);
            return;
          case 21:
            short index41 = BitConverter.ToInt16(this.readBuffer, index1);
            int startIndex26 = index1 + 2;
            float single5 = BitConverter.ToSingle(this.readBuffer, startIndex26);
            int startIndex27 = startIndex26 + 4;
            float single6 = BitConverter.ToSingle(this.readBuffer, startIndex27);
            int startIndex28 = startIndex27 + 4;
            float single7 = BitConverter.ToSingle(this.readBuffer, startIndex28);
            int startIndex29 = startIndex28 + 4;
            float single8 = BitConverter.ToSingle(this.readBuffer, startIndex29);
            int index42 = startIndex29 + 4;
            byte Stack = this.readBuffer[index42];
            int index43 = index42 + 1;
            string ItemName1 = Encoding.ASCII.GetString(this.readBuffer, index43, length - index43 + start);
            if (Game1.netMode == 1)
            {
              if (ItemName1 == "0")
              {
                Game1.item[(int) index41].active = false;
                return;
              }
              Game1.item[(int) index41].SetDefaults(ItemName1);
              Game1.item[(int) index41].stack = (int) Stack;
              Game1.item[(int) index41].position.X = single5;
              Game1.item[(int) index41].position.Y = single6;
              Game1.item[(int) index41].velocity.X = single7;
              Game1.item[(int) index41].velocity.Y = single8;
              Game1.item[(int) index41].active = true;
              Game1.item[(int) index41].wet = Collision.WetCollision(Game1.item[(int) index41].position, Game1.item[(int) index41].width, Game1.item[(int) index41].height);
              return;
            }
            if (ItemName1 == "0")
            {
              if (index41 < (short) 200)
              {
                Game1.item[(int) index41].active = false;
                NetMessage.SendData(21, number: (int) index41);
              }
            }
            else
            {
              bool flag2 = false;
              if (index41 == (short) 200)
                flag2 = true;
              if (flag2)
              {
                Item obj = new Item();
                obj.SetDefaults(ItemName1);
                index41 = (short) Item.NewItem((int) single5, (int) single6, obj.width, obj.height, obj.type, (int) Stack, true);
              }
              Game1.item[(int) index41].SetDefaults(ItemName1);
              Game1.item[(int) index41].stack = (int) Stack;
              Game1.item[(int) index41].position.X = single5;
              Game1.item[(int) index41].position.Y = single6;
              Game1.item[(int) index41].velocity.X = single7;
              Game1.item[(int) index41].velocity.Y = single8;
              Game1.item[(int) index41].active = true;
              Game1.item[(int) index41].owner = Game1.myPlayer;
              if (flag2)
              {
                NetMessage.SendData(21, number: (int) index41);
                Game1.item[(int) index41].ownIgnore = this.whoAmI;
                Game1.item[(int) index41].ownTime = 100;
                Game1.item[(int) index41].FindOwner((int) index41);
              }
              else
                NetMessage.SendData(21, ignoreClient: this.whoAmI, number: (int) index41);
            }
            return;
          case 22:
            short int16_11 = BitConverter.ToInt16(this.readBuffer, index1);
            byte num14 = this.readBuffer[index1 + 2];
            Game1.item[(int) int16_11].owner = (int) num14;
            Game1.item[(int) int16_11].keepTime = (int) num14 != Game1.myPlayer ? 0 : 15;
            if (Game1.netMode != 2)
              return;
            Game1.item[(int) int16_11].owner = 8;
            Game1.item[(int) int16_11].keepTime = 15;
            NetMessage.SendData(22, number: (int) int16_11);
            return;
          case 23:
            short int16_12 = BitConverter.ToInt16(this.readBuffer, index1);
            int startIndex30 = index1 + 2;
            float single9 = BitConverter.ToSingle(this.readBuffer, startIndex30);
            int startIndex31 = startIndex30 + 4;
            float single10 = BitConverter.ToSingle(this.readBuffer, startIndex31);
            int startIndex32 = startIndex31 + 4;
            float single11 = BitConverter.ToSingle(this.readBuffer, startIndex32);
            int startIndex33 = startIndex32 + 4;
            float single12 = BitConverter.ToSingle(this.readBuffer, startIndex33);
            int startIndex34 = startIndex33 + 4;
            int int16_13 = (int) BitConverter.ToInt16(this.readBuffer, startIndex34);
            int index44 = startIndex34 + 2;
            int num15 = (int) this.readBuffer[index44] - 1;
            int index45 = index44 + 1;
            int num16 = (int) this.readBuffer[index45] - 1;
            int startIndex35 = index45 + 1;
            int int16_14 = (int) BitConverter.ToInt16(this.readBuffer, startIndex35);
            int num17 = startIndex35 + 2;
            float[] numArray1 = new float[NPC.maxAI];
            for (int index46 = 0; index46 < NPC.maxAI; ++index46)
            {
              numArray1[index46] = BitConverter.ToSingle(this.readBuffer, num17);
              num17 += 4;
            }
            string Name = Encoding.ASCII.GetString(this.readBuffer, num17, length - num17 + start);
            if (!Game1.npc[(int) int16_12].active || Game1.npc[(int) int16_12].name != Name)
            {
              Game1.npc[(int) int16_12].active = true;
              Game1.npc[(int) int16_12].SetDefaults(Name);
            }
            Game1.npc[(int) int16_12].position.X = single9;
            Game1.npc[(int) int16_12].position.Y = single10;
            Game1.npc[(int) int16_12].velocity.X = single11;
            Game1.npc[(int) int16_12].velocity.Y = single12;
            Game1.npc[(int) int16_12].target = int16_13;
            Game1.npc[(int) int16_12].direction = num15;
            Game1.npc[(int) int16_12].life = int16_14;
            if (int16_14 <= 0)
              Game1.npc[(int) int16_12].active = false;
            for (int index47 = 0; index47 < NPC.maxAI; ++index47)
              Game1.npc[(int) int16_12].ai[index47] = numArray1[index47];
            return;
          case 24:
            short int16_15 = BitConverter.ToInt16(this.readBuffer, index1);
            byte number2_2 = this.readBuffer[index1 + 2];
            Game1.npc[(int) int16_15].StrikeNPC(Game1.player[(int) number2_2].inventory[Game1.player[(int) number2_2].selectedItem].damage, Game1.player[(int) number2_2].inventory[Game1.player[(int) number2_2].selectedItem].knockBack, Game1.player[(int) number2_2].direction);
            if (Game1.netMode != 2)
              return;
            NetMessage.SendData(24, ignoreClient: this.whoAmI, number: (int) int16_15, number2: (float) number2_2);
            NetMessage.SendData(23, number: (int) int16_15);
            return;
          case 25:
            int whoAmI4 = (int) this.readBuffer[start + 1];
            if (Game1.netMode == 2)
              whoAmI4 = this.whoAmI;
            byte num18 = this.readBuffer[start + 2];
            byte num19 = this.readBuffer[start + 3];
            byte num20 = this.readBuffer[start + 4];
            string text2 = Encoding.ASCII.GetString(this.readBuffer, start + 5, length - 5);
            if (Game1.netMode == 1)
            {
              string newText = text2;
              if (whoAmI4 < 8)
              {
                newText = "<" + Game1.player[whoAmI4].name + "> " + text2;
                Game1.player[whoAmI4].chatText = text2;
                Game1.player[whoAmI4].chatShowTime = Game1.chatLength / 2;
              }
              Game1.NewText(newText, num18, num19, num20);
              return;
            }
            if (Game1.netMode != 2)
              return;
            string lower = text2.ToLower();
            if (lower == "/playing")
            {
              string str3 = "";
              for (int index48 = 0; index48 < 8; ++index48)
              {
                if (Game1.player[index48].active)
                  str3 = !(str3 == "") ? str3 + ", " + Game1.player[index48].name : str3 + Game1.player[index48].name;
              }
              NetMessage.SendData(25, this.whoAmI, text: "Current players: " + str3 + ".", number: 8, number2: (float) byte.MaxValue, number3: 240f, number4: 20f);
            }
            else if (lower.Length >= 4 && lower.Substring(0, 4) == "/me ")
              NetMessage.SendData(25, text: "*" + Game1.player[this.whoAmI].name + " " + text2.Substring(4), number: 8, number2: 200f, number3: 100f);
            else if (lower.Length >= 3 && lower.Substring(0, 3) == "/p ")
            {
              if (Game1.player[this.whoAmI].team != 0)
              {
                for (int remoteClient = 0; remoteClient < 8; ++remoteClient)
                {
                  if (Game1.player[remoteClient].team == Game1.player[this.whoAmI].team)
                    NetMessage.SendData(25, remoteClient, text: text2.Substring(3), number: whoAmI4, number2: (float) Game1.teamColor[Game1.player[this.whoAmI].team].R, number3: (float) Game1.teamColor[Game1.player[this.whoAmI].team].G, number4: (float) Game1.teamColor[Game1.player[this.whoAmI].team].B);
                }
              }
              else
                NetMessage.SendData(25, this.whoAmI, text: "You are not in a party!", number: 8, number2: (float) byte.MaxValue, number3: 240f, number4: 20f);
            }
            else
              NetMessage.SendData(25, text: text2, number: whoAmI4, number2: (float) num18, number3: (float) num19, number4: (float) num20);
            return;
          case 26:
            byte number7 = this.readBuffer[index1];
            int index49 = index1 + 1;
            int num21 = (int) this.readBuffer[index49] - 1;
            int startIndex36 = index49 + 1;
            short int16_16 = BitConverter.ToInt16(this.readBuffer, startIndex36);
            byte number4_2 = this.readBuffer[startIndex36 + 2];
            bool pvp1 = false;
            if (number4_2 != (byte) 0)
              pvp1 = true;
            Game1.player[(int) number7].Hurt((int) int16_16, num21, pvp1, true);
            if (Game1.netMode != 2)
              return;
            NetMessage.SendData(26, ignoreClient: this.whoAmI, number: (int) number7, number2: (float) num21, number3: (float) int16_16, number4: (float) number4_2);
            return;
          case 27:
            short int16_17 = BitConverter.ToInt16(this.readBuffer, index1);
            int startIndex37 = index1 + 2;
            float single13 = BitConverter.ToSingle(this.readBuffer, startIndex37);
            int startIndex38 = startIndex37 + 4;
            float single14 = BitConverter.ToSingle(this.readBuffer, startIndex38);
            int startIndex39 = startIndex38 + 4;
            float single15 = BitConverter.ToSingle(this.readBuffer, startIndex39);
            int startIndex40 = startIndex39 + 4;
            float single16 = BitConverter.ToSingle(this.readBuffer, startIndex40);
            int startIndex41 = startIndex40 + 4;
            float single17 = BitConverter.ToSingle(this.readBuffer, startIndex41);
            int startIndex42 = startIndex41 + 4;
            short int16_18 = BitConverter.ToInt16(this.readBuffer, startIndex42);
            int index50 = startIndex42 + 2;
            byte num22 = this.readBuffer[index50];
            int index51 = index50 + 1;
            byte Type = this.readBuffer[index51];
            int startIndex43 = index51 + 1;
            float[] numArray2 = new float[Projectile.maxAI];
            for (int index52 = 0; index52 < Projectile.maxAI; ++index52)
            {
              numArray2[index52] = BitConverter.ToSingle(this.readBuffer, startIndex43);
              startIndex43 += 4;
            }
            int number8 = 1000;
            for (int index53 = 0; index53 < 1000; ++index53)
            {
              if (Game1.projectile[index53].owner == (int) num22 && Game1.projectile[index53].identity == (int) int16_17 && Game1.projectile[index53].active)
              {
                number8 = index53;
                break;
              }
            }
            if (number8 == 1000)
            {
              for (int index54 = 0; index54 < 1000; ++index54)
              {
                if (!Game1.projectile[index54].active)
                {
                  number8 = index54;
                  break;
                }
              }
            }
            if (!Game1.projectile[number8].active || Game1.projectile[number8].type != (int) Type)
              Game1.projectile[number8].SetDefaults((int) Type);
            Game1.projectile[number8].identity = (int) int16_17;
            Game1.projectile[number8].position.X = single13;
            Game1.projectile[number8].position.Y = single14;
            Game1.projectile[number8].velocity.X = single15;
            Game1.projectile[number8].velocity.Y = single16;
            Game1.projectile[number8].damage = (int) int16_18;
            Game1.projectile[number8].type = (int) Type;
            Game1.projectile[number8].owner = (int) num22;
            Game1.projectile[number8].knockBack = single17;
            for (int index55 = 0; index55 < Projectile.maxAI; ++index55)
              Game1.projectile[number8].ai[index55] = numArray2[index55];
            if (Game1.netMode != 2)
              return;
            NetMessage.SendData(27, ignoreClient: this.whoAmI, number: number8);
            return;
          case 28:
            short int16_19 = BitConverter.ToInt16(this.readBuffer, index1);
            int startIndex44 = index1 + 2;
            short int16_20 = BitConverter.ToInt16(this.readBuffer, startIndex44);
            int startIndex45 = startIndex44 + 2;
            float single18 = BitConverter.ToSingle(this.readBuffer, startIndex45);
            int num23 = (int) this.readBuffer[startIndex45 + 4] - 1;
            if (int16_20 >= (short) 0)
            {
              Game1.npc[(int) int16_19].StrikeNPC((int) int16_20, single18, num23);
            }
            else
            {
              Game1.npc[(int) int16_19].life = 0;
              Game1.npc[(int) int16_19].HitEffect();
              Game1.npc[(int) int16_19].active = false;
            }
            if (Game1.netMode != 2)
              return;
            NetMessage.SendData(28, ignoreClient: this.whoAmI, number: (int) int16_19, number2: (float) int16_20, number3: single18, number4: (float) num23);
            NetMessage.SendData(23, number: (int) int16_19);
            return;
          case 29:
            short int16_21 = BitConverter.ToInt16(this.readBuffer, index1);
            byte number2_3 = this.readBuffer[index1 + 2];
            for (int index56 = 0; index56 < 1000; ++index56)
            {
              if (Game1.projectile[index56].owner == (int) number2_3 && Game1.projectile[index56].identity == (int) int16_21 && Game1.projectile[index56].active)
              {
                Game1.projectile[index56].Kill();
                break;
              }
            }
            if (Game1.netMode != 2)
              return;
            NetMessage.SendData(29, ignoreClient: this.whoAmI, number: (int) int16_21, number2: (float) number2_3);
            return;
          case 30:
            byte number9 = this.readBuffer[index1];
            byte num24 = this.readBuffer[index1 + 1];
            Game1.player[(int) number9].hostile = num24 == (byte) 1;
            if (Game1.netMode != 2)
              return;
            NetMessage.SendData(30, ignoreClient: this.whoAmI, number: (int) number9);
            string str4 = " has enabled PvP!";
            if (num24 == (byte) 0)
              str4 = " has disabled PvP!";
            NetMessage.SendData(25, text: Game1.player[(int) number9].name + str4, number: 8, number2: (float) Game1.teamColor[Game1.player[(int) number9].team].R, number3: (float) Game1.teamColor[Game1.player[(int) number9].team].G, number4: (float) Game1.teamColor[Game1.player[(int) number9].team].B);
            return;
          case 31:
            if (Game1.netMode != 2)
              return;
            int int32_11 = BitConverter.ToInt32(this.readBuffer, index1);
            int startIndex46 = index1 + 4;
            int int32_12 = BitConverter.ToInt32(this.readBuffer, startIndex46);
            num1 = startIndex46 + 4;
            int chest = Chest.FindChest(int32_11, int32_12);
            if (chest > -1 && Chest.UsingChest(chest) == -1)
            {
              for (int number2_4 = 0; number2_4 < Chest.maxItems; ++number2_4)
                NetMessage.SendData(32, this.whoAmI, number: chest, number2: (float) number2_4);
              NetMessage.SendData(33, this.whoAmI, number: chest);
              Game1.player[this.whoAmI].chest = chest;
            }
            return;
          case 32:
            int int16_22 = (int) BitConverter.ToInt16(this.readBuffer, index1);
            int index57 = index1 + 2;
            int index58 = (int) this.readBuffer[index57];
            int index59 = index57 + 1;
            int num25 = (int) this.readBuffer[index59];
            int index60 = index59 + 1;
            string ItemName2 = Encoding.ASCII.GetString(this.readBuffer, index60, length - index60 + start);
            if (Game1.chest[int16_22] == null)
              Game1.chest[int16_22] = new Chest();
            if (Game1.chest[int16_22].item[index58] == null)
              Game1.chest[int16_22].item[index58] = new Item();
            Game1.chest[int16_22].item[index58].SetDefaults(ItemName2);
            Game1.chest[int16_22].item[index58].stack = num25;
            return;
          case 33:
            int int16_23 = (int) BitConverter.ToInt16(this.readBuffer, index1);
            int startIndex47 = index1 + 2;
            int int32_13 = BitConverter.ToInt32(this.readBuffer, startIndex47);
            int int32_14 = BitConverter.ToInt32(this.readBuffer, startIndex47 + 4);
            if (Game1.netMode == 1)
            {
              if (Game1.player[Game1.myPlayer].chest == -1)
              {
                Game1.playerInventory = true;
                Game1.PlaySound(10);
              }
              else if (Game1.player[Game1.myPlayer].chest != int16_23 && int16_23 != -1)
              {
                Game1.playerInventory = true;
                Game1.PlaySound(12);
              }
              else if (Game1.player[Game1.myPlayer].chest != -1 && int16_23 == -1)
                Game1.PlaySound(11);
              Game1.player[Game1.myPlayer].chest = int16_23;
              Game1.player[Game1.myPlayer].chestX = int32_13;
              Game1.player[Game1.myPlayer].chestY = int32_14;
              return;
            }
            Game1.player[this.whoAmI].chest = int16_23;
            return;
          case 34:
            if (Game1.netMode != 2)
              return;
            int int32_15 = BitConverter.ToInt32(this.readBuffer, index1);
            int int32_16 = BitConverter.ToInt32(this.readBuffer, index1 + 4);
            WorldGen.KillTile(int32_15, int32_16);
            if (!Game1.tile[int32_15, int32_16].active)
              NetMessage.SendData(17, number2: (float) int32_15, number3: (float) int32_16);
            return;
          case 35:
            int number10 = (int) this.readBuffer[index1];
            int startIndex48 = index1 + 1;
            int int16_24 = (int) BitConverter.ToInt16(this.readBuffer, startIndex48);
            num1 = startIndex48 + 2;
            if (number10 != Game1.myPlayer)
              Game1.player[number10].HealEffect(int16_24);
            if (Game1.netMode != 2)
              return;
            NetMessage.SendData(35, ignoreClient: this.whoAmI, number: number10, number2: (float) int16_24);
            return;
          case 36:
            int index61 = (int) this.readBuffer[index1];
            int index62 = index1 + 1;
            int num26 = (int) this.readBuffer[index62];
            int index63 = index62 + 1;
            int num27 = (int) this.readBuffer[index63];
            int index64 = index63 + 1;
            int num28 = (int) this.readBuffer[index64];
            int index65 = index64 + 1;
            int num29 = (int) this.readBuffer[index65];
            num1 = index65 + 1;
            Game1.player[index61].zoneEvil = num26 != 0;
            Game1.player[index61].zoneMeteor = num27 != 0;
            Game1.player[index61].zoneDungeon = num28 != 0;
            if (num29 == 0)
            {
              Game1.player[index61].zoneJungle = false;
              return;
            }
            Game1.player[index61].zoneJungle = true;
            return;
          case 37:
            if (Game1.netMode != 1)
              return;
            Netplay.password = "";
            Game1.menuMode = 31;
            return;
          case 38:
            if (Game1.netMode != 2)
              return;
            if (Encoding.ASCII.GetString(this.readBuffer, index1, length - index1 + start) == Netplay.password)
            {
              Netplay.serverSock[this.whoAmI].state = 1;
              NetMessage.SendData(3, this.whoAmI);
            }
            else
              NetMessage.SendData(2, this.whoAmI, text: "Incorrect password.");
            return;
          case 39:
            num2 = Game1.netMode != 1 ? 1 : 0;
            break;
          default:
            num2 = 1;
            break;
        }
        if (num2 == 0)
        {
          short int16_25 = BitConverter.ToInt16(this.readBuffer, index1);
          Game1.item[(int) int16_25].owner = 8;
          NetMessage.SendData(22, number: (int) int16_25);
        }
        else
        {
          switch (msgType)
          {
            case 40:
              byte number11 = this.readBuffer[index1];
              int startIndex49 = index1 + 1;
              int int16_26 = (int) BitConverter.ToInt16(this.readBuffer, startIndex49);
              num1 = startIndex49 + 2;
              Game1.player[(int) number11].talkNPC = int16_26;
              if (Game1.netMode != 2)
                break;
              NetMessage.SendData(40, ignoreClient: this.whoAmI, number: (int) number11);
              break;
            case 41:
              byte number12 = this.readBuffer[index1];
              int startIndex50 = index1 + 1;
              float single19 = BitConverter.ToSingle(this.readBuffer, startIndex50);
              int int16_27 = (int) BitConverter.ToInt16(this.readBuffer, startIndex50 + 4);
              Game1.player[(int) number12].itemRotation = single19;
              Game1.player[(int) number12].itemAnimation = int16_27;
              if (Game1.netMode != 2)
                break;
              NetMessage.SendData(41, ignoreClient: this.whoAmI, number: (int) number12);
              break;
            case 42:
              int whoAmI5 = (int) this.readBuffer[index1];
              int startIndex51 = index1 + 1;
              int int16_28 = (int) BitConverter.ToInt16(this.readBuffer, startIndex51);
              int int16_29 = (int) BitConverter.ToInt16(this.readBuffer, startIndex51 + 2);
              if (Game1.netMode == 2)
                whoAmI5 = this.whoAmI;
              Game1.player[whoAmI5].statMana = int16_28;
              Game1.player[whoAmI5].statManaMax = int16_29;
              if (Game1.netMode != 2)
                break;
              NetMessage.SendData(42, ignoreClient: this.whoAmI, number: whoAmI5);
              break;
            case 43:
              int number13 = (int) this.readBuffer[index1];
              int startIndex52 = index1 + 1;
              int int16_30 = (int) BitConverter.ToInt16(this.readBuffer, startIndex52);
              num1 = startIndex52 + 2;
              if (number13 != Game1.myPlayer)
                Game1.player[number13].ManaEffect(int16_30);
              if (Game1.netMode != 2)
                break;
              NetMessage.SendData(43, ignoreClient: this.whoAmI, number: number13, number2: (float) int16_30);
              break;
            case 44:
              byte number14 = this.readBuffer[index1];
              int index66 = index1 + 1;
              int num30 = (int) this.readBuffer[index66] - 1;
              int startIndex53 = index66 + 1;
              short int16_31 = BitConverter.ToInt16(this.readBuffer, startIndex53);
              byte number4_3 = this.readBuffer[startIndex53 + 2];
              bool pvp2 = false;
              if (number4_3 != (byte) 0)
                pvp2 = true;
              Game1.player[(int) number14].KillMe((double) int16_31, num30, pvp2);
              if (Game1.netMode != 2)
                break;
              NetMessage.SendData(44, ignoreClient: this.whoAmI, number: (int) number14, number2: (float) num30, number3: (float) int16_31, number4: (float) number4_3);
              break;
            case 45:
              int number15 = (int) this.readBuffer[index1];
              int index67 = index1 + 1;
              int index68 = (int) this.readBuffer[index67];
              num1 = index67 + 1;
              int team = Game1.player[number15].team;
              Game1.player[number15].team = index68;
              if (Game1.netMode != 2)
                break;
              NetMessage.SendData(45, ignoreClient: this.whoAmI, number: number15);
              string str5 = "";
              switch (index68)
              {
                case 0:
                  str5 = " is no longer on a party.";
                  break;
                case 1:
                  str5 = " has joined the red party.";
                  break;
                case 2:
                  str5 = " has joined the green party.";
                  break;
                case 3:
                  str5 = " has joined the blue party.";
                  break;
                case 4:
                  str5 = " has joined the yellow party.";
                  break;
              }
              for (int remoteClient = 0; remoteClient < 8; ++remoteClient)
              {
                if (remoteClient == this.whoAmI || team > 0 && Game1.player[remoteClient].team == team || index68 > 0 && Game1.player[remoteClient].team == index68)
                  NetMessage.SendData(25, remoteClient, text: Game1.player[number15].name + str5, number: 8, number2: (float) Game1.teamColor[index68].R, number3: (float) Game1.teamColor[index68].G, number4: (float) Game1.teamColor[index68].B);
              }
              break;
            case 46:
              if (Game1.netMode != 2)
                break;
              int int32_17 = BitConverter.ToInt32(this.readBuffer, index1);
              int startIndex54 = index1 + 4;
              int int32_18 = BitConverter.ToInt32(this.readBuffer, startIndex54);
              num1 = startIndex54 + 4;
              int number16 = Sign.ReadSign(int32_17, int32_18);
              if (number16 >= 0)
                NetMessage.SendData(47, this.whoAmI, number: number16);
              break;
            case 47:
              int int16_32 = (int) BitConverter.ToInt16(this.readBuffer, index1);
              int startIndex55 = index1 + 2;
              int int32_19 = BitConverter.ToInt32(this.readBuffer, startIndex55);
              int startIndex56 = startIndex55 + 4;
              int int32_20 = BitConverter.ToInt32(this.readBuffer, startIndex56);
              int index69 = startIndex56 + 4;
              string text3 = Encoding.ASCII.GetString(this.readBuffer, index69, length - index69 + start);
              Game1.sign[int16_32] = new Sign();
              Game1.sign[int16_32].x = int32_19;
              Game1.sign[int16_32].y = int32_20;
              Sign.TextSign(int16_32, text3);
              if (Game1.netMode != 1 || Game1.sign[int16_32] == null || int16_32 == Game1.player[Game1.myPlayer].sign)
                break;
              Game1.playerInventory = false;
              Game1.player[Game1.myPlayer].talkNPC = -1;
              Game1.editSign = false;
              Game1.PlaySound(10);
              Game1.player[Game1.myPlayer].sign = int16_32;
              Game1.npcChatText = Game1.sign[int16_32].text;
              break;
            case 48:
              int int32_21 = BitConverter.ToInt32(this.readBuffer, index1);
              int startIndex57 = index1 + 4;
              int int32_22 = BitConverter.ToInt32(this.readBuffer, startIndex57);
              int index70 = startIndex57 + 4;
              byte num31 = this.readBuffer[index70];
              int index71 = index70 + 1;
              byte num32 = this.readBuffer[index71];
              num1 = index71 + 1;
              if (Game1.tile[int32_21, int32_22] == null)
                Game1.tile[int32_21, int32_22] = new Tile();
              lock (Game1.tile[int32_21, int32_22])
              {
                Game1.tile[int32_21, int32_22].liquid = num31;
                Game1.tile[int32_21, int32_22].lava = num32 == (byte) 1;
                if (Game1.netMode == 2)
                  Liquid.NetAddWater(int32_21, int32_22);
              }
              break;
          }
        }
      }
    }
  }
}
