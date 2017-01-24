using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Zenject;
using UniRx;

namespace Assets.MuscleDrop.Scripts.GamaManagers.UI.Battle
{
    /// <summary>
    /// 生成されたPlayerをUIに紐付ける
    /// </summary>
    public class PlayerStatusPanelDispatcher : MonoBehaviour
    {
        [Inject]
        PlayerProvider playerProvider;

        [SerializeField]
        private Transform root;
        [SerializeField]
        private GameObject _playerStatePanel;

        void Start()
        {
            playerProvider.Players
                .ObserveAdd()
                .Select(x => x.Value)
                .StartWith(playerProvider.Players.AsEnumerable().Select(x => x.Value))
                .Subscribe(p =>
                {
                    var panel = Instantiate(_playerStatePanel);
                    var presenter = panel.GetComponent<PlayerStatusPanelPresenter>();
                    presenter.Initialize(p);
                    panel.transform.SetParent(root, false);
                });
        }
    }
}
