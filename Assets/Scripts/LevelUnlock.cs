using UnityEngine;

public class LevelUnlock : MonoBehaviour
{
    public GameObject lockUI; // Optional: something to show it's locked
    public GameObject levelEntrance; // The trigger or portal

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
