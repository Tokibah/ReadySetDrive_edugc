using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class IntersectionTrigger : MonoBehaviour
{
    public enum Direction { North, South, West, East}
    public Direction direction;
    public TrafficLightControllerNorth trafficLightNorth;
    public TrafficLightControllerSouth trafficLightSouth;
    public TrafficLightControllerWest trafficLightWest;
    public Rigidbody body;


    private void OnTriggerEnter(Collider other)
    {

        Vector3 playerDirection = body.linearVelocity ;
        Vector3 laneDirection = transform.right;

        float angle = Vector3.Angle(laneDirection, playerDirection);

        Debug.Log(angle);
        if (angle > 90 && angle < 100)
        {
            if (trafficLightNorth.currentState == TrafficLightControllerNorth.LightState.Red)
            {
                Debug.Log("Player crossing road during red light!");
            }
            else
            {
                Debug.Log("Good to go.");
            }
        }
        else if (angle < 90)
        {
            if (trafficLightSouth.currentState == TrafficLightControllerSouth.LightState.Red)
            {
                Debug.Log("Player crossing road during red light!");
            }
            else
            {
                Debug.Log("Good to go.");
            }
        }
        else if (angle > 150 && angle < 190)
        {
            if (trafficLightWest.currentState == TrafficLightControllerWest.LightState.Red)
            {
                Debug.Log("Player crossing road during red light!");
            }
            else
            {
                Debug.Log("Good to go.");
            }
        }



    }

    
}
