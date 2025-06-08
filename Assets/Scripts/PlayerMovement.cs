using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody rb;

    public Transform orientation; // Assign to camera or a separate child that follows horizontal look direction

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = orientation.forward * vertical + orientation.right * horizontal;
        direction.y = 0f; // Don't move up/down

        rb.MovePosition(rb.position + direction.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}

