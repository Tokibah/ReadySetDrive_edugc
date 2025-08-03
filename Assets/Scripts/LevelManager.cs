using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public Transform player;
    public Transform[] levelSpawnPoints;
    public GameObject summaryUI;
    public GameObject pauseMenu;
    public GameObject pauseBtn;
    public GameObject endpoints;
    public Text levelCounter;
    private int levelDisplay;

    private bool isPaused;
    private bool allowPause = true;
    public int currentLevel = 1;

    private void Awake()
    {
        instance = this;
    }

    public void GoToNextLevel()
    {

            currentLevel = currentLevel + 1;

            if (currentLevel >= levelSpawnPoints.Length)
        {
            Debug.Log("No more levels.All levels already unlocked");
            return;
        }
            LevelUnlock.UnlockLevel(currentLevel);
            Vector3 spawnPosition = levelSpawnPoints[currentLevel].position;
            player.rotation = levelSpawnPoints[currentLevel].rotation;
            player.position = spawnPosition;

            Time.timeScale = 1;
            PointCounter.instance.resetPoint();
            PointCounter.instance.nextLevelBtn.SetActive(false);
            summaryUI.SetActive(false);
        //CheckpointArrow.instance.updateArrow(currentLevel);

        StartLevel.instance.endpoints.SetActive(currentLevel != 5);

        if (currentLevel == 4)
        {
            GameObject[] cones = GameObject.FindGameObjectsWithTag("Cones");
            foreach (GameObject cone in cones)
            {
                cone.SetActive(false);
            }

            // If you have roadblock colliders, also disable them:
            endpoints.SetActive(false);
        }

        
            
            yesPause();
            Debug.Log("Player moved to Level " + (currentLevel + 1));
        
        
    }

    public void RestartLevel()
    {
        Vector3 spawnPosition = levelSpawnPoints[currentLevel].position;
        player.rotation = levelSpawnPoints[currentLevel].rotation;
        player.position = spawnPosition;

        Time.timeScale = 1;
        PointCounter.instance.resetPoint();
        PointCounter.instance.retryBtn.SetActive(false);
        summaryUI.SetActive(false);
        yesPause();
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
        isPaused = true;
    }

    public void resumeGame()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        pauseBtn.SetActive(true);
        isPaused = false;
    }

   public void setLevel(int lvl)
    {
        currentLevel = lvl;
    }

    public void noPause()
    {
        allowPause = false;
    }

    public void yesPause()
    {
        allowPause = true;
    }

    private void Update()
    {
        levelDisplay = currentLevel + 1;
        levelCounter.text = levelDisplay.ToString();
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (allowPause && !isPaused)
            {
                pauseGame();
            }
            
            if (isPaused)
            {
                resumeGame();
            }
        }
    }
}
