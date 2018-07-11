using mana.Foundation;
using mana.Foundation.Network.Server;
using System;
using System.Net;
using System.Net.Sockets;


namespace mana.Network.TCP.Sever
{
    public class TCPServer : AbstractServer
    {
        private readonly BufferManager mBufferManager;

        private Socket mListenSocket;

        public TCPServer(ServerSetting setting) 
            : base(setting)
        {
            mBufferManager = new BufferManager(setting.connMax * 2, setting.connBuffSize);
        }

        public override void Start(IPEndPoint localEndPoint)
        {
            if (mListenSocket != null)
            {
                Logger.Error("Server is had start!");
                return;
            }
            Logger.Print("{0} start:{1}", this.GetType().Name, localEndPoint.ToString());
            // create the socket which listens for incoming connections
            mListenSocket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            mListenSocket.Bind(localEndPoint);
            // start the server with a listen backlog of 100 connections
            mListenSocket.Listen(100);
            // post accepts on the listening socket
            StartAccept(null);
            // start the check timeout thread
            StartDeamonThread();
        }

        /// <summary>
        ///  Begins an operation to accept a connection request from the client 
        /// </summary>
        /// <param name="acceptEventArg">The context object to use when issuing the accept operation on the server's listening socket</param>
        void StartAccept(SocketAsyncEventArgs acceptEventArg)
        {
            if (acceptEventArg == null)
            {
                acceptEventArg = new SocketAsyncEventArgs();
                acceptEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(AcceptEventArg_Completed);
            }
            else
            {
                // socket must be cleared since the context object is being reused
                acceptEventArg.AcceptSocket = null;
            }

            mTokenPool.WaitOne();
            bool willRaiseEvent = mListenSocket.AcceptAsync(acceptEventArg);
            if (!willRaiseEvent)
            {
                ProcessAccept(acceptEventArg);
            }
        }

        // This method is the callback method associated with Socket.AcceptAsync 
        // operations and is invoked when an accept operation is complete
        void AcceptEventArg_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }

        void ProcessAccept(SocketAsyncEventArgs e)
        {
            try
            {
                if (e.AcceptSocket.Connected)
                {
                    OnSocketConnected(e.AcceptSocket.RemoteEndPoint.ToString());
                    var token = mTokenPool.Get() as TCPUserToken;
                    token.Init(e.AcceptSocket);
                    token.Send(ProtocolPacket);
                }
            }
            catch (Exception ex)
            {
                Logger.Exception(ex);
            }
            finally
            {
                // Accept the next connection request
                StartAccept(e);
            }
        }

        public override UserToken CreateNewToken()
        {
            return new TCPUserToken(mBufferManager, this);
        }
    }
}