using UnityEngine;
using Assets.MuscleDrop.Scripts.Players;

namespace Assets.MuscleDrop.Scripts.Attacks.AttackerImpls
{
    /// <summary>
    /// フィールド上のギミックからの攻撃を表す
    /// </summary>
    public struct NonPlayerAttacker : IAttacker
    {
        public static NonPlayerAttacker Default = new NonPlayerAttacker();
    }
}

