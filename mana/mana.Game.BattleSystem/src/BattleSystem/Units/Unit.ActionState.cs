using System;

namespace BattleSystem.Units
{
    public partial class Unit
    {
        public enum ActionState : byte
        {
            none = 0,
            idle = 1,
            move = 2,
            attack = 3,
            injury = 4,
            cast = 5,
            dead = 6,
        }

        public ActionState curActionState
        {
            get;
            private set;
        }

        public void ChangeActionState(ActionState actionCode)
        {
            if (this.curActionState != actionCode)
            {
                this.curActionState = actionCode;
                battle.recorder.PushActionState(this);
            }
        }

        public int curAnimation
        {
            get;
            private set;
        }

        public void PlayAnimation(int animCode, float animTime = 0)
        {
            this.curAnimation = animCode;
            battle.recorder.PushAnimPlay(this, animTime);
        }

        public int GetDefaultAnimation(ActionState ast)
        {
            return (int)ast;
        }
    }
}