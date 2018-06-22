using System;

namespace mana.Foundation
{
    public static class DataObjectExtension
    {
        public static T DeepClone<T>(this T src) 
            where T : class, DataObject, new()
        {
            var ret = ObjectCache.Get<T>();
            src.DeepCopyTo(ret);
            return ret;
        }

        #region <<Reader Extension>>
        public static T Read<T>(this IReadableBuffer br) 
            where T : class, DataObject, new()
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
            int _readLimit = br.Position + len;
            try
            {
                var ret = ObjectCache.Get<T>();
                ret.Decode(br);
                return ret;
            }
            catch (Exception e)
            {
                Logger.Exception(e);
            }
            finally
            {
                br.Seek(_readLimit);
            }
            return null;
        }

        public static T[] ReadArray<T>(this IReadableBuffer br) 
            where T : class, DataObject, new()
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
            var ret = new T[len];
            for (var i = 0; i < len; i++)
            {
                ret[i] = br.Read<T>();
            }
            return ret;
        }

        public static DataObject ReadUnknow(this IReadableBuffer br)
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
            int _readLimit = br.Position + len;
            try
            {
                var typeCode = br.ReadUnsignedShort();
                var typeName = Protocol.Instance.GetDataType(typeCode);
                var t = Type.GetType(typeName);
                if (t != null)
                {
                    var ret = ObjectCache.Get(t) as DataObject;
                    ret.Decode(br);
                    return ret;
                }
                else
                {
                    Logger.Error("can't find type [{0}]", typeName);
                }
            }
            catch (Exception e)
            {
                Logger.Exception(e);
            }
            finally
            {
                br.Seek(_readLimit);
            }
            return null;
        }

        public static DataObject[] ReadUnknowArray(this IReadableBuffer br)
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
            var ret = new DataObject[len];
            for (var i = 0; i < len; i++)
            {
                ret[i] = br.ReadUnknow();
            }
            return ret;
        }


        #endregion

        #region <<Writer Extension>>
        public static void Write<T>(this IWritableBuffer bw, T obj, bool isWriteAll) where T : DataObject, new()
        {
            if (obj != null)
            {
                using (var tempBw = ByteBuffer.Pool.Get())
                {
                    obj.Encode(tempBw, isWriteAll);

                    bw.WriteUnsignedVarint(tempBw.Length);
                    bw.Write(tempBw);
                }
            }
            else
            {
                bw.WriteUnsignedVarint(0);
            }
        }

        public static void WriteArray<T>(this IWritableBuffer bw, T[] arr, bool isWriteAll) where T : DataObject, new()
        {
            int len = arr == null ? 0 : arr.Length;
            bw.WriteUnsignedVarint(len);
            for (int i = 0; i < len; i++)
            {
                bw.Write(arr[i], isWriteAll);
            }
        }

        public static void WriteUnknow(this IWritableBuffer bw, DataObject obj, bool isWriteAll)
        {
            if (obj != null)
            {
                using (var tempBw = ByteBuffer.Pool.Get())
                {
                    var typeCode = Protocol.Instance.GetTypeCode(obj.GetType().FullName);
                    tempBw.WriteUnsignedShort(typeCode);
                    if (typeCode == 0)
                    {
                        Logger.Error("GetTypeCode failed! -> {0}", obj.GetType());
                    }
                    else
                    {
                        obj.Encode(tempBw, isWriteAll);
                    }
                    bw.WriteUnsignedVarint(tempBw.Length);
                    bw.Write(tempBw);
                }
            }
            else
            {
                bw.WriteUnsignedVarint(0);
            }
        }

        public static void WriteUnknowArray(this IWritableBuffer bw, DataObject[] arr, bool isWriteAll)
        {
            int len = arr == null ? 0 : arr.Length;
            bw.WriteUnsignedVarint(len);
            for (int i = 0; i < len; i++)
            {
                bw.WriteUnknow(arr[i], isWriteAll);
            }
        }
        #endregion

    }
}
