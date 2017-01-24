using Assets.MuscleDrop.Scripts.Effects;
using UniRx;
using UnityEngine;

namespace Assets.MuscleDrop.Scripts.Players
{
    public class PlayerEffect : BasePlayerComponent
    {

        [SerializeField]
        private GameObject _damageEffectGO;
        [SerializeField]
        private GameObject _deadEffectGO;

        protected override void OnInitialize()
        {
            Core.OnDamaged.Subscribe(x => 
                {
                    _damageEffectGO.SetActive(true);
                });
            
            Core.OnDead.Subscribe(x => 
                {
                    _deadEffectGO.SetActive(true);
                });
        }
    }
}