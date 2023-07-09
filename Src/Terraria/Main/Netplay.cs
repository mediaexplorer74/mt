// NetPlay

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using GameManager.IO;
using GameManager.Net;
using GameManager.Net.Sockets;

namespace GameManager
{
    public class Netplay
    {
        public static string BanFilePath = "banlist.txt";
        public static string ServerPassword = "";
        public static RemoteClient[] Clients = new RemoteClient[256];
        public static RemoteServer Connection = new RemoteServer();
        public static string ServerIPText = "";
        public static int ListenPort = 7777;
        public static bool IsServerRunning = false;
        public static bool IsListening = true;
        public static bool UseUPNP = true;
        public static bool disconnect = false;
        public static bool spamCheck = false;
        public static bool anyClients = false;
        public const int MaxConnections = 256;
        public const int NetBufferSize = 1024;
        public static IPAddress ServerIP;
        public static ISocket TcpListener;
        public static string portForwardIP;
        public static int portForwardPort;
        public static bool portForwardOpen;

        public static event Action OnDisconnect;

        private static void OpenPort()
        {
            Netplay.portForwardIP = Netplay.GetLocalIPAddress();
            Netplay.portForwardPort = Netplay.ListenPort;
        }

        public static void closePort()
        {
        }

        public static string GetLocalIPAddress()
        {
            string str = "";

            //RnD
            str = "127.0.0.1";
            //foreach (IPAddress ipAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            //{
            //    if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
            //    {
            //        str = ipAddress.ToString();
            //        break;
            //    }
            //}


            return str;
        }

        public static void ResetNetDiag()
        {
            Game1.rxMsg = 0;
            Game1.rxData = 0;
            Game1.txMsg = 0;
            Game1.txData = 0;
            for (int index = 0; index < Game1.maxMsg; ++index)
            {
                Game1.rxMsgType[index] = 0;
                Game1.rxDataType[index] = 0;
                Game1.txMsgType[index] = 0;
                Game1.txDataType[index] = 0;
            }
        }

        public static void ResetSections()
        {
            for (int index1 = 0; index1 < 256; ++index1)
            {
                for (int index2 = 0; index2 < Game1.maxSectionsX; ++index2)
                {
                    for (int index3 = 0; index3 < Game1.maxSectionsY; ++index3)
                        Netplay.Clients[index1].TileSections[index2, index3] = false;
                }
            }
        }

        public static void AddBan(int plr)
        {
            //RnD
            //RemoteAddress remoteAddress = Netplay.Clients[plr].Socket.GetRemoteAddress();
            //using (StreamWriter streamWriter = new StreamWriter(Netplay.BanFilePath, true))
            //{
            //    streamWriter.WriteLine("//" + Main.player[plr].name);
            //    streamWriter.WriteLine(remoteAddress.GetIdentifier());
            //}
        }

