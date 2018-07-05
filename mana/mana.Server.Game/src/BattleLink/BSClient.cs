﻿using mana.Foundation;
using mana.Foundation.Network.Client;
using System;
using System.Net;

namespace mana.Server.Game.BattleLink
{
    class BSClient
    {
        readonly IPEndPoint address;

        readonly NetClient channel;

        readonly BSCMgr manager;

        /// <summary>
        /// 战斗服的负载状态
        /// </summary>
        public float BalanceStatus
        {
            get;
            private set;
        }

        public BSClient(BSCMgr mgr, string addr)
        {
            this.manager = mgr;
            this.channel = new NetClientIOCP(5000, 2048, 1024, true);
            this.address = Utils.GetIPEndPoint(addr);
            this.InitPacketListener();
        }

        internal void StartConnect()
        {
            if (channel.Connected)
            {
                return;
            }
            channel.Connect(address, (sucess) =>
            {
                if (sucess)
                {
                    BindToken();
                }
                else
                {
                    StartConnect();
                }
            });
        }

        void InitPacketListener()
        {
            channel.AddPushListener<SevStatus>("Server.Status", (ss) =>
            {
                this.BalanceStatus = ss.balance;
                Logger.Error("BalanceStatus:" + BalanceStatus);
            });
            channel.AddPacketListener("Battle.Sync", (p) =>
            {
                var playerUID = p.msgAttach;
                p.SetAttach(null);
                manager.server.Send(playerUID, p);
            });
        }

        void BindToken()
        {
            channel.Request<AccountInfo, Result>("Connector.BindToken",
                (acc) =>
                {
                    acc.username = Config.ServerSetting.name;
                    acc.password = "12345678";
                },
                (res) =>
                {
                    Logger.Print("BindToken:{0}", res.code);
                });
        }



        internal void SendPacket(Packet msg)
        {
            channel.SendPacket(msg);
        }

        internal void Update(int deltaTime)
        {
            channel.Update(deltaTime);
        }

    }
}
