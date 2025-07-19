using UnityEngine;
using UnityEngine.UI; // Required for UI elements like Text, Button (though we'll use TMPro for text)
using System.Collections; // Required for Coroutines
using TMPro; // Required for TextMeshProUGUI
using UnityEngine.Video; // Required for VideoPlayer

public class DialogueTrigger : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject popupMessageUI; // UI element to show "Press E to interact"
    public GameObject dialoguePanelUI; // Main panel containing conversation options
    public TextMeshProUGUI[] optionTexts; // Changed from Text[] to TextMeshProUGUI[]

    [Header("Player References")]
    public PlayerMovement playerMovementScript; // Reference to your player's movement script
                                                // Make sure this script has a public method like SetCanMove(bool canMove)
                                                // or a public property like bool CanMove { get; set; }

    [Header("Video Playback")]
    public VideoPlayer videoPlayer; // Reference to the VideoPlayer component
    public VideoClip stopSignVideo; // The video clip for "Papan tanda berhenti"
    public VideoClip schoolAreaVideo; // The video clip for "Kawasan Sekolah"
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
        // Ensure UI elements are hidden at the start
        if (popupMessageUI != null) popupMessageUI.SetActive(false);
        if (dialoguePanelUI != null) dialoguePanelUI.SetActive(false);
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
        if (dialoguePanelUI != null) dialoguePanelUI.SetActive(true);

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
        if (dialoguePanelUI != null) dialoguePanelUI.SetActive(false);

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
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedOptionIndex--;
            if (selectedOptionIndex < 0)
            {
                selectedOptionIndex = optionTexts.Length - 1; // Wrap around to last option
            }
            UpdateOptionDisplay();
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedOptionIndex++;
            if (selectedOptionIndex >= optionTexts.Length)
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
        if (optionTexts == null || optionTexts.Length == 0) return;

        for (int i = 0; i < optionTexts.Length; i++)
        {
            if (optionTexts[i] != null)
            {
                // Highlight the selected option, dim others
                if (i == selectedOptionIndex)
                {
                    optionTexts[i].color = Color.yellow; // Or any highlight color
                    optionTexts[i].fontStyle = FontStyles.Bold; // Use FontStyles for TextMeshPro
                }
                else
                {
                    optionTexts[i].color = Color.white; // Default color
                    optionTexts[i].fontStyle = FontStyles.Normal; // Use FontStyles for TextMeshPro
                }
            }
        }
    }

    private void SelectOption()
    {
        // Disable selection immediately after an option is chosen to prevent double-clicks
        canSelectOption = false;

        switch (selectedOptionIndex)
        {
            case 0: // Papan tanda berhenti
                Debug.Log("Selected: Papan tanda berhenti - Playing video...");
                PlayRoadSignVideo("StopSign");
                break;
            case 1: // Kawasan Sekolah
                Debug.Log("Selected: Kawasan Sekolah - Playing video...");
                PlayRoadSignVideo("SchoolArea");
                break;
            case 2: // Nevermind...
                Debug.Log("Selected: Nevermind... - Exiting conversation.");
                ExitConversationState();
                break;
            default:
                Debug.LogWarning("Invalid option selected.");
                break;
        }
    }

    // --- Video Playback Implementation ---

    private void PlayRoadSignVideo(string videoType)
    {
        // 1. Hide the dialogue panel
        if (dialoguePanelUI != null) dialoguePanelUI.SetActive(false);

        // 2. Ensure VideoPlayer and video clips are assigned
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer is not assigned in the Inspector!");
            ExitConversationState(); // Exit to prevent being stuck
            return;
        }

        if (videoType == "StopSign")
        {
            if (stopSignVideo == null)
            {
                Debug.LogError("Stop Sign VideoClip is not assigned!");
                ExitConversationState();
                return;
            }
            videoPlayer.clip = stopSignVideo;
        }
        else if (videoType == "SchoolArea")
        {
            if (schoolAreaVideo == null)
            {
                Debug.LogError("School Area VideoClip is not assigned!");
                ExitConversationState();
                return;
            }
            videoPlayer.clip = schoolAreaVideo;
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
        if (dialoguePanelUI != null)
        {
            dialoguePanelUI.SetActive(true);
        }

        // Re-enable option selection after a short delay
        StartCoroutine(EnableSelectionAfterDelay(0.1f));
    }
}
