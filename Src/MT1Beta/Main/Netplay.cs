// Decompiled with JetBrains decompiler
// Type: GameManager.Netplay
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 09A1FDD7-68B3-40FF-A030-32890DF6B0E7
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager v1 Beta\GameManager.exe

using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Windows.System.Threading;

namespace GameManager
{
  public class Netplay
  {
    public const int bufferSize = 1024;
    public const int maxConnections = 9;
    public static ServerSock[] serverSock = new ServerSock[9];
    public static ClientSock clientSock = new ClientSock();
    public static TcpListener tcpListener;
    public static IPAddress serverListenIP;
    public static IPAddress serverIP;
    public static int serverPort = 31337;
    public static bool disconnect = false;
    public static string password = "";

    public static void ClientLoop(object threadContext)
    {
      if (Game1.rand == null)
        Game1.rand = new Random((int) DateTime.Now.Ticks);
      if (WorldGen.genRand == null)
        WorldGen.genRand = new Random((int) DateTime.Now.Ticks);
      Game1.player[Game1.myPlayer].hostile = false;
      Game1.clientPlayer = (Player) Game1.player[Game1.myPlayer].clientClone();
      Game1.menuMode = 10;
      Game1.menuMode = 14;
      Game1.statusText = "Connecting to " + (object) Netplay.serverIP;
      Game1.netMode = 1;
      Netplay.disconnect = false;
      Netplay.clientSock = new ClientSock();
      Netplay.clientSock.tcpClient.NoDelay = true;
      Netplay.clientSock.readBuffer = new byte[1024];
      Netplay.clientSock.writeBuffer = new byte[1024];
      try
      {
        //RnD
        Netplay.clientSock.tcpClient.ConnectAsync(Netplay.serverIP, Netplay.serverPort);//.Connect(Netplay.serverIP, Netplay.serverPort);
        Netplay.clientSock.networkStream = Netplay.clientSock.tcpClient.GetStream();
      }
      catch (Exception ex)
      {
        if (Netplay.disconnect || !Game1.gameMenu)
        {
          Debug.WriteLine("   Exception normal: Player hit cancel before initiating a connection.");
        }
        else
        {
          Game1.menuMode = 15;
          Game1.statusText = ex.ToString();
          Netplay.disconnect = true;
          Debug.WriteLine("   Exception normal: May be caused by a connection timeout.");
        }
      }
      NetMessage.buffer[9].Reset();
      int num = -1;
      while (!Netplay.disconnect)
      {
        if (Netplay.clientSock.tcpClient.Connected)
        {
          if (NetMessage.buffer[9].checkBytes)
            NetMessage.CheckBytes();
          Netplay.clientSock.active = true;
          if (Netplay.clientSock.state == 0)
          {
            Game1.statusText = "Found server";
            Netplay.clientSock.state = 1;
            NetMessage.SendData(1);
          }
          if (Netplay.clientSock.state == 2 && num != Netplay.clientSock.state)
            Game1.statusText = "Sending player data...";
          if (Netplay.clientSock.state == 3 && num != Netplay.clientSock.state)
            Game1.statusText = "Requesting world information";
          if (Netplay.clientSock.state == 4)
          {
            WorldGen.worldCleared = false;
            Netplay.clientSock.state = 5;
            WorldGen.clearWorld();
          }
          if (Netplay.clientSock.state == 5 && WorldGen.worldCleared)
          {
            Netplay.clientSock.state = 6;
            NetMessage.SendData(8);
          }
          if (Netplay.clientSock.state == 6 && num != Netplay.clientSock.state)
            Game1.statusText = "Requesting tile data";
          if (!Netplay.clientSock.locked && !Netplay.disconnect && Netplay.clientSock.networkStream.DataAvailable)
          {
            Netplay.clientSock.locked = true;

            //RnD
            //Netplay.clientSock.networkStream.BeginRead(Netplay.clientSock.readBuffer, 0, Netplay.clientSock.readBuffer.Length, new AsyncCallback(Netplay.clientSock.ClientReadCallBack), (object) Netplay.clientSock.networkStream);
          }
          if (Netplay.clientSock.statusMax > 0 && Netplay.clientSock.statusText != "")
          {
            if (Netplay.clientSock.statusCount >= Netplay.clientSock.statusMax)
            {
              Game1.statusText = Netplay.clientSock.statusText + ": Complete!";
              Netplay.clientSock.statusText = "";
              Netplay.clientSock.statusMax = 0;
              Netplay.clientSock.statusCount = 0;
            }
            else
              Game1.statusText = Netplay.clientSock.statusText + ": " + (object) (int) ((double) Netplay.clientSock.statusCount / (double) Netplay.clientSock.statusMax * 100.0) + "%";
          }
        }
        else if (Netplay.clientSock.active)
        {
          Game1.statusText = "Lost connection";
          Netplay.disconnect = true;
        }
        num = Netplay.clientSock.state;
      }
      try
      {
                Netplay.clientSock.networkStream.Dispose();//.Close();
        Netplay.clientSock.networkStream = Netplay.clientSock.tcpClient.GetStream();
      }
      catch
      {
        Debug.WriteLine("   Exception normal: Redundant closing of the TCP Client and/or Network Stream.");
      }
      if (!Game1.gameMenu)
      {
        Game1.netMode = 0;
        Player.SavePlayer(Game1.player[Game1.myPlayer], Game1.playerPathName);
        Game1.gameMenu = true;
        Game1.menuMode = 14;
      }
      NetMessage.buffer[9].Reset();
      if (Game1.menuMode == 15 && Game1.statusText == "Lost connection")
        Game1.menuMode = 14;
      if (Netplay.clientSock.statusText != "" && Netplay.clientSock.statusText != null)
        Game1.statusText = "Lost connection";
      Netplay.clientSock.statusCount = 0;
      Netplay.clientSock.statusMax = 0;
      Netplay.clientSock.statusText = "";
      Game1.netMode = 0;
    }

