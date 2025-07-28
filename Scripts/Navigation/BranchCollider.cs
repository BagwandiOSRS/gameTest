using UnityEngine;

public class BranchCollider : MonoBehaviour
{
    [SerializeField] private string[] choices;
    private int branchId;
    [SerializeField] private LayerMask playerLayer = 1 << 6; // Default to layer 6, but configurable
    private int playerLayerIndex;
    
    void Awake()
    {
        // Cache the layer index for faster comparison
        playerLayerIndex = Mathf.RoundToInt(Mathf.Log(playerLayer.value, 2));
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != playerLayerIndex) return;
        
        PlayerMovement.Instance.CacheDirection();
        PlayerMovement.Instance.InCollider = true;
        PlayerMovement.Instance.SetActiveBranch(branchId);
        string filter = PlayerMovement.Instance.FilterChoices();
        int filterIndex = 0;

        // Optimized string comparison loop
        for (int i = 0; i < choices.Length; i++)
        {
            if (string.Equals(choices[i], filter, System.StringComparison.Ordinal))
            {
                filterIndex = i;
                break;
            }
        }
        PathBrowser.Instance.SetChoices(choices, filterIndex);
        TogglePathButtonDisplay(true);
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != playerLayerIndex) return;
        TogglePathButtonDisplay(false);
        PlayerMovement.Instance.InCollider = false;
    }
    
    public void TogglePathButtonDisplay(bool display)
    {
        if (display)
        {
            PathBrowser.Instance.DisplayButtons();
        }
        else
        {
            PathBrowser.Instance.DestroyAllChildren();
        }
    }
    
    public void SetBranchData(int id, string[] displayChoices)
    {
        branchId = id;
        choices = displayChoices;
    }
}