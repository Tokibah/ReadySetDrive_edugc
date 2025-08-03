using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private float time = 60f;
    private bool timeStart = false;
    public GameObject timeObject;
    public Text timeText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && LevelManager.instance != null)
        {
            // ? Only trigger timer if current level is 4 (because your LevelManager uses 0-based indexing)
            if (LevelManager.instance.currentLevel == 4)
            {
                timeObject.SetActive(true);
                timeStart = true;
            }
        }
    }

    void Update()
    {
        if (timeStart)
        {
            time -= Time.deltaTime;
            timeText.text = Mathf.CeilToInt(time).ToString();
            if (time <= 0)
            {
                endTime();
            }
        }
    }

    void endTime()
    {
        timeObject.SetActive(false);
        timeStart = false;
    }
}
