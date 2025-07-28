using UnityEngine;

public class EmptyGizmo : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1.0f); // Customize shape and size
    }
}