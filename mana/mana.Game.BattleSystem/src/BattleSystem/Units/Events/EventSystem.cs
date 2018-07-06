using BattleSystem.Units.Abilities;
using xxd.battle;

namespace BattleSystem.Units.Events
{
    public interface IUnitEventHandler
    {
    }

    public interface IAttackModifier : IUnitEventHandler
    {
        void onAttackModifier(Damage dmg, Unit unit);
    }

    public interface IAttackFeedback : IUnitEventHandler
    {
        void onAttackFeedback(Damage dmg, Unit unit);
    }

    public interface IKilledHandler : IUnitEventHandler
    {
        void onKill(Damage dmg, Unit unit);
    }

    public interface IInjuryModifier : IUnitEventHandler
    {
        void onInjuryModifier(Damage dmg, Unit unit);
    }

    public interface IInjuryFeedback : IUnitEventHandler
    {
        void onInjuryFeedback(Damage dmg, Unit unit);
    }

    public interface IDeadHandler : IUnitEventHandler
    {
        void onDead(Damage dmg, Unit unit);
    }

    public interface ICureModifier : IUnitEventHandler
    {
        void onCureModifier(Healing hel, Unit unit);
    }

    public interface ICureFeedback : IUnitEventHandler
    {
        void onCureFeedback(Healing hel, Unit unit);
    }

    public interface IHealModifier : IUnitEventHandler
    {
        void onHealModifier(Healing hel, Unit unit);
    }

    public interface IHealFeedback : IUnitEventHandler
    {
        void onHealFeedback(Healing hel, Unit unit);
    }

    public interface IBeforeAbilityCast : IUnitEventHandler
    {
        void beforeCast(Ability skill);
    }

    public interface IAfterAbilityCast : IUnitEventHandler
    {
        void afterCast(Ability skill);
    }

    public interface IRoundFinishedHandler : IUnitEventHandler
    {
        void onRoundFinished();
    }

    public interface IRoundPreparedHandler : IUnitEventHandler
    {
        void onRoundPrepared();
    }
}
