using mana.Foundation;
using mana.Foundation.Network.Sever;
using mana.Server.Game.BattleLink;
using System;
using System.Net;
using System.Threading;

namespace mana.Server.Game
{
    class GameServer : IOCPServer
    {
        public static GameServer StartNew(AppSetting setting)
        {
            var sev = new GameServer(setting);
            var ipa = string.IsNullOrEmpty(setting.host) ? IPAddress.Any : IPAddress.Parse(setting.host);
            sev.Start(new IPEndPoint(ipa, setting.port));
            sev.battleManager.StartConnect();
            return sev;
        }

        private readonly BSCMgr battleManager;

        private GameServer(AppSetting setting)
            : base(setting)
        {
            this.battleManager = new BSCMgr(this , setting.battleServers);
            this.StartUpdateThread();
        }

        internal void ForwardingToBattleServer(GamePlayer gamePlayer, Packet packet)
        {
            packet.SetMsgToken(gamePlayer.token);
            battleManager.Send(gamePlayer.bscId, packet);
        }

        internal void ForwardingToBattleServer(int bscId, Packet packet)
        {
            battleManager.Send(bscId, packet);
        }

        public override string GenUID(AccountInfo accountInfo)
        {
            return accountInfo.username + accountInfo.password;
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
                battleManager.Update(TimeUtil.GetTimeSpan(curTime, preTime));
                preTime = curTime;
                Thread.Sleep(kUpdateInterval);
            }
        }

        #endregion
    }
}