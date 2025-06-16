using UnityEngine;
using UnityEngine.SceneManagement;

public class busTeleport : MonoBehaviour
{
    public GameObject popup;
    public bool playerInRange = false;



    private void OnTriggerEnter(Collider other)
    {
        popup.SetActive(true);
        playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        popup.SetActive(false);
        playerInRange = false;
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            //Time.timeScale = 1;

            Scene scene = SceneManager.GetActiveScene();
            if (scene.name == "HomeScene")
            {
                SceneManager.LoadScene("DCScene");
            }
            else
            {
                SceneManager.LoadScene("HomeScene");
            }
            Time.timeScale = 1;
        }
    }
}
