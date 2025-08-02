using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIHandleDC : MonoBehaviour
{
    // Make sure this is assigned to the "Panel" that contains Resume, Settings, etc.
    public GameObject pauseMenu; 
    
    // This should be assigned to your "Panel (1)" that will contain the sliders.
    public GameObject settingsPanel; 
    
    public GameObject pauseBtn;

    private bool isPaused;
    private bool allowPause;
    
    private void Start()
    {
        allowPause = true;
        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void GoHome()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("DCScene");
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1; 
        SceneManager.LoadScene("Title Screen");
    }
    
    // NEW: Function for the Exit button
    public void ExitGame()
    {
        Application.Quit();
        // Log for the editor since Application.Quit() won't work there
        Debug.Log("Quitting game..."); 
    }

    public void pauseGame()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        pauseBtn.SetActive(false);
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void resumeGame()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false); 
        
        pauseBtn.SetActive(true);
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OpenSettings()
    {
        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (pauseMenu != null) pauseMenu.SetActive(true);
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
            else if (isPaused)
            {
                if (settingsPanel != null && settingsPanel.activeInHierarchy)
                {
                    CloseSettings();
                }
                else
                {
                    resumeGame();
                }
            }
        }
    }
}