using UnityEngine;

public class checkLane : MonoBehaviour
{
    public Transform roundaboutCenter; // assign in Inspector
    public float allowedAngle = 90f; // threshold before considering wrong direction
    private bool alreadyTriggered = false;
    public Rigidbody rb;

    private void OnTriggerStay(Collider other)
    {


        
        if (rb != null && rb.linearVelocity.magnitude > 0.1f)
        {
            Vector3 toPlayer = (other.transform.position - roundaboutCenter.position).normalized;
            Vector3 playerDirection = rb.linearVelocity.normalized;

            // Clockwise desired: tangent is -cross(toPlayer, up)
            Vector3 correctTangent = -Vector3.Cross(toPlayer, Vector3.up);

            float angle = Vector3.Angle(correctTangent, playerDirection);

            if (angle > allowedAngle)
            {
                Debug.Log("WRONG WAY IN ROUNDABOUT! -15 points");
                alreadyTriggered = true;
                Invoke(nameof(ResetTrigger), 2f);
            }
        }
        else
        {
            Debug.Log("uhh no rb detected");
        }
    }

    private void ResetTrigger()
    {
        alreadyTriggered = false;
    }
}
