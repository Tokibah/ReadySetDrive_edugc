// SpeedLimitZone.cs
// playerCar script is in the PROMOTEO - Car Control folder inside Custom Packages
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class SpeedLimitScript : MonoBehaviour
{
    [Header("Speed Limit Settings")]
    [Tooltip("The maximum speed allowed in this zone (units per second).")]
    public float speedLimit = 30f; // Set your desired speed limit here

    void OnTriggerEnter(Collider other)
    {
        // Check if the entering collider is the player car

        PrometeoCarController playerCar = other.GetComponentInParent<PrometeoCarController>();
        if (playerCar != null)
        {
            Debug.Log($"Player entered Speed Limit Zone! Limit: {speedLimit} units/sec. Speeding will cause consequence.");
            popupController.instance.ShowPopup("Had laju di jalan ini ialah 90km/h. Tolong ikut had laju!");
            // Inform the CarController that it's in a speed limit zone and pass the limit
            playerCar.SetSpeedLimitZoneStatus(true, speedLimit);
            // Optional: You could show a "SPEED LIMIT X" UI overlay here
        }
        else
        {
            Debug.Log("Uhhh");
        }

    }



    void OnTriggerExit(Collider other)
    {
        // Check if the exiting collider is the player car
        
            PrometeoCarController playerCar = other.GetComponentInParent<PrometeoCarController>();
            if (playerCar != null)
            {
                Debug.Log("Player exited Speed Limit Zone. No longer subject to this limit.");
                // Inform the CarController that it's no longer in a speed limit zone
                playerCar.SetSpeedLimitZoneStatus(false);

            if (playerCar.isPlayerSpeeding() == false)
            {
                Debug.Log("Player obeyed speed limit. Add reputation.");
                PointCounter.instance.ruleFollowed();
            }
        }
        else
        {
            Debug.Log("speeding tak detect!!");
        }
        
    }
}