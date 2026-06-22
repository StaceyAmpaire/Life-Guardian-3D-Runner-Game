using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialPanel; // The main panel that holds the tutorial UI
    public TextMeshProUGUI tutorialText; // Text component to display tutorial content
    public Image tutorialImage;
    public Button nextButton; // Button to advance to the next page
    public Button backButton; // Button to go back to the previous page
    public Button understandButton; // Button to close the tutorial on the last page
    public GameObject portalsParent;
    private const string TUTORIAL_SEEN_KEY = "TutorialSeen";

    private List<string> tutorialPages = new List<string>();
    public List<Sprite> tutorialSprites = new List<Sprite>();
    private int currentPageIndex = 0;

    void Awake()
    {
        // Initialize tutorial content
       tutorialPages.Add(
"Welcome to Life Guardian!\n\nYour mission is to protect the Baobab Tree by making healthy food and activity choices.\n\nThe tree's Life reflects your health journey."
);

tutorialPages.Add(
"There are two paths:\n\n Prevention Path\nLearn healthy eating habits to prevent diabetes.\n\n Management Path\nLearn healthy lifestyle habits to manage diabetes.\nBoth paths help you care for the tree."
);

tutorialPages.Add(
"As you run, collect healthy foods and healthy activities.\nHealthy choices:\n- Increase Tree Life\n- Earn Healing Dew\n- Unlock achievements\n\nUnhealthy choices can slow your progress."
);

tutorialPages.Add(
"Your choices affect your Guardian.\n\n Unhealthy foods increase body size.\n\n Healthy foods restore your Guardian toward normal.\n\n Healthy activities improve movement speed.\n Unhealthy habits reduce speed."
);

tutorialPages.Add(
"Healing Dew is earned through good choices.\n\nHealing Dew helps unlock new levels and achievements.\n\nExplore both paths and keep the Baobab Tree thriving!"
);

tutorialPages.Add(
"Life Guardian teaches healthy habits and diabetes awareness.\n\nIt is intended for education only and is not a substitute for professional medical advice.\n\nAlways consult healthcare professionals for medical concerns."
);

tutorialPages.Add(
"You are now ready to begin your journey.\n\nProtect the Baobab Tree and become a true Life Guardian.\n\nClick 'I Understand' to begin."
);

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

public static bool HasSeenTutorial()
{
    int value = PlayerPrefs.GetInt(TUTORIAL_SEEN_KEY, 0);

    Debug.Log("TutorialSeen = " + value);

    return value == 1;
}

  void UpdatePageContent()
{
    Debug.Log("Showing page " + currentPageIndex);

    tutorialText.text = tutorialPages[currentPageIndex];

    if (tutorialImage != null &&
        currentPageIndex < tutorialSprites.Count)
    {
        tutorialImage.sprite = tutorialSprites[currentPageIndex];

        

        Debug.Log("Sprite = " +
            tutorialSprites[currentPageIndex].name);
    }

    bool isLastPage =
        currentPageIndex == tutorialPages.Count - 1;

    backButton.gameObject.SetActive(currentPageIndex > 0);
    nextButton.gameObject.SetActive(!isLastPage);
    understandButton.gameObject.SetActive(isLastPage);
}

   public void CloseTutorial()
{
    tutorialPanel.SetActive(false);
    portalsParent.SetActive(true);

    PlayerPrefs.SetInt(TUTORIAL_SEEN_KEY, 1);
    PlayerPrefs.Save();
}
}
