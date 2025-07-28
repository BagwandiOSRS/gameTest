using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public static class WaypointBaker
{
    [MenuItem("Tools/Bake Branches and Waypoints")]
    public static void Bake()
    {
        var markers = GameObject.FindObjectsOfType<WayPointMarker>();
        string fn = "Assets/StreamingAssets/waypoints.bytes";
        string branchFn = "Assets/StreamingAssets/branches.bytes";
        using var w = new BinaryWriter(File.Open(fn, FileMode.Create));
        w.Write(markers.Length);
        List<BakedBranch> bakedBranches = new();
        foreach (var marker in markers)
        {

            int hash = PathUtils.Hash(marker.id.ToLower());
            Vector2 pos = marker.Bake().position;
            Debug.Log(hash + " " + pos);
            if (marker is BranchMarker branch)
            {
                string[] choiceNames = new string[branch.branches.Length];
                int[][] pathHashes = new int[branch.branches.Length][];

                for (int i = 0; i < branch.branches.Length; i++)
                {
                    BranchChoice choice = branch.branches[i];
                    choiceNames[i] = choice.choiceName.ToLower();
                    pathHashes[i] = new int[choice.pathSequence.Length];

                    for (int j = 0; j < choice.pathSequence.Length; j++)
                    {
                        pathHashes[i][j] = PathUtils.Hash(choice.pathSequence[j].ToLower());
                    }
                }

                BakedBranch newBranch = new BakedBranch(hash, choiceNames, pathHashes);
                bakedBranches.Add(newBranch);
            }

            w.Write(hash);
            w.Write(pos.x);
            w.Write(pos.y);
        }
        Debug.Log("Successfully baked " + markers.Length + " waypoints to: " + fn);

        using var bw = new BinaryWriter(File.Open(branchFn, FileMode.Create));
        // why do yall have to give it a different name
        bw.Write(bakedBranches.Count);
        foreach (var branch in bakedBranches)
        {
            bw.Write(branch.branchId);
            bw.Write(branch.choiceNames.Length);
            for (int i = 0; i < branch.choiceNames.Length; i++)
            {
                bw.Write(branch.choiceNames[i]);
                int[] path = branch.pathHashes[i];
                bw.Write(path.Length);
                foreach (int hash in path)
                {
                    bw.Write(hash);
                }
            }
        }
        Debug.Log("Successfully baked " + bakedBranches.Count + " waypoints to: " + branchFn);
    }
    // https://learn.microsoft.com/en-us/dotnet/api/system.object.gethashcode?view=net-9.0#system-object-gethashcode
    
}
struct BakedBranch
{
    public int branchId;
    public string[] choiceNames;
    public int[][] pathHashes;
    public BakedBranch(int id, string[] choices, int[][] hashes)
    {
        branchId = id;
        choiceNames = choices;
        pathHashes = hashes;
    }
}