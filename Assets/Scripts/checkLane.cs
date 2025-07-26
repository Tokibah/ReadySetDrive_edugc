using UnityEngine;

public class checkLane : MonoBehaviour
{
    public Transform roundaboutCenter; // assign in Inspector
    public float allowedAngle = 90f; // threshold before considering wrong direction
    private bool alreadyTriggered = false;
    private bool rightWay = true,wrongWay = false;
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

            Debug.Log(angle);

            if (angle > allowedAngle)
            {
                Debug.Log("Higher than allowed: " + angle);
                if (alreadyTriggered) return;
                wrongWay = true;
                Debug.Log("WRONG WAY IN ROUNDABOUT!");
                alreadyTriggered = true;

                
            }
            else
            {
                rightWay = true;
            }
        }
        else
        {
            Debug.Log("uhh no rb detected");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (rightWay && !wrongWay)
        {
            Debug.Log("Car followed the right roundabout way.");
            PointCounter.instance.ruleFollowed();
        }
        else
        {
            popupController.instance.ShowPopup("Jangan lawan arah dalam roundabout! Tolak reputasi!");
            PointCounter.instance.ruleBroken();
        }
    }

  
}
