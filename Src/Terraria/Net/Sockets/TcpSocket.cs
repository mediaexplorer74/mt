﻿// TcpSocket

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using GameManager;
using GameManager.Net;

namespace GameManager.Net.Sockets
{
    public class TcpSocket : ISocket
    {
        private TcpClient _connection;
        private TcpListener _listener;
        private SocketConnectionAccepted _listenerCallback;
        private RemoteAddress _remoteAddress;
        private bool _isListening;

        public TcpSocket()
        {
            this._connection = new TcpClient();
            this._connection.NoDelay = true;
        }

        public TcpSocket(TcpClient tcpClient)
        {
            this._connection = tcpClient;
            this._connection.NoDelay = true;
            IPEndPoint ipEndPoint = (IPEndPoint)tcpClient.Client.RemoteEndPoint;
            this._remoteAddress = (RemoteAddress)new TcpAddress(ipEndPoint.Address, ipEndPoint.Port);
        }

        void ISocket.Close()
        {
            this._remoteAddress = (RemoteAddress)null;
            this._connection.Dispose();//.Close();
        }

        bool ISocket.IsConnected()
        {
            if (this._connection == null || this._connection.Client == null)
                return false;
            return this._connection.Connected;
        }

        void ISocket.Connect(RemoteAddress address)
        {
            TcpAddress tcpAddress = (TcpAddress)address;
            this._connection.ConnectAsync(tcpAddress.Address, tcpAddress.Port);//.Connect(tcpAddress.Address, tcpAddress.Port);
            this._remoteAddress = address;
        }

        private void ReadCallback(IAsyncResult result)
        {
            Tuple<SocketReceiveCallback, object> tuple = (Tuple<SocketReceiveCallback, object>)result.AsyncState;

            //RnD
            // tuple.Item1(tuple.Item2, this._connection.GetStream().EndRead(result));
            
        }

        private void SendCallback(IAsyncResult result)
        {
            Tuple<SocketSendCallback, object> tuple = (Tuple<SocketSendCallback, object>)result.AsyncState;
            try
            
            {
                // RnD
                //this._connection.GetStream().EndWrite(result);
                tuple.Item1(tuple.Item2);
            }
            catch
            {
                ((ISocket)this).Close();
            }
        }

        void ISocket.AsyncSend(byte[] data, int offset, int size, SocketSendCallback callback, object state)
        {
            //RnD
           // this._connection.GetStream().BeginWrite(data, 0, size,
           // new AsyncCallback(this.SendCallback),
           // (object)new Tuple<SocketSendCallback, object>(callback, state));
        }

        void ISocket.AsyncReceive(byte[] data, int offset, int size, SocketReceiveCallback callback, object state)
        {
            //this._connection.GetStream().BeginRead(data, offset, size,
            //new AsyncCallback(this.ReadCallback),
            //(object)new Tuple<SocketReceiveCallback, object>(callback, state));
        }

        bool ISocket.IsDataAvailable()
        {
            return this._connection.GetStream().DataAvailable;
        }

        RemoteAddress ISocket.GetRemoteAddress()
        {
            return this._remoteAddress;
        }

        bool ISocket.StartListening(SocketConnectionAccepted callback)
        {
            this._isListening = true;
            this._listenerCallback = callback;
            if (this._listener == null)
                this._listener = new TcpListener(IPAddress.Any, Netplay.ListenPort);
            try
            {
                this._listener.Start();
            }
            catch
            {
                return false;
            }

            //RnD
            //ThreadPool.QueueUserWorkItem(new WaitCallback(this.ListenLoop));
            this.ListenLoop(default);
            return true;
        }

        void ISocket.StopListening()
        {
            this._isListening = false;
        }

        private void ListenLoop(object unused)
        {
            while (this._isListening)
            {
                if (!Netplay.disconnect)
                {
                    try
                    {
                        //RnD
                        ISocket client = (ISocket)new TcpSocket();//(ISocket)new TcpSocket(this._listener.AcceptTcpClient());
                        Console.WriteLine((string)(object)client.GetRemoteAddress()
                            + (object)" is connecting...");
                        this._listenerCallback(client);
                    }
                    catch { }
                }
                else
                    break;
            }
            this._listener.Stop();
        }
    }
}
