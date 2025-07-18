using UnityEngine;
using UnityEngine.UI;

public class Interaction : MonoBehaviour
{
    public float interactDistance = 3f;
    public LayerMask interactLayer;

    public Text hoverTextUI; // Assign this in Inspector
    public GameObject UI_Background;

    void Update()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance, interactLayer))
        {
            if (hit.collider.CompareTag("Inspectable"))
            {
                hoverTextUI.text = hit.collider.name + ", Press E to inspect."; // or custom label
                hoverTextUI.enabled = true;
                UI_Background.SetActive(true);

              


                if (Input.GetKeyDown(KeyCode.E))
                {
                    var inspectable = hit.collider.GetComponent<IInspectable>();
                    if (inspectable != null)
                    {
                        StartCoroutine(inspectable.Inspect());
                    }
                    else
                    {
                        Debug.Log("uhhh");
                    }

                }

                

                return;
            }
        }

        // Hide when not looking at anything interactable
        hoverTextUI.enabled = false;
        UI_Background.SetActive(false);
    }
}

