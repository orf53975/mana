using BattleSystem;
using mana.Foundation;
using mana.Foundation.Network.Sever;
using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;
using xxd.battle;

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

        readonly ConcurrentDictionary<string, BattleScene> battles = new ConcurrentDictionary<string, BattleScene>();

        public BattleScene CreateBattle(BattleCreateInfo bcd, string creatorId)
        {
            var battle = new BattleScene(bcd, creatorId, this);
            if (!battles.TryAdd(battle.uid, battle))
            {
                Logger.Error("Battle uid[{0}] conflict!", battle.uid);
                return null;
            }
            this.BroadcastServerStatus();
            return battle;
        }

        public BattleScene GetBattle(string battleId)
        {
            BattleScene ret;
            if (battles.TryGetValue(battleId, out ret))
            {
                return ret;
            }
            return null;
        }

        private void DeleteBattle(string battleId)
        {
            BattleScene rmved;
            if (!battles.TryRemove(battleId, out rmved))
            {
                Logger.Error("Delete battle[{0}] failed!" , battleId);
            }
        }

        private void UpdateBattles(int deltaTime)
        {
            using (var rmvs = ListCache<string>.Get())
            {
                foreach (var kv in battles)
                {
                    try
                    {
                        var battle = kv.Value;
                        battle.Update(deltaTime);
                        if (battle.Destroyable)
                        {
                            rmvs.Add(kv.Key);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Exception(ex);
                    }
                }
                for (var i = rmvs.Count - 1; i >= 0; i--)
                {
                    DeleteBattle(rmvs[i]);
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
