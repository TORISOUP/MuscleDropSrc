using System.Collections.Generic;
using UnityEngine;

namespace Assets.MuscleDrop.Scripts.Utilities.ExtensionMethods
{
    public static class Vector3Extensions
    {
        public static Vector3 SetX(this Vector3 original, float x)
        {
            return new Vector3(x, original.y, original.z);
        }

        public static Vector3 SetY(this Vector3 original, float y)
        {
            return new Vector3(original.x, y, original.z);
        }

        public static Vector3 SetZ(this Vector3 original, float z)
        {
            return new Vector3(original.x, original.y, z);
        }

        public static Vector3 SuppressY(this Vector3 original)
        {
            return original.SetY(0);
        }

        public static float Length(this Vector3 original, Vector3 target)
        {
            return Vector3.Magnitude(original - target);
        }

        public static float SqrLength(this Vector3 original, Vector3 target)
        {
            return Vector3.SqrMagnitude(original - target);
        }

        /// <summary>
        /// ベクトルの水平成分のみを取り出して正規化する
        /// </summary>
        public static Vector3 ToHorizontalDirection(this Vector3 original)
        {
            return original.SetY(0).normalized;
        }

        /// <summary>
        /// 中心座標を返す
        /// </summary>
        public static Vector3 CenterPosition(this IEnumerable<Vector3> originals)
        {
            float sx = 0, sy = 0, sz = 0;
            var lenght = 0;
            foreach (var original in originals)
            {
                sx += original.x;
                sy += original.y;
                sz += original.z;
                lenght++;
            }
            if (lenght == 0) return Vector3.zero;
            return new Vector3(sx, sy, sz) / (float)lenght;
        }
    }
}
