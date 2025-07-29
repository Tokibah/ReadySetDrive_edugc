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

        

        
            hoverTextUI.text = "Oil has sufficient amount, in good condition.";
            Debug.Log("Oil has sufficient amount, in good condition.");
        RpkCheck.instance.componentChecked();



        yield return new WaitForSeconds(2f);
        hoverTextUI.enabled = false;
        UI_Background.SetActive(false);
    }
}
