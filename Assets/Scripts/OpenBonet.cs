using UnityEngine;

public class OpenBonet : MonoBehaviour, IInspectable
{
    public bool isOpen = false;
    public float openAngle = -70f;
    public float closedAngle = 0f;
    public float speed = 2f;

    private Quaternion targetRotation;
    private bool isMoving = false;

    void Start()
    {
        targetRotation = transform.localRotation;
    }

    void Update()
    {
        if (isMoving)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * speed);
        }
    }

    public void Inspect()
    {
        isOpen = !isOpen;
        float targetYAngle = isOpen ? openAngle : closedAngle;
        targetRotation = Quaternion.Euler(targetYAngle, 0f, 0f);
        isMoving = true;
    }
}


