using UnityEngine;

public abstract class CameraState : MonoBehaviour
{
    public virtual void OnEnter()
    {
        CameraManager.Instance.transform.SetPositionAndRotation(
            transform.position,
            transform.rotation
        );
    }
    public virtual void OnUpdate() { }
    public virtual void OnExit() { }
}