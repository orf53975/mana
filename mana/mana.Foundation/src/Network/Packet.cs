using System;

namespace mana.Foundation
{
    public sealed class Packet
    {

        #region <<Definition MessageType>>
        public enum MessageType : byte
        {
            UNKNOW      = 0,
            REQUEST     = 1,
            RESPONSE    = 2,
            NOTIFY      = 3,
            PUSH        = 4
        }
        #endregion

        #region <<Definition Flag>>

        private enum Flag : byte
        {
            NONE            = 0,
            ENCODE_ROUTE    = 1 << 0,
            COMPRESS        = 1 << 1,
        }

        static bool CheckFlag(Flag value, Flag flag)
        {
            return (value & flag) == flag;
        }

        #endregion

        #region <<Definition ICompressor>>

        public interface ICompressor
        {

            int Encode(byte[] input, int inputOffset, int inputLength, byte[] output, int outputOffset, int outputLength);

            int Decode(byte[] input, int inputOffset, int inputLength, byte[] output, int outputOffset, int outputLength);
        }

        public static ICompressor compressor { get; private set; }

        #endregion

        #region <<Static Cache>>

        private static readonly ObjectPool<Packet> Cache = new ObjectPool<Packet>(
            () => new Packet(), null, (e) => e.Clear());

        #endregion

        #region <<Static Functions>>

        public static Packet CreatRequest(string msgRoute, int requestId, ISerializable msgObject)
        {
            var p = Packet.Cache.Get();
            p.msgType = MessageType.REQUEST;
            p.msgRequestId = requestId;
            p.msgRoute = msgRoute;
            if (msgObject != null)
            {
                msgObject.Encode(p.msgData);
            }
            return p;
        }

        public static Packet CreatResponse(string msgRoute, int requestId, ISerializable msgObject)
        {
            var p = Packet.Cache.Get();
            p.msgType = MessageType.RESPONSE;
            p.msgRequestId = requestId;
            p.msgRoute = msgRoute;
            if (msgObject != null)
            {
                msgObject.Encode(p.msgData);
            }
            return p;
        }

        public static Packet CreatNotify(string msgRoute, ISerializable msgObject)
        {
            var p = Packet.Cache.Get();
            p.msgType = MessageType.NOTIFY;
            p.msgRoute = msgRoute;
            if (msgObject != null)
            {
                msgObject.Encode(p.msgData);
            }
            return p;
        }

        public static Packet CreatPush(string msgRoute, ISerializable msgObject)
        {
            var p = Packet.Cache.Get();
            p.msgType = MessageType.PUSH;
            p.msgRoute = msgRoute;
            if (msgObject != null)
            {
                msgObject.Encode(p.msgData);
            }
            return p;
        }

        public static Packet TryDecode(ByteBuffer br)
        {
            if (br.Available < Packet.HEAD_SIZE)
            {
                return null;
            }
            var markPosition = br.Position;
            var packetHead = br.ReadByte();
            var packetSize = br.ReadInt24();
            if (br.Available < packetSize)
            {
                br.Seek(markPosition);
                return null;
            }
            var msgType = (MessageType)((packetHead >> 4) & 0xF);
            var msgFlag = (Flag)((packetHead) & 0xF);

            var endPositon = br.Position + packetSize;
            var p = Packet.Cache.Get();
            // -- 2. message type
            p.msgType = msgType;
            // -- 3. message route
            if (CheckFlag(msgFlag, Flag.ENCODE_ROUTE))
            {
                var routeCode = br.ReadUnsignedShort();
                p.msgRoute = Protocol.Instance.GetRoutePath(routeCode);
            }
            else
            {
                p.msgRoute = br.ReadUTF8();
            }
            // -- 4. message id
            if (msgType == MessageType.REQUEST ||
                msgType == MessageType.RESPONSE)
            {
                p.msgRequestId = br.ReadInt();
            }
            // -- 5. message body
            var msgDataLength = endPositon - br.Position;
            if (CheckFlag(msgFlag, Flag.COMPRESS))
            {
                //TODO 处理压缩数据
                throw new NotImplementedException();
                //TODO END
            }
            else
            {
                p.msgData.Write(msgDataLength, br, (data, offset, len, param) =>
                {
                    param.Read(data, offset, len);
                });
            }
            return p;
        }

