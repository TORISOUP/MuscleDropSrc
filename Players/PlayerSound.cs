using System;
using Assets.MuscleDrop.Scripts.Audio;
using UnityEngine;
using UniRx;
using Random = UnityEngine.Random;

namespace Assets.MuscleDrop.Scripts.Players
{
    public class PlayerSound : BasePlayerComponent
    {
        [SerializeField]
        private SoundEffect[] damageSounds;

        [SerializeField]
        private SoundEffect[] jumnpSounds;

        protected override void OnInitialize()
        {
            if (damageSounds.Length != 0)
            {

                Core.OnDamaged
                    .ThrottleFirst(TimeSpan.FromSeconds(1))
                    .Subscribe(_ =>
                {
                    var s = damageSounds[Random.Range(0, damageSounds.Length)];
                    AudioManager.PlaySoundEffect(s);
                });
            }
            var mover = GetComponent<PlayerMover>();
            if (jumnpSounds.Length != 0)
            {
                
                mover.IsJumping
                    .Where(x => x)
                    .Subscribe(_ =>
                    {
                        var s = jumnpSounds[Random.Range(0, jumnpSounds.Length)];
                        AudioManager.PlaySoundEffect(s);
                    });
            }

            mover.OnHipDropped
                .Subscribe(_ =>
                {
                    AudioManager.PlaySoundEffect(SoundEffect.Impact1);
                });
        }
    }
}
