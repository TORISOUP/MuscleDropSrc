using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Assets.MuscleDrop.Scripts.Utilities.UI
{
    public class ElementRotate : MonoBehaviour
    {

        [SerializeField]
        private float period = 2.0f;

        [SerializeField]
        private float amp = 1.0f;

        private RectTransform rect;
        private Quaternion originalRotation;

        void Awake()
        {
            rect = GetComponent<RectTransform>();
            originalRotation = rect.rotation;
        }

        void Start()
        {
            
            this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                var offset = amp * Mathf.Sin(Time.time * 2 * Mathf.PI / period);
                rect.rotation = Quaternion.AngleAxis(offset, Vector3.forward) * originalRotation;
            });
        }
    }
}
