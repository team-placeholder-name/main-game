using UnityEngine;

public class CameraData
{
    private Camera camera;

    public CameraData()
    {
        camera = Camera.main;
    }

    public Vector3 GetCameraPosition()
    {
        return camera.transform.position;
    }

    public Quaternion GetCameraRotation()
    {
        return camera.transform.rotation;
    }
}