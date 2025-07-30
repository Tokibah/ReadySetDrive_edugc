using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class LoadScene : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject ui, logo;
    public int level;
    
    public void startGame()
    {
        ui.SetActive(false);
        logo.SetActive(false);
        videoPlayer.loopPointReached += onVideoEnd;
        videoPlayer.Play();
        //SceneManager.LoadSceneAsync(1);
    }

    void onVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(level);
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
