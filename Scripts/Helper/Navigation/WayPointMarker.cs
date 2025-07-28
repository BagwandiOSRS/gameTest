using UnityEngine;

public class WayPointMarker : MonoBehaviour
{
    public string id;
    void Awake()
    {
        id = transform.name.ToLower();
    }
    public WayPoint Bake()
    {
        return new WayPoint(
            new(transform.position.x, transform.position.z)
        );
    }
}