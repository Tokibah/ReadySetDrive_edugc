using UnityEngine;
using UnityEngine.UI;

public class PlayerGuide : MonoBehaviour
{
    [TextArea]
    public string[] guideSteps; // Instructions to show
    public Text guideTextUI;    // Assign your UI Text here
    public GameObject guidePanel; // The panel that contains the text

    private int currentStep = 0;
    private bool isShowing = false;

    void Start()
    {
        if (guideSteps.Length > 0)
        {
            guidePanel.SetActive(true);
            guideTextUI.text = guideSteps[0];
            isShowing = true;
        }
    }

    void Update()
    {
        if (!isShowing) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentStep++;

            if (currentStep < guideSteps.Length)
            {
                guideTextUI.text = guideSteps[currentStep];
            }
            else
            {
                guidePanel.SetActive(false);
                isShowing = false;
            }
        }
    }
}
