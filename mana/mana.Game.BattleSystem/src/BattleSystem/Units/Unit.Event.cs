using BattleSystem.Units.Abilities;
using BattleSystem.Units.Events;
using BattleSystem.Units.SM;
using BattleSystem.Util;
using mana.Foundation;
using System;
using System.Collections.Generic;
using xxd.battle;

namespace BattleSystem.Units
{
    public partial class Unit
    {
        #region <<OnAttackModifier>>
        public event Action<Damage, Unit> AttackModifierListener;
        private readonly List<IAttackModifier> attackModifierHandlers = new List<IAttackModifier>();
        private void OnAttackModifier(Damage dmg, Unit unit)
        {
            attackModifierHandlers.ForEach((e) => e.onAttackModifier(dmg, unit));
            if (AttackModifierListener != null)
            {
                AttackModifierListener.Invoke(dmg, unit);
            }
        }
        #endregion

        #region <<OnAttackFeedback>>
        public event Action<Damage, Unit> AttackFeedbackListener;
        private readonly List<IAttackFeedback> attackFeedbackHandlers = new List<IAttackFeedback>();
        private void OnAttackFeedback(Damage dmg, Unit unit)
        {
            attackFeedbackHandlers.ForEach((e) => e.onAttackFeedback(dmg, unit));
            if (AttackFeedbackListener != null)
            {
                AttackFeedbackListener.Invoke(dmg, unit);
            }
        }
        #endregion

        #region <<OnKill>>
        public event Action<Damage, Unit> KillListener;
        private readonly List<IKilledHandler> killHandlers = new List<IKilledHandler>();
        private void OnKill(Damage dmg, Unit unit)
        {
            killHandlers.ForEach((e) => e.onKill(dmg, unit));
            if (KillListener != null)
            {
                KillListener.Invoke(dmg, unit);
            }
        }
        #endregion

        #region <<OnInjuryModifier>>
        public event Action<Damage, Unit> InjuryModifierListener;
        private readonly List<IInjuryModifier> injuryModifierHandlers = new List<IInjuryModifier>();
        private void OnInjuryModifier(Damage dmg, Unit unit)
        {
            injuryModifierHandlers.ForEach((e) => e.onInjuryModifier(dmg, unit));
            if (InjuryModifierListener != null)
            {
                InjuryModifierListener.Invoke(dmg, unit);
            }
        }
        #endregion

        #region <<OnInjuryFeedback>>
        public event Action<Damage, Unit> InjuryFeedbackListener;
        private readonly List<IInjuryFeedback> injuryFeedbackHandlers = new List<IInjuryFeedback>();
        private void OnInjuryFeedback(Damage dmg, Unit unit)
        {
            injuryFeedbackHandlers.ForEach((e) => e.onInjuryFeedback(dmg, unit));
            if (InjuryFeedbackListener != null)
            {
                InjuryFeedbackListener.Invoke(dmg, unit);
            }
        }
        #endregion

        #region <<OnDead>>
        public event Action<Damage, Unit> DeadListener;
        private readonly List<IDeadHandler> deadHandlers = new List<IDeadHandler>();
        private void OnDead(Damage dmg, Unit unit)
        {
            dmg.killedParam = 1;
            deadHandlers.ForEach((e) => e.onDead(dmg, unit));
            if (DeadListener != null)
            {
                DeadListener.Invoke(dmg, unit);
            }
        }
        #endregion

        #region <<OnCureModifier>>
        public event Action<Healing, Unit> CureModifierListener;
        private readonly List<ICureModifier> cureModifierHandlers = new List<ICureModifier>();
        private void OnCureModifier(Healing hel, Unit unit)
        {
            cureModifierHandlers.ForEach((e) => e.onCureModifier(hel, unit));
            if (CureModifierListener != null)
            {
                CureModifierListener.Invoke(hel, unit);
            }
        }
        #endregion

        #region <<OnCureFeedback>>
        public event Action<Healing, Unit> CureFeedbackListener;
        private readonly List<ICureFeedback> cureFeedbackHandlers = new List<ICureFeedback>();
        private void OnCureFeedback(Healing hel, Unit unit)
        {
            cureFeedbackHandlers.ForEach((e) => e.onCureFeedback(hel, unit));
            if (CureFeedbackListener != null)
            {
                CureFeedbackListener.Invoke(hel, unit);
            }
        }
        #endregion

