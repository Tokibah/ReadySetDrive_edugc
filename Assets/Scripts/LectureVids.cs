using UnityEngine;
using UnityEngine.UI; // Required for UI elements like Text, Button (though we'll use TMPro for text)
using System.Collections; // Required for Coroutines
using TMPro; // Required for TextMeshProUGUI
using UnityEngine.Video; // Required for VideoPlayer

public class LectureVids : MonoBehaviour
{
    private int unlockedVideoCount = 1; // default to 1 video unlocked

    [Header("UI Elements")]
    public GameObject popupMessageUI; // UI element to show "Press E to interact"
    public GameObject lectureUI;
    public TextMeshProUGUI[] optionButtons; // Changed from Text[] to TextMeshProUGUI[]
    public TextMeshProUGUI exit;

    [Header("Player References")]
    public PlayerMovement playerMovementScript; // Reference to your player's movement script
                                                // Make sure this script has a public method like SetCanMove(bool canMove)
                                                // or a public property like bool CanMove { get; set; }

    [Header("Video Playback")]
    public VideoPlayer videoPlayer; // Reference to the VideoPlayer component
    public VideoClip[] lectureVideos; // The video clip for "Papan tanda berhenti"
    public GameObject videoScreenObject; // A GameObject where the video will be rendered (e.g., a RawImage on UI or a plane in world)
    public RawImage videoDisplayRawImage; // If videoScreenObject is a RawImage, assign it here
    public RenderTexture videoRenderTexture; // A RenderTexture to render the video onto

    private bool playerInRange = false;
    private bool inConversation = false;
    private int selectedOptionIndex = 0; // 0: Papan tanda berhenti, 1: Kawasan Sekolah, 2: Nevermind...

    private bool canSelectOption = false; // New flag to control when options can be selected

    // --- Unity Lifecycle Methods ---

    private void Start()
    {
        unlockedVideoCount = PlayerPrefs.GetInt("UnlockedVideos", 4);
        // Ensure UI elements are hidden at the start
        if (popupMessageUI != null) popupMessageUI.SetActive(false);
        if (lectureUI!= null) lectureUI.SetActive(false);
        if (videoScreenObject != null) videoScreenObject.SetActive(false); // Ensure video screen is hidden

        // Initialize option text colors
        UpdateOptionDisplay();

        // Initialize VideoPlayer settings
        if (videoPlayer != null)
        {
            videoPlayer.playOnAwake = false; // Don't play automatically
            videoPlayer.isLooping = false; // Don't loop by default

            // Set the target texture for the video player
            if (videoRenderTexture != null)
            {
                videoPlayer.renderMode = VideoRenderMode.RenderTexture;
                videoPlayer.targetTexture = videoRenderTexture;
            }
            else
            {
                Debug.LogWarning("Video Render Texture is not assigned. Video might not display correctly.");
            }

            // If using a RawImage, assign the RenderTexture to it
            if (videoDisplayRawImage != null && videoRenderTexture != null)
            {
                videoDisplayRawImage.texture = videoRenderTexture;
            }
        }
    }

    private void Update()
    {
        // Check for interaction to enter conversation
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !inConversation)
        {
            EnterConversationState();
        }

