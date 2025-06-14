using UnityEngine;

public class TitleScreenAnimation : MonoBehaviour
{

    [Header("Road Rotation")]
    public Transform roadObject;
    public float roadRotationSpeed = 100f;

    [Header("Rail Rotation")]
    public Transform[] railObjects;
    public float railRotationSpeed = 100f;

    void Update()
    {
        
        if (roadObject != null)
        {
            roadObject.Rotate(Vector3.down * roadRotationSpeed * Time.deltaTime, Space.Self);
        }

        
        foreach (Transform rail in railObjects)
        {
            if (rail != null)
            {
                rail.Rotate(Vector3.forward * railRotationSpeed * Time.deltaTime, Space.Self);
            }
        }
    }
}

