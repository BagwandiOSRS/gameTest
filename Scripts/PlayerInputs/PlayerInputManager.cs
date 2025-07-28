using UnityEngine;

public struct InputsImage
{
    public bool AimKey;
    public bool MoveKey;
    public bool TurnKey;
    public bool FireKey;
    public bool ReloadKey;
    public bool SwapKey;
    public override readonly string ToString()
    {
        return $"Aim: {AimKey}, Move: {MoveKey}, Turn: {TurnKey}, Fire: {FireKey}, Reload: {ReloadKey}";
    }
}
public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance;
    void Awake()
    {
        Instance = this;
    }
    public InputsImage GetInputs()
    {
        return new InputsImage
        {
            AimKey = Input.GetKey(KeyCode.Mouse1),
            MoveKey = Input.GetKey(KeyCode.D),
            TurnKey = Input.GetKeyDown(KeyCode.S),
            FireKey = Input.GetKey(KeyCode.Mouse0),
            ReloadKey = Input.GetKeyDown(KeyCode.R),
            SwapKey = Input.GetKeyDown(KeyCode.F)
        };
    }
}
