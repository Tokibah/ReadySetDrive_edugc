using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class TitleScreenManager : MonoBehaviour
{
    [Header("UI")]
    [Tooltip("The CanvasGroup component of your logo. This is used for fading.")]
    public CanvasGroup logoCanvasGroup; // Changed from RectTransform to CanvasGroup
    public GameObject pressAnyButtonText;
    public GameObject mainMenuPanel;
    public GameObject quitPanel;

    [Header("Buttons")]
    public Button continueButton;
    public Button newGameButton;
    public Button settingsButton;


    [Header("Logo Fade Animation")] // Renamed header
    [Tooltip("How fast the logo will fade out.")]
    public float logoFadeSpeed = 1.5f; // Speed for fading (alpha per second)
    [Tooltip("The target alpha value for the logo (0 for fully transparent).")]
    [Range(0f, 1f)]
    public float logoTargetAlpha = 0f; // Target alpha for fading out

    [Header("Camera Movement & Spawner")] // Updated header
    public CameraMover cameraMover;
    [Tooltip("The Spawner script to disable after a button press.")]
    public Spawner spawner; // Added reference to Spawner

    private bool hasPressedStart = false;
    private bool isLogoFading = false; // Renamed for clarity

    public VideoPlayer videoPlayer;
    public int level;

    void Start()
    {
        mainMenuPanel.SetActive(false);

        // Ensure the logo starts fully visible if it's meant to fade out
        if (logoCanvasGroup != null)
        {
            logoCanvasGroup.alpha = 1f;
            logoCanvasGroup.interactable = true; // Make sure it's interactable before fade
            logoCanvasGroup.blocksRaycasts = true; // Make sure it blocks raycasts before fade
        }

        bool isFirstTime = !PlayerPrefs.HasKey("HasPlayedBefore");
        continueButton.gameObject.SetActive(!isFirstTime);

        // Basic validation for logoCanvasGroup
        if (logoCanvasGroup == null)
        {
            Debug.LogError("TitleScreenManager: 'Logo Canvas Group' is not assigned! Please assign the CanvasGroup component of your logo in the Inspector.", this);
            // You might want to disable fading if it's not set up
        }
    }

    void Update()
    {
        if (!hasPressedStart && Input.anyKeyDown)
        {
            hasPressedStart = true;
            pressAnyButtonText.SetActive(false);
            isLogoFading = true; // Start the fade animation

            if (cameraMover != null)
            {
                cameraMover.StartCameraMove();
            }

            // Disable the spawner after the button is pressed
            if (spawner != null)
            {
                spawner.enabled = false;
                Debug.Log("Spawner disabled.");
            }
        }

        // Handle logo fading
        if (isLogoFading && logoCanvasGroup != null)
        {
            // Gradually decrease the alpha towards the target (0)
            logoCanvasGroup.alpha = Mathf.MoveTowards(
                logoCanvasGroup.alpha,
                logoTargetAlpha,
                logoFadeSpeed * Time.deltaTime
            );

            // Check if the logo has faded out completely
            if (Mathf.Abs(logoCanvasGroup.alpha - logoTargetAlpha) < 0.01f)
            {
                logoCanvasGroup.alpha = logoTargetAlpha; // Snap to target alpha
                isLogoFading = false; // Stop fading

                // Optionally disable interaction and raycasts once fully faded
                logoCanvasGroup.interactable = false;
                logoCanvasGroup.blocksRaycasts = false;

                ShowMainMenu();
            }
        }
    }

    void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
    }

    public void OnNewGame()
    {
        LevelUnlock.ResetAllProgress();
         pressAnyButtonText.SetActive(false);
        mainMenuPanel.SetActive(false);
        videoPlayer.loopPointReached += onVideoEnd;
        videoPlayer.Play();
    }

    public void OnContinue()
    {
        SceneManager.LoadScene("HomeScene");
    }

    public void OnSettings()
    {
        Debug.Log("Settings clicked");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void quitConfirm()
    {
        quitPanel.SetActive(true);
    }

    public void quitCancel()
    {
        quitPanel.SetActive(false);
    }

    void onVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(level);
    }
}
