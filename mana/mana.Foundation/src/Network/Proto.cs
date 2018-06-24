﻿using System;
using System.Collections.Generic;

namespace mana.Foundation
{
    public sealed class Proto
    {
        #region <<Static Functions>>

        public static Proto Decode(IReadableBuffer br)
        {
            var rt = br.ReadUTF8();
            var pt = (ProtoType)br.ReadByte();
            string c2s, s2c;
            if (pt == ProtoType.REQRSP)
            {
                c2s = br.ReadUTF8();
                s2c = br.ReadUTF8();
            }
            else if (pt == ProtoType.NOTIFY)
            {
                c2s = br.ReadUTF8();
                s2c = null;
            }
            else if (pt == ProtoType.PUSH)
            {
                c2s = null;
                s2c = br.ReadUTF8();
            }
            else
            {
                c2s = null;
                s2c = null;
            }
            return new Proto(rt, pt, c2s, s2c);
        }

        #endregion
 
        public readonly string route;
        public readonly ProtoType ptype;
        public readonly string c2sdt;
        public readonly string s2cdt;

        public Proto(string rout, ProtoType p_t, string c2s, string s2c)
        {
            this.route = rout;
            this.ptype = p_t;
            this.c2sdt = c2s;
            this.s2cdt = s2c;
        }

        public void EncodeTo(IWritableBuffer bw)
        {
            bw.WriteUTF8(route);
            bw.WriteByte((byte)ptype);
            if (ptype == ProtoType.REQRSP)
            {
                bw.WriteUTF8(c2sdt);
                bw.WriteUTF8(s2cdt);
            }
            else if (ptype == ProtoType.NOTIFY)
            {
                bw.WriteUTF8(c2sdt);
            }
            else if (ptype == ProtoType.PUSH)
            {
                bw.WriteUTF8(s2cdt);
            }
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
                this.ptype == p.ptype &&
                this.c2sdt == p.c2sdt &&
                this.s2cdt == p.s2cdt;
        }


        public override string ToString()
        {
            var sb = StringBuilderCache.Acquire();
            sb.Append("ptype = ").Append(ptype).Append(',');
            sb.Append("route = ").Append(route).Append(',');
            sb.Append("c2sdt = ").Append(c2sdt).Append(',');
            sb.Append("s2cdt = ").Append(s2cdt);
            return StringBuilderCache.GetAndRelease(sb);
        }


        public override int GetHashCode()
        {
            var hashCode = -1723101724;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(route);
            hashCode = hashCode * -1521134295 + ptype.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(c2sdt);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(s2cdt);
            return hashCode;
        }
    }
}