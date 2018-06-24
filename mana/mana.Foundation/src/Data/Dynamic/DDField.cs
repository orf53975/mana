using System;

namespace mana.Foundation
{
    public sealed class DDField : ISerializable, ICacheable, IFormatString
    {
        const string ERR_FMT_SET_VALUE_FAILED = "SetValue failed! {0} Do not accept a {1}!";

        internal void InitTmpl(DDFieldTmpl tmpl)
        {
            this.Tmpl = tmpl;
        }

        public DDFieldTmpl Tmpl
        {
            get;
            internal set;
        }

        public bool maskBit;

        public Int32 int32Value
        {
            get;
            private set;
        }

        public Int64 int64Value
        {
            get;
            private set;
        }

        public float floatValue
        {
            get;
            private set;
        }

        public string strValue
        {
            get;
            private set;
        }

        public DDNode objValue
        {
            get;
            private set;
        }

        public Array arrValue
        {
            get;
            private set;
        }


 
        #region <<SetValue>>

        public bool SetValue(bool v)
        {
            if (Tmpl.token != DDToken.ft_bool)
            {
                Logger.Error(ERR_FMT_SET_VALUE_FAILED, Tmpl, v.GetType());
                return false;
            }

            int32Value = v ? 1 : 0;
            maskBit = true;
            return true;
        }

        public bool SetValue(int v)
        {
            if (Tmpl.token != DDToken.ft_byte &&
                Tmpl.token != DDToken.ft_int16 &&
                Tmpl.token != DDToken.ft_int32)
            {
                Logger.Error(ERR_FMT_SET_VALUE_FAILED, Tmpl, v.GetType());
                return false;
            }
            int32Value = v;
            maskBit = true;
            return true;
        }

        public bool SetValue(long v)
        {
            if (Tmpl.token != DDToken.ft_int64 &&
                Tmpl.token != DDToken.ft_intX &&
                Tmpl.token != DDToken.ft_intXU)
            {
                Logger.Error(ERR_FMT_SET_VALUE_FAILED, Tmpl, v.GetType());
                return false;
            }
            int64Value = v;
            maskBit = true;
            return true;
        }

        public bool SetValue(float v)
        {
            if (Tmpl.token != DDToken.ft_float &&
                Tmpl.token != DDToken.ft_float16)
            {
                Logger.Error(ERR_FMT_SET_VALUE_FAILED, Tmpl, v.GetType());
                return false;
            }
            floatValue = v;
            maskBit = true;
            return true;
        }

        public bool SetValue(string v)
        {
            if (Tmpl.token != DDToken.ft_str)
            {
                Logger.Error(ERR_FMT_SET_VALUE_FAILED, Tmpl, v.GetType());
                return false;
            }
            strValue = v;
            maskBit = true;
            return true;
        }

        private bool MatchTmplType(DDNode v)
        {
            if (Tmpl.token == DDToken.ft_object)
            {
                return v == null || Tmpl.isUnknowType || Tmpl.objTmpl == v.Tmpl.fullName;
            }
            return false;
        }

        public bool SetValue(DDNode v)
        {
            if (!MatchTmplType(v))
            {
                Logger.Error(ERR_FMT_SET_VALUE_FAILED, Tmpl, v.Tmpl.fullName);
                return false;
            }
            objValue = v;
            maskBit = true;
            return true;
        }


        public bool SetValue(bool[] v)
        {
            if (!Tmpl.isArray && Tmpl.token != DDToken.ft_bool)
            {
                Logger.Error(ERR_FMT_SET_VALUE_FAILED, Tmpl, v.GetType());
                return false;
            }
            arrValue = v;
            maskBit = true;
            return true;
        }


        public bool SetValue(byte[] v)
        {
            if (!Tmpl.isArray && Tmpl.token != DDToken.ft_byte)
            {
                Logger.Error(ERR_FMT_SET_VALUE_FAILED, Tmpl, v.GetType());
                return false;
            }
            arrValue = v;
            maskBit = true;
            return true;
        }

