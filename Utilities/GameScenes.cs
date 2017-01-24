using UnityEngine;

namespace Assets.MuscleDrop.Scripts.Utilities
{
    public enum GameScenes
    {
        Title,
        Menu,
        BattleManager,
        StageColosseum,
        Stage2,
        Stage3,
    }

    /// <summary>
    /// シーンをまたいでデータを受け渡すときに利用する
    /// </summary>
    public abstract class SceneDataPack
    {
        /// <summary>
        /// 前のシーン
        /// </summary>
        public abstract GameScenes PreviousGameScene { get; }
    }

    public class DefaultSceneDataPack : SceneDataPack
    {
        private readonly GameScenes _prevGameScenes;

        public override GameScenes PreviousGameScene
        {
            get { return _prevGameScenes; }
        }

        public DefaultSceneDataPack(GameScenes prev)
        {
            _prevGameScenes = prev;
        }
    }

    public class ToBattleSceneDataPack : SceneDataPack
    {
        private readonly GameScenes _prevGameScenes;
        public int PlayerCount { get; private set; }
        public override GameScenes PreviousGameScene
        {
            get { return _prevGameScenes; }
        }

        public ToBattleSceneDataPack(GameScenes prev, int playerCount)
        {
            _prevGameScenes = prev;
            PlayerCount = playerCount;
        }
    }
}
