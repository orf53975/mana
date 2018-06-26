using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace mana.Foundation.Network.Sever
{
    internal sealed class UserTokenPool
    {
        internal readonly Semaphore acceptedClientsSemaphore;

        readonly Dictionary<string, int> m_indexMap = new Dictionary<string, int>();

        readonly UserToken[] m_pool;

        readonly IOCPServer mServer;

        internal UserTokenPool(IOCPServer sev , int capacity, int bufferSize)
        {
            this.acceptedClientsSemaphore = new Semaphore(capacity, capacity);
            this.mServer = sev;
            this.m_pool = new UserToken[capacity];
        }

        public UserToken Get()
        {
            for (int i = 0; i < m_pool.Length; i++)
            {
                if (m_pool[i] == null)
                {
                    m_pool[i] = new UserToken(null, mServer);
                    return m_pool[i];
                }
                else if (m_pool[i].State == UserToken.kStateIdle)
                {
                    return m_pool[i];
                }
            }
            return null;
        }

        internal UserToken Find(string uid)
        {
            lock (m_indexMap)
            {
                int idx = -1;
                if (m_indexMap.TryGetValue(uid, out idx))
                {
                    return m_pool[idx];
                }
                return null;
            }
        }

        void Bind(int tokenIndex)
        {
            var token = m_pool[tokenIndex];
            Trace.TraceInformation("BindClient: {0}->{1}", token.uid, token.socket.RemoteEndPoint.ToString());
            try
            {
                m_indexMap.Add(token.uid, tokenIndex);
                token.OnBinded();
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
            }
        }

        void Unbind(int tokenIndex)
        {
            var token = m_pool[tokenIndex];
            if (m_indexMap.Remove(token.uid))
            {
                Trace.TraceInformation("unbind token! uid = {0}", token.uid);
                acceptedClientsSemaphore.Release();
            }
            else
            {
                Trace.TraceError("unbind token error! uid = {0}", token.uid);
            }
            token.Reset();
        }

        internal void Update(int curTime)
        {
            UserToken ut;
            for (int i = 0; i < m_pool.Length; i++)
            {
                ut = m_pool[i];
                if (ut == null)
                {
                    break;
                }
                try
                {
                    switch (ut.State)
                    {
                        case UserToken.kStateIdle:
                            break;
                        case UserToken.kStateLink:
                            if (ut.binded)
                            {
                                ut.CheckWorkStateTimeOut(curTime);
                            }
                            else if (ut.uid == null)
                            {
                                ut.CheckLinkStateTimeOut(curTime);
                            }
                            else
                            {
                                Bind(i);
                            }
                            break;
                        case UserToken.kStateFree:
                            if (ut.binded)
                            {
                                Unbind(i);
                            }
                            break;
                    }
                }
                catch (Exception e)
                {
                    Trace.TraceError(e.ToString());
                }
            }
        }


        internal void ForEachWorking(Action<UserToken> action)
        {
            for (int i = 0; i < m_pool.Length; i++)
            {
                if (m_pool[i] == null)
                {
                    break;
                }
                if (!m_pool[i].isWorking)
                {
                    continue;
                }
                try
                {
                    action(m_pool[i]);
                }
                catch (Exception e)
                {
                    Trace.TraceError(e.ToString());
                }
            }
        }

        internal void ForEach(Action<UserToken> action)
        {
            for (int i = 0; i < m_pool.Length; i++)
            {
                if (m_pool[i] == null)
                {
                    break;
                }
                try
                {
                    action(m_pool[i]);
                }
                catch (Exception e)
                {
                    Trace.TraceError(e.ToString());
                }
            }
        }

    }
}
