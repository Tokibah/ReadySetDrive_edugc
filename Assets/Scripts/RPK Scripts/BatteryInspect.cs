using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BatteryInspect : MonoBehaviour, IInspectable
{
    //public void Inspect()
    //{
    //    Debug.Log("Inspecting Battery...");
    //    // Add custom inspection logic or UI popup here
    //}

    public Text hoverTextUI; // Assign this in Inspector
    public GameObject UI_Background;
    public IEnumerator Inspect()
    {
        Debug.Log("Inspecting Battery...");

        hoverTextUI.enabled = true;
        UI_Background.SetActive(true);
        hoverTextUI.text = "Inspecting Battery..."; // or custom label

        yield return new WaitForSeconds(5f);

            hoverTextUI.text = "Battery in working condition.";
            Debug.Log("Battery in working condition.");
        RpkCheck.instance.componentChecked();
       

        yield return new WaitForSeconds(2f);
        hoverTextUI.enabled=false;
        UI_Background.SetActive(false);
    }
}

