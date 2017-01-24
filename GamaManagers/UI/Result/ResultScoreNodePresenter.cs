using System;
using System.Collections;
using Assets.MuscleDrop.Scripts.Players;
using Assets.MuscleDrop.Scripts.Utilities;
using Assets.MuscleDrop.Scripts.Utilities.ExtensionMethods;
using Assets.MuscleDrop.Scripts.Utilities.UI;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.MuscleDrop.Scripts.GamaManagers.UI.Result
{
    /// <summary>
    /// 最終結果の列
    /// </summary>
    public class ResultScoreNodePresenter : MonoBehaviour
    {
        [SerializeField]
        private Text _playerName;

        [SerializeField]
        private RichOutline _richOutline;

        [SerializeField]
        private Text _killText;
        [SerializeField]
        private Text _deadText;
        [SerializeField]
        private Text _suicideText;
        [SerializeField]
        private Text _rankingText;

        [SerializeField]
        private ElementShake _rankingTextShaker;

        private PlayerId _myId;
        private ResultScore _resultScore;

        private ResultManager _resultManager;

        void Start()
        {
            _killText.text = "";
            _deadText.text = "";
            _suicideText.text = "";
            _rankingText.text = "";

            _resultManager.OnScoreAsObservable
                .FirstOrDefault(x => x.PlayerId == _myId)
                .Subscribe(x =>
                {
                    _resultScore = x;
                });

            //各要素の表示命令が来たら表示する
            _resultManager.OnShowKillScoreAsObservable
                .Subscribe(_ => _killText.text = string.Format("{0}", _resultScore.KillCount.ToString("+#;-#;0")));

            _resultManager.OnShowDeadScoreAsObservable
                .Subscribe(_ => _deadText.text = string.Format("{0}", _resultScore.DeadCount.ToString("-#;+#;0")));

            _resultManager.OnShowSuicideScoreAsObservable
                .Subscribe(_ => _suicideText.text = string.Format("{0}", _resultScore.SuicideCount.ToString("-#;+#;0")));

            //ランキングは自分のタイミングがきたら表示する
            _resultManager.OnShowRankingAsObservable
                .FirstOrDefault(x => x == _resultScore.Rank)
                .Subscribe(_ =>
                {
                    _rankingText.text = string.Format("{0}<size=100>位</size>", _resultScore.Rank);
                    _rankingTextShaker.ShakePosition(30, 10.0f);
                    _rankingTextShaker.ShakeRotation(10, 10.0f);
                });

        }

        public void Initialize(PlayerId id, ResultManager resultManager)
        {
            _myId = id;

            _playerName.text = _myId.ToName();
            _richOutline.effectColor = _myId.ToColor();

            _resultManager = resultManager;
        }


    }
}