        public bool SetValue(short[] v)
        {
            if (!Tmpl.isArray && Tmpl.token != DDToken.ft_int16)
            {
                Logger.Error(ERR_FMT_SET_VALUE_FAILED, Tmpl, v.GetType());
                return false;
            }
            arrValue = v;
            maskBit = true;
            return true;
        }

        public bool SetValue(int[] v)
        {
            if (!Tmpl.isArray && Tmpl.token != DDToken.ft_int32)
            {
                Logger.Error(ERR_FMT_SET_VALUE_FAILED, Tmpl, v.GetType());
                return false;
            }
            arrValue = v;
            maskBit = true;
            return true;
        }

        public bool SetValue(long[] v)
        {
            if (!Tmpl.isArray || (
                Tmpl.token != DDToken.ft_int64 &&
                Tmpl.token != DDToken.ft_intX &&
                Tmpl.token != DDToken.ft_intXU))
            {
                Logger.Error(ERR_FMT_SET_VALUE_FAILED, Tmpl, v.GetType());
                return false;
            }
            arrValue = v;
            maskBit = true;
            return true;
        }

        public bool SetValue(string[] v)
        {
            if (!Tmpl.isArray || Tmpl.token != DDToken.ft_str)
            {
                Logger.Error(ERR_FMT_SET_VALUE_FAILED, Tmpl, v.GetType());
                return false;
            }
            arrValue = v;
            maskBit = true;
            return true;
        }

        public bool SetValue(DDNode[] v)
        {
            if (!Tmpl.isArray || Tmpl.token != DDToken.ft_object)
            {
                Logger.Error(ERR_FMT_SET_VALUE_FAILED, Tmpl, v.GetType());
                return false;
            }
            if (v != null)
            {
                for (var i = v.Length - 1; i >= 0; i--)
                {
                    if (!MatchTmplType(v[i]))
                    {
                        Logger.Error(ERR_FMT_SET_VALUE_FAILED, Tmpl, v.GetType());
                        return false;
                    }
                }
            }
            arrValue = v;
            maskBit = true;
            return true;
        }
        #endregion

        #region <<implement ISerializable>>

        public void Encode(IWritableBuffer bw)
        {
            switch (Tmpl.token)
            {
                case DDToken.ft_bool:
                    if (Tmpl.isArray)
                    {
                        bw.WriteBooleanArray((bool[])arrValue);
                    }
                    else
                    {
                        var bv = int32Value != 0 ? true : false;
                        bw.WriteBoolean(bv);
                    }
                    break;
                case DDToken.ft_byte:
                    if (Tmpl.isArray)
                    {
                        bw.WriteByteArray((byte[])arrValue);
                    }
                    else
                    {
                        bw.WriteByte((byte)int32Value);
                    }
                    break;
                case DDToken.ft_int16:
                    if (Tmpl.isArray)
                    {
                        bw.WriteShortArray((short[])arrValue);
                    }
                    else
                    {
                        bw.WriteShort((short)int32Value);
                    }
                    break;
                case DDToken.ft_int32:
                    if (Tmpl.isArray)
                    {
                        bw.WriteIntArray((int[])arrValue);
                    }
                    else
                    {
                        bw.WriteInt(int32Value);
                    }
                    break;
                case DDToken.ft_int64:
                    if (Tmpl.isArray)
                    {
                        bw.WriteLongArray((long[])arrValue);
                    }
                    else
                    {
                        bw.WriteLong(int64Value);
                    }
                    break;
                case DDToken.ft_intXU:
                    if (Tmpl.isArray)
                    {
                        bw.WriteUnsignedVarintArray((long[])arrValue);
                    }
                    else
                    {
                        bw.WriteUnsignedVarint(int64Value);
                    }
                    break;
                case DDToken.ft_intX:
                    if (Tmpl.isArray)
                    {
                        bw.WriteVarintArray((long[])arrValue);
                    }
                    else
                    {
                        bw.WriteVarint(int64Value);
                    }
                    break;
                case DDToken.ft_float:
                    if (Tmpl.isArray)
                    {
                        bw.WriteFloatArray((float[])arrValue);
                    }
                    else
                    {
                        bw.WriteFloat(floatValue);
                    }
                    break;
                case DDToken.ft_float16:
                    if (Tmpl.isArray)
                    {
                        bw.WriteFloat16Array((float[])arrValue);
                    }
                    else
                    {
                        bw.WriteFloat16(floatValue);
                    }
                    break;
                case DDToken.ft_str:
                    if (Tmpl.isArray)
                    {
                        bw.WriteUTF8Array((string[])arrValue);
                    }
                    else
                    {
                        bw.WriteUTF8(strValue);
                    }
                    break;
                case DDToken.ft_object:
                    if (Tmpl.isArray)
                    {
                        bw.WriteNodeArray((DDNode[])arrValue, Tmpl);
                    }
                    else
                    {
                        bw.WritNode(objValue, Tmpl);
                    }
                    break;
            }
        }

