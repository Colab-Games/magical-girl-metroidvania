using UnityEngine;

namespace Math2D
{
    public static class Vector2Extension
    {
        public static Vector3 ToVector3OnXY(this Vector2 v2, float z)
        {
            return new Vector3(v2.x, v2.y, z);
        }

        public static Vector3 ToVector3OnXZ(this Vector2 v2, float y)
        {
            return new Vector3(v2.x, y, v2.y);
        }

        public static Vector3 ToVector3OnYZ(this Vector2 v2, float x)
        {
            return new Vector3(x, v2.x, v2.y);
        }
    }
}
