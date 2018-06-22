using System;
using System.Collections.Generic;

namespace mana.Foundation
{
    public sealed partial class Protocol : ISerializable , IFormatString
    {
        internal static readonly Protocol Instance = new Protocol();

        private Protocol()
        {
            AddProto(0x0001, new Proto("Connector.UpdateProtocol", ProtoType.PUSH, null, typeof(Protocol).Name));
            AddProto(0x0002, new Proto("Connector.Heartbeat", ProtoType.REQRSP, null, null));
        }

        #region <<protoMap>>

        readonly Dictionary<string, ushort> routeMap = new Dictionary<string, ushort>();

        readonly Dictionary<ushort, Proto>  protoMap = new Dictionary<ushort, Proto>();

        public ushort GetRouteCode(string routePath)
        {
            ushort ret;
            if (routeMap.TryGetValue(routePath, out ret))
            {
                return ret;
            }
            return 0;
        }

        public string GetRoutePath(ushort routeCode)
        {
            Proto ret;
            if (protoMap.TryGetValue(routeCode, out ret))
            {
                return ret.route;
            }
            return null;
        }

        public Proto GetProto(ushort routeCode)
        {
            Proto ret;
            if (protoMap.TryGetValue(routeCode, out ret))
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
                if (protoMap.TryGetValue(routeCode, out ret))
                {
                    return ret;
                }
            }
            return null;
        }

        private bool AddProto(ushort code, Proto proto)
        {
            Proto existed;
            if (protoMap.TryGetValue(code, out existed))
            {
                if (!existed.Equals(proto))
                {
                    Logger.Error("proto code[0x{0:X4}] conflict! {1}->{2}", code, proto.route, existed.route);
                    return false;
                }
                return true;
            }
            protoMap.Add(code, proto);
            routeMap.Add(proto.route, code);
            return true;
        }


        ushort protoCodeGenIndexer = 0x1001;
        private ushort GenProtoCode()
        {
            var ret = protoCodeGenIndexer++;
            while (protoMap.ContainsKey(ret))
            {
                ret++;
            }
            return ret;
        }

        public bool AddProtoAutoGenCode(string route, ProtoType type, Type uldt, Type dldt)
        {
            return this.AddProto(GenProtoCode(), new Proto(route, type, uldt.Name, dldt.Name));
        }

        #endregion

        #region <<dataTypeMap>>

        readonly Dictionary<ushort, string> code2dataType = new Dictionary<ushort, string>();

        readonly Dictionary<string, ushort> dataType2code = new Dictionary<string, ushort>();

        ushort typeCodeGenIndexer = 0xC001;
        private ushort GenDataTypeCode()
        {
            var ret = typeCodeGenIndexer++;
            while (code2dataType.ContainsKey(ret))
            {
                ret++;
            }
            return ret;
        }

        public string GetDataType(ushort typeCode)
        {
            string t;
            if (code2dataType.TryGetValue(typeCode, out t))
            {
                return t;
            }
            return null;
        }

        public ushort GetTypeCode(string dataType)
        {
            ushort ret;
            if (dataType2code.TryGetValue(dataType, out ret))
            {
                return ret;
            }
            return 0;
        }

        private bool AddDataType(ushort typeCode, string dataType)
        {
            string existed = null;
            if (code2dataType.TryGetValue(typeCode, out existed))
            {
                if (existed != dataType)
                {
                    Logger.Error("type code[0x{0:X4}] conflict! {1}->{2}", typeCode, dataType, existed);
                    return false;
                }
                return true;
            }
            code2dataType.Add(typeCode, dataType);
            dataType2code.Add(dataType, typeCode);
            return true;
        }

        public bool AddDataTypeByAutoGenCode<T>() where T : DataObject
        {
            var type = typeof(T);
            var typeName = type.FullName;

            var cfg = type.TryGetAttribute<DataObjectConfigAttribute>();
            if (cfg != null)
            {
                return AddDataType(cfg.TypeCode, typeName);
            }
            if (dataType2code.ContainsKey(typeName))
            {
                return true;
            }
            return AddDataType(GenDataTypeCode(), typeName);
        }

        #endregion

        #region <<implement ISerializable>>

        public void Encode(IWritableBuffer bw, bool bMaskAll)
        {
            // -- protos
            bw.WriteUnsignedShort(protoMap.Count);
            var iter1 = protoMap.GetEnumerator();
            while (iter1.MoveNext())
            {
                bw.WriteShort(iter1.Current.Key);
                iter1.Current.Value.EncodeTo(bw);
            }
            // -- types
            bw.WriteUnsignedShort(code2dataType.Count);
            var iter2 = code2dataType.GetEnumerator();
            while (iter2.MoveNext())
            {
                bw.WriteShort(iter2.Current.Key);
                bw.WriteUTF8(iter2.Current.Value);
            }
        }

        public void Encode(IWritableBuffer bw)
        {
            this.Encode(bw, false);
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
                this.AddDataType(br.ReadUnsignedShort(), br.ReadUTF8());
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
            foreach (var kv in protoMap)
            {
                sb.Append(indent1);
                sb.Append("0x").Append(kv.Key.ToString("X4")).Append(" : ").Append(kv.Value);
                sb.Append(",\r\n");
            }
            sb.Append(curIndent).Append("},\r\n");
            // -- types
            sb.Append(curIndent).Append("typeMap = {\r\n");
            foreach (var kv in code2dataType)
            {
                sb.Append(indent1);
                sb.Append("0x").Append(kv.Key.ToString("X4")).Append(" : ").Append(kv.Value);
                sb.Append(",\r\n");
            }
            sb.Append(curIndent).Append('}');

            sb.Append("\r\n");
            sb.Append(nlIndent).Append('}');
            return StringBuilderCache.GetAndRelease(sb);
        }
        #endregion
    }
}