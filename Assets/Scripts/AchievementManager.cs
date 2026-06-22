using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance { get; private set; }

    // ---------- Achievement Keys ----------
    public const string ACH_FIRST_RUN = "Ach_FirstRun";
    public const string ACH_LEVEL2_UNLOCKED = "Ach_Level2Unlocked";
    public const string ACH_AVOCADO_LOVER = "Ach_AvocadoLover";
    public const string ACH_WATERMELON_FAN = "Ach_WatermelonFan";
    public const string ACH_FISH_FIRST = "Ach_FishFirst";
    public const string ACH_HEALTHY_PLATE = "Ach_HealthyPlate";
    public const string ACH_DEW_COLLECTOR = "Ach_DewCollector";
    public const string ACH_BACK_ON_TRACK = "Ach_BackOnTrack";
    public const string ACH_PATH_EXPLORER = "Ach_PathExplorer";
    public const string ACH_AVATAR_EXPLORER = "Ach_AvatarExplorer";

    // ---------- Stat Keys ----------
    public const string STAT_AVOCADO_COUNT = "Stat_AvocadoCount";
    public const string STAT_WATERMELON_COUNT = "Stat_WatermelonCount";
    public const string STAT_FISH_COUNT = "Stat_FishCount";
    public const string STAT_HEALTHY_FOOD_COUNT = "Stat_HealthyFoodCount";
    public const string STAT_TOTAL_DEW_EARNED = "Stat_TotalDewEarned";
    public const string STAT_MANAGEMENT_RECOVERY_COUNT = "Stat_ManagementRecoveryCount";

    public const string STAT_PLAYED_PREVENTION = "Stat_PlayedPrevention";
    public const string STAT_PLAYED_MANAGEMENT = "Stat_PlayedManagement";
    public const string STAT_USED_MALE = "Stat_UsedMale";
    public const string STAT_USED_FEMALE = "Stat_UsedFemale";

    // Recovery tracking
    private bool recoveryPending = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // =========================================================
    // FOOD TRACKING
    // =========================================================
    public void RegisterFoodChoice(string foodName, bool isHealthy, int dewEarned)
    {
        if (!string.IsNullOrEmpty(foodName))
        {
            string lowerFood = foodName.Trim().ToLower();

            if (lowerFood == "avocado")
            {
                int count = PlayerPrefs.GetInt(STAT_AVOCADO_COUNT, 0) + 1;
                PlayerPrefs.SetInt(STAT_AVOCADO_COUNT, count);

                if (count >= 10)
                    UnlockAchievement(ACH_AVOCADO_LOVER);
            }
            else if (lowerFood == "watermelon")
            {
                int count = PlayerPrefs.GetInt(STAT_WATERMELON_COUNT, 0) + 1;
                PlayerPrefs.SetInt(STAT_WATERMELON_COUNT, count);

                if (count >= 10)
                    UnlockAchievement(ACH_WATERMELON_FAN);
            }
            else if (lowerFood == "fish")
            {
                int count = PlayerPrefs.GetInt(STAT_FISH_COUNT, 0) + 1;
                PlayerPrefs.SetInt(STAT_FISH_COUNT, count);

                if (count >= 10)
                    UnlockAchievement(ACH_FISH_FIRST);
            }
        }

        if (isHealthy)
        {
            int healthyCount = PlayerPrefs.GetInt(STAT_HEALTHY_FOOD_COUNT, 0) + 1;
            PlayerPrefs.SetInt(STAT_HEALTHY_FOOD_COUNT, healthyCount);

            if (healthyCount >= 25)
                UnlockAchievement(ACH_HEALTHY_PLATE);
        }

        if (dewEarned > 0)
        {
            int totalDew = PlayerPrefs.GetInt(STAT_TOTAL_DEW_EARNED, 0) + dewEarned;
            PlayerPrefs.SetInt(STAT_TOTAL_DEW_EARNED, totalDew);

            if (totalDew >= 50)
                UnlockAchievement(ACH_DEW_COLLECTOR);
        }

        PlayerPrefs.Save();
    }

    // =========================================================
    // RUN / LEVEL / PATH / AVATAR
    // =========================================================
    public void MarkFirstRunComplete()
    {
        UnlockAchievement(ACH_FIRST_RUN);
    }

    public void MarkLevel2Unlocked()
    {
        UnlockAchievement(ACH_LEVEL2_UNLOCKED);
    }

    public void MarkPathPlayed(string pathName)
    {
        if (pathName == "Prevention")
            PlayerPrefs.SetInt(STAT_PLAYED_PREVENTION, 1);

        if (pathName == "Management")
            PlayerPrefs.SetInt(STAT_PLAYED_MANAGEMENT, 1);

        int prevention = PlayerPrefs.GetInt(STAT_PLAYED_PREVENTION, 0);
        int management = PlayerPrefs.GetInt(STAT_PLAYED_MANAGEMENT, 0);

        if (prevention == 1 && management == 1)
            UnlockAchievement(ACH_PATH_EXPLORER);

        PlayerPrefs.Save();
    }

    public void MarkAvatarSelected(int avatarIndex)
    {
        // 0 = male, 1 = female
        if (avatarIndex == 0)
            PlayerPrefs.SetInt(STAT_USED_MALE, 1);

        if (avatarIndex == 1)
            PlayerPrefs.SetInt(STAT_USED_FEMALE, 1);

        int male = PlayerPrefs.GetInt(STAT_USED_MALE, 0);
        int female = PlayerPrefs.GetInt(STAT_USED_FEMALE, 0);

        if (male == 1 && female == 1)
            UnlockAchievement(ACH_AVATAR_EXPLORER);

        PlayerPrefs.Save();
    }

    // =========================================================
    // RECOVERY TRACKING
    // =========================================================
    public void NotifyLifeChanged(int currentLife)
    {
        // If life falls below 50 at any point, mark recovery pending
        if (currentLife < 50)
        {
            recoveryPending = true;
        }

        // If it was below 50 before, and now it has recovered to 50+
        if (recoveryPending && currentLife >= 50)
        {
            recoveryPending = false;

            int recoveries = PlayerPrefs.GetInt(STAT_MANAGEMENT_RECOVERY_COUNT, 0) + 1;
            PlayerPrefs.SetInt(STAT_MANAGEMENT_RECOVERY_COUNT, recoveries);

            UnlockAchievement(ACH_BACK_ON_TRACK);
            PlayerPrefs.Save();
        }
    }

    // =========================================================
    // HELPERS
    // =========================================================
    public void UnlockAchievement(string achievementKey)
    {
        if (PlayerPrefs.GetInt(achievementKey, 0) == 1)
            return;

        PlayerPrefs.SetInt(achievementKey, 1);
        PlayerPrefs.Save();

        Debug.Log("Achievement unlocked: " + achievementKey);
    }

    public bool IsUnlocked(string achievementKey)
    {
        return PlayerPrefs.GetInt(achievementKey, 0) == 1;
    }

    public int GetStat(string statKey)
    {
        return PlayerPrefs.GetInt(statKey, 0);
    }
}