        public void Decode(IReadableBuffer br)
        {
            switch (Tmpl.token)
            {
                case DDToken.ft_bool:
                    if (Tmpl.isArray)
                    {
                        arrValue = br.ReadBoolArray();
                    }
                    else
                    {
                        var bv = br.ReadBoolean() ? 1 : 0;
                        int32Value = bv;
                    }
                    break;
                case DDToken.ft_byte:
                    if (Tmpl.isArray)
                    {
                        arrValue = br.ReadByteArray();
                    }
                    else
                    {
                        int32Value = br.ReadByte();
                    }
                    break;
                case DDToken.ft_int16:
                    if (Tmpl.isArray)
                    {
                        arrValue = br.ReadShortArray();
                    }
                    else
                    {
                        int32Value = br.ReadShort();
                    }
                    break;
                case DDToken.ft_int32:
                    if (Tmpl.isArray)
                    {
                        arrValue = br.ReadIntArray();
                    }
                    else
                    {
                        int32Value = br.ReadInt();
                    }
                    break;
                case DDToken.ft_int64:
                    if (Tmpl.isArray)
                    {
                        arrValue = br.ReadLongArray();
                    }
                    else
                    {
                        int64Value = br.ReadLong();
                    }
                    break;
                case DDToken.ft_intXU:
                    if (Tmpl.isArray)
                    {
                        arrValue = br.ReadUnsignedVarintArray();
                    }
                    else
                    {
                        int64Value = br.ReadUnsignedVarint();
                    }
                    break;
                case DDToken.ft_intX:
                    if (Tmpl.isArray)
                    {
                        arrValue = br.ReadVarintArray();
                    }
                    else
                    {
                        int64Value = br.ReadVarint();
                    }
                    break;
                case DDToken.ft_float:
                    if (Tmpl.isArray)
                    {
                        arrValue = br.ReadFloatArray();
                    }
                    else
                    {
                        floatValue = br.ReadFloat();
                    }
                    break;
                case DDToken.ft_float16:
                    if (Tmpl.isArray)
                    {
                        arrValue = br.ReadFloat16Array();
                    }
                    else
                    {
                        floatValue = br.ReadFloat16();
                    }
                    break;
                case DDToken.ft_str:
                    if (Tmpl.isArray)
                    {
                        arrValue = br.ReadUTF8Array();
                    }
                    else
                    {
                        strValue = br.ReadUTF8();
                    }
                    break;
                case DDToken.ft_object:
                    if (Tmpl.isArray)
                    {
                        arrValue = br.ReadNodeArray(Tmpl);
                    }
                    else
                    {
                        objValue = br.ReadNode(Tmpl);
                    }
                    break;
            }
        }

        #endregion

        #region <<implement IFormatString>>
        public string ToFormatString(string nlIndent)
        {
            //fd.FormatStringValue(fields[i], curIndent)
            throw new NotImplementedException();
        }
        #endregion

        #region <<implement ICacheable>>
        public void ReleaseToCache()
        {
            Tmpl = DDFieldTmpl.Empty;

            if (objValue != null)
            {
                objValue.ReleaseToCache();
                objValue = null;
            }

            if (arrValue != null)
            {
                var objArr = arrValue as DDNode[];
                if (objArr != null)
                {
                    objArr.ReleaseToCache();
                }
                arrValue = null;
            }
            strValue = null;
            int32Value = 0;
            int64Value = 0;
            floatValue = 0;
            ObjectCache.Put(this);
        }

        #endregion
    }
}