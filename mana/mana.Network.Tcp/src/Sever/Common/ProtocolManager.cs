using System;

namespace mana.Foundation.Network.Sever
{
    public static class ProtocolManager
    {
        public static void InitCodeGenerator(ushort protoCodeGen, ushort typesCodeGen)
        {
            protoCodeGenIndexer = protoCodeGen != 0 ? protoCodeGen : protoCodeGenIndexer;
            typesCodeGenIndexer = typesCodeGen != 0 ? typesCodeGen : typesCodeGenIndexer;
        }

        public static bool AddProto(string route, ProtoType type, Type uldt, Type dldt)
        {
            var proto = new Proto(route, type, uldt != null ? uldt.FullName : null, dldt != null ? dldt.FullName : null);
            var exist = Protocol.Instance.GetProto(proto.route);
            if (exist == null)
            {
                return Protocol.Instance.AddProto(GenProtoCode(route), proto);
            }
            if (proto.Equals(exist))
            {
                return true;
            }
            Logger.Error("proto route conflict! {0}->{1}", proto, exist);
            return false;
        }

        public static bool AddProtoRequest<TREQ, TRSP>(string route)
            where TREQ : class, ISerializable
            where TRSP : class, ISerializable
        {
            return AddProto(route, ProtoType.Request, typeof(TREQ), typeof(TRSP));
        }

        public static bool AddProtoNotify<T>(string route)
            where T : class, ISerializable
        {
            return AddProto(route, ProtoType.Notify, typeof(T), null);
        }

        public static bool AddProtoPush<T>(string route)
            where T : class, ISerializable
        {
            return AddProto(route, ProtoType.Push, null, typeof(T));
        }

        static ushort protoCodeGenIndexer = 0x1001;
        static ushort GenProtoCode(string route)
        {
            var ret = protoCodeGenIndexer++;
            while (Protocol.Instance.GetProto(ret) != null)
            {
                ret++;
            }
            return ret;
        }

        public static bool AddTypeCode(Type type)
        {
            var typeName = type.FullName;
            var cfg = type.TryGetAttribute<DataObjectConfigAttribute>();
            if (cfg != null)
            {
                return Protocol.Instance.AddTypeCode(typeName, cfg.TypeCode);
            }
            var existed = Protocol.Instance.GetTypeCode(typeName);
            if (existed != 0)
            {
                return true;
            }
            var typeCode = GenDataTypeCode(typeName);
            return Protocol.Instance.AddTypeCode(typeName, typeCode);
        }
         
        public static bool AddTypeCode<T>() where T : DataObject
        {
            return AddTypeCode(typeof(T));
        }

        static ushort typesCodeGenIndexer = 0x0001;
        static ushort GenDataTypeCode(string typeName)
        {
            var ret = typesCodeGenIndexer++;
            while (Protocol.Instance.GetTypeName(ret) != null)
            {
                ret++;
            }
            return ret;
        }
    }
}