        #region <<OnHealModifier>>
        public event Action<Healing, Unit> HealModifierListener;
        private readonly List<IHealModifier> healModifierHandlers = new List<IHealModifier>();
        private void OnHealModifier(Healing hel, Unit unit)
        {
            healModifierHandlers.ForEach((e) => e.onHealModifier(hel, unit));
            if (HealModifierListener != null)
            {
                HealModifierListener.Invoke(hel, unit);
            }
        }
        #endregion

        #region <<OnHealFeedback>>
        public event Action<Healing, Unit> HealFeedbackListener;
        private readonly List<IHealFeedback> healFeedbackHandlers = new List<IHealFeedback>();
        private void OnHealFeedback(Healing hel, Unit unit)
        {
            healFeedbackHandlers.ForEach((e) => e.onHealFeedback(hel, unit));
            if (HealFeedbackListener != null)
            {
                HealFeedbackListener.Invoke(hel, unit);
            }
        }
        #endregion

        #region <<BeforeCast>>
        public event Action<Ability> BeforeCastListener;
        private readonly List<IBeforeAbilityCast> beforeCastHandlers = new List<IBeforeAbilityCast>();
        private void BeforeCast(Ability ab)
        {
            beforeCastHandlers.ForEach((e) => e.beforeCast(ab));
            if (BeforeCastListener != null)
            {
                BeforeCastListener.Invoke(ab);
            }
        }
        #endregion

        #region <<AfterCast>>
        public event Action<Ability> AfterCastListener;
        private readonly List<IAfterAbilityCast> afterCastHandlers = new List<IAfterAbilityCast>();
        private void AfterCast(Ability ab)
        {
            afterCastHandlers.ForEach((e) => e.afterCast(ab));
            if (AfterCastListener != null)
            {
                AfterCastListener.Invoke(ab);
            }
        }
        #endregion

        #region <<OnRoundPrepared>>
        public event Action RoundPreparedListener;
        private readonly List<IRoundPreparedHandler> roundPreparedHandlers = new List<IRoundPreparedHandler>();
        private void OnRoundPrepared()
        {
            roundPreparedHandlers.ForEach((e) => e.onRoundPrepared());
            if (RoundPreparedListener != null)
            {
                RoundPreparedListener.Invoke();
            }
        }
        #endregion

        #region <<OnRoundFinished>>
        public event Action RoundFinishedListener;
        private readonly List<IRoundFinishedHandler> roundFinishedHandlers = new List<IRoundFinishedHandler>();
        private void OnRoundFinished()
        {
            roundFinishedHandlers.ForEach((e) => e.onRoundFinished());
            if (RoundFinishedListener != null)
            {
                RoundFinishedListener.Invoke();
            }
        }
        #endregion

        #region <<EventHandler -- ADD RMV>>

        private void AddEventHandler(IUnitEventHandler eh)
        {
            if (eh is IAttackModifier)
            {
                attackModifierHandlers.Add(eh as IAttackModifier);
            }
            if (eh is IAttackFeedback)
            {
                attackFeedbackHandlers.Add(eh as IAttackFeedback);
            }
            if (eh is IKilledHandler)
            {
                killHandlers.Add(eh as IKilledHandler);
            }
            if (eh is IInjuryModifier)
            {
                injuryModifierHandlers.Add(eh as IInjuryModifier);
            }
            if (eh is IInjuryFeedback)
            {
                injuryFeedbackHandlers.Add(eh as IInjuryFeedback);
            }
            if (eh is IDeadHandler)
            {
                deadHandlers.Add(eh as IDeadHandler);
            }
            if (eh is ICureModifier)
            {
                cureModifierHandlers.Add(eh as ICureModifier);
            }
            if (eh is ICureFeedback)
            {
                cureFeedbackHandlers.Add(eh as ICureFeedback);
            }
            if (eh is IHealModifier)
            {
                healModifierHandlers.Add(eh as IHealModifier);
            }
            if (eh is IHealFeedback)
            {
                healFeedbackHandlers.Add(eh as IHealFeedback);
            }
            if (eh is IBeforeAbilityCast)
            {
                beforeCastHandlers.Add(eh as IBeforeAbilityCast);
            }
            if (eh is IAfterAbilityCast)
            {
                afterCastHandlers.Add(eh as IAfterAbilityCast);
            }
            if (eh is IRoundPreparedHandler)
            {
                roundPreparedHandlers.Add(eh as IRoundPreparedHandler);
            }
            if (eh is IRoundFinishedHandler)
            {
                roundFinishedHandlers.Add(eh as IRoundFinishedHandler);
            }
        }

