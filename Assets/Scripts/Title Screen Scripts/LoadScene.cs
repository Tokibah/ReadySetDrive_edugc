using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void startGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    //public void quitGame()
    //{
    //    Application.Quit();
    //}

    //public void OpenLevel(int levelId)
    //{
    //    string levelName = "Level" + levelId;
    //    SceneManager.LoadSceneAsync(levelName);
    //}
}
