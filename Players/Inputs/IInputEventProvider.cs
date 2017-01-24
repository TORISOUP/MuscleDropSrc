using UniRx;
using UnityEngine;

namespace Assets.MuscleDrop.Scripts.Players.Inputs
{
    public interface IInputEventProvider
    {
        IReadOnlyReactiveProperty<bool> JumpButton { get; }
        IReadOnlyReactiveProperty<bool> AttackButton { get; }
        IReadOnlyReactiveProperty<Vector3> MoveDirection { get; }
    }
}
