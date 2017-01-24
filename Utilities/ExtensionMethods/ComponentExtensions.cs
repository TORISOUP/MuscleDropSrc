using System.Collections.Generic;
using UnityEngine;

namespace Assets.MuscleDrop.Scripts.Utilities.ExtensionMethods
{
    public static class ComponentExtensions
    {
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            var component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }

            return component;
        }


        public static IEnumerable<Transform> GetChildren(this Transform t)
        {
            var i = 0;

            while (i < t.childCount)
            {
                yield return t.GetChild(i);
                ++i;
            }
        }
    }
}
