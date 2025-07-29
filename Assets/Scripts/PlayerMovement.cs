using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private CharacterController characterController;
    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float fastSpeed = 2.0f;

    // gravity
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private Transform groundCheck; // This variable is not currently used in the provided script, but kept for completeness.
    [SerializeField] private LayerMask groundMask; // This variable is not currently used in the provided script, but kept for completeness.
    private Vector3 velocity;
    private bool isGrounded; // This variable is not currently used in the provided script, but kept for completeness.

    // New variable to control player movement
    private bool canMove = true; 

    public float PlayerSpeed
    {
        get
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                return fastSpeed;
            }
            else return playerSpeed;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Ensure characterController is assigned
        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
            if (characterController == null)
            {
                Debug.LogError("PlayerMovement requires a CharacterController component!");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Apply horizontal movement only if canMove is true
        if (canMove)
        {
            Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            Vector3 move = new Vector3(input.x, 0, input.y);
            
            // Ensure Camera.main is not null before accessing its transform
            if (Camera.main != null)
            {
                Vector3 camForward = Camera.main.transform.forward;
                camForward.y = 0f; // Flatten the vector to the horizontal plane
                camForward = camForward.normalized;

                Vector3 camRight = Camera.main.transform.right;
                camRight.y = 0f; // Flatten the vector to the horizontal plane
                camRight = camRight.normalized;

                move = camForward * move.z + camRight * move.x;
            }
            else
            {
                Debug.LogWarning("Main Camera not found. Player movement might not be relative to camera.");
            }

            // Move the character controller
            if (characterController != null)
            {
                characterController.Move(move * Time.deltaTime * PlayerSpeed);
            }
        }

        // Always apply gravity, regardless of whether the player can move horizontally
        if (characterController != null)
        {
            velocity.y += gravity * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);

            // Check if grounded to reset vertical velocity (if you implement jumping later)
            // isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            // if (isGrounded && velocity.y < 0)
            // {
            //     velocity.y = -2f; // Small negative value to keep player grounded
            // }
        }
    }

    /// <summary>
    /// Sets whether the player can move horizontally. Gravity will still apply.
    /// </summary>
    /// <param name="state">True to enable movement, false to disable.</param>
    public void SetCanMove(bool state)
    {
        canMove = state;
        Debug.Log("Player movement set to: " + state);

        // If movement is disabled, stop any current horizontal velocity
        if (!canMove && characterController != null)
        {
            // Reset horizontal velocity to prevent sliding
            Vector3 currentVelocity = characterController.velocity;
            characterController.Move(new Vector3(-currentVelocity.x, 0, -currentVelocity.z)); // Counteract horizontal movement
        }
    }
}
