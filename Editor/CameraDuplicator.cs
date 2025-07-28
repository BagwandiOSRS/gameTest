using UnityEngine;
using UnityEditor;

public class CameraDuplicator : MonoBehaviour
{
    [MenuItem("Tools/Duplicate GameObject on Z Axis")]
    public static void DuplicateGameObject()
    {
        GameObject original = Selection.activeGameObject;
        if (original == null)
        {
            Debug.LogError("Select a GameObject in the hierarchy before running this.");
            return;
        }

        for (int i = 1; i <= 48; i++)
        {
            GameObject clone = Instantiate(original);
            clone.name = original.name + "_Clone_" + i;
            clone.transform.position = original.transform.position + new Vector3(0, 0, 2 * i);

            var wpComp = clone.GetComponent<CameraWall>();
            if (wpComp != null)
            {
                wpComp.positiveWayPoint = $"waypoint{i + 1}";
                wpComp.negativeWayPoint = $"waypoint{i}";
            }
            Undo.RegisterCreatedObjectUndo(clone, "Duplicate GameObject");
        }
    }
}
