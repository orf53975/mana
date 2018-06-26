using System.Collections.Generic;
using System.Net.Sockets;

namespace mana.Foundation.Network.Sever
{
    internal sealed class BufferManager
    {
        readonly Stack<int> m_freeIndexPool = new Stack<int>();

        public readonly int bufferCount;

        public readonly int bufferSize;

        private readonly byte[] m_buffer;

        private int m_currentIndex;

        public BufferManager(int bufferCount, int bufferSize)
        {
            this.bufferCount = bufferCount;
            this.bufferSize = bufferSize;
            m_buffer = new byte[bufferCount * bufferSize];
            m_currentIndex = 0;
        }

        public bool SetBuffer(SocketAsyncEventArgs args)
        {
            if (m_freeIndexPool.Count > 0)
            {
                args.SetBuffer(m_buffer, m_freeIndexPool.Pop(), bufferSize);
            }
            else
            {
                if (m_currentIndex >= m_buffer.Length)
                {
                    return false;
                }
                args.SetBuffer(m_buffer, m_currentIndex, bufferSize);
                m_currentIndex += bufferSize;
            }
            return true;
        }

        public void FreeBuffer(SocketAsyncEventArgs args)
        {
            m_freeIndexPool.Push(args.Offset);
            args.SetBuffer(null, 0, 0);
        }
    }
}
