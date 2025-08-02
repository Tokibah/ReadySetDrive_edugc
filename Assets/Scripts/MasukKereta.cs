using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.Audio; // <-- Add this namespace

public class MasukKereta : MonoBehaviour
{
    // The AudioMixer to control. Assign this in the Inspector.
    [Header("Audio")]
    public AudioMixer gameAudioMixer;
    public string masterVolumeExposer = "MasterVolume"; // The name of the exposed parameter

    public GameObject enterPromptUI;
    public VideoPlayer videoPlayer;
    public GameObject videoController;

    private bool isPlayerInZone = false;
    private bool hasEntered = false;

    // We can use a boolean flag to track if the audio was muted.
    private bool wasAudioMuted = false;

    void Update()
    {
        if (!PlayerPrefs.HasKey("HasPlayedBefore"))
        {
            if (isPlayerInZone && !hasEntered && Input.GetKeyDown(KeyCode.E))
            {
                hasEntered = true;
                enterPromptUI.SetActive(false);
                
                // Mute the audio mixer here before starting the cutscene
                MuteAudioMixer();

                StartCutscene();
            }
        }
        else
        {
            // The return statement is not necessary here, as nothing else happens after this block.
            // The PlayerPrefs check can also be moved to a cleaner location if needed.
            return;
        }
    }

    void StartCutscene()
    {
        videoController.SetActive(true);
        videoPlayer.Play();
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        // Unmute the audio mixer here before loading the next scene
        UnmuteAudioMixer();

        SceneManager.LoadScene(3);
    }
    
    // Helper function to mute the AudioMixer
    private void MuteAudioMixer()
    {
        if (gameAudioMixer != null)
        {
            // Setting the exposed parameter to -80f is considered a full mute
            gameAudioMixer.SetFloat(masterVolumeExposer, -80f);
            wasAudioMuted = true;
        }
    }

    // Helper function to unmute the AudioMixer
    private void UnmuteAudioMixer()
    {
        if (gameAudioMixer != null && wasAudioMuted)
        {
            // Set the master volume back to its default value (e.g., 0 dB)
            gameAudioMixer.SetFloat(masterVolumeExposer, 0f);
            wasAudioMuted = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!PlayerPrefs.HasKey("HasPlayedBefore"))
        {
            if (other.CompareTag("Player"))
            {
                isPlayerInZone = true;
                enterPromptUI.SetActive(true);
            }
        }
        else
        {
            return;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!PlayerPrefs.HasKey("HasPlayedBefore"))
        {
            if (other.CompareTag("Player"))
            {
                isPlayerInZone = false;
                enterPromptUI.SetActive(false);
            }
        }
        else
        {
            return;
        }
    }
}