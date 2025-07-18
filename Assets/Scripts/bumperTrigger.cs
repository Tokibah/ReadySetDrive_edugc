using UnityEngine;
using UnityEngine.Rendering;

public class bumperTrigger : MonoBehaviour
{
    public float speedLimit = 20f;
    public Rigidbody rb;
    private bool recorded = false;


    private void OnTriggerEnter(Collider other)
    {
        if (recorded) return;

        if (rb != null)
        {
            float speed = rb.linearVelocity.magnitude;
            if (speed > speedLimit)
            {
                Debug.Log("Approached the bumper too fast!");
                PointCounter.instance.ruleBroken();
            }
            else
            {
                Debug.Log("uhhh speed rendah?");
                PointCounter.instance.ruleFollowed();
            }

            recorded = true;

        }
        else
        {
            Debug.Log("uhhh ");
        }
    }

}
    
