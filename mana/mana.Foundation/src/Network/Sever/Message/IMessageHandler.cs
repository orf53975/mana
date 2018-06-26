namespace mana.Foundation.Network.Sever
{
    public interface IMessageHandler : ITypeInitializable
    {
        void Process(UserToken token, Packet msg);
    }
}
