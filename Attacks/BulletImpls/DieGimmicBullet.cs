using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Assets.MuscleDrop.Scripts.Attacks;
using Assets.MuscleDrop.Scripts.Attacks.AttackerImpls;
using Assets.MuscleDrop.Scripts.Players;
using Assets.MuscleDrop.Scripts.Damages;

namespace Assets.MuscleDrop.Scripts.Attacks.BulletImpls
{
    public class DieGimmicBullet : BaseBullet
    {
        void Start()
        {
            this.OnCollisionEnterAsObservable()
                .Subscribe(x =>
                    {
                        var dieable = x.gameObject.GetComponent<IDieable>();
                        if (dieable != null)
                        {
                            dieable.Kill(NonPlayerAttacker.Default);
                        }
                    });
        }
    }
}
