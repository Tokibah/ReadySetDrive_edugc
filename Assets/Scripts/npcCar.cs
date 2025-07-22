using UnityEngine;

public class npcCar : MonoBehaviour
{
    public float speed = 80f; // car forward speed
    public GameObject summaryUI;

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
       
        if (collision.gameObject.CompareTag("Player"))
        {
            popupController.instance.ShowPopup("Kereta berlanggar! Ulang sekali lagi!");
            Time.timeScale = 0f;
            PointCounter.instance.accident();
            PointCounter.instance.levelSummary();

        }
    }



}
