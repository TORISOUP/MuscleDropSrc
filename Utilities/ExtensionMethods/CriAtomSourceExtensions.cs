using Assets.MuscleDrop.Scripts.Audio;
using UniRx;

namespace Assets.MuscleDrop.Scripts.Utilities.ExtensionMethods
{
    public static class CriAtomSourceExtensions
    {

        /// <summary>
        /// BGMを再生する
        /// </summary>
        /// <param name="source"></param>
        /// <param name="bgms"></param>
        public static void Play(this CriAtomSource source, BGM bgms, bool isLoop)
        {
            source.loop = isLoop;
            source.Play(bgms.ToString());
        }

        public static void Play(this CriAtomSource source, SoundEffect soundEffect)
        {
            source.Play(soundEffect.ToString());
        }
    }
}