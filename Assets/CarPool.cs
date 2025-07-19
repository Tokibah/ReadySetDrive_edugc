using UnityEngine;
using System.Collections.Generic;

public class CarPool : MonoBehaviour
{
    public GameObject carPrefab;
    public int poolSize = 10;
    private List<GameObject> pool = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(carPrefab);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetCar()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        // Optional: expand pool if needed
        GameObject newCar = Instantiate(carPrefab);
        newCar.SetActive(true);
        pool.Add(newCar);
        return newCar;
    }
}
