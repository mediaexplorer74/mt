// Decompiled with JetBrains decompiler
// Type: GameManager.ServerSock
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 09A1FDD7-68B3-40FF-A030-32890DF6B0E7
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager v1 Beta\GameManager.exe

using System;
using System.Net.Sockets;

namespace GameManager
{
  public class ServerSock
  {
    public Socket clientSocket;
    public NetworkStream networkStream;
    public TcpClient tcpClient = new TcpClient();
    public int whoAmI = 0;
    public string statusText2;
    public int statusCount;
    public int statusMax;
    public bool[,] tileSection = new bool[Game1.maxTilesX / 200, Game1.maxTilesY / 150];
    public string statusText = "";
    public bool active = false;
    public bool locked = false;
    public bool kill = false;
    public int timeOut = 0;
    public bool announced = false;
    public string name = "Anonymous";
    public string oldName = "";
    public int state = 0;
    public byte[] readBuffer;
    public byte[] writeBuffer;

    public void Reset()
    {
      for (int index1 = 0; index1 < Game1.maxSectionsX; ++index1)
      {
        for (int index2 = 0; index2 < Game1.maxSectionsY; ++index2)
          this.tileSection[index1, index2] = false;
      }
      if (this.whoAmI < 8)
        Game1.player[this.whoAmI] = new Player();
      this.timeOut = 0;
      this.statusCount = 0;
      this.statusMax = 0;
      this.statusText2 = "";
      this.statusText = "";
      this.name = "Anonymous";
      this.state = 0;
      this.locked = false;
      this.kill = false;
      this.active = false;
      NetMessage.buffer[this.whoAmI].Reset();

      if (this.networkStream != null)
        this.networkStream.Dispose();//.Close();

      if (this.tcpClient == null)
        return;

      this.tcpClient.Dispose();//.Close();
    }

    public void ServerWriteCallBack(IAsyncResult ar)
    {
      --NetMessage.buffer[this.whoAmI].spamCount;
      if (this.statusMax <= 0)
        return;
      ++this.statusCount;
    }

    public void ServerReadCallBack(IAsyncResult ar)
    {
      int streamLength = 0;
      if (!Netplay.disconnect)
      {
        try
        {
            //RnD
            streamLength = 0;//this.networkStream.EndRead(ar);
        }
        catch
        {
        }
        if (streamLength == 0)
          this.kill = true;
        else
          NetMessage.RecieveBytes(this.readBuffer, streamLength, this.whoAmI);
      }
      this.locked = false;
    }
  }
}
