using System;

namespace mana.Foundation
{
    internal static class DDNodeExtension
    {
        internal static DDNode ReadNode(this IReadableBuffer br, DDFieldTmpl fieldTmpl)
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
                    var ret = DDNode.Creat(tmpl);
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

        internal static DDNode[] ReadNodeArray(this IReadableBuffer br, DDFieldTmpl fieldTmpl)
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
            var ret = new DDNode[len];
            for (var i = 0; i < len; i++)
            {
                ret[i] = br.ReadNode(fieldTmpl);
            }
            return ret;
        }

        internal static void WritNode(this IWritableBuffer bw, DDNode obj, DDFieldTmpl fieldTmpl)
        {
            if (obj != null)
            {
                using (var tempBw = ByteBuffer.Pool.Get())
                {
                    if (fieldTmpl.isUnknowType)
                    {
                        var typeCode = Protocol.Instance.GetTypeCode(obj.Tmpl.fullName);
                        tempBw.WriteUnsignedShort(typeCode);
                        if (typeCode == 0)
                        {
                            Logger.Error("GetTypeCode failed! -> {0}", obj.GetType());
                        }
                        else
                        {
                            obj.Encode(tempBw);
                        }
                    }
                    else
                    {
                        if (fieldTmpl.objTmpl != obj.Tmpl.fullName)
                        {
                            Logger.Error("write failed! can't match tmpl {0} -> {1} ", fieldTmpl.objTmpl, obj.Tmpl.fullName);
                        }
                        else
                        {
                            obj.Encode(tempBw);
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

        internal static void WriteNodeArray(this IWritableBuffer bw, DDNode[] arr, DDFieldTmpl dataTmpl)
        {
            int len = arr == null ? 0 : arr.Length;
            bw.WriteUnsignedVarint(len);
            for (int i = 0; i < len; i++)
            {
                bw.WritNode(arr[i], dataTmpl);
            }
        }
    }
}
