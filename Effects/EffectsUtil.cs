using UnityEngine;

namespace Assets.MuscleDrop.Scripts.Effects
{
    public class EffectEmmiter
    {
        /// <summary>
        /// エフェクトの事前読み込み
        /// </summary>
        public static void PreLoad()
        {
            var effectTypes = System.Enum.GetValues(typeof(EffectType));
            foreach (var effectType in effectTypes)
            {
                EffekseerSystem.LoadEffect(effectType.ToString());
            }
        }

        /// <summary>
        /// エフェクトを再生する
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="effectType"></param>
        public static void FireEffect(Vector3 position, EffectType effectType)
        {
            var prefabPath = "Effects/" + effectType.ToString();
            var prefab = Resources.Load<GameObject>(prefabPath);

            var go = GameObject.Instantiate(prefab, position, Quaternion.identity);
            go.transform.localScale = Vector3.one;
        }

        /// <summary>
        /// 全てのエフェクトを停止する
        /// </summary>
        public static void StopEffects()
        {
            // AutoDestroyが設定されていたら自動的に消えてくれる。(それ以外は残るので自前で管理すること)
            EffekseerSystem.StopAllEffects();
        }
    }
}
