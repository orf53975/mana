using BattleSystem.Units.SM;
 
using System;

namespace BattleSystem.Units
{
    public partial class Unit
    {
        private StatePool mStatePool = null;

        State curState = null;

        State nxtState = null;

        private void InitState()
        {
            if (mStatePool == null)
            {
                mStatePool = new StatePool(this);
            }
        }

        public bool IsInNullState()
        {
            return curState == null;
        }

        private void SetState(State nst)
        {
            if (curState != null)
            {
                curState.Exit();
                mStatePool.Put(curState);
                curState = null;
            }
            curState = nst;
            if (curState != null)
            {
                curState.Enter();
            }
        }

        void TryChangeState(State nst)
        {
            if (nst == nxtState)
            {
                return;
            }
            if (nxtState == null)
            {
                nxtState = nst;
            }
            else if (nxtState.priority <= nst.priority)
            {
                mStatePool.Put(nxtState);
                nxtState = null;
                nxtState = nst;
            }
        }

        internal void TryChangeState<T>(Action<T> stInitFunc = null) where T : State
        {
            var st = mStatePool.Get<T>();
            if (stInitFunc != null)
            {
                stInitFunc.Invoke(st);
            }
            this.TryChangeState(st);
        }

        public void Update()
        {
            this.passedTime += this.battle.frameTime;
            if (curState != null)
            {
                if (!curState.Update())
                {
                    SetState(null);
                }
                else if (curState.openAI)
                {
                    this.DoAI();
                }
            }
            else
            {
                this.DoAI();
            }
        }

        public void LateUpdate()
        {
            if (nxtState != null && nxtState != curState)
            {
                if (nxtState.TestEnter() && (curState == null || curState.CanInterruptedBy(nxtState)))
                {
                    SetState(nxtState);
                }
                nxtState = null;
            }
        }
    }
}