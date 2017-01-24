using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Assets.MuscleDrop.Scripts.Effects
{
    public class Effect : MonoBehaviour
    {
        [SerializeField]
        EffekseerEmitter m_Emitter;

        [SerializeField]
        bool m_AutoDestroy; // Effekseerは自動的に消えてくれないので自動的に消えてくれるようにする

        [SerializeField]
        bool m_AutoInactive; // Effekseerは自動的に消えてくれないので自動的に消えてくれるようにする

        private void Reset()
        {
            var t = transform;
            m_Emitter = t.GetComponent<EffekseerEmitter>();

            // デフォルトではエフェクトの自動削除
            m_AutoDestroy = true;

            // エフェクトのActive/Inactiveを切り替える
            m_AutoInactive = true;
        }

        void Start()
        {
            // エフェクトの自動削除
            this.UpdateAsObservable()
                .Where(_ => m_AutoDestroy && !m_Emitter.exists)
                .Subscribe(_ =>
            {
                // エフェクトの自動削除
                Destroy(gameObject);
            });

            this.UpdateAsObservable()
                .Where(_ => m_AutoInactive && !m_Emitter.exists)
                .Subscribe(_ =>
                {
                    // エフェクトを非アクティブ
                    gameObject.SetActive(!m_AutoInactive);
                });
        }

        public void Fire()
        {
            gameObject.SetActive(true);
            m_Emitter.Play();
        }
    }
}
