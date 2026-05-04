using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialPanel; // The main panel that holds the tutorial UI
    public TextMeshProUGUI tutorialText; // Text component to display tutorial content
    public Button nextButton; // Button to advance to the next page
    public Button backButton; // Button to go back to the previous page
    public Button understandButton; // Button to close the tutorial on the last page
    public GameObject portalsParent;

    private List<string> tutorialPages = new List<string>();
    private int currentPageIndex = 0;

    void Awake()
    {
        // Initialize tutorial content
        tutorialPages.Add("Welcome to Life Guardian!! You Have been Chosen.\n\nIn this journey, you will explore the world of diabetes through choices.");
        tutorialPages.Add("Your decisions will directly impact your Dew Points: make healthy choices to gain them, but beware, unhealthy decisions will cause you to lose them.");
        tutorialPages.Add("Dew Points nourish and maintain the ancient Baobab Tree, the very heart of our world. Now, brave Guardian, choose your path forward: will you focus on Prevention or Management?");
        tutorialPages.Add(" Click 'I Understand' to begin your journey!");

        // Assign button listeners
        nextButton.onClick.AddListener(ShowNextPage);
        backButton.onClick.AddListener(ShowPreviousPage);
        understandButton.onClick.AddListener(CloseTutorial);
    }

    void Start()
    {
        // Ensure the tutorial panel is initially hidden unless explicitly shown by MainMenuManager
        tutorialPanel.SetActive(false);
    }

    public void ShowTutorial()
{
    portalsParent.SetActive(false); //  disable portal clicks
    currentPageIndex = 0;
    UpdatePageContent();
    tutorialPanel.SetActive(true);
}


 public void ShowNextPage()
{
    if (currentPageIndex < tutorialPages.Count - 1)
    {
        currentPageIndex++;
        UpdatePageContent();
    }
}

public void ShowPreviousPage()
{
    if (currentPageIndex > 0)
    {
        currentPageIndex--;
        UpdatePageContent();
    }
}



    void UpdatePageContent()
    {
        tutorialText.text = tutorialPages[currentPageIndex];

        // Manage button visibility
        backButton.gameObject.SetActive(true);
        nextButton.gameObject.SetActive(currentPageIndex < tutorialPages.Count - 1);
        understandButton.gameObject.SetActive(currentPageIndex == tutorialPages.Count - 1);
    }

   public void CloseTutorial()
{
    tutorialPanel.SetActive(false);
    portalsParent.SetActive(true); //  re-enable portals
}
}
