using System;
using System.Collections.Generic;

namespace mana.Foundation
{
    public sealed class Protocol : DataObject
    {

        #region <<Single Instance>>

        public static readonly Protocol Instance = new Protocol(true);

        private Protocol(bool bInstance)
        {
            // -- defult protos
            AddProto(0x0001, new Proto("Connector.Protocol", ProtoType.Push, null, typeof(Protocol).FullName));
            AddProto(0x0002, new Proto("Connector.Ping", ProtoType.Notify, typeof(Heartbeat).FullName, null));
            AddProto(0x0003, new Proto("Connector.Pong", ProtoType.Push, null, typeof(Heartbeat).FullName));
            AddProto(0x0004, new Proto("Connector.BindToken", ProtoType.Request, typeof(AccountInfo).FullName, typeof(Result).FullName));
            AddProto(0x0005, new Proto("Connector.Kick", ProtoType.Push, null, typeof(Result).FullName));
            AddProto(0x0006, new Proto("Server.Status", ProtoType.Push, null, typeof(SevStatus).FullName));
            // -- default types
            AddTypeCode(typeof(Protocol).FullName, 0x0001);
            AddTypeCode(typeof(Heartbeat).FullName, 0x0003);
            AddTypeCode(typeof(Result).FullName, 0x0002);
            AddTypeCode(typeof(AccountInfo).FullName, 0x0004);
        }

        public static void UpdateInstace(Protocol other)
        {
            var sb = StringBuilderCache.Acquire(256);
            lock (Protocol.Instance)
            {
                sb.Append("Protocol Update");
                for (var iter = other.protosDict.GetEnumerator(); iter.MoveNext();)
                {
                    if (Instance.AddProto(iter.Current.Key, iter.Current.Value))
                    {
                        sb.AppendFormat("\n\tProto:0x{0:X} , {1}", iter.Current.Key, iter.Current.Value);
                    }
                }
                for (var iter = other.typesCode.GetEnumerator(); iter.MoveNext();)
                {
                    if (Instance.AddTypeCode(iter.Current.Key, iter.Current.Value))
                    {
                        sb.AppendFormat("\n\tTypeCode:0x{0:X}, {1}", iter.Current.Value, iter.Current.Key);
                    }
                }
            }
            Logger.Print(sb.ToString());
        }

        #endregion

        public Protocol() { }

        #region <<protosDic>>

        readonly Dictionary<ushort, Proto> protosDict = new Dictionary<ushort, Proto>();

        readonly Dictionary<string, ushort> routesCode = new Dictionary<string, ushort>();

        public string GetRoutePath(ushort routeCode)
        {
            Proto ret;
            if (protosDict.TryGetValue(routeCode, out ret))
            {
                return ret.route;
            }
            return null;
        }

        public Proto GetProto(ushort routeCode)
        {
            Proto ret;
            if (protosDict.TryGetValue(routeCode, out ret))
            {
                return ret;
            }
            return null;
        }


        public ushort GetRouteCode(string routePath)
        {
            ushort ret;
            if (routesCode.TryGetValue(routePath, out ret))
            {
                return ret;
            }
            return 0;
        }


        public Proto GetProto(string routePath)
        {
            var routeCode = GetRouteCode(routePath);
            return GetProto(routeCode);
        }

        internal bool AddProto(ushort code, Proto proto)
        {
            if (code == 0)
            {
                Logger.Error("proto code must not be zero!");
                return false;
            }
            // -- 1 check code conflict
            var existed = GetProto(code);
            if (existed != null)
            {
                if (!existed.Equals(proto))
                {
                    Logger.Error("proto code[0x{0:X4}] conflict! \n\t{1}\n\t{2}", code, proto, existed);
                }
                return false;
            }
            // -- 2 check route conflict
            existed = GetProto(proto.route);
            if (existed != null)
            {
                if (!existed.Equals(proto))
                {
                    Logger.Error("proto route conflict!,\n\t{0}\n\t{1}", proto, existed);
                }
                else
                {
                    Logger.Error("[{0}]duplicate add!, but code disagree!", proto.route);
                }
                return false;
            }
            protosDict.Add(code, proto);
            routesCode.Add(proto.route, code);
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
                }
                return false;
            }
            // -- 2 check typeCode
            var existTypeName = GetTypeName(typeCode);
            if (existTypeName != null)
            {
                if (existTypeName != dataType)
                {
                    Logger.Error("[{0},{1}] typeCode conflit!", dataType, existTypeName);
                }
                return false;
            }
            typesCode.Add(dataType, typeCode);
            return true;
        }

        #endregion

        #region <<implement ISerializable>>

        public void Encode(IWritableBuffer bw)
        {
            // -- protos
            bw.WriteUnsignedShort(protosDict.Count);
            for (var iter = protosDict.GetEnumerator(); iter.MoveNext();)
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
            foreach (var kv in protosDict)
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