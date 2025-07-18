using UnityEngine;

public class oppositeLane : MonoBehaviour
{
    public float angleTolerance = 90f; // degrees
    public Rigidbody body;

    private void OnTriggerEnter(Collider other)
    {
        
            if (body != null)
            {
                Vector3 playerDirection = body.linearVelocity.normalized;
                Vector3 laneDirection = transform.right;

                float angle = Vector3.Angle(laneDirection, playerDirection);

                if (angle > angleTolerance)
                {
                    Debug.Log("WRONG WAY! -10 points");
                }
            }
        
    }
}
