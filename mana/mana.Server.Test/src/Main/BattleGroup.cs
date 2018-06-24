#if ___AAA

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace mana.Server.Test
{
    internal class BattleGroup
    {
        readonly List<Battle> battles = new List<Battle>();

        internal readonly string tokenId;

        internal readonly MainServer server;

        internal BattleGroup(string tokenId, MainServer server)
        {
            this.tokenId = tokenId;
            this.server = server;
        }

        internal string TryAddBattle(Battle battle)
        {
            var battleId = battle.UUID;
            var index = xUtil.Util.BinarySearch(battles, (e) => e.UUID - battleId);
            if (index >= 0)
            {
                return string.Format("Battle UUID[{0}] conflict", battleId);
            }
            else
            {
                battles.Insert(~index, battle);
                return null;
            }
        }

        internal Battle FindBattle(long battleId)
        {
            var findIndex = Utils.BinarySearch(battles, (e) => e.UUID - battleId);
            if (findIndex >= 0)
            {
                return battles[findIndex];
            }
            return null;
        }

        internal void Update(int deltaTime)
        {
            Battle battle;
            for (int i = battles.Count - 1; i >= 0; i--)
            {
                battle = battles[i];
                try
                {
                    var ret = battle.Update(deltaTime);
                    if (ret != null)
                    {
                        server.Send(tokenId, Packing.build(ret));
                    }
                    if (battle.isFinished)
                    {
                        battles.RemoveAt(i);
                    }
                }
                catch (Exception e)
                {
                    Trace.TraceError(e.ToString());
                }
            }
        }
    }
}
#endif