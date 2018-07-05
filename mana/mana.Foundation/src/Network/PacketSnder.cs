using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace mana.Foundation
{
    public sealed class PacketSnder
    {
        private readonly Queue<Packet> sendingQue = new Queue<Packet>();

        private readonly ByteBuffer buff = new ByteBuffer(1024);

        public bool HasSendingData
        {
            get
            {
                return sendingQue.Count > 0 || buff.Available > 0;
            }
        }

        void TryUpdateSendingBuff()
        {
            lock (sendingQue)
            {
                while (sendingQue.Count > 0 && buff.Available < 1024)
                {
                    var p = sendingQue.Dequeue();
                    Packet.Encode(p, buff);
                    p.Release();
                }
            }
        }

        public void WriteTo(byte[] buffer, ref int offset, int sendBufferLimit)
        {
            lock (buff)
            {
                if (sendingQue.Count > 0 && buff.Available < 1024)
                {
                    this.TryUpdateSendingBuff();
                }
                var nBytes = sendBufferLimit - offset;
                if (nBytes > buff.Available)
                {
                    nBytes = buff.Available;
                }
                if (nBytes > 0)
                {
                    buff.Read(buffer, offset, nBytes);
                    offset += nBytes;
                    buff.DiscardReadBytes();
                }
            }
        }


        public int WriteTo<T>(T param, Func<T, byte[], int, int, int> data_receiver)
        {
            lock (buff)
            {
                if (sendingQue.Count > 0 && buff.Available < 1024)
                {
                    this.TryUpdateSendingBuff();
                }
                if (buff.Available == 0)
                {
                    return 0;
                }
                var count = data_receiver.Invoke(param, buff.data, buff.Position, buff.Available);
                if (count > 0)
                {
                    buff.Skip(count);
                    buff.DiscardReadBytes();
                }
                return count;
            }
        }

        public int WriteTo(Socket socket)
        {
            lock (buff)
            {
                if (sendingQue.Count > 0 && buff.Available < 1024)
                {
                    this.TryUpdateSendingBuff();
                }
                if (buff.Available == 0)
                {
                    return 0;
                }
                var count = socket.Send(buff.data, buff.Position, buff.Available, SocketFlags.None);
                if (count > 0)
                {
                    buff.Skip(count);
                    buff.DiscardReadBytes();
                }
                return count;
            }
        }

        public void Push(Packet p)
        {
            p.Retain();
            lock (sendingQue)
            {
                sendingQue.Enqueue(p);
            }
        }

        public void Clear()
        {
            lock (sendingQue) { sendingQue.Clear(); }
            lock (buff) { buff.Clear(); }
        }
    }
}
