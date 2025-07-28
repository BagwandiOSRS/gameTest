using UnityEngine;

public interface ICameraState
{
    void OnEnter(Transform cam);
    void OnUpdate(Transform cam);
    void OnExit(Transform cam);
}