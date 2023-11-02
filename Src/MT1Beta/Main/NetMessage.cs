// Decompiled with JetBrains decompiler
// Type: GameManager.NetMessage
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 09A1FDD7-68B3-40FF-A030-32890DF6B0E7
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager v1 Beta\GameManager.exe

using System;
using System.Diagnostics;
using System.Text;

namespace GameManager
{
  public class NetMessage
  {
    public static messageBuffer[] buffer = new messageBuffer[10];

    public static void SendData(
      int msgType,
      int remoteClient = -1,
      int ignoreClient = -1,
      string text = "",
      int number = 0,
      float number2 = 0.0f,
      float number3 = 0.0f,
      float number4 = 0.0f)
    {
      int whoAmi = 9;
      if (Game1.netMode == 2 && remoteClient >= 0)
        whoAmi = remoteClient;
      lock (NetMessage.buffer[whoAmi])
      {
        int count = 5;
        int dstOffset1 = count;
        int num1;
        switch (msgType)
        {
          case 1:
            byte[] bytes1 = BitConverter.GetBytes(msgType);
            byte[] bytes2 = Encoding.ASCII.GetBytes("Terraria" + (object) Game1.curRelease);
            count += bytes2.Length;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes1, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            Buffer.BlockCopy((Array) bytes2, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 5, bytes2.Length);
            break;
          case 2:
            byte[] bytes3 = BitConverter.GetBytes(msgType);
            byte[] bytes4 = Encoding.ASCII.GetBytes(text);
            count += bytes4.Length;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes3, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            Buffer.BlockCopy((Array) bytes4, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 5, bytes4.Length);
            break;
          case 3:
            byte[] bytes5 = BitConverter.GetBytes(msgType);
            byte[] bytes6 = BitConverter.GetBytes(remoteClient);
            count += bytes6.Length;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes5, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            Buffer.BlockCopy((Array) bytes6, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 5, bytes6.Length);
            break;
          case 4:
            byte[] bytes7 = BitConverter.GetBytes(msgType);
            byte index1 = (byte) number;
            byte hair = (byte) Game1.player[(int) index1].hair;
            byte[] bytes8 = Encoding.ASCII.GetBytes(text);
            count += 23 + bytes8.Length;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes7, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            NetMessage.buffer[whoAmi].writeBuffer[5] = index1;
            int num2 = dstOffset1 + 1;
            NetMessage.buffer[whoAmi].writeBuffer[6] = hair;
            int index2 = num2 + 1;
            NetMessage.buffer[whoAmi].writeBuffer[index2] = Game1.player[(int) index1].hairColor.R;
            int index3 = index2 + 1;
            NetMessage.buffer[whoAmi].writeBuffer[index3] = Game1.player[(int) index1].hairColor.G;
            int index4 = index3 + 1;
            NetMessage.buffer[whoAmi].writeBuffer[index4] = Game1.player[(int) index1].hairColor.B;
            int index5 = index4 + 1;
            NetMessage.buffer[whoAmi].writeBuffer[index5] = Game1.player[(int) index1].skinColor.R;
            int index6 = index5 + 1;
            NetMessage.buffer[whoAmi].writeBuffer[index6] = Game1.player[(int) index1].skinColor.G;
            int index7 = index6 + 1;
            NetMessage.buffer[whoAmi].writeBuffer[index7] = Game1.player[(int) index1].skinColor.B;
            int index8 = index7 + 1;
            NetMessage.buffer[whoAmi].writeBuffer[index8] = Game1.player[(int) index1].eyeColor.R;
            int index9 = index8 + 1;
            NetMessage.buffer[whoAmi].writeBuffer[index9] = Game1.player[(int) index1].eyeColor.G;
            int index10 = index9 + 1;
            NetMessage.buffer[whoAmi].writeBuffer[index10] = Game1.player[(int) index1].eyeColor.B;
            int index11 = index10 + 1;
            NetMessage.buffer[whoAmi].writeBuffer[index11] = Game1.player[(int) index1].shirtColor.R;
            int index12 = index11 + 1;
            NetMessage.buffer[whoAmi].writeBuffer[index12] = Game1.player[(int) index1].shirtColor.G;
            int index13 = index12 + 1;
            NetMessage.buffer[whoAmi].writeBuffer[index13] = Game1.player[(int) index1].shirtColor.B;
            int index14 = index13 + 1;
            NetMessage.buffer[whoAmi].writeBuffer[index14] = Game1.player[(int) index1].underShirtColor.R;
            int index15 = index14 + 1;
            NetMessage.buffer[whoAmi].writeBuffer[index15] = Game1.player[(int) index1].underShirtColor.G;
            int index16 = index15 + 1;
            NetMessage.buffer[whoAmi].writeBuffer[index16] = Game1.player[(int) index1].underShirtColor.B;
            int index17 = index16 + 1;
            NetMessage.buffer[whoAmi].writeBuffer[index17] = Game1.player[(int) index1].pantsColor.R;
            int index18 = index17 + 1;
            NetMessage.buffer[whoAmi].writeBuffer[index18] = Game1.player[(int) index1].pantsColor.G;
            int index19 = index18 + 1;
            NetMessage.buffer[whoAmi].writeBuffer[index19] = Game1.player[(int) index1].pantsColor.B;
            int index20 = index19 + 1;
            NetMessage.buffer[whoAmi].writeBuffer[index20] = Game1.player[(int) index1].shoeColor.R;
            int index21 = index20 + 1;
            NetMessage.buffer[whoAmi].writeBuffer[index21] = Game1.player[(int) index1].shoeColor.G;
            int index22 = index21 + 1;
            NetMessage.buffer[whoAmi].writeBuffer[index22] = Game1.player[(int) index1].shoeColor.B;
            int dstOffset2 = index22 + 1;
            Buffer.BlockCopy((Array) bytes8, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset2, bytes8.Length);
            break;
          case 5:
            byte[] bytes9 = BitConverter.GetBytes(msgType);
            byte num3 = (byte) number;
            byte num4 = (byte) number2;
            byte num5;
            if ((double) number2 < 44.0)
            {
              num5 = (byte) Game1.player[number].inventory[(int) number2].stack;
              if (Game1.player[number].inventory[(int) number2].stack < 0)
                num5 = (byte) 0;
            }
            else
            {
              num5 = (byte) Game1.player[number].armor[(int) number2 - 44].stack;
              if (Game1.player[number].armor[(int) number2 - 44].stack < 0)
                num5 = (byte) 0;
            }
            byte[] bytes10 = Encoding.ASCII.GetBytes(text ?? "");
            count += 3 + bytes10.Length;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes9, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            NetMessage.buffer[whoAmi].writeBuffer[5] = num3;
            int num6 = dstOffset1 + 1;
            NetMessage.buffer[whoAmi].writeBuffer[6] = num4;
            int num7 = num6 + 1;
            NetMessage.buffer[whoAmi].writeBuffer[7] = num5;
            int dstOffset3 = num7 + 1;
            Buffer.BlockCopy((Array) bytes10, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset3, bytes10.Length);
            break;
          case 6:
            byte[] bytes11 = BitConverter.GetBytes(msgType);
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes11, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            break;
          case 7:
            byte[] bytes12 = BitConverter.GetBytes(msgType);
            byte[] bytes13 = BitConverter.GetBytes((int) Game1.time);
            byte num8 = 0;
            if (Game1.dayTime)
              num8 = (byte) 1;
            byte moonPhase = (byte) Game1.moonPhase;
            byte num9 = 0;
            if (Game1.bloodMoon)
              num9 = (byte) 1;
            byte[] bytes14 = BitConverter.GetBytes(Game1.maxTilesX);
            byte[] bytes15 = BitConverter.GetBytes(Game1.maxTilesY);
            byte[] bytes16 = BitConverter.GetBytes(Game1.spawnTileX);
            byte[] bytes17 = BitConverter.GetBytes(Game1.spawnTileY);
            byte[] bytes18 = BitConverter.GetBytes((int) Game1.worldSurface);
            byte[] bytes19 = BitConverter.GetBytes((int) Game1.rockLayer);
            byte[] bytes20 = Encoding.ASCII.GetBytes(Game1.worldName);
            count += bytes13.Length + 1 + 1 + 1 + bytes14.Length + bytes15.Length + bytes16.Length + bytes17.Length + bytes18.Length + bytes19.Length + bytes20.Length;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes12, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            Buffer.BlockCopy((Array) bytes13, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 5, bytes13.Length);
            int index23 = dstOffset1 + bytes13.Length;
            NetMessage.buffer[whoAmi].writeBuffer[index23] = num8;
            int index24 = index23 + 1;
            NetMessage.buffer[whoAmi].writeBuffer[index24] = moonPhase;
            int index25 = index24 + 1;
            NetMessage.buffer[whoAmi].writeBuffer[index25] = num9;
            int dstOffset4 = index25 + 1;
            Buffer.BlockCopy((Array) bytes14, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset4, bytes14.Length);
            int dstOffset5 = dstOffset4 + bytes14.Length;
            Buffer.BlockCopy((Array) bytes15, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset5, bytes15.Length);
            int dstOffset6 = dstOffset5 + bytes15.Length;
            Buffer.BlockCopy((Array) bytes16, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset6, bytes16.Length);
            int dstOffset7 = dstOffset6 + bytes16.Length;
            Buffer.BlockCopy((Array) bytes17, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset7, bytes17.Length);
            int dstOffset8 = dstOffset7 + bytes17.Length;
            Buffer.BlockCopy((Array) bytes18, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset8, bytes18.Length);
            int dstOffset9 = dstOffset8 + bytes18.Length;
            Buffer.BlockCopy((Array) bytes19, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset9, bytes19.Length);
            int dstOffset10 = dstOffset9 + bytes19.Length;
            Buffer.BlockCopy((Array) bytes20, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset10, bytes20.Length);
            num1 = dstOffset10 + bytes20.Length;
            break;
          case 8:
            byte[] bytes21 = BitConverter.GetBytes(msgType);
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes21, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            break;
          case 9:
            byte[] bytes22 = BitConverter.GetBytes(msgType);
            byte[] bytes23 = BitConverter.GetBytes(number);
            byte[] bytes24 = Encoding.ASCII.GetBytes(text);
            count += bytes23.Length + bytes24.Length;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes22, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            Buffer.BlockCopy((Array) bytes23, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset1, 4);
            int dstOffset11 = dstOffset1 + 4;
            Buffer.BlockCopy((Array) bytes24, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset11, bytes24.Length);
            break;
          case 10:
            short num10 = (short) number;
            int num11 = (int) number2;
            int index26 = (int) number3;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(msgType), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            Buffer.BlockCopy((Array) BitConverter.GetBytes(num10), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset1, 2);
            int dstOffset12 = dstOffset1 + 2;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(num11), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset12, 4);
            int dstOffset13 = dstOffset12 + 4;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(index26), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset13, 4);
            int dstOffset14 = dstOffset13 + 4;
            for (int index27 = num11; index27 < num11 + (int) num10; ++index27)
            {
              byte num12 = 0;
              if (Game1.tile[index27, index26].active)
                ++num12;
              if (Game1.tile[index27, index26].lighted)
                num12 += (byte) 2;
              if (Game1.tile[index27, index26].wall > (byte) 0)
                num12 += (byte) 4;
              if (Game1.tile[index27, index26].liquid > (byte) 0)
                num12 += (byte) 8;
              NetMessage.buffer[whoAmi].writeBuffer[dstOffset14] = num12;
              ++dstOffset14;
              byte[] bytes25 = BitConverter.GetBytes(Game1.tile[index27, index26].frameX);
              byte[] bytes26 = BitConverter.GetBytes(Game1.tile[index27, index26].frameY);
              byte wall = Game1.tile[index27, index26].wall;
              if (Game1.tile[index27, index26].active)
              {
                NetMessage.buffer[whoAmi].writeBuffer[dstOffset14] = Game1.tile[index27, index26].type;
                ++dstOffset14;
                if (Game1.tileFrameImportant[(int) Game1.tile[index27, index26].type])
                {
                  Buffer.BlockCopy((Array) bytes25, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset14, 2);
                  int dstOffset15 = dstOffset14 + 2;
                  Buffer.BlockCopy((Array) bytes26, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset15, 2);
                  dstOffset14 = dstOffset15 + 2;
                }
              }
              if (wall > (byte) 0)
              {
                NetMessage.buffer[whoAmi].writeBuffer[dstOffset14] = wall;
                ++dstOffset14;
              }
              if (Game1.tile[index27, index26].liquid > (byte) 0)
              {
                NetMessage.buffer[whoAmi].writeBuffer[dstOffset14] = Game1.tile[index27, index26].liquid;
                int index28 = dstOffset14 + 1;
                byte num13 = 0;
                if (Game1.tile[index27, index26].lava)
                  num13 = (byte) 1;
                NetMessage.buffer[whoAmi].writeBuffer[index28] = num13;
                dstOffset14 = index28 + 1;
              }
            }
            Buffer.BlockCopy((Array) BitConverter.GetBytes(dstOffset14 - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            count = dstOffset14;
            break;
          case 11:
            byte[] bytes27 = BitConverter.GetBytes(msgType);
            byte[] bytes28 = BitConverter.GetBytes(number);
            byte[] bytes29 = BitConverter.GetBytes((int) number2);
            byte[] bytes30 = BitConverter.GetBytes((int) number3);
            byte[] bytes31 = BitConverter.GetBytes((int) number4);
            count += bytes28.Length + bytes29.Length + bytes30.Length + bytes31.Length;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes27, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            Buffer.BlockCopy((Array) bytes28, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset1, 4);
            int dstOffset16 = dstOffset1 + 4;
            Buffer.BlockCopy((Array) bytes29, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset16, 4);
            int dstOffset17 = dstOffset16 + 4;
            Buffer.BlockCopy((Array) bytes30, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset17, 4);
            int dstOffset18 = dstOffset17 + 4;
            Buffer.BlockCopy((Array) bytes31, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset18, 4);
            num1 = dstOffset18 + 4;
            break;
          case 12:
            byte[] bytes32 = BitConverter.GetBytes(msgType);
            byte num14 = (byte) number;
            ++count;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes32, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            NetMessage.buffer[whoAmi].writeBuffer[dstOffset1] = num14;
            break;
          case 13:
            byte[] bytes33 = BitConverter.GetBytes(msgType);
            byte index29 = (byte) number;
            byte num15 = 0;
            if (Game1.player[(int) index29].controlUp)
              ++num15;
            if (Game1.player[(int) index29].controlDown)
              num15 += (byte) 2;
            if (Game1.player[(int) index29].controlLeft)
              num15 += (byte) 4;
            if (Game1.player[(int) index29].controlRight)
              num15 += (byte) 8;
            if (Game1.player[(int) index29].controlJump)
              num15 += (byte) 16;
            if (Game1.player[(int) index29].controlUseItem)
              num15 += (byte) 32;
            if (Game1.player[(int) index29].direction == 1)
              num15 += (byte) 64;
            byte selectedItem = (byte) Game1.player[(int) index29].selectedItem;
            byte[] bytes34 = BitConverter.GetBytes(Game1.player[number].position.X);
            byte[] bytes35 = BitConverter.GetBytes(Game1.player[number].position.Y);
            byte[] bytes36 = BitConverter.GetBytes(Game1.player[number].velocity.X);
            byte[] bytes37 = BitConverter.GetBytes(Game1.player[number].velocity.Y);
            count += 3 + bytes34.Length + bytes35.Length + bytes36.Length + bytes37.Length;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes33, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            NetMessage.buffer[whoAmi].writeBuffer[5] = index29;
            int num16 = dstOffset1 + 1;
            NetMessage.buffer[whoAmi].writeBuffer[6] = num15;
            int num17 = num16 + 1;
            NetMessage.buffer[whoAmi].writeBuffer[7] = selectedItem;
            int dstOffset19 = num17 + 1;
            Buffer.BlockCopy((Array) bytes34, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset19, 4);
            int dstOffset20 = dstOffset19 + 4;
            Buffer.BlockCopy((Array) bytes35, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset20, 4);
            int dstOffset21 = dstOffset20 + 4;
            Buffer.BlockCopy((Array) bytes36, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset21, 4);
            int dstOffset22 = dstOffset21 + 4;
            Buffer.BlockCopy((Array) bytes37, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset22, 4);
            break;
          case 14:
            byte[] bytes38 = BitConverter.GetBytes(msgType);
            byte num18 = (byte) number;
            byte num19 = (byte) number2;
            count += 2;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes38, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            NetMessage.buffer[whoAmi].writeBuffer[5] = num18;
            NetMessage.buffer[whoAmi].writeBuffer[6] = num19;
            break;
          case 15:
            byte[] bytes39 = BitConverter.GetBytes(msgType);
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes39, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            break;
          case 16:
            byte[] bytes40 = BitConverter.GetBytes(msgType);
            byte index30 = (byte) number;
            byte[] bytes41 = BitConverter.GetBytes((short) Game1.player[(int) index30].statLife);
            byte[] bytes42 = BitConverter.GetBytes((short) Game1.player[(int) index30].statLifeMax);
            count += 1 + bytes41.Length + bytes42.Length;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes40, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            NetMessage.buffer[whoAmi].writeBuffer[5] = index30;
            int dstOffset23 = dstOffset1 + 1;
            Buffer.BlockCopy((Array) bytes41, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset23, 2);
            int dstOffset24 = dstOffset23 + 2;
            Buffer.BlockCopy((Array) bytes42, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset24, 2);
            break;
          case 17:
            byte[] bytes43 = BitConverter.GetBytes(msgType);
            byte num20 = (byte) number;
            byte[] bytes44 = BitConverter.GetBytes((int) number2);
            byte[] bytes45 = BitConverter.GetBytes((int) number3);
            byte num21 = (byte) number4;
            count += 1 + bytes44.Length + bytes45.Length + 1;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes43, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            NetMessage.buffer[whoAmi].writeBuffer[dstOffset1] = num20;
            int dstOffset25 = dstOffset1 + 1;
            Buffer.BlockCopy((Array) bytes44, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset25, 4);
            int dstOffset26 = dstOffset25 + 4;
            Buffer.BlockCopy((Array) bytes45, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset26, 4);
            int index31 = dstOffset26 + 4;
            NetMessage.buffer[whoAmi].writeBuffer[index31] = num21;
            break;
          case 18:
            byte[] bytes46 = BitConverter.GetBytes(msgType);
            BitConverter.GetBytes((int) Game1.time);
            byte num22 = 0;
            if (Game1.dayTime)
              num22 = (byte) 1;
            byte[] bytes47 = BitConverter.GetBytes((int) Game1.time);
            byte[] bytes48 = BitConverter.GetBytes(Game1.sunModY);
            byte[] bytes49 = BitConverter.GetBytes(Game1.moonModY);
            count += 1 + bytes47.Length + bytes48.Length + bytes49.Length;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes46, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            NetMessage.buffer[whoAmi].writeBuffer[dstOffset1] = num22;
            int dstOffset27 = dstOffset1 + 1;
            Buffer.BlockCopy((Array) bytes47, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset27, 4);
            int dstOffset28 = dstOffset27 + 4;
            Buffer.BlockCopy((Array) bytes48, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset28, 2);
            int dstOffset29 = dstOffset28 + 2;
            Buffer.BlockCopy((Array) bytes49, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset29, 2);
            num1 = dstOffset29 + 2;
            break;
          case 19:
            byte[] bytes50 = BitConverter.GetBytes(msgType);
            byte num23 = (byte) number;
            byte[] bytes51 = BitConverter.GetBytes((int) number2);
            byte[] bytes52 = BitConverter.GetBytes((int) number3);
            byte num24 = 0;
            if ((double) number4 == 1.0)
              num24 = (byte) 1;
            count += 1 + bytes51.Length + bytes52.Length + 1;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes50, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            NetMessage.buffer[whoAmi].writeBuffer[dstOffset1] = num23;
            int dstOffset30 = dstOffset1 + 1;
            Buffer.BlockCopy((Array) bytes51, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset30, 4);
            int dstOffset31 = dstOffset30 + 4;
            Buffer.BlockCopy((Array) bytes52, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset31, 4);
            int index32 = dstOffset31 + 4;
            NetMessage.buffer[whoAmi].writeBuffer[index32] = num24;
            break;
          case 20:
            short num25 = (short) number;
            int num26 = (int) number2;
            int num27 = (int) number3;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(msgType), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            Buffer.BlockCopy((Array) BitConverter.GetBytes(num25), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset1, 2);
            int dstOffset32 = dstOffset1 + 2;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(num26), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset32, 4);
            int dstOffset33 = dstOffset32 + 4;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(num27), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset33, 4);
            int dstOffset34 = dstOffset33 + 4;
            for (int index33 = num26; index33 < num26 + (int) num25; ++index33)
            {
              for (int index34 = num27; index34 < num27 + (int) num25; ++index34)
              {
                byte num28 = 0;
                if (Game1.tile[index33, index34].active)
                  ++num28;
                if (Game1.tile[index33, index34].lighted)
                  num28 += (byte) 2;
                if (Game1.tile[index33, index34].wall > (byte) 0)
                  num28 += (byte) 4;
                if (Game1.tile[index33, index34].liquid > (byte) 0 && Game1.netMode == 2)
                  num28 += (byte) 8;
                NetMessage.buffer[whoAmi].writeBuffer[dstOffset34] = num28;
                ++dstOffset34;
                byte[] bytes53 = BitConverter.GetBytes(Game1.tile[index33, index34].frameX);
                byte[] bytes54 = BitConverter.GetBytes(Game1.tile[index33, index34].frameY);
                byte wall = Game1.tile[index33, index34].wall;
                if (Game1.tile[index33, index34].active)
                {
                  NetMessage.buffer[whoAmi].writeBuffer[dstOffset34] = Game1.tile[index33, index34].type;
                  ++dstOffset34;
                  if (Game1.tileFrameImportant[(int) Game1.tile[index33, index34].type])
                  {
                    Buffer.BlockCopy((Array) bytes53, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset34, 2);
                    int dstOffset35 = dstOffset34 + 2;
                    Buffer.BlockCopy((Array) bytes54, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset35, 2);
                    dstOffset34 = dstOffset35 + 2;
                  }
                }
                if (wall > (byte) 0)
                {
                  NetMessage.buffer[whoAmi].writeBuffer[dstOffset34] = wall;
                  ++dstOffset34;
                }
                if (Game1.tile[index33, index34].liquid > (byte) 0 && Game1.netMode == 2)
                {
                  NetMessage.buffer[whoAmi].writeBuffer[dstOffset34] = Game1.tile[index33, index34].liquid;
                  int index35 = dstOffset34 + 1;
                  byte num29 = 0;
                  if (Game1.tile[index33, index34].lava)
                    num29 = (byte) 1;
                  NetMessage.buffer[whoAmi].writeBuffer[index35] = num29;
                  dstOffset34 = index35 + 1;
                }
              }
            }
            Buffer.BlockCopy((Array) BitConverter.GetBytes(dstOffset34 - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            count = dstOffset34;
            break;
          case 21:
            byte[] bytes55 = BitConverter.GetBytes(msgType);
            byte[] bytes56 = BitConverter.GetBytes((short) number);
            byte[] bytes57 = BitConverter.GetBytes(Game1.item[number].position.X);
            byte[] bytes58 = BitConverter.GetBytes(Game1.item[number].position.Y);
            byte[] bytes59 = BitConverter.GetBytes(Game1.item[number].velocity.X);
            byte[] bytes60 = BitConverter.GetBytes(Game1.item[number].velocity.Y);
            byte stack1 = (byte) Game1.item[number].stack;
            string s = "0";
            if (Game1.item[number].active && Game1.item[number].stack > 0)
              s = Game1.item[number].name;
            if (s == null)
              s = "0";
            byte[] bytes61 = Encoding.ASCII.GetBytes(s);
            count += bytes56.Length + bytes57.Length + bytes58.Length + bytes59.Length + bytes60.Length + 1 + bytes61.Length;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes55, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            Buffer.BlockCopy((Array) bytes56, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset1, bytes56.Length);
            int dstOffset36 = dstOffset1 + 2;
            Buffer.BlockCopy((Array) bytes57, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset36, bytes57.Length);
            int dstOffset37 = dstOffset36 + 4;
            Buffer.BlockCopy((Array) bytes58, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset37, bytes58.Length);
            int dstOffset38 = dstOffset37 + 4;
            Buffer.BlockCopy((Array) bytes59, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset38, bytes59.Length);
            int dstOffset39 = dstOffset38 + 4;
            Buffer.BlockCopy((Array) bytes60, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset39, bytes60.Length);
            int index36 = dstOffset39 + 4;
            NetMessage.buffer[whoAmi].writeBuffer[index36] = stack1;
            int dstOffset40 = index36 + 1;
            Buffer.BlockCopy((Array) bytes61, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset40, bytes61.Length);
            break;
          case 22:
            byte[] bytes62 = BitConverter.GetBytes(msgType);
            byte[] bytes63 = BitConverter.GetBytes((short) number);
            byte owner = (byte) Game1.item[number].owner;
            count += bytes63.Length + 1;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes62, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            Buffer.BlockCopy((Array) bytes63, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset1, bytes63.Length);
            int index37 = dstOffset1 + 2;
            NetMessage.buffer[whoAmi].writeBuffer[index37] = owner;
            break;
          case 23:
            byte[] bytes64 = BitConverter.GetBytes(msgType);
            byte[] bytes65 = BitConverter.GetBytes((short) number);
            byte[] bytes66 = BitConverter.GetBytes(Game1.npc[number].position.X);
            byte[] bytes67 = BitConverter.GetBytes(Game1.npc[number].position.Y);
            byte[] bytes68 = BitConverter.GetBytes(Game1.npc[number].velocity.X);
            byte[] bytes69 = BitConverter.GetBytes(Game1.npc[number].velocity.Y);
            byte[] bytes70 = BitConverter.GetBytes((short) Game1.npc[number].target);
            byte[] bytes71 = BitConverter.GetBytes((short) Game1.npc[number].life);
            if (!Game1.npc[number].active)
              bytes71 = BitConverter.GetBytes((short) 0);
            byte[] bytes72 = Encoding.ASCII.GetBytes(Game1.npc[number].name);
            count += bytes65.Length + bytes66.Length + bytes67.Length + bytes68.Length + bytes69.Length + bytes70.Length + bytes71.Length + NPC.maxAI * 4 + bytes72.Length + 1 + 1;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes64, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            Buffer.BlockCopy((Array) bytes65, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset1, bytes65.Length);
            int dstOffset41 = dstOffset1 + 2;
            Buffer.BlockCopy((Array) bytes66, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset41, bytes66.Length);
            int dstOffset42 = dstOffset41 + 4;
            Buffer.BlockCopy((Array) bytes67, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset42, bytes67.Length);
            int dstOffset43 = dstOffset42 + 4;
            Buffer.BlockCopy((Array) bytes68, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset43, bytes68.Length);
            int dstOffset44 = dstOffset43 + 4;
            Buffer.BlockCopy((Array) bytes69, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset44, bytes69.Length);
            int dstOffset45 = dstOffset44 + 4;
            Buffer.BlockCopy((Array) bytes70, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset45, bytes70.Length);
            int index38 = dstOffset45 + 2;
            NetMessage.buffer[whoAmi].writeBuffer[index38] = (byte) (Game1.npc[number].direction + 1);
            int index39 = index38 + 1;
            NetMessage.buffer[whoAmi].writeBuffer[index39] = (byte) (Game1.npc[number].directionY + 1);
            int dstOffset46 = index39 + 1;
            Buffer.BlockCopy((Array) bytes71, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset46, bytes71.Length);
            int dstOffset47 = dstOffset46 + 2;
            for (int index40 = 0; index40 < NPC.maxAI; ++index40)
            {
              byte[] bytes73 = BitConverter.GetBytes(Game1.npc[number].ai[index40]);
              Buffer.BlockCopy((Array) bytes73, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset47, bytes73.Length);
              dstOffset47 += 4;
            }
            Buffer.BlockCopy((Array) bytes72, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset47, bytes72.Length);
            break;
          case 24:
            byte[] bytes74 = BitConverter.GetBytes(msgType);
            byte[] bytes75 = BitConverter.GetBytes((short) number);
            byte num30 = (byte) number2;
            count += bytes75.Length + 1;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes74, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            Buffer.BlockCopy((Array) bytes75, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset1, bytes75.Length);
            int index41 = dstOffset1 + 2;
            NetMessage.buffer[whoAmi].writeBuffer[index41] = num30;
            break;
          case 25:
            byte[] bytes76 = BitConverter.GetBytes(msgType);
            byte num31 = (byte) number;
            byte[] bytes77 = Encoding.ASCII.GetBytes(text);
            byte num32 = (byte) number2;
            byte num33 = (byte) number3;
            byte num34 = (byte) number4;
            count += 1 + bytes77.Length + 3;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes76, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            NetMessage.buffer[whoAmi].writeBuffer[dstOffset1] = num31;
            int index42 = dstOffset1 + 1;
            NetMessage.buffer[whoAmi].writeBuffer[index42] = num32;
            int index43 = index42 + 1;
            NetMessage.buffer[whoAmi].writeBuffer[index43] = num33;
            int index44 = index43 + 1;
            NetMessage.buffer[whoAmi].writeBuffer[index44] = num34;
            int dstOffset48 = index44 + 1;
            Buffer.BlockCopy((Array) bytes77, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset48, bytes77.Length);
            break;
          case 26:
            byte[] bytes78 = BitConverter.GetBytes(msgType);
            byte num35 = (byte) number;
            byte num36 = (byte) ((double) number2 + 1.0);
            byte[] bytes79 = BitConverter.GetBytes((short) number3);
            byte num37 = (byte) number4;
            count += 2 + bytes79.Length + 1;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes78, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            NetMessage.buffer[whoAmi].writeBuffer[dstOffset1] = num35;
            int index45 = dstOffset1 + 1;
            NetMessage.buffer[whoAmi].writeBuffer[index45] = num36;
            int dstOffset49 = index45 + 1;
            Buffer.BlockCopy((Array) bytes79, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset49, bytes79.Length);
            int index46 = dstOffset49 + 2;
            NetMessage.buffer[whoAmi].writeBuffer[index46] = num37;
            break;
          case 27:
            byte[] bytes80 = BitConverter.GetBytes(msgType);
            byte[] bytes81 = BitConverter.GetBytes((short) Game1.projectile[number].identity);
            byte[] bytes82 = BitConverter.GetBytes(Game1.projectile[number].position.X);
            byte[] bytes83 = BitConverter.GetBytes(Game1.projectile[number].position.Y);
            byte[] bytes84 = BitConverter.GetBytes(Game1.projectile[number].velocity.X);
            byte[] bytes85 = BitConverter.GetBytes(Game1.projectile[number].velocity.Y);
            byte[] bytes86 = BitConverter.GetBytes(Game1.projectile[number].knockBack);
            byte[] bytes87 = BitConverter.GetBytes((short) Game1.projectile[number].damage);
            Buffer.BlockCopy((Array) bytes80, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            Buffer.BlockCopy((Array) bytes81, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset1, bytes81.Length);
            int dstOffset50 = dstOffset1 + 2;
            Buffer.BlockCopy((Array) bytes82, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset50, bytes82.Length);
            int dstOffset51 = dstOffset50 + 4;
            Buffer.BlockCopy((Array) bytes83, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset51, bytes83.Length);
            int dstOffset52 = dstOffset51 + 4;
            Buffer.BlockCopy((Array) bytes84, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset52, bytes84.Length);
            int dstOffset53 = dstOffset52 + 4;
            Buffer.BlockCopy((Array) bytes85, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset53, bytes85.Length);
            int dstOffset54 = dstOffset53 + 4;
            Buffer.BlockCopy((Array) bytes86, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset54, bytes86.Length);
            int dstOffset55 = dstOffset54 + 4;
            Buffer.BlockCopy((Array) bytes87, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset55, bytes87.Length);
            int index47 = dstOffset55 + 2;
            NetMessage.buffer[whoAmi].writeBuffer[index47] = (byte) Game1.projectile[number].owner;
            int index48 = index47 + 1;
            NetMessage.buffer[whoAmi].writeBuffer[index48] = (byte) Game1.projectile[number].type;
            int dstOffset56 = index48 + 1;
            for (int index49 = 0; index49 < Projectile.maxAI; ++index49)
            {
              byte[] bytes88 = BitConverter.GetBytes(Game1.projectile[number].ai[index49]);
              Buffer.BlockCopy((Array) bytes88, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset56, bytes88.Length);
              dstOffset56 += 4;
            }
            count += dstOffset56;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            break;
          case 28:
            byte[] bytes89 = BitConverter.GetBytes(msgType);
            byte[] bytes90 = BitConverter.GetBytes((short) number);
            byte[] bytes91 = BitConverter.GetBytes((short) number2);
            byte[] bytes92 = BitConverter.GetBytes(number3);
            byte num38 = (byte) ((double) number4 + 1.0);
            count += bytes90.Length + bytes91.Length + bytes92.Length + 1;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes89, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            Buffer.BlockCopy((Array) bytes90, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset1, bytes90.Length);
            int dstOffset57 = dstOffset1 + 2;
            Buffer.BlockCopy((Array) bytes91, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset57, bytes91.Length);
            int dstOffset58 = dstOffset57 + 2;
            Buffer.BlockCopy((Array) bytes92, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset58, bytes92.Length);
            int index50 = dstOffset58 + 4;
            NetMessage.buffer[whoAmi].writeBuffer[index50] = num38;
            break;
          case 29:
            byte[] bytes93 = BitConverter.GetBytes(msgType);
            byte[] bytes94 = BitConverter.GetBytes((short) number);
            byte num39 = (byte) number2;
            count += bytes94.Length + 1;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes93, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            Buffer.BlockCopy((Array) bytes94, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset1, bytes94.Length);
            int index51 = dstOffset1 + 2;
            NetMessage.buffer[whoAmi].writeBuffer[index51] = num39;
            break;
          case 30:
            byte[] bytes95 = BitConverter.GetBytes(msgType);
            byte index52 = (byte) number;
            byte num40 = 0;
            if (Game1.player[(int) index52].hostile)
              num40 = (byte) 1;
            count += 2;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes95, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            NetMessage.buffer[whoAmi].writeBuffer[dstOffset1] = index52;
            int index53 = dstOffset1 + 1;
            NetMessage.buffer[whoAmi].writeBuffer[index53] = num40;
            break;
          case 31:
            byte[] bytes96 = BitConverter.GetBytes(msgType);
            byte[] bytes97 = BitConverter.GetBytes(number);
            byte[] bytes98 = BitConverter.GetBytes((int) number2);
            count += bytes97.Length + bytes98.Length;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes96, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            Buffer.BlockCopy((Array) bytes97, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset1, bytes97.Length);
            int dstOffset59 = dstOffset1 + 4;
            Buffer.BlockCopy((Array) bytes98, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset59, bytes98.Length);
            break;
          case 32:
            byte[] bytes99 = BitConverter.GetBytes(msgType);
            byte[] bytes100 = BitConverter.GetBytes((short) number);
            byte num41 = (byte) number2;
            byte stack2 = (byte) Game1.chest[number].item[(int) number2].stack;
            byte[] src = Game1.chest[number].item[(int) number2].name != null ? Encoding.ASCII.GetBytes(Game1.chest[number].item[(int) number2].name) : Encoding.ASCII.GetBytes("");
            count += bytes100.Length + 1 + 1 + src.Length;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes99, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            Buffer.BlockCopy((Array) bytes100, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset1, bytes100.Length);
            int index54 = dstOffset1 + 2;
            NetMessage.buffer[whoAmi].writeBuffer[index54] = num41;
            int index55 = index54 + 1;
            NetMessage.buffer[whoAmi].writeBuffer[index55] = stack2;
            int dstOffset60 = index55 + 1;
            Buffer.BlockCopy((Array) src, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset60, src.Length);
            break;
          case 33:
            byte[] bytes101 = BitConverter.GetBytes(msgType);
            byte[] bytes102 = BitConverter.GetBytes((short) number);
            byte[] bytes103;
            byte[] bytes104;
            if (number > -1)
            {
              bytes103 = BitConverter.GetBytes(Game1.chest[number].x);
              bytes104 = BitConverter.GetBytes(Game1.chest[number].y);
            }
            else
            {
              bytes103 = BitConverter.GetBytes(0);
              bytes104 = BitConverter.GetBytes(0);
            }
            count += bytes102.Length + bytes103.Length + bytes104.Length;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes101, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            Buffer.BlockCopy((Array) bytes102, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset1, bytes102.Length);
            int dstOffset61 = dstOffset1 + 2;
            Buffer.BlockCopy((Array) bytes103, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset61, bytes103.Length);
            int dstOffset62 = dstOffset61 + 4;
            Buffer.BlockCopy((Array) bytes104, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset62, bytes104.Length);
            break;
          case 34:
            byte[] bytes105 = BitConverter.GetBytes(msgType);
            byte[] bytes106 = BitConverter.GetBytes(number);
            byte[] bytes107 = BitConverter.GetBytes((int) number2);
            count += bytes106.Length + bytes107.Length;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes105, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            Buffer.BlockCopy((Array) bytes106, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset1, bytes106.Length);
            int dstOffset63 = dstOffset1 + 4;
            Buffer.BlockCopy((Array) bytes107, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset63, bytes107.Length);
            break;
          case 35:
            byte[] bytes108 = BitConverter.GetBytes(msgType);
            byte num42 = (byte) number;
            byte[] bytes109 = BitConverter.GetBytes((short) number2);
            count += 1 + bytes109.Length;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes108, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            NetMessage.buffer[whoAmi].writeBuffer[5] = num42;
            int dstOffset64 = dstOffset1 + 1;
            Buffer.BlockCopy((Array) bytes109, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset64, 2);
            break;
          case 36:
            byte[] bytes110 = BitConverter.GetBytes(msgType);
            byte index56 = (byte) number;
            byte num43 = 0;
            if (Game1.player[(int) index56].zoneEvil)
              num43 = (byte) 1;
            byte num44 = 0;
            if (Game1.player[(int) index56].zoneMeteor)
              num44 = (byte) 1;
            byte num45 = 0;
            if (Game1.player[(int) index56].zoneDungeon)
              num45 = (byte) 1;
            byte num46 = 0;
            if (Game1.player[(int) index56].zoneJungle)
              num46 = (byte) 1;
            count += 4;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes110, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            NetMessage.buffer[whoAmi].writeBuffer[dstOffset1] = index56;
            int index57 = dstOffset1 + 1;
            NetMessage.buffer[whoAmi].writeBuffer[index57] = num43;
            int index58 = index57 + 1;
            NetMessage.buffer[whoAmi].writeBuffer[index58] = num44;
            int index59 = index58 + 1;
            NetMessage.buffer[whoAmi].writeBuffer[index59] = num45;
            int index60 = index59 + 1;
            NetMessage.buffer[whoAmi].writeBuffer[index60] = num46;
            num1 = index60 + 1;
            break;
          case 37:
            byte[] bytes111 = BitConverter.GetBytes(msgType);
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes111, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            break;
          case 38:
            byte[] bytes112 = BitConverter.GetBytes(msgType);
            byte[] bytes113 = Encoding.ASCII.GetBytes(text);
            count += bytes113.Length;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes112, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            Buffer.BlockCopy((Array) bytes113, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset1, bytes113.Length);
            break;
          case 39:
            byte[] bytes114 = BitConverter.GetBytes(msgType);
            byte[] bytes115 = BitConverter.GetBytes((short) number);
            count += bytes115.Length;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes114, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            Buffer.BlockCopy((Array) bytes115, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset1, bytes115.Length);
            break;
          case 40:
            byte[] bytes116 = BitConverter.GetBytes(msgType);
            byte index61 = (byte) number;
            byte[] bytes117 = BitConverter.GetBytes((short) Game1.player[(int) index61].talkNPC);
            count += 1 + bytes117.Length;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes116, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            NetMessage.buffer[whoAmi].writeBuffer[dstOffset1] = index61;
            int dstOffset65 = dstOffset1 + 1;
            Buffer.BlockCopy((Array) bytes117, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset65, bytes117.Length);
            num1 = dstOffset65 + 2;
            break;
          case 41:
            byte[] bytes118 = BitConverter.GetBytes(msgType);
            byte index62 = (byte) number;
            byte[] bytes119 = BitConverter.GetBytes(Game1.player[(int) index62].itemRotation);
            byte[] bytes120 = BitConverter.GetBytes((short) Game1.player[(int) index62].itemAnimation);
            count += 1 + bytes119.Length + bytes120.Length;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes118, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            NetMessage.buffer[whoAmi].writeBuffer[dstOffset1] = index62;
            int dstOffset66 = dstOffset1 + 1;
            Buffer.BlockCopy((Array) bytes119, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset66, bytes119.Length);
            int dstOffset67 = dstOffset66 + 4;
            Buffer.BlockCopy((Array) bytes120, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset67, bytes120.Length);
            break;
          case 42:
            byte[] bytes121 = BitConverter.GetBytes(msgType);
            byte index63 = (byte) number;
            byte[] bytes122 = BitConverter.GetBytes((short) Game1.player[(int) index63].statMana);
            byte[] bytes123 = BitConverter.GetBytes((short) Game1.player[(int) index63].statManaMax);
            count += 1 + bytes122.Length + bytes123.Length;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes121, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            NetMessage.buffer[whoAmi].writeBuffer[5] = index63;
            int dstOffset68 = dstOffset1 + 1;
            Buffer.BlockCopy((Array) bytes122, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset68, 2);
            int dstOffset69 = dstOffset68 + 2;
            Buffer.BlockCopy((Array) bytes123, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset69, 2);
            break;
          case 43:
            byte[] bytes124 = BitConverter.GetBytes(msgType);
            byte num47 = (byte) number;
            byte[] bytes125 = BitConverter.GetBytes((short) number2);
            count += 1 + bytes125.Length;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes124, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            NetMessage.buffer[whoAmi].writeBuffer[5] = num47;
            int dstOffset70 = dstOffset1 + 1;
            Buffer.BlockCopy((Array) bytes125, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset70, 2);
            break;
          case 44:
            byte[] bytes126 = BitConverter.GetBytes(msgType);
            byte num48 = (byte) number;
            byte num49 = (byte) ((double) number2 + 1.0);
            byte[] bytes127 = BitConverter.GetBytes((short) number3);
            byte num50 = (byte) number4;
            count += 2 + bytes127.Length + 1;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes126, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            NetMessage.buffer[whoAmi].writeBuffer[dstOffset1] = num48;
            int index64 = dstOffset1 + 1;
            NetMessage.buffer[whoAmi].writeBuffer[index64] = num49;
            int dstOffset71 = index64 + 1;
            Buffer.BlockCopy((Array) bytes127, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset71, bytes127.Length);
            int index65 = dstOffset71 + 2;
            NetMessage.buffer[whoAmi].writeBuffer[index65] = num50;
            break;
          case 45:
            byte[] bytes128 = BitConverter.GetBytes(msgType);
            byte index66 = (byte) number;
            byte team = (byte) Game1.player[(int) index66].team;
            count += 2;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes128, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            NetMessage.buffer[whoAmi].writeBuffer[5] = index66;
            int index67 = dstOffset1 + 1;
            NetMessage.buffer[whoAmi].writeBuffer[index67] = team;
            break;
          case 46:
            byte[] bytes129 = BitConverter.GetBytes(msgType);
            byte[] bytes130 = BitConverter.GetBytes(number);
            byte[] bytes131 = BitConverter.GetBytes((int) number2);
            count += bytes130.Length + bytes131.Length;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes129, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            Buffer.BlockCopy((Array) bytes130, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset1, bytes130.Length);
            int dstOffset72 = dstOffset1 + 4;
            Buffer.BlockCopy((Array) bytes131, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset72, bytes131.Length);
            break;
          case 47:
            byte[] bytes132 = BitConverter.GetBytes(msgType);
            byte[] bytes133 = BitConverter.GetBytes((short) number);
            byte[] bytes134 = BitConverter.GetBytes(Game1.sign[number].x);
            byte[] bytes135 = BitConverter.GetBytes(Game1.sign[number].y);
            byte[] bytes136 = Encoding.ASCII.GetBytes(Game1.sign[number].text);
            count += bytes133.Length + bytes134.Length + bytes135.Length + bytes136.Length;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes132, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            Buffer.BlockCopy((Array) bytes133, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset1, bytes133.Length);
            int dstOffset73 = dstOffset1 + bytes133.Length;
            Buffer.BlockCopy((Array) bytes134, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset73, bytes134.Length);
            int dstOffset74 = dstOffset73 + bytes134.Length;
            Buffer.BlockCopy((Array) bytes135, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset74, bytes135.Length);
            int dstOffset75 = dstOffset74 + bytes135.Length;
            Buffer.BlockCopy((Array) bytes136, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset75, bytes136.Length);
            num1 = dstOffset75 + bytes136.Length;
            break;
          case 48:
            byte[] bytes137 = BitConverter.GetBytes(msgType);
            byte[] bytes138 = BitConverter.GetBytes(number);
            byte[] bytes139 = BitConverter.GetBytes((int) number2);
            byte liquid = Game1.tile[number, (int) number2].liquid;
            byte num51 = 0;
            if (Game1.tile[number, (int) number2].lava)
              num51 = (byte) 1;
            count += bytes138.Length + bytes139.Length + 1 + 1;
            Buffer.BlockCopy((Array) BitConverter.GetBytes(count - 4), 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 0, 4);
            Buffer.BlockCopy((Array) bytes137, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, 4, 1);
            Buffer.BlockCopy((Array) bytes138, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset1, 4);
            int dstOffset76 = dstOffset1 + 4;
            Buffer.BlockCopy((Array) bytes139, 0, (Array) NetMessage.buffer[whoAmi].writeBuffer, dstOffset76, 4);
            int index68 = dstOffset76 + 4;
            NetMessage.buffer[whoAmi].writeBuffer[index68] = liquid;
            int index69 = index68 + 1;
            NetMessage.buffer[whoAmi].writeBuffer[index69] = num51;
            num1 = index69 + 1;
            break;
        }
        if (Game1.netMode == 1)
        {
          if (Netplay.clientSock.tcpClient.Connected)
          {
            try
            {
              ++NetMessage.buffer[whoAmi].spamCount;
              
              //RnD
              //Netplay.clientSock.networkStream.BeginWrite(NetMessage.buffer[whoAmi].writeBuffer, 0, count, new AsyncCallback(Netplay.clientSock.ClientWriteCallBack), (object) Netplay.clientSock.networkStream);
            }
            catch
            {
              Debug.WriteLine("    Exception normal: Tried to send data to the server after losing connection");
            }
          }
        }
        else if (remoteClient == -1)
        {
          for (int index70 = 0; index70 < 9; ++index70)
          {
            if (index70 != ignoreClient && (NetMessage.buffer[index70].broadcast || Netplay.serverSock[index70].state >= 3 && msgType == 10))
            {
              if (Netplay.serverSock[index70].tcpClient.Connected)
              {
                try
                {
                  ++NetMessage.buffer[index70].spamCount;

                  //RnD
                  //Netplay.serverSock[index70].networkStream.BeginWrite(NetMessage.buffer[whoAmi].writeBuffer, 0, count, new AsyncCallback(Netplay.serverSock[index70].ServerWriteCallBack), (object) Netplay.serverSock[index70].networkStream);
                }
                catch
                {
                  Debug.WriteLine("    Exception normal: Tried to send data to a client after losing connection");
                }
              }
            }
          }
        }
        else if (Netplay.serverSock[remoteClient].tcpClient.Connected)
        {
          try
          {
            ++NetMessage.buffer[remoteClient].spamCount;
            //RnD
            //Netplay.serverSock[remoteClient].networkStream.BeginWrite(NetMessage.buffer[whoAmi].writeBuffer, 0, count, new AsyncCallback(Netplay.serverSock[remoteClient].ServerWriteCallBack), (object) Netplay.serverSock[remoteClient].networkStream);
          }
          catch
          {
            Debug.WriteLine("    Exception normal: Tried to send data to a client after losing connection");
          }
        }
        if (Game1.verboseNetplay)
        {
          Debug.WriteLine("Sent:");
          for (int index71 = 0; index71 < count; ++index71)
            Debug.Write(NetMessage.buffer[whoAmi].writeBuffer[index71].ToString() + " ");
          Debug.WriteLine("");
          for (int index72 = 0; index72 < count; ++index72)
            Debug.Write((object) (char) NetMessage.buffer[whoAmi].writeBuffer[index72]);
          Debug.WriteLine("");
          Debug.WriteLine("");
        }
        NetMessage.buffer[whoAmi].writeLocked = false;
        if (msgType == 19 && Game1.netMode == 1)
        {
          int size = 5;
          NetMessage.SendTileSquare(whoAmi, (int) number2, (int) number3, size);
        }
        if (msgType != 2 || Game1.netMode != 2)
          return;
        Netplay.serverSock[whoAmi].kill = true;
      }
    }

    public static void RecieveBytes(byte[] bytes, int streamLength, int i = 9)
    {
      lock (NetMessage.buffer[i])
      {
        try
        {
          Buffer.BlockCopy((Array) bytes, 0, (Array) NetMessage.buffer[i].readBuffer, NetMessage.buffer[i].totalData, streamLength);
          NetMessage.buffer[i].totalData += streamLength;
          NetMessage.buffer[i].checkBytes = true;
        }
        catch
        {
          if (Game1.netMode == 1)
          {
            Debug.WriteLine("    Exception cause: Bad header lead to a read buffer overflow.");
            Game1.menuMode = 15;
            Game1.statusText = "Bad header lead to a read buffer overflow.";
            Netplay.disconnect = true;
          }
          else
          {
            Debug.WriteLine("    Exception cause: Bad header lead to a read buffer overflow.");
            Netplay.serverSock[i].kill = true;
          }
        }
      }
    }

    public static void CheckBytes(int i = 9)
    {
      lock (NetMessage.buffer[i])
      {
        int num = 0;
        if (NetMessage.buffer[i].totalData < 4)
          return;
        if (NetMessage.buffer[i].messageLength == 0)
          NetMessage.buffer[i].messageLength = BitConverter.ToInt32(NetMessage.buffer[i].readBuffer, 0) + 4;
        for (; NetMessage.buffer[i].totalData >= NetMessage.buffer[i].messageLength + num && NetMessage.buffer[i].messageLength > 0; NetMessage.buffer[i].messageLength = NetMessage.buffer[i].totalData - num < 4 ? 0 : BitConverter.ToInt32(NetMessage.buffer[i].readBuffer, num) + 4)
        {
          NetMessage.buffer[i].GetData(num + 4, NetMessage.buffer[i].messageLength - 4);
          num += NetMessage.buffer[i].messageLength;
        }
        if (num == NetMessage.buffer[i].totalData)
          NetMessage.buffer[i].totalData = 0;
        else if (num > 0)
        {
          Buffer.BlockCopy((Array) NetMessage.buffer[i].readBuffer, num, (Array) NetMessage.buffer[i].readBuffer, 0, NetMessage.buffer[i].totalData - num);
          NetMessage.buffer[i].totalData -= num;
        }
        NetMessage.buffer[i].checkBytes = false;
      }
    }

    public static void SendTileSquare(int whoAmi, int tileX, int tileY, int size)
    {
      int num = (size - 1) / 2;
      NetMessage.SendData(20, whoAmi, number: size, number2: (float) (tileX - num), number3: (float) (tileY - num));
    }

    public static void SendSection(int whoAmi, int sectionX, int sectionY)
    {
      Netplay.serverSock[whoAmi].tileSection[sectionX, sectionY] = true;
      int number2 = sectionX * 200;
      int num = sectionY * 150;
      for (int number3 = num; number3 < num + 150; ++number3)
        NetMessage.SendData(10, whoAmi, number: 200, number2: (float) number2, number3: (float) number3);
    }

    public static void greetPlayer(int plr)
    {
      NetMessage.SendData(25, plr, text: "Welcome to " + Game1.worldName + "!", number: 8, number2: (float) byte.MaxValue, number3: 240f, number4: 20f);
      string str = "";
      for (int index = 0; index < 8; ++index)
      {
        if (Game1.player[index].active)
          str = !(str == "") ? str + ", " + Game1.player[index].name : str + Game1.player[index].name;
      }
      NetMessage.SendData(25, plr, text: "Current players: " + str + ".", number: 8, number2: (float) byte.MaxValue, number3: 240f, number4: 20f);
    }

    public static void sendWater(int x, int y)
    {
      for (int remoteClient = 0; remoteClient < 9; ++remoteClient)
      {
        if ((NetMessage.buffer[remoteClient].broadcast || Netplay.serverSock[remoteClient].state >= 3) && Netplay.serverSock[remoteClient].tcpClient.Connected)
        {
          int index1 = x / 200;
          int index2 = y / 150;
          if (Netplay.serverSock[remoteClient].tileSection[index1, index2])
            NetMessage.SendData(48, remoteClient, number: x, number2: (float) y);
        }
      }
    }

    public static void syncPlayers()
    {
      for (int index = 0; index < 8; ++index)
      {
        int number2_1 = 0;
        if (Game1.player[index].active)
          number2_1 = 1;
        if (Netplay.serverSock[index].state == 10)
        {
          NetMessage.SendData(14, ignoreClient: index, number: index, number2: (float) number2_1);
          NetMessage.SendData(13, ignoreClient: index, number: index);
          NetMessage.SendData(16, ignoreClient: index, number: index);
          NetMessage.SendData(30, ignoreClient: index, number: index);
          NetMessage.SendData(45, ignoreClient: index, number: index);
          NetMessage.SendData(42, ignoreClient: index, number: index);
          NetMessage.SendData(4, ignoreClient: index, text: Game1.player[index].name, number: index);
          for (int number2_2 = 0; number2_2 < 44; ++number2_2)
            NetMessage.SendData(5, ignoreClient: index, text: Game1.player[index].inventory[number2_2].name, number: index, number2: (float) number2_2);
          NetMessage.SendData(5, ignoreClient: index, text: Game1.player[index].armor[0].name, number: index, number2: 44f);
          NetMessage.SendData(5, ignoreClient: index, text: Game1.player[index].armor[1].name, number: index, number2: 45f);
          NetMessage.SendData(5, ignoreClient: index, text: Game1.player[index].armor[2].name, number: index, number2: 46f);
          NetMessage.SendData(5, ignoreClient: index, text: Game1.player[index].armor[3].name, number: index, number2: 47f);
          NetMessage.SendData(5, ignoreClient: index, text: Game1.player[index].armor[4].name, number: index, number2: 48f);
          NetMessage.SendData(5, ignoreClient: index, text: Game1.player[index].armor[5].name, number: index, number2: 49f);
          NetMessage.SendData(5, ignoreClient: index, text: Game1.player[index].armor[6].name, number: index, number2: 50f);
          NetMessage.SendData(5, ignoreClient: index, text: Game1.player[index].armor[7].name, number: index, number2: 51f);
          if (!Netplay.serverSock[index].announced)
          {
            Netplay.serverSock[index].announced = true;
            NetMessage.SendData(25, ignoreClient: index, text: Game1.player[index].name + " has joined.", number: 8, number2: (float) byte.MaxValue, number3: 240f, number4: 20f);
          }
        }
        else
        {
          NetMessage.SendData(14, ignoreClient: index, number: index, number2: (float) number2_1);
          if (Netplay.serverSock[index].announced)
          {
            Netplay.serverSock[index].announced = false;
            NetMessage.SendData(25, ignoreClient: index, text: Netplay.serverSock[index].oldName + " has left.", number: 8, number2: (float) byte.MaxValue, number3: 240f, number4: 20f);
          }
        }
      }
    }
  }
}
