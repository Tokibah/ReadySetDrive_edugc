using UnityEngine;

public class dividerHit : MonoBehaviour
{
    Rigidbody rb;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit the divider!");
        }
    }

    
}
