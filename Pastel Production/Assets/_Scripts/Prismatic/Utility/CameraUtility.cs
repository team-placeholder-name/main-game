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

    public static float AdjustAngle(float change, float current, float min, float max)
    {
        if (max == 0)
        {
            return current + change;
        }
        
        return Mathf.Clamp(current + change, min, max);
    }

    public static Quaternion GetView(SimulationData data, Vector3 entityCenter, float groundDistance)
    {
        
        Physics.Raycast(entityCenter, Vector3.down, out RaycastHit hit, groundDistance, 1 << LayerMask.NameToLayer("Default"));
        Vector3 movementNormal = hit.normal;

        Vector3 viewForward = (data.ViewTarget - data.ViewPosition);
        viewForward.y = 0;
        viewForward = viewForward.normalized;
        Vector3 moveForward = Vector3.ProjectOnPlane(viewForward, movementNormal).normalized;
        return Quaternion.LookRotation(moveForward, Vector3.up);
    }

    public static float DetermineDistance(SimulationData data, float distance)
    {
        float t = Mathf.Sin(Mathf.Deg2Rad * data.XYAngles.y);
        return Mathf.Lerp(2.5f, distance, t);
    }

    public static float DetermineFOV(float cameraAngle)
    {
        float inverse = 1.0f - Mathf.Sin(Mathf.Deg2Rad * cameraAngle);
        return Mathf.Lerp(60, 90, inverse); ;
    }
}
