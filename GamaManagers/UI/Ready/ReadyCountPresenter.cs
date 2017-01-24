using Assets.MuscleDrop.Scripts.Utilities.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using UniRx;
namespace Assets.MuscleDrop.Scripts.GamaManagers.UI
{
    public class ReadyCountPresenter : MonoBehaviour
    {
        [Inject]
        private GameTimeManager timerManager;


        void Start()
        {
            var text = GetComponent<Text>();

            var shaker = GetComponent<ElementShake>();

            timerManager.ReadyTime
                .Subscribe(x =>
                {

                    if (x > 0)
                    {
                        shaker.ShakePosition(20.0f, 10);
                        text.text = x.ToString();
                    }
                    else
                    {
                        shaker.ShakePosition(40.0f, 10);
                        text.text = "レッツ マッチョ";
                    }
                });

        }

    }
}
