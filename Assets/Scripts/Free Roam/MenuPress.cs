using UnityEngine;
using UnityEngine.Playables;

public class MenuPress : MonoBehaviour
{
    public GameObject pausePanel;
    public PlayableDirector pauseTimeline;

    private bool isPaused = false;

    void Start()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
                ShowPauseMenu();
            else
                HidePauseMenu();
        }
    }

    void ShowPauseMenu()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f; // Freeze game
        pauseTimeline.Play();
    }

    void HidePauseMenu()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ResumeGame()
    {
        HidePauseMenu();
    }
}
