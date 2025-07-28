using UnityEngine;
//lazy af do more hw pls
public class StateInjector : MonoBehaviour
{
    [SerializeField] private PlayerStateType injectedState;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            PlayerStateManager.Instance.SetState(injectedState);
        }  
    }
}