using System.Collections;
using UnityEngine;

public class intersectionStopTrigger : MonoBehaviour
{
    // Make sure your player GameObject has this tag assigned in the Inspector
    public string playerTag = "Player";

    public float waitDuration = 4f;
    private Coroutine waitCoroutine;
    private bool waited = false;
    private bool playerInTrigger = false; // New: Track if player is currently in the trigger

    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering collider belongs to the player
        if (other.CompareTag(playerTag))
        {
            playerInTrigger = true;
            // Stop any existing coroutine to prevent issues if player re-enters before old one finishes
            if (waitCoroutine != null)
            {
                StopCoroutine(waitCoroutine);
            }
            waitCoroutine = StartCoroutine(StopAndWait());
        }
    }

    IEnumerator StopAndWait()
    {
        Debug.Log("Please wait at intersection");
        // Only show popup for player
        if (playerInTrigger && popupController.instance != null)
        {
            popupController.instance.ShowPopup("Berhenti dahulu di garisan putih!"); // You can customize this initial popup
        }
        
        waited = false;
        yield return new WaitForSeconds(waitDuration);

        // Check again if player is still in trigger after waiting
        // This prevents giving points if player somehow left and re-entered
        if (playerInTrigger)
        {
            Debug.Log("Good to go. Add reputation.");
            waited = true; // Set waited to true ONLY if player is still in trigger
            PointCounter.instance.ruleFollowed();
        }
        else
        {
            // If player left during wait, treat as not waited
            waited = false;
            Debug.Log("Player left during wait period, no reputation gain for waiting.");
        }
        waitCoroutine = null; // Clear the coroutine reference
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the exiting collider belongs to the player
        if (other.CompareTag(playerTag))
        {
            playerInTrigger = false; // Player has exited the trigger

            // If the coroutine is still running, it means the player left before the wait duration completed.
            if (waitCoroutine != null)
            {
                StopCoroutine(waitCoroutine); // Stop the timer early
                Debug.Log("Left too early, reduce reputation!");
                if (popupController.instance != null)
                {
                    popupController.instance.ShowPopup("Awak sepatutnya berhenti dahulu! Bahaya keluar macam itu!");
                }
                if (PointCounter.instance != null)
                {
                    PointCounter.instance.ruleBroken(); 
                }
                waited = false; // Ensure waited is false since they left early
                waitCoroutine = null; // Clear the coroutine reference
            }
            else if (waited) // Player waited the full duration and then exited
            {
                Debug.Log("Player exited after waiting. Add reputation.");
                // PointCounter.instance.ruleFollowed(); // Already handled in StopAndWait if you enable it there.
                // Or you can put the ruleFollowed here if you only want to award upon successful exit after wait.
                // I recommend doing it after the wait, not on exit, to reward for the act of waiting itself.
            }
            else // This case handles if player enters, instantly leaves (duration 0), or similar oddities
            {
                Debug.Log("Player exited but didn't wait or no wait was initiated properly.");
                // You might choose to penalize here or do nothing, depending on desired strictness.
            }
        }
    }
}