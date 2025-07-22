using System.Collections;
using UnityEngine;

public class intersectionStopTrigger : MonoBehaviour
{


    public float waitDuration = 4f;
    private Coroutine waitCoroutine;
    private bool waited = false;
    private bool isPlayerInside = false;

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(StopAndWait());
    }

    IEnumerator StopAndWait()
    {
        Debug.Log("Please wait at intersection");
        waited = false;
        yield return new WaitForSeconds(waitDuration);
        Debug.Log("Good to go.");
        waited = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!waited)
        {
            Debug.Log("Left too early, reduce reputation!");
            popupController.instance.ShowPopup("Awak sepatutnya berhenti dahulu! Bahaya keluar macam itu!");
            PointCounter.instance.ruleBroken(); 
        }
        else
        {
            Debug.Log("Add reputation.");
            popupController.instance.ShowPopup("Bagus, ini namanya pemanduan berhemah!");
            PointCounter.instance.ruleFollowed();
        }
    }


}