    public static void ServerLoop(object threadContext)
    {
      if (Game1.rand == null)
        Game1.rand = new Random((int) DateTime.Now.Ticks);
      if (WorldGen.genRand == null)
        WorldGen.genRand = new Random((int) DateTime.Now.Ticks);
      Game1.myPlayer = 8;
      string ipString = "?";

     //RnD
                                 // Dns.GetHostEntry(Dns.GetHostName()).AddressList)
      //foreach (IPAddress address in Dns.GetHostEntryAsync(Dns.GetHostName()).AddressList)
      //{
      //  if (address.AddressFamily == AddressFamily.InterNetwork)
      //    ipString = address.ToString();
      //}
      Netplay.serverIP = IPAddress.Parse(ipString);
      Netplay.serverListenIP = Netplay.serverIP;
      Game1.menuMode = 14;
      Game1.statusText = "Starting server...";
      Game1.netMode = 2;
      Netplay.disconnect = false;
      for (int index = 0; index < 9; ++index)
      {
        Netplay.serverSock[index] = new ServerSock();
        Netplay.serverSock[index].Reset();
        Netplay.serverSock[index].whoAmI = index;
        Netplay.serverSock[index].tcpClient = new TcpClient();
        Netplay.serverSock[index].tcpClient.NoDelay = true;
        Netplay.serverSock[index].readBuffer = new byte[1024];
        Netplay.serverSock[index].writeBuffer = new byte[1024];
      }
      Netplay.tcpListener = new TcpListener(Netplay.serverListenIP, Netplay.serverPort);
      try
      {
        Netplay.tcpListener.Start();
      }
      catch (Exception ex)
      {
        Game1.menuMode = 15;
        Game1.statusText = ex.ToString();
        Netplay.disconnect = true;
        Debug.WriteLine("   Exception normal: Tried to run two servers on the same PC");
      }
      if (!Netplay.disconnect)
      {
        //RnD
        //ThreadPool.QueueUserWorkItem(new WaitCallback(Netplay.ListenForClients), (object) 1);
        Netplay.ListenForClients(default);

        Game1.statusText = "Server started";
      }
      while (!Netplay.disconnect)
      {
        int num = 0;
        for (int i = 0; i < 9; ++i)
        {
          if (NetMessage.buffer[i].checkBytes)
            NetMessage.CheckBytes(i);
          if (Netplay.serverSock[i].kill)
          {
            Netplay.serverSock[i].Reset();
            NetMessage.syncPlayers();
          }
          else if (Netplay.serverSock[i].tcpClient.Connected)
          {
            if (!Netplay.serverSock[i].active)
              Netplay.serverSock[i].state = 0;
            Netplay.serverSock[i].active = true;
            ++num;
            if (!Netplay.serverSock[i].locked)
            {
              try
              {
                Netplay.serverSock[i].networkStream = Netplay.serverSock[i].tcpClient.GetStream();
                if (Netplay.serverSock[i].networkStream.DataAvailable)
                {
                  Netplay.serverSock[i].locked = true;
                                    //RnD
                  //Netplay.serverSock[i].networkStream.BeginRead(Netplay.serverSock[i].readBuffer, 0, Netplay.serverSock[i].readBuffer.Length, new AsyncCallback(Netplay.serverSock[i].ServerReadCallBack), (object) Netplay.serverSock[i].networkStream);
                }
              }
              catch
              {
                Debug.WriteLine("   Exception normal: Tried to get data from a client after losing connection");
                Netplay.serverSock[i].kill = true;
              }
            }
            if (Netplay.serverSock[i].statusMax > 0 && Netplay.serverSock[i].statusText2 != "")
            {
              if (Netplay.serverSock[i].statusCount >= Netplay.serverSock[i].statusMax)
              {
                Netplay.serverSock[i].statusText = "(" + (object) Netplay.serverSock[i].tcpClient.Client.RemoteEndPoint + ") " + Netplay.serverSock[i].name + " " + Netplay.serverSock[i].statusText2 + ": Complete!";
                Netplay.serverSock[i].statusText2 = "";
                Netplay.serverSock[i].statusMax = 0;
                Netplay.serverSock[i].statusCount = 0;
              }
              else
                Netplay.serverSock[i].statusText = "(" + (object) Netplay.serverSock[i].tcpClient.Client.RemoteEndPoint + ") " + Netplay.serverSock[i].name + " " + Netplay.serverSock[i].statusText2 + ": " + (object) (int) ((double) Netplay.serverSock[i].statusCount / (double) Netplay.serverSock[i].statusMax * 100.0) + "%";
            }
            else if (Netplay.serverSock[i].state == 0)
              Netplay.serverSock[i].statusText = "(" + (object) Netplay.serverSock[i].tcpClient.Client.RemoteEndPoint + ") " + Netplay.serverSock[i].name + " is connecting...";
            else if (Netplay.serverSock[i].state == 1)
              Netplay.serverSock[i].statusText = "(" + (object) Netplay.serverSock[i].tcpClient.Client.RemoteEndPoint + ") " + Netplay.serverSock[i].name + " is sending player data...";
            else if (Netplay.serverSock[i].state == 2)
              Netplay.serverSock[i].statusText = "(" + (object) Netplay.serverSock[i].tcpClient.Client.RemoteEndPoint + ") " + Netplay.serverSock[i].name + " requested world information";
            else if (Netplay.serverSock[i].state != 3 && Netplay.serverSock[i].state == 10)
              Netplay.serverSock[i].statusText = "(" + (object) Netplay.serverSock[i].tcpClient.Client.RemoteEndPoint + ") " + Netplay.serverSock[i].name + " is playing";
          }
          else if (Netplay.serverSock[i].active)
          {
            Netplay.serverSock[i].kill = true;
          }
          else
          {
            Netplay.serverSock[i].statusText2 = "";
            if (i < 8)
              Game1.player[i].active = false;
          }
        }
        if (!WorldGen.saveLock)
          Game1.statusText = num != 0 ? num.ToString() + " clients connected" : "Waiting for clients...";
      }
      Netplay.tcpListener.Stop();
      for (int index = 0; index < 9; ++index)
        Netplay.serverSock[index].Reset();
      if (Game1.menuMode != 15)
      {
        Game1.netMode = 0;
        Game1.menuMode = 10;
        WorldGen.saveWorld();

        //RnD
        do
        { }
        while (WorldGen.saveLock);
        
       Game1.menuMode = 0;
      }
      else
        Game1.netMode = 0;
      Game1.myPlayer = 0;
    }

