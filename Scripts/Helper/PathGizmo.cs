using UnityEngine;
public class PathGizmo : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, 1.5f); // Customize shape and size
    }
}