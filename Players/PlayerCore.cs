using System;
using Assets.MuscleDrop.Scripts.Attacks;
using Assets.MuscleDrop.Scripts.Attacks.AttackerImpls;
using Assets.MuscleDrop.Scripts.Damages;
using Assets.MuscleDrop.Scripts.GamaManagers;
using Assets.MuscleDrop.Scripts.Items;
using Assets.MuscleDrop.Scripts.Utilities.ExtensionMethods;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject.SpaceFighter;

namespace Assets.MuscleDrop.Scripts.Players
{
    public class PlayerCore : MonoBehaviour, IDamageApplicable, IDieable
    {
        private PlayerId _playerId;
        public PlayerId PlayerId { get { return _playerId; } }

        private IAttacker _lastDamagedAttacker;
        public IAttacker LastDamagedAttacker { get { return _lastDamagedAttacker; } }

        private ReactiveProperty<bool> _isAlive = new BoolReactiveProperty(true);
        public IReadOnlyReactiveProperty<bool> IsAlive { get { return _isAlive; } }

        private CameraMultiTargetObjective cameraMultiTarget;

        private IGameStateProvider gameStateProvider;

        public IReadOnlyReactiveProperty<GameState> CurrentGameState
        {
            get { return gameStateProvider.CurrentGameState; }
        }

        /// <summary>
        /// リスポーン位置一覧
        /// </summary>
        private Vector3[] _respawnPoints;

        #region Events

        /// <summary>
        /// 死亡通知
        /// </summary>
        public IObservable<DeadReason> OnDead { get { return _deadSubject; } }
        private Subject<DeadReason> _deadSubject = new Subject<DeadReason>();


        private Subject<Damage> _damageSubject = new Subject<Damage>();
        public IObservable<Damage> OnDamaged { get { return _damageSubject; } }

        private Subject<ItemEffect> _pickUpItemSubject = new Subject<ItemEffect>();
        public IObservable<ItemEffect> OnPickUpItem { get { return _pickUpItemSubject; } }

        #endregion


        #region PlayerParams

        [SerializeField]
        private PlayerParameters DefaultPlayerParameter = new PlayerParameters();
        private ReactiveProperty<PlayerParameters> _currentPlayerParameter;

        [SerializeField]
        public Collider HipCollisionGO;

        /// <summary>
        /// 現在のPlayerのパラメータ
        /// </summary>
        public IReadOnlyReactiveProperty<PlayerParameters> CurrentPlayerParameter { get { return _currentPlayerParameter; } }

        /// <summary>
        /// プレイヤのパラメータを規定値に戻す
        /// </summary>
        public void ResetPlayerParameter()
        {
            _currentPlayerParameter.Value = DefaultPlayerParameter;
        }

        public void SetPlayerParameter(PlayerParameters parameters)
        {
            _currentPlayerParameter.Value = parameters;
        }

        #endregion

        void Awake()
        {
            _currentPlayerParameter = new ReactiveProperty<PlayerParameters>(DefaultPlayerParameter);

            cameraMultiTarget = GetComponent<CameraMultiTargetObjective>();

            _onInitializeAsyncSubject.Subscribe(_ =>
            {
                IsAlive.Where(x => x).Skip(1)
                    .Subscribe(__ =>
                    {
                        //生き返ったら座標を書き換える

                        var p = _respawnPoints[UnityEngine.Random.Range(0, _respawnPoints.Length)];
                        transform.position = p;

                    });

                //画面外に出たら死ぬ
                this.ObserveEveryValueChanged(x => x.transform.position.y)
                    .Where(x => x < -3 && this.IsAlive.Value)
                    .Subscribe(__ => Kill(NonPlayerAttacker.Default))
                    .AddTo(this);

                //ダメージを受けたら誰からのダメージか記憶する
                OnDamaged
                    .Do(x => _lastDamagedAttacker = x.Attacker)
                    .Throttle(TimeSpan.FromSeconds(3))
                    .Subscribe(__ => _lastDamagedAttacker = null);

                IsAlive.Subscribe(x => cameraMultiTarget.EnableTracking = x);

                //死んだ
                OnDead
                    .Subscribe(__ =>
                    {
                        //死んだら4秒後に復活
                        Observable.Timer(TimeSpan.FromSeconds(2))
                            .Subscribe(___ => _isAlive.Value = true);
                    });

                this.OnTriggerEnterAsObservable()
                    .Where(__ => IsAlive.Value)
                    .Subscribe(x =>
                    {
                        var i = x.GetComponent<ItemBase>();
                        if (i != null)
                        {
                            _pickUpItemSubject.OnNext(i.ItemEffect);
                            i.PickedUp();
                        }
                    });
                                
                    this.OnTriggerEnterAsObservable()
                        .Where(__ => IsAlive.Value)
                        .Subscribe(x =>
                            {
                                var i = x.GetComponent<ItemBase>();
                                if (i != null)
                                {
                                    _pickUpItemSubject.OnNext(i.ItemEffect);
                                    i.PickedUp();
                                }
                            });

            });
        }

        #region Initialize
        /// <summary>
        /// 初期化を実行を開始するイベント通知
        /// </summary>
        public IObservable<PlayerId> OnInitializeAsync { get { return _onInitializeAsyncSubject; } }
        private readonly AsyncSubject<PlayerId> _onInitializeAsyncSubject = new AsyncSubject<PlayerId>();

        /// <summary>
        /// PlayerにIdを登録する
        /// プレイや生成後にGameManagerがこれを呼び出して初期化する
        /// </summary>
        public void InitializePlayer(PlayerId myId, Vector3[] respawnPoints,IGameStateProvider provider)
        {
            gameStateProvider = provider;
            _playerId = myId;
            _respawnPoints = respawnPoints;
            _onInitializeAsyncSubject.OnNext(myId);
            _onInitializeAsyncSubject.OnCompleted();
        }
        #endregion

        #region Impls
        /// <summary>
        /// ダメージを与える
        /// </summary>
        public void ApplyDamage(Damage damage)
        {
            _damageSubject.OnNext(damage);
        }

        /// <summary>
        /// 即死
        /// </summary>
        public void Kill(IAttacker attacker)
        {
            if (attacker is PlayerAttacker)
            {
                //プレイヤが直接の原因の場合はそれを死因にする
                _deadSubject.OnNext(new DeadReason(this.PlayerId, attacker));
            }
            else
            {
                var at = _lastDamagedAttacker ?? NonPlayerAttacker.Default;
                _deadSubject.OnNext(new DeadReason(this.PlayerId, at));
            }
            _isAlive.Value = false;
        }

        #endregion


    }
}
