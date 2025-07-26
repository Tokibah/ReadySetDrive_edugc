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

    private void Awake()
    {
        instance = this;
    }
    private void OnCollisionEnter(Collision collision)
    {
        
        if (PointCounter.instance.collectedRep >= requiredScore)
        {
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
            PointCounter.instance.levelSuccess();
        }




        Time.timeScale = 0;
        LevelManager.instance.noPause();
        PointCounter.instance.levelSummary();
    }

    
}
