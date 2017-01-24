using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.MuscleDrop.Scripts.Players;

namespace Assets.MuscleDrop.Scripts.GamaManagers
{
    public struct ResultScore
    {
        public PlayerId PlayerId { get; private set; }
        public int KillCount { get; private set; }
        public int DeadCount { get; private set; }
        public int SuicideCount { get; private set; }
        public int Rank { get; private set; }

        public ResultScore(PlayerId playerId, int killCount, int deadCount, int suicideCount, int rank) : this()
        {
            PlayerId = playerId;
            KillCount = killCount;
            DeadCount = deadCount;
            SuicideCount = suicideCount;
            Rank = rank;
        }
    }

}
