using BattleSystem;
using mana.Foundation;
using mana.Foundation.Network.Sever;
using System;
using System.Collections.Generic;
using System.Threading;

namespace mana.Server.Battle
{
    class BattleServer : IOCPServer
    {
        internal BattleServer(int numConnections, int bufferSize, int tokenUnbindTimeOut = 10000, int tokenWorkTimeOut = 30000)
             : base(numConnections, bufferSize, tokenUnbindTimeOut, tokenWorkTimeOut)
        {
            StartUpdateThread();
        }

        readonly Dictionary<string, BattleScene> battles = new Dictionary<string, BattleScene>();

        public BattleScene CreateBattle(Packet packet)
        {
            //var battle = new BattleScene(bcd);
            //lock(battles)
            //{
            //    battles.Add(battle.uid , battle);
            //}
            //return battle;
            throw new NotImplementedException();
        }

        public BattleScene GetBattle(string battleId)
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
                using (var rmvs = ListCache<string>.Get())
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
