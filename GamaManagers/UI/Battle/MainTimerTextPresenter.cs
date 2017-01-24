using Assets.MuscleDrop.Scripts.Utilities.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using UniRx;
namespace Assets.MuscleDrop.Scripts.GamaManagers.UI.Battle
{
    public class MainTimerTextPresenter : MonoBehaviour
    {
        [Inject]
        private GameTimeManager timerManager;

        [SerializeField]
        private Color shortTimeColor;

        void Start()
        {
            var text = GetComponent<Text>();
            var shaker = GetComponent<ElementShake>();

            timerManager.RemainingTime
                .Subscribe(t =>
                {
                    var m = t / 60;
                    var s = t % 60;
                    var str = string.Format("{0:D2}:{1:D2}", m, s);
                    text.text = str;

                    if (t <= 10)
                    {
                        text.color = shortTimeColor;
                        shaker.ShakePosition(20, 5.0f);
                    }
                    if (t == 0)
                    {
                        text.text = "";
                    }

                });
        }

    }
}
