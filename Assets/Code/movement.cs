using UnityEngine;

public class movement : MonoBehaviour
{
    public Rigidbody characterRigidBody;
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    private Vector2 moveInput;

    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    private bool isGrounded;

    

    private void Update()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        moveInput.Normalize();

        Vector3 moveVelocity = new Vector3(moveInput.x * moveSpeed, characterRigidBody.linearVelocity.y, moveInput.y * moveSpeed);
        characterRigidBody.linearVelocity = moveVelocity;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            characterRigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

    }
}
