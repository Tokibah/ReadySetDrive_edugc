using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Playables;
using System.Collections; // Still good to keep in case you need other coroutines
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    [Header("UI Elements")]
    [Tooltip("The root Canvas GameObject that contains all dialogue UI.")]
    public GameObject dialogueCanvasGameObject;
    [Tooltip("The TextMeshProUGUI component that displays the dialogue lines (should be a child of the Canvas).")]
    public TextMeshProUGUI dialogueText;

    [Header("Timeline Settings")]
    [Tooltip("Drag the PlayableDirector component from this GameObject here.")]
    public PlayableDirector cutsceneTimeline;

    [Header("Dialogue Content")]
    [TextArea(3, 10)]
    [Tooltip("List of dialogue sentences to display in order.")]
    public List<string> dialogueLines;

    [Header("Input Settings")]
    [Tooltip("The key to press to advance to the next dialogue line or end the cutscene.")]
    public KeyCode advanceDialogueKey = KeyCode.Space;

    private int currentDialogueIndex = 0;
    private bool waitingForInput = false;

    void Update()
    {
        if (waitingForInput && Input.GetKeyDown(advanceDialogueKey))
        {
            AdvanceDialogue();
        }
    }

    /// <summary>
    /// Called by a Signal Emitter from the Timeline to begin or advance a dialogue segment.
    /// This method will display the current dialogue line and pause the Timeline.
    /// </summary>
    public void StartDialogueSegment()
    {
        // This check is crucial for when AdvanceDialogue() allows timeline to continue
        // and it reaches the NEXT StartDialogueSegment signal.
        // It ensures we don't try to show dialogue beyond our list.
        if (currentDialogueIndex < dialogueLines.Count)
        {
            dialogueCanvasGameObject.SetActive(true); // Show the entire dialogue Canvas
            dialogueText.text = dialogueLines[currentDialogueIndex]; // Set the current dialogue text

            waitingForInput = true; // Set flag to wait for user input
            cutsceneTimeline.Pause(); // Pause the Timeline until user advances
            
            // --- Optional: Add code here to disable player movement, game input, etc. ---
        }
        else
        {
            // If the Timeline somehow triggers this at the very end when no more lines are expected,
            // ensure it cleans up. This can happen if the last signal is too close to the end.
            EndDialogue();
        }
    }

    /// <summary>
    /// Called when the user presses the 'advanceDialogueKey'.
    /// This method progresses the dialogue by resuming the Timeline.
    /// </summary>
    private void AdvanceDialogue()
    {
        if (waitingForInput) // Ensure we are actually in a waiting state
        {
            waitingForInput = false; // Stop waiting for input for this line
            currentDialogueIndex++; // Increment for the *next* dialogue segment

            cutsceneTimeline.Resume(); // <-- **This is the key:** Resume Timeline and let it play.

            // The Timeline will now play until it hits the next Signal Emitter,
            // which will call StartDialogueSegment() again (and pause it).
            // Or, if no more signals, it will play to the end.

            // We don't call StartDialogueSegment() directly here anymore,
            // because the Timeline will call it when it hits the next signal.
            
            // We do need to handle the case where this was the *last* dialogue line.
            // If the timeline finished and didn't hit another signal, it needs to clean up.
            // A small delay and a check can help, or rely on EndDialogue() being called at the Timeline's end.
            if (currentDialogueIndex >= dialogueLines.Count)
            {
                // If this was the last dialogue line, and no more signals are coming,
                // ensure the dialogue canvas hides and the timeline fully plays out.
                EndDialogue(); // Call EndDialogue if all lines are done
                startGame(); // moves to the next scene
            }
        }
    }

    /// <summary>
    /// Hides the dialogue UI and ensures the Timeline is fully resumed.
    /// This signifies the end of the dialogue cutscene.
    /// </summary>
    public void EndDialogue()
    {
        dialogueCanvasGameObject.SetActive(false); // Hide the entire dialogue Canvas

        if (cutsceneTimeline.state == PlayState.Paused)
        {
            cutsceneTimeline.Resume(); // Make sure timeline is always playing at the end
        }
        
        // --- Optional: Add code here to re-enable player movement, game input, etc. ---
        Debug.Log("Dialogue cutscene ended.");
    }

    /// <summary>
    /// Call this method from the Timeline (e.g., using a Signal Emitter at 0:00)
    /// to ensure the cutscene starts with the dialogue Canvas hidden and variables reset.
    /// </summary>
    public void InitializeCutscene()
    {
        currentDialogueIndex = 0;
        dialogueCanvasGameObject.SetActive(false); // Ensure the dialogue Canvas is hidden at the very start
        waitingForInput = false;
        
        // --- Optional: Add code here to disable player movement at the beginning of the cutscene ---
    }

    public void startGame()
    {
        SceneManager.LoadSceneAsync(2);
    }

}