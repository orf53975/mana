using System;
using System.Collections.Generic;

namespace mana.Foundation
{
    public sealed partial class Protocol : DataObject
    {

        #region <<Single Instance>>

        public static readonly Protocol Instance = new Protocol(true);

        private Protocol(bool bInstance)
        {
            // -- defult protos
            AddProto(0x0001, new Proto("Connector.Protocol", ProtoType.Push, null, typeof(Protocol).FullName));
            AddProto(0x0002, new Proto("Connector.Ping", ProtoType.Notify, typeof(Heartbeat).FullName, null));
            AddProto(0x0003, new Proto("Connector.Pong", ProtoType.Push, null, typeof(Heartbeat).FullName));

            // -- default types
            AddTypeCode(typeof(Protocol).FullName, 0x0001);
            AddTypeCode(typeof(Response).FullName, 0x0002);
            AddTypeCode(typeof(Heartbeat).FullName, 0x0003);
        }

        #endregion

        public Protocol() { }

        #region <<protosDic>>

        readonly Dictionary<ushort, Proto> protosDic = new Dictionary<ushort, Proto>();

        public ushort GetRouteCode(string routePath)
        {
            for (var it = protosDic.GetEnumerator(); it.MoveNext();)
            {
                if (it.Current.Value.route == routePath)
                {
                    return it.Current.Key;
                }
            }
            return 0;
        }

        public string GetRoutePath(ushort routeCode)
        {
            Proto ret;
            if (protosDic.TryGetValue(routeCode, out ret))
            {
                return ret.route;
            }
            return null;
        }

        public Proto GetProto(ushort routeCode)
        {
            Proto ret;
            if (protosDic.TryGetValue(routeCode, out ret))
            {
                return ret;
            }
            return null;
        }

        public Proto GetProto(string routePath)
        {
            var routeCode = this.GetRouteCode(routePath);
            if (routeCode != 0)
            {
                Proto ret;
                if (protosDic.TryGetValue(routeCode, out ret))
                {
                    return ret;
                }
            }
            return null;
        }

        internal bool AddProto(ushort code, Proto proto)
        {
            if (code == 0)
            {
                Logger.Error("proto code must not be zero!");
                return false;
            }
            Proto existed;
            if (protosDic.TryGetValue(code, out existed))
            {
                if (!existed.Equals(proto))
                {
                    Logger.Error("proto code[0x{0:X4}] conflict! {1}->{2}", code, proto.route, existed.route);
                    return false;
                }
                return true;
            }
            protosDic.Add(code, proto);
            return true;
        }

        #endregion

        #region <<typesCode>>

        readonly Dictionary<string, ushort> typesCode = new Dictionary<string, ushort>();

        public string GetTypeName(ushort typeCode)
        {
            for (var it = typesCode.GetEnumerator(); it.MoveNext();)
            {
                if (it.Current.Value == typeCode)
                {
                    return it.Current.Key;
                }
            }
            return null;
        }

        public ushort GetTypeCode(string dataType)
        {
            ushort c;
            if (typesCode.TryGetValue(dataType, out c))
            {
                return c;
            }
            return 0;
        }

        internal bool AddTypeCode(string dataType, ushort typeCode)
        {
            if (typeCode == 0)
            {
                Logger.Error("typeCode must not be zero!");
                return false;
            }
            // -- 1 check dataType
            var existTypeCode = GetTypeCode(dataType);
            if (existTypeCode != 0)
            {
                if (existTypeCode != typeCode)
                {
                    Logger.Error("[{0}]duplicate add![{1},{2}]", dataType, typeCode, existTypeCode);
                    return false;
                }
                return true;
            }
            // -- 2 check typeCode
            var existTypeName = GetTypeName(typeCode);
            if (existTypeName != null)
            {
                if(existTypeName != dataType)
                {
                    Logger.Error("[{0},{1}] typeCode conflit!", dataType, existTypeName);
                    return false;
                }
            }

            typesCode.Add(dataType, typeCode);
            return true;
        }

        #endregion

        #region <<Push Protocol>>
        public void Push(Protocol other)
        {
            for (var iter = other.protosDic.GetEnumerator(); iter.MoveNext();)
            {
                this.AddProto(iter.Current.Key, iter.Current.Value);
            }
            for (var iter = other.typesCode.GetEnumerator(); iter.MoveNext();)
            {
                this.AddTypeCode(iter.Current.Key, iter.Current.Value);
            }
        }
        #endregion

        #region <<implement ISerializable>>

        public void Encode(IWritableBuffer bw)
        {
            // -- protos
            bw.WriteUnsignedShort(protosDic.Count);
            for (var iter = protosDic.GetEnumerator(); iter.MoveNext();)
            {
                bw.WriteShort(iter.Current.Key);
                iter.Current.Value.EncodeTo(bw);
            }
            // -- types
            bw.WriteUnsignedShort(typesCode.Count);
            for (var iter = typesCode.GetEnumerator(); iter.MoveNext();)
            {
                bw.WriteUTF8(iter.Current.Key);
                bw.WriteShort(iter.Current.Value);
            }
        }

        public void Decode(IReadableBuffer br)
        {
            // -- protos
            var count = br.ReadUnsignedShort();
            for (int i = 0; i < count; i++)
            {
                this.AddProto(br.ReadUnsignedShort(), Proto.Decode(br));
            }
            // -- types
            count = br.ReadUnsignedShort();
            for (int i = 0; i < count; i++)
            {
                this.AddTypeCode(br.ReadUTF8(), br.ReadUnsignedShort());
            }
        }

        #endregion

        #region <<implement IFormatString>>
        public string ToFormatString(string nlIndent)
        {
            var sb = StringBuilderCache.Acquire();
            sb.Append("{\r\n");
            var curIndent = nlIndent + '\t';
            sb.Append(curIndent).Append("tmpl = \"Protocol\",\r\n");

            var indent1 = curIndent + '\t';
            // -- protos
            sb.Append(curIndent).Append("protoMap = {\r\n");
            foreach (var kv in protosDic)
            {
                sb.Append(indent1);
                sb.Append("0x").Append(kv.Key.ToString("X4")).Append(" : ").Append(kv.Value);
                sb.Append(",\r\n");
            }
            sb.Append(curIndent).Append("},\r\n");
            // -- types
            sb.Append(curIndent).Append("typeMap = {\r\n");
            foreach (var kv in typesCode)
            {
                sb.Append(indent1);
                sb.Append("0x").Append(kv.Value.ToString("X4")).Append(" : ").Append(kv.Key);
                sb.Append(",\r\n");
            }
            sb.Append(curIndent).Append('}');

            sb.Append("\r\n");
            sb.Append(nlIndent).Append('}');
            return StringBuilderCache.GetAndRelease(sb);
        }

        #endregion

        #region <<implement ICacheable>>

        public void ReleaseToCache()
        {
            ObjectCache.Put(this);
        }

        #endregion

    }
}