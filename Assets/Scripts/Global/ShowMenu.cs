using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ShowMenu : MonoBehaviour
{
    public GameObject menu;
    public GameObject guide;
    public GameObject canvas1;
    public GameObject canvas2;
    public GameObject loadingScreen;
    private void OnTriggerStay(Collider other)
    {
        guide.SetActive(true);
        if (Input.GetKeyDown(KeyCode.E))
        {
            //menu.SetActive(true);
            Time.timeScale = 0;
            canvas1.SetActive(false);
            canvas2.SetActive(false);
            loadingScreen.SetActive(true);
            SceneManager.LoadScene(4);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        guide.SetActive(false);
    }

    


}
