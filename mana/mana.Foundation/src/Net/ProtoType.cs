namespace mana.Foundation
{
    public enum ProtoType : byte
    {
        /// <summary>
        /// 请求响应
        /// </summary>
        REQRSP = 0,

        /// <summary>
        /// 服务器推送
        /// </summary>
        PUSH = 1,

        /// <summary>
        /// 客户端通知
        /// </summary>
        NOTIFY = 2
    }
}