using UnityEngine;
using UnityEditor;

public class WayPointDuplicator : MonoBehaviour
{
    [MenuItem("Tools/Duplicate With Camera Marker")]
    public static void DuplicateCameraWaypoints()
    {
        GameObject original = Selection.activeGameObject;
        GameObject playerRoot = GameObject.Find("PlayerRoot");
        if (original == null)
        {
            Debug.LogError("Select a GameObject in the hierarchy before running this.");
            return;
        }
        Transform playerTransform = playerRoot.transform;
        for (int i = 1; i <= 98; i++)
        {
            GameObject clone = Instantiate(original);
            clone.name = $"{original.name}_Waypoint_{i}";
            clone.transform.position = original.transform.position + new Vector3(0, 0, 5 * i);

            string waypointId = $"waypoint{i}";

            if (i % 2 == 0)
            {
                RotateCameraMarker rotateMarker = clone.AddComponent<RotateCameraMarker>();
                rotateMarker.id = waypointId;
                rotateMarker.target = playerTransform;
            }
            else
            {
                FixedCameraMarker fixedMarker = clone.AddComponent<FixedCameraMarker>();
                fixedMarker.id = waypointId;
            }

            Undo.RegisterCreatedObjectUndo(clone, "Created Waypoint Clone");
        }
    }
}
