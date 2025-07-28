using UnityEngine;

public class CameraWall : MonoBehaviour
{
    // [SerializeField] private CameraState positiveWayPoint;
    // [SerializeField] private CameraState negativeWayPoint;
    public string positiveWayPoint;
    public string negativeWayPoint;
    private float angleTolerance = 0.25f;
    private void OnTriggerExit(Collider other)
    {
        // !!! hardcoded layer index
        if (other.gameObject.layer != 6) return;

        Vector2 playerXZ = new(
            other.transform.position.x,
            other.transform.position.z
        );

        Vector2 wallXZ = new(
            transform.position.x,
            transform.position.z
        );

        Vector2 playerDirection = (playerXZ - wallXZ).normalized;

        Vector3 wallForward3D = transform.forward;
        Vector2 frontOfWall = new Vector2(wallForward3D.x, wallForward3D.z).normalized;

        float dot = Vector2.Dot(frontOfWall, playerDirection);

        if (dot > angleTolerance)
        {
            CameraManager.Instance.SetWayPointNew(positiveWayPoint);
        }
        else if (dot < -angleTolerance)
        {
            CameraManager.Instance.SetWayPointNew(negativeWayPoint);
        }
        else
        {
            Debug.LogError("player went in sideways");
        }
    }
    #if UNITY_EDITOR
        void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward);
        }
    #endif
}
