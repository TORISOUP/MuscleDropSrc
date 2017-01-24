using Assets.MuscleDrop.Scripts.Audio;
using UnityEngine;
using Zenject;
using UniRx;
namespace Assets.MuscleDrop.Scripts.GamaManagers
{
    public class BattleSoundManager : MonoBehaviour
    {
        [Inject]
        private GameTimeManager timerManager;

        [Inject]
        private ResultManager resultManager;

        [Inject] private MainGameManager gameManager;

        void Start()
        {
            timerManager.ReadyTime
                .Where(_=>gameManager.CurrentState.Value == GameState.Ready)
                .Subscribe(x =>
            {
                if (x > 0)
                {
                    AudioManager.PlaySoundEffect(SoundEffect.BattleCountDown);
                }
                else
                {
                    AudioManager.PlaySoundEffect(SoundEffect.BattleStart);
                }
            });


            timerManager.RemainingTime.FirstOrDefault(x => x == 0)
                .Subscribe(_ => AudioManager.PlaySoundEffect(SoundEffect.BattleEnd));

            timerManager.RemainingTime.Where(x => x <= 10 && x > 0)
                .Subscribe(_ => AudioManager.PlaySoundEffect(SoundEffect.BattleEndCountDown));

            resultManager.OnShowDeadScoreAsObservable
                .Subscribe(_ => AudioManager.PlaySoundEffect(SoundEffect.BattleResultOpen));
            resultManager.OnShowKillScoreAsObservable
             .Subscribe(_ => AudioManager.PlaySoundEffect(SoundEffect.BattleResultOpen));
            resultManager.OnShowSuicideScoreAsObservable
                .Subscribe(_ => AudioManager.PlaySoundEffect(SoundEffect.BattleResultOpen));

            resultManager.OnShowRankingAsObservable
                .Subscribe(x =>
                {
                    if (x == 1)
                    {
                        AudioManager.PlaySoundEffect(SoundEffect.BattleResultRank1);
                    }
                    else
                    {
                        AudioManager.PlaySoundEffect(SoundEffect.BattleResultRank2);
                    }
                });
        }

    }
}
