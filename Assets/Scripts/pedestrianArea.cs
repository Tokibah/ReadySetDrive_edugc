// Example script for the PedestrianCrossingTrigger GameObject

using UnityEngine;
using System.Collections; // For Coroutines

public class pedestrianArea : MonoBehaviour
{
    // Assign your NPCs in the Inspector
    public GameObject[] npcsToCross;
    // Assign your Traffic Light GameObject (if you have one)

    private bool playerInZone = false;
    private bool npcsAreCrossing = false;

   

    void OnTriggerEnter(Collider other)
    {
        // Check if the entering collider is the player's car
        if (!playerInZone)
        {
            playerInZone = true;
            Debug.Log("Player car entered crossing zone!");

            // Trigger NPCs to cross and change traffic light
            if (!npcsAreCrossing)
            {
                StartCoroutine(InitiateCrossingSequence());
            }
        }
        else
        {
            Debug.Log("uhhh");
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Reset when player leaves the zone (optional, depending on desired behavior)
        
            playerInZone = false;
            Debug.Log("Player car exited crossing zone.");
            // You might want to reset the traffic light here if NPCs are not crossing
            // or if the player leaves before they finish.
        
    }

    IEnumerator InitiateCrossingSequence()
    {
        npcsAreCrossing = true;

       

        // Optional: Small delay before NPCs start moving for realism
        yield return new WaitForSeconds(1.5f);

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

        // 3. Wait for NPCs to finish crossing
        // This is a simplified wait. In a real game, you'd check if all NPCs have reached their destination.
        // For now, let's just wait a fixed time.
        yield return new WaitForSeconds(5f); // Adjust based on how long it takes NPCs to cross

        

        npcsAreCrossing = false;
        Debug.Log("NPCs finished crossing. Traffic light reset.");
    }
}



// Example script for a simple Traffic Light

