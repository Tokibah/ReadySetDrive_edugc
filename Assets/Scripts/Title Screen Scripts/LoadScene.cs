using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.Audio;

public class LoadScene : MonoBehaviour
{
    // The AudioMixer to control. Assign this in the Inspector.
    [Header("Audio")]
    public AudioMixer gameAudioMixer;

    // References to the UI and video player components
    [Header("UI & Video")]
    public VideoPlayer videoPlayer;
    public GameObject ui, logo;
    public int level;

    // This method is called when the start button is clicked.
    public void startGame()
    {
        // Check if an AudioMixer has been assigned.
        if (gameAudioMixer != null)
        {
            // Mute the master volume by setting the exposed parameter to -80 dB.
            // This assumes you have exposed a parameter named "MasterVolume" in your AudioMixer.
            gameAudioMixer.SetFloat("MasterVolume", -80f);
        }

        // Hide UI elements to prepare for the video playback.
        ui.SetActive(false);
        logo.SetActive(false);
        
        // Subscribe to the video player's event for when the video finishes.
        videoPlayer.loopPointReached += onVideoEnd;
        
        // Start playing the video.
        videoPlayer.Play();

        if (PlayerPrefs.HasKey("HasPlayedBefore"))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }



    // This method is the callback for when the video has finished playing.
    void onVideoEnd(VideoPlayer vp)
    {
        // Unmute the audio before loading the next scene.
        // We set the volume back to a reasonable default (e.g., 0 dB, which is full volume).
        if (gameAudioMixer != null)
        {
            gameAudioMixer.SetFloat("MasterVolume", 0f);
        }
        
        // Load the next scene specified by the 'level' variable.
        SceneManager.LoadScene(level);
    }
}