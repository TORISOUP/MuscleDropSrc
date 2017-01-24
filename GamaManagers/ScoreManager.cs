using System.Collections.Generic;
using System.Linq;
using Assets.MuscleDrop.Scripts.Attacks;
using Assets.MuscleDrop.Scripts.Attacks.AttackerImpls;
using Assets.MuscleDrop.Scripts.Players;
using UnityEngine;
using UniRx;
namespace Assets.MuscleDrop.Scripts.GamaManagers
{
    /// <summary>
    /// 点数計算する
    /// </summary>
    public class ScoreManager : MonoBehaviour
    {
        private ReactiveCollection<DeadReason> deadReasons
            = new ReactiveCollection<DeadReason>();

        public void Register(PlayerCore player)
        {
            player.OnDead
                .Subscribe(d => deadReasons.Add(d))
                .AddTo(gameObject);
        }


        /// <summary>
        /// プレイヤ毎の死因一覧
        /// </summary>
        public Dictionary<PlayerId, IEnumerable<IAttacker>> GetPlayerDeadReasons()
        {
            return deadReasons.AsEnumerable()
                .GroupBy(x => x.DeadPlayerId, x => x.Attacker)
                .ToDictionary(g => g.Key, g => g.AsEnumerable());
        }

        /// <summary>
        /// プレイヤ毎の倒したプレイヤリスト
        /// </summary>
        public Dictionary<PlayerId, IEnumerable<PlayerId>> GetPlayerKillSocre()
        {
            return deadReasons.Where(x => x.Attacker is PlayerAttacker)
                  .Select(x => new { AttackerId = ((PlayerAttacker)x.Attacker).PlayerId, Dead = x.DeadPlayerId })
                  .GroupBy(x => x.AttackerId, x => x.Dead)
                  .ToDictionary(x => x.Key, x => x.AsEnumerable());
        }
    }
}
