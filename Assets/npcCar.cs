using UnityEngine;

public class npcCar : MonoBehaviour
{
    public float speed = 80f; // car forward speed
    public GameObject summaryUI;

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DespawnZone"))
        {
            // Option 1: Disable the car
            gameObject.SetActive(false);

        }

        if (other.CompareTag("Player"))
        {
            Time.timeScale = 0f;
            PointCounter.instance.accident();
            PointCounter.instance.levelSummary();

        }
    }



}
