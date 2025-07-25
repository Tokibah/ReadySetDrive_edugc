using UnityEngine;

public class ColliderHit : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Time.timeScale = 0;
            PointCounter.instance.accident();
            PointCounter.instance.levelSummary();
        }
    }
}
