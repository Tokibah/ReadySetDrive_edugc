using System;
using UnityEngine;
using UnityEngine.UI;
public class NewBehaviourScript : MonoBehaviour
{

    public Image Image;
    public Transform Target;
    public Text Meter;
    public Vector3 Offset;
    private void Start()
    {
        float minWidth_test = Image.GetPixelAdjustedRect().width;
        Debug.Log("minWidth = " + minWidth_test);
        minWidth_test = minWidth_test / 2;
        Debug.Log("minWidth / 2 = " + minWidth_test);

    }
    // Update is called once per frame
    void Update()
    {
        float minX = Image.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;

        float minY = Image.GetPixelAdjustedRect().height / 2;
        float maxY = Screen.height - minY;

        Vector3 pos = Camera.main.WorldToScreenPoint(Target.position + Offset);

        if (Vector3.Dot((Target.position - transform.position), transform.forward) < 0)
        {
            //Target behind player
            if (pos.x < Screen.width / 2)
            {
                pos.x = maxX;
            }
            else
            {
                pos.x = minX;
            }
        }

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        Image.transform.position = pos;
        Meter.text = ((int)Vector3.Distance(Target.position, transform.position)).ToString() + "M";
    }
}
