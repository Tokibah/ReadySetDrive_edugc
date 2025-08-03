using UnityEngine;
using UnityEngine.UI;

public class MissionNavigator : MonoBehaviour
{
    public Transform[] objectives; 
    public Image arrowUI;
    public Transform player;
    public float arrivalDistance = 5f;

    private int currentIndex = 0;

    void Update()
    {
        if (currentIndex >= objectives.Length) return;

        Transform target = objectives[currentIndex];
        Vector3 direction = target.position - player.position;

        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        arrowUI.rectTransform.rotation = Quaternion.Euler(0, 0, -angle);

        if (direction.magnitude <= arrivalDistance)
        {
            currentIndex++;
            Debug.Log("Objective reached!");
        }
    }
}