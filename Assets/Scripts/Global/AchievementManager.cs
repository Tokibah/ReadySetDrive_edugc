using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static PointCounter instance;

    // 1st achievement: reputation score reaches 100 (Pemandu Berhemah)
    // 2nd achievement: complete the last level of the chapter (Ready, Set, Drive!
    // 3rd achievement: obeyed 10 road rules in total
    public void achievementChecks()
    {
        if (!PlayerPrefs.HasKey("Pemandu Berhemah") && PlayerPrefs.GetInt("PlayerTotalScore") >= 100)
        {
            collectAchievement("Pemandu Berhemah");
        }

        //if (!PlayerPrefs.HasKey("Ready Set Drive") && PlayerPrefs.GetInt(""))
    }

    public void collectAchievement(string achievementName)
    {
        PlayerPrefs.SetString(achievementName, "true");
        PlayerPrefs.Save();
    }

    
}
