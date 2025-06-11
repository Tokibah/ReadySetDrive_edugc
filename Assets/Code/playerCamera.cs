using UnityEngine;

public class playerCamera : MonoBehaviour
{
    public Transform player;
    public float mouseSensitivity = 2f;

    float cameraVerticalRotation = 0f;

    private void Start()
    {
        // lock and hide cursor
    }

    private void Update()
    {
        // collect mouse input
        float inputX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float inputY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // rotate the camera around its local X axis

        cameraVerticalRotation -= inputY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);

        transform.localEulerAngles = Vector3.right * cameraVerticalRotation;

        // rotate the player object and the camera around its y axis
        player.Rotate(Vector3.up * inputX);
    }
}
