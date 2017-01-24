using System;
using System.Collections;
using System.Linq;
using Assets.MuscleDrop.Scripts.Utilities.ExtensionMethods;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.MuscleDrop.Scripts.Utilities
{
    /// <summary>
    /// シーン遷移を管理する
    /// </summary>
    public class TransitionManager : SingletonMonoBehaviour<TransitionManager>
    {
        private EMTransition transitionComponent;
        private RawImage image;

        private GameScenes _currentGameScene;

        /// <summary>
        /// 現在のシーン
        /// </summary>
        public GameScenes CurrentGameScene
        {
            get { return _currentGameScene; }
        }

        ///
        private ReactiveProperty<bool> ToMoveFlag = new ReactiveProperty<bool>(false);

        /// <summary>
        /// トランザクション状態の内部通知用
        /// </summary>
        private Subject<Unit> onTransactionStartInternal = new Subject<Unit>();

        /// <summary>
        /// トランザクション状態の内部通知用
        /// </summary>
        private Subject<Unit> onTransactionFinishedInternal = new Subject<Unit>();

        /// <summary>
        /// トランジションが終了しシーンが開始したことを通知する
        /// </summary>
        private Subject<Unit> onTransitionAnimationFinishedSubject = new Subject<Unit>();

        private Subject<Unit> onAllSceneLoaded = new Subject<Unit>();

        /// <summary>
        /// 全シーンのロードが完了したことを通知する
        /// </summary>
        public IObservable<Unit> OnScenesLoaded { get { return onAllSceneLoaded; } }

        /// <summary>
        /// トランジションが終了し、シーンが開始したことを通知する
        /// OnCompletedもセットで発行する
        /// </summary>
        public IObservable<Unit> OnTransitionAnimationFinished
        {
            get { return onTransitionAnimationFinishedSubject.FirstOrDefault(); }
        }

        private ReactiveProperty<bool> _isTransitioning = new BoolReactiveProperty();
        /// <summary>
        /// シーン遷移処理を実行中か
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsTransitioning { get { return _isTransitioning; } }

        /// <summary>
        /// AutMoveにfalseを指定した際に、トランジションアニメーションを終了させる
        /// </summary>
        public void Open()
        {
            ToMoveFlag.Value = true;
        }

        private void Awake()
        {
            try
            {
                _currentGameScene =
                    (GameScenes)Enum.Parse(typeof(GameScenes), SceneManager.GetActiveScene().name, false);
            }
            catch
            {
                _currentGameScene = GameScenes.Title;
            }
        }

        private void Start()
        {
            transitionComponent = GetComponent<EMTransition>();
            image = GetComponent<RawImage>();
            image.raycastTarget = false; //イベント貫通
            transitionComponent.flipAfterAnimation = true;
            transitionComponent.onTransitionStart.AddListener(() => onTransactionStartInternal.OnNext(Unit.Default));
            transitionComponent.onTransitionComplete.AddListener(() => onTransactionFinishedInternal.OnNext(Unit.Default));


            //生成時に発行する（デバッグ用）
            onAllSceneLoaded.OnNext(Unit.Default);
        }

        /// <summary>
        /// シーン遷移を実行する
        /// </summary>
        /// <param name="nextScene">次のシーン</param>
        /// <param name="data">次のシーンへ引き継ぐデータ</param>
        /// <param name="additiveLoadScenes">追加ロードするシーン</param>
        /// <param name="autoMove">トランジションの自動遷移を行うか</param>
        public void StartTransaction(GameScenes nextScene,
            SceneDataPack data,
            GameScenes[] additiveLoadScenes,
            bool autoMove
            )
        {
            StartCoroutine(TransitionCoroutine(nextScene, data, additiveLoadScenes, autoMove));
        }

        private IEnumerator TransitionCoroutine(
            GameScenes nextScene,
            SceneDataPack data,
            GameScenes[] additiveLoadScenes,
             bool autoMove
            )
        {
            ToMoveFlag.Value = autoMove;
            _isTransitioning.Value = true;
            while (transitionComponent == null)
            {
                yield return new WaitForSeconds(0.2f);
                transitionComponent.threshold = 0;
            }
            //タッチイベントを吸収させる
            image.raycastTarget = true;
            transitionComponent.flip = false;
            transitionComponent.Play();
            yield return onTransactionFinishedInternal.FirstOrDefault().StartAsCoroutine();
            transitionComponent.threshold = 1;

            SceneLoader.PreviousSceneData = data;

            //シーン遷移
            yield return SceneManager.LoadSceneAsync(nextScene.ToString(), LoadSceneMode.Single);

            //追加シーンがある場合は一緒に読み込む
            if (additiveLoadScenes != null)
            {
                yield return additiveLoadScenes.Select(scene =>
                    SceneManager.LoadSceneAsync(scene.ToString(), LoadSceneMode.Additive)
                        .ObserveEveryValueChanged(x => x.isDone)
                        .FirstOrDefault(x => x)
                    ).WhenAll().StartAsCoroutine();
            }
            Resources.UnloadUnusedAssets();
            GC.Collect();

            onAllSceneLoaded.OnNext(Unit.Default);
            if (!autoMove)
            {
                //自動遷移しないなら待機
                yield return ToMoveFlag.FirstOrDefault(x => x).StartAsCoroutine();
            }
            ToMoveFlag.Value = false;

            //反転
            transitionComponent.Play();
            yield return onTransactionFinishedInternal.FirstOrDefault().StartAsCoroutine();
            image.raycastTarget = false;
            transitionComponent.threshold = 0;
            onTransitionAnimationFinishedSubject.OnNext(Unit.Default);
            _isTransitioning.Value = false;
        }
    }
}