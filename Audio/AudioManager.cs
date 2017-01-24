using UnityEngine;

namespace Assets.MuscleDrop.Scripts.Audio
{
    public static class AudioManager
    {

        private static AudioManagerComponent _audioManagerComponent;

        private static AudioManagerComponent Manager
        {
            get
            {
                if (_audioManagerComponent != null) return _audioManagerComponent;
                if (AudioManagerComponent.Instance == null)
                {
                    var resource = Resources.Load("Audio/AudioManager");
                    UnityEngine.Object.Instantiate(resource);
                }
                _audioManagerComponent = AudioManagerComponent.Instance;
                return _audioManagerComponent;
            }
        }


        public static AudioManagerComponent PreLoad()
        {
            return Manager;
        }


        /// <summary>
        /// BGMを再生する
        /// </summary>
        /// <param name="bgm">再生したいBGM</param>
        /// <param name="isLoop">ループさせるか</param>
        /// <param name="fadeoutMill">終了時のフェードアウト時間</param>
        public static void PlayBGM(BGM bgm, bool isLoop, float fadeoutMill = 100.0f)
        {
            Manager.PlayBGM(bgm, isLoop, fadeoutMill);
        }

        /// <summary>
        /// BGMを停止する
        /// </summary>
        public static void StopBGM()
        {
            Manager.StopBGM();
        }

        /// <summary>
        /// SEを再生する
        /// </summary>
        public static void PlaySoundEffect(SoundEffect soundEffect)
        {
            Manager.PlaySoundEffect(soundEffect);
        }
    }



}
