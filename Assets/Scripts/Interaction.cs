using UnityEngine;
using UnityEngine.UI;

public class Interaction : MonoBehaviour
{
    public float interactDistance = 3f;
    public LayerMask interactLayer;

    public Text hoverTextUI; // Assign this in Inspector

    void Update()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance, interactLayer))
        {
            if (hit.collider.CompareTag("Inspectable"))
            {
                hoverTextUI.text = hit.collider.name; // or custom label
                hoverTextUI.enabled = true;

                if (Input.GetMouseButtonDown(0))
                {
                    hit.collider.GetComponent<IInspectable>()?.Inspect();
                }

                return;
            }
        }

        // Hide when not looking at anything interactable
        hoverTextUI.enabled = false;
    }
}

