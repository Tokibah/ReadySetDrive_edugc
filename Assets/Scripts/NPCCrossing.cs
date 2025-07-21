// Example script for an individual NPC
using UnityEngine;
using UnityEngine.UIElements;

public class NPCCrossing : MonoBehaviour
{
    public Transform targetCrossingPoint; // Assign the point on the other side of the road
    public float moveSpeed = 2f;

    private bool isCrossing = false;
    private Vector3 initialPosition;

    public GameObject summaryUI;

    private void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        if (isCrossing)
        {
            // Move towards the target crossing point
            transform.position = Vector3.MoveTowards(transform.position, targetCrossingPoint.position, moveSpeed * Time.deltaTime);

            // Optional: Rotate to face the target
            Vector3 direction = (targetCrossingPoint.position - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }

            // Check if arrived at destination
            if (Vector3.Distance(transform.position, targetCrossingPoint.position) < 0.1f)
            {
                isCrossing = false;
                Debug.Log(gameObject.name + " finished crossing.");
                // Potentially signal back to the manager or trigger script
                transform.position = initialPosition;
            }

        }

        
    }

    public void StartCrossing()
    {
        isCrossing = true;
        Debug.Log(gameObject.name + " started crossing.");
        // Trigger walking animation here
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isCrossing = true;
            Time.timeScale = 0f;
            PointCounter.instance.accident();
            PointCounter.instance.levelSummary();
            
        }
    }
}
    