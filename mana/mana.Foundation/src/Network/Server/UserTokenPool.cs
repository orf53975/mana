using System;
using System.Threading;

namespace mana.Foundation.Network.Server
{
    public sealed class UserTokenPool
    {
        readonly Semaphore acceptedClientsSemaphore;

        readonly SynchronizedDictionary<string, int> m_indexMap = new SynchronizedDictionary<string, int>();

        readonly UserToken[] m_pool;

        readonly Func<UserToken> userTokenGenerator;

        internal UserTokenPool(int capacity, int bufferSize , Func<UserToken> userTokenGen)
        {
            this.acceptedClientsSemaphore = new Semaphore(capacity, capacity);
            this.m_pool = new UserToken[capacity];
            this.userTokenGenerator = userTokenGen;
        }

        public UserToken Get()
        {
            for (int i = 0; i < m_pool.Length; i++)
            {
                if (m_pool[i] == null)
                {
                    m_pool[i] = userTokenGenerator.Invoke();
                    return m_pool[i];
                }
                if (m_pool[i].State == UserToken.kStateIdle)
                {
                    return m_pool[i];
                }
            }
            return null;
        }

        public UserToken Find(string uid)
        {
            var idx = m_indexMap.GetValue(uid, -1);
            if (idx >= 0)
            {
                return m_pool[idx];
            }
            return null;
        }

        public bool WaitOne()
        {
            return acceptedClientsSemaphore.WaitOne();
        }

        void Bind(int tokenIndex)
        {
            var token = m_pool[tokenIndex];
            Logger.Print("BindClient: {0}->{1}", token.uid, token.Address);
            try
            {
                m_indexMap.TryAdd(token.uid, tokenIndex);
                token.OnBinded();
            }
            catch (Exception e)
            {
                Logger.Exception(e);
            }
        }

        void Unbind(int tokenIndex)
        {
            var token = m_pool[tokenIndex];
            if (m_indexMap.Remove(token.uid))
            {
                Logger.Print("unbind token failed! uid = {0}", token.uid);
                acceptedClientsSemaphore.Release();
            }
            else
            {
                Logger.Error("unbind token error!  uid = {0}", token.uid);
            }
            token.Reset();
        }

        public void Update(int curTime)
        {
            UserToken ut;
            for (int i = 0; i < m_pool.Length; i++)
            {
                ut = m_pool[i];
                if (ut == null)
                {
                    continue;
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
                catch (Exception ex)
                {
                    Logger.Exception(ex);
                }
            }
        }


        public void ForEachWorking(Action<UserToken> action)
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
                    Logger.Exception(e);
                }
            }
        }

        public void ForEach(Action<UserToken> action)
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
                    Logger.Exception(e);
                }
            }
        }

    }
}
