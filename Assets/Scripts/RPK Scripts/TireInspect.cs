using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Transactions;

public class TireInspect : MonoBehaviour, IInspectable
{
    //public void Inspect()
    //{
    //    Debug.Log("Inspecting Tire...");
    //    // Add custom inspection logic or UI popup here
    //}

    public Text hoverTextUI; // Assign this in Inspector
    public GameObject UI_Background;
    private bool isInspected = false;

    public IEnumerator Inspect()
    {

        if (isInspected)
        {
            hoverTextUI.enabled = true;
            UI_Background.SetActive(true);
            hoverTextUI.text = "This tire is already checked.";

            yield return new WaitForSeconds(2f);
            UI_Background.SetActive(false);
            yield break;
        }

        isInspected = true;

        // perform inspection
        Debug.Log("Inspecting Tire...");

        hoverTextUI.enabled = true;
        UI_Background.SetActive(true);
        hoverTextUI.text = "Inspecting Tire..."; // or custom label

        yield return new WaitForSeconds(5f);

            hoverTextUI.text = "Tire in working condition.";
            Debug.Log("Tire in working condition.");
            RpkCheck.instance.componentChecked();


        yield return new WaitForSeconds(2f);
        hoverTextUI.enabled = false;
        UI_Background.SetActive(false);
    }
}
