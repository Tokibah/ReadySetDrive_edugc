using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string characterName; // Optional, for speaker's name
    [TextArea(3, 10)] // Makes it a multi-line text area in Inspector
    public string dialogueText;
    public float duration; // How long this line takes to appear, if typing effect
    public int cutsceneEventIndex = -1; // Index to trigger a specific Timeline marker/event
}

[System.Serializable]
public class DialogueSequence
{
    public DialogueLine[] lines;
}