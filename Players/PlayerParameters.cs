using System;
using Assets.MuscleDrop.Scripts.Attacks;
using UnityEngine;

namespace Assets.MuscleDrop.Scripts.Players
{
    public enum PlayerId
    {
        Player1 = 1,
        Player2 = 2,
        Player3 = 3,
        Player4 = 4
    }

    static class PlayerIdExtensions
    {
        public static Color ToColor(this PlayerId id)
        {
            switch (id)
            {
                case PlayerId.Player1:
                    return Color.red;
                case PlayerId.Player2:
                    return Color.blue;
                case PlayerId.Player3:
                    return Color.green;
                case PlayerId.Player4:
                    return Color.yellow;
                default:
                    throw new ArgumentOutOfRangeException("id", id, null);
            }
        }

        public static string ToName(this PlayerId id)
        {
            switch (id)
            {
                case PlayerId.Player1:
                    return "1P";
                case PlayerId.Player2:
                    return "2P";
                case PlayerId.Player3:
                    return "3P";
                case PlayerId.Player4:
                    return "4P";
                default:
                    throw new ArgumentOutOfRangeException("id", id, null);
            }
        }
    }

    /// <summary>
    /// 死亡理由
    /// </summary>
    public struct DeadReason
    {
        private PlayerId _deadPlayerId;
        private IAttacker _attacker;

        /// <summary>
        /// 死亡したPlayerId
        /// </summary>
        public PlayerId DeadPlayerId
        {
            get { return _deadPlayerId; }
        }

        /// <summary>
        /// 死亡原因となった攻撃者情報
        /// </summary>
        public IAttacker Attacker
        {
            get { return _attacker; }
        }

        public DeadReason(PlayerId deadPlayerId, IAttacker attacker)
        {
            _deadPlayerId = deadPlayerId;
            _attacker = attacker;
        }
    }

    [Serializable]
    public struct PlayerParameters
    {
        /// <summary>
        /// ジャンプ力
        /// </summary>
        public float JumpPower;

        /// <summary>
        /// 移動速度
        /// </summary>
        public float MoveSpeed;

        /// <summary>
        /// 吹っ飛びやすさ
        /// </summary>
        public float FuttobiRate;

        /// <summary>
        /// ヒップドロップの範囲
        /// </summary>
        public float HipDropScale;

        /// <summary>
        /// ヒップドロップの伝播速度
        /// </summary>
        public float HipDropSpeed;

        /// <summary>
        /// ヒップドロップの吹っ飛び力
        /// </summary>
        public float HipDropPower;

        /// <summary>
        /// ヒップドロップの距離減衰率
        /// </summary>
        public float HipDropDamping;
    }
}
