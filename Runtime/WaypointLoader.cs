using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class WaypointLoader
{
    public static Dictionary<int, Vector2> Load()
    {
        var path = Path.Combine(Application.streamingAssetsPath, "waypoints.bytes");
        using var reader = new BinaryReader(File.OpenRead(path));

        int count = reader.ReadInt32();
        var dict = new Dictionary<int, Vector2>(count);

        for (int i = 0; i < count; i++)
        {
            int id = reader.ReadInt32();
            float x = reader.ReadSingle();
            float y = reader.ReadSingle();
            dict[id] = new Vector2(x, y);
        }
        return dict;
    }
}