    public static void ListenForClients(object threadContext)
    {
      while (!Netplay.disconnect)
      {
        int index1 = -1;
        for (int index2 = 0; index2 < 8; ++index2)
        {
          if (!Netplay.serverSock[index2].tcpClient.Connected)
          {
            index1 = index2;
            break;
          }
        }
        if (index1 >= 0)
        {
          try
          {
                        //RnD
            //Netplay.serverSock[index1].tcpClient = Netplay.tcpListener.AcceptTcpClient();
            Netplay.serverSock[index1].tcpClient.NoDelay = true;
          }
          catch (Exception ex)
          {
            if (!Netplay.disconnect)
            {
              Game1.menuMode = 15;
              Game1.statusText = ex.ToString();
              Netplay.disconnect = true;
            }
            else
              Debug.WriteLine("   Exception normal: Server shut down.");
          }
        }
      }
    }

        public static void StartClient()
        {
            //ThreadPool.QueueUserWorkItem(new WaitCallback(Netplay.ClientLoop), (object)1); 
            Netplay.ClientLoop(default);
        }

        public static void StartServer()
        {
            //ThreadPool.QueueUserWorkItem(new WaitCallback(Netplay.ServerLoop), (object)1);
            Netplay.ServerLoop(default);
        }

        public static void SetIP(string newIP)
        { 
            Netplay.serverIP = IPAddress.Parse(newIP); 
        }

    public static void Init()
    {
      for (int index = 0; index < 10; ++index)
      {
        if (index < 9)
        {
          Netplay.serverSock[index] = new ServerSock();
          Netplay.serverSock[index].tcpClient.NoDelay = true;
        }
        NetMessage.buffer[index] = new messageBuffer();
        NetMessage.buffer[index].whoAmI = index;
      }
      Netplay.clientSock.tcpClient.NoDelay = true;
    }

    public static int GetSectionX(int x) => x / 200;

    public static int GetSectionY(int y) => y / 150;
  }
}
