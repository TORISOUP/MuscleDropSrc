using System.Collections;
using UniRx;
using UnityEngine;

namespace Assets.MuscleDrop.Scripts.GamaManagers
{
    public class GameTimeManager : MonoBehaviour
    {

        [SerializeField]
        private IntReactiveProperty _readyCountDownTime = new IntReactiveProperty(3);

        [SerializeField]
        private IntReactiveProperty _remainingTime = new IntReactiveProperty(120);

        /// <summary>
        /// ゲーム開始前のカウントダウン
        /// </summary>
        public IReadOnlyReactiveProperty<int> ReadyTime
        {
            get { return _readyCountDownTime; }
        }

        /// <summary>
        /// ゲームの残り時間
        /// </summary>
        public IReadOnlyReactiveProperty<int> RemainingTime
        {
            get { return _remainingTime; }
        }

        #region Ready

        /// <summary>
        /// ゲーム開始前の3,2,1のカウントダウンを開始する
        /// </summary>
        public void StartGameReadyCountDown()
        {
            StartCoroutine(ReadyCountCoroutine());
        }

        IEnumerator ReadyCountCoroutine()
        {
            yield return new WaitForSeconds(0.5f);

            _readyCountDownTime.SetValueAndForceNotify(_readyCountDownTime.Value);

            yield return new WaitForSeconds(1);
            while (_readyCountDownTime.Value > 0)
            {
                _readyCountDownTime.Value -= 1;
                yield return new WaitForSeconds(1);
            }
        }
        #endregion

        #region Battle

        /// <summary>
        /// ゲーム本編のタイマーのカウントを開始する
        /// </summary>
        public void StartBattleCountDown()
        {
            StartCoroutine(BattleCountDownCoroutine());
        }

        IEnumerator BattleCountDownCoroutine()
        {
            while (_remainingTime.Value > 0)
            {
                yield return new WaitForSeconds(1);
                _remainingTime.Value--;
            }
        }
        #endregion

    }
}
