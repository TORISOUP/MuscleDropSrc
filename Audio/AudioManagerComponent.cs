using Assets.MuscleDrop.Scripts.Utilities;
using Assets.MuscleDrop.Scripts.Utilities.ExtensionMethods;
using UnityEngine;

namespace Assets.MuscleDrop.Scripts.Audio
{
    public class AudioManagerComponent : SingletonMonoBehaviour<AudioManagerComponent>
    {
        [SerializeField]
        private CriAtomSource bgmPlayer;

        [SerializeField]
        private CriAtomSource soundEffectPlayer;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        #region BGM
        private BGM currentBGM;

        /// <summary>
        /// BGMを再生する
        /// </summary>
        public void PlayBGM(BGM bgm, bool isLoop, float fadeoutMill = 1000.0f)
        {
            if (IsPlayingBGM && bgm == currentBGM)
            {
                //既に同じBGMを再生しているときは何もしない
                return;
            }
            bgmPlayer.player.SetEnvelopeReleaseTime(fadeoutMill);
            bgmPlayer.Play(bgm, isLoop);
            currentBGM = bgm;
        }

        /// <summary>
        /// BGMを停止する
        /// </summary>
        public void StopBGM()
        {
            bgmPlayer.Stop();
        }

        /// <summary>
        /// BGMを再生中か？
        /// </summary>
        public bool IsPlayingBGM
        {
            get
            {
                var status = bgmPlayer.player.GetStatus();
                return status == CriAtomExPlayer.Status.Prep || status == CriAtomExPlayer.Status.Playing;
            }
        }
        #endregion


        #region SoundEffect

        public void PlaySoundEffect(SoundEffect soundEffect)
        {
            soundEffectPlayer.Play(soundEffect);
        }

        /// <summary>
        /// SEを停止する
        /// </summary>
        public void StopSE()
        {
            soundEffectPlayer.Stop();
        }
        #endregion
    }
}
