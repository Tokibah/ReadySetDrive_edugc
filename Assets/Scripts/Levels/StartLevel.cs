using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;

public class StartLevel : MonoBehaviour
{
    public static StartLevel instance;
    public Transform player;            // Drag your player prefab or object here
    public Collider[] spawnZoneCollider;  // Drag the spawn zone collider here
    public GameObject levelUI, pause, speed, summary;
    const int totalLevels = 5;
    public Button[] levelButtons;
    public GameObject endpoints;
    private GameObject[] cones;


    private void Awake()
    {
        instance = this;
        cones = GameObject.FindGameObjectsWithTag("Cones");
        
    }
    void Start()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
        PlayerPrefs.SetInt("CurrentPlay", 0);

        for (int i = 1; i <= totalLevels; i++)
        {
            if (LevelUnlock.IsLevelUnlocked(i))
            {
                levelButtons[i - 1].interactable = true;
                Debug.Log("unlocked level" + i);
            }
            else
            {
                levelButtons[i - 1].interactable = false;
                Debug.Log("still locked level" + i);

            }
        }

        levelUI.SetActive(true);
        LevelManager.instance.noPause();
    }

    public void selectLevel()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
        for (int i = 1; i <= totalLevels; i++)
        {
            if (LevelUnlock.IsLevelUnlocked(i))
            {
                levelButtons[i - 1].interactable = true;
            }
            else
            {
                levelButtons[i - 1].interactable = false;
            }
        }
        Debug.Log("Latest level: " + PlayerPrefs.GetInt("LatestLevel", 1));
        levelUI.SetActive(true);
        summary.SetActive(false);
        pause.SetActive(false);
        

        // If you have roadblock colliders, also disable them:
        endpoints.SetActive(true);
        LevelManager.instance.noPause();
    }

    public void checkLevel()
    {
        for (int i = 1; i <= totalLevels; i++)
        {
            if (LevelUnlock.IsLevelUnlocked(i))
            {
                levelButtons[i - 1].interactable = true;
            }
            else
            {
                levelButtons[i - 1].interactable = false;
            }
        }
    }

    public void level1()
    {
        PlayerPrefs.SetInt("CurrentPlay", 1);
        if (player != null && spawnZoneCollider != null)
        {
            Time.timeScale = 1;
            levelUI.SetActive(false);
            pause.SetActive(true);
            speed.SetActive(true);
            //CheckpointArrow.instance.updateArrow(0) ;
            LevelManager.instance.yesPause();
            LevelManager.instance.setLevel(0);


            // Get a random point inside the spawn zone's bounds
            Vector3 center = spawnZoneCollider[0].bounds.center;
            Vector3 size = spawnZoneCollider[0].bounds.size;

            Vector3 randomPosition = new Vector3(
                Random.Range(center.x - size.x / 2, center.x + size.x / 2),
                center.y,
                Random.Range(center.z - size.z / 2, center.z + size.z / 2)
            );

            foreach (GameObject cone in cones)
            {
                cone.SetActive(true);
                Debug.Log("Cone should be gone.");
            }
            // Move player to spawn position
            player.position = randomPosition;
            player.rotation = Quaternion.identity;
            PointCounter.instance.resetPoint();



        }
    }

    public void level2()
    {
        PlayerPrefs.SetInt("CurrentPlay", 2);
        if (player != null && spawnZoneCollider != null)
        {
            Time.timeScale = 1;
            levelUI.SetActive(false);
            pause.SetActive(true);
            speed.SetActive(true);
            //CheckpointArrow.instance.updateArrow(1);
            LevelManager.instance.yesPause();   
            LevelManager.instance.setLevel(1);


            // Get a random point inside the spawn zone's bounds
            Vector3 center = spawnZoneCollider[1].bounds.center;
            Vector3 size = spawnZoneCollider[1].bounds.size;

            Vector3 randomPosition = new Vector3(
                Random.Range(center.x - size.x / 2, center.x + size.x / 2),
                center.y,
                Random.Range(center.z - size.z / 2, center.z + size.z / 2)
            );

            foreach (GameObject cone in cones)
            {
                cone.SetActive(true);
                Debug.Log("Cone should be gone.");

            }

            // Move player to spawn position
            player.position = randomPosition;
            player.rotation = Quaternion.identity;
            PointCounter.instance.resetPoint();


        }
    }

    public void level3()
    {
        PlayerPrefs.SetInt("CurrentPlay", 3);
        if (player != null && spawnZoneCollider != null)
        {
            Time.timeScale = 1;
            levelUI.SetActive(false);
            pause.SetActive(true);
            speed.SetActive(true);
            //CheckpointArrow.instance.updateArrow(2);
            LevelManager.instance.yesPause();
            LevelManager.instance.setLevel(2);
            popupController.instance.ShowPopup("Bawa kereta ke hujung lebuh raya tanpa dilanggar. Peka dengan keadaan sekeliling!");

            // Get a random point inside the spawn zone's bounds
            Vector3 center = spawnZoneCollider[2].bounds.center;
            Vector3 size = spawnZoneCollider[2].bounds.size;

            Vector3 randomPosition = new Vector3(
                Random.Range(center.x - size.x / 2, center.x + size.x / 2),
                center.y,
                Random.Range(center.z - size.z / 2, center.z + size.z / 2)
            );

            foreach (GameObject cone in cones)
            {
                cone.SetActive(true);
                Debug.Log("Cone should be gone.");

            }

            // Move player to spawn position
            player.position = randomPosition;
            player.rotation = Quaternion.identity;
            PointCounter.instance.resetPoint();


        }
    }

    public void level4()
    {
        PlayerPrefs.SetInt("CurrentPlay", 4);
        if (player != null && spawnZoneCollider != null)
        {
            Time.timeScale = 1;
            levelUI.SetActive(false);
            pause.SetActive(true);
            speed.SetActive(true);
            //CheckpointArrow.instance.updateArrow(3);
            LevelManager.instance.yesPause();
            LevelManager.instance.setLevel(3);


            // Get a random point inside the spawn zone's bounds
            Vector3 center = spawnZoneCollider[3].bounds.center;
            Vector3 size = spawnZoneCollider[3].bounds.size;

            Vector3 randomPosition = new Vector3(
                Random.Range(center.x - size.x / 2, center.x + size.x / 2),
                center.y,
                Random.Range(center.z - size.z / 2, center.z + size.z / 2)
            );

            foreach (GameObject cone in cones)
            {
                cone.SetActive(true);
                Debug.Log("Cone should be gone.");

            }


            // Move player to spawn position
            player.position = randomPosition;
            player.rotation = Quaternion.identity;
            PointCounter.instance.resetPoint();


        }
    }

    public void level5()
    {
        PlayerPrefs.SetInt("CurrentPlay", 5);
        if (player != null && spawnZoneCollider != null)
        {
            Time.timeScale = 1;
            levelUI.SetActive(false);
            pause.SetActive(true);
            speed.SetActive(true);
            LevelManager.instance.setLevel(4);
            //CheckpointArrow.instance.hideArrow();
            LevelManager.instance.yesPause();
            foreach (GameObject cone in cones)
            {
                cone.SetActive(false);
            }

            // If you have roadblock colliders, also disable them:
            endpoints.SetActive(false);



            // Get a random point inside the spawn zone's bounds
            Vector3 center = spawnZoneCollider[4].bounds.center;
            Vector3 size = spawnZoneCollider[4].bounds.size;

            Vector3 randomPosition = new Vector3(
                Random.Range(center.x - size.x / 2, center.x + size.x / 2),
                center.y,
                Random.Range(center.z - size.z / 2, center.z + size.z / 2)
            );


            // Move player to spawn position
            player.position = randomPosition;
            player.rotation = Quaternion.identity;
            PointCounter.instance.resetPoint();

        }
    }

   
    


}
