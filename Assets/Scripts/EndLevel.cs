using UnityEngine;

public class EndLevel : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public LevelManager levelManager;
    public GameObject nextLvl;
    public GameObject currentLvl;
    private void OnCollisionEnter(Collision collision)
    {
        

        if (currentLvl.name == "lvl1")
        {
            levelManager.GoToNextLevel();
        }
        else
        {
            Time.timeScale = 0;
            PointCounter.instance.levelSummary();
        }

        if (nextLvl.activeSelf == false)
        {
            nextLvl.SetActive(true);
            currentLvl.SetActive(false);

        }


    }
}
