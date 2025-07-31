using UnityEngine;

/// <summary>
/// Spawner.cs
/// This script handles spawning a specified GameObject prefab at a given position.
/// It can be configured to spawn on a button press or continuously over time.
/// </summary>
public class Spawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [Tooltip("The prefab GameObject to be spawned.")]
    public GameObject objectToSpawn;

    [Tooltip("The transform representing where the object will be spawned. " +
             "If null, the spawner's own position will be used as a base.")]
    public Transform spawnPoint;

    [Header("Spawn Area (Relative to Spawn Point/Spawner Position)")]
    [Tooltip("The minimum X, Y, Z coordinates for spawning. " +
             "These values are added to the spawnPoint's position.")]
    public Vector3 minSpawnOffset = new Vector3(-30f, 0f, 4f); // Adjusted for user's request
    [Tooltip("The maximum X, Y, Z coordinates for spawning. " +
             "These values are added to the spawnPoint's position.")]
    public Vector3 maxSpawnOffset = new Vector3(-10f, 0f, 33f); // Adjusted for user's request

    [Header("Rotation Settings")]
    [Tooltip("If true, the spawned object will have a random rotation around all axes.")]
    public bool randomRotation = true;

    [Header("Continuous Spawning")]
    [Tooltip("If true, the spawner will continuously spawn objects.")]
    public bool continuousSpawning = false;

    [Tooltip("The interval in seconds between continuous spawns.")]
    [Range(0.1f, 10f)] // Restrict to reasonable values
    public float spawnInterval = 2f;

    private float nextSpawnTime;

    void Start()
    {
        // Initialize the next spawn time if continuous spawning is enabled
        if (continuousSpawning)
        {
            nextSpawnTime = Time.time + spawnInterval;
        }

        // Basic validation: ensure a prefab is assigned
        if (objectToSpawn == null)
        {
            Debug.LogError("Spawner: 'Object To Spawn' is not assigned! Please assign a prefab in the Inspector.", this);
            enabled = false; // Disable the script if no prefab is set
        }
    }

    void Update()
    {
        // Handle continuous spawning
        if (continuousSpawning && Time.time >= nextSpawnTime)
        {
            SpawnObject();
            nextSpawnTime = Time.time + spawnInterval; // Set next spawn time
        }

        // Example: You could also trigger spawning with a key press for testing
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     SpawnObject();
        // }
    }

    /// <summary>
    /// Spawns an instance of the 'objectToSpawn' prefab.
    /// The position will be randomized within the defined min/max offsets
    /// relative to the spawner's position or the assigned spawnPoint.
    /// Rotation will be random if 'randomRotation' is true.
    /// </summary>
    public void SpawnObject()
    {
        if (objectToSpawn == null)
        {
            Debug.LogWarning("Cannot spawn: 'Object To Spawn' is not assigned.", this);
            return;
        }

        // Determine the base position (spawner's position or assigned spawnPoint's position)
        Vector3 basePosition = (spawnPoint != null) ? spawnPoint.position : transform.position;

        // Calculate a random position within the defined offsets
        float randomX = Random.Range(minSpawnOffset.x, maxSpawnOffset.x);
        float randomY = Random.Range(minSpawnOffset.y, maxSpawnOffset.y);
        float randomZ = Random.Range(minSpawnOffset.z, maxSpawnOffset.z);

        Vector3 finalSpawnPosition = basePosition + new Vector3(randomX, randomY, randomZ);

        // Determine the rotation
        Quaternion finalSpawnRotation = randomRotation ? Random.rotation : (spawnPoint != null ? spawnPoint.rotation : transform.rotation);

        // Instantiate the prefab at the determined position and rotation
        GameObject spawnedObject = Instantiate(objectToSpawn, finalSpawnPosition, finalSpawnRotation);
        Debug.Log($"Spawned: {spawnedObject.name} at {spawnedObject.transform.position} with rotation {spawnedObject.transform.rotation.eulerAngles}", spawnedObject);
    }
}
