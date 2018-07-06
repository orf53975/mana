namespace mana.Foundation.Network.Sever
{
    public interface IMessageHandler : ITypeInitializable
    {
        /// <summary>
        /// 请注意 packet对象函数体执行完毕以后会被回收 最好不要在外部持有packet对象 如果实在有必要 可以使用packet.Retain()持有 packet.release()释放 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="packet"></param>
        void Process(UserToken token, Packet packet);
    }
}
