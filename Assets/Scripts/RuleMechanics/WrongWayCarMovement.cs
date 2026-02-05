// WrongWayCarAI.cs
using UnityEngine;

public class WrongWayCarMovement : MonoBehaviour
{
    public Transform targetPlayer; // The player's transform
    private float currentMoveSpeed; // Speed for this AI car
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // Start as kinematic so it doesn't move before activated
        }
    }

    // This method is called by the WrongRoadTrigger to activate and set parameters
    public void SetTargetAndSpeed(Transform player, float speed)
    {
        targetPlayer = player;
        currentMoveSpeed = speed;
        if (rb != null)
        {
            rb.isKinematic = false; // Allow physics to move it now
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; // Freeze X and Z rotation
        }
    }

    void FixedUpdate()
    {
        if (targetPlayer != null && currentMoveSpeed > 0)
        {
            // Calculate direction towards the player
            Vector3 directionToPlayer = (targetPlayer.position - transform.position).normalized;

            // Ensure movement is primarily on the horizontal plane (optional, but good for car AI)
            directionToPlayer.y = 0f;
            directionToPlayer.Normalize(); // Re-normalize after setting Y to 0

            // Move the car using Rigidbody.MovePosition for smooth physics interaction
            rb.MovePosition(rb.position + directionToPlayer * currentMoveSpeed * Time.fixedDeltaTime);

            // Optional: Make the AI car look at the player (or its movement direction)
            if (directionToPlayer != Vector3.zero) // Avoid looking at zero vector
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 5f); // Smooth rotation
            }
        }
    }

    // Handle collision with the player
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            popupController.instance.ShowPopup("Kereta berlanggar! Ulang sekali lagi!");
            Time.timeScale = 0f;
            PointCounter.instance.accident();
            PointCounter.instance.levelSummary();

        }
    }
}