using UnityEngine;

namespace Utilities
{
    public static class HelperMethods
    {
        public static Vector3 FloatToVectorAngle(float angle)
        {
            float rad = angle * (Mathf.PI/180f);
            return new Vector3(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;
        }

        public static float VectorToFloatAngle(Vector3 direction)
        {
            float angle = Mathf.Atan2(direction.normalized.y, direction.normalized.x) * Mathf.Rad2Deg;
            if (angle < 0) angle += 360;
            return angle;
        }
    }

}