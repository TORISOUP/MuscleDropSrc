using System;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.MuscleDrop.Scripts.Utilities.UI
{
    /// <summary>
    /// 選択されている時に揺れる奴
    /// </summary>
    public class SwingOnSelected : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private bool _isSelected;


        [SerializeField]
        private float period = 2.0f;

        [SerializeField]
        private float amp = 1.0f;

        [SerializeField]
        private float shakePower = 5.0f;
        [SerializeField]
        private int shakeFrame = 7;

        private RectTransform rect;
        private Vector3 originalPosition;
        private Quaternion originalRotation;
        private float count = 0;

        void Awake()
        {
            rect = GetComponent<RectTransform>();
            originalRotation = rect.rotation;
            originalPosition = rect.anchoredPosition;
        }

        void Start()
        {
            this.UpdateAsObservable()
                .Where(_ => _isSelected)
                .Subscribe(_ =>
                {
                    count += Time.deltaTime;
                    var offset = amp * Mathf.Sin(count * 2 * Mathf.PI / period);
                    rect.rotation = Quaternion.AngleAxis(offset, Vector3.forward) * originalRotation;
                });

            this.UpdateAsObservable()
                .Where(_ => !_isSelected && count != 0)
                .Subscribe(_ =>
                {
                    count += Time.deltaTime;
                    var offset = amp * Mathf.Sin(count * 2 * Mathf.PI / period);
                    rect.rotation = Quaternion.AngleAxis(offset, Vector3.forward) * originalRotation;
                    if (Math.Abs(offset) < 0.3f)
                    {
                        count = 0;
                    }
                });
        }


        IEnumerator ShakeCoroutine()
        {
            for (var i = 0; i < shakeFrame; i++)
            {
                var ox = UnityEngine.Random.Range(-1.0f, 1.0f);
                var oy = UnityEngine.Random.Range(-1.0f, 1.0f);
                var offset = new Vector3(ox, oy, 0).normalized * UnityEngine.Random.Range(0, shakePower);
                rect.anchoredPosition = originalPosition + offset;
                yield return null;
            }
        }

        public void OnSelect(BaseEventData eventData)
        {
            StartCoroutine(ShakeCoroutine());
            _isSelected = true;
        }

        public void OnDeselect(BaseEventData eventData)
        {

            _isSelected = false;
            rect.rotation = originalRotation;
            StopAllCoroutines();
            rect.anchoredPosition = originalPosition;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            EventSystem.current.SetSelectedGameObject(this.gameObject);
            StartCoroutine(ShakeCoroutine());
            _isSelected = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isSelected = false;
            rect.rotation = originalRotation;
            EventSystem.current.SetSelectedGameObject(this.gameObject);
            StopAllCoroutines();
            rect.anchoredPosition = originalPosition;
        }
    }
}
