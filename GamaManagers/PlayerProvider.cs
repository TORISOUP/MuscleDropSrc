using Assets.MuscleDrop.Scripts.Players;
using UniRx;
using UnityEngine;

namespace Assets.MuscleDrop.Scripts.GamaManagers
{
    /// <summary>
    /// Playerを生成する
    /// </summary>
    public class PlayerProvider : MonoBehaviour
    {
        [SerializeField]
        private GameObject _playerPrefab;

        private ReactiveDictionary<PlayerId, PlayerCore> _players = new ReactiveDictionary<PlayerId, PlayerCore>();

        /// <summary>
        /// 現在のPlayer
        /// </summary>
        public IReadOnlyReactiveDictionary<PlayerId, PlayerCore> Players
        {
            get { return _players; }
        }

        public PlayerCore CreatePlayer(PlayerId id, Vector3 position, Vector3[] respawnPositions, IGameStateProvider gameStateProvider)
        {
            var go = Instantiate(_playerPrefab, position, Quaternion.LookRotation(Vector3.back));
            var core = go.GetComponent<PlayerCore>();
            core.InitializePlayer(id, respawnPositions, gameStateProvider);
            _players.Add(id, core);
            return core;
        }
    }
}
