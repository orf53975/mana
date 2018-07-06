using mana.Foundation;
using xxd.battle.opration;

namespace BattleSystem.Units.SM
{
    public abstract class State
    {
        public readonly StateConfigAttribute configAttr;

        public readonly Unit host;

        public readonly Unit.ActionState actionState;

        public State(Unit unit)
        {
            this.host = unit;
            this.configAttr = this.GetType().TryGetAttribute<StateConfigAttribute>();
            if (configAttr != null)
            {
                this.priority = configAttr.priority;
                this.openAI = configAttr.openAI;
                this.actionState = configAttr.actionState;
            }
            else
            {
                this.priority = 0;
                this.openAI = false;
                this.actionState = Unit.ActionState.none;
            }
        }

        /// <summary>
        /// 状态优先级
        /// </summary>
        public int priority
        {
            get;
            protected set;
        }

        public float passedTime
        {
            get;
            private set;
        }

        public bool openAI
        {
            get;
            protected set;
        }


        public virtual void OnPlayerOperation(MoveRequest mr)
        {
        }

        public virtual void OnPlayerOperation(CastRequest cr)
        {
        }

        /// <summary>
        /// 可否被新状态打断
        /// </summary>
        /// <param name="newState"></param>
        /// <returns></returns>
        public virtual bool CanInterruptedBy(State newState)
        {
            return newState.priority >= this.priority;
        }

        /// <summary>
        /// 测试能否进入状态 如果返回true 才可以进入状态
        /// </summary>
        /// <returns></returns>
        public virtual bool TestEnter()
        {
            return true;
        }

        /// <summary>
        /// 状态进入
        /// </summary>
        public void Enter()
        {
            host.ChangeActionState(this.actionState);
            passedTime = 0;
            this.OnEnter();
        }

        protected abstract void OnEnter();

        /// <summary>
        /// 状态更新函数
        /// </summary>
        /// <returns>如果状态执行完毕返回false</returns>
        public bool Update()
        {
            passedTime += host.battle.frameTime;
            return OnUpdate();
        }

        protected abstract bool OnUpdate();


        /// <summary>
        /// 状态退出
        /// </summary>
        public void Exit()
        {
            this.OnExit();
        }

        protected virtual void OnExit()
        {
        }
    }
}
