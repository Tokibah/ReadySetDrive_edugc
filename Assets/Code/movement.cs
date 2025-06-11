using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class movement : MonoBehaviour
{
    public Rigidbody characterRigidBody;
    public float moveSpeed = 5f;
    //public float jumpForce = 5f;
    private Vector2 moveInput;
    public Transform orientation;

    //public LayerMask groundLayer;
    //public Transform groundCheck;
    //public float groundCheckRadius = 0.2f;
    //private bool isGrounded;

    

    private void Update()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");

        Vector3 moveDirection = orientation.forward * moveInput.y + orientation.right * moveInput.x;

        characterRigidBody.MovePosition(characterRigidBody.position + moveDirection * moveSpeed * Time.deltaTime);


        //isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        //if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        //{
        //    characterRigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        //}

    }
}
