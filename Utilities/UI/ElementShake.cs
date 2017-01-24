using System.Collections;
using UnityEngine;

namespace Assets.MuscleDrop.Scripts.Utilities.UI
{
    public class ElementShake : MonoBehaviour
    {
        private RectTransform rect;
        private Vector3 originalPosition;
        private Quaternion originalRotation;

        void Awake()
        {
            rect = GetComponent<RectTransform>();
            originalPosition = rect.anchoredPosition;
            originalRotation = rect.rotation;
        }

        public void ShakePosition(float power, float frame)
        {
            StartCoroutine(ShakePositionCoroutine(power, frame));
        }

        IEnumerator ShakePositionCoroutine(float power, float frame)
        {
            for (var i = 0; i < frame; i++)
            {
                var ox = UnityEngine.Random.Range(-1.0f, 1.0f);
                var oy = UnityEngine.Random.Range(-1.0f, 1.0f);
                var offset = new Vector3(ox, oy, 0).normalized * UnityEngine.Random.Range(0, power);
                rect.anchoredPosition = originalPosition + offset;
                yield return null;
            }
            rect.anchoredPosition = originalPosition;
        }

        public void ShakeRotation(float angleRange, float frame)
        {
            StartCoroutine(ShakeRotationCorountine(angleRange, frame));
        }

        IEnumerator ShakeRotationCorountine(float angleRange, float frame)
        {
            for (int i = 0; i < frame; i++)
            {
                var angle = UnityEngine.Random.Range(-angleRange, angleRange);
                rect.rotation = Quaternion.AngleAxis(angle, Vector3.forward) * originalRotation;
                yield return null;
            }
            rect.rotation = originalRotation;
        }

    }
}
