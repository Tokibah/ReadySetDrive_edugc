using UnityEngine;

public class messagePopup : MonoBehaviour
{
    public GameObject popup;
    public GameObject dialogue;
    public bool playerInRange = false;

    

    private void OnTriggerEnter(Collider other)
    {
        popup.SetActive(true);
        playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        popup.SetActive(false);
        dialogue.SetActive(false);
        playerInRange = false;
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            dialogue.SetActive(true);
        }
    }
}
   
