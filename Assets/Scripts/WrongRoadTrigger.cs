using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;

public class WrongRoadTrigger : MonoBehaviour
{
    public GameObject wrongWayCar;
    public float wrongWayCarSpeed = 10f;
    
    public float angleTolerance = 90f; // degrees
    public Rigidbody body;
    [TextArea(10, 1000)]
    public string Comment = "Information Here.";
 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 playerDirection = body.linearVelocity.normalized;
            Vector3 laneDirection = transform.right;

            float angle = Vector3.Angle(laneDirection, playerDirection);

            if (angle > angleTolerance)
            {
                //triggerEvent();
                popupController.instance.ShowPopup("Salah masuk jalan! Ini jalan sehala!"); ;
                PointCounter.instance.ruleBroken();
            }
        }

        

      
        


    }
}
    

