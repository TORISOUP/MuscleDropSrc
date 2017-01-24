using System.Collections;
using Assets.MuscleDrop.Scripts.Audio;
using Assets.MuscleDrop.Scripts.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.MuscleDrop.Scripts.Title
{
    public class TitleComponent : MonoBehaviour
    {
        [SerializeField]
        private RawImage ggjImage;
        [SerializeField]
        private Image gameLogo;
        [SerializeField]
        private Image unityLogo;
        [SerializeField]
        private Image adxLogo;

        void Start()
        {
            AudioManager.PreLoad();
            StartCoroutine(TitleCoroutine());

            Application.targetFrameRate = 60;
        }

        IEnumerator TitleCoroutine()
        {
            yield return new WaitForSeconds(0.1f);
            #region GGJ
            var n = 0;
            while (ggjImage.color.a < 1)
            {
                n++;
                var na = Mathf.Lerp(0, 1, n * Time.deltaTime);
                ggjImage.color = new Color(1, 1, 1, na);
                yield return null;
            }

            yield return new WaitForSeconds(1);

            n = 0;
            while (ggjImage.color.a > 0)
            {
                n++;
                var na = Mathf.Lerp(1, 0, n * Time.deltaTime);
                ggjImage.color = new Color(1, 1, 1, na);
                yield return null;
            }
            #endregion

            #region AdxUnity
            n = 0;
            while (adxLogo.color.a < 1)
            {
                n++;
                var na = Mathf.Lerp(0, 1, n * Time.deltaTime);
                adxLogo.color = new Color(1, 1, 1, na);
                unityLogo.color = new Color(1, 1, 1, na);
                yield return null;
            }

            yield return new WaitForSeconds(1);

            n = 0;
            while (adxLogo.color.a > 0)
            {
                n++;
                var na = Mathf.Lerp(1, 0, n * Time.deltaTime);
                adxLogo.color = new Color(1, 1, 1, na);
                unityLogo.color = new Color(1, 1, 1, na);
                yield return null;
            }
            #endregion

            yield return new WaitForSeconds(1.0f);

            gameLogo.color = Color.white;
            AudioManager.PlaySoundEffect(SoundEffect.BattleStart);

            yield return new WaitForSeconds(2);

            SceneLoader.LoadScene(GameScenes.Menu);
        }

    }
}
