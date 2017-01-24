using System;
using System.Collections;
using System.Linq;
using Assets.MuscleDrop.Scripts.Audio;
using Assets.MuscleDrop.Scripts.GamaManagers.Stage;
using Assets.MuscleDrop.Scripts.Players;
using Assets.MuscleDrop.Scripts.Utilities;
using Assets.MuscleDrop.Scripts.Utilities.ExtensionMethods;
using UniRx;
using UnityEngine;
using Zenject;

namespace Assets.MuscleDrop.Scripts.GamaManagers
{
    [RequireComponent(typeof(PlayerProvider))]
    [RequireComponent(typeof(GameTimeManager))]
    [RequireComponent(typeof(ScoreManager))]
    [RequireComponent(typeof(ResultManager))]
    public class MainGameManager : MonoBehaviour, IGameStateProvider
    {
        public GameStateReactiveProperty CurrentState
            = new GameStateReactiveProperty(GameState.Initializing);

        public IReadOnlyReactiveProperty<GameState> CurrentGameState
        {
            get { return CurrentState; }
        }

        [SerializeField]
        private int DebugPlayerCount = 4;

        private PlayerProvider playerProvider;
        private GameTimeManager timeManager;
        private ScoreManager scoreManager;
        private ResultManager resultManager;

        [Inject]
        private StageManager stageManager;

        void Start()
        {
            playerProvider = GetComponent<PlayerProvider>();
            timeManager = GetComponent<GameTimeManager>();
            scoreManager = GetComponent<ScoreManager>();
            resultManager = GetComponent<ResultManager>();

            CurrentState.Subscribe(state =>
            {
                state.Red();
                OnStateChanged(state);
            });
        }

        /// <summary>
        /// ステートが変移した
        /// </summary>
        void OnStateChanged(GameState nextState)
        {
            switch (nextState)
            {
                case GameState.Initializing:
                    StartCoroutine(InitializeCoroutine());
                    break;
                case GameState.Ready:
                    StartCoroutine(ReadyCoroutine());
                    break;
                case GameState.Battle:
                    Battle();
                    break;
                case GameState.Result:
                    Result();
                    break;
                case GameState.Finished:
                    Finished();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 初期化コルーチン
        /// </summary>
        /// <returns></returns>
        private IEnumerator InitializeCoroutine()
        {
            var dataPack = SceneLoader.PreviousSceneData as ToBattleSceneDataPack;
            var count = dataPack == null ? DebugPlayerCount : dataPack.PlayerCount;

            var playerIds = Enum.GetValues(typeof(PlayerId))
                .Cast<PlayerId>()
                .Take(count);

            //プレイヤ生成
            for (var i = 0; i < playerIds.Count(); i++)
            {
                var playerId = playerIds.ElementAt(i);
                var position = stageManager.SpawnPoints[i].position;

                var core = playerProvider.CreatePlayer(
                    playerId,
                    position,
                    stageManager.SpawnPoints.Select(x => x.position).ToArray(),
                    this
                );

                //点数計算登録
                scoreManager.Register(core);
            }

            //UI初期化のために１フレーム待機
            yield return null;

            CurrentState.Value = GameState.Ready;
        }

        /// <summary>
        /// 準備完了シーン
        /// </summary>
        private IEnumerator ReadyCoroutine()
        {
            if (SceneLoader.IsTransitioning.Value)
            {
                SceneLoader.Open();
                //シーン遷移中ならトランジションが終わるのを待つ
                yield return SceneLoader.OnTransitionFinished.FirstOrDefault().ToYieldInstruction();
            }

            //Readyタイマを動かして0になったらシーン遷移
            timeManager.ReadyTime
                .FirstOrDefault(x => x == 0)
                .Delay(TimeSpan.FromSeconds(1))
                .Subscribe(_ => CurrentState.Value = GameState.Battle)
                .AddTo(gameObject);
            timeManager.StartGameReadyCountDown();
        }

        private void Battle()
        {
            //BGM再生開始
            AudioManager.PlayBGM(BGM.BGM2, true, 100);

            //メインタイマを動かして0になったらシーン遷移
            timeManager.RemainingTime
                .FirstOrDefault(x => x == 0)
                .Delay(TimeSpan.FromSeconds(2))
                .Subscribe(_ => CurrentState.Value = GameState.Result);
            timeManager.StartBattleCountDown();
        }

        /// <summary>
        /// 結果表示
        /// </summary>
        private void Result()
        {
            AudioManager.StopBGM();
            resultManager.StartResult()
                .Subscribe(_ => CurrentState.Value = GameState.Finished);
        }

        private void Finished()
        {

            SceneLoader.LoadScene(GameScenes.Menu);
        }
    }
}
