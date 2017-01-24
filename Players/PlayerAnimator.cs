using System;
using Assets.MuscleDrop.Scripts.GamaManagers;
using UniRx;
using UnityEngine;
using Assets.MuscleDrop.Scripts.Utilities.ExtensionMethods;

namespace Assets.MuscleDrop.Scripts.Players
{
    public class PlayerAnimator : BasePlayerComponent
    {
        private Animator _animator;

        private bool IsRunning
        {
            set { _animator.SetBool("IsRunning", value); }
        }

        private bool IsJumping
        {
            set { _animator.SetBool("IsJumping", value); }
        }

        private bool IsHipDroping
        {
            set { _animator.SetBool("IsHipDroping", value); }
        }

        private bool IsDead
        {
            set { _animator.Play(value ? "Down" : "Idle"); }
        }

        protected override void OnInitialize()
        {
            _animator = GetComponent<Animator>();
            var playerMover = GetComponent<PlayerMover>();
            Core.CurrentGameState.FirstOrDefault(x => x == GameState.Battle)
                .Subscribe(__ =>
                {
                    playerMover.IsRunning.Subscribe(x => IsRunning = x);
                    playerMover.IsJumping.Subscribe(x => IsJumping = x);
                    playerMover.IsHipDroping.Subscribe(x => IsHipDroping = x);

                    InputEventProvider.MoveDirection
                        .Where(x => !playerMover.IsOnAir.Value && Core.IsAlive.Value)
                        .Subscribe(x => ChangeRotation(x));

                    Core.IsAlive.Subscribe(x => IsDead = !x);
                });
        }

        private void ChangeRotation(Vector3 inputVector)
        {
            //プレイヤの向き
            var forward = inputVector.SuppressY();
            if ((Math.Abs(forward.magnitude) < 0.1f))
                return;

            var lookRotation = Quaternion.LookRotation(forward);
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                lookRotation,
                Time.deltaTime * 20.0f
            );
        }
    }
}