        // Handle input during conversation
        if (inConversation)
        {
            HandleConversationInput();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering collider is the player
        if (other.CompareTag("Player")) // Make sure your player GameObject has the tag "Player"
        {
            playerInRange = true;
            if (popupMessageUI != null) popupMessageUI.SetActive(true);
            Debug.Log("Player entered range of " + gameObject.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the exiting collider is the player
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            // Always exit conversation if player leaves range
            if (inConversation)
            {
                ExitConversationState();
            }
            if (popupMessageUI != null) popupMessageUI.SetActive(false);
            Debug.Log("Player exited range of " + gameObject.name);
        }
    }

    // --- Conversation State Management ---

    private void EnterConversationState()
    {
        inConversation = true;
        Debug.Log("Entering conversation state.");

        // Disable player movement
        if (playerMovementScript != null)
        {
            playerMovementScript.SetCanMove(false); // Assuming PlayerMovement has this method
        }

        // Hide popup and show dialogue panel
        if (popupMessageUI != null) popupMessageUI.SetActive(false);
        if (lectureUI != null) lectureUI.SetActive(true);

        // Reset selected option and update UI
        selectedOptionIndex = 0;
        UpdateOptionDisplay();

        // Start coroutine to enable option selection after a short delay
        StartCoroutine(EnableSelectionAfterDelay(0.1f)); // 0.1 seconds delay
    }

    public void ExitConversationState() // Made public so it can be called externally if needed (e.g., by video player)
    {
        inConversation = false;
        Debug.Log("Exiting conversation state.");

        // Disable option selection immediately upon exiting conversation
        canSelectOption = false;

        // Enable player movement
        if (playerMovementScript != null)
        {
            playerMovementScript.SetCanMove(true); // Assuming PlayerMovement has this method
        }

        // Hide dialogue panel
        if (lectureUI != null) lectureUI.SetActive(false);

        // Hide video screen if it was active
        if (videoScreenObject != null) videoScreenObject.SetActive(false);

        // Show popup again if player is still in range (after conversation)
        if (playerInRange && popupMessageUI != null)
        {
            popupMessageUI.SetActive(true);
        }
    }

    // Coroutine to introduce a small delay before allowing option selection
    private IEnumerator EnableSelectionAfterDelay(float delay)
    {
        canSelectOption = false; // Ensure selection is disabled initially
        yield return new WaitForSeconds(delay); // Wait for the specified delay
        canSelectOption = true; // Enable selection after the delay
        Debug.Log("Option selection enabled.");
    }

    // --- Input Handling ---

    private void HandleConversationInput()
    {
        // Only process selection input if allowed
        if (!canSelectOption) return;

        // Navigate options with W/S or Up/Down arrow keys
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectedOptionIndex--;
            if (selectedOptionIndex < 0)
            {
                selectedOptionIndex = optionButtons.Length - 1; // Wrap around to last option
            }
            UpdateOptionDisplay();
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectedOptionIndex++;
            if (selectedOptionIndex >= optionButtons.Length)
            {
                selectedOptionIndex = 0; // Wrap around to first option
            }
            UpdateOptionDisplay();
        }

        // Confirm selection with E or Enter key
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return))
        {
            SelectOption();
        }
    }

    private void UpdateOptionDisplay()
    {
        if (optionButtons == null || optionButtons.Length == 0) return;

        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (optionButtons[i] != null)
            {
                bool isUnlocked = i < unlockedVideoCount;
                // Highlight the selected option, dim others
                if (i == selectedOptionIndex)
                {
                    optionButtons[i].color = isUnlocked ? Color.yellow : Color.gray; // Or any highlight color
                    optionButtons[i].fontStyle = isUnlocked ? FontStyles.Bold : FontStyles.Italic; // Use FontStyles for TextMeshPro
                }
                else
                {
                    optionButtons[i].color = isUnlocked ? Color.black : Color.gray; // Default color
                    optionButtons[i].fontStyle = FontStyles.Normal; // Use FontStyles for TextMeshPro
                }
            }
        }
    }

    private void SelectOption()
    {
        // Disable selection immediately after an option is chosen to prevent double-clicks
        canSelectOption = false;

        if (selectedOptionIndex >= unlockedVideoCount)
        {
            Debug.Log("Option is locked.");
            StartCoroutine(EnableSelectionAfterDelay(0.1f)); // re-enable selection
            return;
        }

        switch (selectedOptionIndex)
        {
            case 0: // Nevermind...
                Debug.Log("Selected: Nevermind... - Exiting conversation.");
                ExitConversationState();
                break;
            case 1: // Papan tanda berhenti
                Debug.Log("Selected: Papan tanda berhenti - Playing video...");
                PlayRoadSignVideo(1);
                break;
            case 2: // Kawasan Sekolah
                Debug.Log("Selected: Kawasan Sekolah - Playing video...");
                PlayRoadSignVideo(2);
                break;
            case 3:
                PlayRoadSignVideo(3);
                break;
            case 4:
                PlayRoadSignVideo(4);
                break;
            case 5:
                PlayRoadSignVideo(5);
                break;
            case 6:
                PlayRoadSignVideo(6);
                break;
            case 7:
                PlayRoadSignVideo(7);
                break;
            case 8:
                PlayRoadSignVideo(8);
                break;
            case 9:
                PlayRoadSignVideo(9);
                break;
            default:
                Debug.LogWarning("Invalid option selected.");
                break;
        }
    }

    // --- Video Playback Implementation ---

    private void PlayRoadSignVideo(int videoType)
    {
        // 1. Hide the dialogue panel
        if (lectureUI != null) lectureUI.SetActive(false);

        // 2. Ensure VideoPlayer and video clips are assigned
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer is not assigned in the Inspector!");
            ExitConversationState(); // Exit to prevent being stuck
            return;
        }

        if (videoType == 1)
        {
            if (lectureVideos == null)
            {
                Debug.LogError("Stop Sign VideoClip is not assigned!");
                ExitConversationState();
                return;
            }
            videoPlayer.clip = lectureVideos[0];
        }
        else if (videoType == 2)
        {
            if (lectureVideos == null)
            {
                Debug.LogError("School Area VideoClip is not assigned!");
                ExitConversationState();
                return;
            }
            videoPlayer.clip = lectureVideos[1];
        }
        else if (videoType == 3)
        {
            if (lectureVideos == null)
            {
                Debug.LogError("School Area VideoClip is not assigned!");
                ExitConversationState();
                return;
            }
            videoPlayer.clip = lectureVideos[2];
        }
        else if (videoType == 4)
        {
            if (lectureVideos == null)
            {
                Debug.LogError("School Area VideoClip is not assigned!");
                ExitConversationState();
                return;
            }
            videoPlayer.clip = lectureVideos[3];
        }
        else if (videoType == 5)
        {
            if (lectureVideos == null)
            {
                Debug.LogError("School Area VideoClip is not assigned!");
                ExitConversationState();
                return;
            }
            videoPlayer.clip = lectureVideos[4];
        }
        else if (videoType == 6)
        {
            if (lectureVideos == null)
            {
                Debug.LogError("School Area VideoClip is not assigned!");
                ExitConversationState();
                return;
            }
            videoPlayer.clip = lectureVideos[5];
        }
        else if (videoType == 7)
        {
            if (lectureVideos == null)
            {
                Debug.LogError("School Area VideoClip is not assigned!");
                ExitConversationState();
                return;
            }
            videoPlayer.clip = lectureVideos[6];
        }
        else if (videoType == 8)
        {
            if (lectureVideos == null)
            {
                Debug.LogError("School Area VideoClip is not assigned!");
                ExitConversationState();
                return;
            }
            videoPlayer.clip = lectureVideos[7];
        }
        else if (videoType == 9)
        {
            if (lectureVideos == null)
            {
                Debug.LogError("School Area VideoClip is not assigned!");
                ExitConversationState();
                return;
            }
            videoPlayer.clip = lectureVideos[8];
        }
        else
        {
            Debug.LogWarning("Unknown video type requested: " + videoType);
            ExitConversationState();
            return;
        }

        // 3. Show the screen where the video will play
        if (videoScreenObject != null)
        {
            videoScreenObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Video Screen Object is not assigned. Video might not be visible.");
        }

        // 4. Subscribe to the loopPointReached event to know when the video ends
        videoPlayer.loopPointReached += OnVideoEnd;

        // 5. Play the video
        videoPlayer.Play();
        Debug.Log($"Playing video: {videoPlayer.clip.name}");
    }

    // This method is called when the video finishes playing
    private void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("Video finished playing.");
        vp.Stop(); // Stop the video player
        vp.loopPointReached -= OnVideoEnd; // Unsubscribe to prevent multiple calls

        // Hide video screen
        if (videoScreenObject != null)
        {
            videoScreenObject.SetActive(false);
        }

        // Show dialogue panel again to return to conversation
        if (lectureUI != null)
        {
            lectureUI.SetActive(true);
        }

        // Re-enable option selection after a short delay
        StartCoroutine(EnableSelectionAfterDelay(0.1f));
    }
}
