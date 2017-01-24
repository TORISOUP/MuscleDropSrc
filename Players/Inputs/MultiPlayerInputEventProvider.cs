using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Assets.MuscleDrop.Scripts.Players.Inputs
{
    public class MultiPlayerInputEventProvider : BasePlayerComponent, IInputEventProvider
    {
        private ReactiveProperty<bool> _attack = new BoolReactiveProperty();
        private ReactiveProperty<bool> _jump = new BoolReactiveProperty();
        private ReactiveProperty<Vector3> _moveDirection = new ReactiveProperty<Vector3>();

        public IReadOnlyReactiveProperty<bool> AttackButton { get { return _attack; } }

        public IReadOnlyReactiveProperty<Vector3> MoveDirection { get { return _moveDirection; } }

        public IReadOnlyReactiveProperty<bool> JumpButton { get { return _jump; } }


        protected override void OnInitialize()
        {
            var id = (int)this.PlayerId;

            var jumpButton = "Jump" + id;
            var attackButton = "Attack" + id;
            var hori = "Horizontal" + id;
            var vert = "Vertical" + id;

            this.UpdateAsObservable()
                .Select(_ => Input.GetButton(jumpButton))
                .DistinctUntilChanged()
                .Subscribe(x => _jump.Value = x);

            this.UpdateAsObservable()
                .Select(_ => Input.GetButton(attackButton))
                .DistinctUntilChanged()
                .Subscribe(x => _attack.Value = x);

            this.UpdateAsObservable()
                .Select(_ => new Vector3(Input.GetAxis(hori), 0, Input.GetAxis(vert)))
                .Subscribe(x => _moveDirection.SetValueAndForceNotify(x));
        }
    }
}
