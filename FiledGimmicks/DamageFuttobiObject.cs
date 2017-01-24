using Assets.MuscleDrop.Scripts.Damages;
using UniRx;
using UnityEngine;

namespace Assets.MuscleDrop.Scripts.FiledGimmicks
{
    public class DamageFuttobiObject : MonoBehaviour, IDamageApplicable
    {
        [SerializeField]
        private float FuttobiRate = 1.0f;
        private Rigidbody rigidbody;
        void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        public void ApplyDamage(Damage damage)
        {
            Observable.NextFrame(FrameCountType.FixedUpdate)
                .Subscribe(_ =>
                {
                    rigidbody.AddForce(damage.Direction * damage.Value * FuttobiRate, ForceMode.VelocityChange);
                });
        }
    }
}
