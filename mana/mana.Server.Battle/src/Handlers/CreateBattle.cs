using mana.Foundation;
using mana.Foundation.Network.Sever;

namespace mana.Server.Battle.src.Handlers
{
    [MessageRequest("Battle.CreateBattle", typeof(CreateBattle) , typeof(CreateBattle))]
    class CreateBattle : IMessageHandler
    {
        public void Process(UserToken token, Packet msg)
        {
            //var sev = token.GetServer<BattleServer>();
            //sev.CreateBattle(msg);
            //token.SendResponse<>("Battle.Create" , );//TODO

        }
    }
}
