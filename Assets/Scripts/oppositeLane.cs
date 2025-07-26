using UnityEngine;

public class oppositeLane : MonoBehaviour
{
    public float angleTolerance = 90f; // degrees
    public Rigidbody body;

    private void OnTriggerStay(Collider other)
    {
        
            if (body != null)
            {
                Vector3 playerDirection = body.linearVelocity.normalized;
                Vector3 laneDirection = transform.right;

                float angle = Vector3.Angle(laneDirection, playerDirection);
            Debug.Log("Allowed: " + angle);


            if (angle > angleTolerance)
                {
                Debug.Log("Not allowed: " + angle);
                    Debug.Log("WRONG WAY! -10 points");
                    popupController.instance.ShowPopup("Salah jalan ni! Ikut belah kiri jalan!");

            }

        }
        
    }
}
