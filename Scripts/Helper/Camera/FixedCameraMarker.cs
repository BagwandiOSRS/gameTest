public class FixedCameraMarker : CameraMarker
{
    public override ICameraState Bake()
    {
        return new FixedCamera(transform.position, transform.rotation);
    }
}