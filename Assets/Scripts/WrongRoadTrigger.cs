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
        Vector3 playerDirection = body.linearVelocity.normalized;
        Vector3 laneDirection = transform.right;

        float angle = Vector3.Angle(laneDirection, playerDirection);

        if (angle > angleTolerance)
        {
            //triggerEvent();
            popupController.instance.ShowPopup("Salah masuk jalan! Ini jalan sehala!"); ;
            PointCounter.instance.ruleBroken();
        }

        

        //void triggerEvent()
        //{
        //    Debug.Log("Player entered wrong road! Activating wrong way car.");
        //    eventTriggered = true;

        //    Transform playertransform = other.transform;

        //    if (wrongWayCar != null)
        //    {
        //        wrongWayCar.SetActive(true);

        //        WrongWayCarMovement script = wrongWayCar.GetComponent<WrongWayCarMovement>();
        //        if (script != null)
        //        {
        //            script.SetTargetAndSpeed(playertransform, wrongWayCarSpeed);
        //        }
        //    }
        //    else
        //    {
        //        Debug.LogWarning("WrongRoadTrigger: No wrong way car assigned! Please assign a GameObject in the Inspector.", this);
        //    }
        //}
        


    }
}
    

