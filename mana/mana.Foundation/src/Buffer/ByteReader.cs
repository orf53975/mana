using System;
using System.IO;

namespace mana.Foundation
{
    public sealed class ByteReader : IReadableBuffer, IDisposable
    {

        #region <<Static Pool>>

        private static readonly ObjectPool<ByteReader> Pool = new ObjectPool<ByteReader>(
            () => new ByteReader((e) => Pool.Put(e)), null, (e) => e.Clear());

        public static ByteReader Create(byte[] data)
        {
            var br = ByteReader.Pool.Get();
            br.Init(data);
            return br;
        }

        #endregion


        readonly Action<ByteReader> disposeHandler;

        private byte[] mData = null;

        private int m_len = 0;

        private int m_pos = 0;

        private ByteReader(Action<ByteReader> cb)
        {
            this.disposeHandler = cb;
        }

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

        void Init(byte[] bufferData)
        {
            this.mData = bufferData;
            this.m_len = bufferData.Length;
            this.m_pos = 0;
        }

        private readonly byte[] r_buffer = new byte[8];

        public byte ReadByte()
        {
            var npos = m_pos + 1;
            if (npos > m_len)
            {
                throw new IndexOutOfRangeException();
            }
            var ret = mData[m_pos];
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
                    buffer[offset + i] = this.mData[this.m_pos + i];
                }
            }
            else
            {
                Buffer.BlockCopy(this.mData, this.m_pos, buffer, offset, num);
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

        public void Clear()
        {
            this.mData = null;
            this.m_len = 0;
            this.m_pos = 0;
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