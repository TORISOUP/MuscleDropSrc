using Assets.MuscleDrop.Scripts.Attacks;

namespace Assets.MuscleDrop.Scripts.Damages
{
    public interface IDieable
    {
        void Kill(IAttacker attacker);
    }
}
