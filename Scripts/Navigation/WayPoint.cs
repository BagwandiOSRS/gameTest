using UnityEngine;

public struct WayPoint
{
    public Vector2 position;
    // compile time constant w/ constructor fallback CS1736
    public WayPoint(Vector2 p)
    {
        position = p;
    }
}