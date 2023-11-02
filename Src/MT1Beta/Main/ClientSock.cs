// Decompiled with JetBrains decompiler
// Type: GameManager.ClientSock
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 09A1FDD7-68B3-40FF-A030-32890DF6B0E7
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager v1 Beta\GameManager.exe

using System;
using System.Net.Sockets;

namespace GameManager
{
  public class ClientSock
  {
    public TcpClient tcpClient = new TcpClient();
    public NetworkStream networkStream;
    public string statusText;
    public int statusCount;
    public int statusMax;
    public int timeOut = 0;
    public byte[] readBuffer;
    public byte[] writeBuffer;
    public bool active = false;
    public bool locked = false;
    public int state = 0;

    public void ClientWriteCallBack(IAsyncResult ar) => --NetMessage.buffer[9].spamCount;

    public void ClientReadCallBack(IAsyncResult ar)
    {
      if (!Netplay.disconnect)
      {
        //RnD
        int streamLength = 0;//this.networkStream.EndRead(ar);

        if (streamLength == 0)
        {
          Netplay.disconnect = true;
          Game1.statusText = "Lost connection";
        }
        else
          NetMessage.RecieveBytes(this.readBuffer, streamLength);
      }
      this.locked = false;
    }
  }
}
