using UnityEngine;

namespace PastelUtilities
{
    public static class UtilityFunctions
    {
        public static bool VectorPointIntersect(Vector3 origin, Vector3 direction, Vector3 pointOfInterest, float tolerance)
        {
            if (direction.x == 0 && direction.y == 0 && direction.z == 0)
            {
                // Degenerate case, should never happen but we should guard against it anyway
                return false;
            }

            float tX, tY, tZ;
            Vector3 remainderVector = origin - pointOfInterest;
            if (direction.x == 0 || direction.y == 0 || direction.z == 0)
            {
                // There should never be 0 values in these directions
                return false;
            }
            else
            {
                tX = remainderVector.x / direction.x;
                tY = remainderVector.y / direction.y;
                tZ = remainderVector.z / direction.z;

                // Tolerance checks
                bool tYIsApproxTx = tX - tolerance * 0.5f < tY && tY < tX + tolerance * 0.5f;
                bool tYisApproxTz = tZ - tolerance * 0.5f < tY && tY < tZ + tolerance * 0.5f;
                bool tXisApproxTz = tZ - tolerance * 0.5f < tX && tX < tZ + tolerance * 0.5f;

                // Are all points approximately consistent? If they are, the vector intersects the point of interest
                return tYIsApproxTx && tYisApproxTz && tXisApproxTz;
            }
        }
    }
}
