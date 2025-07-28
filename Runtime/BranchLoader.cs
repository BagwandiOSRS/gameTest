using System.Collections.Generic;
using System.IO;
using UnityEngine;

public struct BranchChoiceRuntime
{
    public string choiceName;
    public int[] pathSequence;

    public BranchChoiceRuntime(string name, int[] path)
    {
        choiceName = name;
        pathSequence = path;
    }
}

public static class BranchLoader
{
    public static Dictionary<int, BranchChoiceRuntime[]> Load()
    {
        var path = Path.Combine(Application.streamingAssetsPath, "branches.bytes");
        using var reader = new BinaryReader(File.OpenRead(path));

        int branchCount = reader.ReadInt32();
        var dict = new Dictionary<int, BranchChoiceRuntime[]>(branchCount);

        for (int i = 0; i < branchCount; i++)
        {
            int branchId = reader.ReadInt32();
            int choiceCount = reader.ReadInt32();

            var choices = new BranchChoiceRuntime[choiceCount];

            for (int j = 0; j < choiceCount; j++)
            {
                string choiceName = reader.ReadString();
                int pathLength = reader.ReadInt32();
                int[] pathHashes = new int[pathLength];

                for (int k = 0; k < pathLength; k++)
                {
                    pathHashes[k] = reader.ReadInt32();
                }

                choices[j] = new BranchChoiceRuntime(choiceName, pathHashes);
            }

            dict[branchId] = choices;
        }

        return dict;
    }
}
