using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementsUIManager : MonoBehaviour
{
    [Header("Cards")]
    public AchievementCardUI firstStepsCard;
    public AchievementCardUI avatarSelectCard;
    public AchievementCardUI level2Card;
    public AchievementCardUI avocadoLoverCard;

    [Header("Left Panel Total Progress UI")]
    public Image largeRingFill;         // Assign 'LargeRingFill' here
    public TMP_Text percentText;        // Assign 'Percenttext' here
    public TMP_Text fractionText;       // Assign 'Fractiontext' here

    private const string CLAIM_FIRST_RUN = "Claim_FirstRun";
    private const string CLAIM_AVATAR = "Claim_Avatar";
    private const string CLAIM_LEVEL2 = "Claim_Level2";
    private const string CLAIM_AVOCADO = "Claim_Avocado";

    private const int FIRST_RUN_REWARD = 3;
    private const int AVATAR_REWARD = 2;
    private const int LEVEL2_REWARD = 10;
    private const int AVOCADO_REWARD = 5;
    private const int AVOCADO_TARGET = 10;

    [SerializeField] private bool resetOnPlay = false;
    private static bool hasResetThisPlaySession = false;

    void Start()
    {
        SetupAll();

        SetupButton(firstStepsCard, ClaimFirstSteps);
        SetupButton(avatarSelectCard, ClaimAvatar);
        SetupButton(level2Card, ClaimLevel2);
        SetupButton(avocadoLoverCard, ClaimAvocadoLover);
    }

    void SetupButton(AchievementCardUI card, System.Action action)
    {
        if (card != null && card.claimButton != null)
        {
            card.claimButton.onClick.RemoveAllListeners();
            card.claimButton.onClick.AddListener(() => action());
        }
    }

    void SetupAll()
    {
        SetupFirstSteps();
        SetupAvatar();
        SetupLevel2();
        SetupAvocadoLover();
        
        // Recalculate total overview panel anytime the UI updates
        UpdateTotalProgressPanel(); 
    }

    // ---------------- TOTAL OVERVIEW LOGIC ----------------
    void UpdateTotalProgressPanel()
    {
        int unlockedCount = 0;
        int totalAchievements = 4; // Total number of cards tracked

        // Check if unlocked in AchievementManager
        if (AchievementManager.Instance != null)
        {
            if (AchievementManager.Instance.IsUnlocked(AchievementManager.ACH_FIRST_RUN)) unlockedCount++;
            if (AchievementManager.Instance.IsUnlocked(AchievementManager.ACH_AVATAR_EXPLORER)) unlockedCount++;
            if (AchievementManager.Instance.IsUnlocked(AchievementManager.ACH_AVOCADO_LOVER)) unlockedCount++;
        }
        
        // Level 2 checks your direct master info bool
        if (MasterInfo.level2Unlocked) unlockedCount++;

        // Update UI Texts & Radial Fill Amount
        if (fractionText != null)
        {
            fractionText.text = $"{unlockedCount} / {totalAchievements}";
        }

        float fillRatio = (float)unlockedCount / totalAchievements;

        if (largeRingFill != null)
        {
            largeRingFill.fillAmount = fillRatio;
        }

        if (percentText != null)
        {
            percentText.text = $"{Mathf.RoundToInt(fillRatio * 100)}%";
        }
    }

    // ---------------- FIRST STEPS ----------------
    void SetupFirstSteps()
    {
        if (firstStepsCard == null) return;
        firstStepsCard.SetTexts("First Steps", "Complete your first run", FIRST_RUN_REWARD);

        bool unlocked = AchievementManager.Instance != null && AchievementManager.Instance.IsUnlocked(AchievementManager.ACH_FIRST_RUN);
        bool claimed = PlayerPrefs.GetInt(CLAIM_FIRST_RUN, 0) == 1;

        int progress = unlocked ? 1 : 0;
        firstStepsCard.SetProgress(progress, 1);

        if (!unlocked) firstStepsCard.ShowProgress();
        else if (!claimed) firstStepsCard.ShowClaim();
        else firstStepsCard.ShowCompleted();
    }

    void ClaimFirstSteps()
    {
        if (PlayerPrefs.GetInt(CLAIM_FIRST_RUN, 0) == 1) return;

        PlayerPrefs.SetInt(CLAIM_FIRST_RUN, 1);
        MasterInfo.dewCount += FIRST_RUN_REWARD;
        MasterInfo.totalDewCount += FIRST_RUN_REWARD;
        MasterInfo.SaveData();
        MasterInfo.Instance?.UpdateDewDisplay();
        PlayerPrefs.Save();

        SetupAll(); //  Use SetupAll to refresh both card and left side panel
    }

    // ---------------- AVATAR SELECT ----------------
    void SetupAvatar()
    {
        if (avatarSelectCard == null) return;
        avatarSelectCard.SetTexts("Avatar Explorer", "Use both Avatars", AVATAR_REWARD);

        bool unlocked = AchievementManager.Instance != null && AchievementManager.Instance.IsUnlocked(AchievementManager.ACH_AVATAR_EXPLORER);
        bool claimed = PlayerPrefs.GetInt(CLAIM_AVATAR, 0) == 1;

        int progress = 0;
        if (AchievementManager.Instance != null)
        {
            progress += AchievementManager.Instance.GetStat(AchievementManager.STAT_USED_MALE);
            progress += AchievementManager.Instance.GetStat(AchievementManager.STAT_USED_FEMALE);
        }

        avatarSelectCard.SetProgress(progress, 2);

        if (!unlocked) avatarSelectCard.ShowProgress();
        else if (!claimed) avatarSelectCard.ShowClaim();
        else avatarSelectCard.ShowCompleted();
    }

    void ClaimAvatar()
    {
        if (PlayerPrefs.GetInt(CLAIM_AVATAR, 0) == 1) return;

        PlayerPrefs.SetInt(CLAIM_AVATAR, 1);
        MasterInfo.dewCount += AVATAR_REWARD;
        MasterInfo.totalDewCount += AVATAR_REWARD;
        MasterInfo.SaveData();
        MasterInfo.Instance?.UpdateDewDisplay();
        PlayerPrefs.Save();

        SetupAll();
    }

    // ---------------- LEVEL 2 ----------------
    void SetupLevel2()
    {
        if (level2Card == null) return;
        level2Card.SetTexts("Growth", "Unlock Level 2", LEVEL2_REWARD);

        bool unlocked = MasterInfo.level2Unlocked;
        bool claimed = PlayerPrefs.GetInt(CLAIM_LEVEL2, 0) == 1;

        int progress = unlocked ? 1 : 0;
        level2Card.SetProgress(progress, 1);

        if (!unlocked) level2Card.ShowProgress();
        else if (!claimed) level2Card.ShowClaim();
        else level2Card.ShowCompleted();
    }

    void ClaimLevel2()
    {
        if (PlayerPrefs.GetInt(CLAIM_LEVEL2, 0) == 1) return;

        PlayerPrefs.SetInt(CLAIM_LEVEL2, 1);
        MasterInfo.dewCount += LEVEL2_REWARD;
        MasterInfo.totalDewCount += LEVEL2_REWARD;
        MasterInfo.SaveData();
        MasterInfo.Instance?.UpdateDewDisplay();
        PlayerPrefs.Save();

        SetupAll();
    }

    // ---------------- AVOCADO LOVER ----------------
    void SetupAvocadoLover()
    {
        if (avocadoLoverCard == null) return;
        avocadoLoverCard.SetTexts("Avocado Lover", "Collect 10 Avocados", AVOCADO_REWARD);

        bool unlocked = AchievementManager.Instance != null && AchievementManager.Instance.IsUnlocked(AchievementManager.ACH_AVOCADO_LOVER);
        bool claimed = PlayerPrefs.GetInt(CLAIM_AVOCADO, 0) == 1;

        int progress = 0;
        if (AchievementManager.Instance != null)
        {
            progress = AchievementManager.Instance.GetStat(AchievementManager.STAT_AVOCADO_COUNT);
        }

        avocadoLoverCard.SetProgress(progress, AVOCADO_TARGET);

        if (!unlocked) avocadoLoverCard.ShowProgress();
        else if (!claimed) avocadoLoverCard.ShowClaim();
        else avocadoLoverCard.ShowCompleted();
    }

    void ClaimAvocadoLover()
    {
        if (PlayerPrefs.GetInt(CLAIM_AVOCADO, 0) == 1) return;

        PlayerPrefs.SetInt(CLAIM_AVOCADO, 1);
        MasterInfo.dewCount += AVOCADO_REWARD;
        MasterInfo.totalDewCount += AVOCADO_REWARD;
        MasterInfo.SaveData();
        MasterInfo.Instance?.UpdateDewDisplay();
        PlayerPrefs.Save();

        SetupAll();
    }

    [ContextMenu("Reset All Achievements")]
    public static void ResetAllAchievements()
    {
        hasResetThisPlaySession = true;
        PlayerPrefs.DeleteKey(AchievementManager.ACH_FIRST_RUN);
        PlayerPrefs.DeleteKey(AchievementManager.ACH_LEVEL2_UNLOCKED);
        PlayerPrefs.DeleteKey(AchievementManager.ACH_AVATAR_EXPLORER);
        PlayerPrefs.DeleteKey(AchievementManager.ACH_AVOCADO_LOVER);

        PlayerPrefs.DeleteKey("Claim_FirstRun");
        PlayerPrefs.DeleteKey("Claim_Avatar");
        PlayerPrefs.DeleteKey("Claim_Level2");
        PlayerPrefs.DeleteKey("Claim_Avocado");

        PlayerPrefs.DeleteKey(AchievementManager.STAT_USED_MALE);
        PlayerPrefs.DeleteKey(AchievementManager.STAT_USED_FEMALE);
        PlayerPrefs.DeleteKey(AchievementManager.STAT_AVOCADO_COUNT);

        PlayerPrefs.Save();
        Debug.Log("ALL achievements + claims reset");
    }
}
