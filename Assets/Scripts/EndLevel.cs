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
                LevelUnlock.UnlockLevel(currentLevel + 1);
                LevelUnlock.SaveLatestLevel(currentLevel + 1);
                if (PlayerPrefs.HasKey("Level_5") == false)
                {
                    PlayerPrefs.SetInt("UnlockedVideos", videoUnlocks);
                    PlayerPrefs.Save();
                }
                //levelEntrance.SetActive(true);
                //if (lockUI != null) lockUI.SetActive(false);

                int initialScore = PlayerPrefs.GetInt("PlayerTotalScore");
                int ruleCount = PlayerPrefs.GetInt("RulesFollowed");
                PlayerPrefs.SetInt("PlayerTotalScore", initialScore + PointCounter.instance.collectedRep);
                PlayerPrefs.SetInt("RulesFollowed", ruleCount + PointCounter.instance.followed);
                PlayerPrefs.Save();
                PointCounter.instance.levelSuccess();
                Debug.Log("Win");
            }
            else
            {

                int initialScore = PlayerPrefs.GetInt("PlayerTotalScore");
                PlayerPrefs.SetInt("PlayerTotalScore", initialScore + PointCounter.instance.collectedRep);
                PlayerPrefs.Save();
                PointCounter.instance.levelFailed();
                Debug.Log("Succesfully Failed");
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


       if (PlayerPrefs.GetInt("CurrentPlay") == 4 || PlayerPrefs.GetInt("CurrentPlay") == 5)
        {

            if (Timer.instance.timeEnd == true && PointCounter.instance.collectedRep < requiredScore)
            {
                //levelEntrance.SetActive(false);
                //if (lockUI != null) lockUI.SetActive(true);

                int initialScore = PlayerPrefs.GetInt("PlayerTotalScore");
                PlayerPrefs.SetInt("PlayerTotalScore", initialScore + PointCounter.instance.collectedRep);
                PlayerPrefs.Save();
                PointCounter.instance.levelFailed();
                Debug.Log("Succesfully Failed");
            }
            else
            {
                int initialScore = PlayerPrefs.GetInt("PlayerTotalScore");
                int ruleCount = PlayerPrefs.GetInt("RulesFollowed");
                PlayerPrefs.SetInt("PlayerTotalScore", initialScore + PointCounter.instance.collectedRep);
                PlayerPrefs.SetInt("RulesFollowed", ruleCount + PointCounter.instance.followed);
                PlayerPrefs.Save();
                PointCounter.instance.levelSuccess();
                Debug.Log("Win");
            }
        }

    }

     

    
}
