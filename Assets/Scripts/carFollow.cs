// CameraFollowCar.cs
using UnityEngine;

public class carFollow : MonoBehaviour
{
    [Header("Target Settings")]
    [Tooltip("The GameObject (car) this camera will follow.")]
    public Transform target; // Assign your car's Transform here.

    [Tooltip("The offset from the target's LOCAL position. E.g., (0, 5, -10) for behind and above.")]
    public Vector3 offset = new Vector3(0f, 5f, -10f); // Adjust these values in the Inspector.
                                                       // A negative Z-value places the camera behind the car.

    [Header("Smoothing Settings")]
    [Range(0.01f, 1.0f)]
    [Tooltip("How smoothly the camera moves towards the target's desired position. Lower values are smoother.")]
    public float positionSmoothSpeed = 0.125f; // Controls positional lag.

    [Range(0.01f, 1.0f)]
    [Tooltip("How smoothly the camera rotates to match the target's rotation. Lower values are smoother.")]
    public float rotationSmoothSpeed = 0.125f; // Controls rotational lag.

    [Header("Look At Options")]
    [Tooltip("If true, the camera will always attempt to look directly at the target.")]
    public bool lookAtTarget = false; // Generally set to false if you're using rotation smoothing.

    [Tooltip("If 'Look At Target' is false, this controls how far ahead of the car the camera looks.")]
    public float lookAheadDistance = 5f; // How many units ahead of the car the camera's focus point is.

    void LateUpdate()
    {
        // Ensure a target is assigned before trying to follow it
        if (target == null)
        {
            Debug.LogWarning("CameraFollowCar: No target (car) assigned! Please assign a target GameObject in the Inspector.", this);
            return; // Stop execution if there's no target
        }

        // --- 1. Calculate Desired Position ---
        // target.TransformPoint(offset) converts the 'offset' (which is in the car's local space)
        // into a world position. This means the camera will always maintain the same position
        // relative to the car's orientation (e.g., always 10 units behind it, even when turning).
        Vector3 desiredPosition = target.TransformPoint(offset);

        // --- 2. Smoothly Move Camera to Desired Position ---
        transform.position = Vector3.Lerp(transform.position, desiredPosition, positionSmoothSpeed);

        // --- 3. Calculate Desired Rotation ---
        Quaternion desiredRotation;
        if (lookAtTarget)
        {
            // Option A: Look directly at the target's position
            desiredRotation = Quaternion.LookRotation(target.position - transform.position);
        }
        else
        {
            // Option B: Smoothly follow the target's rotation, potentially looking slightly ahead
            // This is often preferred for a car camera.
            // First, get the desired orientation of the camera based on the car's orientation
            desiredRotation = target.rotation;

            // Then, if you want the camera to also look slightly ahead of the car,
            // you can combine this with a LookAt.
            // We'll let Slerp handle the primary rotation, and optionally use LookAt for fine-tuning.
            // For a pure smooth follow that rotates with the car:
            // desiredRotation = target.rotation;
        }

        // --- 4. Smoothly Rotate Camera to Desired Rotation ---
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSmoothSpeed);

        // --- 5. Optional: Refine Camera's Look Direction (if not directly looking at target) ---
        // If lookAtTarget is false, the Slerp aligns the camera's overall orientation with the car.
        // You might still want it to pivot slightly to look ahead of the car for better road visibility.
        if (!lookAtTarget && target != null)
        {
            // Calculate a point slightly in front of the car to look at
            Vector3 lookPoint = target.position + target.forward * lookAheadDistance;
            transform.LookAt(lookPoint);
        }
    }
}