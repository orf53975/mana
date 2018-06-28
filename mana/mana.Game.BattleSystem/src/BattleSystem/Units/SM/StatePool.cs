using System;

namespace BattleSystem.Units.SM
{
    internal class StatePool
    {
        private readonly Unit unit;

        internal StatePool(Unit unit)
        {
            this.unit = unit;
        }

        internal T Get<T>() where T : State
        {
            var _t = typeof(T);
            return Activator.CreateInstance(_t, unit) as T;
        }

        internal void Put(State curState)
        {
            //TODO
        }
    }
}
