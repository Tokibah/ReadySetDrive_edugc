using System.Collections;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;

public class bumperTrigger : MonoBehaviour
{
    public float speedLimit = 20f;
    public Rigidbody rb;
    //private bool recorded = false;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (CompareTag("BumperLarge"))
            {
                

                if (rb != null)
                {
                    float speed = rb.linearVelocity.magnitude;
                    if (speed < speedLimit)
                    {
                        Debug.Log("bumper:uhhh speed rendah?");
                        PointCounter.instance.ruleFollowed();

                    }
                    else
                    {
                        Debug.Log("bumper:Approached the bumper too fast!");
                        popupController.instance.ShowPopup("Jangan bawa laju-laju di bonggol! Tolak reputasi!");
                        CarHealth.instance.minusHealth(10);
                        PointCounter.instance.ruleBroken();
                    }

                    //recorded = true;
                    //StartCoroutine(resetTime());

                }
                else
                {
                    Debug.Log("bumper:uhhh ");
                }
            }
            else if (CompareTag("BumperSmall"))
            {
                Debug.Log("bumper:small bumper hit.");
                CarHealth.instance.minusHealth(2);
            }
        }
        else
        {
            Debug.Log("bumper:i dont detect the player.");
        }
        
    }

    IEnumerator resetTime()
    {
        yield return new WaitForSeconds(10f);
        //recorded = false;
    }

}
    
