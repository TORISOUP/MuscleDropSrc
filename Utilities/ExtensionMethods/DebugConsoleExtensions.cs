using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace Assets.MuscleDrop.Scripts.Utilities.ExtensionMethods
{
    public static class DebugConsoleExtensions
    {
        [Conditional("UNITY_EDITOR")]
        public static void Red(this object t)
        {
            Debug.Log(string.Format("<color=red>{0}</color>", t));
        }

        [Conditional("UNITY_EDITOR")]
        public static void Green(this object t)
        {
            Debug.Log(string.Format("<color=green>{0}</color>", t));
        }

        [Conditional("UNITY_EDITOR")]
        public static void Cyan(this object t)
        {
            Debug.Log(string.Format("<color=cyan>{0}</color>", t));
        }

        [Conditional("UNITY_EDITOR")]
        public static void Yellow(this object t)
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", t));
        }

        [Conditional("UNITY_EDITOR")]
        public static void Orange(this object t)
        {
            Debug.Log(string.Format("<color=orange>{0}</color>", t));
        }

        [Conditional("UNITY_EDITOR")]
        public static void Magenta(this object t)
        {
            Debug.Log(string.Format("<color=magenta>{0}</color>", t));
        }

        [Conditional("UNITY_EDITOR")]
        public static void Blue(this object t)
        {
            Debug.Log(string.Format("<color=blue>{0}</color>", t));
        }
    }
}
