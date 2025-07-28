using UnityEngine;

public class ReloadCamera : CameraState
{
    public PlayerData playerData;
    [SerializeField] private Vector2 turn;
    public static ReloadCamera Instance;
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        playerData = PlayerData.Instance;
    }
    public override void OnEnter()
    {
        CameraManager.Instance.transform.SetPositionAndRotation(
            transform.position + playerData.ActiveItem.ReloadPosition,
            transform.rotation * Quaternion.Euler(playerData.ActiveItem.ReloadEuler)
        );
    }
}