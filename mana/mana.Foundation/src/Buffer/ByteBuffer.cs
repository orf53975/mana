using System;
using System.IO;

namespace mana.Foundation
{
    public sealed class ByteBuffer : IReadableBuffer, IWritableBuffer, IDisposable
    {

        #region <<Static Pool>>

        public static readonly ObjectPool<ByteBuffer> Pool = new ObjectPool<ByteBuffer>(
            () => new ByteBuffer((e) => Pool.Put(e)), null, (e) => e.Clear());

        #endregion

        public const int DEFAULT_CAPACITY = 16;

        readonly Action<ByteBuffer> disposeHandler;

        internal byte[] data
        {
            get;
            private set;
        }

        private int m_len;


        private int m_pos;


        public ByteBuffer(int initCapacity = DEFAULT_CAPACITY)
        {
            this.data = new byte[initCapacity];
            this.m_pos = 0;
            this.m_len = 0;
        }

        private ByteBuffer(Action<ByteBuffer> cb)
            : this(DEFAULT_CAPACITY)
        {
            this.disposeHandler = cb;
        }


        #region <<Implemented IReadableBuffer>>

        public int Position
        {
            get
            {
                return m_pos;
            }
        }

        public int Available
        {
            get
            {
                return m_len - m_pos;
            }
        }

        private readonly byte[] r_buffer = new byte[8];

        public byte ReadByte()
        {
            var npos = m_pos + 1;
            if (npos > m_len)
            {
                throw new IndexOutOfRangeException();
            }
            var ret = data[m_pos];
            m_pos = npos;
            return ret;
        }

        public int ReadInt16()
        {
            this.Read(r_buffer, 0, 2);
            return (short)(
                (int)this.r_buffer[0] << 8 |
                (int)r_buffer[1]
                );
        }

        public int ReadInt24()
        {
            this.Read(r_buffer, 0, 3);
            return
                (int)r_buffer[1] << 16 |
                (int)r_buffer[2] << 8 |
                (int)r_buffer[3];
        }

        public int ReadInt32()
        {
            this.Read(r_buffer, 0, 4);
            return
                (int)r_buffer[0] << 24 |
                (int)r_buffer[1] << 16 |
                (int)r_buffer[2] << 8 |
                (int)r_buffer[3];
        }

        public long ReadInt64()
        {
            this.Read(r_buffer, 0, 8);
            return (long)(
                (ulong)r_buffer[0] << 56 |
                (ulong)r_buffer[1] << 48 |
                (ulong)r_buffer[2] << 40 |
                (ulong)r_buffer[3] << 32 |
                (ulong)r_buffer[4] << 24 |
                (ulong)r_buffer[5] << 16 |
                (ulong)r_buffer[6] << 8 |
                (ulong)r_buffer[7]
                );
        }

        public void Read(byte[] buffer, int offset, int count)
        {
            if (buffer == null || offset < 0 || count < 0 || buffer.Length - offset < count)
            {
                throw new ArgumentException();
            }
            int num = count;
            if (num > Available)
            {
                throw new EndOfStreamException();
            }
            if (num <= 8)
            {
                for (int i = num - 1; i >= 0; i--)
                {
                    buffer[offset + i] = this.data[this.m_pos + i];
                }
            }
            else
            {
                Buffer.BlockCopy(this.data, this.m_pos, buffer, offset, num);
            }
            this.m_pos += num;
        }

        public void Skip(int n)
        {
            var npos = m_pos + n;
            if (npos < 0 || npos > m_len)
            {
                throw new ArgumentOutOfRangeException();
            }
            else
            {
                m_pos = npos;
            }
        }

        public void Seek(int newPosition)
        {
            if (newPosition > m_len || newPosition < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            this.m_pos = newPosition;
        }

        #endregion

        #region <<Implemented IWritableBuffer>>

        public int Length
        {
            get
            {
                return m_len;
            }
        }

        private readonly byte[] _buffer = new byte[8];

        public void EnsureCapacity(int minCapacity)
        {
            if (minCapacity < 0 || minCapacity > int.MaxValue)
            {
                throw new OutOfMemoryException();
            }
            // -- grow Capacity
            int curCapacity = data.Length;
            if (minCapacity <= curCapacity)
            {
                return;
            }
            var grow = Math.Max(curCapacity >> 1, 16);
            curCapacity += grow;
            if (curCapacity < minCapacity)
            {
                curCapacity = minCapacity;
            }
            // -- Set new Capacity
            byte[] new_dat = new byte[curCapacity];
            if (data != null)
            {
                Buffer.BlockCopy(data, 0, new_dat, 0, m_len);
            }
            data = new_dat;
        }

        public void WriteByte(byte val)
        {
            var nLen = m_len + 1;
            EnsureCapacity(nLen);
            data[m_len] = val;
            m_len = nLen;
        }

        public void WriteInt16(int val)
        {
            this._buffer[0] = (byte)(val >> 8);
            this._buffer[1] = (byte)(val);
            this.Write(this._buffer, 0, 2);
        }

        public void WriteInt24(int val)
        {
            this._buffer[0] = (byte)(val >> 16);
            this._buffer[1] = (byte)(val >> 8);
            this._buffer[2] = (byte)(val);
            this.Write(this._buffer, 0, 3);
        }

        public void WriteInt32(int val)
        {
            this._buffer[0] = (byte)(val >> 24);
            this._buffer[1] = (byte)(val >> 16);
            this._buffer[2] = (byte)(val >> 8);
            this._buffer[3] = (byte)(val);
            this.Write(this._buffer, 0, 4);
        }

        public void WriteInt64(long val)
        {
            this._buffer[0] = (byte)(val >> 56);
            this._buffer[1] = (byte)(val >> 48);
            this._buffer[2] = (byte)(val >> 40);
            this._buffer[3] = (byte)(val >> 32);
            this._buffer[4] = (byte)(val >> 24);
            this._buffer[5] = (byte)(val >> 16);
            this._buffer[6] = (byte)(val >> 8);
            this._buffer[7] = (byte)(val);
            this.Write(this._buffer, 0, 8);
        }

        public void Write(byte[] bytes, int offset, int count)
        {
            if (bytes == null || offset < 0 || count < 0 || count + offset > bytes.Length)
            {
                throw new ArgumentOutOfRangeException();
            }
            var nLen = m_len + count;
            EnsureCapacity(nLen);
            Buffer.BlockCopy(bytes, offset, data, m_len, count);
            m_len = nLen;
        }

        public void Write<T>(int count, T param, Action<byte[], int, int, T> dataSetter)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            var nLen = m_len + count;
            EnsureCapacity(nLen);
            if (dataSetter != null)
            {
                dataSetter(data, m_len, count, param);
            }
            this.m_len = nLen;
        }

        public void Write(int count, Action<byte[], int, int> dataSetter)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            var nLen = m_len + count;
            EnsureCapacity(nLen);
            this.m_len = nLen;
            if (dataSetter != null)
            {
                dataSetter(data, m_len, count);
            }
        }

        #endregion

        /// <summary>
        /// 清空已读缓存
        /// </summary>
        public void DiscardReadBytes()
        {
            if (m_pos == 0)
            {
                return;
            }
            var d_len = Available;
            if (d_len > 0)
            {
                for (int i = 0; i < d_len; i++)
                {
                    data[i] = data[m_pos + i];
                }
            }
            m_len = d_len;
            m_pos = 0;
        }

        public void Clear()
        {
            m_pos = 0;
            m_len = 0;
        }

        void IDisposable.Dispose()
        {
            if (disposeHandler != null)
            {
                disposeHandler.Invoke(this);
            }
        }
    }
}