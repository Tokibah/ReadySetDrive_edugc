using UnityEngine;
using UnityEngine.SceneManagement;

public class teleportChapter1 : MonoBehaviour
{
    public GameObject popup;
    private void OnTriggerStay(Collider other)
    {
        popup.SetActive(true);
        if (Input.GetKeyDown(KeyCode.E)){
            SceneManager.LoadScene("Chapter 1");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        popup.SetActive(false);
    }
}
