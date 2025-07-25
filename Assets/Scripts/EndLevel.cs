using UnityEngine;
using static CheckpointArrow;

public class EndLevel : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public LevelManager levelManager;
    public GameObject nextLvl;
    public GameObject currentLvl;
    private void OnCollisionEnter(Collision collision)
    {

        

        if (currentLvl.name == "lvl3")
        {
            PointCounter.instance.ruleFollowed();
        }

        if (currentLvl.name == "lvl4")
        {
            PointCounter.instance.ruleFollowed();
        }

        Time.timeScale = 0;
        PointCounter.instance.levelSummary();
    }
}
