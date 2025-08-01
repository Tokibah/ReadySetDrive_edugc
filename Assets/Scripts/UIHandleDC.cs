using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIHandleDC : MonoBehaviour
{


    public GameObject pauseMenu;
    public GameObject pauseBtn;

    private bool isPaused;
    private bool allowPause;

    private void Start()
    {
        allowPause = true;
    }


    public void GoHome()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("DCScene");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Title Screen");
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
        if (Input.GetKeyDown(KeyCode.Escape))
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
