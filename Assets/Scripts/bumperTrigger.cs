using System.Collections;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;

public class bumperTrigger : MonoBehaviour
{
    public float speedLimit = 20f;
    public Rigidbody rb;
    private bool recorded = false;


    private void OnTriggerEnter(Collider other)
    {
        if (CompareTag("BumperLarge"))
        {
            if (recorded) return;

            if (rb != null)
            {
                float speed = rb.linearVelocity.magnitude;
                if (speed < speedLimit)
                {
                    Debug.Log("uhhh speed rendah?");
                    PointCounter.instance.ruleFollowed();

                }
                else
                {
                    Debug.Log("Approached the bumper too fast!");
                    popupController.instance.ShowPopup("Jangan bawa laju-laju di bonggol! Tolak reputasi!");
                    PointCounter.instance.ruleBroken();
                }

                recorded = true;
                StartCoroutine(resetTime());

            }
            else
            {
                Debug.Log("uhhh ");
            }
        }
        else
        {
            Debug.Log("small bumper hit.");
        }
        
    }

    IEnumerator resetTime()
    {
        yield return new WaitForSeconds(10f);
        recorded = false;
    }

}
    
