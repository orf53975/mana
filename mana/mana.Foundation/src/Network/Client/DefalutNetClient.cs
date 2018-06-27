using System;
using System.Net;
using System.Net.Sockets;

namespace mana.Foundation.Network.Client
{
    public class DefalutNetClient : AbstractNetClient
    {
        public int port { get; private set; }

        public IPAddress remote { get; private set; }

        public Socket _socket;

        public bool isConnected
        {
            get
            {
                return _socket != null && _socket.Connected;
            }
        }


        public override void Connect(string ip, ushort port, Action<bool, Exception> callback)
        {
            //this.remote = IPAddress.Parse(ip);
            //this.port = port;
            //this._client = new TcpClient();
            //this._client.Connect(remote, port);
            //Logger.Print("Connect Sucess!");

            //var e = new SocketAsyncEventArgs();
            //e.Completed += new EventHandler<SocketAsyncEventArgs>(AsyncConnected);
            //e.RemoteEndPoint = new IPEndPoint(IPAddress.Parse(yourAddress), yourPort);
            //mySocket.ConnectAsync(e);

        }

        public override void Disconnect()
        {
            try
            {
                if (_socket != null && _socket.Connected)
                {
                    _socket.Shutdown(SocketShutdown.Both);
                }
            }
            catch (Exception e)
            {
                Logger.Exception(e);
            }
            finally
            {
                _socket.Close();
                _socket = null;
            }
        }

        protected override int ChannelPull(byte[] buffer, int offset, int size)
        {
            if (_socket.Available > 0)
            {
                return _socket.Receive(buffer, offset, size, SocketFlags.None);
            }
            else
            {
                return 0;
            }
        }

        protected override int ChannelPush(byte[] buffer, int offset, int size)
        {
            return _socket.Send(buffer, offset, size, SocketFlags.None);
        }
    }
}
