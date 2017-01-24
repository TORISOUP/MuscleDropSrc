using Assets.MuscleDrop.Scripts.Utilities.ExtensionMethods;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Assets.MuscleDrop.Scripts.Players
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerCharcterController : BasePlayerComponent
    {
        private ReactiveProperty<bool> _isGrounded = new BoolReactiveProperty(true);

        public IReadOnlyReactiveProperty<bool> IsGrounded { get { return _isGrounded; } }

        private Rigidbody _rigidbody;
        private CharacterController _controller;

        private Vector3 inputDirection;

        /// <summary>
        /// Move the specified velocity.
        /// </summary>
        /// <param name="velocity">Velocity.</param>
        public void Move(Vector3 velocity)
        {
            inputDirection = velocity;
        }

        /// <summary>
        /// Jump the specified power.
        /// </summary>
        /// <param name="power">Power.</param>
        public void Jump(float power)
        {
            ApplyForce(Vector3.up * power);
        }

        /// <summary>
        /// Stop this instance.
        /// FixiedUpdateから呼ぶ
        /// </summary>
        public void Stop()
        {
            _rigidbody.velocity = Vector3.zero;
            inputDirection = Vector3.zero;
        }

        /// <summary>
        /// Applies the force.
        /// </summary>
        /// <param name="force">Force.</param>
        public void ApplyForce(Vector3 force)
        {
            Observable.NextFrame(FrameCountType.FixedUpdate)
                .Subscribe(_ => _rigidbody.AddForce(force, ForceMode.VelocityChange));
        }

        /// <summary>
        /// プレイヤ情報の初期化が完了した時に実行される
        /// </summary>
        protected override void OnInitialize()
        {
            _rigidbody = GetComponent<Rigidbody>();

            this.FixedUpdateAsObservable()
                .Subscribe(_ =>
                {
                    _rigidbody.AddForce(inputDirection, ForceMode.Acceleration);
                    _isGrounded.Value = CheckGrounded();
                });
        }

        /// <summary>
        /// Checks the grounded.
        /// </summary>
        /// <returns><c>true</c>, if grounded was checked, <c>false</c> otherwise.</returns>
        private bool CheckGrounded()
        {

            //放つ光線の初期位置と姿勢
            //若干身体にめり込ませた位置から発射しないと正しく判定できない時がある
            var ray = new Ray(this.transform.position + Vector3.up * 0.8f, Vector3.down);
            //Raycastがhitするかどうかで判定
            var result = Physics.SphereCast(ray, 0.68f, 1.0f);
           
            return result;
        }
    }
}