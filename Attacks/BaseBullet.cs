using UniRx.Triggers;
using UnityEngine;

namespace Assets.MuscleDrop.Scripts.Attacks
{
    public abstract class BaseBullet : MonoBehaviour
    {
        public IAttacker Attacker { get; set; }

        protected float DamagePower { get; set; }
    }
}
