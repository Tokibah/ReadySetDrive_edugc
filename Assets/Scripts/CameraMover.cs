using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [Header("Target Y Position")]
    public float targetY = 5f;         // Final Y position (bottom view)
    public float moveSpeed = 5f;       // Movement speed

    private bool shouldMove = false;

    public void StartCameraMove()
    {
        shouldMove = true;
    }

    void Update()
    {
        if (shouldMove)
        {
            Vector3 currentPos = transform.position;
            Vector3 targetPos = new Vector3(currentPos.x, targetY, currentPos.z);

            transform.position = Vector3.MoveTowards(
                currentPos,
                targetPos,
                moveSpeed * Time.deltaTime
            );

            if (Mathf.Abs(transform.position.y - targetY) < 0.01f)
            {
                transform.position = targetPos;
                shouldMove = false;
            }
        }
    }
}
