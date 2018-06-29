using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace mana.Foundation.Network.Sever
{
    public class IOCPServer
    {
        public readonly int tokenUnbindTimeOut;

        public readonly int tokenWorkTimeOut;

        private readonly UserTokenPool mTokenPool;

        private Socket mListenSocket;

        private Packet mProtocolPacket = null;

        private int m_numConnectedSockets;
        public int ConnectionNum
        {
            get
            {
                return m_numConnectedSockets;
            }
        }

        public IOCPServer(int numConnections, int bufferSize, int tokenUnbindTimeOut = 1000 * 10, int tokenWorkTimeOut = 3000 * 10)
        {
            this.mTokenPool = new UserTokenPool(this, numConnections, bufferSize);
            this.m_numConnectedSockets = 0;
            this.tokenUnbindTimeOut = tokenUnbindTimeOut;
            this.tokenWorkTimeOut = tokenWorkTimeOut;
            this.DoInitTypes();
            Packet.ChangePoolCapacity(8192);
        }

        private void DoInitTypes()
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            // -- init DataObject
            var doTypes = AppDomain.CurrentDomain.GetClassTypes<DataObject>();
            foreach (var type in doTypes)
            {
                ProtocolManager.AddTypeCode(type);
            }
            // -- init ITypeInitializable
            var types = AppDomain.CurrentDomain.GetClassTypes<ITypeInitializable>();
            foreach(var type in types)
            {
                if (typeof(IMessageHandler).IsAssignableFrom(type))
                {
                    MessageDispatcher.Instance.RegistHandler(type);
                }
                if (typeof(IPushRegister).IsAssignableFrom(type))
                {
                    var pr = Activator.CreateInstance(type) as IPushRegister;
                    pr.DoRegist();
                }
            }
            this.OnInitTypes(types);
            Logger.Print("DoInitTypes:{0}", sw.ElapsedMilliseconds);
            Logger.Print(Protocol.Instance.ToFormatString(""));
        }

        protected virtual void OnInitTypes(List<Type> types)
        {
        }

        // Starts the server such that it is listening for 
        // incoming connection requests.    
        //
        // <param name="localEndPoint">The endpoint which the server will listening 
        // for connection requests on</param>
        public void Start(IPEndPoint localEndPoint)
        {
            if (mListenSocket != null)
            {
                Logger.Error("Server is had start!");
                return;
            }
            Logger.Print(localEndPoint.ToString());
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


        // Begins an operation to accept a connection request from the client 
        //
        // <param name="acceptEventArg">The context object to use when issuing 
        // the accept operation on the server's listening socket</param>
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

            mTokenPool.acceptedClientsSemaphore.WaitOne();
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
                    var token = mTokenPool.Get();
                    token.Init(e.AcceptSocket);
                    if (mProtocolPacket == null)
                    {
                        mProtocolPacket = Packet.CreatPush("Connector.Protocol", Protocol.Instance, false);
                    }
                    token.Send(mProtocolPacket);
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

        void OnSocketConnected(string ipInfo)
        {
            Interlocked.Increment(ref m_numConnectedSockets);
            Logger.Print("socket[{0}] connected ,conn = {1}", ipInfo, m_numConnectedSockets);
        }

        internal void OnSocketClosed(string ipInfo)
        {
            Interlocked.Decrement(ref m_numConnectedSockets);
            Logger.Print("socket[{0}] disconnected ,conn = {1}", ipInfo, m_numConnectedSockets);
        }

        public void ForEachWorking(Action<UserToken> action)
        {
            mTokenPool.ForEachWorking(action);
        }

        public void Send(string uid, Packet p)
        {
            var token = mTokenPool.Find(uid);
            if (token != null)
            {
                token.Send(p);
            }
        }

        public void BroadcastMessage(Packet p)
        {
            mTokenPool.ForEachWorking((u) => u.Send(p));
        }

        #region <<Deamon Thread>>

        const int kDeamonInterval = 100;

        readonly AutoResetEvent deamonSignal = new AutoResetEvent(false);

        private Thread mDeamonThread;

        void StartDeamonThread()
        {
            mDeamonThread = new Thread(DeamonProc);
            mDeamonThread.IsBackground = true;
            mDeamonThread.Start();
        }

        void DeamonProc()
        {
            while (mDeamonThread.IsAlive)
            {
                this.deamonSignal.WaitOne();
                var curTime = Environment.TickCount;
                mTokenPool.Update(curTime);
                Thread.Sleep(kDeamonInterval);
            }
        }

        public void WakeUpDeamon()
        {
            deamonSignal.Set();
        }

        void StopDeamonThread()
        {
            if (mDeamonThread == null)
            {
                return;
            }
            mDeamonThread.Abort();
            mDeamonThread.Join();
        }

        #endregion
    }
}