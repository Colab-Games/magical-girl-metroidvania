using UnityEngine;

namespace Math2D
{
    public static class Vector3Extension
    {
        public static Vector2 ToVector2OnXY(this Vector3 v3)
        {
            return new Vector2(v3.x, v3.y);
        }

        public static Vector2 ToVector2OnXZ(this Vector3 v3)
        {
            return new Vector2(v3.x, v3.z);
        }

        public static Vector2 ToVector2OnYZ(this Vector3 v3)
        {
            return new Vector2(v3.y, v3.z);
        }
    }
}
