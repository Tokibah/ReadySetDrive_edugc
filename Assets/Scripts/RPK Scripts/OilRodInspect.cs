using UnityEngine;
using UnityEngine.UI;
using System.Collections; 

public class OilRodInspect : MonoBehaviour, IInspectable
{
    //public void Inspect()
    //{
    //    Debug.Log("Inspecting Oil Rod...");
    //    // Add custom inspection logic or UI popup here
    //}

    public Text hoverTextUI; // Assign this in Inspector
    public GameObject UI_Background;

    public IEnumerator Inspect()
    {
        Debug.Log("Inspecting Oil Rod...");

        hoverTextUI.enabled = true;
        UI_Background.SetActive(true);
        hoverTextUI.text = "Inspecting Oil..."; // or custom label

        yield return new WaitForSeconds(5f);

        bool result = Random.value > 0.5f;

        if (result)
        {
            hoverTextUI.text = "Oil has sufficient amount, in good condition.";
            Debug.Log("Oil has sufficient amount, in good condition.");
        }
        else
        {
            hoverTextUI.text = "Oil does not have sufficient amount, not in good condition.";
            Debug.Log("Oil does not have sufficient amount, not in good condition.");
        }

        yield return new WaitForSeconds(2f);
        hoverTextUI.enabled = false;
        UI_Background.SetActive(false);
    }
}
