using System;

namespace mana.Foundation
{
    public static class DataObjectExtension
    {

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
                    var obj = ObjectCache.TryGet(t);
                    if (obj == null)
                    {
                        obj = Activator.CreateInstance(t);
                    }
                    var ret = obj as DataObject;
                    if (ret == null)
                    {
                        Logger.Error("{0} is not DataObject", typeName);
                    }
                    else
                    {
                        ret.Decode(br);
                        return ret;
                    }
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
        public static void Write<T>(this IWritableBuffer bw, T obj) where T : DataObject, new()
        {
            if (obj != null)
            {
                using (var tempBw = ByteBuffer.Pool.Get())
                {
                    obj.Encode(tempBw);

                    bw.WriteUnsignedVarint(tempBw.Length);
                    bw.Write(tempBw);
                }
            }
            else
            {
                bw.WriteUnsignedVarint(0);
            }
        }

        public static void WriteArray<T>(this IWritableBuffer bw, T[] arr) where T : DataObject, new()
        {
            int len = arr == null ? 0 : arr.Length;
            bw.WriteUnsignedVarint(len);
            for (int i = 0; i < len; i++)
            {
                bw.Write(arr[i]);
            }
        }

        public static void WriteUnknow(this IWritableBuffer bw, DataObject obj)
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
                        obj.Encode(tempBw);
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

        public static void WriteUnknowArray(this IWritableBuffer bw, DataObject[] arr)
        {
            int len = arr == null ? 0 : arr.Length;
            bw.WriteUnsignedVarint(len);
            for (int i = 0; i < len; i++)
            {
                bw.WriteUnknow(arr[i]);
            }
        }
        #endregion

    }
}
