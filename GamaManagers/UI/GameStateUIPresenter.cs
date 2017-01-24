using System;
using UnityEngine;
using Zenject;
using UniRx;
namespace Assets.MuscleDrop.Scripts.GamaManagers.UI
{
    public class GameStateUIPresenter : MonoBehaviour
    {
        [Inject]
        private MainGameManager mainGameManager;

        [SerializeField]
        private GameObject _readyPanel;

        [SerializeField]
        private GameObject _battlePanel;

        [SerializeField] private GameObject _resultPane;

        void Start()
        {
            mainGameManager.CurrentState
                .Subscribe(state =>
                {
                    SetPanels(state);
                });
        }

        void SetPanels(GameState state)
        {
            switch (state)
            {
                case GameState.Initializing:
                    _readyPanel.SetActive(true);
                    _battlePanel.SetActive(true);
                    _resultPane.SetActive(true);
                    break;
                case GameState.Ready:
                    _readyPanel.SetActive(true);
                    _battlePanel.SetActive(false);
                    _resultPane.SetActive(false);
                    break;
                case GameState.Battle:
                    _battlePanel.SetActive(true);
                    _readyPanel.SetActive(false);
                    break;
                case GameState.Result:
                    _resultPane.SetActive(true);
                    _battlePanel.SetActive(false);
                    break;
                case GameState.Finished:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("state", state, null);
            }
        }
    }
}
