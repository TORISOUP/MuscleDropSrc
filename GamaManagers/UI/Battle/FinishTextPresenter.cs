using Assets.MuscleDrop.Scripts.Utilities.UI;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.MuscleDrop.Scripts.GamaManagers.UI.Battle
{
    public class FinishTextPresenter : MonoBehaviour
    {
        [Inject]
        private GameTimeManager timeManager;

        [SerializeField]
        private string message = "筋肉そこまで！";

        void Start()
        {
            var text = GetComponent<Text>();
            text.text = "";

            var shaker = GetComponent<ElementShake>();

            timeManager.RemainingTime.FirstOrDefault(x => x == 0)
                .Subscribe(_ =>
                {
                    text.text = message;
                    shaker.ShakePosition(30, 10f);
                    shaker.ShakeRotation(10, 10f);
                });
        }

    }
}
