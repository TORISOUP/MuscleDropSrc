using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Assets.MuscleDrop.Scripts.Utilities.UI
{
    public class ElementBound : MonoBehaviour
    {

        [SerializeField]
        private float period;

        [SerializeField]
        private float amp;

        [SerializeField] private Vector2 direction;

        [SerializeField] private bool _useAbs;

        private void Start()
        {
            var rect = GetComponent<RectTransform>();
            var startPosition = rect.anchoredPosition;

            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    var offsetRaw = amp * Mathf.Sin(Time.time * 2 * Mathf.PI / period);
                    var offset = _useAbs ? Mathf.Abs(offsetRaw) : offsetRaw;
                    rect.anchoredPosition = startPosition + direction * offset;

                });

        }

    }
}
