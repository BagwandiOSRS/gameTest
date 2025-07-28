using UnityEngine;

public class BranchCollider : MonoBehaviour
{
    [SerializeField] private string[] choices;
    private int branchId;
    [SerializeField] private LayerMask playerLayer;
    private void OnTriggerEnter(Collider other)
    {
        // !!! hardcoded layer index
        if (other.gameObject.layer != 6) return;
        PlayerMovement.Instance.CacheDirection();
        PlayerMovement.Instance.InCollider = true;
        PlayerMovement.Instance.SetActiveBranch(branchId);
        string filter = PlayerMovement.Instance.FilterChoices();
        int filterIndex = 0;

        for (int i = 0; i < choices.Length; i++)
        {
            if (choices[i] == filter)
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
        // !!! hardcoded layer index
        if (other.gameObject.layer != 6) return;
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