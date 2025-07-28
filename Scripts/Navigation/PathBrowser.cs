using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PathBrowser : MonoBehaviour
{
    public static PathBrowser Instance;
    public GameObject buttonPrefab;
    public GameObject buttonParent;
    private PlayerMovement playerMovement;
    private int filter;
    private string[] choices;
    
    // Object pooling for performance
    private Queue<GameObject> buttonPool = new Queue<GameObject>();
    private List<GameObject> activeButtons = new List<GameObject>();
    private const int INITIAL_POOL_SIZE = 5;
    
    void Awake()
    {
        Instance = this;
        playerMovement = GetComponent<PlayerMovement>();
        InitializeButtonPool();
    }
    
    void InitializeButtonPool()
    {
        for (int i = 0; i < INITIAL_POOL_SIZE; i++)
        {
            GameObject pooledButton = Instantiate(buttonPrefab, buttonParent.transform);
            pooledButton.SetActive(false);
            buttonPool.Enqueue(pooledButton);
        }
    }
    
    GameObject GetPooledButton()
    {
        if (buttonPool.Count > 0)
        {
            return buttonPool.Dequeue();
        }
        else
        {
            return Instantiate(buttonPrefab, buttonParent.transform);
        }
    }
    
    void ReturnButtonToPool(GameObject button)
    {
        button.SetActive(false);
        button.GetComponent<Button>().onClick.RemoveAllListeners();
        buttonPool.Enqueue(button);
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
            
            GameObject newButton = GetPooledButton();
            newButton.SetActive(true);
            activeButtons.Add(newButton);
            
            newButton.GetComponent<PathButton>().pathText.text = choices[i];
            RectTransform buttonRectTransform = newButton.GetComponent<RectTransform>();
            buttonRectTransform.anchoredPosition = new Vector2(0, -yMod * 30);
            
            newButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                playerMovement.Choose(choice);
                DestroyAllChildren();
            });
        }
    }
    
    public void DestroyAllChildren()
    {
        for (int i = 0; i < activeButtons.Count; i++)
        {
            ReturnButtonToPool(activeButtons[i]);
        }
        activeButtons.Clear();
    }
}