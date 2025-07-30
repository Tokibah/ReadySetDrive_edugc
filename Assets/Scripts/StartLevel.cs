using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;

public class StartLevel : MonoBehaviour
{
    public Transform player;            // Drag your player prefab or object here
    public Collider[] spawnZoneCollider;  // Drag the spawn zone collider here
    public GameObject levelUI, pause, speed;

    void Start()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
        levelUI.SetActive(true);
        
        LevelManager.instance.noPause();
    }

    public void level1()
    {
        if (player != null && spawnZoneCollider != null)
        {
            Time.timeScale = 1;
            levelUI.SetActive(false);
            pause.SetActive(true);
            speed.SetActive(true);
            //CheckpointArrow.instance.updateArrow(0) ;
            LevelManager.instance.yesPause();
            LevelManager.instance.lvl1();


            // Get a random point inside the spawn zone's bounds
            Vector3 center = spawnZoneCollider[0].bounds.center;
            Vector3 size = spawnZoneCollider[0].bounds.size;

            Vector3 randomPosition = new Vector3(
                Random.Range(center.x - size.x / 2, center.x + size.x / 2),
                center.y,
                Random.Range(center.z - size.z / 2, center.z + size.z / 2)
            );

            // Move player to spawn position
            player.position = randomPosition;
            player.rotation = Quaternion.identity;



        }
    }

    public void level2()
    {
        if (player != null && spawnZoneCollider != null)
        {
            Time.timeScale = 1;
            levelUI.SetActive(false);
            pause.SetActive(true);
            speed.SetActive(true);
            //CheckpointArrow.instance.updateArrow(1);
            LevelManager.instance.yesPause();   
            LevelManager.instance.lvl2();


            // Get a random point inside the spawn zone's bounds
            Vector3 center = spawnZoneCollider[1].bounds.center;
            Vector3 size = spawnZoneCollider[1].bounds.size;

            Vector3 randomPosition = new Vector3(
                Random.Range(center.x - size.x / 2, center.x + size.x / 2),
                center.y,
                Random.Range(center.z - size.z / 2, center.z + size.z / 2)
            );

            // Move player to spawn position
            player.position = randomPosition;
            player.rotation = Quaternion.identity;


        }
    }

    public void level3()
    {
        if (player != null && spawnZoneCollider != null)
        {
            Time.timeScale = 1;
            levelUI.SetActive(false);
            pause.SetActive(true);
            speed.SetActive(true);
            //CheckpointArrow.instance.updateArrow(2);
            LevelManager.instance.yesPause();
            LevelManager.instance.lvl3();
            popupController.instance.ShowPopup("Bawa kereta ke hujung lebuh raya tanpa dilanggar. Peka dengan keadaan sekeliling!");

            // Get a random point inside the spawn zone's bounds
            Vector3 center = spawnZoneCollider[2].bounds.center;
            Vector3 size = spawnZoneCollider[2].bounds.size;

            Vector3 randomPosition = new Vector3(
                Random.Range(center.x - size.x / 2, center.x + size.x / 2),
                center.y,
                Random.Range(center.z - size.z / 2, center.z + size.z / 2)
            );

            // Move player to spawn position
            player.position = randomPosition;
            player.rotation = Quaternion.identity;


        }
    }

    public void level4()
    {
        if (player != null && spawnZoneCollider != null)
        {
            Time.timeScale = 1;
            levelUI.SetActive(false);
            pause.SetActive(true);
            speed.SetActive(true);
            //CheckpointArrow.instance.updateArrow(3);
            LevelManager.instance.yesPause();
            LevelManager.instance.lvl4();


            // Get a random point inside the spawn zone's bounds
            Vector3 center = spawnZoneCollider[3].bounds.center;
            Vector3 size = spawnZoneCollider[3].bounds.size;

            Vector3 randomPosition = new Vector3(
                Random.Range(center.x - size.x / 2, center.x + size.x / 2),
                center.y,
                Random.Range(center.z - size.z / 2, center.z + size.z / 2)
            );


            // Move player to spawn position
            player.position = randomPosition;
            player.rotation = Quaternion.identity;


        }
    }

    public void level5()
    {
        if (player != null && spawnZoneCollider != null)
        {
            Time.timeScale = 1;
            levelUI.SetActive(false);
            pause.SetActive(true);
            speed.SetActive(true);
            LevelManager.instance.lvl5();
            //CheckpointArrow.instance.hideArrow();
            LevelManager.instance.yesPause();
            GameObject[] borders = GameObject.FindGameObjectsWithTag("WorldBorder");
            foreach (GameObject border in borders)
            {
                border.SetActive(false);
            }


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

        }
    }


}
