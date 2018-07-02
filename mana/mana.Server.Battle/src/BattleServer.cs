using BattleSystem;
using mana.Foundation;
using mana.Foundation.Network.Sever;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using xxd.sync;

namespace mana.Server.Battle
{
    class BattleServer : IOCPServer
    {
        public static BattleServer StartNew(ServerSetting setting)
        {
            var sev = new BattleServer(setting);
            var ipa = string.IsNullOrEmpty(setting.host) ? IPAddress.Any : IPAddress.Parse(setting.host);
            sev.Start(new IPEndPoint(ipa, setting.port));
            return sev;
        }

        private BattleServer(ServerSetting setting) : base(setting)
        {
            StartUpdateThread();
        }

        readonly Dictionary<long, BattleScene> battles = new Dictionary<long, BattleScene>();

        public BattleScene CreateBattle(BattleCreateData bcd)
        {
            var battle = new BattleScene(bcd);
            lock (battles)
            {
                battles.Add(battle.UUID, battle);
            }
            return battle;
        }

        public BattleScene GetBattle(long battleId)
        {
            lock(battles)
            {
                BattleScene ret;
                if (battles.TryGetValue(battleId, out ret))
                {
                    return ret;
                }
            }
            return null;
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

        void UpdateBattles(int deltaTime)
        {
            lock (battles)
            {
                using (var rmvs = ListCache<long>.Get())
                {
                    for (var it = battles.GetEnumerator(); it.MoveNext();)
                    {
                        var battle = it.Current.Value;
                        battle.DoUpdate(deltaTime);
                        if (battle.Destroyable)
                        {
                            rmvs.Add(it.Current.Key);
                        }
                    }
                    for(var i = rmvs.Count - 1; i >= 0; i--)
                    {
                        battles.Remove(rmvs[i]);
                    }
                }
            }
        }

        void UpdateProc()
        {
            var curTime = Environment.TickCount;
            var preTime = curTime;
            while (mUpdateThread.IsAlive)
            {
                curTime = Environment.TickCount;
                UpdateBattles(TimeUtil.GetTimeSpan(curTime, preTime));
                preTime = curTime;
                Thread.Sleep(kUpdateInterval);
            }
        }

        #endregion

    }
}
