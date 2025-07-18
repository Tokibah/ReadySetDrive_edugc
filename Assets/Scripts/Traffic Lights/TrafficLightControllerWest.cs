using System.Collections;
using UnityEngine;

public class TrafficLightControllerWest : MonoBehaviour
{
    public GameObject redLight;
    public GameObject yellowLight;
    public GameObject greenLight;

    public float redDuration = 4f;
    public float yellowDuration = 2f;
    public float greenDuration = 4f;

    public enum LightState { Red, Yellow, Green }
    public LightState currentState;

  

    private void Start()
    {
        StartCoroutine(TrafficLightCycle());
    }

    IEnumerator TrafficLightCycle()
    {
        while (true)
        {
            // Green
            SetLight(false, false, true);
            yield return new WaitForSeconds(greenDuration);

            // Yellow
            SetLight(false, true, false);
            yield return new WaitForSeconds(yellowDuration);

            // Red
            SetLight(true, false, false);
            yield return new WaitForSeconds(redDuration);

            
        }
    }

    void SetLight(bool redOn, bool yellowOn, bool greenOn)
    {
        redLight.SetActive(redOn);
        yellowLight.SetActive(yellowOn);
        greenLight.SetActive(greenOn);

        if (redOn) currentState = LightState.Red;
        else if (yellowOn) currentState = LightState.Yellow;
        else if (greenOn) currentState = LightState.Green;
    }
}
