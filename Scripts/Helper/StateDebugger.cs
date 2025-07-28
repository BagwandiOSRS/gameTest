using System.Collections.Generic;
using UnityEngine;

public class StateDebugger : MonoBehaviour
{
    [SerializeField] private Transform modelRoot;
    [SerializeField] private Renderer modelRenderer; 
    private static readonly Dictionary<PlayerStateType, Color> stateColors = new()
    {
        { PlayerStateType.Idling,         Color.white },
        { PlayerStateType.Aiming,         Color.yellow },
        { PlayerStateType.Reloading,      Color.grey },
        { PlayerStateType.Swapping,       Color.magenta },
        { PlayerStateType.Moving,         Color.green },
        { PlayerStateType.Turning,        Color.red },
        { PlayerStateType.Firing,         Color.red },
        { PlayerStateType.Dashing,        Color.magenta },
        { PlayerStateType.ForwardDashing, Color.black },
        { PlayerStateType.Stunned,        Color.black },
        { PlayerStateType.AllStates,      Color.white }
    };
    public void SetState(PlayerStateType state)
    {
        if (!stateColors.TryGetValue(state, out var color))
        {
            Debug.LogWarning($"No color assigned for state: {state}");
            return;
        }

        modelRenderer.material.color = color;
    }
    void Awake()
    {
        if (modelRenderer == null && modelRoot != null)
        {
            modelRenderer = modelRoot.GetComponentInChildren<Renderer>();
        }
    }
}
