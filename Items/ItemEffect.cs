using System;
using Assets.MuscleDrop.Scripts.Players;

namespace Assets.MuscleDrop.Scripts.Items
{
    /// <summary>
    /// アイテムの効果
    /// </summary>
    [Serializable]
    public struct ItemEffect
    {
        public ItemType ItemType;
        public PlayerParameters PowerUpParameters;
        public float DurationTime;
        

    }
}
