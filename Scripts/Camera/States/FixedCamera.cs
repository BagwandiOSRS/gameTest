using UnityEngine;

public struct FixedCamera : ICameraState
{
    private Vector3 position;
    private Quaternion rotation;
    public FixedCamera(Vector3 p, Quaternion r)
    {
        position = p;
        rotation = r;
    }
    public void OnEnter(Transform cam)
    {
        cam.SetPositionAndRotation(position, rotation);
    }
    public void OnExit(Transform cam){ }
    public void OnUpdate(Transform cam){ }
}