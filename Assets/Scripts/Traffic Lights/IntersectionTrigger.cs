using UnityEngine;
// Ensure these namespaces are included if your popupController and PointCounter are in them
// using UnityEngine.Audio; // Not strictly needed for the logic here
// using UnityEngine.Rendering; // Not strictly needed for the logic here

public class IntersectionTrigger : MonoBehaviour
{
    // Reference to the central TrafficLightManager for THIS intersection.
    // Drag the GameObject with the TrafficLightManager script here in the Inspector.
    public TrafficLightManager trafficLightManager;

    // This trigger corresponds to a specific approach direction for the player.
    // For example, if this trigger is for cars entering the intersection from the North.
    public TrafficLightManager.ApproachDirection triggerApproachDirection;

    // Reference to the Player's Rigidbody. You can either assign this manually,
    // or (better) find it dynamically when the player enters the trigger.
    [Tooltip("Assign the Player's Rigidbody here. If left null, it will try to find it on trigger enter.")]
    public Rigidbody playerRigidbody;

    [Header("Angle Detection Settings")]
    [Tooltip("The acceptable angle difference from the trigger's forward direction for a straight-through movement.")]
    public float straightAngleThreshold = 20f; // e.g., 20 degrees off exact straight.
    [Tooltip("The acceptable angle difference from the trigger's right direction for a right-turn movement.")]
    public float rightTurnAngleThreshold = 20f; // e.g., 20 degrees off exact right.
    [Tooltip("The acceptable angle difference from the trigger's left direction for a left-turn movement.")]
    public float leftTurnAngleThreshold = 20f; // e.g., 20 degrees off exact left.


    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider entering the trigger is the player.
        // Make sure your player car has the tag "Player".
        if (other.CompareTag("Player"))
        {
            // If playerRigidbody is not assigned, try to get it from the entering collider.
            if (playerRigidbody == null)
            {
                playerRigidbody = other.GetComponent<Rigidbody>();
                if (playerRigidbody == null)
                {
                    Debug.LogWarning("IntersectionTrigger: Player Rigidbody not found on entering collider. Cannot check driving rules.", other.gameObject);
                    return;
                }
            }

            // Ensure the TrafficLightManager is assigned
            if (trafficLightManager == null)
            {
                Debug.LogError("IntersectionTrigger: TrafficLightManager is not assigned! Cannot check traffic light rules.", this);
                return;
            }

            // Get the current light state for this specific approach direction
            TrafficLightManager.LightState currentLightState = trafficLightManager.GetLightStateForApproach(triggerApproachDirection);

            // Determine player's movement direction relative to the trigger's forward direction.
            // transform.forward of the trigger should generally point in the "straight through" direction for this lane.
            Vector3 playerVelocityDirection = playerRigidbody.linearVelocity.normalized;
            Vector3 triggerForward = transform.forward; // This trigger's forward is the "straight" path.

            // Calculate angles for different maneuvers
            float angleToForward = Vector3.Angle(triggerForward, playerVelocityDirection);
            float angleToRight = Vector3.Angle(transform.right, playerVelocityDirection); // Assuming transform.right is the right turn direction
            float angleToLeft = Vector3.Angle(-transform.right, playerVelocityDirection); // Assuming -transform.right is the left turn direction


            Debug.Log($"Player entered trigger from {triggerApproachDirection}. Player Velocity Angle to Trigger Forward: {angleToForward}. To Right: {angleToRight}. To Left: {angleToLeft}. Light State: {currentLightState}");

            // --- Apply Driving Rules based on Light State ---

            // The common rule: Crossing on Red or Yellow is usually a rule break.
            if (currentLightState == TrafficLightManager.LightState.Red || currentLightState == TrafficLightManager.LightState.Yellow)
            {
                Debug.Log("Player crossing road during RED/YELLOW light!");
                // Assuming popupController and PointCounter are singletons (e.g., via a static 'instance' property)
                if (popupController.instance != null)
                {
                    popupController.instance.ShowPopup("Pemanduan berbahaya! Berhenti dahulu ketika lampu merah!");
                }
                if (PointCounter.instance != null)
                {
                    PointCounter.instance.ruleBroken();
                }
            }
            else if (currentLightState == TrafficLightManager.LightState.Green)
            {
                Debug.Log("Good to go. Player crossing on GREEN light.");
                if (popupController.instance != null)
                {
                    popupController.instance.ShowPopup("Bagus, pemanduan yang berhemah. Tambah reputasi!");
                }
                if (PointCounter.instance != null)
                {
                    PointCounter.instance.ruleFollowed();
                }
            }
            // You can add more nuanced logic here, e.g., if a right turn on red is allowed in your game's rules
            // You could use `isTurningRight` to check:
            // else if (currentLightState == TrafficLightManager.LightState.Red && isTurningRight && /* check if no oncoming traffic */) { /* Good for right turn on red */ }
        }
    }

    // Optional: Add a visual helper in the editor
    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        // Draw the trigger collider itself (if it's a BoxCollider or SphereCollider)
        Collider col = GetComponent<Collider>();
        if (col != null && col.isTrigger)
        {
            if (col is BoxCollider box)
            {
                // Apply the trigger's transform for accurate drawing of the box
                Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
                Gizmos.DrawWireCube(box.center, box.size);
                Gizmos.matrix = Matrix4x4.identity; // Reset matrix for subsequent Gizmo calls
            }
            else if (col is SphereCollider sphere)
            {
                // For sphere, local position + center, scaled by max of local scales
                // Simplified for visualization: uses world position + center offset
                Gizmos.DrawWireSphere(transform.position + sphere.center, sphere.radius * transform.lossyScale.x);
            }
        }

        // Draw the forward direction of the trigger (where the car should go straight)
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * 5f); // 5f is arbitrary length for visualization
        Gizmos.DrawSphere(transform.position + transform.forward * 5f, 0.5f); // Arrowhead

        // Draw right and left directions (for turn detection visualization)
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.right * 2f);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, -transform.right * 2f);
    }
}