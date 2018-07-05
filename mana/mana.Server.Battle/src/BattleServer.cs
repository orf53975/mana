using BattleSystem;
using mana.Foundation;
using mana.Foundation.Network.Sever;
using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;
using xxd.sync;

namespace mana.Server.Battle
{
    class BattleServer : IOCPServer, BattleScene.IPlayerMessagePusher
    {
        public static BattleServer StartNew(CustomSevSetting setting)
        {
            ProtocolManager.InitCodeGenerator(0xB001, 0xB001);
            var sev = new BattleServer(setting);
            var ipa = string.IsNullOrEmpty(setting.host) ? IPAddress.Any : IPAddress.Parse(setting.host);
            sev.Start(new IPEndPoint(ipa, setting.port));
            return sev;
        }

        public readonly int maxBattleNum;

        private BattleServer(CustomSevSetting setting) : base(setting)
        {
            maxBattleNum = Math.Max(8, setting.battleMaxNum);
            StartUpdateThread();
        }


        public void BroadcastServerStatus()
        {
            var battleNum = battles.Count;
            var p = Packet.CreatPush<SevStatus>("Server.Status", (ss) =>
            {
                ss.balance = (float)battleNum / maxBattleNum;
            });
            this.BroadcastMessage(p);
        }

        #region <<about battles>>

        readonly ConcurrentDictionary<long, BattleScene> battles = new ConcurrentDictionary<long, BattleScene>();

        public BattleScene CreateBattle(BattleCreateData bcd)
        {
            var battle = new BattleScene(bcd, this);
            if (!battles.TryAdd(battle.UUID, battle))
            {
                Logger.Error("Battle UUID[{0}] conflict!", battle.UUID);
                return null;
            }
            this.BroadcastServerStatus();
            return battle;
        }

        public BattleScene GetBattle(long battleId)
        {
            BattleScene ret;
            if (battles.TryGetValue(battleId, out ret))
            {
                return ret;
            }
            return null;
        }

        private void UpdateBattles(int deltaTime)
        {
            using (var rmvs = ListCache<long>.Get())
            {
                foreach(var kv in battles)
                {
                    try
                    {
                        kv.Value.Update(deltaTime);
                        if (kv.Value.Destroyable)
                        {
                            rmvs.Add(kv.Key);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Exception(ex);
                    }
                }
                BattleScene rmved;
                for (var i = rmvs.Count - 1; i >= 0; i--)
                {
                    if(!battles.TryRemove(rmvs[i], out rmved))
                    {

                    }
                }
            }
        }

        [Obsolete]
        void ForwardingToBattleScene(string channel, string playerId, Packet packet)
        {
            throw new NotImplementedException();
        }

        public BattleScene FindBattle(string channel, string playerId)
        {
            throw new NotImplementedException();
        }


        #endregion

        #region <<Implement BattleScene.IPlayerMessagePusher>>

        public void Push(string channelToken, Packet p)
        {
            this.Send(channelToken, p);
        }

        #endregion

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
                UpdateBattles(TimeUtil.GetTimeSpan(curTime, preTime));
                preTime = curTime;
                Thread.Sleep(kUpdateInterval);
            }
        }
        #endregion
    }
}
