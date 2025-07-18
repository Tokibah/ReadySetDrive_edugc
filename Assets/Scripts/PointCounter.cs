using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PointCounter : MonoBehaviour
{
    public static PointCounter instance;

    public Text levelStatus;
    public Text reputation;
    public Text rulesFollowed;
    public Text rulesBroken;
    public GameObject summaryUI;
    string status;
    public int rep, followed, broken;

    public GameObject nextLevelBtn;
    public GameObject retryBtn;
    public GameObject exitBtn;

  




    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        instance = this;
    }
    public void levelSummary()
    {
        int requiredRep = rep * 5;
        nextLevelBtn.SetActive(false);
        retryBtn.SetActive(false);
        summaryUI.SetActive(true);
        reputation.text = "Reputation: " + requiredRep.ToString();
        rulesFollowed.text = "Rules Followed: " + followed.ToString();
        rulesBroken.text = "Rules Broken: " + broken.ToString();

        
        if (requiredRep >= 5)
        {
            levelStatus.text = "Success!";
            nextLevelBtn.SetActive(true);
        }
        else
        {
            levelStatus.text = "Failed!";
            retryBtn.SetActive(true);
        }

       


    }

    public void ruleFollowed()
    {
        followed++;
        rep++;
    }

    public void ruleBroken()
    {
        broken++;
        rep--;
    }

    public void resetPoint()
    {
        rep = 0;
        broken = 0;
        followed = 0;
    }
}
