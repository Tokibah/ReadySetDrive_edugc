using UnityEngine;

public class MaterialUnlock : MonoBehaviour
{
    public GameObject[] npcObjects;

    void Start()
    {
        int latestLevel = PlayerPrefs.GetInt("LatestLevel",0); // default to 0 if not found

        for (int i = 0; i < npcObjects.Length; i++)
        {
            if (i < latestLevel)
                npcObjects[i].SetActive(true);
            else
                npcObjects[i].SetActive(false);
        }
    }
}
