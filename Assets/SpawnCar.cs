using UnityEngine;

public class SpawnCar : MonoBehaviour
{
    public Transform player;
    public CarPool pool;
    public Transform[] spawnPoints;     // Set spawn positions in Inspector
    public float spawnInterval = 2f;    // How often to spawn a car

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnNewCar();
            timer = 0f;
        }
    }

    void SpawnNewCar()
    {
        GameObject car = pool.GetCar();
        int index = Random.Range(0, spawnPoints.Length);
        float laneDifference = Mathf.Abs(player.position.x - spawnPoints[index].position.x);

        Debug.Log(laneDifference);
        if (laneDifference < 12f)
        {
            Debug.Log("Player in the lane " + index);
            return;
        }
        car.transform.position = spawnPoints[index].position;
        car.transform.rotation = spawnPoints[index].rotation;
    }


}
