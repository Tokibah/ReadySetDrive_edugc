using TMPro;
using Unity.Collections;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{

    public TextMeshProUGUI nameLabel;
    public GameObject achievementPanel;
    // 1st achievement: reputation score reaches 100 (Pemandu Berhemah)
    // 2nd achievement: complete the last level of the chapter (Ready, Set, Drive!
    // 3rd achievement: obeyed 10 road rules in total

    private void Update()
    {
        achievementChecks();
    }
    public void achievementChecks()
    {
        if (!PlayerPrefs.HasKey("PemanduBerhemah") && PlayerPrefs.GetInt("PlayerTotalScore") >= 100)
        {
            collectAchievement("Pemandu Berhemah");
        }

        if (!PlayerPrefs.HasKey("ReadySetDrive") && PlayerPrefs.HasKey("Chapter1Completed"))
        {
            collectAchievement("Ready Set Drive");
        }

        if (!PlayerPrefs.HasKey("PakarMemandu") && PlayerPrefs.GetInt("RulesFollowed") >= 10)
        {
            collectAchievement("Pakar Memandu");
        }

        //if (!PlayerPrefs.HasKey("Ready Set Drive") && PlayerPrefs.GetInt(""))
    }

    public void collectAchievement(string achievementName)
    {
        PlayerPrefs.SetString(achievementName, "true");
        PlayerPrefs.Save();
        nameLabel.text = achievementName;
        achievementPanel.SetActive(true);
    }

    
}
