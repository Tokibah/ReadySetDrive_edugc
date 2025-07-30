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
        if (isPlayerInZone && !hasEntered && Input.GetKeyDown(KeyCode.E))
        {
            hasEntered = true;
            enterPromptUI.SetActive(false);
            audio.Pause();
            StartCutscene();
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
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            enterPromptUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
            enterPromptUI.SetActive(false);
        }
    }
}
