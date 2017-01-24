using UnityEngine;

namespace Assets.MuscleDrop.Scripts.GamaManagers.Stage
{
    /// <summary>
    /// ステージ関係のマネージャ
    /// </summary>
    public class StageManager : MonoBehaviour
    {
        [SerializeField]
        private Transform[] _spawnPoints;

        public Transform[] SpawnPoints { get { return _spawnPoints; } }

    }
}
