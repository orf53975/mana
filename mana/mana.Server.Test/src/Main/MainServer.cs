using mana.Foundation;
using System;
using System.Collections.Generic;
using System.Threading;

namespace mana.Server.Test
{
    class MainServer : IOCPServer
    {
        internal MainServer(int numConnections, int bufferSize, int tokenUnbindTimeOut = 10000, int tokenWorkTimeOut = 30000)
            : base(numConnections, bufferSize, tokenUnbindTimeOut, tokenWorkTimeOut)
        {
            StartUpdateThread();
        }
#if ___AAA
        #region <<battleGroups>>

        readonly Dictionary<string, BattleGroup> battleGroups = new Dictionary<string, BattleGroup>();

        private BattleGroup EnsureGetBattleGroup(string tokenId)
        {
            BattleGroup ret;
            if (!battleGroups.TryGetValue(tokenId, out ret))
            {
                ret = new BattleGroup(tokenId, this);
                battleGroups.Add(tokenId, ret);
            }
            return ret;
        }

        internal string TryAddBattle(string channelId, Battle battle)
        {
            return EnsureGetBattleGroup(channelId).TryAddBattle(battle);
        }

        public BattleGroup GetBattleGroup(string channelId)
        {
            BattleGroup ret;
            if (battleGroups.TryGetValue(channelId, out ret))
            {
                return ret;
            }
            return null;
        }

        private void UpdateBattleGroups(int deltaTime)
        {
            foreach (var pair in battleGroups)
            {
                pair.Value.Update(deltaTime);
            }
        }

        #endregion
#endif

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
