using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [Header("Target Position")]
    public Vector3 targetPosition = new Vector3(0, 5f, 0); 
    public float moveSpeed = 5f;

    private bool shouldMove = false;

    public void StartCameraMove()
    {
        shouldMove = true;
    }

    void Update()
    {
        if (shouldMove)
        {
            // Move the camera towards the target position
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            // Check if the camera has arrived at the target position
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                // Snap the position to the exact target and stop moving
                transform.position = targetPosition;
                shouldMove = false;
            }
        }
    }
}