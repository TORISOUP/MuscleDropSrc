using System.Linq;
using Assets.MuscleDrop.Scripts.Utilities.ExtensionMethods;
using UnityEngine;

namespace Assets.MuscleDrop.Scripts.Players
{
    public class PantsChanger : BasePlayerComponent
    {
        [SerializeField] private Renderer rendere;
        protected override void OnInitialize()
        {
            foreach (var m in rendere.materials.Where(x=>x.name.Contains("Pants")))
            {
                m.color = PlayerId.ToColor();
            }
        }
    }
}
