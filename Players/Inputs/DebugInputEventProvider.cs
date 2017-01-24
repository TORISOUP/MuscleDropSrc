using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Assets.MuscleDrop.Scripts.Players.Inputs
{
    /// <summary>
    /// キーボードからのInputEventを発行する
    /// </summary>
    public class DebugInputEventProvider : BasePlayerComponent, IInputEventProvider
    {

        private ReactiveProperty<bool> _attack = new BoolReactiveProperty();
        private ReactiveProperty<bool> _jump = new BoolReactiveProperty();
        private ReactiveProperty<Vector3> _moveDirection = new ReactiveProperty<Vector3>();
        public IReadOnlyReactiveProperty<bool> AttackButton { get { return _attack; } }
        public IReadOnlyReactiveProperty<Vector3> MoveDirection { get { return _moveDirection; } }
        public IReadOnlyReactiveProperty<bool> JumpButton { get { return _jump; } }


        protected override void OnInitialize()
        {
            this.UpdateAsObservable()
                .Select(_ => Input.GetKey(KeyCode.Z))
                .DistinctUntilChanged()
                .Subscribe(x => _jump.Value = x);

            this.UpdateAsObservable()
                .Select(_ => Input.GetKey(KeyCode.X))
                .DistinctUntilChanged()
                .Subscribe(x => _attack.Value = x);

            this.UpdateAsObservable()
                .Select(_ => new Vector3(Input.GetAxis("Horizontal1"), 0, Input.GetAxis("Vertical1")))
                .Subscribe(x => _moveDirection.SetValueAndForceNotify(x));
        }

    }
}
