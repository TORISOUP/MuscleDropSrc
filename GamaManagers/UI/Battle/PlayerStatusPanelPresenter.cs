using Assets.MuscleDrop.Scripts.Players;
using Assets.MuscleDrop.Scripts.Utilities;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.MuscleDrop.Scripts.GamaManagers.UI.Battle
{
    /// <summary>
    /// プレイヤの状態を表示する
    /// </summary>
    public class PlayerStatusPanelPresenter : MonoBehaviour
    {
        [SerializeField]
        private Text _playerName;
        [SerializeField]
        private RichOutline _richOutline;
        [SerializeField]
        private Image _deadImage;

        private PlayerCore _targetPlayerCore;

        public void Initialize(PlayerCore core)
        {
            _targetPlayerCore = core;

            _playerName.text = core.PlayerId.ToName();
            _richOutline.effectColor = core.PlayerId.ToColor();

            _targetPlayerCore.IsAlive
                .Subscribe(x => _deadImage.enabled = !x);
        }

    }
}
