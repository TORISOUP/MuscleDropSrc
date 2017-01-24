using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.MuscleDrop.Scripts.Attacks.AttackerImpls;
using Assets.MuscleDrop.Scripts.Players;
using UniRx;
using UnityEngine;
using Zenject;

namespace Assets.MuscleDrop.Scripts.GamaManagers
{
    /// <summary>
    /// 試合終了時の結果表示を処理する
    /// </summary>
    public class ResultManager : MonoBehaviour
    {
        private ScoreManager _scoreManager;

        private PlayerProvider _playerProvider;

        private Subject<ResultScore> _scoreSubject = new Subject<ResultScore>();
        public IObservable<ResultScore> OnScoreAsObservable { get { return _scoreSubject; } }

        //画面に描画するタイミングを通知する
        private AsyncSubject<Unit> _killAsyncSubject = new AsyncSubject<Unit>();
        public IObservable<Unit> OnShowKillScoreAsObservable { get { return _killAsyncSubject; } }

        private AsyncSubject<Unit> _deadAsyncSubject = new AsyncSubject<Unit>();
        public IObservable<Unit> OnShowDeadScoreAsObservable { get { return _deadAsyncSubject; } }

        private AsyncSubject<Unit> _suicideAsyncSubject = new AsyncSubject<Unit>();
        public IObservable<Unit> OnShowSuicideScoreAsObservable { get { return _suicideAsyncSubject; } }

        private Subject<int> _rankingSubject = new Subject<int>();
        public IObservable<int> OnShowRankingAsObservable { get { return _rankingSubject; } }

        void Start()
        {
            _scoreManager = GetComponent<ScoreManager>();
            _playerProvider = GetComponent<PlayerProvider>();
        }

        /// <summary>
        /// Resultへ移行
        /// </summary>
        public IObservable<Unit> StartResult()
        {
            //点数計算

            var kills = _scoreManager.GetPlayerKillSocre(); 
            var deads = _scoreManager.GetPlayerDeadReasons();

            // <PlayerId,IEnumerable<IAttacker> を分解して
            // PlayerIdごとのスコアを計算する
            var list = (from id in _playerProvider.Players.Select(x => x.Key)
                        let k = kills.ContainsKey(id) ? kills[id].Count() : 0
                        let d = deads.ContainsKey(id) ? deads[id].Count(x => x is PlayerAttacker) : 0
                        let s = deads.ContainsKey(id) ? deads[id].Count(x => x is NonPlayerAttacker) : 0
                        select new ResultScoreDraft(id, k, d, s)).ToList();

            //スコアが大きい方から降順にソート
            var sortedList = list.OrderByDescending(x => x.TotalScore).ToArray();

            var finalResults = new List<ResultScore>();
            foreach (var id in _playerProvider.Players.Select(x => x.Key))
            {
                var d = sortedList.First(x => x.PlayerId == id);
                var myscore = d.TotalScore;

                //スコアの大きさから順位を決定
                var rank = sortedList.TakeWhile(x => x.TotalScore > myscore).Count() + 1;

                //順位を含めた最終結果
                var result = new ResultScore(d.PlayerId, d.KillCount, d.DeadCount, d.SuicideCount, rank);
                _scoreSubject.OnNext(result);
                finalResults.Add(result);
            }

            return Observable.FromCoroutine(_ => ResultCoroutine(finalResults));
        }

        /// <summary>
        /// 結果表示コルーチン
        /// </summary>
        /// <returns></returns>
        private IEnumerator ResultCoroutine(IEnumerable<ResultScore> results)
        {
            yield return new WaitForSeconds(0.75f);

            _killAsyncSubject.OnNext(Unit.Default);
            _killAsyncSubject.OnCompleted();

            yield return new WaitForSeconds(0.75f);
            _deadAsyncSubject.OnNext(Unit.Default);
            _deadAsyncSubject.OnCompleted();

            yield return new WaitForSeconds(0.75f);
            _suicideAsyncSubject.OnNext(Unit.Default);
            _suicideAsyncSubject.OnCompleted();

            //順位のユニーク数だけループを回す
            var rankList = results.Select(x => x.Rank).Distinct().OrderByDescending(x => x);
            foreach (var i in rankList)
            {
                yield return new WaitForSeconds(1);
                _rankingSubject.OnNext(i);
            }
            yield return new WaitForSeconds(3);
        }

        private struct ResultScoreDraft
        {
            public PlayerId PlayerId { get; private set; }
            public int KillCount { get; private set; }
            public int DeadCount { get; private set; }
            public int SuicideCount { get; private set; }

            public int TotalScore
            {
                get { return KillCount - DeadCount - SuicideCount; }
            }

            public ResultScoreDraft(PlayerId playerId, int killCount, int deadCount, int suicideCount) : this()
            {
                PlayerId = playerId;
                KillCount = killCount;
                DeadCount = deadCount;
                SuicideCount = suicideCount;
            }
        }
    }
}
