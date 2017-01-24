using Assets.MuscleDrop.Scripts.Players.Inputs;
using UniRx;
using UnityEngine;

namespace Assets.MuscleDrop.Scripts.Players
{
    /// <summary>
    /// プレイヤのComponentの基底クラス
    /// </summary>
    public abstract class BasePlayerComponent : MonoBehaviour
    {
        private IInputEventProvider _inputEventProvider;

        #region Components
        //各コンポーネントで共通してよく使われるコンポーネント群はここにまとめる

        protected IInputEventProvider InputEventProvider { get { return _inputEventProvider; } }
        protected PlayerCore Core;
        protected PlayerId PlayerId { get { return Core.PlayerId; } }
        protected IReadOnlyReactiveProperty<PlayerParameters> CurrentPlayerParameter
        {
            get
            {
                return Core.CurrentPlayerParameter;
            }
        }
        #endregion
        private void Start()
        {
            Core = GetComponent<PlayerCore>();
            _inputEventProvider = GetComponent<IInputEventProvider>();

            //Coreの情報が確定したら初期化を呼び出す
            Core.OnInitializeAsync
                .Subscribe(_ => OnInitialize());

            OnStart();
        }

        /// <summary>
        /// Start() と同義
        /// </summary>
        protected virtual void OnStart() { }

        /// <summary>
        /// プレイヤ情報の初期化が完了した時に実行される
        /// </summary>
        protected abstract void OnInitialize();
    }
}
