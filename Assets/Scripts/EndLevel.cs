using UnityEngine;
using static CheckpointArrow;

public class EndLevel : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static EndLevel instance;
    public LevelManager levelManager;
    public GameObject nextLvl;
    public GameObject currentLvl;
    public int requiredScore = 30;
    public int currentLevel;
    public int videoUnlocks;

    private void Awake()
    {
        instance = this;
    }
    private void OnCollisionEnter(Collision collision)
    {
        
       if (collision.gameObject.CompareTag("Player"))
        {
            if (PointCounter.instance.collectedRep >= requiredScore)
            {
                LevelUnlock.UnlockLevel(currentLevel+1);
                LevelUnlock.SaveLatestLevel(currentLevel + 1);
                if (PlayerPrefs.HasKey("Level_5") == false)
                {
                    PlayerPrefs.SetInt("UnlockedVideos", videoUnlocks);
                    PlayerPrefs.Save();
                }
                //levelEntrance.SetActive(true);
                //if (lockUI != null) lockUI.SetActive(false);
                PointCounter.instance.levelSuccess();
            }
            else
            {
                //levelEntrance.SetActive(false);
                //if (lockUI != null) lockUI.SetActive(true);
                PointCounter.instance.levelFailed();
            }


            if (currentLvl.name == "lvl3")
            {
                PointCounter.instance.ruleFollowed();
                LevelUnlock.UnlockLevel(currentLevel + 1);
                LevelUnlock.SaveLatestLevel(currentLevel + 1);
                PointCounter.instance.levelSuccess();
            }




            Time.timeScale = 0;
            LevelManager.instance.noPause();
            PointCounter.instance.levelSummary();
        }
    }

    
}
