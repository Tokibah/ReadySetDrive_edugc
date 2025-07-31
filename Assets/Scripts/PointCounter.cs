using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;

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
    private int levelCompleted;
    public Text displayCount;
    public Text requiredRep;

    public GameObject nextLevelBtn;
    public GameObject retryBtn;
    public GameObject exitBtn;

    public int collectedRep = 0;
  




    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        nextLevelBtn.SetActive(false);
        retryBtn.SetActive(false);
    }
    public void levelSummary()
    {
        summaryUI.SetActive(true);
        reputation.text = "Reputasi: " + collectedRep.ToString();
        rulesFollowed.text = "Peraturan Diikuti: " + followed.ToString();
        rulesBroken.text = "Peraturan Dilanggar: " + broken.ToString();
    }

    public void levelSuccess()
    {
        levelCompleted++;
        levelStatus.text = "Berjaya!";
        nextLevelBtn.SetActive(true);
        retryBtn.SetActive(false);

        //if (levelCompleted == 4)
        //{
        //    nextLevelBtn.SetActive(false);
        //    retryBtn.SetActive(false);
        //}
    }

    public void levelFailed()
    {
        levelStatus.text = "Gagal!";
        retryBtn.SetActive(true);
        nextLevelBtn.SetActive(false);
    }



   

    public void ruleFollowed()
    {
        ++followed;
        ++rep;
    }

    public void ruleBroken()
    {
        ++broken;
        --rep;
    }

    public void resetPoint()
    {
        rep = 0;
        broken = 0;
        followed = 0;
    }

    public void accident()
    {
        rep = -50;
        retryBtn.SetActive(true);
    }

    private void Update()
    {
        collectedRep = rep * 5;
        requiredRep.text = EndLevel.instance.requiredScore.ToString() ;
        displayCount.text = collectedRep.ToString();
    }


}
