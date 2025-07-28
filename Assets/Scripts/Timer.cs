using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private int time = 60;
    private bool timeStart = false;
    public Text timeText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            timeStart = true;
        }
    }
    void Update()
    {
        if (timeStart)
        {
            time -= 1;
            timeText.text = time.ToString();
        }
    }
}
