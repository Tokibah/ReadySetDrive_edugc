using UnityEngine;

public class StartLevel : MonoBehaviour
{
    public Transform player;            // Drag your player prefab or object here
    public Collider spawnZoneCollider;  // Drag the spawn zone collider here

    void Start()
    {
        if (player != null && spawnZoneCollider != null)
        {
            // Get a random point inside the spawn zone's bounds
            Vector3 center = spawnZoneCollider.bounds.center;
            Vector3 size = spawnZoneCollider.bounds.size;

            Vector3 randomPosition = new Vector3(
                Random.Range(center.x - size.x / 2, center.x + size.x / 2),
                center.y,
                Random.Range(center.z - size.z / 2, center.z + size.z / 2)
            );

            // Move player to spawn position
            player.position = randomPosition;
        }
        else
        {
            Debug.LogWarning("Player or SpawnZone is not assigned.");
        }
    }
}
