using System.Collections;
using Assets.MuscleDrop.Scripts.Utilities.ExtensionMethods;
using UniRx;
using UnityEngine;
using UniRx.Triggers;
using System;
using Assets.MuscleDrop.Scripts.GamaManagers;

namespace Assets.MuscleDrop.Scripts.Players
{
    [RequireComponent(typeof(PlayerCharcterController))]
    public class PlayerMover : BasePlayerComponent
    {
        BoolReactiveProperty _isRunning = new BoolReactiveProperty();
        BoolReactiveProperty _isOnAir = new BoolReactiveProperty();
        BoolReactiveProperty _isJumping = new BoolReactiveProperty();
        BoolReactiveProperty _isHovering = new BoolReactiveProperty();
        BoolReactiveProperty _isHipDroping = new BoolReactiveProperty();
        Subject<Unit> _hipDroppedSubject = new Subject<Unit>();

        private PlayerCharcterController cc;
        private bool isFreezed;

        public IReadOnlyReactiveProperty<bool> IsRunning { get { return _isRunning; } }
        public IReadOnlyReactiveProperty<bool> IsOnAir { get { return _isOnAir; } }
        public IReadOnlyReactiveProperty<bool> IsJumping { get { return _isJumping; } }
        public IReadOnlyReactiveProperty<bool> IsHovering { get { return _isHovering; } }
        public IReadOnlyReactiveProperty<bool> IsHipDroping { get { return _isHipDroping; } }
        public IObservable<Unit> OnHipDropped { get { return _hipDroppedSubject; } }

        protected override void OnInitialize()
        {
            cc = GetComponent<PlayerCharcterController>();

            var rb = GetComponent<Rigidbody>();

            // ジャンプ動作
            InputEventProvider.JumpButton
                .Where(_=>Core.CurrentGameState.Value == GameState.Battle)
                .Where(x => x && Core.IsAlive.Value && !IsOnAir.Value && !isFreezed)
                .ThrottleFirst(TimeSpan.FromSeconds(1))
                .Subscribe(_ =>
                {
                    cc.Jump(CurrentPlayerParameter.Value.JumpPower);
                    _isJumping.Value = true;
                });

            // 着地処理
            cc.IsGrounded
                .Where(x => x && !isFreezed)
                .Subscribe(x =>
                {
                    if (_isHipDroping.Value)
                    {
                        _hipDroppedSubject.OnNext(Unit.Default);
                    }
                    StartCoroutine(FreezeCoroutine());
                    _isJumping.Value = false;
                    _isHipDroping.Value = false;
                });

            // 移動処理
            InputEventProvider.MoveDirection
                .Where(_ => Core.CurrentGameState.Value == GameState.Battle)
                .Subscribe(x =>
                {
                    var value = (IsOnAir.Value || !Core.IsAlive.Value) ? Vector3.zero :
                    x.normalized * CurrentPlayerParameter.Value.MoveSpeed;
                    cc.Move(value);
                });

            // ヒップドロップ動作
            InputEventProvider.AttackButton
                .Where(_ => Core.CurrentGameState.Value == GameState.Battle)
                .Where(x => x && Core.IsAlive.Value && !_isHovering.Value && IsOnAir.Value)
                .Subscribe(x =>
                {
                    StartCoroutine(HoverCoroutine());
                    _isJumping.Value = false;
                });
            this.FixedUpdateAsObservable()
                .Where(x => Core.IsAlive.Value && _isHovering.Value)
                .Subscribe(x => cc.Stop());

            // 走行中かどうかの通知
            InputEventProvider.MoveDirection
                .Where(x => Core.IsAlive.Value)
                .Subscribe(x => _isRunning.Value = !IsOnAir.Value && x.magnitude >= 0.1f);

            // 上空かどうかの通知
            cc.IsGrounded
                .Where(x => Core.IsAlive.Value)
                .Subscribe(x => _isOnAir.Value = !x);

            // 空中停止中かどうか
            _isHovering
                .Where(x => !x && Core.IsAlive.Value && IsOnAir.Value)
                .Subscribe(x =>
                {
                    cc.ApplyForce(Vector3.down * 300);
                    _isHipDroping.Value = true;
                });

            // やられたときの処理
            Core.IsAlive.Where(x => x).Subscribe(x =>
              {
                  _isRunning.Value = false;
                  _isOnAir.Value = false;
                  _isJumping.Value = false;
                  _isHovering.Value = false;
                  _isHipDroping.Value = false;
              });

            Core.IsAlive.Subscribe(x =>
            {
                rb.useGravity = x;
                if (!x)
                {
                    //死んだ
                    rb.constraints = RigidbodyConstraints.None;
                }
                else
                {
                    rb.constraints = RigidbodyConstraints.FreezeRotation;
                    transform.rotation = Quaternion.LookRotation(Vector3.back);
                }
            });


            // ダメージ時の処理
            Core.OnDamaged.Subscribe(x =>
            {
                var force = x.Direction * x.Value * CurrentPlayerParameter.Value.FuttobiRate;
                cc.ApplyForce(force);
            });

            //着地したら止める
            OnHipDropped.Subscribe(_ => cc.Stop());
        }

        IEnumerator FreezeCoroutine()
        {
            // 一定時間再ジャンプ禁止
            isFreezed = true;
            yield return new WaitForSeconds(0.3f);
            isFreezed = false;
        }

        IEnumerator HoverCoroutine()
        {
            // 一定時間待機
            _isHovering.Value = true;
            yield return new WaitForSeconds(0.3f);
            _isHovering.Value = false;
        }
    }
}

