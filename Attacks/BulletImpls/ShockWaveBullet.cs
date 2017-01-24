using Assets.MuscleDrop.Scripts.Attacks.AttackerImpls;
using Assets.MuscleDrop.Scripts.Audio;
using Assets.MuscleDrop.Scripts.Damages;
using Assets.MuscleDrop.Scripts.Players;
using Assets.MuscleDrop.Scripts.Utilities.ExtensionMethods;
using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Assets.MuscleDrop.Scripts.Attacks.BulletImpls
{
    public class ShockWaveBullet : BaseBullet
    {
        public enum Type
        {
            Normal,
            Powerup
        }

        [SerializeField]
        Type m_WaveType;

        [SerializeField]
        EffekseerEmitter m_Emitter;

        [SerializeField]
        SphereCollider m_Collider;

        [SerializeField]
        private float FuttobiPower = 100.0f;

        [SerializeField]
        private AnimationCurve curve;

        // NormalとPowerupの設定
        [SerializeField, Header("衝撃波の初期半径")]
        float m_CollisionInitialSize;

        [SerializeField, Header("衝撃波の最大半径")]
        float m_CollisionMaxSize;

        [SerializeField, Header("広げる大きさ")]
        float m_ExpansionValue;

        [SerializeField, Header("広げる時間")]
        float m_ExpansionTime;

        [SerializeField, Header("衝撃波の辺り判定の幅")]
        float m_CollisionWidth;

        void Reset()
        {
            var t = transform;
            m_Collider = GetComponent<SphereCollider>();
            m_Emitter = t.FindChild("Effect").GetComponent<EffekseerEmitter>();

            m_CollisionInitialSize = 0.22f;
            m_CollisionMaxSize = 3.8f;
            m_ExpansionValue = 0.22f;
            m_ExpansionTime = 0.5f;
            m_CollisionWidth = 1.0f;
        }

        void Start()
        {
            m_Collider.enabled = false;

            // ShockWaveエフェクトの衝撃波本体の白い部分が出るのを待つ
            this.UpdateAsObservable()
                .Where(_ => m_Emitter.exists)
                .Delay(TimeSpan.FromSeconds(0.14))
                .First()
                .Subscribe(_ =>
                {
                    m_Collider.enabled = true;
                    m_Collider.radius = m_CollisionInitialSize;
                    if (m_Collider.attachedRigidbody != null)
                    {
                        m_Collider.attachedRigidbody.WakeUp();
                    }
                });

            // ShockWaveエフェクトの衝撃波本体の白い部分が消えるのを待つ
            this.UpdateAsObservable()
                .Where(_ => m_Emitter.exists)
                .Delay(TimeSpan.FromSeconds(m_ExpansionTime))
                .First()
                .Subscribe(_ =>
                {
                    m_Collider.enabled = false;
                });

            // Colliderを広げる
            this.UpdateAsObservable()
                .Where(_ => m_Collider.enabled)
                .Subscribe(x =>
                {
                    m_Collider.radius += m_ExpansionValue;
                });

            // エフェクトが消えたら自分も削除
            this.UpdateAsObservable()
                .Where(_ => !m_Emitter.exists)
                .Subscribe(x =>
                {
                    Destroy(gameObject);
                });

            // SEを鳴らす
            AudioManager.PlaySoundEffect(SoundEffect.Impact1);
        }

        // Colliderの大きさを 0-1 に正規化して返す
        // radiusが小さい程 1.0 に近い値を返す
        float CalcAttackPower()
        {
            return 1.0f - (m_Collider.radius / (m_CollisionMaxSize - m_CollisionInitialSize));
        }

        // 吹き飛ばしの方向を計算
        Vector3 CalcAttackDirection(Transform target)
        {
            return ((target.position - transform.position).SuppressY().normalized + Vector3.up * 0.15f).normalized;
        }

        Damage CalcDamage(Transform target)
        {
            return new Damage()
            {
                Attacker = this.Attacker,
                Value = CalcAttackPower() * FuttobiPower,
                Direction = CalcAttackDirection(target),
                Type = AttackType.Shockwave
            };
        }

        // ダメージの範囲内であるかどうか
        // SphereColliderだが、衝撃波の値判定は先端のみなので計算
        bool IsInDamageArea(Transform target)
        {
            return (m_Collider.radius - m_CollisionWidth) <= Vector3.Distance(transform.position, target.position);
        }

        // このShockWaveBulletを
        bool IsAttackerPlayer(PlayerId playerId)
        {
            if (this.Attacker.GetType() != typeof(PlayerAttacker))
            {
                return false;
            }
            return (((PlayerAttacker)this.Attacker).PlayerId == playerId);
        }

        private void OnTriggerEnter(Collider other)
        {
            var damageApplicable = other.gameObject.GetComponent<IDamageApplicable>();
            if (damageApplicable != null)
            {
                // 対象がプレイヤーかつ、自分以外の場合にダメージを与える
                var player = other.GetComponent<PlayerCore>();
                if (player != null)
                {
                    if (!IsAttackerPlayer(player.PlayerId))
                    {
                        // 衝撃波は先端にのみ当たり判定があるのでチェック
                        if (IsInDamageArea(other.transform))
                        {
                            damageApplicable.ApplyDamage(CalcDamage(other.transform));
                        }
                    }
                }
                else
                {
                    // 衝撃波は先端にのみ当たり判定があるのでチェック
                    if (IsInDamageArea(other.transform))
                    {
                        damageApplicable.ApplyDamage(CalcDamage(other.transform));
                    }
                }
            }
        }
    }
}
