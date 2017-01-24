using System;
using UniRx;
using UnityEngine;

namespace Assets.MuscleDrop.Scripts.GamaManagers
{
    public enum GameState
    {
        Initializing,
        Ready,
        Battle,
        Result,
        Finished
    }

    [Serializable]
    public class GameStateReactiveProperty : ReactiveProperty<GameState>
    {
        public GameStateReactiveProperty()
        {
        }

        public GameStateReactiveProperty(GameState initialValue)
            : base(initialValue)
        {
        }
    }
}
