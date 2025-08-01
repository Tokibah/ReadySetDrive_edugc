using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MasukKereta : MonoBehaviour
{
    public GameObject enterPromptUI;            // "Press E to enter" text
    public VideoPlayer videoPlayer;             // Drag your video player here
    public GameObject videoController;          // GameObject that holds video
    public AudioSource audio;

    private bool isPlayerInZone = false;
    private bool hasEntered = false;

    void Update()
    {
        if (!PlayerPrefs.HasKey("HasPlayedBefore"))
        {
            if (isPlayerInZone && !hasEntered && Input.GetKeyDown(KeyCode.E))
            {
                hasEntered = true;
                enterPromptUI.SetActive(false);
                audio.Pause();
                StartCutscene();
            }
        }
        else
        {
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
        SceneManager.LoadScene(3);
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
