using UnityEngine;

public struct RotateCamera : ICameraState
{
    private Vector3 position;
    private Quaternion rotation;
    private Transform target;
    public RotateCamera(Vector3 p, Quaternion r, Transform t)
    {
        position = p;
        rotation = r;
        target = t;
    }
    public void OnEnter(Transform cam)
    {
        cam.SetPositionAndRotation(position, rotation);
    }
    public void OnExit(Transform cam){ }
    public void OnUpdate(Transform cam)
    {
        cam.transform.LookAt(target);
    }
}