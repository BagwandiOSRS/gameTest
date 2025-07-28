using UnityEngine;

public class RotateCameraMarker : CameraMarker
{
    public Transform target;
    public override ICameraState Bake()
    {
        return new RotateCamera(
            transform.position,
            transform.rotation,
            target.transform
        );
    }
}