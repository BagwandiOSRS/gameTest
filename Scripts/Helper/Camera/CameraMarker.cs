using UnityEngine;

public abstract class CameraMarker : MonoBehaviour
{
    // todo more efficient than this?
    public string id;
    public abstract ICameraState Bake();
}