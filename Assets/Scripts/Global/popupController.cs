using System.Collections;
using UnityEngine;
using TMPro;

public class popupController : MonoBehaviour
{
    public static popupController instance;

    public GameObject popupPanel; // UI Panel or background for the popup
    public TextMeshProUGUI popupText; // The message text
    public TextMeshProUGUI popupEndText;
    public GameObject pauseBtn;
    public GameObject speedText;

    void Awake()
    {
        instance = this;
    }

    public void ShowPopup(string message, float duration = 3f)
    {
        StartCoroutine(ShowPopupCoroutine(message, duration));
    }

    public void summaryPopup(string message)
    {
        
        popupEndText.text = message;
    }

    IEnumerator ShowPopupCoroutine(string message, float duration)
    {


        // Show the popup
        popupPanel.SetActive(true);
        pauseBtn.SetActive(false);
        speedText.SetActive(false);
        popupText.text = message;

        // Wait in real time (unaffected by Time.timeScale)
        yield return new WaitForSecondsRealtime(duration);

        // Hide the popup
        popupPanel.SetActive(false);
        pauseBtn.SetActive(true);
        speedText.SetActive(true);


    }
}
