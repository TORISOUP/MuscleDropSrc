using System;
using UnityEngine;
using Assets.MuscleDrop.Scripts.Attacks;
using Assets.MuscleDrop.Scripts.Attacks.AttackerImpls;
using Assets.MuscleDrop.Scripts.Players;
using Assets.MuscleDrop.Scripts.Damages;
using UniRx;

namespace Assets.MuscleDrop.Scripts.Attacks.BulletImpls
{
    public class ChainGimmicBullet : BaseBullet, IDamageApplicable
    {
        [SerializeField]
        BaseBullet[] _ShowWaveGO;
        [SerializeField]
        BaseBullet _ShowWavePowerUpGO;

        private Subject<Damage> damageSubject = new Subject<Damage>();

        void Start()
        {
            damageSubject.ThrottleFirst(TimeSpan.FromMilliseconds(500))
                .Subscribe(d =>
                {
                    var playerId = ((PlayerAttacker)d.Attacker).PlayerId;
                    var prefab = _ShowWaveGO[(int)playerId - 1];
                    var go = Instantiate(prefab);
                    go.transform.position = transform.position;
                    go.Attacker = d.Attacker;
                    Attacker = d.Attacker;
                });
        }

        public void ApplyDamage(Damage damage)
        {
            if (damage.Attacker == Attacker)
                return;

            switch (damage.Type)
            {
                case AttackType.Shockwave:
                    {
                        damageSubject.OnNext(damage);
                    }
                    break;
                case AttackType.HipDrop:
                    break;
                default:
                    break;
            }

        }
    }
}
