using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float limit;
    [SerializeField] private int destination = 1;
    [SerializeField] private int cachedPlayerdestination;
    private float speed = 10.0f;
    private LayerMask groundLayer;
    private Transform parentRoot;
    private Transform modelRoot;
    public static PlayerMovement Instance;
    private Vector3 cachedDirection;
    public Transform waypointTransform;
    public int Direction = 1;
    private Quaternion targetRotation;
    private bool isRotating = false;
    public Vector2[] path;
    private PathBrowser pathBrowser;
    public bool DisplayToggle = true;
    public bool InCollider = true;
    private Dictionary<int, Vector2> wpDict = new();
    private Dictionary<int, BranchChoiceRuntime[]> branchDict = new();
    private int activeBranchId;
    [SerializeField] private int[] testPathKeys;
    [SerializeField] private int testBranchKey;
    public int LastBranchId { get; private set; }
    void Awake()
    {
        Instance = this;
        parentRoot = transform.parent;
        modelRoot = parentRoot.Find("PlayerModelRoot");
        groundLayer = LayerMask.GetMask("Ground");
        AlignToGround();
        wpDict = WaypointLoader.Load();
        branchDict = BranchLoader.Load();

        if (testPathKeys != null && testPathKeys.Length > 0)
        {
            UpdatePath(testPathKeys, testBranchKey);
            destination = path.Length - 2;
            SetDirection();
        }
    }
    void AlignToGround()
    {
        Vector3 origin = parentRoot.position + Vector3.up;
        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 2f, groundLayer))
        {
            Vector3 pos = parentRoot.position;
            pos.y = hit.point.y;
            parentRoot.position = pos;
        }
    }
    void CacheRotationTowards(Vector2 targetPos)
    {
        Vector2 playerXZ = new(parentRoot.position.x, parentRoot.position.z);
        Vector2 dirXZ = (targetPos - playerXZ).normalized;
        cachedDirection = new Vector3(dirXZ.x, 0f, dirXZ.y);
        targetRotation = Quaternion.LookRotation(cachedDirection, Vector3.up);
    }
    public void SetDirection()
    {
        CacheRotationTowards(path[destination]);
        parentRoot.rotation = targetRotation;
    }
    public void Move()
    {
        Vector2 playerXZ = new(parentRoot.position.x, parentRoot.position.z);
        float sqrDist = (path[destination] - playerXZ).sqrMagnitude;

        if (sqrDist < 1)
        {
            if (destination == 0 || destination == path.Length - 1) return;

            destination += Direction;
            CacheRotationTowards(path[destination]);
            isRotating = true;
        }

        parentRoot.position += speed * Time.deltaTime * cachedDirection;

        if (isRotating)
        {
            parentRoot.rotation = Quaternion.RotateTowards(parentRoot.rotation, targetRotation, 500 * Time.deltaTime);

            if (Quaternion.Angle(parentRoot.rotation, targetRotation) < 0.5f)
            {
                parentRoot.rotation = targetRotation;
                isRotating = false;
            }
        }
    }
    public void Turn()
    {
        Direction *= -1;
        destination += Direction;
        SetDirection();

        if (InCollider)
        {
            if (Direction == cachedPlayerdestination)
                PathBrowser.Instance.DisplayButtons();
            else
                PathBrowser.Instance.DestroyAllChildren();
        }
    }
    void UpdatePath(int[] wpKeys, int branchKey)
    {
        path = new Vector2[wpKeys.Length + 1];
        for (int i = 0; i < wpKeys.Length; i++)
        {
            path[i] = wpDict[wpKeys[i]];
        }
        path[wpKeys.Length] = wpDict[branchKey];
    }
    public void Choose(int choice)
    {
        var selected = branchDict[activeBranchId][choice];
        UpdatePath(selected.pathSequence, activeBranchId);
        Direction = -1;
        destination = path.Length - 2;
        SetDirection();
    }
    public void CacheDirection()
    {
        cachedPlayerdestination = Direction;
    }
    public void SetActiveBranch(int branchId)
    {
        activeBranchId = branchId;
        LastBranchId = branchId;
    }
    public string FilterChoices()
    {
        var choices = branchDict[activeBranchId];
        if (path.Length < 2) return "";

        Vector2 comparator = Direction == 1 ? path[^2] : path[1];

        foreach (var choice in choices)
        {
            int[] keys = choice.pathSequence;
            if (keys.Length < 2) continue;

            // if the wp directly behind you equals the first wp of a loop, filter it
            Vector2 end = wpDict[keys[^1]];
            bool valid = wpDict[keys[0]] == wpDict[activeBranchId] ?
                end == comparator :
                end == path[^2];

            if (valid)
                return choice.choiceName;
        }

        Debug.LogWarning("filter error");
        return "";
    }

}