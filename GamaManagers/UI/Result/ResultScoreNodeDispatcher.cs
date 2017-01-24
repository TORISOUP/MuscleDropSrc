using System.Linq;
using Assets.MuscleDrop.Scripts.GamaManagers.UI.Battle;
using Assets.MuscleDrop.Scripts.Utilities.ExtensionMethods;
using UniRx;
using UnityEngine;
using Zenject;

namespace Assets.MuscleDrop.Scripts.GamaManagers.UI.Result
{
    public class ResultScoreNodeDispatcher : MonoBehaviour
    {
        [SerializeField]
        private GameObject _nodePrefab;

        [SerializeField]
        private Transform _root;

        [Inject]
        private PlayerProvider _playerProvider;

        [Inject]
        private ResultManager _resultManager;

        void Start()
        {

            _playerProvider.Players
                .ObserveAdd()
                .Select(x => x.Value)
                .StartWith(_playerProvider.Players.AsEnumerable().Select(x => x.Value))
                .Subscribe(p =>
                {
                    var panel = Instantiate(_nodePrefab);
                    var presenter = panel.GetComponent<ResultScoreNodePresenter>();
                    presenter.Initialize(p.PlayerId, _resultManager);
                    panel.transform.SetParent(_root, false);
                });

        }

    }
}
