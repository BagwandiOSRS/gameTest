using UnityEngine;

public class BranchMarker : WayPointMarker
{
    public BranchChoice[] branches;
    void Awake()
    {
        id = transform.name.ToLower();
        int hash = PathUtils.Hash(id); // keep the hash logic in one place
        string[] displayNames = new string[branches.Length];
        for (int i = 0; i < branches.Length; i++)
            displayNames[i] = branches[i].choiceName.ToLower();

        GetComponent<BranchCollider>().SetBranchData(hash, displayNames);
    }

}