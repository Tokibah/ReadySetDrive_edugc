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
        //RaycastHit hit;
        RaycastHit[] hits = Physics.RaycastAll(ray,interactDistance, interactLayer);
        RaycastHit closestHit = default;
        float closestDist = Mathf.Infinity;
        bool foundInspectable = false;

        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag("Inspectable"))
            {
                float dist = Vector3.Distance(Camera.main.transform.position, hit.point);
                if (dist < closestDist)
                {
                    closestDist = dist; ;
                    closestHit = hit;
                    foundInspectable = true;
                }
            }
        }

        if (foundInspectable)
        {
            hoverTextUI.text = closestHit.collider.name + ", Press E to inspect.";  
            hoverTextUI.enabled = true;
            UI_Background.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                var inspectable = closestHit.collider.GetComponent<IInspectable>();
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

        

        // Hide when not looking at anything interactable
        hoverTextUI.enabled = false;
        UI_Background.SetActive(false);
    }
}

