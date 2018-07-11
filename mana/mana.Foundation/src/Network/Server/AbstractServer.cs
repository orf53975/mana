using System;
using System.Net;
using System.Threading;

namespace mana.Foundation.Network.Server
{
    public abstract class AbstractServer
    {
        protected readonly UserTokenPool mTokenPool;

        internal readonly int bindTimeOut;
        internal readonly int pingTimeOut;

        private int m_numConnectedSockets;
        public int ConnectionNum
        {
            get { return m_numConnectedSockets; }
        }

        private Packet mProtocolPacket;
        public Packet ProtocolPacket
        {
            get
            {
                if (mProtocolPacket == null)
                {
                    mProtocolPacket = Packet.CreatPush("Connector.Protocol", Protocol.Instance, false);
                }
                return mProtocolPacket;
            }
        }

        public AbstractServer(int numConnections, int bufferSize, int bindTimeOut = 2000 * 10, int pingTimeOut = 3000 * 10)
        {
            this.mTokenPool = new UserTokenPool(numConnections, bufferSize, CreateNewToken);
            this.m_numConnectedSockets = 0;
            this.bindTimeOut = bindTimeOut;
            this.pingTimeOut = pingTimeOut;
            this.InitialTypes();
            Packet.ChangePoolCapacity(8192);
        }

        public AbstractServer(ServerSetting setting) :
            this(setting.connMax, setting.connBuffSize, setting.bindTimeOut, setting.pingTimeOut)
        {
            LoadPlugins(setting.plugins);
        }

        private void LoadPlugins(string[] plugins)
        {
            foreach (var fp in plugins)
            {
                var er = TypeUtil.LoadDll(AppDomain.CurrentDomain, fp);
                if (er == null)
                {
                    Logger.Print("dll loaded > {0}", fp);
                }
                else
                {
                    Logger.Error(er);
                }
            }
        }

        private void InitialTypes()
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            var types = AppDomain.CurrentDomain.GetClassTypes<ITypeInitializable>();
            foreach (var type in types)
            {
                if (typeof(IDataTypeRegister).IsAssignableFrom(type))
                {
                    (Activator.CreateInstance(type) as IDataTypeRegister).RegistDataType();
                }
                if (typeof(IMessageHandler).IsAssignableFrom(type))
                {
                    MessageDispatcher.Instance.RegistHandler(type);
                }
                if (typeof(IPushProtoRegister).IsAssignableFrom(type))
                {
                    (Activator.CreateInstance(type) as IPushProtoRegister).RegistPushProto();
                }
            }
            Logger.Print("InitialTypes:{0}", sw.ElapsedMilliseconds);
            Logger.Print(Protocol.Instance.ToFormatString(""));
        }

        public abstract UserToken CreateNewToken();


        /// <summary>
        /// Starts the server such that it is listening for incoming connection requests.  
        /// </summary>
        /// <param name="localEndPoint">The endpoint which the server will listening for connection requests on</param>
        public abstract void Start(IPEndPoint localEndPoint);

        protected void OnSocketConnected(string ipInfo)
        {
            Interlocked.Increment(ref m_numConnectedSockets);
            Logger.Print("socket[{0}] connected ,conn = {1}", ipInfo, m_numConnectedSockets);
        }

        public void OnSocketDisconnected(string ipInfo)
        {
            Interlocked.Decrement(ref m_numConnectedSockets);
            Logger.Print("socket[{0}] disconnected ,conn = {1}", ipInfo, m_numConnectedSockets);
        }

        public void ForEachWorking(Action<UserToken> action)
        {
            mTokenPool.ForEachWorking(action);
        }

        public void TryKick(string uid)
        {
            var token = mTokenPool.Find(uid);
            if (token != null)
            {
                token.SendPush<Result>("Connector.Kick", (ret) =>
                {
                    ret.code = Result.Code.unknow;
                    ret.info = "----------------";
                });
                Thread.Sleep(200);
                token.Release();
            }
        }

        public void Send(string uid, Packet p)
        {
            var token = mTokenPool.Find(uid);
            if (token != null)
            {
                token.Send(p);
            }
        }

        public virtual string GenUID(AccountInfo accountInfo)
        {
            return Guid.NewGuid().ToString();
        }

        public void BroadcastMessage(Packet p)
        {
            mTokenPool.ForEachWorking((u) => u.Send(p));
        }

        #region <<Deamon Thread>>

        const int kDeamonInterval = 2000;

        readonly AutoResetEvent deamonSignal = new AutoResetEvent(false);

        private Thread mDeamonThread;

        protected void StartDeamonThread()
        {
            mDeamonThread = new Thread(DeamonProc);
            mDeamonThread.IsBackground = true;
            mDeamonThread.Start();
        }

        void DeamonProc()
        {
            while (mDeamonThread.IsAlive)
            {
                deamonSignal.WaitOne(kDeamonInterval);
                var curTime = Environment.TickCount;
                mTokenPool.Update(curTime);
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