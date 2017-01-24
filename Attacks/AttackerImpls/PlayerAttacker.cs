using Assets.MuscleDrop.Scripts.Players;

namespace Assets.MuscleDrop.Scripts.Attacks.AttackerImpls
{
    /// <summary>
    /// プレイヤからの攻撃を表す
    /// </summary>
    public struct PlayerAttacker : IAttacker
    {
        public PlayerId PlayerId { get; private set; }

        public PlayerAttacker(PlayerId playerId) : this()
        {
            PlayerId = playerId;
        }
    }
}