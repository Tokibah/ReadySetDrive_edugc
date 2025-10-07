using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public static Timer instance;
    private float time = 120f;
    private bool timeStart = false;
    public GameObject timeObject;
    public Text timeText;
    public static float initTime = 120f;
    public bool timeEnd = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
    public void ResetTimer()
    {
        if (LevelManager.instance.currentLevel == 4 || LevelManager.instance.currentLevel == 5)
        {
            time = 120f;
            timeStart = false;
            timeEnd = false;
            timeObject.SetActive(false);
            timeText.text = Mathf.Ceil(time).ToString();
        }
            
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && LevelManager.instance != null)
        {
            // ? Only trigger timer if current level is 4 (because your LevelManager uses 0-based indexing)
            if (LevelManager.instance.currentLevel == 4 || LevelManager.instance.currentLevel == 5)
            {
                time = initTime;
                timeEnd = false;
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
        timeEnd = true;
        PointCounter.instance.levelSummary();
        if(PointCounter.instance.collectedRep > 20)
        {
            PointCounter.instance.levelSuccess();
        }
        else
        {
            PointCounter.instance.levelFailed();
        }
    }
}