        public static bool IsBanned(RemoteAddress address)
        {
            try
            {
                string identifier = address.GetIdentifier();

                // RnD
                if (System.IO.File.Exists(Netplay.BanFilePath))
                {
                //    using (StreamReader streamReader = new StreamReader(Netplay.BanFilePath))
                //    {
                //        string str;
                //        while ((str = streamReader.ReadLine()) != null)
                //        {
                //            if (str == identifier)
                //                return true;
                //        }
                //    }
                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        public static void newRecent()
        {
            if (Netplay.Connection.Socket.GetRemoteAddress().Type != AddressType.Tcp)
                return;
            for (int index1 = 0; index1 < Game1.maxMP; ++index1)
            {
                if (Game1.recentIP[index1].ToLower() == Netplay.ServerIPText.ToLower() && Game1.recentPort[index1] == Netplay.ListenPort)
                {
                    for (int index2 = index1; index2 < Game1.maxMP - 1; ++index2)
                    {
                        Game1.recentIP[index2] = Game1.recentIP[index2 + 1];
                        Game1.recentPort[index2] = Game1.recentPort[index2 + 1];
                        Game1.recentWorld[index2] = Game1.recentWorld[index2 + 1];
                    }
                }
            }
            for (int index = Game1.maxMP - 1; index > 0; --index)
            {
                Game1.recentIP[index] = Game1.recentIP[index - 1];
                Game1.recentPort[index] = Game1.recentPort[index - 1];
                Game1.recentWorld[index] = Game1.recentWorld[index - 1];
            }
            Game1.recentIP[0] = Netplay.ServerIPText;
            Game1.recentPort[0] = Netplay.ListenPort;
            Game1.recentWorld[0] = Game1.worldName;
            Game1.SaveRecent();
        }

        public static void SocialClientLoop(object threadContext)
        {
            ISocket socket = (ISocket)threadContext;
            Netplay.ClientLoopSetup(socket.GetRemoteAddress());
            Netplay.Connection.Socket = socket;
            Netplay.InnerClientLoop();
        }

        public static void TcpClientLoop(object threadContext)
        {
            Netplay.ClientLoopSetup((RemoteAddress)new TcpAddress(Netplay.ServerIP, Netplay.ListenPort));
            Game1.menuMode = 14;
            bool flag = true;
            while (flag)
            {
                flag = false;
                try
                {
                    Netplay.Connection.Socket.Connect((RemoteAddress)new TcpAddress(Netplay.ServerIP, Netplay.ListenPort));
                    flag = false;
                }
                catch
                {
                    if (!Netplay.disconnect)
                    {
                        if (Game1.gameMenu)
                            flag = true;
                    }
                }
            }
            Netplay.InnerClientLoop();
        }

        private static void ClientLoopSetup(RemoteAddress address)
        {
            Netplay.ResetNetDiag();
            Game1.ServerSideCharacter = false;
            if (Game1.rand == null)
                Game1.rand = new Random((int)DateTime.Now.Ticks);
            if (WorldGen.genRand == null)
                WorldGen.genRand = new Random((int)DateTime.Now.Ticks);
            Game1.player[Game1.myPlayer].hostile = false;
            Game1.clientPlayer = (Player)Game1.player[Game1.myPlayer].clientClone();
            for (int index = 0; index < (int)byte.MaxValue; ++index)
            {
                if (index != Game1.myPlayer)
                    Game1.player[index] = new Player();
            }
            Game1.netMode = 1;
            Game1.menuMode = 14;
            if (!Game1.autoPass)
                Game1.statusText = "Connecting to " + address.GetFriendlyName();
            Netplay.disconnect = false;
            Netplay.Connection = new RemoteServer();
            Netplay.Connection.ReadBuffer = new byte[1024];
        }

        private static void InnerClientLoop()
        {
            try
            {
                NetMessage.buffer[256].Reset();
                int num1 = -1;
                while (!Netplay.disconnect)
                {
                    if (Netplay.Connection.Socket.IsConnected())
                    {
                        if (NetMessage.buffer[256].checkBytes)
                            NetMessage.CheckBytes(256);
                        Netplay.Connection.IsActive = true;
                        if (Netplay.Connection.State == 0)
                        {
                            Game1.statusText = "Found server";
                            Netplay.Connection.State = 1;
                            NetMessage.SendData(1, -1, -1, "", 0, 0.0f, 0.0f, 0.0f, 0, 0, 0);
                        }
                        if (Netplay.Connection.State == 2 && num1 != Netplay.Connection.State)
                            Game1.statusText = "Sending player data...";
                        if (Netplay.Connection.State == 3 && num1 != Netplay.Connection.State)
                            Game1.statusText = "Requesting world information";
                        if (Netplay.Connection.State == 4)
                        {
                            WorldGen.worldCleared = false;
                            Netplay.Connection.State = 5;
                            Game1.cloudBGAlpha = (double)Game1.cloudBGActive < 1.0 ? 0.0f : 1f;
                            Game1.windSpeed = Game1.windSpeedSet;
                            Cloud.resetClouds();
                            Game1.cloudAlpha = Game1.maxRaining;
                            WorldGen.clearWorld();
                            if (Game1.mapEnabled)
                                Game1.Map.Load();
                        }
                        if (Netplay.Connection.State == 5 && Game1.loadMapLock)
                        {
                            float num2 = (float)Game1.loadMapLastX / (float)Game1.maxTilesX;
                            Game1.statusText = string.Concat(new object[4]
              {
                (object) Lang.gen[68],
                (object) " ",
                (object) (int) ((double) num2 * 100.0 + 1.0),
                (object) "%"
              });
                        }
                        else if (Netplay.Connection.State == 5 && WorldGen.worldCleared)
                        {
                            Netplay.Connection.State = 6;
                            Game1.player[Game1.myPlayer].FindSpawn();
                            NetMessage.SendData(8, -1, -1, "", Game1.player[Game1.myPlayer].SpawnX, (float)Game1.player[Game1.myPlayer].SpawnY, 0.0f, 0.0f, 0, 0, 0);
                        }
                        if (Netplay.Connection.State == 6 && num1 != Netplay.Connection.State)
                            Game1.statusText = "Requesting tile data";
                        if (!Netplay.Connection.IsReading && !Netplay.disconnect && Netplay.Connection.Socket.IsDataAvailable())
                        {
                            Netplay.Connection.IsReading = true;
                            Netplay.Connection.Socket.AsyncReceive(Netplay.Connection.ReadBuffer, 0, Netplay.Connection.ReadBuffer.Length, new SocketReceiveCallback(Netplay.Connection.ClientReadCallBack), (object)null);
                        }
                        if (Netplay.Connection.StatusMax > 0 && Netplay.Connection.StatusText != "")
                        {
                            if (Netplay.Connection.StatusCount >= Netplay.Connection.StatusMax)
                            {
                                Game1.statusText = Netplay.Connection.StatusText + ": Complete!";
                                Netplay.Connection.StatusText = "";
                                Netplay.Connection.StatusMax = 0;
                                Netplay.Connection.StatusCount = 0;
                            }
                            else
                                Game1.statusText = string.Concat(new object[4]
                {
                  (object) Netplay.Connection.StatusText,
                  (object) ": ",
                  (object) (int) ((double) Netplay.Connection.StatusCount / (double) Netplay.Connection.StatusMax * 100.0),
                  (object) "%"
                });
                        }
                        Thread.Sleep(1);
                    }
                    else if (Netplay.Connection.IsActive)
                    {
                        Game1.statusText = "Lost connection";
                        Netplay.disconnect = true;
                    }
                    num1 = Netplay.Connection.State;
                }
                try
                {
                    Netplay.Connection.Socket.Close();
                }
                catch
                {
                }
                if (!Game1.gameMenu)
                {
                    Game1.SwitchNetMode(0);
                    Player.SavePlayer(Game1.ActivePlayerFileData, false);
                    Game1.ActivePlayerFileData.StopPlayTimer();
                    Game1.gameMenu = true;
                    Game1.menuMode = 14;
                }
                NetMessage.buffer[256].Reset();
                if (Game1.menuMode == 15 && Game1.statusText == "Lost connection")
                    Game1.menuMode = 14;
                if (Netplay.Connection.StatusText != "" && Netplay.Connection.StatusText != null)
                    Game1.statusText = "Lost connection";
                Netplay.Connection.StatusCount = 0;
                Netplay.Connection.StatusMax = 0;
                Netplay.Connection.StatusText = "";
                Game1.SwitchNetMode(0);
            }
            catch (Exception ex)
            {
                try
                {
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
            if (Netplay.OnDisconnect == null)
                return;
            Netplay.OnDisconnect();
        }

        private static int FindNextOpenClientSlot()
        {
            for (int index = 0; index < Game1.maxNetPlayers; ++index)
            {
                if (!Netplay.Clients[index].Socket.IsConnected())
                    return index;
            }
            return -1;
        }

        private static void OnConnectionAccepted(ISocket client)
        {
            int nextOpenClientSlot = Netplay.FindNextOpenClientSlot();
            if (nextOpenClientSlot != -1)
            {
                Netplay.Clients[nextOpenClientSlot].Socket = client;
                Debug.WriteLine((string)(object)client.GetRemoteAddress() + (object)" is connecting...");
            }
            if (Netplay.FindNextOpenClientSlot() != -1)
                return;
            Netplay.StopListening();
        }

        public static void OnConnectedToSocialServer(ISocket client)
        {
            Netplay.StartSocialClient(client);
        }

        private static bool StartListening()
        {
            return Netplay.TcpListener.StartListening(new SocketConnectionAccepted(Netplay.OnConnectionAccepted));
        }

        private static void StopListening()
        {
            Netplay.TcpListener.StopListening();
        }

        public static void ServerLoop(object threadContext)
        {
            Netplay.ResetNetDiag();
            if (Game1.rand == null)
                Game1.rand = new Random((int)DateTime.Now.Ticks);
            if (WorldGen.genRand == null)
                WorldGen.genRand = new Random((int)DateTime.Now.Ticks);
            Game1.myPlayer = (int)byte.MaxValue;
            Netplay.ServerIP = IPAddress.Any;
            Game1.menuMode = 14;
            Game1.statusText = "Starting server...";
            Game1.netMode = 2;
            Netplay.disconnect = false;
            for (int index = 0; index < 256; ++index)
            {
                Netplay.Clients[index] = new RemoteClient();
                Netplay.Clients[index].Reset();
                Netplay.Clients[index].Id = index;
                Netplay.Clients[index].ReadBuffer = new byte[1024];
            }
            Netplay.TcpListener = (ISocket)new TcpSocket();
            if (!Netplay.disconnect)
            {
                if (!Netplay.StartListening())
                {
                    Game1.menuMode = 15;
                    Game1.statusText = "Tried to run two servers on the same PC";
                    Netplay.disconnect = true;
                }
                Game1.statusText = "Server started";
            }
            if (Netplay.UseUPNP)
            {
                try
                {
                    Netplay.OpenPort();
                }
                catch
                {
                }
            }
            int num1 = 0;
            while (!Netplay.disconnect)
            {
                if (!Netplay.IsListening)
                {
                    int num2 = -1;
                    for (int index = 0; index < Game1.maxNetPlayers; ++index)
                    {
                        if (!Netplay.Clients[index].Socket.IsConnected())
                        {
                            num2 = index;
                            break;
                        }
                    }
                    if (num2 >= 0)
                    {
                        if (Game1.ignoreErrors)
                        {
                            try
                            {
                                Netplay.StartListening();
                                Netplay.IsListening = true;
                            }
                            catch
                            {
                            }
                        }
                        else
                        {
                            Netplay.StartListening();
                            Netplay.IsListening = true;
                        }
                    }
                }
                int num3 = 0;
                for (int bufferIndex = 0; bufferIndex < 256; ++bufferIndex)
                {
                    if (NetMessage.buffer[bufferIndex].checkBytes)
                        NetMessage.CheckBytes(bufferIndex);
                    if (Netplay.Clients[bufferIndex].PendingTermination)
                    {
                        Netplay.Clients[bufferIndex].Reset();
                        NetMessage.syncPlayers();
                    }
                    else if (Netplay.Clients[bufferIndex].Socket.IsConnected())
                    {
                        if (!Netplay.Clients[bufferIndex].IsActive)
                            Netplay.Clients[bufferIndex].State = 0;
                        Netplay.Clients[bufferIndex].IsActive = true;
                        ++num3;
                        if (!Netplay.Clients[bufferIndex].IsReading)
                        {
                            try
                            {
                                if (Netplay.Clients[bufferIndex].Socket.IsDataAvailable())
                                {
                                    Netplay.Clients[bufferIndex].IsReading = true;
                                    Netplay.Clients[bufferIndex].Socket.AsyncReceive(Netplay.Clients[bufferIndex].ReadBuffer, 0, Netplay.Clients[bufferIndex].ReadBuffer.Length, new SocketReceiveCallback(Netplay.Clients[bufferIndex].ServerReadCallBack), (object)null);
                                }
                            }
                            catch
                            {
                                Netplay.Clients[bufferIndex].PendingTermination = true;
                            }
                        }
                        if (Netplay.Clients[bufferIndex].StatusMax > 0 && Netplay.Clients[bufferIndex].StatusText2 != "")
                        {
                            if (Netplay.Clients[bufferIndex].StatusCount >= Netplay.Clients[bufferIndex].StatusMax)
                            {
                                Netplay.Clients[bufferIndex].StatusText = "(" + (object)Netplay.Clients[bufferIndex].Socket.GetRemoteAddress() + ") " + Netplay.Clients[bufferIndex].Name + " " + Netplay.Clients[bufferIndex].StatusText2 + ": Complete!";
                                Netplay.Clients[bufferIndex].StatusText2 = "";
                                Netplay.Clients[bufferIndex].StatusMax = 0;
                                Netplay.Clients[bufferIndex].StatusCount = 0;
                            }
                            else
                                Netplay.Clients[bufferIndex].StatusText = "(" + (object)Netplay.Clients[bufferIndex].Socket.GetRemoteAddress() + ") " + Netplay.Clients[bufferIndex].Name + " " + Netplay.Clients[bufferIndex].StatusText2 + ": " + (string)(object)(int)((double)Netplay.Clients[bufferIndex].StatusCount / (double)Netplay.Clients[bufferIndex].StatusMax * 100.0) + "%";
                        }
                        else if (Netplay.Clients[bufferIndex].State == 0)
                            Netplay.Clients[bufferIndex].StatusText = "(" + (object)Netplay.Clients[bufferIndex].Socket.GetRemoteAddress() + ") " + Netplay.Clients[bufferIndex].Name + " is connecting...";
                        else if (Netplay.Clients[bufferIndex].State == 1)
                            Netplay.Clients[bufferIndex].StatusText = "(" + (object)Netplay.Clients[bufferIndex].Socket.GetRemoteAddress() + ") " + Netplay.Clients[bufferIndex].Name + " is sending player data...";
                        else if (Netplay.Clients[bufferIndex].State == 2)
                            Netplay.Clients[bufferIndex].StatusText = "(" + (object)Netplay.Clients[bufferIndex].Socket.GetRemoteAddress() + ") " + Netplay.Clients[bufferIndex].Name + " requested world information";
                        else if (Netplay.Clients[bufferIndex].State != 3)
                        {
                            if (Netplay.Clients[bufferIndex].State == 10)
                            {
                                try
                                {
                                    Netplay.Clients[bufferIndex].StatusText = "(" + (object)Netplay.Clients[bufferIndex].Socket.GetRemoteAddress() + ") " + Netplay.Clients[bufferIndex].Name + " is playing";
                                }
                                catch
                                {
                                    Netplay.Clients[bufferIndex].PendingTermination = true;
                                }
                            }
                        }
                    }
                    else if (Netplay.Clients[bufferIndex].IsActive)
                    {
                        Netplay.Clients[bufferIndex].PendingTermination = true;
                    }
                    else
                    {
                        Netplay.Clients[bufferIndex].StatusText2 = "";
                        if (bufferIndex < (int)byte.MaxValue)
                            Game1.player[bufferIndex].active = false;
                    }
                }
                ++num1;
                if (num1 > 10)
                {
                    Thread.Sleep(1);
                    num1 = 0;
                }
                else
                    Thread.Sleep(0);
                if (!WorldGen.saveLock && !Game1.dedServ)
                    Game1.statusText = num3 != 0 ? (string)(object)num3 + (object)" clients connected" : "Waiting for clients...";
                Netplay.anyClients = num3 != 0;
                Netplay.IsServerRunning = true;
            }
            Netplay.StopListening();
            try
            {
                Netplay.closePort();
            }
            catch
            {
            }
            for (int index = 0; index < 256; ++index)
                Netplay.Clients[index].Reset();
            if (Game1.menuMode != 15)
            {
                Game1.netMode = 0;
                Game1.menuMode = 10;
                WorldFile.saveWorld();
                while (WorldGen.saveLock);
                Game1.menuMode = 0;
            }
            else
                Game1.netMode = 0;
            Game1.myPlayer = 0;
        }

        public static void StartSocialClient(ISocket socket)
        {
            //ThreadPool.QueueUserWorkItem(new WaitCallback(Netplay.SocialClientLoop), (object)socket);
            Netplay.SocialClientLoop(default);
        }

        public static void StartTcpClient()
        {
            //ThreadPool.QueueUserWorkItem(new WaitCallback(Netplay.TcpClientLoop), (object)1);
            Netplay.TcpClientLoop(default);
        }

        public static void StartServer()
        {
            //ThreadPool.QueueUserWorkItem(new WaitCallback(Netplay.ServerLoop), (object)1);
            Netplay.ServerLoop(default);
        }

        public static bool SetRemoteIP(string remoteAddress)
        {
            try
            {
                IPAddress address;
                if (IPAddress.TryParse(remoteAddress, out address))
                {
                    Netplay.ServerIP = address;
                    Netplay.ServerIPText = address.ToString();
                    return true;
                }

                // RnD
                IPAddress[] addressList = default;//Dns.GetHostEntry(remoteAddress).AddressList;
                for (int index = 0; index < addressList.Length; ++index)
                {
                    if (addressList[index].AddressFamily == AddressFamily.InterNetwork)
                    {
                        Netplay.ServerIP = addressList[index];
                        Netplay.ServerIPText = remoteAddress;
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        public static void Initialize()
        {
            NetMessage.buffer[256] = new MessageBuffer();
            NetMessage.buffer[256].whoAmI = 256;
        }

        public static int GetSectionX(int x)
        {
            return x / 200;
        }

        public static int GetSectionY(int y)
        {
            return y / 150;
        }
    }
}
