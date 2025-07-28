using UnityEngine;
using UnityEngine.UI;

public class PathBrowser : MonoBehaviour
{
    public static PathBrowser Instance;
    public GameObject buttonPrefab;
    public GameObject buttonParent;
    private PlayerMovement playerMovement;
    private int filter;
    private string[] choices;
    void Awake()
    {
        Instance = this;
        playerMovement = GetComponent<PlayerMovement>();
    }
    public void SetChoices(string[] newChoices, int f)
    {
        choices = newChoices;
        filter = f;
    }
    public void DisplayButtons()
    {
        int yMod = -1;
        for (int i = 0; i < choices.Length; i++)
        {
            int choice = i;
            if (i == filter) continue;
            yMod++;
            GameObject newButton = Instantiate(buttonPrefab, buttonParent.transform);
            newButton.GetComponent<PathButton>().pathText.text = choices[i];
            RectTransform buttonRectTransform = newButton.GetComponent<RectTransform>();
            buttonRectTransform.anchoredPosition += new Vector2(0, -yMod * 30);
            newButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                playerMovement.Choose(choice);
                DestroyAllChildren();
            });
        }
    }
    public void DestroyAllChildren()
    {
        foreach (Transform child in buttonParent.transform)
        {
            Destroy(child.gameObject);
        }
    }
}