        public static void Encode(Packet p, ByteBuffer bw)
        {
            var msgType = p.msgType;
            var msgFlag = Flag.NONE;
            var msgRoute = p.msgRoute;
            var msgData = p.msgData;

            ByteBuffer msgCompressData = null;


            var packetSize = 0;
            var msgRouteCode = Protocol.Instance.GetRouteCode(msgRoute);
            if (msgRouteCode != 0)
            {
                msgFlag |= Flag.ENCODE_ROUTE;
                packetSize += 2;
            }
            else
            {
                packetSize += CodingUtil.GetByteCountUTF8(msgRoute);
            }
            if (compressor != null && msgData.Length > 256)
            {
                msgFlag |= Flag.COMPRESS;

                msgCompressData = ByteBuffer.Pool.Get();

                //TODO 处理压缩数据
 

                //TODO END
                packetSize += msgCompressData.Length;
            }
            else
            {
                packetSize += msgData.Length;
            }
            // -- 
            if (msgType == MessageType.REQUEST || 
                msgType == MessageType.RESPONSE)
            {
                packetSize += 4;
            }

            var packetHead = (byte)(
                ((byte)msgType) |
                ((byte)msgFlag << 4)
                );

            // -- 
            bw.WriteByte(packetHead);
            bw.WriteInt24(packetSize);
            if (msgRouteCode != 0)
            {
                bw.WriteUnsignedShort(msgRouteCode);
            }
            else
            {
                bw.WriteUTF8(msgRoute);
            }
            if (msgType == MessageType.REQUEST || 
                msgType == MessageType.RESPONSE)
            {
                bw.WriteInt(p.msgRequestId);
            }
            if (msgCompressData != null)
            {
                bw.Write(msgCompressData.data, 0, msgCompressData.Length);
                ((IDisposable)msgCompressData).Dispose();
                msgCompressData = null;
            }
            else
            {
                bw.Write(msgData.data, 0, msgData.Length);
            }
        }

        #endregion

        public const int HEAD_SIZE = 4;

        public MessageType msgType
        {
            get;
            private set;
        }

        public string msgRoute
        {
            get;
            private set;
        }

        public int msgRequestId
        {
            get;
            private set;
        }

        readonly ByteBuffer msgData;

        private Packet()
        {
            msgData = new ByteBuffer(64);
        }

        public bool TryGet(ISerializable obj)
        {
            if (obj == null)
            {
                return false;
            }
            int savePos = msgData.Position;
            try
            {
                this.msgData.Seek(0);
                obj.Decode(msgData);
                return true;
            }
            catch (Exception e)
            {
                Logger.Exception(e);
                return false;
            }
            finally
            {
                this.msgData.Seek(savePos);
            }
        }

        public ISerializable TryGet(Type t)
        {
            var obj = ObjectCache.TryGet(t);
            if (obj == null)
            {
                obj = Activator.CreateInstance(t);
            }
            var ret = obj as ISerializable;
            if (ret == null)
            {
                Logger.Error("Type[] does not implement the interface", t.FullName);
                return null;
            }
            if (!this.TryGet(ret))
            {
                var cachedObj = ret as ICacheable;
                if (cachedObj != null)
                {
                    cachedObj.ReleaseToCache();
                }
                return null;
            }
            return ret;
        }

        public T TryGet<T>() 
            where T : class, ISerializable, new()
        {
            return TryGet(typeof(T)) as T;
        }

        public DDNode TryGetDataNode(string tmplName)
        {
            var ret = DDNode.Creat(tmplName);
            if (!this.TryGet(ret))
            {
                ret.ReleaseToCache();
                return null;
            }
            return ret;
        }

        public void Clear()
        {
            this.msgType = MessageType.UNKNOW;
            this.msgRoute = null;
            this.msgRequestId = 0;
            this.msgData.Clear();
        }
    }
}