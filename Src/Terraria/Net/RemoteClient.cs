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
using GameManager.Net.Sockets;

namespace GameManager
{
    public class RemoteClient
    {
        public ISocket Socket = (ISocket)new TcpSocket();
        public string Name = "Anonymous";
        public string StatusText = "";
        public bool[,] TileSections = new bool[Game1.maxTilesX / 200 + 1, Game1.maxTilesY / 150 + 1];
        public float SpamProjectileMax = 100f;
        public float SpamAddBlockMax = 100f;
        public float SpamDeleteBlockMax = 500f;
        public float SpamWaterMax = 50f;
        public int Id;
        public bool IsActive;
        public bool PendingTermination;
        public bool IsAnnouncementCompleted;
        public bool IsReading;
        public int State;
        public int TimeOutTimer;
        public string StatusText2;
        public int StatusCount;
        public int StatusMax;
        public byte[] ReadBuffer;
        public float SpamProjectile;
        public float SpamAddBlock;
        public float SpamDeleteBlock;
        public float SpamWater;

        public void SpamUpdate()
        {
            if (!Netplay.spamCheck)
            {
                this.SpamProjectile = 0.0f;
                this.SpamDeleteBlock = 0.0f;
                this.SpamAddBlock = 0.0f;
                this.SpamWater = 0.0f;
            }
            else
            {
                if ((double)this.SpamProjectile > (double)this.SpamProjectileMax)
                    NetMessage.BootPlayer(this.Id, "Cheating attempt detected: Projectile spam");
                if ((double)this.SpamAddBlock > (double)this.SpamAddBlockMax)
                    NetMessage.BootPlayer(this.Id, "Cheating attempt detected: Add tile spam");
                if ((double)this.SpamDeleteBlock > (double)this.SpamDeleteBlockMax)
                    NetMessage.BootPlayer(this.Id, "Cheating attempt detected: Remove tile spam");
                if ((double)this.SpamWater > (double)this.SpamWaterMax)
                    NetMessage.BootPlayer(this.Id, "Cheating attempt detected: Liquid spam");
                this.SpamProjectile -= 0.4f;
                if ((double)this.SpamProjectile < 0.0)
                    this.SpamProjectile = 0.0f;
                this.SpamAddBlock -= 0.3f;
                if ((double)this.SpamAddBlock < 0.0)
                    this.SpamAddBlock = 0.0f;
                this.SpamDeleteBlock -= 5f;
                if ((double)this.SpamDeleteBlock < 0.0)
                    this.SpamDeleteBlock = 0.0f;
                this.SpamWater -= 0.2f;
                if ((double)this.SpamWater >= 0.0)
                    return;
                this.SpamWater = 0.0f;
            }
        }

        public void SpamClear()
        {
            this.SpamProjectile = 0.0f;
            this.SpamAddBlock = 0.0f;
            this.SpamDeleteBlock = 0.0f;
            this.SpamWater = 0.0f;
        }

        public static void CheckSection(int playerIndex, Vector2 position, int fluff = 1)
        {
            int index1 = playerIndex;
            int sectionX = Netplay.GetSectionX((int)((double)position.X / 16.0));
            int sectionY1 = Netplay.GetSectionY((int)((double)position.Y / 16.0));
            int num = 0;
            for (int index2 = sectionX - fluff; index2 < sectionX + fluff + 1; ++index2)
            {
                for (int index3 = sectionY1 - fluff; index3 < sectionY1 + fluff + 1; ++index3)
                {
                    if (index2 >= 0 && index2 < Game1.maxSectionsX && (index3 >= 0 && index3 < Game1.maxSectionsY) && !Netplay.Clients[index1].TileSections[index2, index3])
                        ++num;
                }
            }
            if (num <= 0)
                return;
            int number = num;
            NetMessage.SendData(9, index1, -1, Lang.inter[44], number, 0.0f, 0.0f, 0.0f, 0, 0, 0);
            Netplay.Clients[index1].StatusText2 = "is receiving tile data";
            Netplay.Clients[index1].StatusMax += number;
            for (int index2 = sectionX - fluff; index2 < sectionX + fluff + 1; ++index2)
            {
                for (int sectionY2 = sectionY1 - fluff; sectionY2 < sectionY1 + fluff + 1; ++sectionY2)
                {
                    if (index2 >= 0 && index2 < Game1.maxSectionsX && (sectionY2 >= 0 && sectionY2 < Game1.maxSectionsY) && !Netplay.Clients[index1].TileSections[index2, sectionY2])
                    {
                        NetMessage.SendSection(index1, index2, sectionY2, false);
                        NetMessage.SendData(11, index1, -1, "", index2, (float)sectionY2, (float)index2, (float)sectionY2, 0, 0, 0);
                    }
                }
            }
        }

        public bool SectionRange(int size, int firstX, int firstY)
        {
            for (int index = 0; index < 4; ++index)
            {
                int x = firstX;
                int y = firstY;
                if (index == 1)
                    x += size;
                if (index == 2)
                    y += size;
                if (index == 3)
                {
                    x += size;
                    y += size;
                }
                if (this.TileSections[Netplay.GetSectionX(x), Netplay.GetSectionY(y)])
                    return true;
            }
            return false;
        }

        public void ResetSections()
        {
            for (int index1 = 0; index1 < Game1.maxSectionsX; ++index1)
            {
                for (int index2 = 0; index2 < Game1.maxSectionsY; ++index2)
                    this.TileSections[index1, index2] = false;
            }
        }

        public void Reset()
        {
            this.ResetSections();
            if (this.Id < (int)byte.MaxValue)
                Game1.player[this.Id] = new Player();
            this.TimeOutTimer = 0;
            this.StatusCount = 0;
            this.StatusMax = 0;
            this.StatusText2 = "";
            this.StatusText = "";
            this.State = 0;
            this.IsReading = false;
            this.PendingTermination = false;
            this.SpamClear();
            this.IsActive = false;
            NetMessage.buffer[this.Id].Reset();
            this.Socket.Close();
        }

        public void ServerWriteCallBack(object state)
        {
            --NetMessage.buffer[this.Id].spamCount;
            if (this.StatusMax <= 0)
                return;
            ++this.StatusCount;
        }

        public void ServerReadCallBack(object state, int length)
        {
            if (!Netplay.disconnect)
            {
                int streamLength = length;
                if (streamLength == 0)
                    this.PendingTermination = true;
                else if (Game1.ignoreErrors)
                {
                    try
                    {
                        NetMessage.RecieveBytes(this.ReadBuffer, streamLength, this.Id);
                    }
                    catch
                    {
                    }
                }
                else
                    NetMessage.RecieveBytes(this.ReadBuffer, streamLength, this.Id);
            }
            this.IsReading = false;
        }
    }
}
