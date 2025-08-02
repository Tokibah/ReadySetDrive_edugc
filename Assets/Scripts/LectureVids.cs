using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.Video;
using UnityEngine.Audio;

public class LectureVids : MonoBehaviour
{
    private int unlockedVideoCount = 1;

    [Header("UI Elements")]
    public GameObject popupMessageUI;
    public GameObject lectureUI;
    public TextMeshProUGUI[] optionButtons;
    public TextMeshProUGUI exit;

    [Header("Player References")]
    public PlayerMovement playerMovementScript;

    [Header("Video Playback")]
    public VideoPlayer videoPlayer;
    public VideoClip[] lectureVideos;
    [Tooltip("The GameObject where the video will be rendered, typically a plane or a RawImage for UI.")]
    public GameObject videoScreenObject;

    [Header("Video Settings")]
    [Tooltip("The volume to set the video player to when it starts playing (0.0 to 1.0).")]
    [Range(0f, 1f)]
    public float videoVolume = 0.11f;
    
    [Header("Audio Mixer")]
    [Tooltip("Assign your main AudioMixer to mute when the video plays.")]
    public AudioMixer gameAudioMixer;
    public string masterVolumeExposer = "MasterVolume";
    private float originalMasterVolume;

    private bool playerInRange = false;
    private bool inConversation = false;
    private int selectedOptionIndex = 0;
    private bool canSelectOption = false;

    private void Start()
    {
        unlockedVideoCount = PlayerPrefs.GetInt("UnlockedVideos", 4);
        if (popupMessageUI != null) popupMessageUI.SetActive(false);
        if (lectureUI != null) lectureUI.SetActive(false);
        if (videoScreenObject != null) videoScreenObject.SetActive(false);

        UpdateOptionDisplay();

        if (videoPlayer != null)
        {
            videoPlayer.playOnAwake = false;
            videoPlayer.isLooping = false;

            // --- CHANGED: Use CameraNearPlane render mode ---
            videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !inConversation)
        {
            EnterConversationState();
        }

        if (inConversation)
        {
            HandleConversationInput();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (popupMessageUI != null) popupMessageUI.SetActive(true);
            Debug.Log("Player entered range of " + gameObject.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (inConversation)
            {
                ExitConversationState();
            }
            if (popupMessageUI != null) popupMessageUI.SetActive(false);
            Debug.Log("Player exited range of " + gameObject.name);
        }
    }

    private void EnterConversationState()
    {
        inConversation = true;
        Debug.Log("Entering conversation state.");

        if (playerMovementScript != null)
        {
            playerMovementScript.SetCanMove(false);
        }

        if (popupMessageUI != null) popupMessageUI.SetActive(false);
        if (lectureUI != null) lectureUI.SetActive(true);

        selectedOptionIndex = 0;
        UpdateOptionDisplay();

        StartCoroutine(EnableSelectionAfterDelay(0.1f));
    }

    public void ExitConversationState()
    {
        inConversation = false;
        Debug.Log("Exiting conversation state.");

        canSelectOption = false;

        if (playerMovementScript != null)
        {
            playerMovementScript.SetCanMove(true);
        }

        if (lectureUI != null) lectureUI.SetActive(false);
        if (videoScreenObject != null) videoScreenObject.SetActive(false);

        if (playerInRange && popupMessageUI != null)
        {
            popupMessageUI.SetActive(true);
        }
    }

    private IEnumerator EnableSelectionAfterDelay(float delay)
    {
        canSelectOption = false;
        yield return new WaitForSeconds(delay);
        canSelectOption = true;
        Debug.Log("Option selection enabled.");
    }

    private void HandleConversationInput()
    {
        if (!canSelectOption) return;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectedOptionIndex--;
            if (selectedOptionIndex < 0)
            {
                selectedOptionIndex = optionButtons.Length - 1;
            }
            UpdateOptionDisplay();
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectedOptionIndex++;
            if (selectedOptionIndex >= optionButtons.Length)
            {
                selectedOptionIndex = 0;
            }
            UpdateOptionDisplay();
        }

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
                if (i == selectedOptionIndex)
                {
                    optionButtons[i].color = isUnlocked ? Color.yellow : Color.gray;
                    optionButtons[i].fontStyle = isUnlocked ? FontStyles.Bold : FontStyles.Italic;
                }
                else
                {
                    optionButtons[i].color = isUnlocked ? Color.black : Color.gray;
                    optionButtons[i].fontStyle = FontStyles.Normal;
                }
            }
        }
    }

    private void SelectOption()
    {
        canSelectOption = false;

        if (selectedOptionIndex >= unlockedVideoCount)
        {
            Debug.Log("Option is locked.");
            StartCoroutine(EnableSelectionAfterDelay(0.1f));
            return;
        }

        switch (selectedOptionIndex)
        {
            case 0:
                Debug.Log("Selected: Nevermind... - Exiting conversation.");
                ExitConversationState();
                break;
            case 1:
                Debug.Log("Selected: Papan tanda berhenti - Playing video...");
                PlayRoadSignVideo(1);
                break;
            case 2:
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

    private void PlayRoadSignVideo(int videoType)
    {
        if (lectureUI != null) lectureUI.SetActive(false);

        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer is not assigned in the Inspector!");
            ExitConversationState();
            return;
        }

        if (videoType >= 1 && videoType <= lectureVideos.Length)
        {
            if (lectureVideos[videoType - 1] == null)
            {
                Debug.LogError($"VideoClip for type {videoType} is not assigned!");
                ExitConversationState();
                return;
            }
            videoPlayer.clip = lectureVideos[videoType - 1];
        }
        else
        {
            Debug.LogWarning("Unknown video type requested: " + videoType);
            ExitConversationState();
            return;
        }

        if (gameAudioMixer != null)
        {
            gameAudioMixer.GetFloat(masterVolumeExposer, out originalMasterVolume);
            gameAudioMixer.SetFloat(masterVolumeExposer, -80f);
        }

        if (videoScreenObject != null)
        {
            videoScreenObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Video Screen Object is not assigned. Video might not be visible.");
        }

        videoPlayer.loopPointReached += OnVideoEnd;

        if (videoPlayer.audioOutputMode == VideoAudioOutputMode.Direct)
        {
            videoPlayer.SetDirectAudioVolume(0, videoVolume);
        }
        else
        {
             Debug.LogWarning("Video Player's Audio Output Mode is not set to 'Direct'. Video volume will not be controlled.");
        }

        videoPlayer.Play();
        Debug.Log($"Playing video: {videoPlayer.clip.name}");
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("Video finished playing.");
        vp.Stop();

        if (gameAudioMixer != null)
        {
            gameAudioMixer.SetFloat(masterVolumeExposer, originalMasterVolume);
        }

        if (vp.audioOutputMode == VideoAudioOutputMode.Direct)
        {
            vp.SetDirectAudioVolume(0, 0f);
        }

        vp.loopPointReached -= OnVideoEnd;

        if (videoScreenObject != null)
        {
            videoScreenObject.SetActive(false);
        }

        if (lectureUI != null)
        {
            lectureUI.SetActive(true);
        }

        StartCoroutine(EnableSelectionAfterDelay(0.1f));
    }
}