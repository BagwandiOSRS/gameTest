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
    
    // Performance constants
    private const float SQR_ARRIVAL_THRESHOLD = 1f;
    private const float ROTATION_SPEED = 500f;
    private const float ROTATION_THRESHOLD = 0.5f;
    private const int INITIAL_PATH_CAPACITY = 32;
    
    // Cached values to reduce allocations
    private Vector2 playerXZ = new Vector2();
    private Vector2 tempVector2 = new Vector2();
    private List<Vector2> pathList = new List<Vector2>(INITIAL_PATH_CAPACITY);
    
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
        // Reuse cached Vector2 to avoid allocation
        playerXZ.x = parentRoot.position.x;
        playerXZ.y = parentRoot.position.z;
        
        tempVector2.x = targetPos.x - playerXZ.x;
        tempVector2.y = targetPos.y - playerXZ.y;
        tempVector2.Normalize();
        
        cachedDirection.x = tempVector2.x;
        cachedDirection.y = 0f;
        cachedDirection.z = tempVector2.y;
        
        targetRotation = Quaternion.LookRotation(cachedDirection, Vector3.up);
    }
    
    public void SetDirection()
    {
        CacheRotationTowards(path[destination]);
        parentRoot.rotation = targetRotation;
    }
    
    public void Move()
    {
        // Reuse cached Vector2
        playerXZ.x = parentRoot.position.x;
        playerXZ.y = parentRoot.position.z;
        
        tempVector2.x = path[destination].x - playerXZ.x;
        tempVector2.y = path[destination].y - playerXZ.y;
        float sqrDist = tempVector2.x * tempVector2.x + tempVector2.y * tempVector2.y;

        if (sqrDist < SQR_ARRIVAL_THRESHOLD)
        {
            if (destination == 0 || destination == path.Length - 1) return;

            destination += Direction;
            CacheRotationTowards(path[destination]);
            isRotating = true;
        }

        parentRoot.position += speed * Time.deltaTime * cachedDirection;

        if (isRotating)
        {
            parentRoot.rotation = Quaternion.RotateTowards(parentRoot.rotation, targetRotation, ROTATION_SPEED * Time.deltaTime);

            if (Quaternion.Angle(parentRoot.rotation, targetRotation) < ROTATION_THRESHOLD)
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
        // Use List to avoid frequent array reallocations
        pathList.Clear();
        if (pathList.Capacity < wpKeys.Length + 1)
        {
            pathList.Capacity = wpKeys.Length + 1;
        }
        
        for (int i = 0; i < wpKeys.Length; i++)
        {
            pathList.Add(wpDict[wpKeys[i]]);
        }
        pathList.Add(wpDict[branchKey]);
        
        // Convert to array only when necessary
        path = pathList.ToArray();
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
        Vector2 currentBranchPos = wpDict[activeBranchId]; // Cache this lookup

        foreach (var choice in choices)
        {
            int[] keys = choice.pathSequence;
            if (keys.Length < 2) continue;

            // Cache dictionary lookups
            Vector2 choiceStart = wpDict[keys[0]];
            Vector2 choiceEnd = wpDict[keys[^1]];
            
            // Optimized comparison logic
            bool valid = (choiceStart.x == currentBranchPos.x && choiceStart.y == currentBranchPos.y) ?
                (choiceEnd.x == comparator.x && choiceEnd.y == comparator.y) :
                (choiceEnd.x == path[^2].x && choiceEnd.y == path[^2].y);

            if (valid)
                return choice.choiceName;
        }

        Debug.LogWarning("filter error");
        return "";
    }
}