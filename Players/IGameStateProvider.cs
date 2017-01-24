using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.MuscleDrop.Scripts.GamaManagers;
using UniRx;

namespace Assets.MuscleDrop.Scripts.Players
{
    public interface IGameStateProvider
    {
        IReadOnlyReactiveProperty<GameState> CurrentGameState { get; }
    }
}
