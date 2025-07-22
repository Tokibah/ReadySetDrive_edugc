using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public Transform player;
    public Transform[] levelSpawnPoints;
    public GameObject summaryUI;
    public GameObject pauseMenu;
    public GameObject pauseBtn;


    private int currentLevel = 0;

    private void Awake()
    {
        instance = this;
    }

    public void GoToNextLevel()
    {

        if (currentLevel < levelSpawnPoints.Length)
        {
            currentLevel++;
            Vector3 spawnPosition = levelSpawnPoints[currentLevel].position;
            player.rotation = levelSpawnPoints[currentLevel].rotation;
            player.position = spawnPosition;

            Time.timeScale = 1;
            PointCounter.instance.resetPoint();
            summaryUI.SetActive(false);
            Debug.Log("Player moved to Level " + (currentLevel + 1));
        }
        
    }

    public void RestartLevel()
    {
        Vector3 spawnPosition = levelSpawnPoints[currentLevel].position;
        player.rotation = levelSpawnPoints[currentLevel].rotation;
        player.position = spawnPosition;

        Time.timeScale = 1;
        PointCounter.instance.resetPoint();
        summaryUI.SetActive(false);
        Debug.Log("Player restarted the level!");
    }

    public void GoHome()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("DCScene");
    }

    public void pauseGame()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        pauseBtn.SetActive(false);
    }

    public void resumeGame()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        pauseBtn.SetActive(true);
    }

    public void lvl1()
    {
        currentLevel = 0;
    }

    public void lvl2()
    {
        currentLevel = 1;
    }

    public void lvl3()
    {
        currentLevel = 2;
    }

    public void lvl4()
    {
        currentLevel = 3;
    }

    public void lvl5()
    {
        currentLevel = 4;
    }
}
