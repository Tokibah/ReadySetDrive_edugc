using UnityEngine;
using UnityEngine.InputSystem;

public class ShowMenu : MonoBehaviour
{
    public GameObject menu;
    public GameObject guide;
    private void OnTriggerStay(Collider other)
    {
        guide.SetActive(true);
        if (Input.GetKeyDown(KeyCode.E))
        {
            menu.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        guide.SetActive(false);
    }

    


}
