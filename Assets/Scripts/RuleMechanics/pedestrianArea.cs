// Example script for the PedestrianCrossingTrigger GameObject

using UnityEngine;
using System.Collections; // For Coroutines

public class pedestrianArea : MonoBehaviour
{
    // Assign your NPCs in the Inspector
    public GameObject[] npcsToCross;

    private bool playerInZone = false;
    private bool npcsAreCrossing = false;

   

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Check if the entering collider is the player's car
            if (!playerInZone)
            {
                playerInZone = true;
                Debug.Log("Player car entered crossing zone!");


                npcsAreCrossing = true;
                StartCoroutine(InitiateCrossingSequence());
            }
            else
            {
                Debug.Log("uhhh");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Reset when player leaves the zone (optional, depending on desired behavior)
            playerInZone = false;
            if (npcsAreCrossing)
            {
                popupController.instance.ShowPopup("Kenapa lintas macam itu sahaja? Ini kawasan sekolah, bahaya!");
                PointCounter.instance.ruleBroken();
            }
            else
            {
                PointCounter.instance.ruleFollowed();
            }
        }
    }

    IEnumerator InitiateCrossingSequence()
    {

        // Optional: Small delay before NPCs start moving for realism
        yield return new WaitForSeconds(2f);

        // 2. Trigger NPCs to start crossing
        foreach (GameObject npc in npcsToCross)
        {
            // Assuming NPCs have a script like 'NPCCrossingBehavior'
            NPCCrossing npcBehavior = npc.GetComponent<NPCCrossing>();
            if (npcBehavior != null)
            {
                npcBehavior.StartCrossing(); // Call a method on the NPC's script
            }
        }

        npcsAreCrossing = false;
    }
}




