using System;

namespace mana.Foundation
{
    public static class DataNodeExtension
    {
        #region <<Reader Extension>>

        public static DataNode ReadDataNode(this IReadableBuffer br, DataFieldTmpl fieldTmpl)
        {
            var len = (int)br.ReadUnsignedVarint();
            if (len == 0)
            {
                return null;
            }
            if (len < 0)
            {
                throw new OutOfMemoryException();
            }
            int _readLimitMark = br.Position + len;
            try
            {
                string tmpl = null;
                if (fieldTmpl.isUnknowType)
                {
                    tmpl = Protocol.Instance.GetDataType(br.ReadUnsignedShort());
                }
                else
                {
                    tmpl = fieldTmpl.objTmpl;
                }
                if (tmpl != null)
                {
                    var ret = DataNode.Creat(tmpl);
                    ret.Decode(br);
                    return ret;
                }
                return null;
            }
            catch (Exception e)
            {
                Logger.Exception(e);
            }
            finally
            {
                br.Seek(_readLimitMark);
            }
            return null;
        }

        public static DataNode[] ReadDataNodeArray(this IReadableBuffer br, DataFieldTmpl fieldTmpl)
        {
            var len = (int)br.ReadUnsignedVarint();
            if (len == 0)
            {
                return null;
            }
            if (len < 0)
            {
                throw new OutOfMemoryException();
            }
            var ret = new DataNode[len];
            for (var i = 0; i < len; i++)
            {
                ret[i] = br.ReadDataNode(fieldTmpl);
            }
            return ret;
        }

        #endregion

        #region <<Writer Extension>>

        public static void WriteDataNode(this IWritableBuffer bw, DataNode obj, DataFieldTmpl fieldTmpl, bool isMaskAll)
        {
            if (obj != null)
            {
                using (var tempBw = ByteBuffer.Pool.Get())
                {
                    if (fieldTmpl.isUnknowType)
                    {
                        var typeCode = Protocol.Instance.GetTypeCode(obj.Tmpl.name);
                        tempBw.WriteUnsignedShort(typeCode);
                        if (typeCode == 0)
                        {
                            Logger.Error("GetTypeCode failed! -> {0}", obj.GetType());
                        }
                        else
                        {
                            obj.Encode(tempBw, isMaskAll);
                        }
                    }
                    else
                    {
                        if (fieldTmpl.objTmpl != obj.Tmpl.name)
                        {
                            Logger.Error("write failed! can't match tmpl {0} -> {1} ", fieldTmpl.objTmpl, obj.Tmpl.name);
                        }
                        else
                        {
                            obj.Encode(tempBw, isMaskAll);
                        }
                    }
                    bw.WriteUnsignedVarint(tempBw.Length);
                    bw.Write(tempBw);
                }
            }
            else
            {
                bw.WriteShort(0);
            }
        }

        public static void WriteDataNodeArray(this IWritableBuffer bw, DataNode[] arr, DataFieldTmpl dataTmpl, bool isMaskAll)
        {
            int len = arr == null ? 0 : arr.Length;
            bw.WriteUnsignedVarint(len);
            for (int i = 0; i < len; i++)
            {
                bw.WriteDataNode(arr[i], dataTmpl, isMaskAll);
            }
        }

        #endregion
    }
}
