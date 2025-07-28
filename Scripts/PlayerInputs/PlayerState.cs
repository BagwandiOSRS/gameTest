using System;

public struct PlayerState
{
    // Name of the state as a gerund
    public string Name { get; }
    // How long the state lasts without other inputs
    public float Duration { get; set; }
    // which states can we transition to this state from?
    public readonly PlayerStateType[] AllowedFrom;
    // if the state has a duration, what other states can interrupt it?
    public readonly PlayerStateType[] InterruptedBy;
    // hooks
    private Action _onEnter;
    private Action _onUpdate;
    private Action _onExit;
    public PlayerState(
        string name,
        float duration,
        PlayerStateType[] allowedFrom,
        PlayerStateType[] interruptedBy,
        Action onEnter = null,
        Action onUpdate = null,
        Action onExit = null
        )
    {
        Name = name;
        Duration = duration;
        AllowedFrom = allowedFrom;
        InterruptedBy = interruptedBy;
        _onEnter = onEnter ?? (() => { });
        _onUpdate = onUpdate ?? (() => { });
        _onExit = onExit ?? (() => { });
    }
    public void Enter() => _onEnter.Invoke();
    public void Execute() => _onUpdate.Invoke();
    public void Exit() => _onExit.Invoke();
}