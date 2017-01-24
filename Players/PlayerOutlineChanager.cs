using HighlightingSystem;
using UnityEngine;
using UniRx;
namespace Assets.MuscleDrop.Scripts.Players
{
    [RequireComponent(typeof(Highlighter))]
    public class PlayerOutlineChanager : BasePlayerComponent
    {
        private Highlighter highlighter;

        protected override void OnInitialize()
        {
            highlighter = GetComponent<Highlighter>();

            Core.IsAlive.Subscribe(x =>
            {
                if (x)
                {
                    highlighter.ConstantOnImmediate(this.PlayerId.ToColor());
                }
                else
                {
                    highlighter.ConstantOff();
                }
            });
        }

    }
}
