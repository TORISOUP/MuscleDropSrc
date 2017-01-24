using System;
using System.Linq;
using Assets.MuscleDrop.Scripts.Audio;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.EventSystems;
using Assets.MuscleDrop.Scripts.Utilities;
using Assets.MuscleDrop.Scripts.Utilities.ExtensionMethods;

namespace Assets.MuscleDrop.Scripts.Menus.UI
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField]
        private EventSystem eventSystem;

        [SerializeField]
        private GameObject playerCanvas;

        [SerializeField]
        private PlayerInfo[] playerInfo;

        [SerializeField]
        private GameObject stageCanvas;

        [SerializeField]
        private StageInfo[] stageInfo;

        private enum State
        {
            SelectingPlayer,
            SelectingStage,
            Starting,
        }

        private State currentState = State.SelectingPlayer;

        private int selectedPlayerNum;
        private GameScenes selectedStageScene;

        // 初期化
        private void Start()
        {
            AudioManager.PlayBGM(BGM.BGM1, true, 2000);

            SetState(State.SelectingPlayer);

            //foreach (var info in playerInfo)
            //{
            //    info.Button.OnClickAsObservable()
            //        .ThrottleFirst(TimeSpan.FromSeconds(1))
            //        .Subscribe(_ =>
            //        {
            //            selectedPlayerNum = info.Num;
            //            SetState(State.SelectingStage);
            //            AudioManager.PlaySoundEffect(SoundEffect.UIEnter);
            //        });
            //}

            playerInfo
                .ToObservable()
                .SelectMany(x => x.Button.OnClickAsObservable().Select(_ => x.Num))
                .ThrottleFirst(TimeSpan.FromSeconds(2))
                .Subscribe(num =>
                {
                    selectedPlayerNum = num;
                    SetState(State.SelectingStage);
                    AudioManager.PlaySoundEffect(SoundEffect.UIEnter);
                }).AddTo(this);


            stageInfo
                .ToObservable()
                .SelectMany(x => x.Button.OnClickAsObservable().Select(_ => x.GameScene).FirstOrDefault())
                .FirstOrDefault()
                .Subscribe(scene =>
                {
                    selectedStageScene = scene;
                    SetState(State.Starting);
                    AudioManager.PlaySoundEffect(SoundEffect.UIEnter);
                }).AddTo(this);

            //foreach (var info in stageInfo)
            //{
            //    info.Button.OnClickAsObservable()
            //        .FirstOrDefault()
            //        .Subscribe(_ =>
            //        {
            //            selectedStageScene = info.GameScene;
            //            SetState(State.Starting);
            //            AudioManager.PlaySoundEffect(SoundEffect.UIEnter);
            //        });
            //}

            this.UpdateAsObservable()
                .Subscribe(_ => UpdateOutline());

            eventSystem.ObserveEveryValueChanged(x => x.currentSelectedGameObject)
                .Where(x => x != null)
                .Skip(1)
                .Subscribe(x => AudioManager.PlaySoundEffect(SoundEffect.UISelect));

            eventSystem.ObserveEveryValueChanged(x => x.currentSelectedGameObject)
                .Where(x => x == null)
                .Subscribe(_ =>
                {
                    if (currentState == State.SelectingPlayer)
                    {
                        eventSystem.SetSelectedGameObject(playerInfo.First().Button.gameObject);
                    }
                    else if (currentState == State.SelectingStage)
                    {
                        eventSystem.SetSelectedGameObject(stageInfo.First().Button.gameObject);
                    }
                });
        }

        // アウトライン更新
        private void UpdateOutline()
        {
            switch (currentState)
            {
                case State.SelectingPlayer:
                    SetPlayerOutline();
                    break;

                case State.SelectingStage:
                    SetSelectOutline();
                    break;
            }
        }

        // 選択中のアイコンのアウトライン設定
        private void SetPlayerOutline()
        {
            foreach (var info in playerInfo)
            {
                var outline = info.Button.GetComponent<RichOutline>();
                if (outline == null)
                {
                    continue;
                }

                outline.enabled = eventSystem.currentSelectedGameObject == outline.gameObject;
            }
        }

        // 選択中のアイコンのアウトライン設定
        private void SetSelectOutline()
        {
            foreach (var info in stageInfo)
            {
                var outline = info.Outline.GetComponent<RichOutline>();
                if (outline == null)
                {
                    continue;
                }

                outline.enabled = eventSystem.currentSelectedGameObject == outline.gameObject;
            }
        }

        // 内部状態設定
        private void SetState(State state)
        {
            switch (state)
            {
                case State.SelectingPlayer:
                    playerCanvas.SetActive(true);
                    stageCanvas.SetActive(false);
                    eventSystem.SetSelectedGameObject(playerInfo[0].Button.gameObject);
                    break;

                case State.SelectingStage:
                    playerCanvas.SetActive(false);
                    stageCanvas.SetActive(true);
                    eventSystem.SetSelectedGameObject(stageInfo[0].Button.gameObject);
                    break;

                case State.Starting:
                    playerCanvas.SetActive(false);
                    stageCanvas.SetActive(true);

                    ("Player Num : " + selectedPlayerNum).Red();
                    ("Selected Stage : " + selectedStageScene).Red();

                    // ゲーム開始
                    SceneLoader.LoadScene(selectedStageScene,
                        new ToBattleSceneDataPack(GameScenes.Menu, selectedPlayerNum),
                        new GameScenes[] {
                        GameScenes.BattleManager
                        }, autoMove: false);

                    AudioManager.StopBGM();
                    break;
            }

            currentState = state;
        }
    }
}