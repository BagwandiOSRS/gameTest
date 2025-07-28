using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    public Transform player;
    [SerializeField] private CameraState injectedState;
    [SerializeField] private CameraState currentWayPoint;
    private Dictionary<string, ICameraState> stateDict;
    private ICameraState activeWayPoint;
    private Transform mainCamera;
    void Awake()
    {
        Profiler.BeginSample("Camera Awake");
        Instance = this;
        mainCamera = Camera.main.transform;
        if (player == null)
        {
            Debug.LogError("no player detetcted");
        }
        // todo efficient?
        stateDict = new Dictionary<string, ICameraState>();

        foreach (var marker in FindObjectsOfType<CameraMarker>())
        {
            stateDict.Add(marker.id, marker.Bake());
        }
        Profiler.EndSample();
    }
    void Start()
    {
        Profiler.BeginSample("Camera Start");
        if (activeWayPoint == null)
        {
            activeWayPoint = stateDict["pathtestcam"];
        }
        activeWayPoint.OnEnter(mainCamera);
        Profiler.EndSample();
    }
    void Update()
    {
        Profiler.BeginSample("Camera Update");
        if (injectedState != null)
        {
            injectedState.OnUpdate();
        }
        else
        {
            activeWayPoint?.OnUpdate(mainCamera);
        }
        Profiler.EndSample();
    }
    
    public void SetWayPoint(CameraState wayPoint)
    {
        currentWayPoint.OnExit();
        currentWayPoint = wayPoint;
        currentWayPoint.OnEnter();
    }
    public void SetWayPointNew(string stateID)
    {
        activeWayPoint.OnExit(mainCamera);
        activeWayPoint = stateDict[stateID];
        activeWayPoint.OnEnter(mainCamera);
    }
    public void Inject(CameraState newState)
    {
        if (injectedState != null)
            injectedState.OnExit();
        injectedState = newState;
        injectedState.OnEnter();
    }
    public void ExitInject()
    {
        if (injectedState != null)
        {
            injectedState.OnExit();
            injectedState = null;
            currentWayPoint.OnEnter();
        }
    }
}