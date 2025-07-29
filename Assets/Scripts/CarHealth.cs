using UnityEngine;
using UnityEngine.UI;

public class CarHealth : MonoBehaviour
{
    public static CarHealth instance;
    private int health = 100;
    public Text healthCount;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        healthCount.text = health.ToString();

        if (health <= 0)
        {
            Time.timeScale = 0f;
            PointCounter.instance.accident();
            PointCounter.instance.levelSummary();
        }
    }

    public void minusHealth(int damage)
    {
        health -= damage;
    }

}
