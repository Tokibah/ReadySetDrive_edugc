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
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;
    private Vector3 velocity;
    private bool isGrounded;


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
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector3 move = new Vector3(input.x, 0, input.y);
        Vector3 camForward = Camera.main.transform.forward;
        camForward = camForward.normalized;
        Vector3 camRight = Camera.main.transform.right;
        camRight.y = 0f;
        camForward.y = 0f;
        move = camForward * move.z + camRight * move.x;
        characterController.Move(move *Time.deltaTime * PlayerSpeed);

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);


    }
}
