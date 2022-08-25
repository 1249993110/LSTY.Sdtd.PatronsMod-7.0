using UnityEngine;

namespace LSTY.Sdtd.PatronsMod.Extensions
{
    internal static class Vector3Extension
    {
        public static Position ToPosition(this Vector3 v)
        {
            return new Position(v.x, v.y, v.z);
        }

        public static Position ToPosition(this Vector3i v)
        {
            return new Position(v.x, v.y, v.z);
        }
    }
}