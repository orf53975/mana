using mana.Foundation.Network.Sever;
using System;
using System.Net;
using System.Threading;

namespace mana.Server.Test
{
    class MainServer : IOCPServer
    {
        public static MainServer StartNew(SevSetting setting)
        {
            var sev = new MainServer(setting);
            var ipa = string.IsNullOrEmpty(setting.host) ? IPAddress.Any : IPAddress.Parse(setting.host);
            sev.Start(new IPEndPoint(ipa, setting.port));
            return sev;
        }

        private MainServer(SevSetting setting) : base(setting)
        {
            StartUpdateThread();
        }


        #region <<UpdateThread>>

        const int kUpdateInterval = 500;
        private Thread mUpdateThread;

        void StartUpdateThread()
        {
            mUpdateThread = new Thread(UpdateProc);
            mUpdateThread.IsBackground = true;
            mUpdateThread.Start();
        }

        void StopUpdateThread()
        {
            if (mUpdateThread == null)
            {
                return;
            }
            mUpdateThread.Abort();
            mUpdateThread.Join();
        }

        void UpdateProc()
        {
            var curTime = Environment.TickCount;
            var preTime = curTime;
            while (mUpdateThread.IsAlive)
            {
                curTime = Environment.TickCount;
                //TODO UpdateBattleGroups(Utils.GetTimeSpan(curTime, preTime));
                preTime = curTime;
                Thread.Sleep(kUpdateInterval);
            }
        }

        #endregion
    }
}
