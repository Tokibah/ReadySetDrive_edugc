using UnityEngine;
using TMPro;

public class TriggerTextCycler : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject textboxUI;
    public TextMeshProUGUI textDisplay;

    [Header("Text Messages")]
    public string[] messages;

    private int currentMessageIndex = 0;
    private bool isPlayerInside = false;

    private void Start()
    {
        if (textboxUI != null)
        {
            textboxUI.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // This is the key change. We check if the object has a CharacterController
        // or a Rigidbody component, which typically represents the main player body.
        // This prevents other colliders (like a weapon or a child object) from triggering it.
        CharacterController playerController = other.GetComponent<CharacterController>();
        Rigidbody playerRigidbody = other.GetComponent<Rigidbody>();

        if (playerController != null || playerRigidbody != null)
        {
            // Now we know for sure it's the main player object.
            isPlayerInside = true;
            ShowNextMessage();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CharacterController playerController = other.GetComponent<CharacterController>();
        Rigidbody playerRigidbody = other.GetComponent<Rigidbody>();
        
        if (playerController != null || playerRigidbody != null)
        {
            isPlayerInside = false;
            if (textboxUI != null)
            {
                textboxUI.SetActive(false);
            }
        }
    }

    private void ShowNextMessage()
    {
        if (textboxUI == null || textDisplay == null) return;

        textboxUI.SetActive(true);

        if (messages.Length > 0)
        {
            textDisplay.text = messages[currentMessageIndex];
            currentMessageIndex++;

            if (currentMessageIndex >= messages.Length)
            {
                currentMessageIndex = 0;
            }
        }
        else
        {
            textDisplay.text = "No messages to display.";
        }
    }
}