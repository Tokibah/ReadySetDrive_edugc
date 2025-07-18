using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Transform player;
    public Transform[] levelSpawnPoints;
    public GameObject summaryUI;

    private int currentLevel = 0;

    public void GoToNextLevel()
    {
        currentLevel++;

        if (currentLevel < levelSpawnPoints.Length)
        {
            Vector3 spawnPosition = levelSpawnPoints[currentLevel].position;
            player.rotation = levelSpawnPoints[currentLevel].rotation;
            player.position = spawnPosition;

            Time.timeScale = 1;
            summaryUI.SetActive(false);
            PointCounter.instance.resetPoint();
            Debug.Log("Player moved to Level " + (currentLevel + 1));
        }
        else
        {
            Debug.Log("No more levels!");
            // Show victory UI or loop/restart as needed
        }
    }
}
