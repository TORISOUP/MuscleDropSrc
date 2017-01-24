using UniRx;
using UnityEngine;
using Assets.MuscleDrop.Scripts.Effects;
using Assets.MuscleDrop.Scripts.Attacks;
using Assets.MuscleDrop.Scripts.Attacks.AttackerImpls;
using Assets.MuscleDrop.Scripts.Attacks.BulletImpls;
using Assets.MuscleDrop.Scripts.Damages;
using Assets.MuscleDrop.Scripts.Utilities.ExtensionMethods;
using UniRx.Triggers;

namespace Assets.MuscleDrop.Scripts.Players
{
    public class PlayerWeapon : BasePlayerComponent
    {
        [SerializeField]
        BaseBullet[] _ShowWaveNomalGO;
        [SerializeField]
        BaseBullet _ShowWavePowerUpGO;

        [SerializeField]
        private Collider hipDroCollider;

        public PlayerAttacker PlayerAttacker { get; private set; }

        private ReactiveProperty<bool> _isAttacking = new BoolReactiveProperty(true);

        public IReadOnlyReactiveProperty<bool> IsAttacking { get { return _isAttacking; } }

        protected override void OnInitialize()
        {
            PlayerAttacker = new PlayerAttacker(PlayerId);
            var playerMover = GetComponent<PlayerMover>();
            playerMover.OnHipDropped.DelayFrame(1).Subscribe(_ =>
                {
                    var prefab = _ShowWaveNomalGO[(int)PlayerId - 1];
                    var go = Instantiate(prefab);
                    go.transform.position = transform.position;
                    go.Attacker = PlayerAttacker;
                }).AddTo(gameObject);

            // ヒップドロップ時
            playerMover.IsHipDroping.Subscribe(x =>
            {
                hipDroCollider.enabled = x;
            });

            hipDroCollider.OnTriggerEnterAsObservable()
                .Select(x => x.GetComponent<IDamageApplicable>())
                .Where(x => x != null)
                .Subscribe(x =>
                {
                    var damage = new Damage
                    {
                        Attacker = new PlayerAttacker(PlayerId),
                        Value = 300,
                        Direction = Vector3.down,
                        Type = AttackType.HipDrop
                    };
                    x.ApplyDamage(damage);
                });
        }
    }
}