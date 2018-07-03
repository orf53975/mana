namespace mana.Foundation
{
    public enum ProtoType : int
    {
        Unknow = -1,

        /// <summary>
        /// 请求响应
        /// </summary>
        Request = 0,

        /// <summary>
        /// 服务器推送
        /// </summary>
        Push = 1,

        /// <summary>
        /// 客户端通知
        /// </summary>
        Notify = 2
    }
}