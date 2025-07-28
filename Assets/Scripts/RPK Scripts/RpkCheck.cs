using JetBrains.Annotations;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;

public class RpkCheck : MonoBehaviour
{
    public static RpkCheck instance;
    private const int maxComponents = 8;
    private int checkedComponents = 0;

    public Text componentCount;

    private void Awake()
    {
        instance = this;
    }
    public void checkProgress()
    {
        if (checkedComponents < maxComponents)
        {
            Debug.Log("RPK tak lengkap. Komponen yang dah diperiksa: " + checkedComponents + "/" +  maxComponents);
        }
        else
        {
            Debug.Log("RPK dilakukan dengan lengkap");
            componentCount.text = "RPK dilakukan dengan lengkap!";
        }

       
    }

    public void componentChecked()
    {
        checkedComponents++;
    }

    private void OnTriggerEnter(Collider other)
    {
        checkProgress();
    }

    private void Update()
    {
        componentCount.text = "RPK Progress: " + checkedComponents.ToString() + " components checked";
    }

}
