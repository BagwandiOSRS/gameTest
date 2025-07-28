using UnityEngine;
using static PlayerStateType;

public enum PlayerStateType
{
    Idling,
    Aiming,
    Reloading,
    Moving,
    Turning,
    Firing,
    Dashing,
    ForwardDashing,
    Stunned,
    Jumping,
    Swapping,
    AllStates
}
public class PlayerStateManager : MonoBehaviour
{
    [SerializeField] StateDebugger stateDebugger;
    private PlayerStateType CurrentState = Idling;
    public PlayerStateType PreviousState = Idling;
    public PlayerStateType PreviousDiffState = Idling;
    private PlayerState[] playerStateArray;
    public static PlayerStateManager Instance;
    protected static PlayerData playerData;
    protected static Camera mainCamera;
    protected static CameraManager cameraManager;
    protected static PlayerInputManager playerInputManager;
    private PlayerMovement playerMovement;
    private float stateTimer;
    private bool didChange;
    private void Awake()
    {
        Instance = this;
        playerMovement = GetComponent<PlayerMovement>();
    }
    private void Start()
    {
        if (playerInputManager == null)
        {
            playerInputManager = PlayerInputManager.Instance;
        }
        if (playerData == null)
        {
            playerData = PlayerData.Instance;
        }
        if (cameraManager == null)
        {
            mainCamera = Camera.main;
            // movementCamera = mainCamera.GetComponent<MovementCamera>();
            cameraManager = CameraManager.Instance;
        }
        InitializeActionStates();
    }
    private void InitializeActionStates()
    {
        playerStateArray = PlayerStateFactory.CreateStates(
            cameraManager,
            playerData,
            playerMovement,
            GetComponent<FirstPersonCamera>(),
            GetComponent<ReloadCamera>(),
            () => CurrentState,
            () => PreviousState
        );
    }
    void Update()
    {
        didChange = false;
        InputsImage inputs = playerInputManager.GetInputs();
        // Debug.Log(inputs);
        PreviousState = CurrentState;
        if (stateTimer > 0f)
        {
            // todo reread deltatime
            stateTimer -= Time.deltaTime;
        }
        if (stateTimer <= 0f)
            CurrentState = Idling;

        // order dictates input priority
        if (inputs.MoveKey && IsStateAllowed(Moving))
            CurrentState = Moving;

        if (inputs.AimKey && IsStateAllowed(Aiming))
            CurrentState = Aiming;

        if (inputs.ReloadKey && IsStateAllowed(Reloading))
            CurrentState = Reloading;
            
        if (inputs.SwapKey && IsStateAllowed(Swapping))
            CurrentState = Swapping;

        if (inputs.FireKey && IsStateAllowed(Firing))
                CurrentState = Firing;

        if (CurrentState == Idling && inputs.TurnKey)
            CurrentState = Turning;

        if (CurrentState == Moving && inputs.TurnKey)
            CurrentState = Dashing;

        if (CurrentState == Turning && inputs.MoveKey && inputs.TurnKey)
            CurrentState = Dashing;

        if (PreviousState != CurrentState)
            Transition(CurrentState, false);
        playerStateArray[(int)CurrentState].Execute();
        // Debug.Log(CurrentState);
    }
    private void Transition(PlayerStateType newState, bool injected)
    {
        // reconciliation between native lifecycle update hook and injection hooks
        if (didChange) return;
        if (injected)
            CurrentState = newState;
        didChange = true;
        // stateDebugger.SetState(newState);
        PreviousDiffState = PreviousState;
        playerStateArray[(int)PreviousDiffState].Exit();
        playerStateArray[(int)newState].Enter();
        stateTimer = playerStateArray[(int)newState].Duration;
    }
    private bool IsStateAllowed(PlayerStateType newState) =>
        ArrayContains(playerStateArray[(int)newState].AllowedFrom, CurrentState);
    private bool CanStateInterrupt(PlayerStateType newState) =>
        ArrayContains(playerStateArray[(int)CurrentState].InterruptedBy, newState);
    private bool ArrayContains(PlayerStateType[] array, PlayerStateType target)
    {
        for (int i = 0; i < array.Length; i++)
            if (array[i] == target)
                return true;
        return false;
    }
    public void SetState(PlayerStateType injectedState)
    {
        if (CanStateInterrupt(injectedState))
        {
            Transition(injectedState, true);
        }
        #if UNITY_EDITOR
            else
            {
                Debug.Log(injectedState + " cannot interrupt " + CurrentState);
            }
        #endif
    }
}