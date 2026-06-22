using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UnlockSequence : MonoBehaviour
{
    [Header("UI")]
    public GameObject overlay;
    public GameObject popup;

    [Header("Buttons")]
    public Button playNowButton;
    public Button closeButton;

    [Header("Level Card")]
    public Image levelImage;          // Drag LevelImage here
    public Sprite lockedSprite;       // Current padlock sprite
    public Sprite unlockedSprite;     // Activity level sprite

    [SerializeField] private string activitySceneToLoad = "ActivityRun";

    IEnumerator Start()
{
    if (!MasterInfo.level2Unlocked || MasterInfo.level2UnlockAnimationPlayed)
    {
        yield break;
    }

    // 🔥 Force set and save this immediately so it cannot execute twice concurrently
    MasterInfo.level2UnlockAnimationPlayed = true;
    MasterInfo.SaveData(); 

    // Show dark overlay
    if (overlay != null)
    {
        overlay.SetActive(true);
    }

    yield return new WaitForSeconds(0.5f);

    // Change card image
    if (levelImage != null && unlockedSprite != null)
    {
        levelImage.sprite = unlockedSprite;
    }

    // Show popup
    if (popup != null)
    {
        popup.SetActive(true);
        if (AudioManager2.Instance != null)
        {
            AudioManager2.Instance.PlayLevelUnlockedSound();
        }
    }

    // Hook up buttons
    if (playNowButton != null)
    {
        playNowButton.onClick.RemoveAllListeners();
        playNowButton.onClick.AddListener(OnPlayNowClicked);
    }

    if (closeButton != null)
    {
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(OnCloseClicked);
    }
}


    public void OnPlayNowClicked()
    {
        MasterInfo.ResetRunData();

        PlayerPrefs.SetString("SceneToLoad", activitySceneToLoad);
        UnityEngine.SceneManagement.SceneManager.LoadScene("LoadingScene");
    }

    public void OnCloseClicked()
    {
        if (popup != null)
        {
            popup.SetActive(false);
        }

        if (overlay != null)
        {
            overlay.SetActive(false);
        }
    }
}