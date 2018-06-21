using System;
using System.Collections.Generic;

namespace mana.Foundation
{
    public enum ProtoType : byte
    {
        REQRSP = 0, PUSH = 1, NOTIFY = 2
    }

    public sealed class Proto
    {
        #region <<Static Functions>>

        public static Proto Decode(IReadableBuffer br)
        {
            var rt = br.ReadUTF8();
            var pt = (ProtoType)br.ReadByte();
            var ud = br.ReadUTF8();
            var dd = br.ReadUTF8();
            return new Proto(rt, pt, ud, dd);
        }

        #endregion

        /// <summary>
        /// 协议路由
        /// </summary>
        public readonly string route;

        /// <summary>
        /// 协议类型
        /// </summary>
        public readonly ProtoType type;

        /// <summary>
        /// 上行数据类型
        /// </summary>
        public readonly string uldt;

        /// <summary>
        /// 下行数据类型
        /// </summary>
        public readonly string dldt;

        public Proto(string route, ProtoType type, string uldt, string dldt)
        {
            this.route = route;
            this.type = type;
            this.uldt = uldt;
            this.dldt = dldt;
        }


        public void EncodeTo(IWritableBuffer bw)
        {
            bw.WriteUTF8(route);
            bw.WriteByte((byte)type);
            bw.WriteUTF8(uldt);
            bw.WriteUTF8(dldt);
        }


        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            var p = obj as Proto;
            if (obj == null)
            {
                return false;
            }
            return
                this.route == p.route &&
                this.type == p.type &&
                this.uldt == p.uldt &&
                this.dldt == p.dldt;
        }


        public override string ToString()
        {
            var sb = StringBuilderCache.Acquire();
            sb.Append("type = ").Append(type).Append(',');
            sb.Append("route = ").Append(route).Append(',');
            sb.Append("uldt = ").Append(uldt).Append(',');
            sb.Append("dldt = ").Append(dldt);
            return StringBuilderCache.GetStringAndRelease(sb);
        }

        public override int GetHashCode()
        {
            var hashCode = -1723101724;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(route);
            hashCode = hashCode * -1521134295 + type.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(uldt);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(dldt);
            return hashCode;
        }
    }
}