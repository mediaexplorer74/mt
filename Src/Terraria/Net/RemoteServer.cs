/*
  _____                 ____                 
 | ____|_ __ ___  _   _|  _ \  _____   _____ 
 |  _| | '_ ` _ \| | | | | | |/ _ \ \ / / __|
 | |___| | | | | | |_| | |_| |  __/\ V /\__ \
 |_____|_| |_| |_|\__,_|____/ \___| \_/ |___/
          <http://emudevs.com>
             Terraria 1.3
*/

using System;
using System.Diagnostics;
using System.IO;
using GameManager.Net.Sockets;

namespace GameManager
{
    public class RemoteServer
    {
        public ISocket Socket = (ISocket)new TcpSocket();
        public bool IsActive;
        public int State;
        public int TimeOutTimer;
        public bool IsReading;
        public byte[] ReadBuffer;
        public string StatusText;
        public int StatusCount;
        public int StatusMax;

        public void ClientWriteCallBack(object state)
        {
            --NetMessage.buffer[256].spamCount;
        }

        public void ClientReadCallBack(object state, int length)
        {
            try
            {
                if (!Netplay.disconnect)
                {
                    int streamLength = length;
                    if (streamLength == 0)
                    {
                        Netplay.disconnect = true;
                        Game1.statusText = "Lost connection";
                    }
                    else if (Game1.ignoreErrors)
                    {
                        try
                        {
                            NetMessage.RecieveBytes(this.ReadBuffer, streamLength, 256);
                        }
                        catch
                        {
                        }
                    }
                    else
                        NetMessage.RecieveBytes(this.ReadBuffer, streamLength, 256);
                }
                this.IsReading = false;
            }
            catch (Exception ex)
            {
                try
                {
                    Debug.WriteLine("[ex] " + (object)DateTime.Now);
                    Debug.WriteLine("[ex] " + ex.Message);
                    //using (StreamWriter streamWriter = new StreamWriter("client-crashlog.txt", true))
                    //{
                    //    streamWriter.WriteLine((object)DateTime.Now);
                    //    streamWriter.WriteLine((object)ex);
                    //    streamWriter.WriteLine("");
                    //}
                }
                catch
                {
                }
                Netplay.disconnect = true;
            }
        }
    }
}
