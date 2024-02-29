using Prismatic;
using UnityEngine;
public static class CameraUtility 
{
    public static Vector3 CalculateLookAtDirection(SimulationData data, float distance, float height)
    {
        Vector3 pivotPosition = CalculateEyeLevel(data, height);
        Vector3 locationOffset = Quaternion.AngleAxis(data.XYAngles.x, Vector3.up) * Quaternion.AngleAxis(data.XYAngles.y, Vector3.right) * Vector3.back * distance;

        return pivotPosition + locationOffset;
    }

    public static Vector3 CalculateEyeLevel(SimulationData data, float height)
    {
        return data.currentEntity.Position + Vector3.up * height;
    }

    public static float AdjustVerticalAngle(float change, float current, float max)
    {
        return Mathf.Clamp(current + change, -0.55f * max, max);
    }
}
