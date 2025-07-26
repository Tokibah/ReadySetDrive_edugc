using UnityEngine;
using UnityEngine.SceneManagement;

public class teleportChapter : MonoBehaviour
{
    public GameObject menu;
    public void chapter1()
    {
        SceneManager.LoadScene("Chapter 1");
    }

    public void chapter2()
    {
        SceneManager.LoadScene("Chapter 2");
    }

    public void chapter3()
    {
        SceneManager.LoadScene("Chapter 3");
    }

    public void hideMenu()
    {
        Time.timeScale = 1;
        menu.SetActive(false);
    }

}
