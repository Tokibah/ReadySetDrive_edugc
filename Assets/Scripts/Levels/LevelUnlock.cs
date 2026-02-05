using UnityEngine;

public class LevelUnlock : MonoBehaviour
{
    public static void UnlockLevel(int levelIndex)
    {
        // Example: unlock level 2 by calling UnlockLevel(2)
        PlayerPrefs.SetInt("Level_" + levelIndex, 1);
        Debug.Log("level " + levelIndex + " unlocked");
        PlayerPrefs.Save();
    }

    public static bool IsLevelUnlocked(int levelIndex)
    {
        if (levelIndex == 1) return true; // Level 1 is always unlocked
        return PlayerPrefs.GetInt("Level_" + levelIndex, 0) == 1;
    }

    public static void SaveLatestLevel(int latest)
    {
        PlayerPrefs.SetInt("LatestLevel", latest);
    }

    public static void ResetAllProgress()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
