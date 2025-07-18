using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LightsInpect : MonoBehaviour,IInspectable
{
    //public void Inspect()
    //{
    //    Debug.Log("Inspecting Headlights...");
    //    // Add custom inspection logic or UI popup here
    //}
    public Text hoverTextUI; // Assign this in Inspector
    public GameObject UI_Background;


    public IEnumerator Inspect()
    {
        Debug.Log("Inspecting Headlights...");

        hoverTextUI.enabled = true;
        UI_Background.SetActive(true);
        hoverTextUI.text = "Inspecting Headlight..."; // or custom label

        yield return new WaitForSeconds(5f);

        bool result = Random.value > 0.5f;

        if (result)
        {
            hoverTextUI.text = "Headlight in working condition.";
            Debug.Log("Headlight in working condition.");
        }
        else
        {
            hoverTextUI.text = "Headlight not in working condition.";
            Debug.Log("Headlight is not in working condition.");
        }

        yield return new WaitForSeconds(2f);
        hoverTextUI.enabled = false;
        UI_Background.SetActive(false);
    }
}
