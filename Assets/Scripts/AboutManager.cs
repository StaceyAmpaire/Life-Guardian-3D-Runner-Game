using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class AboutManager : MonoBehaviour
{
    public GameObject aboutPanel;
    public TextMeshProUGUI aboutText;
    public Image aboutImage;

    public Button nextButton;
    public Button backButton;
    public Button closeButton;

    private List<string> aboutPages = new List<string>();
    public List<Sprite> aboutSprites = new List<Sprite>();

    private int currentPageIndex = 0;

    void Awake()
    {
        aboutPages.Add(
"ABOUT LIFE GUARDIAN\n\n" +
"Life Guardian is an educational game designed to teach healthy lifestyle choices and diabetes awareness through interactive gameplay."
);

        aboutPages.Add(
"FEATURES\n\n" +
"• Prevention Path\n" +
"• Management Path\n" +
"• Healing Dew Rewards\n" +
"• Achievements\n" +
"• Avatar Progression\n" +
"• Baobab Tree Life System"
);

        aboutPages.Add(
"DEVELOPER\n\n" +
"Developed by:\n" +
"Life Guardian Team\n\n" +
"Bachelor of Science in Software Engineering\n" +
"Makerere University"
);

        aboutPages.Add(
"CONTACT & DISCLAIMER\n\n" +
"Email:\n" +
"lifeguardianteam@gmail.com\n\n" +
"Life Guardian is intended for educational purposes only and does not replace professional medical advice."
);

        nextButton.onClick.AddListener(ShowNextPage);
        backButton.onClick.AddListener(ShowPreviousPage);
        closeButton.onClick.AddListener(CloseAbout);
    }

    void Start()
    {
        aboutPanel.SetActive(false);
    }

    public void ShowAbout()
    {
        currentPageIndex = 0;
        UpdatePageContent();
        aboutPanel.SetActive(true);
    }

    public void ShowNextPage()
    {
        if (currentPageIndex < aboutPages.Count - 1)
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
        aboutText.text = aboutPages[currentPageIndex];

        if (aboutImage != null &&
            currentPageIndex < aboutSprites.Count)
        {
            aboutImage.sprite = aboutSprites[currentPageIndex];
        }

        bool isLastPage =
            currentPageIndex == aboutPages.Count - 1;

        backButton.gameObject.SetActive(currentPageIndex > 0);
        nextButton.gameObject.SetActive(!isLastPage);
    }

    public void CloseAbout()
    {
        aboutPanel.SetActive(false);
    }
}