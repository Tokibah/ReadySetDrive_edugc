using UnityEngine;

public class CapsuleVanLooper : MonoBehaviour
{
    public float speed = 5f;
    public float loopStartX = 10f;
    public float loopEndX = -10f;

    private Vector3 startPosition;
    private float baseY;

    void Start()
    {
        startPosition = transform.position;
        baseY = startPosition.y;
    }

    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        
        float bob = Mathf.Sin(Time.time * 2f) * 0.2f;
        transform.position = new Vector3(transform.position.x, baseY + bob, startPosition.z);

        if (transform.position.x < loopEndX)
        {
            transform.position = new Vector3(loopStartX, baseY, startPosition.z);
        }
    }
}