        private void RmvEventHandler(IUnitEventHandler eh)
        {
            if (eh is IAttackModifier)
            {
                attackModifierHandlers.Remove(eh as IAttackModifier);
            }
            if (eh is IAttackFeedback)
            {
                attackFeedbackHandlers.Remove(eh as IAttackFeedback);
            }
            if (eh is IKilledHandler)
            {
                killHandlers.Remove(eh as IKilledHandler);
            }
            if (eh is IInjuryModifier)
            {
                injuryModifierHandlers.Remove(eh as IInjuryModifier);
            }
            if (eh is IInjuryFeedback)
            {
                injuryFeedbackHandlers.Remove(eh as IInjuryFeedback);
            }
            if (eh is IDeadHandler)
            {
                deadHandlers.Remove(eh as IDeadHandler);
            }
            if (eh is ICureModifier)
            {
                cureModifierHandlers.Remove(eh as ICureModifier);
            }
            if (eh is ICureFeedback)
            {
                cureFeedbackHandlers.Remove(eh as ICureFeedback);
            }
            if (eh is IHealModifier)
            {
                healModifierHandlers.Remove(eh as IHealModifier);
            }
            if (eh is IHealFeedback)
            {
                healFeedbackHandlers.Remove(eh as IHealFeedback);
            }
            if (eh is IBeforeAbilityCast)
            {
                beforeCastHandlers.Remove(eh as IBeforeAbilityCast);
            }
            if (eh is IAfterAbilityCast)
            {
                afterCastHandlers.Remove(eh as IAfterAbilityCast);
            }
            if (eh is IRoundPreparedHandler)
            {
                roundPreparedHandlers.Remove(eh as IRoundPreparedHandler);
            }
            if (eh is IRoundFinishedHandler)
            {
                roundFinishedHandlers.Remove(eh as IRoundFinishedHandler);
            }
        }

        #endregion

        public void Attack(Unit target, float dmgBaseValue, int dmgType, int dmgFlag = 0)
        {
            if (target.dead )
            {
                return;
            }
            // -- 0
            var dmg = DamageHelper.CreatDamage(dmgBaseValue, dmgType, dmgFlag);
            dmg.unitId = target.uid;
            // -- 0
            this.OnAttackModifier(dmg, target);
            // -- 1
            target.TakeDamage(dmg, this);
            // -- 2
            this.OnAttackFeedback(dmg, target);
            // -- 3
            if (target.dead)
            {
                this.OnKill(dmg, target);
            }
            // -- 4
            this.battle.recorder.PushDamage(dmg);
        }

        private void TakeDamage(Damage dmg, Unit attacker)
        {
            // -- 0
            dmg.unitId = uid;
            // -- 1
            this.OnInjuryModifier(dmg, attacker);
            // -- 2
            this.ChangeHp(-dmg.value);
            // -- 3
            this.OnInjuryFeedback(dmg, attacker);
            // -- 4
            if (!this.dead)
            {
                this.TryChangeState<InjuryState>();
            }
            else
            {
                this.OnDead(dmg, attacker);
                this.TryChangeState<DeadState>();
            }
        }

        public void Treat(Unit target, int healBaseValue)
        {
            if (target.dead)
            {
                return;
            }
            // -- 0
            var hel = ObjectCache.Get<Healing>();
            hel.baseValue = healBaseValue;
            hel.unitId = target.uid;
            // -- 1
            this.OnCureModifier(hel, target);
            // -- 2
            target.TakeHealing(hel, this);
            // -- 3
            this.OnCureFeedback(hel, target);
            // -- 4
            battle.recorder.PushHealing(hel);
        }

        private void TakeHealing(Healing hel, Unit healer)
        {
            // -- 1
            this.OnHealModifier(hel, healer);
            // -- 2
            this.ChangeHp(hel.value);
            // -- 3
            this.OnHealFeedback(hel, healer);
        }
    }
}