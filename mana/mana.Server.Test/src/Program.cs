﻿using mana.Foundation;
using mana.Foundation.Test;
using mana.Server.Test.Config;
using System;
using System.Diagnostics;
using System.Net;

namespace mana.Server.Test
{
    class Program : ConsoleProgram
    {
        static void Main(string[] args)
        {
            Instance.Start(args);
        }

        public static readonly Program Instance = new Program();

        public MainServer Server { get; private set; }

        void LoadPlugins(string[] plugins)
        {
            foreach (var s in plugins)
            {
                var fp = ConfigMgr.GetFullPath(s);
                var er = TypeUtil.LoadDll(AppDomain.CurrentDomain, fp);
                if (er == null)
                {
                    Trace.TraceInformation("dll loaded > " + s);
                }
                else
                {
                    Trace.TraceError(er);
                }
            }
        }

        protected override void OnStarted(params string[] args)
        {
            var setting = ConfigMgr.AppSetting;
            this.LoadPlugins(setting.plugins);

            Server = new MainServer(setting.connMax, setting.connBuffSize);
            var ipAddr = string.IsNullOrEmpty(setting.host) ? IPAddress.Any : IPAddress.Parse(setting.host);
            Server.Start(new IPEndPoint(ipAddr, setting.port));
        }

        protected override bool OnInputed(string cmd)
        {
            return base.OnInputed(cmd);
        }
    }
}
