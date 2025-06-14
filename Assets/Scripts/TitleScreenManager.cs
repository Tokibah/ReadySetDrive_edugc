using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{

    [Header("UI")]
    public RectTransform logo; // UI Logo
    public GameObject pressAnyButtonText;
    public GameObject mainMenuPanel;

    [Header("Buttons")]
    public Button continueButton;
    public Button newGameButton;
    public Button settingsButton;

    [Header("Logo Animation")]
    public float logoSlideSpeed = 800f;
    public Vector2 logoTargetPosition = new Vector2(-600f, 0f);

    [Header("Camera Movement")]
    public CameraMover cameraMover; // Drag your camera here

    private bool hasPressedStart = false;
    private bool isLogoMoving = false;

    void Start()
    {
        mainMenuPanel.SetActive(false);

        bool isFirstTime = !PlayerPrefs.HasKey("HasPlayedBefore");
        continueButton.gameObject.SetActive(!isFirstTime);
    }

    void Update()
    {
        if (!hasPressedStart && Input.anyKeyDown)
        {
            hasPressedStart = true;
            pressAnyButtonText.SetActive(false);
            isLogoMoving = true;

            if (cameraMover != null)
            {
                cameraMover.StartCameraMove(); // 🔽 Start camera drop
            }
        }

        if (isLogoMoving)
        {
            logo.anchoredPosition = Vector2.MoveTowards(
                logo.anchoredPosition,
                logoTargetPosition,
                logoSlideSpeed * Time.deltaTime
            );

            if (Vector2.Distance(logo.anchoredPosition, logoTargetPosition) < 1f)
            {
                logo.anchoredPosition = logoTargetPosition;
                isLogoMoving = false;
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
        PlayerPrefs.SetInt("HasPlayedBefore", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("GameScene");
    }

    public void OnContinue()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnSettings()
    {
        Debug.Log("Settings clicked");
    }
}
