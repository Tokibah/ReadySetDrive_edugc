using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TireInspect : MonoBehaviour, IInspectable
{
    //public void Inspect()
    //{
    //    Debug.Log("Inspecting Tire...");
    //    // Add custom inspection logic or UI popup here
    //}

    public Text hoverTextUI; // Assign this in Inspector
    public GameObject UI_Background;

    public IEnumerator Inspect()
    {
        Debug.Log("Inspecting Tire...");

        hoverTextUI.enabled = true;
        UI_Background.SetActive(true);
        hoverTextUI.text = "Inspecting Tire..."; // or custom label

        yield return new WaitForSeconds(5f);

        bool result = Random.value > 0.5f;

        if (result)
        {
            hoverTextUI.text = "Tire in working condition.";
            Debug.Log("Tire in working condition.");
        }
        else
        {
            hoverTextUI.text = "Tire not in working condition.";
            Debug.Log("Tire is not in working condition.");
        }

        yield return new WaitForSeconds(2f);
        hoverTextUI.enabled = false;
        UI_Background.SetActive(false);
    }
}
