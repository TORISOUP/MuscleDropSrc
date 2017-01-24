using Assets.MuscleDrop.Scripts.Attacks;
using System;
using UnityEngine;

namespace Assets.MuscleDrop.Scripts.Damages
{
    [Serializable]
    public struct Damage
    {
        /// <summary>
        /// 攻撃者
        /// </summary>
        public IAttacker Attacker;

        /// <summary>
        /// ダメージ値
        /// </summary>
        public float Value;

        /// <summary>
        /// 吹っ飛ばす向き
        /// </summary>
        public Vector3 Direction;

        /// <summary>
        /// 攻撃種別
        /// </summary>
        public AttackType Type;
    }

    public enum AttackType
    {
        Shockwave,
        Toge,
        HipDrop
    }
}
