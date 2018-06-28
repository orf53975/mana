using mana.Foundation;
using mana.Foundation.Network.Sever;
using System.Threading.Tasks;

namespace mana.Server.Test.Handler
{
    [MessageNotify("Connector.Ping", typeof(Heartbeat) , 1)]
    public sealed class OnPing : IMessageHandler
    {
        public void Process(UserToken token, Packet msg)
        {
            Logger.Warning(msg.TryGet<Heartbeat>().ToFormatString(""));

            //TODO test
            var p = Packet.CreatPush("Connector.Protocol", Protocol.Instance, false);
            token.Send(p);
        }

        public static void OnTaskSendPong(UserToken token)
        {
            Task.Factory.StartNew(() =>
            {
                token.SendPush<Heartbeat>("Connector.Pong", (hb) =>
                {
                    hb.timestamp = System.Environment.TickCount;
                });
            });
        }

        public static void OnThreadSendPong(UserToken token)
        {
            var t = new System.Threading.Thread(() => {
                token.SendPush<Heartbeat>("Connector.Pong", (hb) =>
                {
                    hb.timestamp = System.Environment.TickCount;
                });
            });
            t.Start();
        }

    }
}