using System;
using System.Collections;
using Assets.MuscleDrop.Scripts.Items;
using UnityEngine;
using UniRx;
namespace Assets.MuscleDrop.Scripts.Players
{
    /// <summary>
    /// アイテム拾った管理
    /// </summary>
    public class PlayerItem : BasePlayerComponent
    {
        private BoolReactiveProperty _isItemEnabled = new BoolReactiveProperty();
        public IReadOnlyReactiveProperty<bool> IsItemEnabled { get { return _isItemEnabled; } }
        public ItemType CurrentItem { get; private set; }

        private Coroutine currentCoroutine;

        protected override void OnInitialize()
        {
            Core.OnPickUpItem
                .Subscribe(item =>
                {
                    ChangePlayerStatus(item);
                });
        }

        private void ChangePlayerStatus(ItemEffect item)
        {
            Core.ResetPlayerParameter();

            if (currentCoroutine != null)
            {
                _isItemEnabled.Value = false;
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(ItemCoroutine(item));
        }

        private IEnumerator ItemCoroutine(ItemEffect item)
        {
            CurrentItem = item.ItemType;
            _isItemEnabled.Value = true;

            Core.SetPlayerParameter(item.PowerUpParameters);

            yield return new WaitForSeconds(item.DurationTime);

            _isItemEnabled.Value = false;
            Core.ResetPlayerParameter();

        }
    }
}
