using UnityEngine;

public class FirstPersonCamera : CameraState
{
    [SerializeField] Vector2 turn = new(0,0);
    public override void OnEnter()
    {
        turn = new(0, 0);
        // CameraManager.Instance.transform.SetPositionAndRotation(
        //     PlayerStateManager.Instance.transform.position + Vector3.up,
        //     PlayerStateManager.Instance.transform.rotation
        // );
    }
    public override void OnUpdate()
    {
        // todo this makes the camera not look where the player is looking
        /*
        float sensitivity = 300f;
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        turn.x += mouseX;
        turn.y += mouseY;
        turn.y = Mathf.Clamp(turn.y, -85f, 85f);
        cam.transform.localRotation = Quaternion.Euler(-turn.y, turn.x, 0f);
        */
    }
}